Feature: The admin area should have all the necessary links and functionality 
	To be able to administrate the blog
	As a logged in user
	I should be able to edit, update and delete content

@LoggedIn
Scenario: I should have links to all the relevant areas
	Given I am on the admin page
	Then there should be a pages link
	And there should be a posts link
	And there should be a comments link
	And there should be a configuration link
