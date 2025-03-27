using System;
using System.Threading.Tasks;

namespace smart_local
{
    /// <summary>
    /// Main program
    /// </summary>
    public static class Program
    {
        private const string _defaultFhirUrl = "https://launch.smarthealthit.org/v/r4/sim/WzIsIiIsImU0NDNhYzU4LThlY2UtNDM4NS04ZDU1LTc3NWMxYjhmM2EzNyIsIkFVVE8iLDAsMCwwLCIiLCIiLCIiLCIiLCIiLCIiLCIiLDAsMSwiIl0/fhir";

        /// <summary>
        /// Program to access a SMART FHIR server with a local webserver for redirection
        /// </summary>
        /// <param name="fhirServerUrl">FHIR R4 endpoint</param>
        /// <returns></returns>
        static async Task<int> Main(string fhirServerUrl = _defaultFhirUrl)
        {
            try
            {
                Console.WriteLine($"FHIR server:{fhirServerUrl}");

                Hl7.Fhir.Rest.FhirClient client = new(fhirServerUrl);

                var oauthEndpoints = await FhirUtils.GetOAuthEndpointsAsync(client);

                Console.WriteLine($"OAuth AuthorizationUrl: {oauthEndpoints.AuthorizationUrl}");
                Console.WriteLine($"OAuth TokenUrl: {oauthEndpoints.TokenUrl}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }
    }

}
