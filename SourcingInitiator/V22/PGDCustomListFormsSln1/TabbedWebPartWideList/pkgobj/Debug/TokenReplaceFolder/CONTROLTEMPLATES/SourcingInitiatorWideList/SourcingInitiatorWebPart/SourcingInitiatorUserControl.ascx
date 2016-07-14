<%@ Assembly Name="SourcingInitiatorWideList, Version=1.0.0.0, Culture=neutral, PublicKeyToken=49be9fba15cd86bc" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SourcingInitiatorUserControl.ascx.cs" Inherits="SourcingInitiatorWideList.SourcingInitiatorWebPart.SourcingInitiatorUserControl" %>

<%--<SharePoint:ScriptLink Name="clienttemplates.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink Name="clientforms.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink Name="clientpeoplepicker.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink Name="autofill.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink Name="sp.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink Name="sp.runtime.js" runat="server" LoadAfterUI="true" Localizable="false" />
<SharePoint:ScriptLink Name="sp.core.js" runat="server" LoadAfterUI="true" Localizable="false" />--%>


<link href="/_layouts/15/SourcingInitiator/jquery-ui-1.11.1/jquery-ui.css" rel="stylesheet" />
<script src="/_layouts/15/SourcingInitiator/jquery-ui-1.11.1/external/jquery/jquery.js"></script>
<script src="/_layouts/15/SourcingInitiator/jquery-ui-1.11.1/jquery-ui.js"></script>
<script src="/_layouts/15/SourcingInitiator/SourcingInitiatorLogic.js"></script>

<style>
    #tabs table {
        width: 100%;
        border: 1px solid #000;
        border-collapse: collapse;
    }

     #tabs th, #tabs td {
        
        border-collapse: collapse;
    }

    #tabs th, #tabs td {
        padding: 5px;
        text-align: left;
        font-size:13px;
        width: 100px;
    }

    #mocDashboard tr:nth-child(even), #mocOriginationData tr:nth-child(even) {
        background-color: #cdd2d2;
    }

    #mocDashboard tr:nth-child(odd), #mocOriginationData tr:nth-child(odd) {
        background-color: #ffffff;
    }

    #mocDashboard th, #mocOriginationData th {
        color: #fff;
        background-color: #e1000f;
    }

    input[type=text] {
        width: 200px;
    }




    .container {
        display: table;
        width: 95%;
        border-collapse: collapse;
    }

    .heading {
        font-weight: bold;
        display: table-row;
        background-color: #e1000f;
        text-align: center;
        line-height: 25px;
        font-size: 15px;
        font-family: georgia;
        color: #fff;
    }

    .table-row {
        display: table-row;
        text-align: left;
    }

    .col {
        display: table-cell;
        border: 1px solid #CCC;
    }
</style>



