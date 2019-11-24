# Suit Alterations 

![Home page](https://i.imgur.com/cTRQp05.png)

This Web App consists of SPA and the independent service client which is working on a background process and subscribed to the `OrderPaid` message which can be sent to the Azure Service Bus Topic and this service will able to process it. 
Website allows to salesrep to enter alterations of the customer's suit and then check the status of the further work. After that when the customer paid the order to alteration, then the POS terminal should post the `OrderPaid` notification to Azure Service Bus Topic. Once our Azure Subscription service client will process this OrderPaid message, salesrep will see the changed status for this order to alteration.

[Public Web App is available by this link](http://suitalterations.azurewebsites.net/)

## Getting Started (for developers)

### Prerequisites

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
* EF Core & Code-First Approach
* Azure SQL server
* Azure Service Bus Topic and subscription
