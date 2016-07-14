<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %> 
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ModifyPropertyBag.aspx.cs" Inherits="AcmeCorp.Engagements.FarmConfiguration.ModifyPropertyBagSite" MasterPageFile="~/_layouts/15/pbs2013/pbspopup.master"   %>

<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
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
                                            <a href="javascript:__doPostBack(&#39;ctl00$PlaceHolderMain$ProfileSave&#39;,&#39;&#39;)"
                                                title="Save and Close">
                                                <asp:ImageButton ID="btnSave1" AlternateText="Save and Close" ImageUrl="/_layouts/images/save.gif"
                                                    BorderWidth="0" runat="server" OnClick="Save_Click" /></a>
                                        </td>
                                        <td class="" width="100%" nowrap>
                                            <asp:LinkButton ID="lnkSave1" runat="server" CssClass="ms-toolbar" ToolTip="Save and Close"
                                                OnClick="Save_Click">Save</asp:LinkButton>
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
                                            <asp:CheckBox ID="cbMask" Text="Encrypt" runat="server" CssClass="ms-input"></asp:CheckBox>
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
                                            <a href="javascript:__doPostBack(&#39;ctl00$PlaceHolderMain$ProfileSave&#39;,&#39;&#39;)"
                                                title="Save and Close">
                                                <asp:ImageButton ID="btnSave2" OnClick="Save_Click" AlternateText="Save and Close"
                                                    ImageUrl="/_layouts/images/save.gif" BorderWidth="0" runat="server" /></a>
                                        </td>
                                        <td width="100%" nowrap>
                                            <asp:LinkButton ID="lnkSave2" OnClick="Save_Click" CssClass="ms-toolbar" runat="server"
                                                ToolTip="Save">Save</asp:LinkButton>
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

<asp:Content ID="Content2" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Property Bag Settings
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
    Modify Property
</asp:Content>
