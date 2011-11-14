using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.Extensions;

namespace NBlog.Helpers
{
    public static class TempAndViewDataHelpers
    {
        public static void AddInfoMessage(this TempDataDictionary tempData, string infoMessage)
        {
            tempData.AddErrorMessage("info", infoMessage);
        }

        public static void AddInfoMessage(this ViewDataDictionary viewData, string infoMessage)
        {
            viewData.AddErrorMessage("info", infoMessage);
        }

        public static void AddSuccessMessage(this TempDataDictionary tempData, string successMessage)
        {
            tempData.AddErrorMessage("success", successMessage);
        }

        public static void AddSuccessMessage(this ViewDataDictionary viewData, string successMessage)
        {
            viewData.AddErrorMessage("success", successMessage);
        }

        public static void AddErrorMessage(this TempDataDictionary tempData, string errorMessage)
        {
            tempData.AddErrorMessage("error", errorMessage);
        }

        public static void AddErrorMessage(this ViewDataDictionary viewData, string errorMessage)
        {
            viewData.AddErrorMessage("error", errorMessage);            
        }

        public static Dictionary<string, List<string>> GetFlashMessages(this TempDataDictionary tempData)
        {
            return GetOrCreateFlashDictionary(tempData);
        }

        public static Dictionary<string, List<string>> GetFlashMessages(this ViewDataDictionary viewData)
        {
            return GetOrCreateFlashDictionary(viewData);
        }

        private static void AddErrorMessage(this IDictionary<string, object> dictionary, string category, string errorMessage)
        {
            var flashDictionary = GetOrCreateFlashDictionary(dictionary);
            var flashMessagesForCategory = GetOrCreateFlashMessages(flashDictionary, category);
            flashMessagesForCategory.Add(errorMessage);
        }

        private static List<string> GetOrCreateFlashMessages(Dictionary<string, List<string>> flashDictionary, string category)
        {
            if (flashDictionary.ContainsKey(category).IsFalse())
            {
                var flashMessages = new List<string>();
                flashDictionary.Add(category, flashMessages);
            }
            return flashDictionary[category];
        }

        private static Dictionary<string, List<string>> GetOrCreateFlashDictionary(IDictionary<string, object> dictionary)
        {
            if (dictionary.ContainsKey("flash").IsFalse())
            {
                var flashDictionary = new Dictionary<string, List<string>>();
                dictionary.Add("flash", flashDictionary);
            }
            return dictionary["flash"] as Dictionary<string, List<string>>;
        }
    }
}