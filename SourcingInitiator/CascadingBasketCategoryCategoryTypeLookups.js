

<script type="text/javascript">
var SourcingInitiatorJSLink = SourcingInitiatorJSLink || {};

SourcingInitiatorJSLink.Loaded = false;
 
(function () {
 
    //link required plugins from CDN
   
    //(window.jQuery || document.write('<script src="//ajax.aspnetcdn.com/ajax/jquery/jquery-1.10.2.min.js"><\/script><script src="//code.jquery.com/ui/1.11.3/jquery-ui.js"><\/script>'));
    //document.write('<script src="//cdnjs.cloudflare.com/ajax/libs/chosen/1.1.0/chosen.jquery.min.js"><\/script>')
    //document.write('<link href="//cdnjs.cloudflare.com/ajax/libs/chosen/1.1.0/chosen.css" rel="stylesheet" type="text\/css"><\/script>')
 
  loadCss('//ajax.googleapis.com/ajax/libs/jqueryui/1.11.1/themes/smoothness/jquery-ui.css');
    loadScript("//ajax.googleapis.com/ajax/libs/jquery/1.11.1/jquery.min.js", function () {
        loadScript("//ajax.googleapis.com/ajax/libs/jqueryui/1.11.1/jquery-ui.min.js", function () {
            
    //alert("jquery loaded");
 
    $('body').on('change', "*[id='" + "categorySelectField" + "']", function() {
	category = $("#categorySelectField").val();
	//alert("change=" + category);
        //getBaskets(category);
    });

    
    var categoryType= $("#categoryTypeSelectField").val();		
    getCategories(categoryType);
   

        });
    });

 
    // Create object that have the context information about the field that we want to change it output render 
    var sourcingInitiatorContext = {};
    sourcingInitiatorContext.Templates = {};
    sourcingInitiatorContext.Templates.Fields = {
        // Apply the new rendering 
    "Basket": {
            "NewForm": basketFieldTemplate,
            "EditForm": basketFieldTemplate
        },
    "CategoryType": {
            "NewForm": categoryTypeFieldTemplate,
            "EditForm": categoryTypeFieldTemplate
        },
    "Category": {
            "NewForm": categoryFieldTemplate,
            "EditForm": categoryFieldTemplate
        },
    "BU": {
            "NewForm": buFieldTemplate,
            "EditForm": buFieldTemplate
        }
     
    };
 
 
    SPClientTemplates.TemplateManager.RegisterTemplateOverrides(sourcingInitiatorContext);
 
 
    _spBodyOnLoadFunctionNames.push("setValues");
 
    getBusinessUnits();  
 
})();
 

//Helper function to inject CSS.
function loadCss(url) {
    var css = document.createElement('link');
    css.rel = 'stylesheet';
    css.href = url;
    document.getElementsByTagName("head")[0].appendChild(css);

    logMe("CSS loaded [" + url + "]");
}

//Helper function to inject Javascript.
function loadScript(url, callback) {
    var script = document.createElement("script")
    script.type = "text/javascript";
    if (script.readyState) { //IE
        script.onreadystatechange = function () {
            if (script.readyState == "loaded" || script.readyState == "complete") {
                script.onreadystatechange = null;
                callback();
            }
        };
    } else { //Others
        script.onload = function () {
            callback();
        };
    }
    script.src = url;
    document.getElementsByTagName("head")[0].appendChild(script);
    logMe("External Script loaded [" + url + "]");
}

//Helper function to Log a message to the console.
function logMe(message) {
    try {
        var rightNow = new Date();
        window.console.log(dateToYYYYMMDDhhmmss(rightNow) + " - " + message);
    } catch (e) {
        //Logging should not cause errors
    } 
}

//Helper function used when logging messages
function dateToYYYYMMDDhhmmss(date) {
    function pad(num) {
        num = num + '';
        return num.length < 2 ? '0' + num : num;
    }
    return date.getFullYear() + '/' +
        pad(date.getMonth() + 1) + '/' +
        pad(date.getDate()) + ' ' +
        pad(date.getHours()) + ':' +
        pad(date.getMinutes()) + ':' +
        pad(date.getSeconds());
}


function buFieldTemplate(ctx) {

    var formCtx = SPClientTemplates.Utility.GetFormContextForCurrentField(ctx);
   
    formCtx.registerGetValueCallback(formCtx.fieldName, function () {
	var buString = "";
	 $('#buSelectField').children('option:selected').each( function() {
        	var $this = $(this);
        	buString += $this.val() + ';#' + $this.text() + ';#';
    	});
	//strip off the last ;#
	var len = buString.length;
    	if (buString.substr(len-2,2) == ";#") {
        	buString = buString.substring(0,len-2);
    	}
	return buString;
    });


    var fieldHtml = '<select ';
    fieldHtml += 'id="buSelectField">';
    fieldHtml += '"</select>&nbsp;&nbsp;';
  
    return fieldHtml;
}

    
 
