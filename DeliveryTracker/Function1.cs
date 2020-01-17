using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Linq;

namespace DeliveryTracker
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
			log.LogInformation("C# HTTP trigger function processed a request.");

			var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			string body = data?.body;

			var regex = new Regex(@"\d{4}-?\d{4}-?\d{4}");
			var matches = regex.Matches(body);
			foreach (var match in matches.OfType<Match>()) {
				// Database Insert
				match.Value;
			}

			return new OkObjectResult(null);
		}
    }
}
