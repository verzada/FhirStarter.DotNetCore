# Getting started 

This guide helps you create a barebone service to begin with.

## Quick start

If you want to get quickly started with developing a FHIR Service, just copy the [FhirStarter.R4.Twisted.Core.Nuget](https://github.com/verzada/FhirStarter.DotNetCore/tree/master/src/FhirStarter.R4.Twisted.Core.Nuget) project into your solution and do the necessary changes.

Alternatively, create a new Web project and include the [Detonator](https://www.nuget.org/packages/FhirStarter.R4.Detonator.Core/) and [Instigator](https://www.nuget.org/packages/FhirStarter.R4.Instigator.Core/) nuget libraries from [nuget.org](https://www.nuget.org/)

Alternatively you can go to the [Creating a FHIR service](CreatingAFHIRService.md)

## The structure of the main libraries in the Visual Studio solution

Before you get to create a FHIR web service with .Net Core, you need to get familiar with the structure in FhirStarter.DotNetCore source code:

| Name of the library               | Explaination                                                 |
| --------------------------------- | ------------------------------------------------------------ |
| FhirStarter.R4.Detonator.Core     | The "backend" of FhirStarter. The library contains the core logic for processing FHIR objects such as the [IFhirService] interface. Spark.Engine logic from [furore spark.engine link] is also present in this library. |
| FhirStarter.R4.Instigator.Core    | The "frontend" library. Contains logic to setup the web application and make it run. |
| FhirStarter.R4.Twisted.Core       | An example project with direct links to the above projects   |
| FhirStarter.R4.Twisted.Core.Nuget | An example project which consumes the official nuget packages. |

A FhirStarter service is based on the [IFhirService](https://github.com/verzada/FhirStarter.DotNetCore/blob/master/src/FhirStarter.R4.Detonator.Core/Interface/IFhirService.cs) interface. The interface contains all the necessary methods for the [FhirController](https://github.com/verzada/FhirStarter.DotNetCore/blob/master/src/FhirStarter.R4.Instigator.Core/Controllers/FhirController.cs) to interpret a FHIR Resource service.

The FhirController uses dependency injection to create instances of IFhirServices , so you can have as many services as you want, but the resource type in each service must be unique. So you can't for example have two [Patient](https://www.hl7.org/fhir/patient.html) service in the same FHIR web service endpoint.


