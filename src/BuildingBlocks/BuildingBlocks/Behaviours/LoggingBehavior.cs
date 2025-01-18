using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Behaviours
{
    public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest,TResponse>> _logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull,IRequest<TResponse> where TResponse : notnull
    {
        public  async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("[START] Handel request={Request} - Response={Response}  of the complete Request {CompleteRequest}", typeof(TRequest).Name,typeof(TResponse).Name,request);
            var timer = new Stopwatch();
            timer.Start();
            var response = await next();
            timer.Stop();
            var timeTaken = timer.Elapsed;
            if (timeTaken.Seconds > 3)
            {
                _logger.LogWarning("[PEROFORMANCE] The request {Request} took time {TimeTaken} ",typeof(TRequest).Name,timeTaken.Seconds);

            }
            _logger.LogInformation("[END] Handeled {Request} with {Response}", typeof(TRequest).Name, typeof(TResponse).Name);
            return response;
        }
    }
}
