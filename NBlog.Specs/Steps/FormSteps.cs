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
                                                               {"log in", "LogIn"}
                                                           };
        [When(@"I enter the following information")]
        public void WhenEnterTheFollowingInformation(Table table)
        {
            foreach (var row in table.Rows)
            {
                var id = row["InputField"];
                var value = row["Input"];
                var field = WebBrowser.Current.TextField(Find.ById(id));
                if (field.Exists.IsFalse())
                {
                    Assert.Fail("Missing input field with id {0}", id);
                }
                field.TypeText(value);
            }
        }
        
        [When(@"I click the (.*) button")]
        public void WhenIClickTheLoginButton(string buttonId)
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

        [Then(@"there should be a create button")]
        public void ThenThereShouldBeACreateButton()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"no login button")]
        public void ThenNoLoginButton()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
