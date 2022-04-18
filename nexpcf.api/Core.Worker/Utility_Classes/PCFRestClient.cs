using Microsoft.Extensions.Configuration;
using RestSharp;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Worker.Utility_Classes
{
    public class PCFRestClient
    {
        private readonly IConfiguration _configuration;

        public PCFRestClient(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<PcfApiResponse> ExecuteAsync(RestRequest request, string urlActionMethod = "", Method httpCallMethod = Method.Get)
        {

            var producerapilink = string.Format("{0}/api/{1}", _configuration.GetValue<string>("PcfProducerApiBaseUrl"), urlActionMethod);


            var options = new RestClientOptions(string.Format("{0}/api/{1}", _configuration.GetValue<string>("PcfProducerApiBaseUrl"), urlActionMethod));
            /*{
                ThrowOnAnyError = true,
                Timeout = 1000
            };*/
            var client = new RestClient(options);
            switch (httpCallMethod)
            {
                case Method.Post:
                    {
                        return await client.PostAsync<PcfApiResponse>(request);
                    }
                case Method.Get:
                    {
                        return await client.GetAsync<PcfApiResponse>(request);
                    }
            }
            return await client.PostAsync<PcfApiResponse>(request);
        }
    }
}
