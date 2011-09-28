Feature: As a user with an account I should be able to log in and log off

@NotLoggedIn
Scenario: Setup initial account
	Given it doesn't exist a user
	When I navigate to the login page
	Then there should be a create button
	And no log in button
	When I enter the following information
		| InputField           | Input    |
		| UserName             | admin    |
		| Password             | asdf1234 |
		| PasswordConfirmation | asdf1234 |
		| Name                 | tomas    |
	And I click the create button
	Then I should be redirected to the admin page
	And there should be a log off link
	
@NotLoggedIn
Scenario: Log in successful
	Given it exist an account with the credentials
		| UserName | Password | Name  |
		| admin    | asdf1234 | Tomas |
	When I navigate to the login page
	And I enter the following information
		| InputField | Input    |
		| UserName   | admin    |
		| Password   | asdf1234 |
	And I click the log in button
	Then I should be redirected to the admin page
	And there should be a log off link

@NotLoggedIn
Scenario: Log in unsuccessful
	Given it exist an account with the credentials
		| UserName | Password | Name  |
		| admin    | asdf1234 | Tomas |
	When I navigate to the login page
	And I enter the following information
		| InputField | Input    |
		| UserName   | admin    |
		| Password   | asdf1232 |
	And I click the log in button
	Then I should see a error message
	