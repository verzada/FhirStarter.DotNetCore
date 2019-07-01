using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Hl7.Fhir.Specification.Source;
using Hl7.Fhir.Validation;

namespace FhirStarter.R4.Instigator.Core.Validation
{
    public static class ValidatorFactory
    {
        public static Validator GetValidator()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var location = new Uri(assembly.GetName().CodeBase);
            var directoryInfo = new FileInfo(location.AbsolutePath).Directory;

            var structureDefinitions = directoryInfo?.FullName + @"\Resources\StructureDefinitions";
            var includeSubDirectories = new DirectorySourceSettings { IncludeSubDirectories = true };
            var directorySource = new DirectorySource(structureDefinitions, includeSubDirectories);

            var cachedResolver = new CachedResolver(directorySource);
            var coreSource = new CachedResolver(ZipSource.CreateValidationSource());
            var combinedSource = new MultiResolver(cachedResolver, coreSource);
            var settings = new ValidationSettings
            {
                EnableXsdValidation = true,
                GenerateSnapshot = true,
                Trace = true,
                ResourceResolver = combinedSource,
                ResolveExteralReferences = true,
                SkipConstraintValidation = false
            };
            var validator = new Validator(settings);
            return validator;
        }
    }
}
