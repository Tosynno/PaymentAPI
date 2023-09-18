# PaymentAPI
Payment API
The goal is to allow both merchants and customers to input their details into the payments system.

Documentation
Getting started
This page contains important information on how to properly configure Payment API 

This Application is implemented using .Net6, a clean architecture, also known as the Clean Architecture or Onion Architecture, is a software architectural style that encourages separation of concerns and maintainability by dividing the program into discrete layers, each with a defined duty. The Presentation Layer, Application Layer, Domain Layer, and Infrastructure Layer are common examples of these layers. Here's a rundown on how to build a clean architecture in.NET 6:

The Presentation Layer is in charge of all user interaction and user interface concerns. This might be a web application (ASP.NET Core), a desktop application, or a mobile app in.NET 6.

Application Layer: Also known as use cases or application services, the application layer houses business logic that is particular to a certain application. It serves as a bridge between the Presentation and Domain Layers. This layer is in charge of coordinating operations and ensuring that business logic is appropriately applied.

Project links
We have set up a network of two BankSystem instances and one DemoShop, connected through a CentralApi. This network supports all BankSystem functionality, including Global transfers, Direct payments and Card payments.

Project	Address
DemoShop	https://banksystem-demoshop.herokuapp.com/
Test Bank 1

Transfer details:
* Bank name - Bank system
* Bank country - Germany
* Bank code - ABC	https://banksystem-1.herokuapp.com/
Test Bank 2

Transfer details:
* Bank name - Bank system 2
* Bank country - Netherlands
* Bank code - CBA	https://banksystem-2.herokuapp.com/
All of these projects have a demo account already registered:

Email	Password
test@test.com	Test123$
Additionally, all bank accounts created have an initial balance of €500 for demonstration purposes.

Bank accounts
Bank accounts hold information about their owner, balance, transactions, date of creation, etc.

Money transfers
BankSystem supports two types of money transfers - internal and global / worldwide.

Cards
Cards are used for making purchases on other websites using the CentralApi.

Direct payments
Direct payments are a way to securely pay on websites directly through a bank account without the need to provide card details.

DemoShop
DemoShop is an example web application implementing direct and card payments.

User settings
Bank administration
Used technologies:

C#
ASP.NET Core
ASP.NET Core MVC
ASP.NET Core Web API
Entity Framework Core
Asymmetric & hybrid encryption
jQuery
AJAX
HTML
CSS
Bootstrap
