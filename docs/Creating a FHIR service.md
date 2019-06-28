# Step 1 Settings

Whether you chose to create a service from scratch with the nuget libraries or copied the nuget project from GitHub, the process is pretty much the same.

## Checking the appSettings file

| Setting               | Explanation                                                  |
| --------------------- | ------------------------------------------------------------ |
| FhirServiceAssemblies | The assemblies loaded at startup. The assemblies must be listed so the service knows which assemblies are going to be used. If there were no limitations on which were used, then the service would have to search through the myriads of libraries .Net Core uses to find the specific FHIR service libraries. |
| MockupEnabled         | If the IFhirMockupService interface is used to create mockup endpoints, then this property must be enabled with a 'true' |
| EnableValidation      | Enables validation of resources that are returned as responses from the FHIR R4 service. |
| LogRequestWhenError   | Can set to true/false for debugging scenarios                |
| FhirPublisher         | Used in the metadata response - can be empty                 |
| FhirDescription       | Used in the metadata response - can be empty                 |



The which must be added in appsettings if missing:

```json
"FhirStarterSettings": {
    "FhirServiceAssemblies":["FhirStarter.R4.Twisted.Core","PatientAdministration.Person.Lib" ],
    "MockupEnabled": "false",
    "EnableValidation": "false",
    "LogRequestWhenError": "false",
    "FhirPublisher": "ACME",
    "FhirDescription":  "ACME delivers the best services" 
}
```

A fully configured appSettings.json file will look something like this:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "AllowedHosts": "*",
  "FhirStarterSettings": {
    "FhirServiceAssemblies": [ "FhirStarter.R4.Twisted.Core", "PatientAdministration.Person.Lib" ],
    "MockupEnabled": "false",
    "EnableValidation": "false",
    "LogRequestWhenError": "false",
    "FhirPublisher": "ACME",
    "FhirDescription":  "ACME delivers the best services" 
  }
}
```

**Note:** that the FhirStarterSettings section must exist in order for the service to run.

**Note II:** Previous versions of FhirStarter R4 had the specific settings for FhirStarter in a separate file. 