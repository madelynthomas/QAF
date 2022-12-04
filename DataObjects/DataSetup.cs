using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace QAF.DataObjects
{
    public static class DataSetup
    {
        private static string
            ClientId,
            ClientToken,
            GrantType,
            Scope,
            ClientSecret;
        private static Uri
            APIUri,
            APIUriTerm,
            APIUriToken;

        static DataSetup()
        {
            SetUpVariables();
        }

        private static void SetUpVariables()
        {
            ClientId = "";
            GrantType = "";
            Scope = "";

            switch (ConfigurationManager.AppSettings.Get("Environment").ToLower())
            {
                case Environment.Dev:
                    ClientSecret = "";
                    APIUri = new Uri("");
                    APIUriToken = new Uri("");
                    APIUriTerm = new Uri("");
                    break;
                case Environment.Qa:
                    ClientSecret = "";
                    APIUri = new Uri("");
                    APIUriToken = new Uri("");
                    APIUriTerm = new Uri("");
                    break;
                case Environment.Uat:
                    ClientSecret = "";
                    APIUri = new Uri("");
                    APIUriToken = new Uri("");
                    APIUriTerm = new Uri("");
                    break;
                default:
                    Console.WriteLine("No environment variable set in config");
                    break;
            }
        }

        public static async Task<TokenResponse> GetClientToken()
        {
            var client = new HttpClient { BaseAddress = APIUriToken };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret),
                new KeyValuePair<string, string>("grant_type", GrantType),
                new KeyValuePair<string, string>("scope", Scope),
            });

            HttpResponseMessage response = await client.PostAsync("core/connect/token", content);
            response.EnsureSuccessStatusCode();

            if (response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                ClientToken = token.Access_Token;

                return token;
            }
            else
            {
                throw new Exception("No content in token response");
            }
        }

        public static async Task EnrollStudent(Student student)
        {
            if (ClientToken == null)
            {
                await GetClientToken();
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = APIUri;
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ClientToken);

                string content = JsonConvert.SerializeObject(new
                {
                    firstName = student.FirstName,
                    lastName = student.LastName,
                    dateOfBirth = student.DateOfBirth,
                    gender = student.Gender,
                    gradeLevel = student.GradeLevel
                });

                HttpResponseMessage response = await client.PostAsync("enrollment", new StringContent(content, Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
