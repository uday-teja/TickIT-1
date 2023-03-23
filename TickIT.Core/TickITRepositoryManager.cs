using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using TockIT.Auth.Models;

namespace TickIT.Auth
{
    public class TickITRepositoryManager
    {
        private IPublicClientApplication _clientApp;

        private readonly string _instance = "https://login.microsoftonline.com/";

        private readonly string _messages = "https://graph.microsoft.com/v1.0/me/messages";

        private string _accessToken;

        public async void PushToAzure()
        {
            string fileName = "GraphData.json";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            var text = File.ReadAllText(filePath);
            var response = JsonSerializer.Deserialize<GraphApiResponse<Message>>(text);

            if (_accessToken == null)
            {
                AuthenticationResult authResult = await GetAccessToken();
                _accessToken = authResult.AccessToken;
            }
            var messages = await GetHttpContentWithToken(_messages, _accessToken);
        }

        private async Task<AuthenticationResult> GetAccessToken()
        {
            var builder = PublicClientApplicationBuilder.Create("f1ac50e2-3609-478a-9f03-a423e98b96e1")
                                                        .WithAuthority($"{_instance}865cc515-a530-4538-8ef8-072b7b2be759")
                                                        .WithDefaultRedirectUri();
            _clientApp = builder.Build();

            List<string> scopes = new List<string>
            {
                "User.Read",
                "Mail.Read"
            };

            AuthenticationResult authResult = await _clientApp.AcquireTokenInteractive(scopes)
                                                              .WithPrompt(Prompt.SelectAccount)
                                                              .ExecuteAsync();
            _accessToken = authResult.AccessToken;
            return authResult;
        }

        public async Task<string> GetHttpContentWithToken(string url, string token)
        {
            var httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpResponseMessage response;
            try
            {
                var request = new System.Net.Http.HttpRequestMessage(System.Net.Http.HttpMethod.Get, url);
                //Add the token in Authorization header
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                response = await httpClient.SendAsync(request);
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            catch (Microsoft.Graph.ServiceException ex)
            {
                if (ex.ResponseStatusCode == 401)
                {
                    _accessToken = (await GetAccessToken()).AccessToken;
                    return await GetHttpContentWithToken(url, token);
                }
                else
                {
                    return ex.ToString();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