// This function provides the rendering logic
function basketFieldTemplate(ctx) {
 
        var formCtx = SPClientTemplates.Utility.GetFormContextForCurrentField(ctx);
    //need to get the ID of this field so we can use it to wire up a change event
    SourcingInitiatorJSLink.BasketFieldId = formCtx.fieldName + '_' + formCtx.fieldSchema.Id + '_$LookupField';
 
    SourcingInitiatorJSLink.EditBasket = ctx.CurrentItem["Basket"];
    var tempCategoryType = ctx.CurrentItem["CategoryType"].toString().split(";#");
    //SharePoint stores multilookup data in ths following format:
    //ID1;#Value1;#ID2;#Value2;#ID2;#Value3
    SourcingInitiatorJSLink.EditCategoryType = [];
    SourcingInitiatorJSLink.EditCategoryTypeLabels = [];
    for (var i = 0; i < tempCategoryType.length; i++) {
        (i % 2 == 0 ? SourcingInitiatorJSLink.EditCategoryType : SourcingInitiatorJSLink.EditCategoryTypeLabels).push(tempCategoryType[i]);
    }
     
    //here we wire up the change event  
    $('body').on('change', "*[id='" + "basketSelectField" + "']", function() {
            //alert(this.value);
        var basket = $(this).find("option:selected").val();
        getCategoryTypes(basket);
         
    });
    //otherwise return the default rendering of this field
    //return SPFieldLookup_Edit(ctx);
 
    fieldHtml  = "<input type='text' id='" + "basketSelectField" + "'>"; 
    return fieldHtml;
}
 
function categoryTypeFieldTemplate(ctx) {
 
    var formCtx = SPClientTemplates.Utility.GetFormContextForCurrentField(ctx);
    SourcingInitiatorJSLink.CategoryTypeFieldId = formCtx.fieldName + '_' + formCtx.fieldSchema.Id + '_$LookupField';
   
    //here we wire up the change event  
    //$('body').on('change', "*[id='" +  "categoryTypeSelectField"  + "']", function() {
    //        //alert("categoryTypeFieldTemplate=" +  SourcingInitiatorJSLink.CategoryFieldId);
    //    var categoryType= $(this).find("option:selected").val();
    //    getCategories(categoryType);
         
    //});

$("#categorySelectField").on('change', function () {
		OnAutoCompleteChanged("categorySelectField", $("#categorySelectField").val());
    	        var categoryType= $("#categorySelectField").val();
    		getCategories(categoryType);
	});

    // Register a callback just before submit.  
    formCtx.registerGetValueCallback(formCtx.fieldName, function () {
    var categoryTypeString = "";
     $('#categoryTypeSelectField').children('option:selected').each( function() {
            var $this = $(this);
            categoryTypeString += $this.val() + ';#' + $this.text() + ';#';
        });
    //strip off the last ;#
    var len = categoryTypeString.length;
        if (categoryTypeString.substr(len-2,2) == ";#") {
            categoryTypeString = categoryTypeString.substring(0,len-2);
        }
    return categoryTypeString;
    });
 
 
     //return SPFieldLookup_Edit(ctx);

    var fieldHtml = '<select ';
    fieldHtml += 'id="categoryTypeSelectField">';
    fieldHtml += '"</select>&nbsp;&nbsp;';
     
    return fieldHtml;
}
 

function categoryFieldTemplate(ctx) {
 
     var formCtx = SPClientTemplates.Utility.GetFormContextForCurrentField(ctx);
    //need to get the ID of this field so we can use it to wire up a change event
    SourcingInitiatorJSLink.CategoryFieldId = formCtx.fieldName + '_' + formCtx.fieldSchema.Id + '_$LookupField';
    //alert("ID=" + SourcingInitiatorJSLink.CategoryFieldId);
    
    SourcingInitiatorJSLink.EditCategory = ctx.CurrentItem["Category"];
    var tempCategoryType = ctx.CurrentItem["CategoryType"].toString().split(";#");
    //SharePoint stores multilookup data in ths following format:
    //ID1;#Value1;#ID2;#Value2;#ID2;#Value3
    SourcingInitiatorJSLink.EditCategoryType = [];
    SourcingInitiatorJSLink.EditCategoryTypeLabels = [];
    for (var i = 0; i < tempCategoryType.length; i++) {
        (i % 2 == 0 ? SourcingInitiatorJSLink.EditCategoryType : SourcingInitiatorJSLink.EditCategoryTypeLabels).push(tempCategoryType[i]);
    }
     
    //here we wire up the change event  
    $('body').on('change', "*[id='" + "categorySelectField" + "']", function() {
        //alert("categoryFieldTemplate=" + SourcingInitiatorJSLink.CategoryFieldId);
	
    });


    //otherwise return the default rendering of this field
    //return SPFieldLookup_Edit(ctx);
  
    fieldHtml  = "<input type='text' id='" + "categorySelectField" + "'>"; 
    return fieldHtml;
}

 
 
