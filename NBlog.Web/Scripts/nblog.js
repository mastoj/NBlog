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
    $('#Content').each(function() {
        var elem = $(this);
        initTimeUpdate(elem, 2000, stylePreview);
    });
    styleCode();
    $("textarea.resizable").TextAreaResizer();
    $("#Title").each(function() {
        var elem = $(this);
        var targetElem = $(".post header h1");
        initTimeUpdate(elem, 1000, function() { targetElem.html(elem.val()) });
    });
});

function initTimeUpdate(target, delay, action) {
    target.keydown(function () {
        if (target.timer) {
            clearTimeout(target.timer);
        }
        target.timer = setTimeout(action, delay);
    });
}

function stylePreview() {
    $(".wmd-preview pre").addClass("prettyprint");
    $(".wmd-preview code").html(window.prettyPrint());
    //$(".wmd-preview code").html(window.prettyPrint()($(".wmd-preview code").html()));
}

function styleCode() {
    if (typeof disableStyleCode != "undefined") {
        return;
    }

    var a = false;

    $("pre code").parent().each(function () {
        if (!$(this).hasClass("prettyprint")) {
            $(this).addClass("prettyprint");
            a = true;
        }
    });

    if (a) {
        window.prettyPrint();
    }
}