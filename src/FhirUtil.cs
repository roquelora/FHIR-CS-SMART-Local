using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Hl7.Fhir.Serialization;

namespace smart_local
{
    /// <summary>
    /// Utility class for FHIR operations
    /// </summary>
    public static class FhirUtils
    {
        private static readonly string _restSecurityExtensionUrl = "http://fhir-registry.smarthealthit.org/StructureDefinition/oauth-uris";

        /// <summary>
        /// Get the OAuth endpoints from the FHIR server metadata.
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<FhirOAuthEndpoints> GetOAuthEndpointsAsync(
            FhirClient client)
        {
            // Get the FHIR server metadata, aka its capabilities.
            var response = await client.GetAsync("metadata");
            if (response is not CapabilityStatement capabilities)
            {
                throw new Exception("Failed to retrieve FHIR server metadata.");
            }

            // Extract the authorization and token URLs from the metadata
            string authorizationUrl = string.Empty;
            string tokenUrl = string.Empty;

            foreach (CapabilityStatement.RestComponent component in capabilities.Rest)
            {
                if (component.Security == null)
                {
                    continue;
                }

                foreach (Extension securityExtension in component.Security.Extension)
                {
                    if (securityExtension.Url != _restSecurityExtensionUrl)
                    {
                        continue;
                    }

                    foreach (Extension urlExtension in securityExtension.Extension)
                    {
                        if (urlExtension.Url == "authorize")
                        {
                            authorizationUrl = ((FhirUri)urlExtension.Value).Value.ToString();
                        }
                        else if (urlExtension.Url == "token")
                        {
                            tokenUrl = ((FhirUri)urlExtension.Value).Value.ToString();
                        }
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(authorizationUrl) || string.IsNullOrWhiteSpace(tokenUrl))
            {
                throw new Exception("Authorization or Token URL not found in FHIR server metadata.");
            }

            return new FhirOAuthEndpoints
            {
                AuthorizationUrl = authorizationUrl,
                TokenUrl = tokenUrl
            };
        }
    }
}