function setValues() {
     
        if (SourcingInitiatorJSLink.EditBasket && (SourcingInitiatorJSLink.Loaded==false)){
        var str = SourcingInitiatorJSLink.EditBasket;
        //we want the test here instead of the SharePoint ID
        var editBasket = str.substring(str.indexOf('#')+1);
        getCategoryTypes(editbasket,SourcingInitiatorJSLink.EditBasket);
        getCategories(editbasket,SourcingInitiatorJSLink.EditBasket);
	getBusinessUnits();
        SourcingInitiatorJSLink.Loaded=true;
    }
    else{
        getCategoryTypes();
	getCategories();
	getBusinessUnits();
    }
}
 
function getCategoryTypes(category, selectedCategoryTypes) {
    //var queryUrl = "http://esicloud01:5000/sites/henkel/_api/web/lists/GetByTitle('CategoryTypes')/items?$select=CategoryTypeID,Name";
    //if (category){
    //    queryUrl = "http://esicloud01:5000/sites/henkel/_api/web/lists/GetByTitle('Categories')/items?$select=CategoryID,Name";
    //    queryUrl += "&$filter=FK_CategoryType eq '" + encodeURIComponent(category) + "'";   
    //}


    var queryUrl = "http://esicloud01:5000/sites/henkel/_vti_bin/steeringmatrix/masterdata.svc/GetCategoryTypes";
    //alert(queryUrl);
    $.ajax({
        cache: false,
        url: queryUrl,
        type: 'GET',
        dataType: 'json',
        async: false,
        headers: {
            "accept": "application/json;odata=verbose;charset=utf-8"
        },
        success: function (data) {
             
	    if(category) {
		$('#categorySelectField').empty();
	    } else {
            	$('#categoryTypeSelectField').empty();
	    }
 




	    var optionString;

	    for(var i=0;i<data.GetCategoryTypesResult.length;i++) {	  
			optionString = "<option value='" + data.GetCategoryTypesResult[i].ID.toString() + "'>" + data.GetCategoryTypesResult[i].Name.toString() + "</option>";
			$('#categoryTypeSelectField').append(optionString);		
    	   		}



            
            if (selectedCategoryTypes){
                $('#categoryTypeSelectField').val(selectedCategoryTypes);
            }
            $("#categoryTypeSelectField_chosen").width(400);
            $('#categoryTypeSelectField').trigger("chosen:updated")
             
        }, 
        error: function ajaxError(response) {
            alert(response.status + ' ' + response.statusText);
        },
    }); //end of ajax
}
 


function getCategories(categoryType, selectedCategories) {
    var queryUrl = "http://esicloud01:5000/sites/henkel/_vti_bin/steeringmatrix/masterdata.svc/GetCategories";

    //alert(SourcingInitiatorJSLink.CategoryFieldId);
    fieldName = "#" + SourcingInitiatorJSLink.CategoryFieldId + "'";

var restUrl = "http://esicloud01:5000/sites/henkel/_vti_bin/steeringmatrix/masterdata.svc/GetCategories";

$.ajax({
        cache: false,
        url: restUrl,
        type: 'GET',
        dataType: 'json',
        async: false,
        headers: {
            "accept": "application/json;odata=verbose;charset=utf-8"
        },
        success: function (data) {
             
	   //alert("success=" + data.GetCategoriesResult.length);
	   //alert("categorytype=" + categoryType);
	   //categorySelectField

	   var dataArray = new Array();

    	   for(var i=0;i<data.GetCategoriesResult.length;i++) {	   
	   	//dataArray.push(data.GetCategoriesResult[i].Name.toString());
		if (data.GetCategoriesResult[i].CategoryTypeID.toString() == categoryType)
		{
 			dataArray.push({
            			value: data.GetCategoriesResult[i].Name.toString(), 
            			categoryid: data.GetCategoriesResult[i].ID.toString(),
				categorytypeid: data.GetCategoriesResult[i].CategoryTypeID.toString()          
			});  
		}
    	   }

         $('#categorySelectField').empty();
 	         
         $("#categorySelectField").autocomplete({           		
  		source: dataArray,
		autoFocus: true,
		select: function (event, ui) {
			var value = ui.item.value;
			var label = ui.item.categoryid;
 		
			//Attach the event when a user selects an option  from the AutoComplete Control
			OnCategoryAutoCompleteChanged("categorySelectField", value, label); 
			
			$("#categorySelectField").html(label);
			}
         });
    	      
            
	    //alert("after json");
	
	$(function () {
  		$('#categorySelectField').val(label);
		});
	    
           
            if (selectedCategories){
                $('#categorySelectField').val(selectedCategories);
            }
            $("#categorySelectField_chosen").width(400);
            $('#categorySelectField').trigger("chosen:updated")
             
        }, 
        error: function ajaxError(response) {
            alert(response.status + ' ' + response.statusText);
        },
    }); //end of ajax

 
//alert("getCategory - End");
    
}



