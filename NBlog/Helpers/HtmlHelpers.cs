using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TJ.Extensions;

namespace NBlog.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString DictionaryToUlLiList<TKey, TValue>(this HtmlHelper html, IDictionary<TKey, List<TValue>> dictionary, string wrappingElement = null, string ulClass = null)
        {
            var outputHtml = new StringBuilder();
            foreach (var entry in dictionary)
            {
                var listBuffer = new StringBuilder();
                listBuffer.Append("<ul class=\"" + ulClass + " " + entry.Key.ToString() + "\"/>");
                AddItems(listBuffer, entry.Value);
                listBuffer.Append("</ul>");
                AddWrappingElement(listBuffer, entry.Key.ToString(), wrappingElement);
                outputHtml.Append(listBuffer);
            }
            return new MvcHtmlString(outputHtml.ToString());
        }

        private static void AddWrappingElement(StringBuilder listBuffer, string cssClass, string wrappingElement)
        {
            if (wrappingElement.IsNotNull())
            {
                listBuffer.Insert(0, "<" + wrappingElement + " class=\"super-container " + cssClass + "\">");
                listBuffer.Append("</" + wrappingElement + ">");
            }
        }

        private static void AddItems<TValue>(StringBuilder outputHtml, List<TValue> value)
        {
            foreach (var item in value)
            {
                outputHtml.Append("<li>" + item + "</li>");
            }
        }
    }
}