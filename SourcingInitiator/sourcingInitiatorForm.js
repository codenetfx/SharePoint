<script src="//code.jquery.com/jquery-1.10.1.min.js"></script>

<style type="text/css">
  .ms-formtable
  {display:none;}
</style>

<script type="text/javascript">
    $(document).ready(function() {
        //loop through all the spans in the custom layout        
        $("span.sourcingInitiatorForm").each(function()
        {
            //get the display name from the custom layout
            displayName = $(this).attr("data-displayName");
            elem = $(this);
            //find the corresponding field from the default form and move it
            //into the custom layout
            $("table.ms-formtable td").each(function(){
                if (this.innerHTML.indexOf('FieldName="'+displayName+'"') != -1){
                    $(this).contents().appendTo(elem);
                }
            });
        });
    });
</script>