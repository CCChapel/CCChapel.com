﻿using System;
using System.Linq;
using System.Web.UI;
using System.Text;

using CMS.UIControls;
using CMS.Helpers;
using CMS.PortalEngine;
using CMS.ExtendedControls;
using CMS.IO;


public partial class CMSModules_AdminControls_Controls_UIControls_Theme : CMSAbstractUIWebpart
{
    #region "Variables"

    private string defaultAllowedExtensions = "gif;png;bmp;jpg;jpeg;css;skin";
    private string defaultNewTextFileExtension = "css";

    #endregion


    #region "Properties"

    /// <summary>
    /// Gets or sets the theme path
    /// </summary>
    public string Path
    {
        get
        {
            return GetStringContextValue("Path");
        }
        set
        {
            SetValue("Path", value);
        }
    }


    /// <summary>
    /// String containing list of allowed extensions separated by semicolon.
    /// </summary>
    public string AllowedExtensions
    {
        get
        {
            return GetStringContextValue("AllowedExtensions", defaultAllowedExtensions);
        }
        set
        {
            SetValue("AllowedExtensions", value);
        }
    }


    /// <summary>
    /// Extension allowed for creation of a new text file.
    /// </summary>
    public string NewTextFileExtension
    {
        get
        {
            return GetStringContextValue("NewTextFileExtension", defaultNewTextFileExtension);
        }
        set
        {
            SetValue("NewTextFileExtension", value);
        }
    }

    #endregion


    #region "Methods"

    /// <summary>
    /// Method that is called when the page content is loaded, override to implement the control initialization after the content has been loaded.
    /// </summary>
    public override void OnContentLoaded()
    {
        SetupControl();

        base.OnContentLoaded();
    }


    /// <summary>
    /// PreRender event handler.
    /// </summary>
    protected override void OnPreRender(EventArgs e)
    {
        if (!StopProcessing)
        {
            // Register scripts
            ScriptHelper.RegisterJQuery(Page);
            CMSDialogHelper.RegisterDialogHelper(Page);
            ScriptManager.RegisterStartupScript(Page, typeof(Page), "InitResizers", "$cmsj(InitResizers());", true);
            CSSHelper.RegisterCSSBlock(Page, "themeCss", ".TooltipImage{max-width:200px; max-height:200;}");

            ScriptHelper.HideVerticalTabs(Page);
        }

        base.OnPreRender(e);
    }


    /// <summary>
    /// Setups the control.
    /// </summary>
    private void SetupControl()
    {
        // Get path
        string filePath = Path;

        if (string.IsNullOrEmpty(filePath))
        {
            IThemeInfo themeObject = UIContext.EditedObject as IThemeInfo;
            if (themeObject != null)
            {
                // Get the specific theme path for the current theme object
                filePath = themeObject.GetThemePath();
            }
            else
            {
                // Use the general theme path
                filePath = "~/App_Themes/";
            }
        }

        // Setup the file system browser
        if (!String.IsNullOrEmpty(filePath))
        {
            string absoluteFilePath = string.Empty;

            try
            {
                absoluteFilePath = Server.MapPath(filePath);
            }
            catch (Exception ex)
            {
                selFile.Visible = false;
                ShowError(ex.Message);
                return;
            }

            // Create folder if does not exist
            if (!Directory.Exists(absoluteFilePath))
            {
                Directory.CreateDirectory(absoluteFilePath);
            }

            // Setup the browser
            var config = new FileSystemDialogConfiguration();
            config.StartingPath = filePath;
            config.AllowedExtensions = AllowedExtensions;
            config.NewTextFileExtension = NewTextFileExtension;
            config.ShowFolders = false;
            config.AllowZipFolders = true;
            config.AllowManage = true;
            
            selFile.Config = config;
        }
    }

    #endregion
}
