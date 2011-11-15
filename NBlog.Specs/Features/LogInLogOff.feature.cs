﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.7.1.0
//      SpecFlow Generator Version:1.7.0.0
//      Runtime Version:4.0.30319.239
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
    [NUnit.Framework.DescriptionAttribute("As a user with an account I should be able to log in and log off")]
    public partial class AsAUserWithAnAccountIShouldBeAbleToLogInAndLogOffFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "LogInLogOff.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "As a user with an account I should be able to log in and log off", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Setup initial account")]
        public virtual void SetupInitialAccount()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Setup initial account", ((string[])(null)));
#line 3
this.ScenarioSetup(scenarioInfo);
#line 4
 testRunner.Given("it doesn\'t exist a user");
#line 5
 testRunner.When("I navigate to the \"login page\"");
#line 6
 testRunner.Then("there should be a \"create\" button");
#line 7
 testRunner.And("no \"log in\" button");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "InputField",
                        "Input"});
            table1.AddRow(new string[] {
                        "UserName",
                        "admin"});
            table1.AddRow(new string[] {
                        "Password",
                        "asdf1234"});
            table1.AddRow(new string[] {
                        "PasswordConfirmation",
                        "asdf1234"});
            table1.AddRow(new string[] {
                        "Name",
                        "tomas"});
#line 8
 testRunner.When("I enter the following information", ((string)(null)), table1);
#line 14
 testRunner.And("I click the \"create\" button");
#line 15
 testRunner.Then("I should be redirected to the \"admin page\"");
#line 16
 testRunner.And("there should be a \"log off\" link");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Log in successful")]
        [NUnit.Framework.CategoryAttribute("NotAuthenticated")]
        public virtual void LogInSuccessful()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Log in successful", new string[] {
                        "NotAuthenticated"});
#line 19
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "UserName",
                        "Password",
                        "Name"});
            table2.AddRow(new string[] {
                        "admin",
                        "asdf1234",
                        "Tomas"});
#line 20
 testRunner.Given("it exist an account with the credentials", ((string)(null)), table2);
#line 23
 testRunner.When("I navigate to the \"login page\"");
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "InputField",
                        "Input"});
            table3.AddRow(new string[] {
                        "UserName",
                        "admin"});
            table3.AddRow(new string[] {
                        "Password",
                        "asdf1234"});
#line 24
 testRunner.And("I enter the following information", ((string)(null)), table3);
#line 28
 testRunner.And("I click the \"log in\" button");
#line 29
 testRunner.Then("I should be redirected to the \"admin page\"");
#line 30
 testRunner.And("there should be a \"log off\" link");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Log in unsuccessful")]
        [NUnit.Framework.CategoryAttribute("NotAuthenticated")]
        public virtual void LogInUnsuccessful()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Log in unsuccessful", new string[] {
                        "NotAuthenticated"});
#line 33
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "UserName",
                        "Password",
                        "Name"});
            table4.AddRow(new string[] {
                        "admin",
                        "asdf1234",
                        "Tomas"});
#line 34
 testRunner.Given("it exist an account with the credentials", ((string)(null)), table4);
#line 37
 testRunner.When("I navigate to the \"login page\"");
#line hidden
            TechTalk.SpecFlow.Table table5 = new TechTalk.SpecFlow.Table(new string[] {
                        "InputField",
                        "Input"});
            table5.AddRow(new string[] {
                        "UserName",
                        "admin"});
            table5.AddRow(new string[] {
                        "Password",
                        "asdf1232"});
#line 38
 testRunner.And("I enter the following information", ((string)(null)), table5);
#line 42
 testRunner.And("I click the \"log in\" button");
#line 43
 testRunner.Then("I should see a error message");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#endregion
