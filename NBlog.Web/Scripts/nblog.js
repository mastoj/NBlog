/// <reference path="jquery-1.8.2.intellisense.js" />
$(function() {
    (function () {
        var converter = Markdown.getSanitizingConverter();
        var options = {
            inputId: "Content"
        }
        var editor = new Markdown.Editor(converter, options);
        editor.run();
        $('#Content').keydown(function () {
            var elem = $(this);
            if(elem.timer) {
                clearTimeout(elem.timer);
            }
            elem.timer = setTimeout('stylePreview()', 2000);
        });
        styleCode();
    })();
});

function stylePreview() {
    $(".wmd-preview pre").addClass("prettyprint");
    $(".wmd-preview code").html(prettyPrintOne($(".wmd-preview code").html()));
}

function styleCode() {
    if (typeof disableStyleCode != "undefined") {
        return;
    }

    var a = false;

    $("pre code").parent().each(function () {
        if (!$(this).hasClass("prettyprint")) {
            $(this).addClass("prettyprint");
            a = true
        }
    });

    if (a) { prettyPrint() }
}