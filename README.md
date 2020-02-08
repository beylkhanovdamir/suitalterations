# Suit Alterations 

### ASP.NET Core Blazor App with CQRS implementation and DDD using Clean Architecture

App architecture is designed in accordance with the Clean Architecture where we have the next layers, see below diagram:

![diagram](https://github.com/beylkhanovdamir/suitalterations/blob/master/app-architecture.png)

Website allows to salesrep to enter alterations of the customer's suit and then check the status of the further work. After that when the customer paid the order to alteration, then the POS terminal should post the `OrderPaid` notification to Azure Service Bus Topic. Once our Azure Subscription service client will process this OrderPaid message, salesrep will see the changed status for this order to alteration by incoming notification of this event.

Screenshot of SPA
![Home page](https://i.imgur.com/cTRQp05.png)

[Public Web App is available by this link](http://suitalterations.azurewebsites.net/)

## Getting Started (for developers)

### Prerequisites
Physical project structure is organised in accordance with [MS .NET project structure](https://gist.github.com/davidfowl/ed7564297c61fe9ab814)

To build locally this web application you need to have installed Visual Studio Build Tools 2019 16.3 or later and the .NET Core 3.0+ SDK on your machine. 
If you want to open this project in development purposes, then you need to install any of these IDE - Visual Studio 2019 (for Mac) / Jetbrains Rider.
You can deploy this web app in docker, all you need is to have installed Docker v2.1+ 
To process event messages from Azure Service Bus Topic, you need to have Azure Subscription with created Azure Service Bus resource to publish and receive messages.
The Web App has two sections with the appsettings. The first one is ConnectionString to connect to your Database and the other one is AzureServiceBus with ConnectionString to the Service Bus namespace, and predefined Topic and Subscription names accordingly.

### Running web app on Docker

Follow below instructions:

0. Build the docker image by the following command: `docker build -t suitalterations .` Make sure that command was successfully completed to start use your tagged docker image *suitalterations:latest*

1. Run the `docker run --rm -it --name suitalterations -p 5555:80 suitalterations:latest` command

2. Open the URL http://localhost:5555/ in your browser.

### Technologies & Frameworks
* ASP.NET Core 3.0
* Blazor with WebAssembly support
* MediatR
* FluentValidation
* Autofac
* Automapper
* FluentAssertions
* EF Core & Code-First Approach
* Azure SQL server
* Azure Service Bus Topic and subscription

### Patterns & Design Approaches
* Domain Driven Design approach & The Dependency Rule
* Clean Architecture (see [details](http://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html))
* CQRS (see [details](https://martinfowler.com/bliki/CQRS.html))
* SOLID principles 
* Decorator (for the dispatching events)
* Domain Event (see [details](https://martinfowler.com/eaaDev/DomainEvent.html))
* Value Object (see [details](https://martinfowler.com/bliki/ValueObject.html)
* Aggregate (we have two clusters - Customer and SuitAlteration. See [details](https://www.martinfowler.com/bliki/DDD_Aggregate.html))
* Seedwork (see [details](https://www.martinfowler.com/bliki/Seedwork.html)
* Mediator (reduce dependencies between objects)
* Unit of Work (see [details](https://www.martinfowler.com/eaaCatalog/unitOfWork.html))
* Shadow Properties (`CustomerId` is a shadow property in the SuitAlteration entity model. Shadow Properties allows us to decouple our entities from database schema. Thereby, we no need to include foreign keys in our Domain Model)
* _Event Sourcing - TBD, but not implemented yet (work in progress)_
