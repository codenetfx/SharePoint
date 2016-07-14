<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportPropertyBag.aspx.cs" Inherits="AcmeCorp.Engagements.FarmConfiguration.ImportPropertyBag" DynamicMasterPageFile="~masterurl/default.master" %>
 

<%@ Assembly Name="Microsoft.SharePoint.Portal, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>
<%@ Assembly Name="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%>  
<%@ Import Namespace="Microsoft.SharePoint.WebControls" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Import Namespace="Microsoft.SharePoint" %>
 <%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="LinksTable" src="/_controltemplates/15/LinksTable.ascx" %>
 <%@ Register TagPrefix="wssuc" TagName="InputFormSection" src="/_controltemplates/15/InputFormSection.ascx" %> 
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" src="/_controltemplates/15/InputFormControl.ascx" %> 
<%@ Register TagPrefix="wssuc" TagName="LinkSection" src="/_controltemplates/15/LinkSection.ascx" %>
 <%@ Register TagPrefix="wssuc" TagName="ButtonSection" src="/_controltemplates/15/ButtonSection.ascx" %> 
<%@ Register TagPrefix="wssuc" TagName="ActionBar" src="/_controltemplates/15/ActionBar.ascx" %>
 <%@ Register TagPrefix="wssuc" TagName="ToolBar" src="/_controltemplates/15/ToolBar.ascx" %> 
<%@ Register TagPrefix="wssuc" TagName="ToolBarButton" src="/_controltemplates/15/ToolBarButton.ascx" %>
 <%@ Register TagPrefix="wssuc" TagName="Welcome" src="/_controltemplates/15/Welcome.ascx" %>
<%@ Register Tagprefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>


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

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <script>
        var checkBoxGroupName = "propertiesCheckboxes";
        function checkBoxClick() {
            
            var checkBoxHidden = (document.getElementById("<%= HiddenPropertiesSelections.ClientID %>"));
            checkBoxHidden.value = "";
            var selectAllCheckBox = document.getElementById("selectAllCheckBox");
            if (selectAllCheckBox != null)
                selectAllCheckBox.checked = false;
            var i;
             
          

            for (i = 0; i < theForm.length; i++) {                
                //curGroup = theForm[i].group;
                if (theForm[i].id != 'ctl00_PlaceHolderMain_cbOverwrite')
                if (  theForm[i].checked == true) {
                   
                    checkBoxHidden.value += theForm[i].name + "#";
                }
            }
           
        }
        var allchecked = false;

        function selectAllCheckBoxClick(selectAllCheckBox) {
            
            allchecked = !allchecked;
            for (i = 0; i < theForm.length; i++) {
                //curGroup = theForm[i].group;
                if (theForm[i].id != 'ctl00_PlaceHolderMain_cbOverwrite')
                    theForm[i].checked = allchecked;
            }
            checkBoxClick();
        }
        function _spBodyOnLoad() {
             
            var checkBoxHidden = (document.getElementById("<%= HiddenPropertiesSelections.ClientID %>"));
            checkBoxHidden.value = "";
        }

    </script>
    <input type="hidden" id="HiddenPropertiesSelections" runat="server" />
    <h3>
        <asp:Label runat="server" ID="txtImportHeader"></asp:Label>
    </h3>
    <asp:Label runat="server" ID="lblDesc" Text="Paste the import file url into the textbox to import the properties:"></asp:Label>
    <br />
    <asp:TextBox runat="server" ID="txtUrl" CssClass="ms-input" Width="320"></asp:TextBox>
    <asp:Button runat="server" ID="btnLoadProperties" Text="Load Properties" OnClick="btnLoadProperties_Click"
        CssClass="ms-ButtonHeightWidth" /><br /><br />
    <asp:CheckBox runat="server" ID="cbOverwrite" Text="Overwrite existing properties" /><br />
    If you select 'Overwrite existing properties', then the existing property is overwritten.<br /><br />
    <asp:Label runat="server" ID="lblMsg" ForeColor="Red" CssClass="ms-descriptiontext"></asp:Label>
    <br />
    <wssuc:ToolBar id="Toolbar" runat="server" CssClass="ms-toolbar">
        <template_buttons>
            <wssuc:ToolBarButton runat="server"
                    id="btnImport"
                    Text="Import selected properties"
                    ToolTip="Click here to start the import."
                    ImageUrl="/_layouts/images/IMPITEM.GIF"
                    OnClick="btnImport_Click"
                    Padding="2px"
                    AccessKey="A" />
        </template_buttons>
    </wssuc:ToolBar>
    <table width="100%" class="propertysheet" cellspacing="0" cellpadding="0" border="0">
        <tr>
            <td class="ms-descriptionText">
            </td>
            <SharePoint:SPGridView ID="grdProperties" runat="server" Width="100%" AllowSorting="True"
                AutoGenerateColumns="false">
                <AlternatingRowStyle CssClass="ms-alternating" />
                <Columns>
                    <asp:BoundField DataField="ID" DataFormatString='&lt;input type="checkbox" group="propertiesCheckboxes" name="{0}" onclick="checkBoxClick();">'
                        HtmlEncode="false" HeaderText='<img id="selectAllCheckBox" src="/_layouts/images/unchecka.gif" onclick="selectAllCheckBoxClick(this);" />'
                        ItemStyle-Width="1" />
                    <asp:BoundField DataField="Key" HeaderText="Key" />
                    <asp:BoundField DataField="Value" HeaderText="Value" />
                </Columns>
            </SharePoint:SPGridView>
        </tr>
        <tr>
            <td height="10px" class="ms-descriptiontext" colspan="2">
                <img src="/_layouts/images/blank.gif" width="1" height="10" alt="">
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <table cellpadding="0" cellspacing="0" width="100%">
                    <colgroup>
                        <col width="99%" />
                        <col width="1%" />
                    </colgroup>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button CssClass="ms-ButtonHeightWidth" name="BtnCancel" ID="BtnCancel" OnClick="btnCancel_Click"
                                Text="Close" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Import Properties
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    Import Properties
</asp:Content>