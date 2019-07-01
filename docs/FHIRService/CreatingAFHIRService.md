# Creating a FHIR service

## If you are doing it from scratch

If you want to integrate a FHIR service into an already existing .Net Core web project, you'll need to

- Add the two libraries [Detonator](https://www.nuget.org/packages/FhirStarter.R4.Detonator.Core/) and [Instigator](https://www.nuget.org/packages/FhirStarter.R4.Instigator.Core/) nuget libraries from [nuget.org](https://www.nuget.org/)
- Add the [FhirStarter specific json settings](Setting up FhirStarter specific settings.md)
- Recommended: Create a Service folder for where your Service using [IFhirService](https://github.com/verzada/FhirStarter.DotNetCore/blob/master/src/FhirStarter.R4.Detonator.Core/Interface/IFhirService.cs) is located
- Create the FHIR service 

That's it.

## If you are using the template nuget project

Copy the project and rename it. Make sure the assembly also reflects the name change.

That's it.

## If you want validation

You'll need profiles to validate from to make it properly work. However you can use the standard profile to initally test and see if the validation works.
Go to [Getting started with Validation](../Validation/GettingStartedWithValidation.md) to learn more.