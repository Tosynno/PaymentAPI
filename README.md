# PaymentAPI
Payment API
The goal is to allow both merchants and customers to input their details into the payments system.

Documentation:

Getting started:

This page contains important information on how to properly configure Payment API.

This Application is implemented using .Net6, a clean architecture, also known as the Clean Architecture or Onion Architecture, is a software architectural style that encourages separation of concerns and maintainability by dividing the program into discrete layers, each with a defined duty. The Presentation Layer, Application Layer, Domain Layer, and Infrastructure Layer are common examples of these layers. Here's a rundown on how to build a clean architecture in.NET 6:

The Presentation Layer is in charge of all user interaction and user interface concerns. This might be a web application (ASP.NET Core), a desktop application, or a mobile app in.NET 6.

Application Layer: Also known as use cases or application services, the application layer houses business logic that is particular to a certain application. It serves as a bridge between the Presentation and Domain Layers. This layer is in charge of coordinating operations and ensuring that business logic is appropriately applied.

Domain Layer:
The Domain Layer is where the essential business logic and domain entities reside. It is the application's heart and should be independent of any specific technology or framework. This layer specifies your application's rules, validation, and behavior.

Entities: Domain entities are defined as data table types that reflect the essential business ideas.

Infrastructure Layer:
The Infrastructure Layer is responsible for anything connected to external concerns, such as databases, external services, and frameworks. It is the least abstract layer and should be changeable without impacting the application's core.

Database: Use Entity Framework Core or similar ORM to implement data access.

AccountController

TransactionController


Used technologies:

C#
ASP.NET Core,
ASP.NET Core MVC,
ASP.NET Core Web API,
Entity Framework Core,
.net6,
FluentValidation.AspNetCore,
Quartz
