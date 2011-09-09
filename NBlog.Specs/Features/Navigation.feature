Feature: The site should have all the pages set up and it should be possible to navigate to them.

Scenario: Anonymous user can access start page
	When I navigatae to the start page
	Then I should get a successful response
	And it should have a title
