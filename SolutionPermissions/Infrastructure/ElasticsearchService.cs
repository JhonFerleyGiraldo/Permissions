using Application;
using Application.DTOs;
using Microsoft.Extensions.Options;
using Nest;

namespace Infrastructure
{
    public class ElasticsearchService : IElasticsearchService
    {
        private readonly IElasticClient _client;
        private readonly string _indexName = "permissions";

        public ElasticsearchService(IOptions<ElasticsearchSettingsDto> elasticsearchSettings)
        {
            var settings = new ConnectionSettings(new Uri(elasticsearchSettings.Value.Uri))
                .DefaultIndex(_indexName);

            _client = new ElasticClient(settings);

            var indexExistsResponse = _client.Indices.Exists(_indexName);
            if (!indexExistsResponse.Exists)
            {
                _client.Indices.Create(_indexName, c => c
                    .Map<PermissionDto>(m => m.AutoMap())
                );
            }
        }

        public async Task IndexPermissionAsync(PermissionDto permissionDto)
        {
            var response = await _client.IndexDocumentAsync(permissionDto);

            if (!response.IsValid)
            {
                Console.WriteLine($"Error al indexar el permiso con ID {permissionDto.Id} en Elasticsearch: {response.DebugInformation}");
            }
        }
    }
}
