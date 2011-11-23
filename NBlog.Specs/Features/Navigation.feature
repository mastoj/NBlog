Feature: The site should have all the pages set up and it should be possible to navigate to them.

@NotAuthenticated
Scenario: Anonymous user can access start page
	When I navigate to the "start page"
	Then I should get a successful response
	And it should have a title

@NotAuthenticated
Scenario: Anonymous user can access login page
	When I navigate to the "login page"
	Then I should get a successful response
	And it should have a title

@NotAuthenticated
Scenario: Logged in user get redirected from the login page
	Given I am logged in as the admin user
	When I navigate to the "login page"
	Then I should be redirected to the "start page"

@NotAuthenticated
Scenario: Anonymous user get redirected to create admin page if no user exists
	Given it doesn't exist a user
	When I navigate to the "login page"
	Then I should be redirected to the "create admin page"

@AdminUserExists
@NotAuthenticated
Scenario: Anonymous user get redirected to login page if user exists
	When I navigate to the "create admin page"
	Then I should be redirected to the "login page"

@AdminUserExists
@NotAuthenticated
Scenario: Anonymous user have no access to the admin area
	When I navigate to the "admin page"
	Then I should be redirected to the "login page"
