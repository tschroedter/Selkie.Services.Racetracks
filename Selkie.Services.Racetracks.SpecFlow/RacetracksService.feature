Feature: RacetracksService
 
Scenario: Ping RacetracksService
	Given Service is running
	And Did not receive ping response message
	When I send a ping message
	Then the result should be a ping response message

Scenario: Stop service
	Given Service is running
	And Did not receive ping response message
	When I send a stop message
	Then the result should be service not running

Scenario: Stopping service sends message
	Given Service is running
	When I send a stop message
	Then the result should be that I received a ServiceStoppedMessage

Scenario: Starting service sends message
	Given Service is running
	Then the result should be that I received a ServiceStartedMessage

Scenario: CostMatrix request and response
	Given Service is running
	And Did not receive CostMatrixResponseMessage
	When I send a CostMatrixRequestMessage
	Then the result should be that I received a CostMatrixResponseMessage

Scenario: CostMatrixCalculate request and response
	Given Service is running
	And Did not receive CostMatrixResponseMessage
	When I send a CostMatrixCalculateMessage
	Then the result should be that I received a CostMatrixResponseMessage

Scenario: Calculating CostMatrix sends CostMatrixResponseMessage
	Given Service is running
	And Did not receive CostMatrixResponseMessage
	When I send a CostMatrixCalculateMessage
	Then the CostMatrixResponseMessage contains the racetracks

Scenario: RacetracksGetMessage request and response
	Given Service is running
	And Did not receive CostMatrixResponseMessage
	When I send a CostMatrixCalculateMessage
	Then the CostMatrixResponseMessage contains the racetracks
	Given Did not receive RacetracksResponseMessage
	When I send a RacetracksGetMessage
	Then the result should be that I received a RacetracksResponseMessage