function OnCategoryAutoCompleteChanged(originalContentTypeControlId, valueSelectedOrEntered, labelSelectedOrEntered) {
   //alert("change triggered on " + originalContentTypeControlId + "with value=" + valueSelectedOrEntered + " and label=" + labelSelectedOrEntered);
  
   getBaskets(labelSelectedOrEntered);
 
   $("#" + originalContentTypeControlId + " option").filter(function() {
         return $(this).html() == valueSelectedOrEntered;
    }).prop('selected', true);

    $("#" + originalContentTypeControlId).trigger('change');
   
}

function getBaskets(category, selectedCategories) {
    var queryUrl = "http://esicloud01:5000/sites/henkel/_vti_bin/steeringmatrix/masterdata.svc/GetBaskets";
	
    //alert(queryUrl);
    $.ajax({
        cache: false,
        url: queryUrl,
        type: 'GET',
        dataType: 'json',
        async: false,
        headers: {
            "accept": "application/json;odata=verbose;charset=utf-8"
        },
        success: function (data) {
             
            $('#basketSelectField').empty();
 
           
            if (selectedCategories){
                $('#basketSelectField').val(selectedCategories);
            }

	  //alert("baskets categoryid filter=" + category);
	   var dataArray = new Array();

    	   for(var i=0;i<data.GetBasketsResult.length;i++) {	  
		if (data.GetBasketsResult[i].CategoryID.toString() == category)
		{
 			dataArray.push({
            			value: data.GetBasketsResult[i].Name.toString(), 
            			basketid: data.GetBasketsResult[i].ID.toString(),
				categoryid: data.GetBasketsResult[i].CategoryID.toString()          
			});  
		}
    	   }

	

         $('#basketSelectField').empty();
 	         
         $("#basketSelectField").autocomplete({           		
  		source: dataArray,
		autoFocus: true,
		select: function (event, ui) {
			var value = ui.item.value;
			var label = ui.item.basketid;
 		
			//Attach the event when a user selects an option  from the AutoComplete Control
			OnBasketAutoCompleteChanged("basketSelectField", value, label); 
			
			$("#basketSelectField").html(label);
			}
         });
    	      
 
            $("#basketSelectField_chosen").width(400);
            $('#basketSelectField').trigger("chosen:updated")
             
        }, 
        error: function ajaxError(response) {
            alert(response.status + ' ' + response.statusText);
        },
    }); //end of ajax
}

function OnBasketsAutoCompleteChanged(originalContentTypeControlId, valueSelectedOrEntered, labelSelectedOrEntered) {
   //alert("change triggered on " + originalContentTypeControlId + "with value=" + valueSelectedOrEntered + " and label=" + labelSelectedOrEntered);
 
   $("#" + originalContentTypeControlId + " option").filter(function() {
         return $(this).html() == valueSelectedOrEntered;
    }).prop('selected', true);

    //$("#" + originalContentTypeControlId).trigger('change');
   
}


function getBusinessUnits() {
	var queryUrl = "http://esicloud01:5000/sites/henkel/_vti_bin/steeringmatrix/masterdata.svc/GetBusinessUnits";	
	//alert(queryUrl);
	$.ajax({
		cache: false,
		url: queryUrl,
		type: 'GET',
		dataType: 'json',
		async: false,
		headers: {
   			"accept": "application/json;odata=verbose;charset=utf-8"
		},
		success: function (data) {
			//alert("BU success=" + data.GetBusinessUnitsResult.length);
			$('#buSelectField').empty();

 			
			var optionString;

			for(var i=0;i<data.GetBusinessUnitsResult.length;i++) {	  		
			optionString = "<option value='" + data.GetBusinessUnitsResult[i].ID.toString() + "'>" + data.GetBusinessUnitsResult[i].Name.toString() + "</option>";
			//alert(optionString);
			$('#buSelectField').append(optionString);
		
    	   		}
			
			$("#buSelectField_chosen").width(400);
			$('#buSelectField').trigger("chosen:updated")
			
		}, 
		error: function ajaxError(response) {
			alert(response.status + ' ' + response.statusText);
		},
	}); //end of ajax
}
</script>