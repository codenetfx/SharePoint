using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint;
using System.Web.UI;

namespace AcmeCorp.Engagements.FarmConfiguration
{
    public class PBSPageBase : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            SPUtility.EnsureSessionCredentials(SPSessionCredentialsFlags.RequireAuthentication);

            if (!SPContext.Current.Web.DoesUserHavePermissions(Microsoft.SharePoint.SPBasePermissions.ManageWeb))
            {
                SPUtility.Redirect(SPUtility.AccessDeniedPage + "?Source=" + SPHttpUtility.UrlKeyValueEncode(SPContext.Current.Web.Site.MakeFullUrl(Request.RawUrl)), SPRedirectFlags.RelativeToLayoutsPage, HttpContext.Current);
            }
            bool featureActivated = FeatureActivated();
            if (!featureActivated)
            {
                SPUtility.Redirect(SPUtility.ErrorLinkNavigateUrl + "?Source=" + SPHttpUtility.UrlKeyValueEncode(SPContext.Current.Web.Site.MakeFullUrl(Request.RawUrl)), SPRedirectFlags.RelativeToLayoutsPage, HttpContext.Current);
            }

          

        }

        private bool FeatureActivated()
        {

            Guid featureGuid = new Guid("3e5dc8a8-d787-4881-83b0-ec2a8e79652e");
            bool found = false;
            //foreach (SPFeatureDefinitionScope feature in SPContext.Current.Site.WebApplication.Farm.FeatureDefinitions)
            //{

            //    if (feature.DefinitionId == featureGuid)
            //    {
            //        found = true;
            //        break;
            //    }
            //}
            return true;// found;
        }

       


    }
}