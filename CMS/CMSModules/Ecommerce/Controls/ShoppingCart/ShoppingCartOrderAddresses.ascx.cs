﻿using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS.DataEngine;
using CMS.Ecommerce;
using CMS.EcommerceProvider;
using CMS.EventLog;
using CMS.Globalization;
using CMS.Helpers;
using CMS.SiteProvider;

public partial class CMSModules_Ecommerce_Controls_ShoppingCart_ShoppingCartOrderAddresses : ShoppingCartStep
{
    #region "ViewState Constants"

    // Constants for billing address
    private const string BILLING_ADDRESS_ID = "BillingAddressID";
    private const string BILLING_ADDRESS_NAME = "BillingAddressName";
    private const string BILLING_ADDRESS_LINE1 = "BillingAddressLine1";
    private const string BILLING_ADDRESS_LINE2 = "BillingAddressLine2";
    private const string BILLING_ADDRESS_CITY = "BillingAddressCity";
    private const string BILLING_ADDRESS_ZIP = "BillingAddressZIP";
    private const string BILLING_ADDRESS_COUNTRY_ID = "BillingAddressCountryID";
    private const string BILLING_ADDRESS_STATE_ID = "BillingAddressStateID";
    private const string BILLING_ADDRESS_PHONE = "BillingAddressPhone";

    // Constants for shipping address
    private const string SHIPPING_ADDRESS_CHECKED = "ShippingAddressChecked";
    private const string SHIPPING_ADDRESS_ID = "ShippingAddressID";
    private const string SHIPPING_ADDRESS_NAME = "ShippingAddressName";
    private const string SHIPPING_ADDRESS_LINE1 = "ShippingAddressLine1";
    private const string SHIPPING_ADDRESS_LINE2 = "ShippingAddressLine2";
    private const string SHIPPING_ADDRESS_CITY = "ShippingAddressCity";
    private const string SHIPPING_ADDRESS_ZIP = "ShippingAddressZIP";
    private const string SHIPPING_ADDRESS_COUNTRY_ID = "ShippingAddressCountryID";
    private const string SHIPPING_ADDRESS_STATE_ID = "ShippingAddressStateID";
    private const string SHIPPING_ADDRESS_PHONE = "ShippingAddressPhone";

    // Constants for company address
    private const string COMPANY_ADDRESS_CHECKED = "CompanyAddressChecked";
    private const string COMPANY_ADDRESS_ID = "CompanyAddressID";
    private const string COMPANY_ADDRESS_NAME = "CompanyAddressName";
    private const string COMPANY_ADDRESS_LINE1 = "CompanyAddressLine1";
    private const string COMPANY_ADDRESS_LINE2 = "CompanyAddressLine2";
    private const string COMPANY_ADDRESS_CITY = "CompanyAddressCity";
    private const string COMPANY_ADDRESS_ZIP = "CompanyAddressZIP";
    private const string COMPANY_ADDRESS_COUNTRY_ID = "CompanyAddressCountryID";
    private const string COMPANY_ADDRESS_STATE_ID = "CompanyAddressStateID";
    private const string COMPANY_ADDRESS_PHONE = "CompanyAddressPhone";

    #endregion


    #region "Temporary values operations"

