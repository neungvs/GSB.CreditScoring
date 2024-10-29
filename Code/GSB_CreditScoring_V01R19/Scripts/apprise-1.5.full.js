function appOpen(appID, appSEQ, appREQ) {

    var url = 'LoanDetails.aspx?id=' + appID + '&seq=' + appSEQ + '&req=' + appREQ;

    $('body').css("overflow", "hidden");
    $('body').append('<div class="appOverlay" id="aOverlay"></div>');
    $('.appOverlay').fadeIn(0);
    $('body').append('<div class="appOpenOuter"><div class="appOpenInner"><div id="appOpenHead">เลขที่ใบคำขอ: ' + appID + '<a id="close" class="pop_button"></a></div><div id="appOpenContent"><iframe height="100%" class="app-iframe" name="app-iframe" frameborder="0" hspace="0" allowtransparency="true" src="' + url + '"></iframe></div></div></div>');

    $(document).keydown(function (e) {
        if ($('.appriseOverlay').is(':visible')) {
            if (e.keyCode == 13) {
                $('#close').click();
            }
            if (e.keyCode == 27) {
                $('#close').click();
            }
        }
    });

    $('#close').click(function () {
        $('body').css("overflow", "auto");
        $('.appOverlay').remove();
        $('.appOpenOuter').remove();
    });
}

function appOpenMymo(appID, appSEQ, appREQ) {

    var url = 'LoanDetailMymo.aspx?id=' + appID + '&seq=' + appSEQ + '&req=' + appREQ;

    $('body').css("overflow", "hidden");
    $('body').append('<div class="appOverlay" id="aOverlay"></div>');
    $('.appOverlay').fadeIn(0);
    $('body').append('<div class="appOpenOuter"><div class="appOpenInner"><div id="appOpenHead">เลขที่ใบคำขอ: ' + appID + '<a id="close" class="pop_button"></a></div><div id="appOpenContent"><iframe height="100%" class="app-iframe" name="app-iframe" frameborder="0" hspace="0" allowtransparency="true" src="' + url + '"></iframe></div></div></div>');

    $(document).keydown(function (e) {
        if ($('.appriseOverlay').is(':visible')) {
            if (e.keyCode == 13) {
                $('#close').click();
            }
            if (e.keyCode == 27) {
                $('#close').click();
            }
        }
    });

    $('#close').click(function () {
        $('body').css("overflow", "auto");
        $('.appOverlay').remove();
        $('.appOpenOuter').remove();
    });
}

function appPop(subject, details) {

    $('body').css("overflow", "hidden");
    $('body').append('<div class="appOverlay" id="aOverlay"></div>');
    $('.appOverlay').fadeIn(0);
    $('body').append('<div class="appriseOuter"><div class="appriseInner"><div id="heading">' + subject + '<a id="close" class="pop_button"></a></div><div id="content"><p>' + details + '</p></div></div></div>');

    $(document).keydown(function (e) {
        if ($('.appriseOverlay').is(':visible')) {
            if (e.keyCode == 13) {
                $('#close').click();
            }
            if (e.keyCode == 27) {
                $('#close').click();
            }
        }
    });


    $('#close').click(function () {
        $('body').css("overflow", "auto");
        $('.appOverlay').remove();
        $('.appriseOuter').remove();
    });
}