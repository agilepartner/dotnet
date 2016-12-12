using Structurizr.Encryption;
using Structurizr.IO.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Structurizr.Client
{
    public class StructurizrClient
    {
        private const string WorkspacePath = "/workspace/";
        private readonly string _version;

        public string Url { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }

        /// <summary>the location where a copy of the workspace will be archived when it is retrieved from the server</summary>
        public DirectoryInfo WorkspaceArchiveLocation { get; set; }

        public EncryptionStrategy EncryptionStrategy { get; set; }

        public bool MergeFromRemote { get; set; }

        public bool Archive { get; set; }

        public StructurizrClient(string apiKey, string apiSecret) : this("https://api.structurizr.com", apiKey, apiSecret)
        {

        }

        public StructurizrClient(string apiUrl, string apiKey, string apiSecret)
        {
            _version = GetType().GetTypeInfo().Assembly
                                .GetName().Version
                                .ToString();

            Url = apiUrl;
            ApiKey = apiKey;
            ApiSecret = apiSecret;

            WorkspaceArchiveLocation = new DirectoryInfo(".");
            MergeFromRemote = true;
        }

        public async Task<Workspace> GetWorkspaceAsync(long workspaceId)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var request = CreateRequest(HttpMethod.Get, workspaceId);
                    var response = await httpClient.SendAsync(request);

                    response.EnsureSuccessStatusCode();

                    var responseString = await response.Content.ReadAsStringAsync();

                    if (Archive)
                    {
                        ArchiveWorkspace(workspaceId, responseString);
                    }

                    StringReader stringReader = new StringReader(responseString);

                    if (EncryptionStrategy == null)
                    {
                        return new JsonReader().Read(stringReader);
                    }
                    else
                    {
                        EncryptedWorkspace encryptedWorkspace = new EncryptedJsonReader().Read(stringReader);
                        encryptedWorkspace.EncryptionStrategy.Passphrase = this.EncryptionStrategy.Passphrase;

                        return encryptedWorkspace.Workspace;
                    }
                }
                catch (Exception e)
                {
                    throw new StructurizrClientException("There was an error getting the workspace: " + e.Message, e);
                }
            }
        }

        public async Task<bool> PutWorkspaceAsync(long workspaceId, Workspace workspace)
        {
            CheckWorkspace(workspaceId, workspace);

            if (MergeFromRemote)
            {
                await MergeFromRemoteAsync(workspaceId, workspace);
            }

            workspace.Id = workspaceId;

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var workspaceAsJson = Serialize(workspace);

                    var request = CreateRequest(HttpMethod.Put, workspaceId, workspaceAsJson);
                    var response = await httpClient.SendAsync(request);

                    response.EnsureSuccessStatusCode();

                    return true;
                }
                catch (Exception e)
                {
                    throw new StructurizrClientException("There was an error putting the workspace: " + e.Message, e);
                }
            }
        }

        private static void CheckWorkspace(long workspaceId, Workspace workspace)
        {
            if (workspace == null)
            {
                throw new ArgumentException("A workspace must be supplied");
            }
            else if (workspaceId <= 0)
            {
                throw new ArgumentException("The workspace ID must be set");
            }
        }

        private HttpRequestMessage CreateRequest(HttpMethod httpMethod, long workspaceId, string json = "")
        {
            var path = WorkspacePath + workspaceId;
            var requestMessage = new HttpRequestMessage(httpMethod, Url + path);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            requestMessage.Content = stringContent;

            var contentType = stringContent.Headers.ContentType.ToString();

            string contentMd5 = new Md5Digest().Generate(json);
            string nonce = "" + getCurrentTimeInMilliseconds();

            var hmac = new HashBasedMessageAuthenticationCode(ApiSecret);
            var hmacContent = new HmacContent(httpMethod.Method, path, contentMd5, contentType, nonce);
            var authorizationHeader = new HmacAuthorizationHeader(ApiKey, hmac.Generate(hmacContent.ToString()));

            requestMessage.Headers.Add(HttpHeaders.UserAgent, $"structurizr-dotnet/{_version}");
            requestMessage.Headers.TryAddWithoutValidation(HttpHeaders.Authorization, authorizationHeader.ToString());
            requestMessage.Headers.Add(HttpHeaders.Nonce, nonce);

            string contentMd5Base64Encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(contentMd5));
            stringContent.Headers.Add(HttpHeaders.ContentMd5, contentMd5Base64Encoded);

            return requestMessage;
        }

        private long getCurrentTimeInMilliseconds()
        {
            DateTime Jan1st1970Utc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(DateTime.UtcNow - Jan1st1970Utc).TotalMilliseconds;
        }

        private void ArchiveWorkspace(long workspaceId, string workspaceAsJson)
        {
            if (WorkspaceArchiveLocation != null)
            {
                var archiveFileName = Path.Combine(WorkspaceArchiveLocation.FullName, "structurizr-" + workspaceId + "-" + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".json");
                File.WriteAllText(archiveFileName, workspaceAsJson);
            }
        }

        private async Task MergeFromRemoteAsync(long workspaceId, Workspace workspace)
        {
            var remoteWorkspace = await GetWorkspaceAsync(workspaceId);

            if (remoteWorkspace != null)
            {
                workspace.Views.CopyLayoutInformationFrom(remoteWorkspace.Views);
                workspace.Views.Configuration.CopyConfigurationFrom(remoteWorkspace.Views.Configuration);
            }
        }

        private string Serialize(Workspace workspace)
        {
            string workspaceAsJson = null;

            using (StringWriter stringWriter = new StringWriter())
            {
                if (EncryptionStrategy == null)
                {
                    JsonWriter jsonWriter = new JsonWriter(false);
                    jsonWriter.Write(workspace, stringWriter);
                }
                else
                {
                    EncryptedWorkspace encryptedWorkspace = new EncryptedWorkspace(workspace, EncryptionStrategy);
                    EncryptedJsonWriter jsonWriter = new EncryptedJsonWriter(false);
                    jsonWriter.Write(encryptedWorkspace, stringWriter);
                }

                stringWriter.Flush();
                workspaceAsJson = stringWriter.ToString();
            }

            return workspaceAsJson;
        }
    }
}