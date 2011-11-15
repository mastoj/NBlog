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
    [NUnit.Framework.DescriptionAttribute("As a user I should be able to read posts")]
    public partial class AsAUserIShouldBeAbleToReadPostsFeature
    {
        
        private static TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "ReadingPost.feature"
#line hidden
        
        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "As a user I should be able to read posts", "", ProgrammingLanguage.CSharp, ((string[])(null)));
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
        [NUnit.Framework.DescriptionAttribute("Post should be listed on the start page")]
        public virtual void PostShouldBeListedOnTheStartPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post should be listed on the start page", ((string[])(null)));
#line 3
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table1 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title",
                        "ShortUrl",
                        "Content",
                        "Excerpt",
                        "PublishDate",
                        "Publish",
                        "Tags",
                        "Categories"});
            table1.AddRow(new string[] {
                        "Demo title",
                        "demopost",
                        "Demo content2",
                        "Excerpt1",
                        "2011-10-01",
                        "true",
                        "tag1, tag3",
                        "cat1, cat2"});
            table1.AddRow(new string[] {
                        "Demo title2",
                        "demopost2",
                        "Demo content3",
                        "Excerpt2",
                        "2011-10-02",
                        "false",
                        "tag1, tag2",
                        "cat1"});
            table1.AddRow(new string[] {
                        "Demo title3",
                        "demopost3",
                        "Demo content4",
                        "Excerpt3",
                        "2011-10-03",
                        "true",
                        "tag1, tag2, tag4",
                        "cat2"});
            table1.AddRow(new string[] {
                        "Demo title4",
                        "demopost4",
                        "Demo content5",
                        "Excerpt4",
                        "Today+1",
                        "true",
                        "tag1",
                        "cat3, cat2"});
#line 4
 testRunner.Given("the following posts exists", ((string)(null)), table1);
#line 10
 testRunner.When("I am on the \"start page\"");
#line hidden
            TechTalk.SpecFlow.Table table2 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title",
                        "ShortUrl",
                        "Content",
                        "Excerpt",
                        "PublishDate",
                        "Publish",
                        "Tags",
                        "Categories"});
            table2.AddRow(new string[] {
                        "Demo title",
                        "demopost",
                        "Demo content2",
                        "Excerpt1",
                        "2011-10-01",
                        "true",
                        "tag1, tag3",
                        "cat1, cat2"});
            table2.AddRow(new string[] {
                        "Demo title3",
                        "demopost3",
                        "Demo content4",
                        "Excerpt3",
                        "2011-10-03",
                        "true",
                        "tag1, tag2, tag4",
                        "cat2"});
#line 11
 testRunner.Then("I should see the following list of posts", ((string)(null)), table2);
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Post should have their own page")]
        public virtual void PostShouldHaveTheirOwnPage()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Post should have their own page", ((string[])(null)));
#line 16
this.ScenarioSetup(scenarioInfo);
#line hidden
            TechTalk.SpecFlow.Table table3 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title",
                        "ShortUrl",
                        "Content",
                        "Excerpt",
                        "PublishDate",
                        "Publish",
                        "Tags",
                        "Categories"});
            table3.AddRow(new string[] {
                        "Demo title",
                        "demopost",
                        "Demo content2",
                        "Excerpt1",
                        "2011-10-01",
                        "true",
                        "tag1, tag3",
                        "cat1, cat2"});
#line 17
 testRunner.Given("the following posts exists", ((string)(null)), table3);
#line 20
 testRunner.When("I am on the post with the \"demopost\" adress");
#line hidden
            TechTalk.SpecFlow.Table table4 = new TechTalk.SpecFlow.Table(new string[] {
                        "Title",
                        "ShortUrl",
                        "Content",
                        "Excerpt",
                        "PublishDate",
                        "Publish",
                        "Tags",
                        "Categories"});
            table4.AddRow(new string[] {
                        "Demo title",
                        "demopost",
                        "Demo content2",
                        "Excerpt1",
                        "2011-10-01",
                        "true",
                        "tag1, tag3",
                        "cat1, cat2"});
#line 21
 testRunner.Then("I should see the post", ((string)(null)), table4);
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#endregion
