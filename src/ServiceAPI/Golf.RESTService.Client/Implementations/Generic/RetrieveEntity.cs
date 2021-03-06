﻿using Golf.Global.Implementations;
using Golf.RESTService.Client.Interfaces;
using Golf.ServiceLayer.Dto.Implementations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Golf.RESTService.Client.Implementations.Generic
{
    public class RetrieveEntity<T> : IRetrieveEntity<T>, IDisposable
        where T: BaseDto
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly List<KeyValuePair<string, string>> _urlArgs;

        public RetrieveEntity(HttpClient httpClient, string baseUrl, List<KeyValuePair<string, string>> urlArgs)
        {
            if (httpClient == null)
            {
                throw new ArgumentNullException(nameof(httpClient));
            }
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }
            if (urlArgs == null)
            {
                _urlArgs = new List<KeyValuePair<string, string>>();
            }

            _httpClient = httpClient;
            _baseUrl = baseUrl;
            _urlArgs = urlArgs;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public async Task<T> RetrieveEntityAsync(int id)
        {
            var url = new UrlEncoder(_baseUrl)
                .AddPath(id.ToString());

            var response = await _httpClient.Clear().GetAsync(url.ToString());

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<T>(json);
            }

            return default(T);
        }

        public async Task<IEnumerable<T>> RetrieveEntitiesAsync()
        {
            var url = new UrlEncoder(_baseUrl)
                .AddToQueryString(_urlArgs);

            var response = await _httpClient.Clear().GetAsync(url.ToString());

            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<List<T>>(json);
            }

            return default(List<T>);
        }
    }
}
