<%@ Assembly Name="PGDCustomListForms1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=1bb4cd64bf72ca34" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" CodeBehind="customDispForm.aspx.cs" Inherits="PGDCustomListForms1.Layouts.PGDCustomListForms1.customDispForm" MasterPageFile="~masterurl/default.master" %>


<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<asp:Content ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    <SharePoint:ListFormPageTitle runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
    <span class="die">
        <SharePoint:ListProperty Property="LinkTitle" runat="server" ID="ID_LinkTitle" />
    </span>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderPageImage" runat="server">
    <img src="/_layouts/15/images/blank.gif?rev=23" width='1' height='1' alt="" />
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <SharePoint:UIVersionedContent UIVersion="4" runat="server">
        <contenttemplate>
	<div style="padding-left:5px">
	</contenttemplate>
    </SharePoint:UIVersionedContent>
    <table class="ms-core-tableNoSpace" id="onetIDListForm">
        <tr>
            <td>
                <h1>My Custom Display Form</h1>
                <WebPartPages:WebPartZone runat="server" FrameType="None" ID="Main" Title="loc:Main">
                    <ZoneTemplate>
                    </ZoneTemplate>
                </WebPartPages:WebPartZone>
                <table border="0" width="100%">
                    <tr>
                        <td class="ms-toolbar" nowrap="nowrap">
                            <table>
                                <tr>
                                    <td width="99%" class="ms-toolbar" nowrap="nowrap">
                                        <img src="/_layouts/15/images/blank.gif" width="1" height="18" /></td>
                                    <td class="ms-toolbar" nowrap="nowrap" align="right">
                                        <SharePoint:GoBackButton runat="server" ControlMode="Display" ID="gobackbutton1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="ms-toolbar" nowrap="nowrap">
                            <SharePoint:FormToolBar runat="server" ControlMode="Display" />
                            <SharePoint:ItemValidationFailedMessage runat="server" ControlMode="Display" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="tabs">

  <ul>

    <li><a href="#tabs-1">Title and Priority</a></li>

    <li><a href="#tabs-2">Dates and comments</a></li>

    <li><a href="#tabs-3">Help</a></li>

  </ul>

  <div id="tabs-1">

    <table border="0" cellspacing="0" width="100%">
                                <tr>
                                    <td width="190px" valign="top" class="ms-formlabel">
                                        <h3 class="ms-standardheader">
                                            <nobr>Title</nobr>
                                        </h3>
                                    </td>
                                    <td width="400px" valign="top" class="ms-formbody">
                                        <SharePoint:FormField runat="server" ID="field_Title" ControlMode="Display" FieldName="Title" />
                                        <SharePoint:FieldDescription runat="server" ID="field_Title_Description" FieldName="Title"
                                            ControlMode="Display" />

                                    </td>
                                </tr>
                                <tr>
                                    <td width="190px" valign="top" class="ms-formlabel">
                                        <h3 class="ms-standardheader">
                                            <nobr>Priority</nobr>
                                        </h3>
                                    </td>
                                    <td width="400px" valign="top" class="ms-formbody">
                                        <SharePoint:FormField runat="server" ID="field_Priority" ControlMode="Display" FieldName="Priority" />
                                        <SharePoint:FieldDescription runat="server" ID="FieldDescription1" FieldName="Priority"
                                            ControlMode="Display" />

                                    </td>
                                </tr>
                                </table>
  </div>

  <div id="tabs-2">

                                <table border="0" cellspacing="0" width="100%">
                                <tr>
                                    <td width="190px" valign="top" class="ms-formlabel">
                                        <h3 class="ms-standardheader">
                                            <nobr>Start Date</nobr>
                                        </h3>
                                    </td>
                                    <td width="400px" valign="top" class="ms-formbody">
                                        <SharePoint:FormField runat="server" ID="FormField1" ControlMode="Display" FieldName="StartDate" />
                                        <SharePoint:FieldDescription runat="server" ID="FieldDescription2" FieldName="StartDate"
                                            ControlMode="Display" />

                                    </td>
                                </tr>
                                <tr>
                                    <td width="190px" valign="top" class="ms-formlabel">
                                        <h3 class="ms-standardheader">
                                            <nobr>End Date</nobr>
                                        </h3>
                                    </td>
                                    <td width="400px" valign="top" class="ms-formbody">
                                        <SharePoint:FormField runat="server" ID="FormField2" ControlMode="Display" FieldName="_EndDate" />
                                        <SharePoint:FieldDescription runat="server" ID="FieldDescription3" FieldName="_EndDate"
                                            ControlMode="Display" />
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td width="190px" valign="top" class="ms-formlabel">
                                        <h3 class="ms-standardheader">
                                            <nobr>Comments</nobr>
                                        </h3>
                                    </td>
                                    <td width="400px" valign="top" class="ms-formbody">
                                                                                <SharePoint:FormField runat="server" ID="FormField3" ControlMode="Display" FieldName="_Comments" />
                                        <SharePoint:FieldDescription runat="server" ID="FieldDescription4" FieldName="_Comments"
                                            ControlMode="Display" />
                                        
                                    </td>
                                </tr>
                                <tr id="idAttachmentsRow">
                                    <td nowrap="true" valign="top" class="ms-formlabel" width="20%">
                                        <SharePoint:FieldLabel ControlMode="Display" FieldName="Attachments" runat="server" />
                                    </td>
                                    <td valign="top" class="ms-formbody" width="80%">
                                        <SharePoint:FormField runat="server" ID="AttachmentsField" ControlMode="Display" FieldName="Attachments"  />
                                        <script>
                                            var elm = document.getElementById("idAttachmentsTable");
                                            if (elm == null || elm.rows.length == 0)
                                                document.getElementById("idAttachmentsRow").style.display = 'none';
                                        </script>
                                    </td>
                                </tr>
                                
                            </table>
  </div>

  <div id="tabs-3">

    <p>Mauris eleifend est et turpis. Duis id erat. Suspendisse potenti. Aliquam vulputate, pede vel vehicula accumsan, mi neque rutrum erat, eu congue orci lorem eget lorem. Vestibulum non ante. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Fusce sodales. Quisque eu urna vel enim commodo pellentesque. Praesent eu risus hendrerit ligula tempus pretium. Curabitur lorem enim, pretium nec, feugiat nec, luctus a, lacus.</p>

    <p>Duis cursus. Maecenas ligula eros, blandit nec, pharetra at, semper at, magna. Nullam ac lacus. Nulla facilisi. Praesent viverra justo vitae neque. Praesent blandit adipiscing velit. Suspendisse potenti. Donec mattis, pede vel pharetra blandit, magna ligula faucibus eros, id euismod lacus dolor eget odio. Nam scelerisque. Donec non libero sed nulla mattis commodo. Ut sagittis. Donec nisi lectus, feugiat porttitor, tempor ac, tempor vitae, pede. Aenean vehicula velit eu tellus interdum rutrum. Maecenas commodo. Pellentesque nec elit. Fusce in lacus. Vivamus a libero vitae lectus hendrerit hendrerit.</p>

  </div>

