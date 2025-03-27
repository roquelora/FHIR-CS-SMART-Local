using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

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

                await CreateHostBuilder().Build().StartAsync();

                int listenPort = await GetListenPortAsync();
                Console.WriteLine($"Listening on port: {listenPort}");

                for (int loops = 0; loops < 5; loops++)
                {
                    await Task.Delay(1000);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1;
            }
        }

        /// <summary>
        /// Get the port number that the web server is listening on.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static async Task<int> GetListenPortAsync()
        {
            for (int loops = 0; loops < 100; loops++)
            {
                await Task.Delay(100);
                string? address = Startup.Addresses?.Addresses.FirstOrDefault();

                if (address == null)
                {
                    continue;
                }

                if (address.Length < 18)
                {
                    continue;
                }

                if (int.TryParse(address[17..], out int listenPort)
                    && listenPort > 0
                    && listenPort < 65536)
                {
                    return listenPort;
                }
            }

            throw new Exception("Failed to get the listen port.");
        }

        /// <summary>
        /// Creates a default web host builder for the application.
        /// </summary>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://127.0.0.1:0");
                    webBuilder.UseKestrel();
                    webBuilder.UseStartup<Startup>();
                });
    }

}
