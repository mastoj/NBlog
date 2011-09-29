﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.7.1.0
//      SpecFlow Generator Version:1.7.0.0
//      Runtime Version:4.0.30319.237
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
namespace NBlog.Specs.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.7.1.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("The site should have all the pages set up and it should be possible to navigate t" +
        "o them.")]
    public partial class TheSiteShouldHaveAllThePagesSetUpAndItShouldBePossibleToNavigateToThem_Feature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "Navigation.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "The site should have all the pages set up and it should be possible to navigate t" +
                    "o them.", "", ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Anonymous user can access start page")]
        [NUnit.Framework.CategoryAttribute("NotLoggedIn")]
        public virtual void AnonymousUserCanAccessStartPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Anonymous user can access start page", new string[] {
                        "NotLoggedIn"});
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
 testRunner.When("I navigate to the start page");
#line 6
 testRunner.Then("I should get a successful response");
#line 7
 testRunner.And("it should have a title");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Anonymous user can access login page")]
        [NUnit.Framework.CategoryAttribute("NotLoggedIn")]
        public virtual void AnonymousUserCanAccessLoginPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Anonymous user can access login page", new string[] {
                        "NotLoggedIn"});
#line 10
this.ScenarioSetup(scenarioInfo);
#line 11
 testRunner.When("I navigate to the login page");
#line 12
 testRunner.Then("I should get a successful response");
#line 13
 testRunner.And("it should have a title");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Logged in user get redirected from the login page")]
        [NUnit.Framework.CategoryAttribute("LoggedInAsAdmin")]
        public virtual void LoggedInUserGetRedirectedFromTheLoginPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Logged in user get redirected from the login page", new string[] {
                        "LoggedInAsAdmin"});
#line 16
this.ScenarioSetup(scenarioInfo);
#line 17
 testRunner.When("I navigate to the login page");
#line 18
 testRunner.Then("I should be redirected to the start page");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Anonymous user get redirected to create admin page if no user exists")]
        [NUnit.Framework.CategoryAttribute("NotLoggedIn")]
        public virtual void AnonymousUserGetRedirectedToCreateAdminPageIfNoUserExists()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Anonymous user get redirected to create admin page if no user exists", new string[] {
                        "NotLoggedIn"});
#line 21
this.ScenarioSetup(scenarioInfo);
#line 22
 testRunner.Given("it doesn\'t exist a user");
#line 23
 testRunner.When("I navigate to the login page");
#line 24
 testRunner.Then("I should be redirected to the create admin page");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Anonymous user get redirected to login page if no user exists")]
        [NUnit.Framework.CategoryAttribute("AdminUserExists")]
        [NUnit.Framework.CategoryAttribute("NotLoggedIn")]
        public virtual void AnonymousUserGetRedirectedToLoginPageIfNoUserExists()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Anonymous user get redirected to login page if no user exists", new string[] {
                        "AdminUserExists",
                        "NotLoggedIn"});
#line 28
this.ScenarioSetup(scenarioInfo);
#line 29
 testRunner.When("I navigate to the create admin page");
#line 30
 testRunner.Then("I should be redirected to the login page");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#endregion
