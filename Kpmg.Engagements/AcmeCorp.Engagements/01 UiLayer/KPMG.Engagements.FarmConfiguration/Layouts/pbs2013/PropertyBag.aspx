<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PropertyBag.aspx.cs" Inherits="AcmeCorp.Engagements.FarmConfiguration.PropertyBag" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
<script type="text/javascript" src="/_layouts/15/blank.js?rev=ZaOXZEobVwykPO9g8hq%2F8A%3D%3D"></script>
<script type="text/javascript" src="/ScriptResource.axd?d=MwU2BqDuBF6PnXea3yF8NbfaoZGZuhaNmq04O3gskCwUTswJFo4K8dG_4OKTdcCXx9fTxMe5hfy8HmUxduC75B5rQIDdLtWw1S9i80tpfgetn0INlbMMhu2Dpi7fSkyaT0q3r0sLcr7BcaU-Mv-MCQB8iXCHrDYIOInDEDeHv3jOUQVSokJ1bgCSgRhyPlHw0&amp;t=6119e399"></script>
<script type="text/javascript" src="/_layouts/15/1033/initstrings.js?rev=uNmvBiHdrBzcPQzXRpm%2FnQ%3D%3D"></script>
<script type="text/javascript" src="/_layouts/15/1033/strings.js?rev=cSu1pcWiRc999fyCNzJplg%3D%3D"></script>
<script type="text/javascript" src="/_layouts/15/sp.init.js?rev=QI1yUCfCoUkadL93jNZLOg%3D%3D"></script>
<script type="text/javascript" src="/_layouts/15/ScriptResx.ashx?culture=en%2Dus&amp;name=SP%2ERes&amp;rev=yNk%2FhRzgBn40LJVP%2BqfgdQ%3D%3D"></script>
<script type="text/javascript" src="/_layouts/15/sp.ui.dialog.js?rev=0xf6wCIW4E1pN83I9nSIJQ%3D%3D"></script>
<script type="text/javascript" src="/_layouts/15/core.js?rev=%2FmcwmyWAFSbQRHlXU4BIBg%3D%3D"></script>
<script type="text/javascript" src="/_layouts/15/clienttemplates.js?rev=vtrc0n3sjgxKB4WQrCEeaA%3D%3D"></script>
<script type="text/javascript" src="/_layouts/15/sp.runtime.js?rev=5f2WkYJoaxlIRdwUeg4WEg%3D%3D"></script>
<script type="text/javascript" src="/_layouts/15/sp.js?rev=PaF2ZrI2GlTO1B5ru0gYug%3D%3D"></script>
<script type="text/javascript" src="/_layouts/15/sp.core.js?rev=movVYpCO4QLi%2BfD07czxdg%3D%3D"></script>
<script type="text/javascript" src="/_layouts/15/sp.ui.tileview.js?rev=HhoSvnBCZIUAf1ulZDPzPA%3D%3D"></script>
<script type="text/javascript" src="/_layouts/15/cui.js?rev=hkelvuz%2BDyOYNl9FmpxUvA%3D%3D"></script>
<script type="text/javascript" src="/_layouts/15/sp.storefront.js?rev=N760YHFo8g%2BMhQZFbPD2Tg%3D%3D"></script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <script type="text/javascript">
        //Dialog opening 
        function OpenDialog(url) {
            var options = SP.UI.$create_DialogOptions();
             
            options.width = 360;
            options.height = 260;
            options.url = "/_layouts/pbs2013/" + url;

            SP.UI.ModalDialog.showModalDialog(options);
        }

        var messageId;

        // Dialog callback 
        function CloseCallback(result, target) {
            if (result === SP.UI.DialogResult.OK) {
                //location.reload(true);
            }
            if (result === SP.UI.DialogResult.cancel) {
                // do nothing
            }
        }

    </script> 
    <hr style="height: 1px; width: 100%; color: Gray" />
    <div>
        <table>
            <tr>
                <td class="ms-descriptiontext ms-inputformdescription" style="width: 100px">
                    Site:
                </td>
                <td>
                    <span cssclass="ms-descriptiontext ms-inputformdescription">
                        <%=lblSite%></span>
                </td>
                <td>
                    <asp:Button ID="Button1" runat="server" Text="View Property Bag" class="ms-ButtonHeightWidth"
                        Style="width: 120px" OnClick="btnSite_Click" />
                </td>
            </tr>
            <tr>
                <td class="ms-descriptiontext ms-inputformdescription" style="width: 100px">
                    List:
                </td>
                <td>
                    <asp:DropDownList class="ms-long" ID="ddlFolder" runat="server">
                        <asp:ListItem Text="No selection" Value="No selection"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="btnFolder" runat="server" Text="View Property Bag" class="ms-ButtonHeightWidth"
                        Style="width: 120px" OnClick="btnFolder_Click" />
                </td>
            </tr>
        </table>
    </div>
    <hr style="height: 1px; width: 100%; color: Gray" />
    <asp:Label ID="Header" runat="server" Text=""></asp:Label>
    <asp:Label ID="PropertyTable" runat="server" Text=""></asp:Label>
    <hr style="height: 1px; width: 100%; color: Gray" />
    <div style="width: 100%; text-align: right">
        <small>Property Bag Settings by <a href="http://havivi.blogspot.com" target="_blank">
            Alon Havivi</a></small></div>
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Property Bag Settings
</asp:Content>
<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    Property Bag Settings
</asp:Content>