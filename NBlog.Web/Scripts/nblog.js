/// <reference path="jquery.textarearesizer.min.js" />
/// <reference path="jquery-1.8.2.intellisense.js" />
$(function() {
    (function () {
        var converter = Markdown.getSanitizingConverter();
        var contentEditor = new Markdown.Editor(converter, {
            inputId: "Content"
        });
        contentEditor.run();
        var excerptEditor = new Markdown.Editor(converter, { inputId: "Excerpt", buttonBarId: "excerptButtonBar", Preview: false });
        excerptEditor.run();
    })();
    $('#Content').keydown(function () {
        var elem = $(this);
        if (elem.timer) {
            clearTimeout(elem.timer);
        }
        elem.timer = setTimeout('stylePreview()', 2000);
    });
    styleCode();
    $("textarea.resizable").TextAreaResizer();
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