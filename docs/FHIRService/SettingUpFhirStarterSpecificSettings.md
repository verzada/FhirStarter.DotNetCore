# Step 3 Setting up FhirStarter specific settings

Whether you chose to create a service from scratch with the nuget libraries or copied the nuget project from GitHub, the process is pretty much the same.

## Checking the appSettings file - an explaination

The expected FhirStarter settings 

### The JSON settings

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

### Explaining the JSON settings

| Setting               | Explanation                                                  | Required     |
| --------------------- | ------------------------------------------------------------ | ------------ |
| FhirServiceAssemblies | The assemblies loaded at startup. The assemblies must be listed so the service knows which assemblies are going to be used. If there were no limitations on which were used, then the service would have to search through the myriads of libraries .Net Core uses to find the specific FHIR service libraries. | Yes          |
| MockupEnabled         | If the IFhirMockupService interface is used to create mockup endpoints and the service is only going to expose mockups, then this property must be enabled with a 'true'. <br />**Note** that if enabled, the FhirController will **only** expose the mockup service. | Yes          |
| EnableValidation      | Enables validation of resources that are returned as responses from the FHIR R4 service. | Yes          |
| LogRequestWhenError   | Can set to true/false for debugging scenarios                | No (test it) |
| FhirPublisher         | Used in the metadata response - can be empty                 | No           |
| FhirDescription       | Used in the metadata response - can be empty                 | No           |

### An example of a configured appSettings.json file
A fully configured appSettings.json file can look something like this:

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

**Note II:** Previous versions of FhirStarter R4 had the specific settings for FhirStarter in a separate file which was copied into the project using the FhirStarter libraries