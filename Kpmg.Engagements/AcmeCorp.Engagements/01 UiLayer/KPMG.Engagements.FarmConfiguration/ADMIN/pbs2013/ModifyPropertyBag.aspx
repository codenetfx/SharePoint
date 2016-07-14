<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %> 
 

<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> 
<%@ Page Language="C#" DynamicMasterPageFile="~masterurl/default.master" CodeBehind="ModifyPropertyBag.aspx.cs" Inherits="AcmeCorp.Engagements.FarmConfiguration.ModifyPropertyBag"       %> 
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
 <%@ Import Namespace="Microsoft.SharePoint" %>
 <%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
 
<%@ Register Tagprefix="STSWC" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Meetings" Namespace="Microsoft.SharePoint.Meetings" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
 
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
 
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
 
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
  <div>
        <table cellpadding="0" cellspacing="0" style="height: 360; width: 100%">
      
            <tr>
                <td valign="top" style="height: 100%;">
                    <table cellpadding="5" cellspacing="5">
                        <tr>
                            <td>
                                <asp:Label ID="lblHeader" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td class="ms-authoringcontrols">
                                            <asp:Label ID="lblKey" runat="server" CssClass="ms-authoringcontrols" Text=" Key:"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtKey" runat="server" Columns="35" MaxLength="4000" CssClass="ms-input"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblValue" runat="server" Text=" Value:" CssClass="ms-authoringcontrols"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtValue" runat="server" Columns="35" MaxLength="4000" CssClass="ms-input"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblMsg"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>           
            <tr>
                <td valign="bottom" style="height: 10%">
                    <table id="Table2" class="ms-toolbar" width="100%" style="padding-left: 3px; padding-right: 5px;"
                        cellspacing="0" cellpadding="2">
                        <tr>
                            <td width="10%">
                                <table cellspacing="1" cellpadding="0" border="0">
                                    <tr>
                                        <td width="0" class="ms-toolbar">
                                            <a href="javascript:__doPostBack(&#39;ctl00$PlaceHolderMain$ProfileSave&#39;,&#39;&#39;)"
                                                title="Save and Close">
                                                <asp:ImageButton ID="btnSave2" OnClick="Save_Click" AlternateText="Save and Close"
                                                    ImageUrl="/_layouts/images/save.gif" BorderWidth="0" runat="server" /></a>
                                        </td>
                                        <td width="100%" nowrap>
                                            <asp:LinkButton ID="lnkSave2" OnClick="Save_Click" CssClass="ms-toolbar"
                                                runat="server" ToolTip="Save">Save</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="ms-separator" width="1%">
                                |
                            </td>
                            <td>
                                <table cellspacing="1" cellpadding="0" border="0">
                                    <tr>
                                        <td width="100%" nowrap>
                                            <a class="ms-toolbar" href="javascript:window.frameElement.commitPopup();">Close</a>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    

    <script language="javascript">
        function confirmModify(key, question) {
            var question = question + key;
            var answer = confirm(question);
            if (answer) {
                return true;
            }
            else if (!answer) {
                return false;
            }

        }

    </script>
</asp:Content>

<%@ Register TagPrefix="wssuc" TagName="TopNavBar" src="~/_controltemplates/15/TopNavBar.ascx" %>
<asp:Content contentplaceholderid="PlaceHolderTopNavBar" runat="server">
  <wssuc:TopNavBar id="IdTopNavBar" runat="server" Version="4" ShouldUseExtra="true"/>
</asp:Content>
<asp:Content contentplaceholderid="PlaceHolderHorizontalNav" runat="server"/>
<asp:Content contentplaceholderid="PlaceHolderSearchArea" runat="server"/>
<asp:Content contentplaceholderid="PlaceHolderTitleBreadcrumb" runat="server">
  <SharePoint:UIVersionedContent ID="UIVersionedContent1" UIVersion="3" runat="server"><ContentTemplate>
	<asp:SiteMapPath
		SiteMapProvider="SPXmlContentMapProvider"
		id="ContentMap"
		SkipLinkText=""
		NodeStyle-CssClass="ms-sitemapdirectional"
		RootNodeStyle-CssClass="s4-die"
		PathSeparator="&#160;&gt; "
		PathSeparatorStyle-CssClass = "s4-bcsep"
		runat="server" />
  </ContentTemplate></SharePoint:UIVersionedContent>
  <SharePoint:UIVersionedContent ID="UIVersionedContent2" UIVersion="4" runat="server"><ContentTemplate>
	<SharePoint:ListSiteMapPath
		runat="server"
		SiteMapProviders="SPSiteMapProvider,SPXmlContentMapProvider"
		RenderCurrentNodeAsLink="false"
		PathSeparator=""
		CssClass="s4-breadcrumb"
		NodeStyle-CssClass="s4-breadcrumbNode"
		CurrentNodeStyle-CssClass="s4-breadcrumbCurrentNode"
		RootNodeStyle-CssClass="s4-breadcrumbRootNode"
		HideInteriorRootNodes="true"
		SkipLinkText="" />
  </ContentTemplate></SharePoint:UIVersionedContent>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderPageTitleInTitleArea" runat="server">
<asp:SiteMapPath runat="server" ParentLevelsDisplayed="1" SiteMapProvider="SPXmlContentMapProvider"> <PATHSEPARATORTEMPLATE> 
    <SharePoint:ClusteredDirectionalSeparatorArrow ID="ClusteredDirectionalSeparatorArrow1" runat="server" /> </PATHSEPARATORTEMPLATE> </asp:SiteMapPath>
</asp:Content>
