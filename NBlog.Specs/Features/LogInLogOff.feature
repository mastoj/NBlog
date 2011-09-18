Feature: As a user with an account I should be able to log in and log off

Scenario: Setup initial account
	Given I am not logged in
	And it doesn't exist a user
	Then there should be a create button
	And no login button
	When I navigate to the login page
	And enter the following information
		| InputField | Input    |
		| userName   | admin    |
		| password   | asdf1234 |
		| name		 | tomas	|
	And I click the create button
	Then I should be re-directed to the start page
	And there should be a log off link

Scenario: Log in successful
	Given I am not logged in
	And it exist an account with the credentials
		| UserName | Password |
		| admin    | asdf1234 |
	When I navigate to the login page
	And enter the following information
		| InputField | Input    |
		| userName   | admin    |
		| password   | asdf1234 |
	And I click the login button
	Then I should be re-directed to the start page
	And there should be a log off link

Scenario: Log in unsuccessful
	Given I am not logged in
	And it exist an account with the credentials
		| UserName | Password |
		| admin    | asdf1234 |
	When I navigate to the login page
	And enter the following information
		| InputField | Input    |
		| userName   | admin    |
		| password   | asdf1232 |
	And I click the login button
	Then I should see a error message
	