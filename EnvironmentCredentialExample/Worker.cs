using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace EnvironmentCredentialExample
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<Worker> _logger;

        public Worker(IConfiguration configuration, ILogger<Worker> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Uri blobContainerUri = _configuration.GetValue<Uri>("blobContainerUri");
            _logger.LogCritical($"Getting Files from {blobContainerUri}");

            var creds = new DefaultAzureCredential(includeInteractiveCredentials: false);

            BlobContainerClient containerClient = new BlobContainerClient(blobContainerUri, creds);
            var blobs = containerClient.GetBlobsAsync(Azure.Storage.Blobs.Models.BlobTraits.None);
            int ind = 1;
            await foreach (var blob in blobs)
            {
                _logger.LogCritical($"{ind++} : {blob.Name}");
            }
        }
    }
}