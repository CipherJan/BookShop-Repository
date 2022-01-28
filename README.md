# Book-Shop Repository

The system consists of 3 microservices and MassTransit contract:
1. BookShop;
2. BookProvider;
3. BookExternalAPI;
4. BookContractLibrary.

The purpose of this system is to provide the backend component for bookstores with extensive functionality.

### System architecture:

![Book Shop sсheme v2](https://user-images.githubusercontent.com/85920527/142666308-0dd11e6e-297b-4061-912c-ee96c0fad01b.jpg "Integration schematic diagram:")

## BookShop

It is the main microservice that allows you to create and manage bookstores and stores books sold and sales history. Features are the following:
- Order new books from the supplier automatically or manually as needed;
- To set a sale on books that are considered obsolete. The discount depends on the book genre or it may not be available at all;
- Top up the balance sheet of the store for a specified amount. 
- The store allows you to view the current library of books and their purchases.

#### Technologies&Libs:
- MS SQL, Entity Framework;
- Quartz;
- RabbitMQ, MassTransit;
- Fluent Validator;
- Serilog with ElasticSearch.

#### Some explanations

- The library is updated automatically when the total number of books decreases to 10% of the original value equal to `MaxBookQuantity`;
- Books can only be of such genres as:
    -  Adventure,
    -  Drama,
    -  Encyclopedia,
    -  Fantastic,
    -  Novel.
- A book is considered obsolete if it was published more than a year ago;
- Discounts:
    - Adventure - 7%,
    - Encyclopedia - 10%,
    - Fantastic - 3%,
    - Other genres do not receive discounts;
- Acceptance of books is paid at the rate of 7% of their total cost.



## BookProvider

- Receives messages via RabbitMQ ordering new books;
- Makes a HTTP request to BookExternalAPI;
- Sends response via RabbitMQ with new books.

#### Technologies&Libs:
- RabbitMQ, MassTransit;
- Serilog, ElasticSearch.

## BookExternalAPI

This app provides a set of random books in response to an HTTP request.

## BookContractLibrary

This project contains MassTransit contract for communicating between BookShop and BookProvider.

NuGet package is published to [nuget org](https://www.nuget.org/packages/BookContractLibrary_from_BookShop/)

# Launch 

You can launch a service using either a Docker or with Visual Studio/JetBrains Rider IDE.

BookShop and BookProvider have MSSQL, RabbitMQ, Elasticsearch & Kibana as an externals dependency. 

You should have MSSQL, RabbitMQ, Elasticsearch & Kibana running somewhere. 

If your choose Docker method of running then external software will be installed automatically.

## Docker

1. Clone a project to your local directory: `git clone https://github.com/CipherJan/BookShop-Repository`;
2. Execute in command line: `cd BookShop-Repository`;
3. Execute in command line: `docker-compose up -d --build`;
4. Launch a browser and go to:
    - BookShop - `localhost:5000/swagger`
5. On BookShop page you will see a swagger page and methods for creating and managing stores;
6. You need to create your first store. Choose `/add-shop` method;
7. Click `Try it out`, put your input data (e.g. `name = "FirstShop"`, `balance = 2000`, `maxBookQuantity = 30`) and click `Execute`.

## IDE

You should have MSSQL, RabbitMQ, Elasticsearch & Kibana installed on your machine.
1. Clone a project to your local directory: `git clone https://github.com/CipherJan/BookShop-Repository`;
2. Open solutions such as BookShop, BookProvider, BookExternalAPI;
3. Launch `BookShop`, `BookProvider`, `BookExternalAPI` projects;
4. You will see a swagger page and methods for all progects;
5. Select BookShop swagger page - `localhost:5000/swagger`;
6. On BookShop page you will see a swagger page and methods for creating and managing stores;
7. You need to create your first store. Choose `/add-shop` method;
8. Click `Try it out`, put your input data (e.g. `name = "FirstShop"`, `balance = 2000`, `maxBookQuantity = 30`) and click `Execute`.
