/// <reference path="jquery-1.8.2.intellisense.js" />
(function () {
    var converter = Markdown.getSanitizingConverter();
    var options = {
        inputId: "Content"
    }
    var editor = new Markdown.Editor(converter, options);
    editor.run();
})();

