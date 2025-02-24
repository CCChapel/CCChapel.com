﻿using System;
using System.Text;
using System.Web;
using System.Xml;

using CMS.Base;
using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.Helpers;
using CMS.Membership;
using CMS.MembershipProvider;
using CMS.Modules;
using CMS.PortalControls;
using CMS.PortalEngine;
using CMS.Protection;
using CMS.SiteProvider;
using CMS.WebAnalytics;

public partial class CMSWebParts_Membership_LinkedIn_LinkedInUsersRequiredData : CMSAbstractWebPart
{
    #region "Constants"

    protected const string SESSION_NAME_USERDATA = "LinkedInUserData";

    #endregion


    #region "Private variables"

    private LinkedInHelper linkedInHelper;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets the value that indicates whether email to user should be sent.
    /// </summary>
    public bool SendWelcomeEmail
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("SendWelcomeEmail"), true);
        }
        set
        {
            SetValue("SendWelcomeEmail", value);
        }
    }


    /// <summary>
    /// Gets or sets registration approval page URL.
    /// </summary>
    public string ApprovalPage
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ApprovalPage"), "");
        }
        set
        {
            SetValue("ApprovalPage", value);
        }
    }


    /// <summary>
    /// Gets or sets the sender email (from).
    /// </summary>
    public string FromAddress
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("FromAddress"), SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSNoreplyEmailAddress"));
        }
        set
        {
            SetValue("FromAddress", value);
        }
    }


    /// <summary>
    /// Gets or sets the recipient email (to).
    /// </summary>
    public string ToAddress
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("ToAddress"), SettingsKeyInfoProvider.GetValue(SiteContext.CurrentSiteName + ".CMSAdminEmailAddress"));
        }
        set
        {
            SetValue("ToAddress", value);
        }
    }


    /// <summary>
    /// Gets or sets the value that indicates whether after successful registration is 
    /// notification email sent to the administrator.
    /// </summary>
    public bool NotifyAdministrator
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("NotifyAdministrator"), false);
        }
        set
        {
            SetValue("NotifyAdministrator", value);
        }
    }


    /// <summary>
    /// Gets or sets the message which is displayed after successful registration.
    /// </summary>
    public string DisplayMessage
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DisplayMessage"), "");
        }
        set
        {
            SetValue("DisplayMessage", value);
        }
    }


    /// <summary>
    /// Gets or sets the value which enables abitity of new user to set password.
    /// </summary>
    public bool AllowFormsAuthentication
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowFormsAuthentication"), false);
        }
        set
        {
            SetValue("AllowFormsAuthentication", value);
            plcPasswordNew.Visible = value;
        }
    }


    /// <summary>
    /// Gets or sets the value which enables abitity join liveid with existing account.
    /// </summary>
    public bool AllowExistingUser
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("AllowExistingUser"), true);
        }
        set
        {
            SetValue("AllowExistingUser", value);
            plcPasswordNew.Visible = value;
        }
    }


    /// <summary>
    /// Gets or sets the default target url (redirection when the user is logged in).
    /// </summary>
    public string DefaultTargetUrl
    {
        get
        {
            return ValidationHelper.GetString(GetValue("DefaultTargetUrl"), "");
        }
        set
        {
            SetValue("DefaultTargetUrl", value);
        }
    }


    /// <summary>
    /// Gets or sets the value which determines the behaviour for no LinkedIn users.
    /// </summary>
    public bool HideForNoLinkedInUserID
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("HideForNoLinkedInUserID"), true);
        }
        set
        {
            SetValue("HideForNoLinkedInUserID", value);
        }
    }

    #endregion


    #region "Conversion properties"

    /// <summary>
    /// Gets or sets the conversion track name used after successful registration.
    /// </summary>
    public string TrackConversionName
    {
        get
        {
            return ValidationHelper.GetString(GetValue("TrackConversionName"), "");
        }
        set
        {
            if ((value != null) && (value.Length > 400))
            {
                value = value.Substring(0, 400);
            }
            SetValue("TrackConversionName", value);
        }
    }


    /// <summary>
    /// Gets or sets the conversion value used after successful registration.
    /// </summary>
    public double ConversionValue
    {
        get
        {
            return ValidationHelper.GetDoubleSystem(GetValue("ConversionValue"), 0);
        }
        set
        {
            SetValue("ConversionValue", value);
        }
    }

    #endregion


    #region "Page events"

    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (!StopProcessing)
        {
            plcError.Visible = false;

            // Check renamed DLL library
            if (!SystemContext.IsFullTrustLevel)
            {
                // Error label is displayed when OpenID library is not enabled
                lblError.ResourceString = "socialnetworking.fulltrustrequired";
                plcError.Visible = true;
                plcContent.Visible = false;
            }

            // Check if LinkedIn module is enabled
            if (!LinkedInHelper.LinkedInIsAvailable(SiteContext.CurrentSiteName) && !plcError.Visible)
            {
                // Error label is displayed only in Design mode
                if (PortalContext.IsDesignMode(PortalContext.ViewMode))
                {
                    StringBuilder parameter = new StringBuilder();
                    parameter.Append(UIElementInfoProvider.GetApplicationNavigationString("cms", "Settings") + " -> ");
                    parameter.Append(GetString("settingscategory.cmsmembership") + " -> ");
                    parameter.Append(GetString("settingscategory.cmsmembershipauthentication") + " -> ");
                    parameter.Append(GetString("settingscategory.cmslinkedin"));
                    if (MembershipContext.AuthenticatedUser.CheckPrivilegeLevel(UserPrivilegeLevelEnum.GlobalAdmin))
                    {
                        // Make it link for Admin
                        parameter.Insert(0, "<a href=\"" + URLHelper.GetAbsoluteUrl(UIContextHelper.GetApplicationUrl("cms", "settings")) + "\" target=\"_top\">");
                        parameter.Append("</a>");
                    }

                    lblError.Text = String.Format(GetString("mem.linkedin.disabled"), parameter);
                    plcError.Visible = true;
                    plcContent.Visible = false;
                }
                // In other modes is web part hidden
                else
                {
                    Visible = false;
                }
            }

            // Display web part when no error occurred
            if (!plcError.Visible && Visible)
            {
                // Hide web part if user is authenticated
                if (AuthenticationHelper.IsAuthenticated())
                {
                    Visible = false;
                    return;
                }

                plcPasswordNew.Visible = AllowFormsAuthentication;
                pnlExistingUser.Visible = AllowExistingUser;

                // Load LinkedIn data from session
                linkedInHelper = new LinkedInHelper();
                string linkedInData = ValidationHelper.GetString(SessionHelper.GetValue(SESSION_NAME_USERDATA), string.Empty);
                if (!string.IsNullOrEmpty(linkedInData))
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(linkedInData);
                    linkedInHelper.Initialize(doc);
                }

                // There is no LinkedIn user ID stored in session - hide all
                if (string.IsNullOrEmpty(linkedInHelper.MemberId) && HideForNoLinkedInUserID)
                {
                    Visible = false;
                }
                else if (!RequestHelper.IsPostBack())
                {
                    LoadData();
                }
            }
        }
        else
        {
            Visible = false;
        }
    }


    /// <summary>
    /// Handles btnOkExist click, joins existing user with LinkedIn member id.
    /// </summary>
    protected void btnOkExist_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(linkedInHelper.MemberId))
        {
            if (!String.IsNullOrEmpty(txtUserName.Text))
            {
                // Try to authenticate user
                UserInfo ui = AuthenticationHelper.AuthenticateUser(txtUserName.Text, txtPassword.Text, SiteContext.CurrentSiteName);

                // Check banned IPs
                BannedIPInfoProvider.CheckIPandRedirect(SiteContext.CurrentSiteName, BanControlEnum.Login);

                if (ui != null)
                {
                    // Add LinkedIn profile member id to user
                    ui.UserSettings.UserLinkedInID = linkedInHelper.MemberId;
                    UserInfoProvider.SetUserInfo(ui);

                    // Set authentication cookie and redirect to page
                    SetAuthCookieAndRedirect(ui);
                }
                else // Invalid credentials
                {
                    lblError.Text = GetString("Login_FailureText");
                    plcError.Visible = true;
                }
            }
            else // User did not fill the form
            {
                lblError.Text = GetString("mem.linkedin.fillloginform");
                plcError.Visible = true;
            }
        }
    }


    /// <summary>
    /// Handles btnOkNew click, creates new user and joins it with LinkedIn member id.
    /// </summary>
    protected void btnOkNew_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(linkedInHelper.MemberId))
        {
            string currentSiteName = SiteContext.CurrentSiteName;

            // Validate entered values
            string errorMessage = new Validator().IsRegularExp(txtUserNameNew.Text, "^([a-zA-Z0-9_\\-\\.@]+)$", GetString("mem.linkedin.fillcorrectusername"))
                                                 .IsEmail(txtEmail.Text, GetString("mem.linkedin.fillvalidemail")).Result;

            string password = passStrength.Text;

            // If password is enabled to set, check it
            if (plcPasswordNew.Visible && (String.IsNullOrEmpty(errorMessage)))
            {
                if (String.IsNullOrEmpty(password))
                {
                    errorMessage = GetString("mem.linkedin.specifyyourpass");
                }
                else if (password != txtConfirmPassword.Text.Trim())
                {
                    errorMessage = GetString("webparts_membership_registrationform.passwordonotmatch");
                }

                // Check policy
                if (!passStrength.IsValid())
                {
                    errorMessage = AuthenticationHelper.GetPolicyViolationMessage(SiteContext.CurrentSiteName);
                }
            }

            // Check whether email is unique if it is required
            if ((String.IsNullOrEmpty(errorMessage)) && !UserInfoProvider.IsEmailUnique(txtEmail.Text.Trim(), currentSiteName, 0))
            {
                errorMessage = GetString("UserInfo.EmailAlreadyExist");
            }

            // Check reserved names
            if ((String.IsNullOrEmpty(errorMessage)) && UserInfoProvider.NameIsReserved(currentSiteName, txtUserNameNew.Text.Trim()))
            {
                errorMessage = GetString("Webparts_Membership_RegistrationForm.UserNameReserved").Replace("%%name%%", HTMLHelper.HTMLEncode(txtUserNameNew.Text.Trim()));
            }

            if (String.IsNullOrEmpty(errorMessage))
            {
                // Check if user with given username already exists
                UserInfo ui = UserInfoProvider.GetUserInfo(txtUserNameNew.Text.Trim());

                // User with given username is already registered
                if (ui != null)
                {
                    plcError.Visible = true;
                    lblError.Text = GetString("mem.openid.usernameregistered");
                }
                else
                {
                    // Register new user
                    string error = DisplayMessage;
                    ui = AuthenticationHelper.AuthenticateLinkedInUser(linkedInHelper.MemberId, linkedInHelper.FirstName, linkedInHelper.LastName, currentSiteName, true, false, ref error);
                    DisplayMessage = error;

                    if (ui != null)
                    {
                        // Set additional information
                        ui.UserName = ui.UserNickName = txtUserNameNew.Text.Trim();
                        ui.Email = txtEmail.Text;

                        if (linkedInHelper.BirthDate != DateTimeHelper.ZERO_TIME)
                        {
                            ui.UserSettings.UserDateOfBirth = linkedInHelper.BirthDate;
                        }

                        // Set password
                        if (plcPasswordNew.Visible)
                        {
                            UserInfoProvider.SetPassword(ui, password);

                            // If user can choose password then is not considered external(external user can't login in common way)
                            ui.IsExternal = false;
                        }

                        UserInfoProvider.SetUserInfo(ui);

                        // Remove live user object from session, won't be needed
                        SessionHelper.Remove(SESSION_NAME_USERDATA);

                        // Notify administrator
                        bool requiresConfirmation = SettingsKeyInfoProvider.GetBoolValue(SiteContext.CurrentSiteName + ".CMSRegistrationEmailConfirmation");
                        if (!requiresConfirmation && NotifyAdministrator && (FromAddress != String.Empty) && (ToAddress != String.Empty))
                        {
                            AuthenticationHelper.NotifyAdministrator(ui, FromAddress, ToAddress);
                        }

                        // Send registration e-mails
                        AuthenticationHelper.SendRegistrationEmails(ui, ApprovalPage, password, true, SendWelcomeEmail);

                        // Log user registration into the web analytics and track conversion if set
                        AnalyticsHelper.TrackUserRegistration(currentSiteName, ui, TrackConversionName, ConversionValue);

                        Activity activity = new ActivityRegistration(ui, DocumentContext.CurrentDocument, AnalyticsContext.ActivityEnvironmentVariables);
                        if (activity.Data != null)
                        {
                            activity.Data.ContactID = ModuleCommands.OnlineMarketingGetUserLoginContactID(ui);
                            activity.Log();
                        }

                        // Set authentication cookie and redirect to page
                        SetAuthCookieAndRedirect(ui);

                        if (!String.IsNullOrEmpty(DisplayMessage))
                        {
                            lblInfo.Visible = true;
                            lblInfo.Text = DisplayMessage;
                            plcForm.Visible = false;
                        }
                        else
                        {
                            URLHelper.Redirect(ResolveUrl("~/Default.aspx"));
                        }
                    }
                }
            }
            // Validation failed - display error message
            else
            {
                lblError.Text = errorMessage;
                plcError.Visible = true;
            }
        }
    }

    #endregion


    #region "Private methods"

    /// <summary>
    /// Helper method, sets authentication cookie and redirects to return URL or default page.
    /// </summary>
    /// <param name="ui">User info</param>
    private void SetAuthCookieAndRedirect(UserInfo ui)
    {
        // Create autentification cookie
        if (ui.Enabled)
        {
            AuthenticationHelper.SetAuthCookieWithUserData(ui.UserName, true, Session.Timeout, new[] { "linkedinlogin" });

            int contactID = ModuleCommands.OnlineMarketingGetUserLoginContactID(ui);
            Activity activityLogin = new ActivityUserLogin(contactID, ui, DocumentContext.CurrentDocument, AnalyticsContext.ActivityEnvironmentVariables);
            activityLogin.Log();

            string returnUrl = QueryHelper.GetString("returnurl", null);

            // Redirect to ReturnURL
            if (!String.IsNullOrEmpty(returnUrl))
            {
                URLHelper.Redirect(ResolveUrl(HttpUtility.UrlDecode(returnUrl)));
            }
            // Redirect to default page
            else if (!String.IsNullOrEmpty(DefaultTargetUrl))
            {
                URLHelper.Redirect(ResolveUrl(DefaultTargetUrl));
            }
            // Otherwise refresh current page
            else
            {
                URLHelper.Redirect(RequestContext.CurrentURL);
            }
        }
    }


    /// <summary>
    /// Loads textboxes with LinkedIn data.
    /// </summary>
    private void LoadData()
    {
        string userName = linkedInHelper.FirstName;

        if (!String.IsNullOrEmpty(linkedInHelper.LastName))
        {
            if (String.IsNullOrEmpty(userName))
            {
                userName = linkedInHelper.LastName;
            }
            else
            {
                userName += "_" + linkedInHelper.LastName;
            }
        }
        txtUserNameNew.Text = userName;
    }

    #endregion
}