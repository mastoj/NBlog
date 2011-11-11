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
    [NUnit.Framework.DescriptionAttribute("Post creation and editing")]
    public partial class PostCreationAndEditingFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "PostFeatures.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "Post creation and editing", "In order to show user blog posts\r\nAs a logged in user\r\nI should be able to create" +
                    " and edit posts", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("When logged in the logged in user should be able to create and edit a post")]
        [NUnit.Framework.CategoryAttribute("LoggedIn")]
        [NUnit.Framework.CategoryAttribute("NoPosts")]
        public virtual void WhenLoggedInTheLoggedInUserShouldBeAbleToCreateAndEditAPost()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("When logged in the logged in user should be able to create and edit a post", new string[] {
                        "LoggedIn",
                        "NoPosts"});
#line 8
this.ScenarioSetup(scenarioInfo);
#line 9
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
                        "Content",
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
                        "tag1, tag2"});
            table1.AddRow(new string[] {
                        "Categories",
                        "string",
                        "cat1, cat2"});
#line 10
 testRunner.When("I enter the following information", ((string)(null)), table1);
#line 19
 testRunner.And("I click the save button");
#line 20
 testRunner.When("I navigate to the admin page");
#line 21
 testRunner.Then("I should find a list of posts with one entry");
#line 22
 testRunner.And("it contains the string \"Demo title\"");
#line 23
 testRunner.And("it contains the string \"demopost\"");
#line 24
 testRunner.And("it contains an edit post link to demopost");
#line 25
 testRunner.And("it contains an delete post link to demopost");
#line 26
 testRunner.When("I navigate to edit of demopost");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "InputField",
                        "DataType",
                        "Input"});
            table2.AddRow(new string[] {
                        "Title",
                        "string",
                        "Demo title"});
            table2.AddRow(new string[] {
                        "ShortUrl",
                        "longstring",
                        "demopost"});
            table2.AddRow(new string[] {
                        "Content",
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
                        "string",
                        "tag1, tag2"});
            table2.AddRow(new string[] {
                        "Categories",
                        "string",
                        "cat1, cat2"});
#line 27
 testRunner.Then("I should see the following pre-filled form", ((string)(null)), table2);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "InputField",
                        "DataType",
                        "Input"});
            table3.AddRow(new string[] {
                        "Title",
                        "string",
                        "Demo title2"});
            table3.AddRow(new string[] {
                        "ShortUrl",
                        "longstring",
                        "demopost2"});
            table3.AddRow(new string[] {
                        "Content",
                        "string",
                        "Demo content2"});
            table3.AddRow(new string[] {
                        "PublishDate",
                        "datetime",
                        "2011-10-02"});
            table3.AddRow(new string[] {
                        "Publish",
                        "bool",
                        "false"});
            table3.AddRow(new string[] {
                        "Tags",
                        "string",
                        "tag2, tag3"});
            table3.AddRow(new string[] {
                        "Categories",
                        "string",
                        "cat2, cat3"});
#line 36
 testRunner.When("I enter the following information", ((string)(null)), table3);
#line 45
 testRunner.And("I click the save button");
#line 46
 testRunner.When("I navigate to the admin page");
#line 47
 testRunner.Then("I should find a list of posts with one entry");
#line 48
 testRunner.And("it contains the string \"Demo title\"");
#line 49
 testRunner.And("it contains the string \"demopost\"");
#line 50
 testRunner.And("it contains an edit post link to demopost");
#line 51
 testRunner.And("it contains an delete post link to demopost");
#line 52
 testRunner.When("I navigate to edit of demopost");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "InputField",
                        "DataType",
                        "Input"});
            table4.AddRow(new string[] {
                        "Title",
                        "string",
                        "Demo title2"});
            table4.AddRow(new string[] {
                        "ShortUrl",
                        "longstring",
                        "demopost2"});
            table4.AddRow(new string[] {
                        "Content",
                        "string",
                        "Demo content2"});
            table4.AddRow(new string[] {
                        "PublishDate",
                        "datetime",
                        "2011-10-02"});
            table4.AddRow(new string[] {
                        "Publish",
                        "bool",
                        "false"});
            table4.AddRow(new string[] {
                        "Tags",
                        "string",
                        "tag2, tag3"});
            table4.AddRow(new string[] {
                        "Categories",
                        "string",
                        "cat2, cat3"});
#line 53
 testRunner.Then("I should see the following pre-filled form", ((string)(null)), table4);
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#endregion