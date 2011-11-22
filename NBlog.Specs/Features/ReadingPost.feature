Feature: As a user I should be able to read published posts

Scenario: Only published posts should be listed on the start page
	Given the following posts exists
		| Title       | ShortUrl  | Content       | Excerpt  | PublishDate | Publish | Tags             | Categories |
		| Demo title  | demopost  | Demo content2 | Excerpt1 | 2011-10-01  | true    | tag1, tag3       | cat1, cat2 |
		| Demo title2 | demopost2 | Demo content3 | Excerpt2 | 2011-10-02  | false   | tag1, tag2       | cat1       |
		| Demo title3 | demopost3 | Demo content4 | Excerpt3 | 2011-10-03  | true    | tag1, tag2, tag4 | cat2       |
		| Demo title4 | demopost4 | Demo content5 | Excerpt4 | Today+1     | true    | tag1             | cat3, cat2 |
	When I am on the "start page"
	Then I should see the following list of posts in this order
		| Title       | ShortUrl  | Content       | Excerpt  | PublishDate | Publish | Tags             | Categories |
		| Demo title3 | demopost3 | Demo content4 | Excerpt3 | 2011-10-03  | true    | tag1, tag2, tag4 | cat2       |
		| Demo title  | demopost  | Demo content2 | Excerpt1 | 2011-10-01  | true    | tag1, tag3       | cat1, cat2 |

Scenario: Post should have their own page
	Given the following posts exists
		| Title       | ShortUrl  | Content       | Excerpt  | PublishDate | Publish | Tags             | Categories |
		| Demo title  | demopost  | Demo content2 | Excerpt1 | 2011-10-01  | true    | tag1, tag3       | cat1, cat2 |
	When I am on the post with the "demopost" adress
	Then I should see the post
		| Title       | ShortUrl  | Content       | Excerpt  | PublishDate | Publish | Tags             | Categories |
		| Demo title  | demopost  | Demo content2 | Excerpt1 | 2011-10-01  | true    | tag1, tag3       | cat1, cat2 |

