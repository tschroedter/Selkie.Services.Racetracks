﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:1.9.0.77
//      SpecFlow Generator Version:1.9.0.0
//      Runtime Version:4.0.30319.42000
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------

#region Designer generated code

using TechTalk.SpecFlow;

#pragma warning disable

namespace Selkie.Services.Racetracks.SpecFlow
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "1.9.0.77")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("RacetracksService")]
    public partial class RacetracksServiceFeature
    {
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }

        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }

        private static TechTalk.SpecFlow.ITestRunner testRunner;

        [NUnit.Framework.TestFixtureSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            var featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"),
                                                                "RacetracksService",
                                                                "",
                                                                ProgrammingLanguage.CSharp,
                                                                ( ( string[] ) ( null ) ));
            testRunner.OnFeatureStart(featureInfo);
        }

        [NUnit.Framework.TestFixtureTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
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
        [NUnit.Framework.DescriptionAttribute("Calculating CostMatrix sends CostMatrixResponseMessage")]
        public virtual void CalculatingCostMatrixSendsCostMatrixResponseMessage()
        {
            var scenarioInfo =
                new TechTalk.SpecFlow.ScenarioInfo("Calculating CostMatrix sends CostMatrixResponseMessage",
                                                   ( ( string[] ) ( null ) ));
#line 36
            this.ScenarioSetup(scenarioInfo);
#line 37
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 38
            testRunner.And("Did not receive CostMatrixResponseMessage",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 39
            testRunner.When("I send a CostMatrixCalculateMessage",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "When ");
#line 40
            testRunner.Then("the CostMatrixResponseMessage contains the racetracks",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CostMatrixCalculate request and response")]
        public virtual void CostMatrixCalculateRequestAndResponse()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CostMatrixCalculate request and response",
                                                                  ( ( string[] ) ( null ) ));
#line 30
            this.ScenarioSetup(scenarioInfo);
#line 31
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 32
            testRunner.And("Did not receive CostMatrixResponseMessage",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 33
            testRunner.When("I send a CostMatrixCalculateMessage",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "When ");
#line 34
            testRunner.Then("the result should be that I received a CostMatrixResponseMessage",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("CostMatrix request and response")]
        public virtual void CostMatrixRequestAndResponse()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("CostMatrix request and response",
                                                                  ( ( string[] ) ( null ) ));
#line 24
            this.ScenarioSetup(scenarioInfo);
#line 25
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 26
            testRunner.And("Did not receive CostMatrixResponseMessage",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 27
            testRunner.When("I send a CostMatrixRequestMessage",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "When ");
#line 28
            testRunner.Then("the result should be that I received a CostMatrixResponseMessage",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Ping RacetracksService")]
        public virtual void PingRacetracksService()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Ping RacetracksService",
                                                                  ( ( string[] ) ( null ) ));
#line 3
            this.ScenarioSetup(scenarioInfo);
#line 4
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 5
            testRunner.And("Did not receive ping response message",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 6
            testRunner.When("I send a ping message",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "When ");
#line 7
            testRunner.Then("the result should be a ping response message",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("RacetracksGetMessage request and response")]
        public virtual void RacetracksGetMessageRequestAndResponse()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("RacetracksGetMessage request and response",
                                                                  ( ( string[] ) ( null ) ));
#line 42
            this.ScenarioSetup(scenarioInfo);
#line 43
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 44
            testRunner.And("Did not receive CostMatrixResponseMessage",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 45
            testRunner.When("I send a CostMatrixCalculateMessage",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "When ");
#line 46
            testRunner.Then("the CostMatrixResponseMessage contains the racetracks",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line 47
            testRunner.Given("Did not receive RacetracksResponseMessage",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 48
            testRunner.When("I send a RacetracksGetMessage",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "When ");
#line 49
            testRunner.Then("the result should be that I received a RacetracksResponseMessage",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Starting service sends message")]
        public virtual void StartingServiceSendsMessage()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Starting service sends message",
                                                                  ( ( string[] ) ( null ) ));
#line 20
            this.ScenarioSetup(scenarioInfo);
#line 21
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 22
            testRunner.Then("the result should be that I received a ServiceStartedMessage",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Stopping service sends message")]
        public virtual void StoppingServiceSendsMessage()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Stopping service sends message",
                                                                  ( ( string[] ) ( null ) ));
#line 15
            this.ScenarioSetup(scenarioInfo);
#line 16
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 17
            testRunner.When("I send a stop message",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "When ");
#line 18
            testRunner.Then("the result should be that I received a ServiceStoppedMessage",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }

        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Stop service")]
        public virtual void StopService()
        {
            var scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Stop service",
                                                                  ( ( string[] ) ( null ) ));
#line 9
            this.ScenarioSetup(scenarioInfo);
#line 10
            testRunner.Given("Service is running",
                             ( ( string ) ( null ) ),
                             ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                             "Given ");
#line 11
            testRunner.And("Did not receive ping response message",
                           ( ( string ) ( null ) ),
                           ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                           "And ");
#line 12
            testRunner.When("I send a stop message",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "When ");
#line 13
            testRunner.Then("the result should be service not running",
                            ( ( string ) ( null ) ),
                            ( ( TechTalk.SpecFlow.Table ) ( null ) ),
                            "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}

#pragma warning restore

#endregion