    /// <summary>
    /// Removes billing address values from ShoppingCart ViewState.
    /// </summary>
    private void RemoveBillingTempValues()
    {
        // Billing address values
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_ID, null);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_CITY, null);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_COUNTRY_ID, null);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_LINE1, null);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_LINE2, null);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_NAME, null);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_PHONE, null);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_STATE_ID, null);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_ZIP, null);
    }


    /// <summary>
    /// Removes shipping address values from ShoppingCart ViewState.
    /// </summary>
    private void RemoveShippingTempValues()
    {
        // Shipping address values
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_CHECKED, null);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_ID, null);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_CITY, null);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_COUNTRY_ID, null);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_LINE1, null);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_LINE2, null);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_NAME, null);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_PHONE, null);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_STATE_ID, null);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_ZIP, null);
    }


    /// <summary>
    /// Removes company address values from ShoppingCart ViewState.
    /// </summary>
    private void RemoveCompanyTempValues()
    {
        // Company address values
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_CHECKED, null);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_ID, null);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_CITY, null);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_COUNTRY_ID, null);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_LINE1, null);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_LINE2, null);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_NAME, null);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_PHONE, null);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_STATE_ID, null);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_ZIP, null);
    }


    /// <summary>
    /// Loads billing address temp values.
    /// </summary>
    private void LoadBillingFromViewState()
    {
        txtBillingName.Text = Convert.ToString(ShoppingCartControl.GetTempValue(BILLING_ADDRESS_NAME));
        txtBillingAddr1.Text = Convert.ToString(ShoppingCartControl.GetTempValue(BILLING_ADDRESS_LINE1));
        txtBillingAddr2.Text = Convert.ToString(ShoppingCartControl.GetTempValue(BILLING_ADDRESS_LINE2));
        txtBillingCity.Text = Convert.ToString(ShoppingCartControl.GetTempValue(BILLING_ADDRESS_CITY));
        txtBillingZip.Text = Convert.ToString(ShoppingCartControl.GetTempValue(BILLING_ADDRESS_ZIP));
        txtBillingPhone.Text = Convert.ToString(ShoppingCartControl.GetTempValue(BILLING_ADDRESS_PHONE));
        CountrySelector1.CountryID = ValidationHelper.GetInteger(ShoppingCartControl.GetTempValue(BILLING_ADDRESS_COUNTRY_ID), 0);
        CountrySelector1.StateID = ValidationHelper.GetInteger(ShoppingCartControl.GetTempValue(BILLING_ADDRESS_STATE_ID), 0);
    }


    /// <summary>
    /// Loads shipping address temp values.
    /// </summary>
    private void LoadShippingFromViewState()
    {
        txtShippingName.Text = Convert.ToString(ShoppingCartControl.GetTempValue(SHIPPING_ADDRESS_NAME));
        txtShippingAddr1.Text = Convert.ToString(ShoppingCartControl.GetTempValue(SHIPPING_ADDRESS_LINE1));
        txtShippingAddr2.Text = Convert.ToString(ShoppingCartControl.GetTempValue(SHIPPING_ADDRESS_LINE2));
        txtShippingCity.Text = Convert.ToString(ShoppingCartControl.GetTempValue(SHIPPING_ADDRESS_CITY));
        txtShippingZip.Text = Convert.ToString(ShoppingCartControl.GetTempValue(SHIPPING_ADDRESS_ZIP));
        txtShippingPhone.Text = Convert.ToString(ShoppingCartControl.GetTempValue(SHIPPING_ADDRESS_PHONE));
        CountrySelector2.CountryID = ValidationHelper.GetInteger(ShoppingCartControl.GetTempValue(SHIPPING_ADDRESS_COUNTRY_ID), 0);
        CountrySelector2.StateID = ValidationHelper.GetInteger(ShoppingCartControl.GetTempValue(SHIPPING_ADDRESS_STATE_ID), 0);
    }


    /// <summary>
    /// Loads company address temp values.
    /// </summary>
    private void LoadCompanyFromViewState()
    {
        txtCompanyName.Text = Convert.ToString(ShoppingCartControl.GetTempValue(COMPANY_ADDRESS_NAME));
        txtCompanyLine1.Text = Convert.ToString(ShoppingCartControl.GetTempValue(COMPANY_ADDRESS_LINE1));
        txtCompanyLine2.Text = Convert.ToString(ShoppingCartControl.GetTempValue(COMPANY_ADDRESS_LINE2));
        txtCompanyCity.Text = Convert.ToString(ShoppingCartControl.GetTempValue(COMPANY_ADDRESS_CITY));
        txtCompanyZip.Text = Convert.ToString(ShoppingCartControl.GetTempValue(COMPANY_ADDRESS_ZIP));
        txtCompanyPhone.Text = Convert.ToString(ShoppingCartControl.GetTempValue(COMPANY_ADDRESS_PHONE));
        CountrySelector3.CountryID = ValidationHelper.GetInteger(ShoppingCartControl.GetTempValue(COMPANY_ADDRESS_COUNTRY_ID), 0);
        CountrySelector3.StateID = ValidationHelper.GetInteger(ShoppingCartControl.GetTempValue(COMPANY_ADDRESS_STATE_ID), 0);
    }


    /// <summary>
    /// Back button actions.
    /// </summary>
    public override void ButtonBackClickAction()
    {
        // Billing address values
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_ID, drpBillingAddr.SelectedValue);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_CITY, txtBillingCity.Text);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_COUNTRY_ID, CountrySelector1.CountryID);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_LINE1, txtBillingAddr1.Text);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_LINE2, txtBillingAddr2.Text);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_NAME, txtBillingName.Text);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_PHONE, txtBillingPhone.Text);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_STATE_ID, CountrySelector1.StateID);
        ShoppingCartControl.SetTempValue(BILLING_ADDRESS_ZIP, txtBillingZip.Text);

        // Shipping address values
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_CHECKED, chkShippingAddr.Checked);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_ID, drpShippingAddr.SelectedValue);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_CITY, txtShippingCity.Text);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_COUNTRY_ID, CountrySelector2.CountryID);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_LINE1, txtShippingAddr1.Text);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_LINE2, txtShippingAddr2.Text);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_NAME, txtShippingName.Text);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_PHONE, txtShippingPhone.Text);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_STATE_ID, CountrySelector2.StateID);
        ShoppingCartControl.SetTempValue(SHIPPING_ADDRESS_ZIP, txtShippingZip.Text);

        // Company address values
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_CHECKED, chkCompanyAddress.Checked);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_ID, drpCompanyAddress.SelectedValue);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_CITY, txtCompanyCity.Text);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_COUNTRY_ID, CountrySelector3.CountryID);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_LINE1, txtCompanyLine1.Text);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_LINE2, txtCompanyLine2.Text);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_NAME, txtCompanyName.Text);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_PHONE, txtCompanyPhone.Text);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_STATE_ID, CountrySelector3.StateID);
        ShoppingCartControl.SetTempValue(COMPANY_ADDRESS_ZIP, txtCompanyZip.Text);

        base.ButtonBackClickAction();
    }

    #endregion


    /// <summary>
    /// Private properties.
    /// </summary>
    private int mCustomerId = 0;

    private SiteInfo mCurrentSite = null;


    /// <summary>
    /// Page load.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        mCurrentSite = SiteContext.CurrentSite;

        CountrySelector1.IsLiveSite = IsLiveSite;
        CountrySelector2.IsLiveSite = IsLiveSite;
        CountrySelector3.IsLiveSite = IsLiveSite;

        lblShippingTitle.Text = GetString("ShoppingCart.ShippingAddress");
        lblCompanyAddressTitle.Text = GetString("ShoppingCart.CompanyAddress");

        drpBillingAddr.SelectedIndexChanged += drpBillingAddr_SelectedIndexChanged;
        drpShippingAddr.SelectedIndexChanged += drpShippingAddr_SelectedIndexChanged;
        drpCompanyAddress.SelectedIndexChanged += drpCompanyAddress_SelectedIndexChanged;

        // Initialize labels.
        LabelInitialize();
        //this.TitleText = GetString("Order_new.ShoppingCartOrderAddresses.Title");

        // Get customer ID from ShoppingCartInfoObj
        mCustomerId = ShoppingCart.ShoppingCartCustomerID;

        // Display/ Hide company address panel with checkbox to display company address detail        
        plcCompanyAll.Visible = ((mCurrentSite != null) && (ECommerceSettings.UseExtraCompanyAddress(mCurrentSite.SiteName)));

        // Get customer info.
        CustomerInfo ci = CustomerInfoProvider.GetCustomerInfo(mCustomerId);

        if (ci != null)
        {
            // Display customer addresses if customer is not anonymous
            if (ci.CustomerID > 0)
            {
                plhBillAddr.Visible = true;
                plhShippAddr.Visible = true;
                plcCompanyAddress.Visible = true;
                if (!ShoppingCartControl.IsCurrentStepPostBack)
                {
                    // Initialize customer billing and shipping addresses
                    InitializeAddresses();
                }
            }
        }
        else
        {
            lblError.Visible = true;
            lblError.Text = GetString("Ecommerce.NoCustomerSelected");

            // Hide forms when no customer selected
            plcCompanyAll.Visible = false;
            plcShippingAddress.Visible = false;
            plcBillingAddress.Visible = false;
        }

        // If shopping cart does not need shipping
        if (!ShoppingCart.IsShippingNeeded)
        {
            // Hide title
            headTitle.Visible = false;

            // Hide shipping address section
            plcShippingAddress.Visible = false;

            // Change current checkout process step caption
            ShoppingCartControl.CheckoutProcessSteps[ShoppingCartControl.CurrentStepIndex].Caption = GetString("order_new.shoppingcartorderaddresses.titlenoshipping");
        }
    }


    /// <summary>
    /// Initialize page labels.
    /// </summary>
    protected void LabelInitialize()
    {
        lblBillingAddr.Text = GetString("ShoppingCartOrderAddresses.BillingAddr");
        lblBillingName.Text = GetString("ShoppingCartOrderAddresses.BillingName");
        lblBillingAddrLine.Text = GetString("ShoppingCartOrderAddresses.BillingAddrLine");
        lblBillingAddrLine2.Text = GetString("ShoppingCartOrderAddresses.BillingAddrLine2");
        lblBillingCity.Text = GetString("ShoppingCartOrderAddresses.BillingCity");
        lblBillingZip.Text = GetString("ShoppingCartOrderAddresses.BillingZIP");
        lblBillingCountry.Text = GetString("ShoppingCartOrderAddresses.BillingCountry");
        lblBillingPhone.Text = GetString("ShoppingCartOrderAddresses.BillingPhone");
        chkShippingAddr.Text = GetString("ShoppingCartOrderAddresses.DifferentShippingAddr");
        lblShippingAddr.Text = GetString("ShoppingCartOrderAddresses.ShippingAddr");

        lblShippingName.Text = lblBillingName.Text;
        lblShippingAddrLine.Text = lblBillingAddrLine.Text;
        lblShippingAddrLine2.Text = lblBillingAddrLine2.Text;
        lblShippingCity.Text = lblBillingCity.Text;
        lblShippingZip.Text = lblBillingZip.Text;
        lblShippingCountry.Text = lblBillingCountry.Text;
        lblShippingPhone.Text = lblBillingPhone.Text;

        lblCompanyName.Text = lblBillingName.Text;
        lblCompanyLines.Text = lblBillingAddrLine.Text;
        lblCompanyLines2.Text = lblBillingAddrLine2.Text;
        lblCompanyCity.Text = lblBillingCity.Text;
        lblCompanyZip.Text = lblBillingZip.Text;
        lblCompanyCountry.Text = lblBillingCountry.Text;
        lblCompanyPhone.Text = lblBillingPhone.Text;

        lblCompanyAddress.Text = GetString("ShoppingCartOrderAddresses.lblCompanyAddress");
        chkCompanyAddress.Text = GetString("ShoppingCartOrderAddresses.chkCompanyAddress");

        lblError.Text = "";
        lblError.Visible = false;
    }


    /// <summary>
    /// Initialize customer's addresses in billing and shipping dropdown lists.
    /// </summary>
    protected void InitializeAddresses()
    {
        // add new item <(new), 0>
        ListItem li = new ListItem(GetString("ShoppingCartOrderAddresses.NewAddress"), "0");


        // get all billing addresses of the customer
        if (drpBillingAddr.Items.Count == 0)
        {
            DataSet ds = AddressInfoProvider.GetAddresses(mCustomerId, true, false, false, true);
            drpBillingAddr.DataSource = ds;
            drpBillingAddr.DataBind();
            drpBillingAddr.Items.Insert(0, li);
        }

        li = new ListItem(GetString("ShoppingCartOrderAddresses.NewAddress"), "0");

        // get all shipping addresses of the customer
        if (drpShippingAddr.Items.Count == 0)
        {
            DataSet ds = AddressInfoProvider.GetAddresses(mCustomerId, false, true, false, true);
            drpShippingAddr.DataSource = ds;
            drpShippingAddr.DataBind();
            drpShippingAddr.Items.Insert(0, li);
        }

        li = new ListItem(GetString("ShoppingCartOrderAddresses.NewAddress"), "0");

        // get all shipping addresses of the customer
        if (drpCompanyAddress.Items.Count == 0)
        {
            DataSet ds = AddressInfoProvider.GetAddresses(mCustomerId, false, false, true, true);
            drpCompanyAddress.DataSource = ds;
            drpCompanyAddress.DataBind();
            drpCompanyAddress.Items.Insert(0, li);
        }

        LoadBillingSelectedValue();

        // Try remove same shipping address as selected billing address
        // (if the selected value is not "0")
        if (drpBillingAddr.SelectedValue != "0")
        {
            try
            {
                drpShippingAddr.Items.Remove(drpBillingAddr.SelectedItem);
            }
            catch
            {
            }
        }
        LoadShippingSelectedValue();


        // Try remove same company address as selected billing address
        // (if the selected value is not "0")
        if (drpBillingAddr.SelectedValue != "0")
        {
            try
            {
                drpCompanyAddress.Items.Remove(drpBillingAddr.SelectedItem);
            }
            catch
            {
            }
        }
        LoadCompanySelectedValue();

        LoadBillingAddressInfo();
        LoadShippingAddressInfo();
        LoadCompanyAddressInfo();
    }


    protected void LoadBillingSelectedValue()
    {
        try
        {
            drpBillingAddr.ClearSelection();
            int lastBillingAddressId = 0;

            // Get last modified billing addresses
            var lastAddress = AddressInfoProvider.GetAddresses(mCustomerId, true, false, false, true).TopN(1).OrderByDescending("AddressLastModified");
            if (!DataHelper.DataSourceIsEmpty(lastAddress))
            {
                lastBillingAddressId = lastAddress.FirstObject.AddressID;
            }

            // Set billing address
            // Try to select billing address from ViewState first
            object viewStateValue = ShoppingCartControl.GetTempValue(BILLING_ADDRESS_ID);
            if (viewStateValue != null)
            {
                try
                {
                    drpBillingAddr.SelectedValue = Convert.ToString(viewStateValue);
                }
                catch
                {
                }
            }
            // If there is already selected billing address, set it
            else if (ShoppingCart.ShoppingCartBillingAddress != null)
            {
                try
                {
                    drpBillingAddr.SelectedValue = ShoppingCart.ShoppingCartBillingAddress.AddressID.ToString();
                }
                catch
                {
                }
            }
            // If there is last used billing address, set it
            else if (lastBillingAddressId != 0)
            {
                try
                {
                    drpBillingAddr.SelectedValue = lastBillingAddressId.ToString();
                }
                catch
                {
                }
            }
            else if (drpBillingAddr.Items.Count > 1)
            {
                drpBillingAddr.SelectedIndex = 1;
            }
        }
        catch
        {
        }
    }


    protected void LoadShippingSelectedValue()
    {
        try
        {
            drpShippingAddr.ClearSelection();
            int lastShippingAddressId = 0;

            // Get last modified shipping addresses
            var lastAddress = AddressInfoProvider.GetAddresses(mCustomerId, false, true, false, true).TopN(1).OrderByDescending("AddressLastModified");
            if (!DataHelper.DataSourceIsEmpty(lastAddress))
            {
                lastShippingAddressId = lastAddress.FirstObject.AddressID;
            }

            // Try to select shipping address from ViewState first
            object viewStateValue = ShoppingCartControl.GetTempValue(SHIPPING_ADDRESS_ID);
            bool viewStateChecked = ValidationHelper.GetBoolean(ShoppingCartControl.GetTempValue(SHIPPING_ADDRESS_CHECKED), false);
            if (viewStateValue != null)
            {
                try
                {
                    drpShippingAddr.SelectedValue = Convert.ToString(viewStateValue);
                    plhShipping.Visible = viewStateChecked;
                    chkShippingAddr.Checked = viewStateChecked;
                    return;
                }
                catch
                {
                }
            }
            else if ((ShoppingCart.ShoppingCartShippingAddress != null) && (ShoppingCart.ShoppingCartShippingAddress.AddressID != ShoppingCart.ShoppingCartBillingAddress.AddressID))
            {
                try
                {
                    drpShippingAddr.SelectedValue = ShoppingCart.ShoppingCartShippingAddress.AddressID.ToString();
                    // Display Shipping part of the form and check checkbox
                    plhShipping.Visible = true;
                    chkShippingAddr.Checked = true;
                    return;
                }
                catch
                {
                }
            }
            // Select some address only if ShippingAddressID is diferent from BillingAddressID
            else if (lastShippingAddressId != 0)
            {
                try
                {
                    drpShippingAddr.SelectedValue = lastShippingAddressId.ToString();
                    if (drpShippingAddr.SelectedIndex != 0)
                    {
                        return;
                    }
                }
                catch
                {
                }
            }

            if (drpShippingAddr.Items.Count > 1)
            {
                drpShippingAddr.SelectedIndex = 1;
            }
        }
        catch
        {
        }
    }


    protected void LoadCompanySelectedValue()
    {
        try
        {
            drpCompanyAddress.ClearSelection();
            int lastCompanyAddressId = 0;

            // Get last modified company addresses
            var lastAddress = AddressInfoProvider.GetAddresses(mCustomerId, false, false, true, true).TopN(1).OrderByDescending("AddressLastModified");
            if (!DataHelper.DataSourceIsEmpty(lastAddress))
            {
                lastCompanyAddressId = lastAddress.FirstObject.AddressID;
            }

            // Try to select company address from ViewState first
            object viewStateValue = ShoppingCartControl.GetTempValue(COMPANY_ADDRESS_ID);
            bool viewStateChecked = ValidationHelper.GetBoolean(ShoppingCartControl.GetTempValue(COMPANY_ADDRESS_CHECKED), false);
            if ((viewStateValue != null))
            {
                try
                {
                    drpCompanyAddress.SelectedValue = Convert.ToString(viewStateValue);
                    plcCompanyDetail.Visible = viewStateChecked;
                    chkCompanyAddress.Checked = viewStateChecked;
                    return;
                }
                catch
                {
                }
            }
            else if ((ShoppingCart.ShoppingCartCompanyAddress != null) && (ShoppingCart.ShoppingCartCompanyAddress.AddressID != ShoppingCart.ShoppingCartBillingAddress.AddressID))
            {
                try
                {
                    drpCompanyAddress.SelectedValue = ShoppingCart.ShoppingCartCompanyAddress.AddressID.ToString();
                    // Display company address part of the form and check checkbox
                    plcCompanyDetail.Visible = true;
                    chkCompanyAddress.Checked = true;
                    return;
                }
                catch
                {
                }
            }
            // Select some address only if CompanyAddressID is diferent from BillingAddressID
            else if (lastCompanyAddressId != 0)
            {
                try
                {
                    drpCompanyAddress.SelectedValue = lastCompanyAddressId.ToString();
                    if (drpCompanyAddress.SelectedIndex != 0)
                    {
                        return;
                    }
                }
                catch
                {
                }
            }

            if (drpCompanyAddress.Items.Count > 1)
            {
                drpCompanyAddress.SelectedIndex = 1;
            }
        }
        catch
        {
        }
    }


    /// <summary>
    /// Change visibility of shipping address.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Parameter</param>
    protected void chkShippingAddr_CheckedChanged(object sender, EventArgs e)
    {
        if (plhShipping.Visible)
        {
            plhShipping.Visible = false;
        }
        else
        {
            plhShipping.Visible = true;
            LoadShippingAddressInfo();
        }
    }


    /// <summary>
    /// Change visibility of company address.
    /// </summary>
    /// <param name="sender">Sender</param>
    /// <param name="e">Parameter</param>
    protected void chkCompanyAddress_CheckedChanged(object sender, EventArgs e)
    {
        if (plcCompanyDetail.Visible)
        {
            plcCompanyDetail.Visible = false;
        }
        else
        {
            plcCompanyDetail.Visible = true;
            LoadCompanyAddressInfo();
        }
    }


    /// <summary>
    /// On drpBillingAddr selected index changed.
    /// </summary>
    private void drpBillingAddr_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateShippingAddresses();
        UpdateCompanyAddresses();

        LoadBillingAddressInfo();
    }


    /// <summary>
    /// On drpShippingAddr selected index changed.
    /// </summary>
    private void drpShippingAddr_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadShippingAddressInfo();
    }


    /// <summary>
    /// On drpCompanyAddress selected index changed.
    /// </summary>
    private void drpCompanyAddress_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCompanyAddressInfo();
    }


    /// <summary>
    /// Clean specified part of the form.
    /// </summary>    
    private void CleanForm(bool billing, bool shipping, bool company)
    {
        int defaultCountryId = 0;
        int defaultStateId = 0;

        // Prefill country from customer if any
        if ((ShoppingCart != null) && (ShoppingCart.Customer != null))
        {
            defaultCountryId = ShoppingCart.Customer.CustomerCountryID;
            defaultStateId = ShoppingCart.Customer.CustomerStateID;
        }

        // Prefill default store country if customers country not found
        if ((defaultCountryId <= 0) && (SiteContext.CurrentSite != null))
        {
            string countryName = ECommerceSettings.DefaultCountryName(SiteContext.CurrentSite.SiteName);
            CountryInfo ci = CountryInfoProvider.GetCountryInfo(countryName);
            defaultCountryId = (ci != null) ? ci.CountryID : 0;
            defaultStateId = 0;
        }

        if (shipping)
        {
            txtShippingName.Text = "";
            txtShippingAddr1.Text = "";
            txtShippingAddr2.Text = "";
            txtShippingCity.Text = "";
            txtShippingZip.Text = "";
            txtShippingPhone.Text = "";

            // Pre-load default country
            CountrySelector2.CountryID = defaultCountryId;
            CountrySelector2.StateID = defaultStateId;
            CountrySelector2.ReloadData(true);
        }
        if (billing)
        {
            txtBillingName.Text = "";
            txtBillingAddr1.Text = "";
            txtBillingAddr2.Text = "";
            txtBillingCity.Text = "";
            txtBillingZip.Text = "";
            txtBillingPhone.Text = "";

            // Pre-load default country
            CountrySelector1.CountryID = defaultCountryId;
            CountrySelector1.StateID = defaultStateId;
            CountrySelector1.ReloadData(true);
        }
        if (company)
        {
            txtCompanyName.Text = "";
            txtCompanyLine1.Text = "";
            txtCompanyLine2.Text = "";
            txtCompanyCity.Text = "";
            txtCompanyZip.Text = "";
            txtCompanyPhone.Text = "";

            // Pre-load default country
            CountrySelector3.CountryID = defaultCountryId;
            CountrySelector3.StateID = defaultStateId;
            CountrySelector3.ReloadData(true);
        }
    }


    /// <summary>
    /// Loads selected billing  address info.
    /// </summary>
    protected void LoadBillingAddressInfo()
    {
        // Try to select company address from ViewState first
        if (!ShoppingCartControl.IsCurrentStepPostBack && ShoppingCartControl.GetTempValue(BILLING_ADDRESS_ID) != null)
        {
            LoadBillingFromViewState();
        }
        else
        {
            int addressId = 0;

            if (drpBillingAddr.SelectedValue != "0")
            {
                addressId = Convert.ToInt32(drpBillingAddr.SelectedValue);

                AddressInfo ai = AddressInfoProvider.GetAddressInfo(addressId);
                if (ai != null)
                {
                    txtBillingName.Text = ai.AddressPersonalName;
                    txtBillingAddr1.Text = ai.AddressLine1;
                    txtBillingAddr2.Text = ai.AddressLine2;
                    txtBillingCity.Text = ai.AddressCity;
                    txtBillingZip.Text = ai.AddressZip;
                    txtBillingPhone.Text = ai.AddressPhone;
                    CountrySelector1.CountryID = ai.AddressCountryID;
                    CountrySelector1.StateID = ai.AddressStateID;
                    CountrySelector1.ReloadData(true);
                }
            }
            else
            {
                // Clean billing part of the form
                CleanForm(true, false, false);

                // Prefill customer company name or full name
                if ((ShoppingCart.Customer != null) &&
                    (ShoppingCart.Customer.CustomerCompany != ""))
                {
                    txtBillingName.Text = ShoppingCart.Customer.CustomerCompany;
                }
                else
                {
                    txtBillingName.Text = ShoppingCart.Customer.CustomerFirstName + " " + ShoppingCart.Customer.CustomerLastName;
                }
            }
        }
    }


    /// <summary>
    /// Loads selected shipping  address info.
    /// </summary>
    protected void LoadShippingAddressInfo()
    {
        int addressId = 0;

        // Load shipping info only if shipping part is visible
        if (plhShipping.Visible)
        {
            // Try to select company address from ViewState first
            if (!ShoppingCartControl.IsCurrentStepPostBack && ShoppingCartControl.GetTempValue(SHIPPING_ADDRESS_ID) != null)
            {
                LoadShippingFromViewState();
            }
            else
            {
                if (drpShippingAddr.SelectedValue != "0")
                {
                    addressId = Convert.ToInt32(drpShippingAddr.SelectedValue);

                    AddressInfo ai = AddressInfoProvider.GetAddressInfo(addressId);
                    if (ai != null)
                    {
                        txtShippingName.Text = ai.AddressPersonalName;
                        txtShippingAddr1.Text = ai.AddressLine1;
                        txtShippingAddr2.Text = ai.AddressLine2;
                        txtShippingCity.Text = ai.AddressCity;
                        txtShippingZip.Text = ai.AddressZip;
                        txtShippingPhone.Text = ai.AddressPhone;
                        CountrySelector2.CountryID = ai.AddressCountryID;
                        CountrySelector2.StateID = ai.AddressStateID;
                        CountrySelector2.ReloadData(true);
                    }
                }
                else
                {
                    // clean shipping part of the form
                    CleanForm(false, true, false);

                    // Prefill customer company name or full name
                    if ((ShoppingCart.Customer != null) &&
                        (ShoppingCart.Customer.CustomerCompany != ""))
                    {
                        txtShippingName.Text = ShoppingCart.Customer.CustomerCompany;
                    }
                    else
                    {
                        txtShippingName.Text = ShoppingCart.Customer.CustomerFirstName + " " + ShoppingCart.Customer.CustomerLastName;
                    }
                }
            }
        }
    }


    /// <summary>
    /// Loads selected company address info.
    /// </summary>
    protected void LoadCompanyAddressInfo()
    {
        int addressId = 0;

        // Load company address info only if company part is visible
        if (plcCompanyDetail.Visible)
        {
            // Try to select company address from ViewState first
            if (!ShoppingCartControl.IsCurrentStepPostBack && ShoppingCartControl.GetTempValue(COMPANY_ADDRESS_ID) != null)
            {
                LoadCompanyFromViewState();
            }
            else
            {
                if (drpCompanyAddress.SelectedValue != "0")
                {
                    addressId = Convert.ToInt32(drpCompanyAddress.SelectedValue);

                    AddressInfo ai = AddressInfoProvider.GetAddressInfo(addressId);
                    if (ai != null)
                    {
                        txtCompanyName.Text = ai.AddressPersonalName;
                        txtCompanyLine1.Text = ai.AddressLine1;
                        txtCompanyLine2.Text = ai.AddressLine2;
                        txtCompanyCity.Text = ai.AddressCity;
                        txtCompanyZip.Text = ai.AddressZip;
                        txtCompanyPhone.Text = ai.AddressPhone;
                        CountrySelector3.CountryID = ai.AddressCountryID;
                        CountrySelector3.StateID = ai.AddressStateID;
                        CountrySelector3.ReloadData(true);
                    }
                }
                else
                {
                    // clean shipping part of the form
                    CleanForm(false, false, true);

                    // Prefill customer company name or full name
                    if ((ShoppingCart.Customer != null) &&
                        (ShoppingCart.Customer.CustomerCompany != ""))
                    {
                        txtCompanyName.Text = ShoppingCart.Customer.CustomerCompany;
                    }
                    else
                    {
                        txtCompanyName.Text = ShoppingCart.Customer.CustomerFirstName + " " + ShoppingCart.Customer.CustomerLastName;
                    }
                }
            }
        }
    }


    /// <summary>
    /// Check if the form is well filled.
    /// </summary>
    /// <returns>True or false.</returns>
    public override bool IsValid()
    {
        Validator val = new Validator();
        // check billing part of the form
        string result = val.NotEmpty(txtBillingName.Text.Trim(), GetString("ShoppingCartOrderAddresses.BillingNameErr"))
                           .NotEmpty(txtBillingAddr1.Text.Trim(), GetString("ShoppingCartOrderAddresses.BillingAddressLineErr"))
                           .NotEmpty(txtBillingCity.Text.Trim(), GetString("ShoppingCartOrderAddresses.BillingCityErr"))
                           .NotEmpty(txtBillingZip.Text.Trim(), GetString("ShoppingCartOrderAddresses.BillingZIPErr")).Result;

        if (result == "")
        {
            // Check shipping address
            if (chkShippingAddr.Checked)
            {
                // check shipping part of the form if checkbox is checked
                result = val.NotEmpty(txtShippingName.Text.Trim(), GetString("ShoppingCartOrderAddresses.ShippingNameErr"))
                            .NotEmpty(txtShippingAddr1.Text.Trim(), GetString("ShoppingCartOrderAddresses.ShippingAddressLineErr"))
                            .NotEmpty(txtShippingCity.Text.Trim(), GetString("ShoppingCartOrderAddresses.ShippingCityErr"))
                            .NotEmpty(txtShippingZip.Text.Trim(), GetString("ShoppingCartOrderAddresses.ShippingZIPErr")).Result;
            }
            // Check company address
            if ((result == "") && (chkCompanyAddress.Checked))
            {
                // check company part of the form if checkbox is checked
                result = val.NotEmpty(txtCompanyName.Text.Trim(), GetString("ShoppingCartOrderAddresses.CompanyNameErr"))
                            .NotEmpty(txtCompanyLine1.Text.Trim(), GetString("ShoppingCartOrderAddresses.CompanyAddressLineErr"))
                            .NotEmpty(txtCompanyCity.Text.Trim(), GetString("ShoppingCartOrderAddresses.CompanyCityErr"))
                            .NotEmpty(txtCompanyZip.Text.Trim(), GetString("ShoppingCartOrderAddresses.CompanyZIPErr")).Result;
            }
        }

        if (result != "")
        {
            // display error
            lblError.Visible = true;
            lblError.Text = result;

            return false;
        }

        return true;
    }


    /// <summary>
    /// Process valid values of this step.
    /// </summary>
    public override bool ProcessStep()
    {
        AddressInfo ai = null;
        bool newAddress = false;
        if (mCustomerId > 0)
        {
            // Clean the viewstate
            RemoveBillingTempValues();
            RemoveShippingTempValues();
            RemoveCompanyTempValues();

            // Check country presence
            if (CountrySelector1.CountryID <= 0)
            {
                lblError.Visible = true;
                lblError.Text = GetString("countryselector.selectedcountryerr");
                return false;
            }

            if (!CountrySelector1.StateSelectionIsValid)
            {
                lblError.Visible = true;
                lblError.Text = GetString("countryselector.selectedstateerr");
                return false;
            }

            // Process billing address
            //------------------------
            ai = AddressInfoProvider.GetAddressInfo(ValidationHelper.GetInteger(drpBillingAddr.SelectedValue, 0));
            if (ai == null)
            {
                ai = new AddressInfo();
                newAddress = true;
            }

            ai.AddressPersonalName = txtBillingName.Text.Trim();
            ai.AddressLine1 = txtBillingAddr1.Text.Trim();
            ai.AddressLine2 = txtBillingAddr2.Text.Trim();
            ai.AddressCity = txtBillingCity.Text.Trim();
            ai.AddressZip = txtBillingZip.Text.Trim();
            ai.AddressCountryID = CountrySelector1.CountryID;
            ai.AddressStateID = CountrySelector1.StateID;
            ai.AddressPhone = txtBillingPhone.Text.Trim();
            if (newAddress)
            {
                ai.AddressIsBilling = true;
                ai.AddressIsShipping = !chkShippingAddr.Checked;
                ai.AddressIsCompany = !chkCompanyAddress.Checked;
                ai.AddressEnabled = true;
            }
            ai.AddressCustomerID = mCustomerId;
            ai.AddressName = AddressInfoProvider.GetAddressName(ai);

            // Save address and set it's ID to ShoppingCartInfoObj
            AddressInfoProvider.SetAddressInfo(ai);

            // Update current contact's address
            MapContactAddress(ai);

            ShoppingCart.ShoppingCartBillingAddress = ai;
            
            // If shopping cart does not need shipping
            if (!ShoppingCart.IsShippingNeeded)
            {
                ShoppingCart.ShoppingCartShippingAddress = null;
            }
            // If shipping address is different from billing address
            else if (chkShippingAddr.Checked)
            {
                // Check country presence
                if (CountrySelector2.CountryID <= 0)
                {
                    lblError.Visible = true;
                    lblError.Text = GetString("countryselector.selectedcountryerr");
                    return false;
                }

                if (!CountrySelector2.StateSelectionIsValid)
                {
                    lblError.Visible = true;
                    lblError.Text = GetString("countryselector.selectedstateerr");
                    return false;
                }

                newAddress = false;
                // Process shipping address
                //-------------------------
                ai = AddressInfoProvider.GetAddressInfo(Convert.ToInt32(drpShippingAddr.SelectedValue));
                if (ai == null)
                {
                    ai = new AddressInfo();
                    newAddress = true;
                }

                ai.AddressPersonalName = txtShippingName.Text.Trim();
                ai.AddressLine1 = txtShippingAddr1.Text.Trim();
                ai.AddressLine2 = txtShippingAddr2.Text.Trim();
                ai.AddressCity = txtShippingCity.Text.Trim();
                ai.AddressZip = txtShippingZip.Text.Trim();
                ai.AddressCountryID = CountrySelector2.CountryID;
                ai.AddressStateID = CountrySelector2.StateID;
                ai.AddressPhone = txtShippingPhone.Text.Trim();
                if (newAddress)
                {
                    ai.AddressIsShipping = true;
                    ai.AddressEnabled = true;
                    ai.AddressIsBilling = false;
                    ai.AddressIsCompany = false;
                    ai.AddressEnabled = true;
                }
                ai.AddressCustomerID = mCustomerId;
                ai.AddressName = AddressInfoProvider.GetAddressName(ai);

                // Save address and set it's ID to ShoppingCartInfoObj
                AddressInfoProvider.SetAddressInfo(ai);
                ShoppingCart.ShoppingCartShippingAddress = ai;
            }
            // Shipping address is the same as billing address
            else
            {
                ShoppingCart.ShoppingCartShippingAddress = ShoppingCart.ShoppingCartBillingAddress;
            }

            if (chkCompanyAddress.Checked)
            {
                // Check country presence
                if (CountrySelector3.CountryID <= 0)
                {
                    lblError.Visible = true;
                    lblError.Text = GetString("countryselector.selectedcountryerr");
                    return false;
                }

                if (!CountrySelector3.StateSelectionIsValid)
                {
                    lblError.Visible = true;
                    lblError.Text = GetString("countryselector.selectedstateerr");
                    return false;
                }

                newAddress = false;
                // Process company address
                //-------------------------
                ai = AddressInfoProvider.GetAddressInfo(Convert.ToInt32(drpCompanyAddress.SelectedValue));
                if (ai == null)
                {
                    ai = new AddressInfo();
                    newAddress = true;
                }

                ai.AddressPersonalName = txtCompanyName.Text.Trim();
                ai.AddressLine1 = txtCompanyLine1.Text.Trim();
                ai.AddressLine2 = txtCompanyLine2.Text.Trim();
                ai.AddressCity = txtCompanyCity.Text.Trim();
                ai.AddressZip = txtCompanyZip.Text.Trim();
                ai.AddressCountryID = CountrySelector3.CountryID;
                ai.AddressStateID = CountrySelector3.StateID;
                ai.AddressPhone = txtCompanyPhone.Text.Trim();
                if (newAddress)
                {
                    ai.AddressIsCompany = true;
                    ai.AddressIsBilling = false;
                    ai.AddressIsShipping = false;
                    ai.AddressEnabled = true;
                }
                ai.AddressCustomerID = mCustomerId;
                ai.AddressName = AddressInfoProvider.GetAddressName(ai);

                // Save address and set it's ID to ShoppingCartInfoObj
                AddressInfoProvider.SetAddressInfo(ai);
                ShoppingCart.ShoppingCartCompanyAddress = ai;
            }
            // Company address is the same as billing address
            else
            {
                // Save information about company address or not (according to the site settings)
                if (ECommerceSettings.UseExtraCompanyAddress(mCurrentSite.SiteName))
                {
                    ShoppingCart.ShoppingCartCompanyAddress = ShoppingCart.ShoppingCartBillingAddress;
                }
                else
                {
                    ShoppingCart.ShoppingCartCompanyAddress = null;
                }
            }

            try
            {
                ShoppingCart.InvalidateCalculations();

                // Update changes in database only when on the live site
                if (!ShoppingCartControl.IsInternalOrder)
                {
                    ShoppingCartInfoProvider.SetShoppingCartInfo(ShoppingCart);
                }
                return true;
            }
            catch (Exception ex)
            {
                // Show error message
                lblError.Visible = true;
                lblError.Text = ex.Message;
                return false;
            }
        }
        else
        {
            lblError.Visible = true;
            lblError.Text = GetString("Ecommerce.NoCustomerSelected");
            return false;
        }
    }


    protected override void Render(HtmlTextWriter writer)
    {
        if (!ShoppingCartControl.IsCurrentStepPostBack)
        {
            // Load values
            LoadShippingSelectedValue();
            LoadBillingSelectedValue();
            LoadCompanySelectedValue();
        }
        base.Render(writer);
    }


    private void UpdateShippingAddresses()
    {
        string oldSelectedValue = drpShippingAddr.SelectedValue;

        drpShippingAddr.ClearSelection();
        drpShippingAddr.SelectedIndex = -1;

        // Add new item <(new), 0>
        ListItem li = new ListItem(GetString("ShoppingCartOrderAddresses.NewAddress"), "0");

        DataSet ds = AddressInfoProvider.GetAddresses(mCustomerId, false, true, false, true);
        drpShippingAddr.DataSource = ds;
        drpShippingAddr.DataBind();
        drpShippingAddr.Items.Insert(0, li);
        // Try to remove selected biling address
        try
        {
            // Do not remove (new) item
            if (drpBillingAddr.SelectedIndex != 0)
            {
                drpShippingAddr.Items.Remove(drpBillingAddr.SelectedItem);
            }

            drpShippingAddr.SelectedValue = oldSelectedValue;
        }
        catch
        {
            LoadShippingSelectedValue();
            LoadShippingAddressInfo();
        }

        if (drpShippingAddr.SelectedIndex != 0)
        {
            LoadShippingSelectedValue();
            LoadShippingAddressInfo();
        }
    }


    private void UpdateCompanyAddresses()
    {
        string oldSelectedValue = drpCompanyAddress.SelectedValue;

        drpCompanyAddress.ClearSelection();
        drpCompanyAddress.SelectedIndex = -1;

        // Add new item <(new), 0>
        ListItem li = new ListItem(GetString("ShoppingCartOrderAddresses.NewAddress"), "0");

        DataSet ds = AddressInfoProvider.GetAddresses(mCustomerId, false, false, true, true);
        drpCompanyAddress.DataSource = ds;
        drpCompanyAddress.DataBind();
        drpCompanyAddress.Items.Insert(0, li);
        // Try to remove selected biling address
        try
        {
            // Do not remove (new) item
            if (drpBillingAddr.SelectedIndex != 0)
            {
                drpCompanyAddress.Items.Remove(drpBillingAddr.SelectedItem);
            }

            drpCompanyAddress.SelectedValue = oldSelectedValue;
        }
        catch
        {
            LoadCompanySelectedValue();
            LoadCompanyAddressInfo();
        }

        if (drpCompanyAddress.SelectedIndex != 0)
        {
            LoadCompanySelectedValue();
            LoadCompanyAddressInfo();
        }
    }


    /// <summary>
    /// Updates contact's address.
    /// </summary>
    /// <param name="addressInfo">Billing address</param>
    private void MapContactAddress(AddressInfo addressInfo)
    {
        try
        {
            if ((addressInfo == null) || !SettingsKeyInfoProvider.GetBoolValue(SiteContext.CurrentSiteName + ".CMSEnableOnlineMarketing"))
            {
                return;
            }

            GeneralizedInfo contactInfo = BaseAbstractInfoProvider.GetInfoById(PredefinedObjectType.CONTACT, ContactID);

            // Check that current contact has not yet filled in address
            if ((contactInfo != null) && String.IsNullOrEmpty(ValidationHelper.GetString(contactInfo.GetValue("ContactAddress1"), "")))
            {
                Func<int, int?> getIntIfValid = i => i > 0 ? i : (int?)null;

                contactInfo.SetValue("ContactAddress1", addressInfo.AddressLine1);
                contactInfo.SetValue("ContactAddress2", addressInfo.AddressLine2);
                contactInfo.SetValue("ContactCity", addressInfo.AddressCity);
                contactInfo.SetValue("ContactZIP", addressInfo.AddressZip);
                contactInfo.SetValue("ContactMobilePhone", addressInfo.AddressPhone);
                contactInfo.SetValue("ContactCountryID", getIntIfValid(addressInfo.AddressCountryID));
                contactInfo.SetValue("ContactStateID", getIntIfValid(addressInfo.AddressStateID));
                contactInfo.SetObject();
            }
        }
        catch (Exception ex)
        {
            // Exception could happen when max length of contact parameters is exceeded
            EventLogProvider.LogException("ShoppingCartOrderAddresses.MapContactAddress", "UPDATECONTACT", ex);
        }
    }
}