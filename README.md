# posts-api

CRUD based API to interact with users posts, created using C# and MongoDB.

Create, Read, Update and Delete posts including its title, date, author and body.

API Endpoints:

	- GET:
	
		- /posts : Get all posts.
	
		- /posts/{title} : Get an unique post based on its title.
		
		- /posts/author/{name} : Get a list of posts based on its author.
		
	- POST:
	
		- /posts : Add a new post to the database.
		
	- PUT:
	
	- /posts/{title} : Update a post based on its title.
	
	- DELETE:
	
	- /posts/{title} : Delete a post based on its title.