# Welcome to the FhirStarter.DotNetCore R4 documentation.

This documentations intends on helping you on your way to create your first FhirStarter **R4** project. If you are planning on creating a FHIR server based on the earlier standards (DSTU, DSTU2 or STU3) you'll have to fork the repository and customize the libraries to accommodate the version of the standard you want to use.

## FhirStarter alternatives
For .Net, [Vonk](https://fire.ly/products/vonk/vonk-fhir-server/) made by Furore, is probably the most wellknown. <br />
HL7 International lists several [Open Source FHIR implementations](http://wiki.hl7.org/index.php?title=Open_Source_FHIR_implementations) on their web site.

## Before you begin

The FhirStarter intends on solving the matter of integrating several data sources to one or more FHIR service(s). The FhirStarter is very basic, however it enables the developer to get a FHIR service up and running in a short amount of time. 

## Some assumptions / requirements

* You have Visual Studio 2017++
* .Net Core 2.2.203
* You know Web API 
* You know how to map FHIR resources (check [R4](https://www.hl7.org/fhir/)) through [Forge](https://fire.ly/products/forge/) if you want to validate your resources.

## Guide index

The guide is not entirely complete, but it should help you along to create a simple FHIR server

- Step 1 [Getting started](FHIRService/GettingStarted.md) focuses on the infrastructure in the FhirStarter libraries 
- Step 2 [Setting up a Resource Service](FHIRService/SettingUpAResourceService.md) looks at how the IFhirService interface is handled
- Step 3 [Setting up FhirStarter specific settings](FHIRService/SettingUpFhirStarterSpecificSettings.md) looks at the appSettings.json configuration
- Step 4 [Setting up the Startup.cs file](FHIRService/SettingUpStartup.md) helps you configure the startup.cs file as well as adding a crucial setting in Program.cs for activating log4net logging
- Step 5 [Setting up Validation for your services](Validation/GettingStartedWithValidation.md) gets your validation going when all the previous steps are done. 

## Troubleshooting

For known issues check the [Troubleshooting](https://github.com/verzada/FhirStarter.DotNetCore/tree/master/docs/Troubleshooting) folder
