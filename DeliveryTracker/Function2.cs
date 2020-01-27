using System;
using System.Threading.Tasks;
using DeliveryTracker.Database;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DeliveryTracker
{
    public static class Function2 {
		private static IConfigurationRoot Configuration {
			get;
		}
		static Function2() {
			var builder = new ConfigurationBuilder()
				.AddJsonFile("local.settings.json", true)
				.AddEnvironmentVariables();

			Configuration = builder.Build();
		}

		[FunctionName("Function2")]
		public static async Task Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log) {
			using var dbContext = GetDbContext();
			foreach (var trackingNumber in await dbContext.TrackingNumbers.ToListAsync()) {
				var number = trackingNumber.Number;

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
