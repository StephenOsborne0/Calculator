using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScriptEngine.Business.Parsers;
using Shared.Models.Parser;

namespace ScriptEngine.AzureFunctions
{
    public static class EvaluateMultiple
    {
        [FunctionName("EvaluateMultiple")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var inputs = JsonConvert.DeserializeObject<List<string>>(requestBody);
                var expressions = new List<Expression>();
                var parser = new Parser();

                foreach (var input in inputs)
                {
                    var parsedExpression = parser.Parse(input);
                    expressions.Add(parser.Evaluate(parsedExpression));
                }

                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
                };

                return expressions.Count == inputs.Count
                    ? (ActionResult) new OkObjectResult(JsonConvert.SerializeObject(expressions, Formatting.Indented,
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
