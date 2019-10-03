﻿/* 
 * Copyright (c) 2018, Firely (info@fire.ly) and contributors
 * See the file CONTRIBUTORS for details.
 * 
 * This file is licensed under the BSD 3-Clause license
 * available at https://raw.github.com/furore-fhir/spark/master/LICENSE
 */

using System.Collections.Generic;
using System.Linq;
using FhirStarter.R4.Detonator.Core.Interface;
using Hl7.Fhir.Model;
using Hl7.Fhir.Utility;

namespace FhirStarter.R4.Detonator.Core.SparkEngine.Service.FhirServiceExtensions
{
    public static class CapabilityStatementBuilder
    {
        public static CapabilityStatement CreateServer(string server, string publisher,
            FHIRVersion fhirVersion)
        {
            var capabilityStatement = new CapabilityStatement
            {
                Name = server,
                Publisher = publisher,
                FhirVersion = fhirVersion,
                Date = Date.Today().Value,
                FhirVersionElement = GetFhirVersion(),
            };
            return capabilityStatement;
        }

        private static Code<FHIRVersion> GetFhirVersion()
        {
            return new Code<FHIRVersion> { Value = FHIRVersion.N4_0_0 };
        }

        public static CapabilityStatement.RestComponent Rest(this CapabilityStatement conformance)
        {
            return conformance.Rest.FirstOrDefault();
        }

        public static CapabilityStatement AddCoreSearchParamsAllResources(this CapabilityStatement capabilityStatement,
    IEnumerable<IFhirService> services)
        {
            var fhirStarterServices = services as IFhirService[] ?? services.ToArray();

            foreach (var service in fhirStarterServices)
            {
                var resourceService = service;
                if (resourceService != null)
                {
                    capabilityStatement.Rest.Add(resourceService.GetRestDefinition());
                }
            }
            return capabilityStatement;
        }

        public static CapabilityStatement.ResourceComponent AddCoreSearchParamsResource(CapabilityStatement.ResourceComponent resourcecomp)
        {
            var parameters = ModelInfo.SearchParameters.Where(sp => sp.Resource == resourcecomp.Type.GetLiteral())
                            .Select(sp => new CapabilityStatement.SearchParamComponent
                            {
                                Name = sp.Name,
                                Type = sp.Type,
                                Documentation = sp.Description,

                            });

            resourcecomp.SearchParam.AddRange(parameters);
            return resourcecomp;
        }

        public static CapabilityStatement AddSearchTypeInteractionForResources(this CapabilityStatement conformance)
        {
            var firstOrDefault = conformance.Rest.FirstOrDefault();
            if (firstOrDefault != null)
                foreach (var r in firstOrDefault.Resource.ToList())
                {
                    conformance.Rest().Resource.Remove(r);
                    conformance.Rest().Resource.Add(AddSearchType(r));
                }
            return conformance;
        }

        public static CapabilityStatement.ResourceComponent AddSearchType(CapabilityStatement.ResourceComponent resourcecomp)
        {
            var type = CapabilityStatement.TypeRestfulInteraction.SearchType;
            var interaction = AddSingleResourceInteraction(resourcecomp, type);
            resourcecomp.Interaction.Add(interaction);
            return resourcecomp;
        }

        public static CapabilityStatement.ResourceInteractionComponent AddSingleResourceInteraction(CapabilityStatement.ResourceComponent resourcecomp, CapabilityStatement.TypeRestfulInteraction type)
        {
            var interaction = new CapabilityStatement.ResourceInteractionComponent { Code = type };
            return interaction;
        }

        public static CapabilityStatement AddOperationDefinition(this CapabilityStatement conformance, IEnumerable<IFhirService> services, Microsoft.AspNetCore.Http.HttpRequest request)
        {
            var operationComponents = new List<CapabilityStatement.OperationComponent>();

            foreach (var service in services)
            {
                var queryService = service;
                var operationDefinition = queryService?.GetOperationDefinition(request);
                if (!string.IsNullOrEmpty(operationDefinition?.Url))
                    operationComponents.Add(new CapabilityStatement.OperationComponent
                    {
                        Name = operationDefinition.Name,
                        DefinitionElement = new Canonical { Value = operationDefinition.Url}
                    });
            }
            if (operationComponents.Count > 0)
                conformance.Server().Operation.AddRange(operationComponents);
            return conformance;
        }

        public static CapabilityStatement.RestComponent Server(this CapabilityStatement capabilityStatement)
        {
            var server =
                capabilityStatement.Rest.FirstOrDefault(r => r.Mode == CapabilityStatement.RestfulCapabilityMode.Server);
            return server ?? capabilityStatement.AddRestComponent(true);
        }

        public static CapabilityStatement.RestComponent AddRestComponent(this CapabilityStatement capabilityStatement,
            bool isServer, Markdown documentation = null)
        {
            var server = new CapabilityStatement.RestComponent()
            {
                Mode =
                    isServer
                        ? CapabilityStatement.RestfulCapabilityMode.Server
                        : CapabilityStatement.RestfulCapabilityMode.Client
            };

            if (documentation != null)
            {
                server.Documentation = documentation;
            }

            capabilityStatement.Rest.Add(server);
            return server;
        }


    }
}
