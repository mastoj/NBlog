using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace NBlog.Specs.Steps
{
    [Binding]
    public class FormSteps
    {
        [When(@"enter the following information")]
        public void WhenEnterTheFollowingInformation(Table table)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I click the (.*) button")]
        public void WhenIClickTheLoginButton(string button)
        {
            ScenarioContext.Current.Pending();
        }
    
        [Then(@"I should see a error message")]
        public void ThenIShouldSeeAErrorMessage()
        {
            ScenarioContext.Current.Pending();
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
