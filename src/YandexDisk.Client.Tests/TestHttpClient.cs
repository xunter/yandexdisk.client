﻿using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using YandexDisk.Client.Http;

namespace YandexDisk.Client.Tests
{
    internal class TestHttpClient : IHttpClient
    {
        private readonly string _methodName;
        private readonly string _url;
        private readonly string _request;
        private readonly HttpStatusCode _httpStatusCode;
        private readonly string _result;

        public static readonly string BaseUrl = "http://ya.ru/api/";
        public static readonly string ApiKey = "test-api-key";

        public TestHttpClient(string methodName,
                              string url,
                              string request = null,
                              HttpStatusCode httpStatusCode = HttpStatusCode.OK,
                              string result = null)
        {
            _methodName = methodName;
            _url = url;
            _request = request;
            _httpStatusCode = httpStatusCode;
            _result = result;
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Assert.NotNull(request);
            Assert.AreEqual(_methodName, request.Method.Method);
            Assert.AreEqual(_url, request.RequestUri.ToString());

            if (request.Content != null && _request != null)
            {
                Assert.AreEqual(_request, await request.Content.ReadAsStringAsync());
            }
            

            return new HttpResponseMessage(_httpStatusCode)
            {
                Content = new StringContent(_result, Encoding.UTF8, "text/json")
            };
        }

        public void Dispose() { }
    }
}
