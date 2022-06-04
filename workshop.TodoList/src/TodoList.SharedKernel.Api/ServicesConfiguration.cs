using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TodoList.SharedKernel.Api
{
    public static class ServicesConfiguration
    {
        public static void AddHttpRetryPolicy(this IServiceCollection services, string endpoint)
        {
            IAsyncPolicy<HttpResponseMessage> httpRetryPolicy =
                Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));

            services.AddHttpClient("RemoteServer", client =>
            {
                client.BaseAddress = new Uri(endpoint);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddPolicyHandler(httpRetryPolicy);
        }

        public static void AddAsyncCircuitBreakerPolicy(this IServiceCollection services, string baseAdress)
        {
            AsyncCircuitBreakerPolicy<HttpResponseMessage> breakerPolicy = Policy
                .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromSeconds(60), 7, TimeSpan.FromSeconds(15),
                    OnBreak, OnReset, OnHalfOpen);

            HttpClient httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAdress) // this is the endpoint HttpClient will hit,
            };
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            services.AddSingleton<HttpClient>(httpClient);
            services.AddSingleton<AsyncCircuitBreakerPolicy<HttpResponseMessage>>(breakerPolicy);
        }

        private static void OnHalfOpen()
        {
            Debug.WriteLine("Connection half open");
        }

        private static void OnReset(Context context)
        {
            Debug.WriteLine("Connection reset");
        }

        private static void OnBreak(DelegateResult<HttpResponseMessage> delegateResult, TimeSpan timeSpan, Context context)
        {
            Debug.WriteLine($"Connection break: {delegateResult.Result}, {delegateResult.Result}");
        }
    }
}