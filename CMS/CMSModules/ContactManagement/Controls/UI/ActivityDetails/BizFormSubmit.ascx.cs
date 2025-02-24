﻿using System;

using CMS.Core;
using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Helpers;
using CMS.OnlineForms;
using CMS.OnlineMarketing;
using CMS.Base;
using CMS.WebAnalytics;

public partial class CMSModules_ContactManagement_Controls_UI_ActivityDetails_BizFormSubmit : ActivityDetail
{
    #region "Methods"

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptHelper.RegisterDialogScript(Page);

        ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "displayModal",
                                               ScriptHelper.GetScript(
                                                   "function DisplaySurveyDetails(params) { \n" +
                                                   "       modalDialog('" + ResolveUrl(@"~\CMSModules\ContactManagement\Controls\UI\ActivityDetails\BizFormDetails.aspx") + "' + params, 'bizdetails', 720, 450); \n" +
                                                   " } \n "));
    }


    public override bool LoadData(ActivityInfo ai)
    {
        if ((ai == null) || !ModuleManager.IsModuleLoaded(ModuleName.BIZFORM) || (ai.ActivityType != PredefinedActivityType.BIZFORM_SUBMIT))
        {
            return false;
        }

        BizFormInfo bfi = BizFormInfoProvider.GetBizFormInfo(ai.ActivityItemID);
        if (bfi == null)
        {
            return false;
        }

        string qs = String.Format("?bizid={0}&recid={1}", bfi.FormID, ai.ActivityItemDetailID);
        qs = URLHelper.AddUrlParameter(qs, "hash", QueryHelper.GetHash(qs));
        btnView.Visible = true;
        btnView.OnClientClick = "DisplaySurveyDetails('" + qs + "'); return false;";
        return true;
    }

    #endregion
}