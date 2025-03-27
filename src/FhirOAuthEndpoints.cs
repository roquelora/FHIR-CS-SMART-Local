namespace smart_local
{
    /// <summary>
    /// Class to hold OAuth endpoints for FHIR
    /// </summary>
    public class FhirOAuthEndpoints
    {
        /// <summary>
        /// Authorization URL for OAuth 2.0
        /// <para>Example: https://launch.smarthealthit.org/v/r4/sim/authorize</para>
        /// </summary>
        public required string AuthorizationUrl { get; set; }

        /// <summary>
        /// Token URL for OAuth 2.0
        /// <para>Example: https://launch.smarthealthit.org/v/r4/sim/token</para>
        /// </summary>
        public required string TokenUrl { get; set; }
    }
}
