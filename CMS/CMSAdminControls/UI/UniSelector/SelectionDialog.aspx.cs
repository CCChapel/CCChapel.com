﻿using System;
using System.Collections;

using CMS.Helpers;
using CMS.UIControls;
using CMS.ExtendedControls;

public partial class CMSAdminControls_UI_UniSelector_SelectionDialog : CMSModalPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Try to get parameters...
        string identifier = QueryHelper.GetString("params", null);
        Hashtable parameters = (Hashtable)WindowHelper.GetItem(identifier);

        CurrentMaster.PanelContent.RemoveCssClass("dialog-content");

        // ... and validate hash
        if ((QueryHelper.ValidateHash("hash", "selectedvalue")) && (parameters != null))
        {
            selectionDialog.LocalizeItems = QueryHelper.GetBoolean("localize", true);
            // Load resource prefix
            string resourcePrefix = ValidationHelper.GetString(parameters["ResourcePrefix"], "general");

            // Set the page title
            string titleText = GetString(resourcePrefix + ".selectitem|general.selectitem");

            PageTitle.TitleText = titleText;
            Page.Title = titleText;

            var selectionMode = (SelectionModeEnum)parameters["SelectionMode"];

            // Show the OK button if needed
            switch (selectionMode)
            {
                case SelectionModeEnum.Multiple:
                case SelectionModeEnum.MultipleTextBox:
                case SelectionModeEnum.MultipleButton:
                    {
                        SetSaveJavascript("return US_Submit();");
                        SetSaveResourceString("general.select");
                    }
                    break;
            }
        }
        else
        {
            // Redirect to error page
            URLHelper.Redirect(UIHelper.GetErrorPageUrl("dialogs.badhashtitle", "dialogs.badhashtext", true));
        }
    }
}