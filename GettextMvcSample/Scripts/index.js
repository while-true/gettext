
$(function () {

    $("#index-js-message").text(Gettext._("Hello world from Javascript!"));

    for (var i = 0; i < 6; i++) {
        $("#index-js-message-plural").append(Gettext._("{fileCount} file", "{fileCount} files", i).formatWith({ "fileCount": i }) + "<br/>");
    }

});
