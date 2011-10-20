﻿using System;
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
        [When(@"I enter the following information")]
        public void WhenEnterTheFollowingInformation(Table table)
        {
            foreach (var row in table.Rows)
            {
                var id = row["InputField"];
                var value = row["Input"];
                var dataType = row.ContainsKey("DataType") ? row["DataType"] : "string";
                switch (dataType)
                {
                    case "bool":
                        SetCheckbox(id, value);
                        break;
                    case "longstring":
                        SetTextArea(id, value);
                        break;
                    default:
                        SetTextField(id, value);
                        break;
                }
            }
        }

        private void SetTextArea(string id, string value)
        {
            SetTextField(id, value);
        }

        private void SetTextField(string id, string value)
        {
            var field = WebBrowser.Current.TextField(Find.ById(id));
            if (field.Exists.IsFalse())
            {
                Assert.Fail("Missing input field with id {0}", id);
            }
            field.TypeText(value);
        }

        private void SetCheckbox(string id, string value)
        {
            var checkBox = WebBrowser.Current.CheckBox(Find.ById(id));
            if (checkBox.Exists.IsFalse())
            {
                Assert.Fail("Missing checkbox with id {0}", id);
            }
            var boolValue = bool.Parse(value);
            checkBox.Checked = boolValue;
        }

        [When(@"I click the (.*) button")]
        public void WhenIClickTheButton(string buttonId)
        {
            var button = WebBrowser.Current.Button(Find.ById(buttonMap[buttonId]));
            if (button.Exists.IsFalse())
            {
                Assert.Fail("Can't find button {0}", buttonId);
            }
            button.Click();
        }
    
        [Then(@"I should see a error message")]
        public void ThenIShouldSeeAErrorMessage()
        {
            var errorContainer = WebBrowser.Current.Element(Find.BySelector("div.error"));
            if (errorContainer.Exists.IsFalse())
            {
                Assert.Fail("Can't find an container with .error class");
            }
        }

        [Then(@"there should be a (.*) button")]
        public void ThenThereShouldBeAButton(string buttonIdentifier)
        {
            var button = WebBrowser.Current.Button(Find.ById(buttonMap[buttonIdentifier]));
            if (button.Exists.IsFalse())
            {
                Assert.Fail("Can't find the {0} button", buttonIdentifier);
            }
        }

        [Then(@"no (.*) button")]
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
