function preloader() {
    document.getElementById("preloader").style.display = "none";
    document.getElementById("wrapper").style.display = "block";
}

window.onload = preloader;

function PageLoad() {
    window.self = preloader;
}

function DatePicker(id) {
    $('#' + id).DatePicker({
        date: $('#' + id).val(),
        format: 'd/m/Y',
        onChange: function (formated, dates) {
            $('#' + id).val(formated);
            $('#' + id).DatePickerHide();
        }
    });
}

$(function () {
    $("input, select, button").uniform();
});

var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_endRequest(function () {
    $(document).ready(function () {
        $("input, textarea, select, button").uniform();
    });
});