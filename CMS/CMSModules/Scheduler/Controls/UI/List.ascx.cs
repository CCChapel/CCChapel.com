﻿using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.Helpers;
using CMS.UIControls;
using CMS.Scheduler;
using CMS.Base;
using CMS.SiteProvider;
using CMS.Membership;
using CMS.DataEngine;
using CMS.ExtendedControls;

public partial class CMSModules_Scheduler_Controls_UI_List : CMSAdminListControl
{
    #region "Properties"

    /// <summary>
    /// Indicates whether list should display standard or system tasks.
    /// </summary>
    public bool SystemTasks
    {
        get;
        set;
    }


    /// <summary>
    /// Site ID or selected site ID.
    /// </summary>
    public int SiteID
    {
        get;
        set;
    }


    /// <summary>
    /// URL to task edit page.
    /// </summary>
    public string EditURL
    {
        get;
        set;
    }

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        gridElem.OnAction += gridElem_OnAction;
        gridElem.OnExternalDataBound += gridElem_OnExternalDataBound;

        gridElem.WhereCondition = GenerateWhereCondition();

        if (SystemTasks)
        {
            gridElem.ObjectType = SystemTaskListInfo.OBJECT_TYPE;
        }
        else
        {
            gridElem.ObjectType = TaskInfo.OBJECT_TYPE;

            // Some non-object scheduled tasks are system
            gridElem.WhereCondition = SqlHelper.AddWhereCondition(gridElem.WhereCondition, "TaskType IS NULL OR TaskType != " + (int)ScheduledTaskTypeEnum.System);
        }
    }


    /// <summary>
    /// Handles the UniGrid's OnAction event.
    /// </summary>
    /// <param name="actionName">Name of item (button) that threw event</param>
    /// <param name="actionArgument">ID (value of Primary key) of corresponding data row</param>
    protected void gridElem_OnAction(string actionName, object actionArgument)
    {
        switch (actionName.ToLowerCSafe())
        {
            case "edit":
                if (!String.IsNullOrEmpty(EditURL))
                {
                    URLHelper.Redirect(String.Format(EditURL, actionArgument));
                }
                break;

            case "delete":
                {
                    // Check "modify" permission
                    if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.ScheduledTasks", "Modify"))
                    {
                        RedirectToAccessDenied("CMS.ScheduledTasks", "Modify");
                    }

                    // Delete the task
                    try
                    {
                        int taskId = Convert.ToInt32(actionArgument);

                        TaskInfo ti = TaskInfoProvider.GetTaskInfo(taskId);
                        if (ti != null)
                        {
                            ti.Generalized.LogSynchronization = SynchronizationTypeEnum.LogSynchronization;
                            ti.Generalized.LogIntegration = true;
                            ti.Generalized.LogEvents = true;
                            TaskInfoProvider.DeleteTaskInfo(ti);
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError(GetString("Task_List.DeleteError"), ex.Message);
                    }
                }
                break;

            case "execute":
                {
                    // Check "modify" permission
                    if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.ScheduledTasks", "Modify"))
                    {
                        RedirectToAccessDenied("CMS.ScheduledTasks", "Modify");
                    }

                    TaskInfo ti = TaskInfoProvider.GetTaskInfo(Convert.ToInt32(actionArgument));
                    if (ti != null)
                    {
                        SiteInfo si = SiteInfoProvider.GetSiteInfo(SiteID);
                        if (!ti.TaskEnabled)
                        {
                            // Task is not enabled (won't be executed at the end of request), run it now
                            SchedulingExecutor.ExecuteTask(ti.TaskID, (si != null) ? si.SiteName : SiteContext.CurrentSiteName);
                        }
                        else
                        {
                            TaskInterval interval = SchedulingHelper.DecodeInterval(ti.TaskInterval);

                            if ((ti.TaskNextRunTime != DateTimeHelper.ZERO_TIME) || (interval.Period == SchedulingHelper.PERIOD_ONCE))
                            {
                                ti.TaskNextRunTime = DateTime.Now;

                                // Update the task
                                TaskInfoProvider.SetTaskInfo(ti);

                                // Run the task
                                SchedulingTimer.RunSchedulerImmediately = true;
                                if (si != null)
                                {
                                    SchedulingTimer.SchedulerRunImmediatelySiteName = si.SiteName;
                                }
                            }
                            else
                            {
                                ShowWarning(GetString("ScheduledTask.TaskAlreadyrunning"));
                                return;
                            }
                        }

                        ShowConfirmation(GetString("ScheduledTask.WasExecuted"));
                    }
                }
                break;
        }
    }


    protected object gridElem_OnExternalDataBound(object sender, string sourceName, object parameter)
    {
        switch (sourceName.ToLowerCSafe())
        {
            case "useexternalservice":
                // Use external service
                {
                    CMSGridActionButton imgButton = sender as CMSGridActionButton;
                    if (imgButton != null)
                    {
                        imgButton.Visible = false;
                        if (SchedulingHelper.UseExternalService)
                        {
                            DataRowView dataRowView = UniGridFunctions.GetDataRowView(imgButton.Parent as DataControlFieldCell);
                            if (dataRowView != null)
                            {
                                var taskInfo = new TaskInfo(dataRowView.Row);

                                imgButton.Visible = SchedulingHelper.IsExternalTaskTooLate(taskInfo, 10);

                                if (imgButton.Visible)
                                {
                                    imgButton.ToolTip = GetString("scheduledtask.useservicewarning");
                                    imgButton.OnClientClick = "return false;";
                                    imgButton.Style.Add(HtmlTextWriterStyle.Cursor, "default");
                                }
                            }
                        }
                    }
                }
                break;

            case "taskexecutions":
                if (string.IsNullOrEmpty(Convert.ToString(parameter)))
                {
                    return 0;
                }
                break;

            case "runactions":
            {
                // Image "run" button
                CMSGridActionButton runButton = ((CMSGridActionButton)sender);

                // Data row and task enabled value
                DataRowView dataRowView = UniGridFunctions.GetDataRowView(runButton.Parent as DataControlFieldCell);
                bool taskEnabled = ValidationHelper.GetBoolean(dataRowView["TaskEnabled"], false);

                if (!taskEnabled)
                {
                    // If not enabled add confirmation dialog
                    runButton.OnClientClick = "if (!confirm(" + ScriptHelper.GetLocalizedString("taskdisabled.sure") + ")) return false;" + runButton.OnClientClick;
                }

                break;
            }
        }
        return parameter;
    }


    /// <summary>
    /// Generates where condition for unigrid.
    /// </summary>    
    private string GenerateWhereCondition()
    {
        if (SiteID > 0)
        {
            return "TaskSiteID = " + SiteID;
        }
        else
        {
            return "TaskSiteID IS NULL";
        }
    }
}