﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Helpers;
using CMS.Membership;
using CMS.Base;
using CMS.SiteProvider;
using CMS.UIControls;
using CMS.WebAnalytics;
using CMS.ExtendedControls;


public partial class CMSModules_WebAnalytics_Pages_Tools_Campaign_CampaignReport : CMSCampaignPage
{
    #region "Variables"

    private int mConversionId;
    private bool mIsBeingSaved;
    private bool mReportDisplayed;
    private bool mAllowNoTimeSelection;
    private string mDataCodeName = String.Empty;
    private string mReportCodeNames = String.Empty;
    private IDisplayReport mUcDisplayReport;

    #endregion


    #region "Properties"

    /// <summary>
    /// Messages placeholder
    /// </summary>
    public override MessagesPlaceHolder MessagesPlaceHolder
    {
        get
        {
            return plcMess;
        }
    }

    #endregion


    #region "Page Events"

    protected void Page_Load(object sender, EventArgs e)
    {
        string uiTrace = QueryHelper.GetString("ui", string.Empty);
        if (uiTrace == "omanalytics")
        {
            // Check UI elements
            if (!CurrentUser.IsAuthorizedPerUIElement("CMS.WebAnalytics", "Analytics.Campaigns.Overview"))
            {
                RedirectToUIElementAccessDenied("CMS.WebAnalytics", "Analytics.Campaigns.Overview");
            }

            if (!CurrentUser.IsAuthorizedPerUIElement("CMS.WebAnalytics", "Campaigns"))
            {
                RedirectToUIElementAccessDenied("CMS.WebAnalytics", "Campaigns");
            }
        }

        mConversionId = QueryHelper.GetInteger("conversionid", 0);
        CurrentMaster.PanelContent.CssClass = "";
        UIHelper.AllowUpdateProgress = false;

        reportHeaderActions.ActionPerformed += HeaderActions_ActionPerformed;

        // Set disabled module info
        ucDisabledModule.SettingsKeys = "CMSAnalyticsEnabled;";
        ucDisabledModule.ParentPanel = pnlDisabled;

        mUcDisplayReport = (IDisplayReport)LoadUserControl("~/CMSModules/Reporting/Controls/DisplayReport.ascx");
        pnlDisplayReport.Controls.Add((Control)mUcDisplayReport);

        mDataCodeName = QueryHelper.GetString("dataCodeName", String.Empty);
        mReportCodeNames = QueryHelper.GetString("reportCodeName", String.Empty);

        // Control initialization (based on displayed report)
        if (mDataCodeName == "conversion")
        {
            CheckWebAnalyticsUI("Conversion.Overview");
        }
        else if (mDataCodeName == "conversionpropertiesdetail")
        {
            CheckWebAnalyticsUI("Conversions.Detail");
            ucReportHeader.ShowConversionSelector = (mConversionId == 0);
            ucGraphType.ShowIntervalSelector = false;
            mAllowNoTimeSelection = true;
            ucGraphType.AllowEmptyDate = true;
        }

        // Set table same first column width for all time
        mUcDisplayReport.TableFirstColumnWidth = Unit.Percentage(20);
    }


    protected override void OnPreRender(EventArgs e)
    {
        DisplayReport();

        reportHeaderActions.ReportName = mUcDisplayReport.ReportName;
        reportHeaderActions.ReportParameters = mUcDisplayReport.ReportParameters;
        reportHeaderActions.SelectedInterval = ucGraphType.SelectedInterval;
        
        base.OnPreRender(e);
    }


