

$(document).ready(function () {

    //NOTE: Ajax command MUST start with the Ajax provider key.  (Plugin Ref)

    $('#syspay_cmdSave').unbind("click");
    $('#syspay_cmdSave').click(function () {
        $('.processing').show();
        $('.actionbuttonwrapper').hide();
        nbxget('nbrightsystempay_savesettings', '.syspaydata', '.syspayreturnmsg');
    });

    $('.selectlang').unbind("click");
    $(".selectlang").click(function () {
        $('.editlanguage').hide();
        $('.actionbuttonwrapper').hide();
        $('.processing').show();
        $("#nextlang").val($(this).attr("editlang"));
        nbxget('nbrightsystempay_selectlang', '.syspaydata', '.syspaydata');
    });


    $(document).on("nbxgetcompleted", NBS_SysPay_nbxgetCompleted); // assign a completed event for the ajax calls

    // function to do actions after an ajax call has been made.
    function NBS_SysPay_nbxgetCompleted(e) {

        $('.processing').hide();
        $('.actionbuttonwrapper').show();
        $('.editlanguage').show();

        if (e.cmd == 'nbrightsyspayajax_selectlang') {
                        
        }

    };

});

