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
using Microsoft.Extensions.Configuration;
using DeliveryTracker.Database;
using Microsoft.EntityFrameworkCore;
using DeliveryTracker.Database.Tables;

namespace DeliveryTracker {
	public static class Function1 {
		private static IConfigurationRoot Configuration {
			get;
		}

		static Function1() {
			var builder = new ConfigurationBuilder()
				.AddJsonFile("local.settings.json", true)
				.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		[FunctionName("GetTrackingNumberFromMail")]
		public static async Task<IActionResult> Run(
			[HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req, ILogger log) {

			var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
			dynamic data = JsonConvert.DeserializeObject(requestBody);
			string body = data?.body;

			var regex = new Regex(@"(^|[^\d\-])(?<number>\d{4}-?\d{4}-?\d{4})($|[^\d\-])");
			var matches = regex.Matches(body);
			foreach (var match in matches.OfType<Match>()) {
				// Database Insert
				await InsertTrackingNumber(match.Groups["number"].Value, log);
			}

			return new OkObjectResult(null);
		}

		private static async Task InsertTrackingNumber(string number, ILogger log) {
			using var dbContext = GetDbContext();
			if (!await dbContext.TrackingNumbers.AnyAsync(x => x.Number == number)) {
				var type = await Tracking.GetTrackingTypeAsync(number);
				using var tran = await dbContext.Database.BeginTransactionAsync();
				log.LogInformation($"registered: number={number},type={type}");
				await dbContext.TrackingNumbers.AddAsync(new TrackingNumber { Number = number, Institution = (int)type });
				await dbContext.SaveChangesAsync();
				await tran.CommitAsync();
			}
		}

		private static DeliveryTrackerDbContext GetDbContext() {
			// DataBase
			var dbContext = new DeliveryTrackerDbContext(DbType.SqlServer, Configuration.GetConnectionString("DefaultSqlConnection"));
			dbContext.Database.EnsureCreated();
			return dbContext;
		}
	}
}
