# Step 5 - Getting started with validation in FhirStarter.DotNetCore

In this section we'll describe how to get the validation running with a [StructureDefintion](http://www.hl7.org/fhir/structuredefinition.html).
FhirStarter assumes that the StructureDefinition is per [Resource](http://www.hl7.org/fhir/resource.html) type and therefore the name of each StructureDefinition must be unique in the Service inheriting IFhirService.

## What a StructureDefinition looks like

A simple StructureDefinition wihtout any sort of requirements other than the type [Patient](http://www.hl7.org/fhir/patient.html):
```xml
<?xml version="1.0" encoding="utf-8"?>
<StructureDefinition xmlns="http://hl7.org/fhir">
  <url value="https://github.com/verzada/FhirStarter.DotNetCore/fhir/StructureDefinition/FhirStarterPatient" />
  <name value="FhirStarterPatient" />
  <status value="draft" />
  <fhirVersion value="4.0.0" />
  <kind value="resource" />
  <abstract value="false" />
  <type value="Patient" />
  <baseDefinition value="http://hl7.org/fhir/StructureDefinition/Patient" />
  <derivation value="constraint" />
</StructureDefinition>
```

A more detailed StructureDefinition for a [Person](http://www.hl7.org/fhir/person.html) service:

```xml
<StructureDefinition xmlns="http://hl7.org/fhir">
  <url value="https://github.com/verzada/FhirStarter.DotNetCore/fhir/StructureDefinition/FhirStarterPerson"/>
  <name value="FhirStarterPerson"/>
  <status value="draft"/>
  <fhirVersion value="4.0.0"/>
  <kind value="resource"/>
  <abstract value="false"/>
  <type value="Person"/>
  <baseDefinition value="http://hl7.org/fhir/StructureDefinition/Person"/>
  <derivation value="constraint"/>
  <differential>
    <element id="Person.telecom">
      <path value="Person.telecom"/>
      <max value="0"/>
    </element>
    <element id="Person.gender">
      <path value="Person.gender"/>
      <min value="1"/>
    </element>
    <element id="Person.birthDate">
      <path value="Person.birthDate"/>
      <min value="1"/>
    </element>
    <element id="Person.photo">
      <path value="Person.photo"/>
      <max value="0"/>
    </element>
    <element id="Person.managingOrganization">
      <path value="Person.managingOrganization"/>
      <max value="0"/>
    </element>
    <element id="Person.link">
      <path value="Person.link"/>
      <max value="0"/>
    </element>
  </differential>
</StructureDefinition>
```

## Where the StructureDefinition is located

The StructureDefinition is located in any web project exposing a FHIR R4 Service.
For the StructureDefinition to be picked up by the FhirStarter controller, the definitions must be placed in a specific folder structure in the web project:

- Root folder
  - Resources
    - StructureDefinitions

The resources available in the folder structure must be set to **Copy if newer** in the *Copy to Output Directory* option for each Structure Definition file.  

