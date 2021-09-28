$(document).ready(function () {
    var name = document.getElementById("AppName").value;
    $('#nameContact').html("name");

    if (window.location.href.indexOf("?Form=Contact") > -1) {
        $('#contact').removeClass("sectionHide");
        $('#default').addClass("sectionHide");
        $("#AppName").removeAttr("required");

        $('select option[value*="Send"]').remove();
        $('#contactInfo').removeClass("sectionHide");

      


    }


    $('#TechSame').click(function () {
        $("#TechName").removeAttr("required");
        $("#TechEmail").removeAttr("required");
        $("#TechPhone").removeAttr("required");
        $("#TechCompany").removeAttr("required");
    });
    $("#AdminSame").click(function () {
        $("#AdminName").removeAttr("required");
        $("#AdminEmail").removeAttr("required");
        $("#AdminPhone").removeAttr("required");
        $("#AdminCompany").removeAttr("required");
    });
    

});
