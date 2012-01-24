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
	Then a post with the following meta data should have been created
		| Title      | ShortUrl | Excerpt             | Tags       | Categories | LastUpdateDate |
		| Demo title | demopost | This is the excerpt | tag1, tag2 | cat1, cat2 | Today          |
	And the following post versions
		| Content      | SaveDate |
		| Demo content | Today    |
	And no published posts
	And I should see a success message

@NoPosts
@Authenticated
Scenario: Create and publish post
	Given I am on the "create post page"
	When I enter the following information
		| InputField | DataType   | Input               |
		| Title      | string     | Demo title          |
		| ShortUrl   | string     | demopost            |
		| Content    | longstring | Demo content        |
		| Excerpt    | string     | This is the excerpt |
		| Tags       | string     | tag1, tag2          |
		| Categories | string     | cat1, cat2          |
	And I click the "publish" button
	Then a post with the following meta data should have been created
		| ShortUrl | Excerpt             | Tags       | Categories | LastUpdateDate |
		| demopost | This is the excerpt | tag1, tag2 | cat1, cat2 | Today          |
	And the following post versions
		| Title       | Content      | SaveDate |
		| Demot title | Demo content | Today    |
	And with the following published post
		| Title       | PublishDate | Content      | SaveDate |
		| Demot title | Today       | Demo content | Today    |
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
	Then a post with the following meta data should have been created
		| ShortUrl | Excerpt             | Tags       | Categories | LastUpdateDate |
		| demopost | This is the excerpt | tag1, tag2 | cat1, cat2 | Today          |
	And the following post versions
		| Title       | Content      | SaveDate |
		| Demot title | Demo content | Today    |
	And with the following published post
		| Title      | PublishDate | Content      | SaveDate |
		| Demo title | 2011-10-01  | Demo content | Today    |
	And I should see a success message

@NoPosts
@Authenticated
Scenario: Creating a post with existing short url should fail
	Given a post exist with the following versions
		| ShortUrl | PublishDate | Excerpt             | Tags       | Categories | LastUpdateDate |
		| demopost | 2011-10-01  | This is the excerpt | tag1, tag2 | cat1, cat2 | Today          |
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
		| ShortUrl | PublishDate | Excerpt             | Tags       | Categories | LastUpdateDate |
		| demopost | 2011-10-01  | This is the excerpt | tag1, tag2 | cat1, cat2 | Today          |

@NoPosts
@Authenticated
Scenario: Updating unpublished post
	Given the following post
		| ShortUrl | PublishDate | Excerpt             | Tags       | Categories | LastUpdateDate |
		| demopost | 2011-10-01  | This is the excerpt | tag1, tag2 | cat1, cat2 | Today          |
	And the following post versions
		| Title      | Content       | SaveDate   |
		| Demo title | Demo content  | 2011-10-02 |
		| Demo title | Demo content1 | 2011-10-03 |
	And with the following published post
		| Title      | Content      | SaveDate   |
		| Demo title | Demo content | 2011-10-02 |
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
		| ShortUrl | PublishDate | Excerpt             | Tags       | Categories | LastUpdateDate |
		| demopost | 2011-10-01  | This is the excerpt | tag1, tag2 | cat1, cat2 | Today          |
	And the following post versions
		| Title       | Content       | SaveDate   |
		| Demo title  | Demo content1 | 2011-10-02 |
		| Demo title2 | Demo content2 | Today      |
	And with the following published post
		| Content       | SaveDate   |
		| Demo content1 | 2011-10-02 |

@NoPosts
@Authenticated
Scenario: Publishing unpublished post
	Given the following post
		| ShortUrl | PublishDate | Excerpt             | Tags       | Categories | LastUpdateDate |
		| demopost | 2011-10-01  | This is the excerpt | tag1, tag2 | cat1, cat2 | Today          |
	And the following post versions
		| Title      | Content       | SaveDate   |
		| Demo title | Demo content  | 2011-10-02 |
		| Demo title | Demo content1 | 2011-10-03 |
	And with the following published post
		| Title      | Content      | SaveDate   |
		| Demo title | Demo content | 2011-10-02 |
	And I am on the "edit post page" for "demopost"
	And I click the "publish" button
	Then I should have the following post versions for post with short url "demopost"
		| ShortUrl | PublishDate | Excerpt             | Tags       | Categories | LastUpdateDate |
		| demopost | 2011-10-01  | This is the excerpt | tag1, tag2 | cat1, cat2 | Today          |
	And the following post versions
		| Title      | Content       | SaveDate   |
		| Demo title | Demo content  | 2011-10-02 |
		| Demo title | Demo content1 | Today      |
	And with the following published post
		| Title      | Content       | SaveDate |
		| Demo title | Demo content1 | Today    |
