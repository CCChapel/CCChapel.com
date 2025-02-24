﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/CMSMasterPages/UI/SimplePage.master"
    Title="Mvtest properties" Inherits="CMSModules_OnlineMarketing_Pages_Content_MVTVariant_Edit"
    Theme="Default"  Codebehind="Edit.aspx.cs" %>

<%@ Register Src="~/CMSModules/OnlineMarketing/Controls/UI/MVTVariant/Edit.ascx"
    TagName="MvtVariantEdit" TagPrefix="cms" %>
<%@ Register Src="~/CMSAdminControls/Basic/DisabledModuleInfo.ascx" TagPrefix="cms"
    TagName="DisabledModule" %>

<asp:Content ID="cntBody" runat="server" ContentPlaceHolderID="plcContent">
    <asp:Panel runat="server" ID="pnlDisabled">
        <cms:DisabledModule runat="server" ID="ucDisabledModule" />
    </asp:Panel>
    <cms:MvtVariantEdit ID="editElem" runat="server" IsLiveSite="false" />
</asp:Content>
