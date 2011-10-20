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

@LoggedIn
@NoPosts
Scenario: When logged in the logged in user should be able to create a post
	Given I am on the create post page
	When I enter the following information
		| InputField  | DataType   | Input        |
		| Title       | string     | Demo title   |
		| ShortUrl    | longstring | demopost     |
		| Content     | string     | Demo content |
		| PublishDate | datetime   | 2011-10-01   |
		| Publish     | bool       | true         |
		| Tags        | string     | tag1 tag2    |
		| Categories  | string     | cat1 cat2    |
	And I click the save button
	When I navigate to the admin page
	Then I should find a list of posts with one entry
	And it contains the string "Demo title"
	And it contains the string "demopost"
