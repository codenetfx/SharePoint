






function actuateLink(link) {
    var allowDefaultAction = true;

    if (link.click) {
        link.click();
        return;
    }
    else if (document.createEvent) {
        var e = document.createEvent('MouseEvents');
        e.initEvent(
           'click'     // event type
           , true      // can bubble?
           , true      // cancelable?
        );
        allowDefaultAction = link.dispatchEvent(e);
    }

    if (allowDefaultAction) {
        var f = document.createElement('form');
        f.action = link.href;
        document.body.appendChild(f);
        f.submit();
    }
}



var $j = jQuery.noConflict(true);






        //----------

        function CheckUserSourcingInitiatorMember() {
            var userId = _spPageContextInfo.userId;
            //var thisLoc = location.protocol + "//" + location.host;
            try {
                $.ajax({
                    url: siteUrl + "/_api/web/sitegroups()?$select=id&$filter=(Title eq 'Sourcing Initiator Member')",
                    method: "GET",
                    headers: {
                        "accept": "application/json; odata=verbose"
                    },
                    success: function (groupData) {
                        var listResults = groupData.d.results;
                        if (listResults.length > 0) {
                            $.ajax({
                                url: siteUrl + "/_api/Web/SiteGroups/GetById(" + listResults[0].Id + ")/Users?$select=Id,Title&$filter=(Id eq " + userId + ")",
                                method: "GET",
                                headers: {
                                    "accept": "application/json; odata=verbose"
                                },
                                success: function (userData) {
                                    if (userData.d.results.length > 0) {
                                        GetCurrentUser();
                                        currentUser = $('#mocCurrentUser').val();
                                        $('#mocEandCMember').val(currentUser);
                                    } else {
                                        $('#mocEandCMember').val('');
                                    }
                                },
                                error: function (err) {
                                    // Error
                                    alert(JSON.stringify(err));
                                }
                            });
                        }
                    },
                    error: function (err) {
                        // Error
                        alert(JSON.stringify(err));
                    }
                });
            }
            catch (err) {
                alert(JSON.stringify(err));
            }
        }

        //----------

        function SendEmail(from, to, body, subject) {
            $.ajax({
                contentType: 'application/json',
                url: siteUrl + "/_api/SP.Utilities.Utility.SendEmail",
                method: "POST",
                data: JSON.stringify({
                    'properties': {
                        '__metadata': { 'type': 'SP.Utilities.EmailProperties' },
                        'From': from,
                        'To': { 'results': to },
                        'Body': body,
                        'Subject': subject
                    }
                }
              ),
                headers: {
                    "Accept": "application/json;odata=verbose",
                    "content-type": "application/json;odata=verbose",
                    "X-RequestDigest": $("#__REQUESTDIGEST").val()
                },
                success: function (data) {
                    alert('Sent mail notification to PSM Coordinators')
                },
                error: function (err) {
                    alert(JSON.stringify(err));
                }
            });
        }

        function EmailSourcingInitiator() {
            try {
                $.ajax({
                    url: siteUrl + "/_api/web/sitegroups/getbyname('SourcingInitiator')/users",
                    method: "GET",
                    headers: {
                        "accept": "application/json; odata=verbose"
                    },
                    success: function (userData) {
                        var userResults = userData.d.results;
                        if (userResults.length > 0) {
                            var psmCoordinators = [];
                            for (var i = 0; i < userResults.length; i++) {
                                psmCoordinators.push(userResults[i].Email);
                            }
                                  
                            var subject = 'Notification for SI Members';
                            var body = 'Body - Notification for SI Members';
                            var from = 'noreply@sharepoint.com';

                            SendEmail(from, psmCoordinators, body, subject);
                            
                        }
                    },
                    error: function (err) {
                        // Error
                        alert(JSON.stringify(err));
                    }
                });
            }
            catch (err) {
                alert(JSON.stringify(err));
            }

        }

        //----------

        function CheckUserDowngradingApprover() {
            var userId = _spPageContextInfo.userId;
            //var thisLoc = location.protocol + "//" + location.host;
            try {
                $.ajax({
                    url: siteUrl + "/_api/web/sitegroups()?$select=id&$filter=(Title eq 'Downgrading Approver')",
                    method: "GET",
                    headers: {
                        "accept": "application/json; odata=verbose"
                    },
                    success: function (groupData) {
                        var listResults = groupData.d.results;
                        if (listResults.length > 0) {
                            $.ajax({
                                url: siteUrl + "/_api/Web/SiteGroups/GetById(" + listResults[0].Id + ")/Users?$select=Id,Title&$filter=(Id eq " + userId + ")",
                                method: "GET",
                                headers: {
                                    "accept": "application/json; odata=verbose"
                                },
                                success: function (userData) {
                                    if (userData.d.results.length > 0) {
                                        GetCurrentUser();
                                        currentUser = $('#siCurrentUser').val();
                                        $('#siDowngradingApproverMember').val(currentUser);
                                    } else {
                                        $('#siDowngradingApproverMember').val('');
                                    }
                                },
                                error: function (err) {
                                    // Error
                                    alert(JSON.stringify(err));
                                }
                            });
                        }
                    },
                    error: function (err) {
                        // Error
                        alert(JSON.stringify(err));
                    }
                });
            }
            catch (err) {
                alert(JSON.stringify(err));
            }
        }

        //----------


        LoadMocDashboard = function () {
           
            //debugger;
            GetCurrentUser();
           

            //---
            RequestID = $('#RequestID').val();
            ProjectName = $('#ProjectName').val();
            Scope = $('#Scope').val();
            SupplierList = $('#SupplierList').val();
            Status = $('#Status').val();
            ProjectStart = $('#ProjectStart').val();


            Basket = $('#Basket').val();
            Category = $('#Category').val();
            CategoryType = $('#CategoryType').val();
            BU = $('#BU').val();
            SubBU = $('#SubBU').val();
            PlannedProject = $('#PlannedProject').val();
            LastModifiedBy = $('#LastModifiedBy').val();
            BiddingStart = $('#BiddingStart').val();
            BiddingType = $('#BiddingType').val();

            TotalProjectSpendBUs = $('#TotalProjectSpendBUs').val();
            TotalProjectSpendRegions = $('#TotalProjectSpendRegions').val();
            ProjectSpendForBUA = $('#ProjectSpendForBUA').val();
            ProjectSpendForBUB = $('#ProjectSpendForBUB').val();
            ProjectSpendForBUL = $('#ProjectSpendForBUL').val();
            ProjectSpendForAP = $('#ProjectSpendForAP').val();
            ProjectSpendForCE = $('#ProjectSpendForCE').val();
            ProjectSpendForCIS = $('#ProjectSpendForCIS').val();
            ProjectSpendForIMEA = $('#ProjectSpendForIMEA').val();
            ProjectSpendForLAN = $('#ProjectSpendForLAN').val();
            ProjectSpendForLAS = $('#ProjectSpendForLAS').val();
            ProjectSpendForNA = $('#ProjectSpendForNA').val();
            ProjectSpendForWE = $('#ProjectSpendForWE').val();
            ExceptionCountry = $('#ExceptionCountry').val();
            AddressableSpend = $('#AddressableSpend').val();
            AuctionableSpend = $('#AuctionableSpend').val();

            SuggestedCollaborationModel = $('#SuggestedCollaborationModel').val();
            GlobalResponsible = $('#GlobalResponsible').val();
            SelectedCollaborationModel = $('#SelectedCollaborationModel').val();
            DowngradingApprover = $('#DowngradingApprover').val();
            SourcingType = $('#SourcingType').val();
            ProjectManager = $('#ProjectManager').val();
            Comment = $('#Comment').val();
           




            $.ajax({

                url: siteUrl + "/_api/web/lists/GetByTitle('SourcingInitiatorList')/items?$select=RequestID,Title,ProjectName,Scope,Status,ProjectStart,Basket",
                type: "GET",
                headers: {
                    "accept": "application/json;odata=verbose",
                },
                success: function (data) {
                    $("#mocDashboard").find("tr:gt(0)").remove();

                    $.each(data.d.results, function (index, item) {
                        dateDisplay = item.ProjectStart;
                        dateDisplay = dateDisplay.substring(0, dateDisplay.indexOf("T"));
                       
                        
                        //currentUser = $('#mocCurrentUser').val();
                       
                        $("#mocDashboard").append("<tr><td><input type='checkbox' id=" + item.ID + "></td><td>" + item.RequestID + "</td><td>" + item.ProjectName + "</td><td>" + item.Scope + "</td><td>"  + item.Status + "</td><td>" + dateDisplay + "</td><td>" + item.Basket + "</td></tr>");
                        

                    });

                   
          

                },
                error: function (error) {
                    alert(JSON.stringify(error));
                }

            });
        }


        LoadSection1 = function () {


            //var selectedItem = $('#selectedItemId').val();

            //if ((selectedItem == "") || (selectedItem == "0")) {
            //    return false;
            //}

            //url: siteUrl + "/_api/web/lists/GetByTitle('SourcingInitiatorList')/items?$select=RequestID,Title,ProjectName,Scope,SupplierList,Status,ProjectStart,Basket,Category,CategoryType,BU,SubBU,PlannedProject,LastModifiedBy,BiddingStart,BiddingType&$filter=ID eq " + selectedItem,


            $.ajax({

                url: siteUrl + "/_api/web/lists/GetByTitle('SourcingInitiatorList')/items?$select=ID,RequestID,Title,ProjectName,Scope,SupplierList,Status,ProjectStart,Basket,Category,CategoryType,BU,SubBU,PlannedProject,LastModifiedBy,BiddingStart,BiddingType",
                type: "GET",
                headers: {
                    "accept": "application/json;odata=verbose",
                },
                success: function (data) {
                    $.each(data.d.results, function (index, item) {
                     
                        dateDisplay = item.ProjectStart;
                        dateDisplay = dateDisplay.substring(0, dateDisplay.indexOf("T"));
                        $("#bdRequestID").text(item.RequestID);
                        $("#bdProjectName").val(item.ProjectName);
                        $("#bdScope").val(item.Scope);
                        $("#bdSupplierList").val(item.SupplierList);
                        $("#bdStatus").val(item.Status);
                        $("#bdProjectStart").val(item.ProjectStart);
                        $("#bdBasket").val(item.Basket);
                        $("#bdCategory").val(item.Category);
                        $("#bdCategoryType").val(item.CategoryType);
                        $("#bdBU").val(item.BU);
                        $("#bdSubBU").val(item.SubBU);
                        $("#bdPlannedProject").val(item.PlannedProject);
                        $("#bdLastModifiedBy").val(item.LastModifiedBy);
                        $("#bdBiddingStart").val(item.BiddingStart);
                        $("#bdBiddingType").val(item.BiddingType);
    

                    });
                },
                error: function (error) {
                    alert(JSON.stringify(error));
                }

            });



            //$("#relevantColumnId").val("OriginationFormDocumentation");

            




        }

      
        LoadSection2 = function () {

            // Specify the unique ID of the DOM element where the
            // picker will render.
            //initializePeoplePicker('peoplePickerDiv');


            //var selectedItem = $('#selectedItemId').val();

            //if (selectedItem == "") {
            //    return false;
            //}

            return false;

            

            $.ajax({

                url: siteUrl + "/_api/web/lists/GetByTitle('SourcingInitiatorList')/items?$filter=ID eq " + selectedItem,

                type: "GET",
                headers: {
                    "accept": "application/json;odata=verbose",
                },
                success: function (data) {
                    $.each(data.d.results, function (index, item) {
                        //todo

                    });

                  

                },
                error: function (error) {
                    alert(JSON.stringify(error));
                }

            });


            $("#activeSectionId").val(2);
            




        }

      
      

        var siteUrl = window.location.protocol + '//' + window.location.host + _spPageContextInfo.siteServerRelativeUrl;
        


        });

        

       


        function ClearForms() {

            //selId = $('#selectedItemId').val();
            $('#tabs-1 input[type="text"]').val('');            
            $('#tabs-1 input[type="radio"]').prop('checked', false);

        }

        


        //-----------

        function GetCurrentUser() {

            var userid = _spPageContextInfo.userId;
            var requestUri = _spPageContextInfo.webAbsoluteUrl + "/_api/web/getuserbyid(" + userid + ")";
            var requestHeaders = { "accept": "application/json;odata=verbose" };
            $.ajax({
                url: requestUri,
                contentType: "application/json;odata=verbose",
                headers: requestHeaders,
                success: onSuccessCurrentUser,
                error: onErrorCurrentUser
            });
        }

        function onSuccessCurrentUser(data, request) {
            var loginName = data.d.Title;
            $('#mocCurrentUser').val(loginName);
            //alert(loginName);
        }

        function onErrorCurrentUser(error) {
            //alert("error");
        }


        function onSuccessLoginName(data, request) {
            var loginName = data.d.Title;
            //alert(loginName);
        }

        function onErrorLoginName(error) {
            //alert("error");
        }

        //-----------



   
        function populateIdControl(e, i) {
            e.preventDefault();
            $(this).val(i.item.value);
            // $("#" + e.target.id + "Id").val(i.item.id);
            //alert(i.item.id);

            var restSource = siteUrl + "/_api/web/siteusers(@v)?@v='" + encodeURIComponent("i:0#.w|" + i.item.id) + "'";

            $.ajax({
                url: restSource,
                method: "GET",
                headers: { "accept": "application/json; odata=verbose" },
                success: function (data) {
                    //var jsonObject = JSON.parse(data.body);
                    // $("#" + e.target.id + "Id").val(jsonObject.d.Id);

                    $("#" + e.target.id + "Id").val(data.d.Id);
                    //alert(JSON.stringify(data));
                },
                error: function (err) {
                    //alert(JSON.stringify(err));
                }
            });


        }

        // search - START

        function searchallusers(request, response) {
            //var appweburl = decodeURIComponent(getQueryStringParameter('SPAppWebUrl'));
            //var hostweburl = decodeURIComponent(getQueryStringParameter('SPHostUrl'));

            //var restSource = appweburl + "/_api/SP.UI.ApplicationPages.ClientPeoplePickerWebServiceInterface.clientPeoplePickerSearchUser";
            var restSource = siteUrl + "/_api/SP.UI.ApplicationPages.ClientPeoplePickerWebServiceInterface.clientPeoplePickerSearchUser";


            //var principalType = this.element[0].getAttribute('principalType');

            //userString = request.term.charAt(0).toUpperCase() + request.term.slice(1);

            //var restSource = siteUrl + "/_api/web/siteusers?$filter=substringof('" + userString + "', Title)";


            $.ajax(
            {
                'url': restSource,
                'method': 'POST',
                'data': JSON.stringify({
                    'queryParams': {
                        '__metadata': {
                            'type': 'SP.UI.ApplicationPages.ClientPeoplePickerQueryParameters'
                        },
                        'AllowEmailAddresses': true,
                        'AllowMultipleEntities': false,
                        'AllUrlZones': false,
                        'MaximumEntitySuggestions': 50,
                        'PrincipalSource': 15,
                        'PrincipalType': principalType,
                        'QueryString': userString
                        //'Required':false,
                        //'SharePointGroupID':null,
                        //'UrlZone':null,
                        //'UrlZoneSpecified':false,
                        //'Web':null,
                        //'WebApplicationID':null
                    }
                }),
                'headers': {
                    'accept': 'application/json;odata=verbose',
                    'content-type': 'application/json;odata=verbose',
                    'X-RequestDigest': requestDigest
                },
                'success': function (data) {
                    var d = data;
                    var results = JSON.parse(data.d.ClientPeoplePickerSearchUser);
                    if (results.length > 0) {
                        response($.map(results, function (item) {
                            return { label: item.DisplayText, value: item.DisplayText }
                        }));
                    }
                },
                'error': function (err) {
                    alert(JSON.stringify(err));
                }
            }
          );


        }

        // search - END

        function searchSiteUsers(request, response) {
            //e.preventDefault();
            //$(this).val(i.item.value);
            //// $("#" + e.target.id + "Id").val(i.item.id);
            //alert(i.item.id);

            userString = request.term.charAt(0).toUpperCase() + request.term.slice(1);

            var restSource = siteUrl + "/_api/web/siteusers?$filter=substringof('" + userString + "', Title)";
            //var restSource = siteUrl + "/_api/web/SiteGroups/getbyname('MOC Members')/users?$filter=substringof('" + userString + "', Title)";      //$select=Id,Title&$filter=startswith(Title, '" + userString + "')";


            $.ajax({
                url: restSource,
                method: "GET",
                headers: { "accept": "application/json; odata=verbose" },
                success: function (data) {

                    var callResults = data.d.results;
                    if (callResults.length > 0) {
                        response($.map(callResults, function (item) {
                            return {
                                label: item.Title,
                                value: item.Title,
                                id: item.Title
                            }
                        }));
                    }
                },
                error: function (err) {
                    alert(JSON.stringify(err));
                }
            });


        }

        function populateTextBox(userId, controlId) {
            $("#" + controlId + "Id").val("");
            $("#" + controlId).val("");

            if (!($.isNumeric(userId))) {
                return false;
            }

            var restSource = siteUrl + "/_api/web/getuserbyid(" + userId + ")";

            $.ajax({
                url: restSource,
                method: "GET",
                headers: { "accept": "application/json; odata=verbose" },
                success: function (data) {
                    //var jsonObject = JSON.parse(data.body);
                    // $("#" + e.target.id + "Id").val(jsonObject.d.Id);

                    $("#" + controlId).val(data.d.Title);

                    //alert(JSON.stringify(data));
                },
                error: function (err) {
                    //alert(JSON.stringify(err));
                }
            });
        }

        function populateRadioGroup(dataValue, controlName) {
            $('input[name=' + controlName + '][value="' + dataValue + '"]').prop('checked', true);
        }

        function populateDateTextBox(dataValue, controlId) {
            if (dataValue) {
                $('#' + controlId).val(dataValue.substring(0, dataValue.indexOf("T")));
            }
        }

        function populateDataFromControl(controlId) {
            var controlVal = $('#'+ controlId).val();
            if (controlVal.length > 0) {
                return controlVal;
            }
            else {
                return null;
            }
        }

   
   
   
        function searchPickerService(request, response) {
            //var appweburl = decodeURIComponent(getQueryStringParameter('SPAppWebUrl'));    
            //var hostweburl = decodeURIComponent(getQueryStringParameter('SPHostUrl'));    
            var restSource = siteUrl + "/_api/SP.UI.ApplicationPages.ClientPeoplePickerWebServiceInterface.clientPeoplePickerSearchUser";
            //var principalType = this.element[0].getAttribute('principalType');
            $.ajax(
                {
                    'url': restSource,
                    'method': 'POST',
                    'data': JSON.stringify({
                        'queryParams': {
                            '__metadata': {
                                'type': 'SP.UI.ApplicationPages.ClientPeoplePickerQueryParameters'
                            },
                            'AllowEmailAddresses': true,
                            'AllowMultipleEntities': true,
                            'AllUrlZones': true,
                            'MaximumEntitySuggestions': 50,
                            'PrincipalSource': 15,
                            'PrincipalType': 1,
                            'QueryString': request.term
                            //'Required':false,                
                            //'SharePointGroupID':null,                
                            //'UrlZone':null,                
                            //'UrlZoneSpecified':false,               
                            //'Web':null,                
                            //'WebApplicationID':null            
                        }
                    }),
                    'headers': {
                        'accept': 'application/json;odata=verbose',
                        'content-type': 'application/json;odata=verbose',
                        'X-RequestDigest': $("#__REQUESTDIGEST").val()
                    },
                    'success': function (data) {
                        var d = data;
                        var results = JSON.parse(data.d.ClientPeoplePickerSearchUser);
                        if (results.length > 0) {
                            response($.map(results, function (item) {
                                return {
                                    label: item.DisplayText,
                                    value: item.DisplayText,
                                    id: item.Description
                                }
                            }));
                        }
                    },
                    'error': function (err) {
                        alert(JSON.stringify(err));
                    }
                });
        }

        });

        


    });




//}($j));

