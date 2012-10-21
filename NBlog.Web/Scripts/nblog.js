/// <reference path="jquery.textarearesizer.min.js" />
/// <reference path="jquery-1.8.2.intellisense.js" />
$(function() {
    (function () {
        var converter = Markdown.getSanitizingConverter();
        initMarkdownEditor(converter, { inputId: "Content" });
        initMarkdownEditor(converter, { inputId: "Excerpt", buttonBarId: "excerptButtonBar", preview: false });
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
    $("#tags").tagit();
});

function initMarkdownEditor(converter, options) {
    var defaults = {
        preview: true
    };
    options = $.extend(defaults, options);
    var inputArea = $("#" + options.inputId);
    if (inputArea.length === 0)
        return;
    var editor = new Markdown.Editor(converter, options);
    editor.run();
}

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