<div id="tabs" style="background:#ffffff">

    <!--ul style="background:#ffffff;border:0px">
        <li style="background:cdd2d2;font"><a href="#tabs-1" id="sectionDashboard">Quick View</a></li>
        <li style="background:cdd2d2"><a href="#tabs-2" id="section1Link">Sourcing Initiator</a></li>
         
    </ul-->

    <!--div id="tabs-1">

        <h2>Sourcing Initiator View</h2>
        <div style="text-align: right">
            
        </div>
        <br />
        
    
        <br />
        <table id="mocDashboard">
            <tr>
                <th></th>
                <th>Request ID
                </th>

                <th>Project Name
                </th>
                <th>Scope</th>
                <th>Status</th>
                <th>Project Start</th>
                <th>Basket</th>
            </tr>
        </table>
        <br />
   
    </div-->

    <div id="tabs-2">

        <!--h2>Sourcing Initiator Data</h2-->
        <br />
        <div style="text-align: left;padding:0px">
            <input style="font-size:12px" type="submit" id="btnSendtoEmptoris" value="Send to Emptoris" />
            <input style="font-size:12px" type="submit" id="btnSendtoGlobal" value="Send to Global" />
            <input style="font-size:12px" type="submit" id="btnSendtoMGL" value="Send to MGL" />
            <input style="font-size:12px" type="submit" id="btnDuplicate" value="Duplicate" />
            <input style="font-size:12px" type="submit" id="btnSave" value="Save" />
            <input style="font-size:12px" type="submit" id="btnNewRequst" value="New Request" />      
            <input type="submit" style="font-size:12px" id="btnRejectRequest" value="Reject Request" />
            <input type="submit" style="font-size:12px" id="btnApproveandSendtoEmptoris" value="Approve and Send to Emptoris" />
            <input type="submit" style="font-size:12px" id="btnRejectDowngrade" value="Reject Downgrade" />
            <!--input type="submit" style="font-size:12px" id="btnDelete" value="Delete" /-->      
            <!--input type="submit" style="font-size:12px" id="btnSendtoSubstitute" value="Send to Substitute" /-->        
        </div>
        <br />

        
       

        <table id="mocBasicData" style="table-layout:fixed; width:100%;border: 1px solid #000;" >
                
                <tr style="background:#cdd2d2">
                    <td >Request ID
                     </td>
                    <td >
                        <input type="text" id="bdRequestID" maxlength="18" />
                    </td>
                     <td >Total Project Spend - BUs</td>
                <td>
                    <input type="text" id="bdTotalProjectSpendBUs" maxlength="18" />
                </td>
                </tr>
                <tr>
                    <td >Project Name
                    </td>
                     <td >
                        <input type="text" id="bdProjectName" maxlength="18" />
                    </td>
                    <td >Total Project Spend - Regions</td>
                <td>
                    <input type="text" id="bdTotalProjectSpendRegions" maxlength="18" />

                </td>
                </tr>

                <tr style="background:#cdd2d2">
                <td >Scope</td>
                    <td >
                        <input type="text" id="bdScope" maxlength="15" />
                    </td>
                    <td>Project Spend for BU A</td>
                <td>                    
                    <input type="text" id="bdTotalProjectSpendBUA" maxlength="15" />
                </td>
                </tr>

                <tr>
                    <td >Supplier List</td>
                    <td >
                        <input type="text" id="bdSupplierList" maxlength="15" />
                    </td>
                    <td>Project Spend for BU B</td>
                 <td>                    
                    <input type="text" id="bdTotalProjectSpendBUB" maxlength="15" />
                </td>
                </tr>
                <tr style="background:#cdd2d2">
                    <td>Status</td>
                    <td >
                        <input type="text" id="bdStatus" maxlength="15" />
                    </td>
                     <td>Project Spend for BU L</td>
                 <td>                    
                    <input type="text" id="bdTotalProjectSpendBUL" maxlength="15" />
                </td>
                </tr>
           
                <tr>
                    <td>Project Start</td>
                    <td >
                        <input type="text" id="bdProjectStart" maxlength="15" />
                    </td>
                    <td>Project Spend for AP</td>
                 <td>                    
                    <input type="text" id="bdTotalProjectSpendAP" maxlength="15" />
                </td>
               </tr>
                
                <tr style="background:#cdd2d2">
                    <td>Basket</td>
                    <td >
                        <input type="text" id="bdBasket" maxlength="15" />
                    </td>
                    <td>Project Spend for CE</td>
                 <td>                    
                    <input type="text" id="bdTotalProjectSpendCE" maxlength="15" />
                </td>
                </tr>
                <tr>
                    <td>Category</td>
                    <td >
                        <input type="text" id="bdCategory" maxlength="15" />
                    </td>
                      <td>Project Spend for CIS</td>
                 <td>                    
                    <input type="text" id="bdTotalProjectSpendCIS" maxlength="15" />
                </td>
                </tr>
                <tr style="background:#cdd2d2">
                    <td>Category Type</td>
                    <td >
                        <input type="text" id="bdCategoryType" maxlength="15" />
                    </td>
                   <td>Project Spend for IMEA</td>
                 <td>                    
                    <input type="text" id="bdTotalProjectSpendIMEA" maxlength="15" />
                </td>
                </tr>
            <tr>
                <td>BU</td>
                <td >
                        <input type="text" id="bdBU" maxlength="15" />
                    </td>
                <td>Project Spend for LAN</td>
                 <td>                    
                    <input type="text" id="bdTotalProjectSpendLAN" maxlength="15" />
                </td>
            </tr>
            <tr style="background:#cdd2d2">
                <td>Sub-BU</td>
                <td >
                        <input type="text" id="bdSubBU" maxlength="15" />
                    </td>
                <td>Project Spend for LAS</td>
                 <td>                    
                    <input type="text" id="bdTotalProjectSpendLAS" maxlength="15" />
                </td>
            </tr>
            <tr>
                <td>Planned Project</td>
                <td >
                        <input type="text" id="bdPlannedProject" maxlength="15" />
                    </td>
                 <td>Project Spend for NA</td>
                 <td>                    
                    <input type="text" id="bdTotalProjectSpendNA" maxlength="15" />
                </td>
            </tr>
            <tr style="background:#cdd2d2">
                <td>Non FP Team Member</td>
                <td >
                        <input type="text" id="bdNonFpTeamMember" maxlength="15" />
                    </td>
                <td>Project Spend for WE</td>
                 <td>                    
                    <input type="text" id="bdTotalProjectSpendWE" maxlength="15" />
                </td>
             </tr>
            <tr>
                <td>Last modified by</td>
                <td >
                        <input type="text" id="bdLastModifiedBy" maxlength="15" />
                    </td>
                <td>Exception Country</td>
                 <td>                    
                    <input type="text" id="bdExceptionCountry" maxlength="15" />
                </td>
            </tr>
            <tr style="background:#cdd2d2">
                <td>Bidding Start</td>
                <td>
                        <input type="text" id="bdBiddingStart" maxlength="15" />
                    </td>
                <td>Addressable Spend</td>
                 <td>                    
                    <input type="text" id="bdAddressableSpend" maxlength="15" />
                </td>
            </tr>
            <tr>
                <td>Bidding Type</td>
                <td>
                        <input type="text" id="bdBiddingType" maxlength="15" />
                    </td>
                <td>Suggested Collaboration Model</td>
                 <td>                    
                    <input type="text" id="bdSuggestedCollaborationModel" maxlength="15" />
                </td>
            </tr>
            <tr style="background:#cdd2d2">
                <td>Auctionable Spend</td>
                 <td>                    
                    <input type="text" id="bdAuctionableSpend" maxlength="15" />
                </td>
                <td>Selected Collaboration Model</td>
                 <td>                    
                    <input type="text" id="bdSelectedCollaborationModel" maxlength="15" />
                </td>
            </tr>

            <tr>
                 <td>Global Responsible</td>
                 <td>                    
                    <input type="text" id="bdGlobalResponsible" maxlength="15" />
                </td>
                <td>Sourcing Type</td>
                 <td>                    
                    <input type="text" id="bdSourcingType" maxlength="15" />
                </td>
            </tr>
            <tr style="background:#cdd2d2">
                 <td>Downgrading Approver</td>
                 <td>                    
                    <input type="text" id="bdDowngradingApprover" maxlength="15" />
                </td>
                 <td>Comment</td> 
                 <td>                    
                    <input type="text" id="bdCollaborationModelComment" maxlength="15" />
                </td>
            </tr>
             <tr>
                <td>Project Manager</td>
                 <td>                    
                    <input type="text" id="bdProjectManager" maxlength="15" />
                </td>
                 <td>Tail End</td> 
                <td>
                        <input id="bdTailEnd" type="checkbox" />
                </td>
            </tr>
            
        </table>
        <br />
         
        <input type="hidden" id="selectedItemId" value="">
        <input type="hidden" id="mocLastMOCNumber" value="">
        <input type="hidden" id="activeSectionId" value="">
        <input type="hidden" id="relevantColumnId" value="">
        <input type="hidden" id="mocCurrentUser" value="">
        <input type="hidden" id="mocPSMCoordinatorMember" value="">
        <input type="hidden" id="mocEandCManagerMember" value="">
    </div>

    
    
   


</div>


