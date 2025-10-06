using Training.Common.Configurations.Http;
using Training.Common.DTO.Integration.File;
using Training.Domain;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Training.Integration.FileRepository
{
    public class FileRepository(MicroServicesUrls microServicesUrls, IHttpClientFactory httpClientFactory, IConfiguration configuration) : IFileRepository
    {

        public async Task<List<TokenDto>> GetTokens(List<Guid> ids)
        {

            try
            {
                var appCode = configuration["AppCode"];

                var url = microServicesUrls.GenerateTokenWithClaims + $"{appCode}";

                var fileManagerApiClient = httpClientFactory.CreateClient(ClientNames.FileManagerApiClient);

                var content = new MultipartFormDataContent();

                // Add each ID as a form field
                for (int i = 0; i < ids.Count; i++)
                {
                    content.Add(new StringContent(ids[i].ToString()), $"ids[{i}]");
                }

                var response = await fileManagerApiClient.PostAsync(url, content);

                response.EnsureSuccessStatusCode();

                var filesTokensResponse = await response.Content.ReadAsStringAsync();


                if (filesTokensResponse == null)
                    throw new Exception(MessagesConstants.ErrorGeneratingFilesTokens);

                var filesTokensResult = JsonConvert.DeserializeObject<dynamic>(filesTokensResponse);
                var tokens = JsonConvert.DeserializeObject<List<TokenDto>>(filesTokensResult!.Data.ToString()!);
                return tokens!;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
