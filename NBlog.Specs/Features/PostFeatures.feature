Feature: Post creation and editing
	In order to show user blog posts
	As a logged in user
	I should be able to create and edit posts

@LoggedIn
@NoPosts
Scenario: When logged in the logged in user should be able to create and edit a post
	Given I am on the create post page
	When I enter the following information
		| InputField  | DataType   | Input        |
		| Title       | string     | Demo title   |
		| ShortUrl    | longstring | demopost     |
		| Content     | string     | Demo content |
		| PublishDate | datetime   | 2011-10-01   |
		| Publish     | bool       | true         |
		| Tags        | string     | tag1, tag2   |
		| Categories  | string     | cat1, cat2   |
	And I click the save button
	When I navigate to the admin page
	Then I should find a list of posts with one entry
	And it contains the string "Demo title"
	And it contains the string "demopost"
	And it contains an edit post link to demopost
	And it contains an delete post link to demopost
	When I navigate to edit of demopost
	Then I should see the following pre-filled form
		| InputField  | DataType   | Input        |
		| Title       | string     | Demo title   |
		| ShortUrl    | longstring | demopost     |
		| Content     | string     | Demo content |
		| PublishDate | datetime   | 2011-10-01   |
		| Publish     | bool       | true         |
		| Tags        | string     | tag1, tag2   |
		| Categories  | string     | cat1, cat2   |
	When I enter the following information
		| InputField  | DataType   | Input         |
		| Title       | string     | Demo title2   |
		| ShortUrl    | longstring | demopost2     |
		| Content     | string     | Demo content2 |
		| PublishDate | datetime   | 2011-10-02    |
		| Publish     | bool       | false         |
		| Tags        | string     | tag2, tag3    |
		| Categories  | string     | cat2, cat3    |
	And I click the save button
	When I navigate to the admin page
	Then I should find a list of posts with one entry
	And it contains the string "Demo title"
	And it contains the string "demopost"
	And it contains an edit post link to demopost
	And it contains an delete post link to demopost
	When I navigate to edit of demopost
	Then I should see the following pre-filled form
		| InputField  | DataType   | Input         |
		| Title       | string     | Demo title2   |
		| ShortUrl    | longstring | demopost2     |
		| Content     | string     | Demo content2 |
		| PublishDate | datetime   | 2011-10-02    |
		| Publish     | bool       | false         |
		| Tags        | string     | tag2, tag3    |
		| Categories  | string     | cat2, cat3    |
	