    protected void HeaderActions_ActionPerformed(object sender, CommandEventArgs e)
    {
        if (e.CommandName == ComponentEvents.SAVE)
        {
            Save();
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Displays the given report
    /// </summary>
    private void DisplayReport()
    {
        // If report was already displayed .. return
        if (mReportDisplayed)
        {
            return;
        }

        ucGraphType.ProcessChartSelectors(false);

        // Prepare report parameters
        DataTable reportParameters = new DataTable();

        // In case of hidden datetime -> for save purpose pass from (to) as now to query parameter
        DateTime from = ((ucGraphType.From == DateTimeHelper.ZERO_TIME) && !pnlHeader.Visible) ? DateTime.Now : ucGraphType.From;
        DateTime to = ((ucGraphType.To == DateTimeHelper.ZERO_TIME) && !pnlHeader.Visible) ? DateTime.Now : ucGraphType.To;

        reportParameters.Columns.Add("FromDate", typeof(DateTime));
        reportParameters.Columns.Add("ToDate", typeof(DateTime));
        reportParameters.Columns.Add("CodeName", typeof(string));
        reportParameters.Columns.Add("ConversionName", typeof(string));
        reportParameters.Columns.Add("SiteID", typeof(int));

        object[] parameters = new object[5];

        parameters[0] = (mAllowNoTimeSelection && from == DateTimeHelper.ZERO_TIME) ? (DateTime?)null : from;
        parameters[1] = (mAllowNoTimeSelection && to == DateTimeHelper.ZERO_TIME) ? (DateTime?)null : to;
        parameters[2] = mDataCodeName;
        parameters[3] = String.Empty;
        parameters[4] = SiteContext.CurrentSiteID;

        // Get report name from query
        string reportName = ucGraphType.GetReportName(mReportCodeNames);

        if (mConversionId == 0)
        {
            // Filter conversion
            string conversionName = ValidationHelper.GetString(ucReportHeader.SelectedConversion, String.Empty);
            if ((conversionName != ucReportHeader.AllRecordValue) && !String.IsNullOrEmpty(conversionName))
            {
                parameters[3] = conversionName;
            }
        }
        else
        {
            ConversionInfo ci = ConversionInfoProvider.GetConversionInfo(mConversionId);
            if (ci != null)
            {
                parameters[3] = ci.ConversionName;
            }
        }

        reportParameters.Rows.Add(parameters);
        reportParameters.AcceptChanges();

        mUcDisplayReport.ReportName = reportName;

        // Set display report
        if (!mUcDisplayReport.IsReportLoaded())
        {
            ShowError(String.Format(GetString("Analytics_Report.ReportDoesnotExist"), HTMLHelper.HTMLEncode(reportName)));
        }
        else
        {
            mUcDisplayReport.LoadFormParameters = false;
            mUcDisplayReport.DisplayFilter = false;
            mUcDisplayReport.ReportParameters = reportParameters.Rows[0];
            mUcDisplayReport.GraphImageWidth = 100;
            mUcDisplayReport.IgnoreWasInit = true;
            mUcDisplayReport.UseExternalReload = true;
            mUcDisplayReport.UseProgressIndicator = true;
            mUcDisplayReport.SelectedInterval = HitsIntervalEnumFunctions.HitsConversionToString(ucGraphType.SelectedInterval);
            mUcDisplayReport.ReloadData(true);
        }

        // Mark as report displayed
        mReportDisplayed = true;
    }


    /// <summary>
    /// Used in rendering to control outside render stage (save method) 
    /// </summary>
    /// <param name="control">Control</param>
    public override void VerifyRenderingInServerForm(Control control)
    {
        if (!mIsBeingSaved)
        {
            base.VerifyRenderingInServerForm(control);
        }
    }


    /// <summary>
    /// Saves the graph report.
    /// </summary>
    private void Save()
    {
        DisplayReport();

        // Check web analytics save permission
        if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.WebAnalytics", "SaveReports"))
        {
            RedirectToAccessDenied("CMS.WebAnalytics", "SaveReports");
        }

        mIsBeingSaved = true;

        if (mUcDisplayReport.SaveReport() > 0)
        {
            ShowConfirmation(String.Format(GetString("Analytics_Report.ReportSavedTo"), mUcDisplayReport.ReportDisplayName + " - " + DateTime.Now));
        }

        mIsBeingSaved = false;
    }

    #endregion
}