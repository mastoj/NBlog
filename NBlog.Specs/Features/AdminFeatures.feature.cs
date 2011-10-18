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
    [NUnit.Framework.DescriptionAttribute("The admin area should have all the necessary links and functionality")]
    public partial class TheAdminAreaShouldHaveAllTheNecessaryLinksAndFunctionalityFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "AdminFeatures.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "The admin area should have all the necessary links and functionality", "To be able to administrate the blog\r\nAs a logged in user\r\nI should be able to edi" +
                    "t, update and delete content", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("I should have links to all the relevant areas")]
        [NUnit.Framework.CategoryAttribute("LoggedIn")]
        public virtual void IShouldHaveLinksToAllTheRelevantAreas()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("I should have links to all the relevant areas", new string[] {
                        "LoggedIn"});
#line 7
this.ScenarioSetup(scenarioInfo);
#line 8
 testRunner.Given("I am on the admin page");
#line 9
 testRunner.Then("there should be a pages link");
#line 10
 testRunner.And("there should be a posts link");
#line 11
 testRunner.And("there should be a comments link");
#line 12
 testRunner.And("there should be a configuration link");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("When logged in the logged in user should be able to create a post")]
        [NUnit.Framework.CategoryAttribute("LoggedIn")]
        public virtual void WhenLoggedInTheLoggedInUserShouldBeAbleToCreateAPost()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("When logged in the logged in user should be able to create a post", new string[] {
                        "LoggedIn"});
#line 15
this.ScenarioSetup(scenarioInfo);
#line 16
 testRunner.Given("I am on the create post page");
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "InputField",
                        "DataType",
                        "Input"});
            table1.AddRow(new string[] {
                        "Title",
                        "string",
                        "Demo title"});
            table1.AddRow(new string[] {
                        "ShortUrl",
                        "longstring",
                        "demopost"});
            table1.AddRow(new string[] {
                        "Post",
                        "string",
                        "Demo content"});
            table1.AddRow(new string[] {
                        "PublishDate",
                        "datetime",
                        "2011-10-01"});
            table1.AddRow(new string[] {
                        "Publish",
                        "bool",
                        "true"});
            table1.AddRow(new string[] {
                        "Tags",
                        "string",
                        "tag1 tag2"});
            table1.AddRow(new string[] {
                        "Categories",
                        "string",
                        "cat1 cat2"});
#line 17
 testRunner.When("I enter the following information", ((string)(null)), table1);
#line 26
 testRunner.And("I click the save button");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "InputField",
                        "DataType",
                        "Value"});
            table2.AddRow(new string[] {
                        "Title",
                        "string",
                        "Demo title"});
            table2.AddRow(new string[] {
                        "ShortUrl",
                        "string",
                        "demopost"});
            table2.AddRow(new string[] {
                        "Post",
                        "string",
                        "Demo content"});
            table2.AddRow(new string[] {
                        "PublishDate",
                        "datetime",
                        "2011-10-01"});
            table2.AddRow(new string[] {
                        "Publish",
                        "bool",
                        "true"});
            table2.AddRow(new string[] {
                        "Tags",
                        "list",
                        "tag1 tag2"});
            table2.AddRow(new string[] {
                        "Categories",
                        "list",
                        "cat1 cat2"});
#line 27
 testRunner.Then("a post should exist with the data", ((string)(null)), table2);
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#endregion
