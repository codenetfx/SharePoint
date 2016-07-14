<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %> 
 <%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExportPropertyBag.aspx.cs" Inherits="AcmeCorp.Engagements.FarmConfiguration.ExportPropertyBag" MasterPageFile="~/_layouts/15/pbs2013/pbspopup.master"  %>
   
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="System.Text" %>
<%@ Import Namespace="Microsoft.SharePoint.Administration" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%--
Property Bag Settings 
By Alon Havivi(havivi@gmail.com, http://havivi.blogspot.com/) 
For full source code and Terms Of Use,
visit http://pbs2010.codeplex.com/ 
--%>
 

<asp:Content ID="Content1" ContentPlaceHolderID="PlaceHolderMain" runat="server">
  <div>
        <table cellpadding="0" cellspacing="0" style="height: 360; width: 100%">
            <tr>
                <td valign="top" style="height: 10%">
                    <table class="ms-toolbar" width="100%" style="padding-left: 3px; padding-right: 5px;"
                        cellspacing="0" cellpadding="2">
                        <tr>
                            <td width="10%">
                                <table cellspacing="1" cellpadding="0" border="0">
                                    <tr>
                                        <td width="0" class="ms-toolbar">
                                            <asp:ImageButton ID="btnExport1" AlternateText="Export" ImageUrl="/_layouts/images/EXPTITEM.gif"
                                                BorderWidth="0" runat="server" OnClick="Export_Click" />
                                        </td>
                                        <td class="" width="100%" nowrap>
                                            <asp:LinkButton ID="lnkExport1" runat="server" CssClass="ms-toolbar" ToolTip="Export"
                                                OnClick="Export_Click">Export</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="ms-separator" width="2%">
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
                                            Select the SharePoint library in which you want to save the Export file.
                                        </td>
                                        <td>
                                            <asp:DropDownList runat="server" ID="ddlLists" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label runat="server" ID="lblMsg" ForeColor="Red" CssClass="ms-descriptiontext"></asp:Label>
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
                                            <asp:ImageButton ID="btnExport2" OnClick="Export_Click" AlternateText="Export" ImageUrl="/_layouts/images/EXPTITEM.gif"
                                                BorderWidth="0" runat="server" />
                                        </td>
                                        <td width="100%" nowrap>
                                            <asp:LinkButton ID="lnkExport2" OnClick="Export_Click" CssClass="ms-toolbar" runat="server"
                                                ToolTip="Export">Export</asp:LinkButton>
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
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Export Properties
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    Export Properties
</asp:Content>