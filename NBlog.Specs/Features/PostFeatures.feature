Feature: Post creation and editing
	In order to show user blog posts
	As a logged in user
	I should be able to create and edit posts

@NoPosts
@Authenticated
Scenario: When logged in the logged in user should be able to create and edit a post
	Given I am on the "create post page"
	When I enter the following information
		| InputField  | DataType   | Input               |
		| Title       | string     | Demo title          |
		| ShortUrl    | longstring | demopost            |
		| Content     | string     | Demo content        |
		| PublishDate | datetime   | 2011-10-01          |
		| Publish     | bool       | true                |
		| Excerpt     | string     | This is the excerpt |
		| Tags        | string     | tag1, tag2          |
		| Categories  | string     | cat1, cat2          |
	And I click the "save" button
	Then a post with the following content should have been created
		| Title      | ShortUrl | Content      | PublishDate | Publish | Excerpt             | Tags       | Categories |
		| Demo title | demopost | Demo content | 2011-10-01  | true    | This is the excerpt | tag1, tag2 | cat1, cat2 |
	And I should see a success message
	