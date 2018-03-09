

$(document).ready(function () {

    $('#paybox_cmdSave').unbind("click");
    $('#paybox_cmdSave').click(function () {
        $('.processing').show();
        $('.actionbuttonwrapper').hide();
        nbxget('nbrightpayboxajax_savesettings', '.payboxdata', '.payboxreturnmsg');
    });

    $('.selectlang').unbind("click");
    $(".selectlang").click(function () {
        $('.editlanguage').hide();
        $('.actionbuttonwrapper').hide();
        $('.processing').show();
        $("#nextlang").val($(this).attr("editlang"));
        nbxget('nbrightpayboxajax_selectlang', '.payboxdata', '.payboxdata');
    });


    $(document).on("nbxgetcompleted", NBS_PayBox_nbxgetCompleted); // assign a completed event for the ajax calls

    // function to do actions after an ajax call has been made.
    function NBS_PayBox_nbxgetCompleted(e) {

        $('.processing').hide();
        $('.actionbuttonwrapper').show();
        $('.editlanguage').show();

        if (e.cmd == 'nbrightpayboxajax_selectlang') {
                        
        }

    };

});

