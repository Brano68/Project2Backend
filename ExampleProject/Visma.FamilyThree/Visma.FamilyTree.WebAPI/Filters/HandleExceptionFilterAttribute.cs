using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;

namespace Visma.FamilyTree.WebAPI.Filters
{
    public class HandleExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public HandleExceptionFilterAttribute(ILogger logger)
        {
            Logger = logger;
        }

        private ILogger Logger { get; set; }

        public override void OnException(ExceptionContext context)
        {
            context.Result = new ContentResult()
            {
                Content = JsonConvert.SerializeObject(new
                {
                    context.Exception.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Type = context.Exception.GetType().Name,

                }),
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };

            Logger.LogError($"Exception cough {context.Exception.Message}. See stack {context.Exception.StackTrace}");
        }
    }
}
