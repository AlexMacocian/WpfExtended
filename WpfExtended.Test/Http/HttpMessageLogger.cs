using Microsoft.Extensions.Logging;
using System.Extensions;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WpfExtended.Tests.Http
{
    public class HttpMessageLogger : DelegatingHandler
    {
        private readonly ILogger logger;
        public HttpMessageLogger(ILogger logger, HttpMessageHandler innerHandler) : base(innerHandler)
        {
            this.logger = logger.ThrowIfNull(nameof(logger));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.logger.LogInformation($"{request.Method} - {request.RequestUri}");
            var response = await base.SendAsync(request, cancellationToken);
            this.logger.LogInformation($"{response.RequestMessage.RequestUri} - {response.StatusCode}");
            return response;
        }
    }
}