</div>



                        </td>
                    </tr>

                    <tr>
                        <td class="ms-toolbar" nowrap="nowrap">
                            <table>
                                <tr>
                                    <td class="ms-descriptiontext" nowrap="nowrap">
                                        <SharePoint:CreatedModifiedInfo ControlMode="Display" runat="server" />
                                    </td>
                                    <td width="99%" class="ms-toolbar" nowrap="nowrap">
                                        <img src="/_layouts/15/images/blank.gif" width="1" height="18" /></td>
                                    <td class="ms-toolbar" nowrap="nowrap" align="right">
                                        <SharePoint:GoBackButton runat="server" ControlMode="Display" ID="gobackbutton2" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                </table>

            </td>
        </tr>
    </table>
    <SharePoint:UIVersionedContent UIVersion="4" runat="server">
        <contenttemplate>
	</div>
	</contenttemplate>
    </SharePoint:UIVersionedContent>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <SharePoint:DelegateControl runat="server" ControlId="FormCustomRedirectControl" AllowMultipleControls="true" />
    <SharePoint:UIVersionedContent UIVersion="4" runat="server">
        <contenttemplate>
		<SharePoint:CssRegistration Name="forms.css" runat="server"/>
	</contenttemplate>
    </SharePoint:UIVersionedContent>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css">

  <script src="//code.jquery.com/jquery-1.10.2.js" type="text/javascript"></script>

  <script src="//code.jquery.com/ui/1.11.1/jquery-ui.js" type="text/javascript"></script>

  <%--<link rel="stylesheet" href="/resources/demos/style.css">--%>

  <script>
      var $j = jQuery.noConflict(true);
      (function ($) {
      $(function () {

          $("#tabs").tabs();
          return false;

      });
      }($j));
  </script>


</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderTitleLeftBorder" runat="server">
    <table cellpadding="0" height="100%" width="100%" cellspacing="0">
        <tr>
            <td class="ms-areaseparatorleft">
                <img src="/_layouts/15/images/blank.gif?rev=23" width='1' height='1' alt="" /></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderTitleAreaClass" runat="server">
    <script type="text/javascript" id="onetidPageTitleAreaFrameScript">
        if (document.getElementById("onetidPageTitleAreaFrame") != null) {
            document.getElementById("onetidPageTitleAreaFrame").className = "ms-areaseparator";
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderBodyAreaClass" runat="server">
    <SharePoint:StyleBlock runat="server">
        .ms-bodyareaframe {
	padding: 8px;
	border: none;
}
    </SharePoint:StyleBlock>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderBodyLeftBorder" runat="server">
    <div class='ms-areaseparatorleft'>
        <img src="/_layouts/15/images/blank.gif?rev=23" width='8' height='100%' alt="" /></div>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderTitleRightMargin" runat="server">
    <div class='ms-areaseparatorright'>
        <img src="/_layouts/15/images/blank.gif?rev=23" width='8' height='100%' alt="" /></div>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderBodyRightMargin" runat="server">
    <div class='ms-areaseparatorright'>
        <img src="/_layouts/15/images/blank.gif?rev=23" width='8' height='100%' alt="" /></div>
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderTitleAreaSeparator" runat="server" />

