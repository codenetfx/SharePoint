// -----------------------------------------------------------------------
// <copyright file="ProjectDataWebPart.cs" company="AcmeCorp">
// AcmeCorp
// </copyright>
// -----------------------------------------------------------------------
namespace AcmeCorp.Engagements.ProjectDataWebPart
{  
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.WebControls.WebParts;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.WebControls;
    using Acme.Core;
    using AcmeCorp.Engagements.UiHelpers.ApiFactory;

    /// <summary>
    /// Project Data web part
    /// </summary>
    [ToolboxItemAttribute(false)]
    public class ProjectDataWebPart : WebPart
    {
        /// <summary>
        /// The property items
        /// </summary>
        private string propertyItems = string.Empty;

        /// <summary>
        /// The property item collection
        /// </summary>
        private string[] propertyItemCollection;

        /// <summary>
        /// The error message
        /// </summary>
        private Label errorMessage = new Label();

        /// <summary>
        /// The property bag items with custom properties
        /// </summary>
        private List<CustomProperty> propertyBagItems = new List<CustomProperty>();

        /// <summary>
        /// Gets or sets the property items.
        /// </summary>
        /// <value>
        /// Semicolon delimited list of custom web properties to be displayed
        /// </value>
        [Personalizable(PersonalizationScope.Shared), WebBrowsable(true),
WebDisplayName("Web Properties"),
WebDescription("Semicollon delimited list of custom web properties to be displayed")]
        public string PropertyItems
        {
            get
            {
                return this.propertyItems;
            }

            set
            {
                this.propertyItems = value;
            }
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            try
            {
                AcmeCorp.Engagements.EngagementsApi.Api api = new ApiFactory(SPContext.Current.Site.WebApplication.Farm, SPContext.Current.Web).Api;

                this.LoadPropertyKeys();
                this.PopulatePropertyValues();
                Table tbl = new Table();

                string lang = System.Threading.Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpper();


                foreach (CustomProperty item in this.propertyBagItems)
                {
                    string propertyValue = string.Empty;
                    propertyValue = this.FormatItemValue(item.PropertyValue);

                    TableRow tr = new TableRow();
                    TableCell td = new TableCell();
                    td.Text = api.Localization.GetValue(item.PropertyKey.ToString(), lang);
                    tr.Cells.Add(td);
                    TableCell td2 = new TableCell();
                    td2.Text = propertyValue;
                    tr.Cells.Add(td2);
                    tbl.Rows.Add(tr);
                }

                this.Controls.Add(tbl);
            }
            catch (Exception ex)
            {
                this.errorMessage.Text += ex.Message;
            }

            this.Controls.Add(this.errorMessage);
        }

        /// <summary>
        /// Format value of property based on its data type
        /// </summary>
        /// <param name="itemValue">object itemValue</param>
        /// <returns>localized string</returns>
        private string FormatItemValue(object itemValue)
        {
            string returnValue = string.Empty;
            Type dataType = itemValue.GetType();
            if (dataType == typeof(System.Int32))
            {
                returnValue = Convert.ToInt32(itemValue, SPContext.Current.Web.Locale).ToString();
            }
            else if (dataType == typeof(System.DateTime))
            {
                returnValue = Convert.ToDateTime(itemValue, SPContext.Current.Web.Locale).ToString();
            }
            else
            {
                returnValue = itemValue.ToString();
            }

            return returnValue;
        }

        /// <summary>
        /// Parse value of custom property and return string array
        /// </summary>
        private void LoadPropertyKeys()
        {
            if (this.propertyItems != string.Empty)
            {
                this.propertyItemCollection = this.propertyItems.Trim().Split(';');
            }
        }

        /// <summary>
        /// Read property bag of SharePoint Web and populate list of values to be displayed in web part
        /// </summary>
        private void PopulatePropertyValues()
        {
            string propertyItem = string.Empty;
            if (this.propertyItemCollection != null && this.propertyItemCollection.Length != 0)
            {
                SPWeb targetWeb = SPContext.Current.Web;
                for (int i = 0; i < this.propertyItemCollection.Length; i++)
                {
                    propertyItem = this.propertyItemCollection[i].Trim();

                    if (propertyItem != null && propertyItem != string.Empty)
                    {
                        if (targetWeb.AllProperties[propertyItem] != null)
                        {
                            this.propertyBagItems.Add(new CustomProperty
                            {
                                PropertyKey = propertyItem,
                                PropertyValue = targetWeb.AllProperties[propertyItem]
                            });
                        }
                        else
                        {
                            this.propertyBagItems.Add(new CustomProperty { PropertyKey = propertyItem, PropertyValue = string.Empty });
                        }
                    }
                }
            }
        }
    }
}
