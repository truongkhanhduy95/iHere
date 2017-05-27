namespace Contemi.Core.Definition.Interfaces.Service
{
    /// <summary>
    /// Configuration for service
    /// </summary>
    public interface IServiceConfig
    {
        /// <summary>
        /// Service url
        /// </summary>
        string ActiveServiceUrl { get; set; }

        string DefaultServiceUrl { get; }

        /// <summary>
        /// Request timeout duration
        /// </summary>
        int RequestTimeout { get; }

        /// <summary>
        /// Number of retry attempt when request timeout
        /// </summary>
        int NumberOfRetry { get; }
    }
}