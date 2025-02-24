﻿using System;
using System.Web.Security;
using System.Web.UI;

using CMS.Community;
using CMS.Core;
using CMS.ExtendedControls;
using CMS.Helpers;
using CMS.MacroEngine;
using CMS.MembershipProvider;
using CMS.PortalControls;
using CMS.Base;
using CMS.SiteProvider;
using CMS.Membership;
using CMS.DocumentEngine;
using CMS.DataEngine;

public partial class CMSWebParts_Community_Shortcuts : CMSAbstractWebPart, IPostBackEventHandler
{
    #region "Variables"

    /// <summary>
    /// Current user info.
    /// </summary>
    protected CurrentUserInfo currentUser = null;

    #endregion


    #region "Public properties"

    /// <summary>
    /// Gets or sets property to mark if Sign in link should be displayed.
    /// </summary>
    public bool DisplaySignIn
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplaySignIn"), true);
        }
        set
        {
            SetValue("DisplaySignIn", value);
        }
    }


    /// <summary>
    /// Gets or sets path to be redirected to when user clicks SignIn link.
    /// </summary>
    public string SignInPath
    {
        get
        {
            // Get path from path selector
            string signInPath = ValidationHelper.GetString(GetValue("SignInPath"), "");

            // If empty then use default logon page from settings
            if (signInPath == "")
            {
                signInPath = ResolveUrl(AuthenticationHelper.GetSecuredAreasLogonPage(SiteContext.CurrentSiteName));
            }
            else
            {
                signInPath = GetUrl(signInPath);
            }

            return signInPath;
        }
        set
        {
            SetValue("SignInPath", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the SignIn link.
    /// </summary>
    public string SignInText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("SignInText"), GetString("webparts_membership_signoutbutton.signin"));
        }
        set
        {
            SetValue("SignInText", value);
            lnkSignIn.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if Join the community link should be displayed.
    /// </summary>
    public bool DisplayJoinCommunity
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayJoinCommunity"), true);
        }
        set
        {
            SetValue("DisplayJoinCommunity", value);
        }
    }


    /// <summary>
    /// Gets or sets path to be redirected to when user clicks Join the community link.
    /// </summary>
    public string JoinCommunityPath
    {
        get
        {
            // Get path from path selector
            return ValidationHelper.GetString(GetValue("JoinCommunityPath"), "");
        }
        set
        {
            SetValue("JoinCommunityPath", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the join community link.
    /// </summary>
    public string JoinCommunityText
    {
        get
        {
            return DataHelper.GetNotEmpty(GetValue("JoinCommunityText"), GetString("group.member.join"));
        }
        set
        {
            SetValue("JoinCommunityText", value);
            lnkJoinCommunity.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if My profile link should be displayed.
    /// </summary>
    public bool DisplayMyProfileLink
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayMyProfileLink"), true);
        }
        set
        {
            SetValue("DisplayMyProfileLink", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the my profile link.
    /// </summary>
    public string MyProfileText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("MyProfileText"), null), GetString("shortcuts.myprofile"));
        }
        set
        {
            SetValue("MyProfileText", value);
            lnkMyProfile.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if Edit my profile link should be displayed.
    /// </summary>
    public bool DisplayEditMyProfileLink
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayEditMyProfileLink"), true);
        }
        set
        {
            SetValue("DisplayEditMyProfileLink", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the edit my profile link.
    /// </summary>
    public string EditMyProfileText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("EditMyProfileText"), null), GetString("shortcuts.editmyprofile"));
        }
        set
        {
            SetValue("EditMyProfileText", value);
            lnkEditMyProfile.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if Sign out link should be displayed.
    /// </summary>
    public bool DisplaySignOut
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplaySignOut"), true);
        }
        set
        {
            SetValue("DisplaySignOut", value);
        }
    }


    /// <summary>
    /// Gets or sets path to be redirected to when user clicks Sign out link.
    /// </summary>
    public string SignOutPath
    {
        get
        {
            // Get path from path selector
            return ValidationHelper.GetString(GetValue("SignOutPath"), "");
        }
        set
        {
            SetValue("SignOutPath", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the SignOut link.
    /// </summary>
    public string SignOutText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("SignOutText"), null), GetString("signoutbutton.signout"));
        }
        set
        {
            SetValue("SignOutText", value);
            btnSignOut.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if Create new group link should be displayed.
    /// </summary>
    public bool DisplayCreateNewGroup
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayCreateNewGroup"), true);
        }
        set
        {
            SetValue("DisplayCreateNewGroup", value);
        }
    }


    /// <summary>
    /// Gets or sets path to be redirected to when user clicks Create new group link.
    /// </summary>
    public string CreateNewGroupPath
    {
        get
        {
            // Get path from path selector
            return ValidationHelper.GetString(GetValue("CreateNewGroupPath"), "");
        }
        set
        {
            SetValue("CreateNewGroupPath", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the create new group link.
    /// </summary>
    public string CreateNewGroupText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("CreateNewGroupText"), null), GetString("group.creategroup"));
        }
        set
        {
            SetValue("CreateNewGroupText", value);
            lnkCreateNewGroup.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if Create new blok link should be displayed.
    /// </summary>
    public bool DisplayCreateNewBlog
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayCreateNewBlog"), true);
        }
        set
        {
            SetValue("DisplayCreateNewBlog", value);
        }
    }


    /// <summary>
    /// Gets or sets path to be redirected to when user clicks Create new blog link.
    /// </summary>
    public string CreateNewBlogPath
    {
        get
        {
            // Get path from path selector
            return ValidationHelper.GetString(GetValue("CreateNewBlogPath"), "");
        }
        set
        {
            SetValue("CreateNewBlogPath", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the create new blog link.
    /// </summary>
    public string CreateNewBlogText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("CreateNewBlogText"), null), GetString("blog.createblog"));
        }
        set
        {
            SetValue("CreateNewBlogText", value);
            lnkCreateNewBlog.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if Join/Leave the group link should be displayed.
    /// </summary>
    public bool DisplayGroupLinks
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayGroupLinks"), true);
        }
        set
        {
            SetValue("DisplayGroupLinks", value);
        }
    }


    /// <summary>
    /// Gets or sets path to be redirected to when user clicks Join the group link.
    /// </summary>
    public string JoinGroupPath
    {
        get
        {
            // Get path from path selector
            return ValidationHelper.GetString(GetValue("JoinGroupPath"), "");
        }
        set
        {
            SetValue("JoinGroupPath", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the join group link.
    /// </summary>
    public string JoinGroupText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("JoinGroupText"), null), GetString("group.joingroup"));
        }
        set
        {
            SetValue("JoinGroupText", value);
            lnkJoinGroup.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets path to be redirected to when user clicks Leave the group link.
    /// </summary>
    public string LeaveGroupPath
    {
        get
        {
            // Get path from path selector
            return ValidationHelper.GetString(GetValue("LeaveGroupPath"), "");
        }
        set
        {
            SetValue("LeaveGroupPath", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the leave group link.
    /// </summary>
    public string LeaveGroupText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("LeaveGroupText"), null), GetString("group.leavegroup"));
        }
        set
        {
            SetValue("LeaveGroupText", value);
            lnkLeaveGroup.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if Friendship link should be displayed.
    /// </summary>
    public bool DisplayFriendshipLinks
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayFriendshipLinks"), true);
        }
        set
        {
            SetValue("DisplayFriendshipLinks", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the reject friendship link.
    /// </summary>
    public string RejectFriendshipText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("RejectFriendshipText"), null), GetString("friends.rejectfriendship"));
        }
        set
        {
            SetValue("RejectFriendshipText", value);
            lnkRejectFriendship.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets text for the request friendship link.
    /// </summary>
    public string RequestFriendshipText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("RequestFriendshipText"), null), GetString("shortcuts.addasfriend"));
        }
        set
        {
            SetValue("RequestFriendshipText", value);
            requestFriendshipElem.LinkText = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if Send message link should be displayed.
    /// </summary>
    public bool DisplaySendMessage
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplaySendMessage"), true);
        }
        set
        {
            SetValue("DisplaySendMessage", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the send message link.
    /// </summary>
    public string SendMessageText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("SendMessageText"), null), GetString("sendmessage.sendmessage"));
        }
        set
        {
            SetValue("SendMessageText", value);
            lnkSendMessage.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if Add to contact list link should be displayed.
    /// </summary>
    public bool DisplayAddToContactList
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayAddToContactList"), true);
        }
        set
        {
            SetValue("DisplayAddToContactList", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the add to contact list link.
    /// </summary>
    public string AddToContactListText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("AddToContactListText"), null), GetString("messsaging.addtocontactlist"));
        }
        set
        {
            SetValue("AddToContactListText", value);
            lnkAddToContactList.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if Add to ignore list link should be displayed.
    /// </summary>
    public bool DisplayAddToIgnoreList
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayAddToIgnoreList"), true);
        }
        set
        {
            SetValue("DisplayAddToIgnoreList", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the add to ignore list link.
    /// </summary>
    public string AddToIgnoreListText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("AddToIgnoreListText"), null), GetString("messsaging.addtoignorelist"));
        }
        set
        {
            SetValue("AddToIgnoreListText", value);
            lnkAddToIgnoreList.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if Invite to group link should be displayed.
    /// </summary>
    public bool DisplayInviteToGroup
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayInviteToGroup"), true);
        }
        set
        {
            SetValue("DisplayInviteToGroup", value);
        }
    }


    /// <summary>
    /// Gets or sets path to be redirected to when user clicks Invite to the group link.
    /// </summary>
    public string InviteGroupPath
    {
        get
        {
            // Get path from path selector
            return ValidationHelper.GetString(GetValue("InviteGroupPath"), string.Empty);
        }
        set
        {
            SetValue("InviteGroupPath", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the ivite to group link.
    /// </summary>
    public string InviteGroupText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("InviteGroupText"), null), GetString("groupinvitation.invite"));
        }
        set
        {
            SetValue("InviteGroupText", value);
            lnkInviteToGroup.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if Manage the group should be displayed.
    /// </summary>
    public bool DisplayManageGroup
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayManageGroup"), true);
        }
        set
        {
            SetValue("DisplayManageGroup", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the manage group link.
    /// </summary>
    public string ManageGroupText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("ManageGroupText"), null), GetString("group.manage"));
        }
        set
        {
            SetValue("ManageGroupText", value);
            lnkManageGroup.Text = value;
        }
    }


    /// <summary>
    /// Indicates if the messaging module is loaded.
    /// </summary>
    public bool MessagingPresent
    {
        get
        {
            if (!RequestStockHelper.Contains("messagingPresent"))
            {
                RequestStockHelper.Add("messagingPresent", ModuleManager.IsModuleLoaded(ModuleName.MESSAGING));
            }
            return ValidationHelper.GetBoolean(RequestStockHelper.GetItem("messagingPresent"), false);
        }
    }


    /// <summary>
    /// Gets or sets property to mark if My messages link should be displayed.
    /// </summary>
    public bool DisplayMyMessages
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayMyMessages"), true);
        }
        set
        {
            SetValue("DisplayMyMessages", value);
        }
    }


    /// <summary>
    /// Gets or sets path to be redirected to when user clicks My messages link.
    /// </summary>
    public string MyMessagesPath
    {
        get
        {
            // Get path from path selector
            return ValidationHelper.GetString(GetValue("MyMessagesPath"), string.Empty);
        }
        set
        {
            SetValue("MyMessagesPath", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the my messages link.
    /// </summary>
    public string MyMessagesText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("MyMessagesText"), null), GetString("mydesk.mymessages"));
        }
        set
        {
            SetValue("MyMessagesText", value);
            lnkMyMessages.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if My friends link should be displayed.
    /// </summary>
    public bool DisplayMyFriends
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayMyFriends"), true);
        }
        set
        {
            SetValue("DisplayMyFriends", value);
        }
    }


    /// <summary>
    /// Gets or sets path to be redirected to when user clicks My messages link.
    /// </summary>
    public string MyFriendsPath
    {
        get
        {
            // Get path from path selector
            return ValidationHelper.GetString(GetValue("MyFriendsPath"), string.Empty);
        }
        set
        {
            SetValue("MyFriendsPath", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the my friends link.
    /// </summary>
    public string MyFriendsText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("MyFriendsText"), null), GetString("mydesk.myfriends"));
        }
        set
        {
            SetValue("MyFriendsText", value);
            lnkMyFriends.Text = value;
        }
    }


    /// <summary>
    /// Gets or sets property to mark if My invitations link should be displayed.
    /// </summary>
    public bool DisplayMyInvitations
    {
        get
        {
            return ValidationHelper.GetBoolean(GetValue("DisplayMyInvitations"), true);
        }
        set
        {
            SetValue("DisplayMyInvitations", value);
        }
    }


    /// <summary>
    /// Gets or sets path to be redirected to when user clicks My messages link.
    /// </summary>
    public string MyInvitationsPath
    {
        get
        {
            // Get path from path selector
            return ValidationHelper.GetString(GetValue("MyInvitationsPath"), string.Empty);
        }
        set
        {
            SetValue("MyInvitationsPath", value);
        }
    }


    /// <summary>
    /// Gets or sets text for the my invitations link.
    /// </summary>
    public string MyInvitationsText
    {
        get
        {
            return DataHelper.GetNotEmpty(ValidationHelper.GetString(GetValue("MyInvitationsText"), null), GetString("shortcuts.myinvitations"));
        }
        set
        {
            SetValue("MyInvitationsText", value);
            lnkMyInvitations.Text = value;
        }
    }

    #endregion


    #region "Events and methods"

    /// <summary>
    /// Content loaded event handler.
    /// </summary>
    public override void OnContentLoaded()
    {
        base.OnContentLoaded();
        SetupControl();
    }


    /// <summary>
    /// Reloads data.
    /// </summary>
    public override void ReloadData()
    {
        base.ReloadData();
        SetupControl();
    }


    /// <summary>
    /// Initializes the control properties.
    /// </summary>
    protected void SetupControl()
    {
        if (StopProcessing)
        {
            // Do nothing
        }
        else
        {
            // Initialize properties
            string script = "";

            // Set current user
            currentUser = MembershipContext.AuthenticatedUser;

            // Get Enable Friends setting
            bool friendsEnabled = UIHelper.IsFriendsModuleEnabled(SiteContext.CurrentSiteName);

            // Initialize strings
            lnkSignIn.Text = SignInText;
            lnkJoinCommunity.Text = JoinCommunityText;
            lnkMyProfile.Text = MyProfileText;
            lnkEditMyProfile.Text = EditMyProfileText;
            btnSignOut.Text = SignOutText;
            lnkCreateNewGroup.Text = CreateNewGroupText;
            lnkCreateNewBlog.Text = CreateNewBlogText;
            lnkJoinGroup.Text = JoinGroupText;
            lnkLeaveGroup.Text = LeaveGroupText;
            lnkRejectFriendship.Text = RejectFriendshipText;
            requestFriendshipElem.LinkText = RequestFriendshipText;
            lnkSendMessage.Text = SendMessageText;
            lnkAddToContactList.Text = AddToContactListText;
            lnkAddToIgnoreList.Text = AddToIgnoreListText;
            lnkInviteToGroup.Text = InviteGroupText;
            lnkManageGroup.Text = ManageGroupText;
            lnkMyMessages.Text = MyMessagesText;
            lnkMyFriends.Text = MyFriendsText;
            lnkMyInvitations.Text = MyInvitationsText;

            // If current user is public...
            if (currentUser.IsPublic())
            {
                // Display Sign In link if set so
                if (DisplaySignIn)
                {
                    // SignInPath returns URL - because of settings value
                    lnkSignIn.NavigateUrl = MacroResolver.ResolveCurrentPath(SignInPath);
                    pnlSignIn.Visible = true;
                    pnlSignInOut.Visible = true;
                }

                // Display Join the community link if set so
                if (DisplayJoinCommunity)
                {
                    lnkJoinCommunity.NavigateUrl = GetUrl(JoinCommunityPath);
                    pnlJoinCommunity.Visible = true;
                    pnlPersonalLinks.Visible = true;
                }
            }
            // If user is logged in
            else
            {
                // Display Sign out link if set so
                if (DisplaySignOut && !RequestHelper.IsWindowsAuthentication())
                {
                    pnlSignOut.Visible = true;
                    pnlSignInOut.Visible = true;
                }

                // Display Edit my profile link if set so
                if (DisplayEditMyProfileLink)
                {
                    lnkEditMyProfile.NavigateUrl = URLHelper.ResolveUrl(DocumentURLProvider.GetUrl(GroupMemberInfoProvider.GetMemberManagementPath(currentUser.UserName, SiteContext.CurrentSiteName)));
                    pnlEditMyProfile.Visible = true;
                    pnlProfileLinks.Visible = true;
                }

                // Display My profile link if set so
                if (DisplayMyProfileLink)
                {
                    lnkMyProfile.NavigateUrl = URLHelper.ResolveUrl(DocumentURLProvider.GetUrl(GroupMemberInfoProvider.GetMemberProfilePath(currentUser.UserName, SiteContext.CurrentSiteName)));
                    pnlMyProfile.Visible = true;
                    pnlProfileLinks.Visible = true;
                }

                // Display Create new group link if set so
                if (DisplayCreateNewGroup)
                {
                    lnkCreateNewGroup.NavigateUrl = GetUrl(CreateNewGroupPath);
                    pnlCreateNewGroup.Visible = true;
                    pnlGroupLinks.Visible = true;
                }

                // Display Create new blog link if set so
                if (DisplayCreateNewBlog)
                {
                    // Check that Community Module is present
                    var entry = ModuleManager.GetModule(ModuleName.BLOGS);
                    if (entry != null)
                    {
                        lnkCreateNewBlog.NavigateUrl = GetUrl(CreateNewBlogPath);
                        pnlCreateNewBlog.Visible = true;
                        pnlBlogLinks.Visible = true;
                    }
                }

                // Display My messages link
                if (DisplayMyMessages)
                {
                    lnkMyMessages.NavigateUrl = GetUrl(MyMessagesPath);
                    pnlMyMessages.Visible = true;
                    pnlPersonalLinks.Visible = true;
                }

                // Display My friends link
                if (DisplayMyFriends && friendsEnabled)
                {
                    lnkMyFriends.NavigateUrl = GetUrl(MyFriendsPath);
                    pnlMyFriends.Visible = true;
                    pnlPersonalLinks.Visible = true;
                }

                // Display My invitations link
                if (DisplayMyInvitations)
                {
                    lnkMyInvitations.NavigateUrl = GetUrl(MyInvitationsPath);
                    pnlMyInvitations.Visible = true;
                    pnlPersonalLinks.Visible = true;
                }

                GroupMemberInfo gmi = null;

                if (CommunityContext.CurrentGroup != null)
                {
                    // Get group info from community context
                    GroupInfo currentGroup = CommunityContext.CurrentGroup;

                    if (DisplayGroupLinks)
                    {
                        script += "function ReloadPage(){" + ControlsHelper.GetPostBackEventReference(this, "") + "}";

                        // Display Join group link if set so and user is visiting a group page
                        gmi = GetGroupMember(MembershipContext.AuthenticatedUser.UserID, currentGroup.GroupID);
                        if (gmi == null)
                        {
                            if (String.IsNullOrEmpty(JoinGroupPath))
                            {
                                script += "function JoinToGroupRequest() {\n" +
                                          "modalDialog('" + AuthenticationHelper.ResolveDialogUrl("~/CMSModules/Groups/CMSPages/JoinTheGroup.aspx") + "?groupid=" + currentGroup.GroupID + "','requestJoinToGroup', 500, 180); \n" +
                                          " } \n";

                                lnkJoinGroup.Attributes.Add("onclick", "JoinToGroupRequest();return false;");
                                lnkJoinGroup.NavigateUrl = RequestContext.CurrentURL;
                            }
                            else
                            {
                                lnkJoinGroup.NavigateUrl = GetUrl(JoinGroupPath);
                            }
                            pnlJoinGroup.Visible = true;
                            pnlGroupLinks.Visible = true;
                        }
                        else if ((gmi.MemberStatus == GroupMemberStatus.Approved) || (MembershipContext.AuthenticatedUser.IsGlobalAdministrator))
                        // Display Leave the group link if user is the group member
                        {
                            if (String.IsNullOrEmpty(LeaveGroupPath))
                            {
                                script += "function LeaveTheGroupRequest() {\n" +
                                          "modalDialog('" + AuthenticationHelper.ResolveDialogUrl("~/CMSModules/Groups/CMSPages/LeaveTheGroup.aspx") + "?groupid=" + currentGroup.GroupID + "','requestLeaveThGroup', 500, 180); \n" +
                                          " } \n";

                                lnkLeaveGroup.Attributes.Add("onclick", "LeaveTheGroupRequest();return false;");
                                lnkLeaveGroup.NavigateUrl = RequestContext.CurrentURL;
                            }
                            else
                            {
                                lnkLeaveGroup.NavigateUrl = GetUrl(LeaveGroupPath);
                            }

                            pnlLeaveGroup.Visible = true;
                            pnlGroupLinks.Visible = true;
                        }
                    }

                    // Display Manage the group link if set so and user is logged as group administrator and user is visiting a group page
                    if (DisplayManageGroup && (currentUser.IsGroupAdministrator(currentGroup.GroupID) || (currentUser.IsGlobalAdministrator)))
                    {
                        lnkManageGroup.NavigateUrl = ResolveUrl(DocumentURLProvider.GetUrl(GroupInfoProvider.GetGroupManagementPath(currentGroup.GroupName, SiteContext.CurrentSiteName)));
                        pnlManageGroup.Visible = true;
                        pnlGroupLinks.Visible = true;
                    }
                }

                // Get user info from site context
                UserInfo siteContextUser = MembershipContext.CurrentUserProfile;

                if (DisplayInviteToGroup)
                {
                    // Get group info from community context
                    GroupInfo currentGroup = CommunityContext.CurrentGroup;

                    // Display invite to group link for user who is visiting a group page
                    if (currentGroup != null)
                    {
                        // Get group user
                        if (gmi == null)
                        {
                            gmi = GetGroupMember(MembershipContext.AuthenticatedUser.UserID, currentGroup.GroupID);
                        }

                        if (((gmi != null) && (gmi.MemberStatus == GroupMemberStatus.Approved)) || (MembershipContext.AuthenticatedUser.IsGlobalAdministrator))
                        {
                            pnlInviteToGroup.Visible = true;

                            if (String.IsNullOrEmpty(InviteGroupPath))
                            {
                                script += "function InviteToGroup() {\n modalDialog('" + AuthenticationHelper.ResolveDialogUrl("~/CMSModules/Groups/CMSPages/InviteToGroup.aspx") + "?groupid=" + currentGroup.GroupID + "','inviteToGroup', 800, 450); \n } \n";
                                lnkInviteToGroup.Attributes.Add("onclick", "InviteToGroup();return false;");
                                lnkInviteToGroup.NavigateUrl = RequestContext.CurrentURL;
                            }
                            else
                            {
                                lnkInviteToGroup.NavigateUrl = GetUrl(InviteGroupPath);
                            }
                        }
                    }
                    // Display invite to group link for user who is visiting another user's page
                    else if ((siteContextUser != null) && (siteContextUser.UserName != currentUser.UserName) && (GroupInfoProvider.GetUserGroupsCount(currentUser, SiteContext.CurrentSite) != 0))
                    {
                        pnlInviteToGroup.Visible = true;

                        if (String.IsNullOrEmpty(InviteGroupPath))
                        {
                            script += "function InviteToGroup() {\n modalDialog('" + AuthenticationHelper.ResolveDialogUrl("~/CMSModules/Groups/CMSPages/InviteToGroup.aspx") + "?invitedid=" + siteContextUser.UserID + "','inviteToGroup', 700, 400); \n } \n";
                            lnkInviteToGroup.Attributes.Add("onclick", "InviteToGroup();return false;");
                            lnkInviteToGroup.NavigateUrl = RequestContext.CurrentURL;
                        }
                        else
                        {
                            lnkInviteToGroup.NavigateUrl = GetUrl(InviteGroupPath);
                        }
                    }
                }

                if (siteContextUser != null)
                {
                    // Display Friendship link if set so and user is visiting an user's page
                    if (DisplayFriendshipLinks && (currentUser.UserID != siteContextUser.UserID) && friendsEnabled)
                    {
                        FriendshipStatusEnum status = MembershipContext.AuthenticatedUser.HasFriend(siteContextUser.UserID);
                        switch (status)
                        {
                            case FriendshipStatusEnum.Approved:
                                // Friendship rejection
                                script += "function ShortcutFriendshipReject(id) { \n" +
                                          "modalDialog('" + AuthenticationHelper.ResolveDialogUrl("~/CMSModules/Friends/CMSPages/Friends_Reject.aspx") + "?userid=" + currentUser.UserID + "&requestid=' + id , 'rejectFriend', 410, 270); \n" +
                                          " } \n";

                                lnkRejectFriendship.Attributes.Add("onclick", "ShortcutFriendshipReject('" + siteContextUser.UserID + "');return false;");
                                lnkRejectFriendship.NavigateUrl = RequestContext.CurrentURL;
                                pnlRejectFriendship.Visible = true;
                                pnlFriendshipLinks.Visible = true;
                                break;

                            case FriendshipStatusEnum.None:
                                requestFriendshipElem.UserID = currentUser.UserID;
                                requestFriendshipElem.RequestedUserID = siteContextUser.UserID;
                                pnlFriendshipLink.Visible = true;
                                pnlFriendshipLinks.Visible = true;
                                break;
                        }
                    }

                    // Show messaging links if enabled
                    if (MessagingPresent && (currentUser.UserID != siteContextUser.UserID))
                    {
                        // Display Send message link if user is visiting an user's page
                        if (DisplaySendMessage)
                        {
                            // Send private message
                            script += "function ShortcutPrivateMessage(id) { \n" +
                                      "modalDialog('" + AuthenticationHelper.ResolveDialogUrl("~/CMSModules/Messaging/CMSPages/SendMessage.aspx") + "?userid=" + currentUser.UserID + "&requestid=' + id , 'sendMessage', 390, 390); \n" +
                                      " } \n";

                            lnkSendMessage.Attributes.Add("onclick", "ShortcutPrivateMessage('" + siteContextUser.UserID + "');return false;");
                            lnkSendMessage.NavigateUrl = RequestContext.CurrentURL;
                            pnlSendMessage.Visible = true;
                            pnlMessageLinks.Visible = true;
                        }

                        // Display Add to contact list link if user is visiting an user's page
                        if (DisplayAddToContactList)
                        {
                            // Check if user is in contact list
                            bool isInContactList = ModuleCommands.MessagingIsInContactList(currentUser.UserID, siteContextUser.UserID);

                            // Add to actions
                            if (!isInContactList)
                            {
                                lnkAddToContactList.Attributes.Add("onclick", "return ShortcutAddToContactList('" + siteContextUser.UserID + "')");
                                lnkAddToContactList.NavigateUrl = RequestContext.CurrentURL;
                                pnlAddToContactList.Visible = true;
                                pnlMessageLinks.Visible = true;

                                // Add to contact list
                                script += "function ShortcutAddToContactList(usertoadd) { \n" +
                                          "var confirmation = confirm(" + ScriptHelper.GetString(GetString("messaging.contactlist.addconfirmation")) + ");" +
                                          "if(confirmation)" +
                                          "{" +
                                          "selectedIdElem = document.getElementById('" + hdnSelectedId.ClientID + "'); \n" +
                                          "if (selectedIdElem != null) { selectedIdElem.value = usertoadd;}" +
                                          ControlsHelper.GetPostBackEventReference(this, "addtocontactlist", false) +
                                          "} return false;}\n";
                            }
                        }

                        // Display Add to ignore list link if user is visiting an user's page
                        if (DisplayAddToIgnoreList)
                        {
                            // Check if user is in ignore list
                            bool isInIgnoreList = ModuleCommands.MessagingIsInIgnoreList(currentUser.UserID, siteContextUser.UserID);

                            // Add to ignore list 
                            if (!isInIgnoreList)
                            {
                                lnkAddToIgnoreList.Attributes.Add("onclick", "return ShortcutAddToIgnoretList('" + siteContextUser.UserID + "')");
                                lnkAddToIgnoreList.NavigateUrl = RequestContext.CurrentURL;
                                pnlAddToIgnoreList.Visible = true;
                                pnlMessageLinks.Visible = true;

                                // Add to ignore list
                                script += "function ShortcutAddToIgnoretList(usertoadd) { \n" +
                                          "var confirmation = confirm(" + ScriptHelper.GetString(GetString("messaging.ignorelist.addconfirmation")) + ");" +
                                          "if(confirmation)" +
                                          "{" +
                                          "selectedIdElem = document.getElementById('" + hdnSelectedId.ClientID + "'); \n" +
                                          "if (selectedIdElem != null) { selectedIdElem.value = usertoadd;}" +
                                          ControlsHelper.GetPostBackEventReference(this, "addtoignorelist", false) +
                                          "} return false; } \n";
                            }
                        }
                    }
                }
            }

            // Register menu management scripts
            ScriptHelper.RegisterClientScriptBlock(this, typeof(string), "Shortcuts_" + ClientID, ScriptHelper.GetScript(script));

            // Register the dialog script
            ScriptHelper.RegisterDialogScript(Page);
        }
    }


    /// <summary>
    /// SignOut click event handler.
    /// </summary>
    protected void btnSignOut_Click(object sender, EventArgs e)
    {
        if (currentUser == null)
        {
            currentUser = MembershipContext.AuthenticatedUser;
        }
        if (AuthenticationHelper.IsAuthenticated())
        {

            string redirectUrl = SignOutPath != "" ? GetUrl(SignOutPath) : RequestContext.CurrentURL;

            // If the user is Windows Live user
            if (!string.IsNullOrEmpty(currentUser.UserSettings.WindowsLiveID))
            {
                string siteName = SiteContext.CurrentSiteName;

                // Get LiveID settings
                string appId = SettingsKeyInfoProvider.GetValue(siteName + ".CMSApplicationID");
                string secret = SettingsKeyInfoProvider.GetValue(siteName + ".CMSApplicationSecret");

                // Check valid Windows LiveID parameters
                if ((appId != string.Empty) && (secret != string.Empty))
                {
                    WindowsLiveLogin wll = new WindowsLiveLogin(appId, secret);

                    // Redirect to Windows Live and back to "home" page
                    string defaultAliasPath = SettingsKeyInfoProvider.GetValue(siteName + ".CMSDefaultAliasPath");
                    string url = DocumentURLProvider.GetUrl(defaultAliasPath);
                    redirectUrl = wll.GetLogoutUrl(URLHelper.GetAbsoluteUrl(url));
                }
            }

            AuthenticationHelper.SignOut();

            Response.Cache.SetNoStore();
            URLHelper.Redirect(redirectUrl);
        }
    }


    /// <summary>
    /// Postback handling.
    /// </summary>
    /// <param name="eventArgument">Argument of postback event</param>
    public void RaisePostBackEvent(string eventArgument)
    {
        if ((eventArgument == null))
        {
            return;
        }

        // Get ID of user
        int selectedId = ValidationHelper.GetInteger(hdnSelectedId.Value, 0);

        // Add only if messaging is present
        if (MessagingPresent)
        {
            // Get the module entry
            if (currentUser == null)
            {
                currentUser = MembershipContext.AuthenticatedUser;
            }

            // Add to contact or ignore list
            switch (eventArgument)
            {
                case "addtoignorelist":
                    ModuleCommands.MessagingAddToIgnoreList(currentUser.UserID, selectedId);
                    break;

                case "addtocontactlist":
                    ModuleCommands.MessagingAddToContactList(currentUser.UserID, selectedId);
                    break;
            }

            URLHelper.Redirect(RequestContext.CurrentURL);
        }
    }


    /// <summary>
    /// Gets URL from given path.
    /// </summary>
    /// <param name="path">Node alias path</param>
    private string GetUrl(string path)
    {
        return ResolveUrl(DocumentURLProvider.GetUrl(MacroResolver.ResolveCurrentPath(path)));
    }


    /// <summary>
    /// Returns group member info, reult is cached in request.
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="groupId">Group ID</param>
    private GroupMemberInfo GetGroupMember(int userId, int groupId)
    {
        GroupMemberInfo gmi = RequestStockHelper.GetItem("CommunityShortCuts" + userId.ToString() + "_" + groupId.ToString()) as GroupMemberInfo;
        if ((gmi == null) && (!RequestStockHelper.Contains("CommunityShortCuts" + userId.ToString() + "_" + groupId.ToString())))
        {
            gmi = GroupMemberInfoProvider.GetGroupMemberInfo(userId, groupId);
            if (gmi != null)
            {
                RequestStockHelper.Add("CommunityShortCuts" + userId.ToString() + "_" + groupId.ToString(), gmi);
            }
            else
            {
                RequestStockHelper.Add("CommunityShortCuts" + userId.ToString() + "_" + groupId.ToString(), false);
            }
        }

        return gmi;
    }

    #endregion
}