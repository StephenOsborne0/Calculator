using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScriptEngine.Business.Parsers;

namespace ScriptEngine.AzureFunctions
{
    public static class Evaluate
    {
        [FunctionName("Evaluate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]
            HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                var parser = new Parser();
                var parsedExpression = parser.Parse(requestBody.Replace("\\\"", "\""));
                var expression = parser.Evaluate(parsedExpression);

                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
                };

                return expression != null
                    ? (ActionResult) new OkObjectResult(JsonConvert.SerializeObject(expression, Formatting.Indented,
                        settings))
                    : new BadRequestObjectResult("Please pass a script in the request body");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
    }
    }
}
