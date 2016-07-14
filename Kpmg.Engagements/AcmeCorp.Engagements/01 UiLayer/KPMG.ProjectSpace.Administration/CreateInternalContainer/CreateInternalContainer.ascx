<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateInternalContainer.ascx.cs" Inherits="AcmeCorp.ProjectSpace.Administration.CreateInternalContainer.CreateInternalContainer" %>


<div style="width:600px">
    <div style="width:600px; float:left; margin-bottom: 20px;">
        <asp:Label ID="lblTitle" runat="server" Text="lblTitle"></asp:Label>
        <br />
        <asp:TextBox ID="tbTitle" runat="server" style="width:590px;"></asp:TextBox>
    </div> 
    <div style="width:200px; float:left;">
        <asp:Label ID="lblContainerOwners" runat="server" Text="lblContainerOwners"></asp:Label>
    </div>
    <div style="width:400px; float:left;">
        <SharePoint:PeopleEditor ID="sppOwners" runat="server" Width="350" SelectionSet="User" MultiSelect="true" />
    </div>

    <div style="width:200px; float:left;">
        <asp:Label ID="lblDeputies" runat="server" Text="lblDeputies"></asp:Label>
    </div>
    <div style="width:400px; float:left;">
        <SharePoint:PeopleEditor ID="sppDeputies" runat="server" Width="350" SelectionSet="User" MultiSelect="true" />
    </div> 

    <div style="width:600px; float:left;">
        <asp:Label ID="lblReason" runat="server" Text="lblReason"></asp:Label>
        <br />
        <asp:TextBox ID="tbReason" runat="server" TextMode="MultiLine" Rows="5" style="width:590px;"></asp:TextBox>
    </div> 
    
    <div style="width:600px; float:left; margin-top:20px;">
         <asp:Button ID="btnSubmit" runat="server" Text="Submit" style="float:left;" OnClick="btnSubmit_Click" />
    </div> 

    <asp:Literal ID="litResult" runat="server"></asp:Literal>
</div> 



<asp:Literal ID="litErrorMsg" runat="server"></asp:Literal>


