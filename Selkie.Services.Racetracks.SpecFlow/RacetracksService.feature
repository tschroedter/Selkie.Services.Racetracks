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
	And Did not receive CostMatrixChangedMessage
	When I send a CostMatrixGetMessage
	Then the result should be that I received a CostMatrixChangedMessage

Scenario: Lines set and changed
	Given Service is running
	And Did not receive LinesChangedMessage
	When I send a LinesSetMessage
	Then the result should be that I received a LinesChangedMessage

Scenario: RacetrackSettings set and changed
	Given Service is running
	And Did not receive RacetrackSettingsChangedMessage
	When I send a RacetrackSettingsSetMessage
	Then the result should be that I received a RacetrackSettingsChangedMessage

Scenario: CostMatrixCalculate set and changed
	Given Service is running
	And Did not receive CostMatrixChangedMessage
	When I send a CostMatrixCalculateMessage
	Then the result should be that I received a CostMatrixChangedMessage

Scenario: CostMatrix is calculated
	Given Service is running
	And Did not receive RacetrackSettingsChangedMessage
	And Did not receive LinesChangedMessage
	And Did not receive CostMatrixChangedMessage
	When I send a RacetrackSettingsSetMessage
	Then the result should be that I received a RacetrackSettingsChangedMessage
	When I send a LinesSetMessage
	Then the result should be that I received a LinesChangedMessage
	When I send a CostMatrixCalculateMessage
	Then the CostMatrixChangedMessage contains the racetracks

#	And Did not receive CostMatrixChangedMessage
#	When I send a RacetrackSettingsSetMessage
#	When I send a LinesSetMessage
#	Then the result should be that I received a LinesChangedMessage
#	When I send a CostMatrixCalculateMessage
#	Then the CostMatrixChangedMessage contains the racetracks
