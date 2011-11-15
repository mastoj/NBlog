using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBlog.Specs.Helpers;
using NUnit.Framework;
using TJ.Extensions;
using TechTalk.SpecFlow;
using WatiN.Core;
using Table = TechTalk.SpecFlow.Table;

namespace NBlog.Specs.Steps
{
    [Binding]
    public class FormSteps
    {
        private Dictionary<string, string> buttonMap = new Dictionary<string, string>
                                                           {
                                                               {"log in", "LogIn"},
                                                               {"create", "Create"},
                                                               {"save", "Save"}
                                                           };

        private static Dictionary<string, Action<string, string>> SetFieldMapping =
            new Dictionary<string, Action<string, string>>
                {
                    {"bool", (id, value) => { SetCheckbox(id, value); }},
                    {"longstring", (id, value) => { SetTextArea(id, value); }},
                    {"string", (id, value) => { SetTextField(id, value); }},
                    {"datetime", (id, value) => { SetTextField(id, value); }}
                };

        private static Dictionary<string, Action<string, string>> CheckFieldMapping =
            new Dictionary<string, Action<string, string>>
                {
                    {"bool", (id, value) => { CheckCheckbox(id, value); }},
                    {"longstring", (id, value) => { CheckTextField(id, value); }},
                    {"string", (id, value) => { CheckTextField(id, value); }},
                    {"datetime", (id, value) => { CheckTextField(id, value); }}
                };

        [When(@"I enter the following information")]
        public void WhenEnterTheFollowingInformation(Table table)
        {
            ExecuteTableCommand(table, SetFieldMapping);
        }

        [Then(@"I should see the following pre-filled form")]
        public void ThenIShouldSeeTheFollowing(Table table)
        {
            ExecuteTableCommand(table, CheckFieldMapping);
        }

        private void ExecuteTableCommand(Table table, Dictionary<string, Action<string, string>> actionMapping)
        {
            foreach (var row in table.Rows)
            {
                var id = row["InputField"];
                var value = row["Input"];
                var dataType = row.ContainsKey("DataType") ? row["DataType"] : "string";
                if (actionMapping.ContainsKey(dataType))
                {
                    actionMapping[dataType](id, value);
                }
                else
                {
                    Assert.Fail("Unknown data type");
                }
            }
        }

        private static void CheckTextField(string id, string value)
        {
            var field = GetField(id);
            var isMatch = field.Value == value;
            AssertIfFalse(isMatch, id, value, field.Value);
        }

        private static void CheckCheckbox(string id, string value)
        {
            var checkBox = GetCheckBox(id);
            var boolValue = bool.Parse(value);
            var isMatch = checkBox.Checked == boolValue;
            AssertIfFalse(isMatch, id, boolValue.ToString(), checkBox.Checked.ToString());
        }

        private static void AssertIfFalse(bool isMatch, string id, string expected, string actual)
        {
            if (isMatch.IsFalse())
            {
                Assert.Fail("Field {0} has mismatch. Expected: {1}, Actual: {2}", id, expected, actual);
            }
        }

        private static void SetTextArea(string id, string value)
        {
            SetTextField(id, value);
        }

        private static void SetTextField(string id, string value)
        {
            var field = GetField(id);
            field.TypeText(value);
        }

        private static TextField GetField(string id)
        {
            var field = WebBrowser.Current.TextField(Find.ById(id));
            if (field.Exists.IsFalse())
            {
                Assert.Fail("Missing input field with id {0}", id);
            }
            return field;
        }

        private static void SetCheckbox(string id, string value)
        {
            var checkBox = GetCheckBox(id);
            var boolValue = bool.Parse(value);
            checkBox.Checked = boolValue;
        }

        private static CheckBox GetCheckBox(string id)
        {
            var checkBox = WebBrowser.Current.CheckBox(Find.ById(id));
            if (checkBox.Exists.IsFalse())
            {
                Assert.Fail("Missing checkbox with id {0}", id);
            }
            return checkBox;
        }

        [When(@"I click the ""(.*)"" button")]
        public void WhenIClickTheButton(string buttonId)
        {
            var button = WebBrowser.Current.Button(Find.ById(buttonMap[buttonId]));
            if (button.Exists.IsFalse())
            {
                Assert.Fail("Can't find button {0}", buttonId);
            }
            button.Click();
        }

        [Then(@"I should see a (.*) message")]
        public void ThenIShouldSeeASuccessMessage(string messageType)
        {
            var selectorDictionary = new Dictionary<string, string>
                                         {
                                             {"success", "div.success"},
                                             {"error", "div.error"}
                                         };
            var selector = selectorDictionary[messageType];
            var errorContainer = WebBrowser.Current.Element(Find.BySelector(selector));
            if (errorContainer.Exists.IsFalse())
            {
                Assert.Fail("Can't find flash for " + messageType);
            }
        }

        [Then(@"there should be a ""(.*)"" button")]
        public void ThenThereShouldBeAButton(string buttonIdentifier)
        {
            var button = WebBrowser.Current.Button(Find.ById(buttonMap[buttonIdentifier]));
            if (button.Exists.IsFalse())
            {
                Assert.Fail("Can't find the {0} button", buttonIdentifier);
            }
        }

        [Then(@"no ""(.*)"" button")]
        public void ThenNoLoginButton(string buttonIdentifier)
        {
            var button = WebBrowser.Current.Button(Find.ById(buttonMap[buttonIdentifier]));
            if (button.Exists.IsTrue())
            {
                Assert.Fail("Find button {0} but it should not be there", buttonIdentifier);
            }
        }
    }
}
