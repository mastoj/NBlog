Feature: The site should have all the pages set up and it should be possible to navigate to them.

Scenario: Anonymous user can access start page
	When I navigate to the start page
	Then I should get a successful response
	And it should have a title

Scenario: Anonymous user can access login page
	When I navigate to the login page
	Then I should get a successful response
	And it should have a title

@AdminUserExists
Scenario: Anonymous user get redirected to create admin page if no user exists
	Given it doesn't exist a user
	When I navigate to the login page
	Then I should be re-directed to the create admin page
