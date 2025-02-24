﻿using System;
using System.Web.UI.WebControls;

using CMS.Base;
using CMS.ExtendedControls;
using CMS.ExtendedControls.ActionsConfig;
using CMS.Helpers;
using CMS.Membership;
using CMS.PortalEngine;
using CMS.Scheduler;
using CMS.UIControls;

public partial class CMSModules_Scheduler_Pages_Tab_Tasks : CMSScheduledTasksPage
{
    #region "Constants"

    // Actions event names
    private const string TASKS_RESET = "TASKSRESET";
    private const string TASKS_RESTART_TIMER = "TASKSRESTART";
    private const string TASKS_RUN = "TASKSRUN";

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


    /// <summary>
    /// Local header actions
    /// </summary>
    public override HeaderActions HeaderActions
    {
        get
        {
            return headerActions;
        }
    }

    #endregion


    #region "Page events"

    protected void Page_Load(object sender, EventArgs e)
    {
        rightHeaderActions.ActionPerformed += RightHeaderActions_ActionPerformed;

        // Show info that scheduler is disabled
        if (!SchedulingHelper.EnableScheduler)
        {
            ShowWarning(GetString("scheduledtask.disabled"));
        }

        listElem.SiteID = SiteID;
        string editUrl = UIContextHelper.GetElementUrl("CMS.ScheduledTasks", GetElementName("EditTask"), true);
        editUrl = URLHelper.AddParameterToUrl(editUrl, "siteid", SiteID.ToString());
        listElem.EditURL = editUrl;

        // New task action
        HeaderActions.AddAction(new HeaderAction
        {
            Text = GetString("Task_List.NewItemCaption"),
            RedirectUrl = String.Format(listElem.EditURL, 0)
        });

        // Refresh action
        HeaderActions.AddAction(new HeaderAction
        {
            Text = GetString("General.Refresh"),
            OnClientClick = "RefreshGrid();"
        });

        if (SiteInfo != null)
        {
            bool usesTimer = SchedulingHelper.UseAutomaticScheduler || !SchedulingHelper.RunSchedulerWithinRequest;
            if (usesTimer)
            {
                // Restart timer action
                rightHeaderActions.AddAction(new HeaderAction
                {
                    Text = GetString("Task_List.Restart"),
                    EventName = TASKS_RESTART_TIMER
                });
            }

            // Run execution ASAP action
            rightHeaderActions.AddAction(new HeaderAction
            {
                Text = GetString("Task_List.RunNow"),
                EventName = TASKS_RUN,
                Enabled = SchedulingHelper.EnableScheduler && (!usesTimer || SchedulingTimer.TimerExists(SiteInfo.SiteName)),
                ButtonStyle = ButtonStyle.Default
            });
        }

        // Reset action to the right
        rightHeaderActions.AddAction(new HeaderAction
        {
            Text = GetString("tasks.resetexecutions"),
            OnClientClick = "if (!confirm(" + ScriptHelper.GetLocalizedString("tasks.resetall") + ")) return false;",
            EventName = TASKS_RESET,
            ButtonStyle = ButtonStyle.Default
        });

        // Force action buttons to cause full postback so that tasks can be properly executed in global.asax
        ControlsHelper.RegisterPostbackControl(listElem);
        ControlsHelper.RegisterPostbackControl(rightHeaderActions);

        if (ControlsHelper.CausedPostBack(btnRefresh))
        {
            // Update content after refresh
            pnlUpdate.Update();
        }
        else if (QueryHelper.GetBoolean("reseted", false))
        {
            // Show reset confirmation after reset button action
            ShowConfirmation(GetString("task.executions.reseted"));
        }
    }


    protected override void OnPreRender(EventArgs e)
    {
        if (SiteInfo != null)
        {
            if (SchedulingHelper.UseAutomaticScheduler || !SchedulingHelper.RunSchedulerWithinRequest)
            {
                lblLastRun.Visible = true;

                string siteName = SiteInfo.SiteName.ToLowerCSafe();
                if (SchedulingTimer.TimerExists(siteName))
                {
                    DateTime lastRun = ValidationHelper.GetDateTime(SchedulingTimer.LastRuns[siteName], DateTimeHelper.ZERO_TIME);
                    if (lastRun != DateTimeHelper.ZERO_TIME)
                    {
                        lblLastRun.Text = GetString("Task_List.LastRun") + " " + lastRun;
                    }
                    else
                    {
                        lblLastRun.Text = GetString("Task_List.Running");
                    }
                }
                else
                {
                    lblLastRun.Text = GetString("Task_List.NoRun");
                }
            }
            else
            {
                lblLastRun.Visible = false;
            }
        }

        ScriptHelper.RegisterStartupScript(Page, typeof(string), "SchedulerRefreshScript", "function RefreshGrid() { " + ControlsHelper.GetPostBackEventReference(btnRefresh) + "; }", true);

        base.OnPreRender(e);
    }

    #endregion


    /// <summary>
    /// Handles action performed event for right HeaderActions.
    /// </summary>
    protected void RightHeaderActions_ActionPerformed(object sender, CommandEventArgs args)
    {
        switch (args.CommandName)
        {
            case TASKS_RESET:

                // Pass 0 for global tasks
                TaskInfoProvider.ResetAllTasks((SiteID > 0) ? SiteID : 0);
                string url = UIContextHelper.GetElementUrl("CMS.ScheduledTasks", GetElementName("Tasks"), true);
                url = URLHelper.AddParameterToUrl(url, "siteid", SiteID.ToString());
                url = URLHelper.AddParameterToUrl(url, "reseted", "1");
                URLHelper.Redirect(url);
                break;

            case TASKS_RESTART_TIMER:

                // Check "modify" permission
                if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.ScheduledTasks", "Modify"))
                {
                    RedirectToAccessDenied("CMS.ScheduledTasks", "Modify");
                }

                SchedulingTimer.RestartTimer(SiteInfo.SiteName);
                break;

            case TASKS_RUN:

                // Check "modify" permission
                if (!MembershipContext.AuthenticatedUser.IsAuthorizedPerResource("CMS.ScheduledTasks", "Modify"))
                {
                    RedirectToAccessDenied("CMS.ScheduledTasks", "Modify");
                }

                SchedulingTimer.RunSchedulerASAP(SiteInfo.SiteName);
                SchedulingTimer.SchedulerRunImmediatelySiteName = SiteInfo.SiteName;
                break;
        }
        pnlUpdate.Update();
    }
}