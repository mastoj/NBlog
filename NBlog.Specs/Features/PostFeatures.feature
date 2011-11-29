Feature: Post creation and editing
	In order to show user blog posts
	As a logged in user
	I should be able to create and edit posts

@NoPosts
@Authenticated
Scenario: Create post
	Given I am on the "create post page"
	When I enter the following information
		| InputField  | DataType   | Input               |
		| Title       | string     | Demo title          |
		| ShortUrl    | longstring | demopost            |
		| Content     | string     | Demo content        |
		| Excerpt     | string     | This is the excerpt |
		| Tags        | string     | tag1, tag2          |
		| Categories  | string     | cat1, cat2          |
	And I click the "save" button
	Then a post with the following content should have been created
		| Title      | Version | ShortUrl | Content      | PublishDate | Published | Excerpt             | Tags       | Categories | LastUpdateDate |
		| Demo title | 1       | demopost | Demo content | Today       | false   | This is the excerpt | tag1, tag2 | cat1, cat2 | Today          |
	And I should see a success message

@NoPosts
@Authenticated
Scenario: Create and publish post
	Given I am on the "create post page"
	When I enter the following information
		| InputField  | DataType   | Input               |
		| Title       | string     | Demo title          |
		| ShortUrl    | longstring | demopost            |
		| Content     | string     | Demo content        |
		| Excerpt     | string     | This is the excerpt |
		| Tags        | string     | tag1, tag2          |
		| Categories  | string     | cat1, cat2          |
	And I click the "publish" button
	Then a post with the following content should have been created
		| Title      | Version | ShortUrl | Content      | PublishDate | Publish | Excerpt             | Tags       | Categories | LastUpdateDate |
		| Demo title | 1       | demopost | Demo content | Today       | true    | This is the excerpt | tag1, tag2 | cat1, cat2 | Today          |
	And I should see a success message

@NoPosts
@Authenticated
Scenario: Create and publish post back in time
	Given I am on the "create post page"
	When I enter the following information
		| InputField  | DataType   | Input               |
		| Title       | string     | Demo title          |
		| ShortUrl    | longstring | demopost            |
		| Content     | string     | Demo content        |
		| Excerpt     | string     | This is the excerpt |
		| PublishDate | datetime   | 2011-10-01          |
		| Tags        | string     | tag1, tag2          |
		| Categories  | string     | cat1, cat2          |
	And I click the "publish" button
	Then a post with the following content should have been created
		| Title      | Version | ShortUrl | Content      | PublishDate | Publish | Excerpt             | Tags       | Categories | LastUpdateDate |
		| Demo title | 1       | demopost | Demo content | 2011-10-01  | true    | This is the excerpt | tag1, tag2 | cat1, cat2 | Today          |
	And I should see a success message

@NoPosts
@Authenticated
Scenario: Creating a post with existing short url should fail
	Given a post exist with the following versions
		| Version | Title      | ShortUrl | Content       | Excerpt  | PublishDate | Publish | Tags       | Categories | LastUpdatedDate |
		| 1       | Demo title | demopost | Demo content2 | Excerpt1 | 2011-10-01  | false   | tag1, tag3 | cat1, cat2 | 2011-10-02      |
	And I am on the "create post page"
	When I enter the following information
		| InputField  | DataType   | Input               |
		| Title       | string     | Demo title          |
		| ShortUrl    | longstring | demopost            |
		| Content     | string     | Demo content        |
		| Excerpt     | string     | This is the excerpt |
		| PublishDate | datetime   | 2011-10-01          |
		| Tags        | string     | tag1, tag2          |
		| Categories  | string     | cat1, cat2          |
	And I click the "save" button
	Then I should see an error message
	And the following post version should exist
		| Version | Title      | ShortUrl | Content       | Excerpt  | PublishDate | Publish | Tags       | Categories | LastUpdatedDate |
		| 1       | Demo title | demopost | Demo content2 | Excerpt1 | 2011-10-01  | false   | tag1, tag3 | cat1, cat2 | 2011-10-02      |

@NoPosts
@Authenticated
Scenario: Updating unpublished post
	Given a post exist with the following versions
		| Version | Title      | ShortUrl | Content       | Excerpt  | PublishDate | Publish | Tags       | Categories | LastUpdatedDate |
		| 1       | Demo title | demopost | Demo content2 | Excerpt1 | 2011-10-01  | true    | tag1, tag3 | cat1, cat2 | 2011-10-02      |
		| 2       | Demo title | demopost | Demo content2 | Excerpt1 | 2011-10-01  | false   | tag1, tag3 | cat1, cat2 | 2011-10-03      |
	And I am on the "edit post page" for "demopost"
	And I enter the following information
		| InputField  | DataType   | Input                |
		| Title       | string     | Demo title2          |
		| ShortUrl    | longstring | demopost             |
		| Content     | string     | Demo content2        |
		| Excerpt     | string     | This is the excerpt2 |
		| PublishDate | datetime   | 2011-10-01           |
		| Tags        | string     | tag1, tag3           |
		| Categories  | string     | cat3, cat2           |
	And I click the "save" button
	Then I should have the following post versions for post with short url "demopost"
		| Version | Title       | ShortUrl | Content       | Excerpt              | PublishDate | Publish | Tags       | Categories | LastUpdatedDate |
		| 1       | Demo title  | demopost | Demo content2 | Excerpt1             | 2011-10-01  | true    | tag1, tag3 | cat1, cat2 | 2011-10-02      |
		| 2       | Demo title2 | demopost | Demo content2 | This is the excerpt2 | 2011-10-01  | false   | tag1, tag3 | cat3, cat2 | Today           |

@NoPosts
@Authenticated
Scenario: Publishing unpublished post
	Given a post exist with the following versions
		| Version | Title      | ShortUrl | Content       | Excerpt  | PublishDate | Publish | Tags       | Categories | LastUpdatedDate |
		| 1       | Demo title | demopost | Demo content2 | Excerpt1 | 2011-10-01  | true    | tag1, tag3 | cat1, cat2 | 2011-10-02      |
		| 2       | Demo title | demopost | Demo content2 | Excerpt1 | 2011-10-01  | false   | tag1, tag3 | cat1, cat2 | 2011-10-03      |
	And I am on the "edit post page" for "demopost"
	And I enter the following information
		| InputField  | DataType   | Input                |
		| Title       | string     | Demo title2          |
		| ShortUrl    | longstring | demopost             |
		| Content     | string     | Demo content2        |
		| Excerpt     | string     | This is the excerpt2 |
		| PublishDate | datetime   | 2011-10-03           |
		| Tags        | string     | tag1, tag3           |
		| Categories  | string     | cat3, cat2           |
	And I click the "publish" button
	Then I should have the following post versions for post with short url "demopost"
		| Version | Title       | ShortUrl | Content       | Excerpt              | PublishDate | Publish | Tags       | Categories | LastUpdatedDate |
		| 1       | Demo title  | demopost | Demo content2 | Excerpt1             | 2011-10-01  | true    | tag1, tag3 | cat1, cat2 | 2011-10-02      |
		| 2       | Demo title2 | demopost | Demo content2 | This is the excerpt2 | 2011-10-03  | true    | tag1, tag3 | cat3, cat2 | Today           |
