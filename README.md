# Book-Shop Repository

The BookShop-Repository contains 4 ASP.NET Core applications that form a common system:
  1. BookShop 
  2. BookProvider
  3. BookExternalAPI
  4. BookContractLibrary

The purpose of this system is to provide the backend component of bookstores with extensive functionality.
The general system diagram is shown in the integration schematic diagram.

### Integration schematic diagram:

![Book Shop sсheme v2](https://user-images.githubusercontent.com/85920527/142666308-0dd11e6e-297b-4061-912c-ee96c0fad01b.jpg "Integration schematic diagram:")

## BookShop

It is the main application that allows you to create and manage bookstores and stores books sold and sales history. Management options allow:
 - Order new books from the supplier automatically when their total number is reduced to 10% of the original value, or manually as needed.
 - To set a sale on books that are considered obsolete (Such books include books that were released more than a year ago). The discount depends on the genre of the book or it may not be available at all
 - Top up the balance sheet of the store for a specified amount
The store also allows you to view the current library of books and their purchases.
