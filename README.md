# Book-Shop Repository

The BookShop-Repository contains 4 ASP.NET Core applications that form a common system:
  1. BookShop; 
  2. BookProvider;
  3. BookExternalAPI;
  4. BookContractLibrary.

The purpose of this system is to provide the backend component of bookstores with extensive functionality.

The general system diagram is shown in the integration schematic diagram.

### Integration schematic diagram:

![Book Shop sсheme v2](https://user-images.githubusercontent.com/85920527/142666308-0dd11e6e-297b-4061-912c-ee96c0fad01b.jpg "Integration schematic diagram:")

## BookShop

It is the main application that allows you to create and manage bookstores and stores books sold and sales history. Management options allow:
 - Order new books from the supplier automatically or manually as needed;
 - To set a sale on books that are considered obsolete. The discount depends on the genre of the book or it may not be available at all;
 - Top up the balance sheet of the store for a specified amount.
The store also allows you to view the current library of books and their purchases.

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


This application, which uses Entity Framework, Quartz, MassTansit, Fluent Validator and Serilog with ElasticSearch.

## BookProvider

This is a companion app for Bookshop. He receives messages via rabbitmq ordering new books. In response, it sends the books and their total cost received via an HTTP request to BookExtenalAPI.
This application, which uses MassTansit and Serilog with ElasticSearch.

## BookExternalAPI

This app provides a set of random books in response to an HTTP request

## BookContractLibrary

The project on the basis of which a package named `BookContractLibrary_from_BookShop` was generated and shared with the NuGet. This NuGet package used to exchange messages via RabbitMQ between BookShop and BookProvider.

# Get started

You can launch a service using either in a Docker or just with Visual Studio/ JetBrains Rider IDE.

BookShop and BookProvider have MSSQL, RabbitMQ, Elasticsearch & Kibana as an externals dependency. You should have MSSQL, RabbitMQ, Elasticsearch & Kibana running somewhere. If your choose Docker method of running then external softwhere will be installed automatically.

## Docker

1. Clone a project to your local directory: `git clone https://github.com/CipherJan/BookShop-Repository`;
2. Execute in command line: `cd BookShop-Repository`;
3. Execute in command line: `docker-compose up -d --build --force-recreate`;
4. Launch a browser and go to:
   - BookShop - `localhost:5000/swagger`,
   - BookProvider - `localhost:5100/swagger`,
   - BookExternalAPI - `localhost:5200/swagger`;
5. On BookShop page you will see a swagger page and methods for creating and managing stores;
6. You need to create your first store. Choose `/add-shop` method; 
7. Click `Try it out`, put your input data (e.g. `name = "FirstShop"`, `balance = 2000`, `maxBookQuantity = 30`) and click `Execute`.

## IDE

You should have MSSQL, RabbitMQ, Elasticsearch & Kibana installed on your machine. 
1. Clone a project to your local directory: `git clone https://github.com/CipherJan/BookShop-Repository`;
2. Open solutions such as BookShop, BookProvider, BookExternalAPI;
3. Launch `BookShop`, `BookProvider`, `BookExternalAPI` projects;
4. You will see a swagger page and methods for all progects;
5. Select BookShop swagger page - `localhost:5000/swagger;
6. On BookShop page you will see a swagger page and methods for creating and managing stores;
7. You need to create your first store. Choose `/add-shop` method; 
8. Click `Try it out`, put your input data (e.g. `name = "FirstShop"`, `balance = 2000`, `maxBookQuantity = 30`) and click `Execute`.
