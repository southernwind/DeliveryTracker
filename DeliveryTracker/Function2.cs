using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DeliveryTracker.Database;
using DeliveryTracker.Institutions;
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

		[FunctionName("CheckStatusUpdate")]
		public static async Task Run([TimerTrigger("0 * * * * *")]TimerInfo myTimer, ILogger log) {
			using var dbContext = GetDbContext();
			foreach (var trackingNumber in await dbContext.TrackingNumbers.Where(x => x.UpdateDate >= DateTime.Now.AddDays(-3)).ToListAsync()) {
				var number = trackingNumber.Number;
				var institution = (TrackingType)trackingNumber.Institution;
				var ds = await Tracking.GetCurrentStatus(number,institution);
				if (ds != null && ds.Time != trackingNumber.UpdateDate) {
					using var tran = await dbContext.Database.BeginTransactionAsync();
					trackingNumber.State = ds.Name;
					trackingNumber.UpdateDate = ds.Time;
					dbContext.TrackingNumbers.Update(trackingNumber);
					log.LogInformation($"[{number}]\nステータス={ds.Name}\n時刻={ds.Time}");
					await HttpClientWrapper.PostAsync(Configuration.GetValue<string>("WebHookAddress"),
						new StringContent($"{{\"text\" : \"[{number}]\nステータス={ds.Name}\n時刻={ds.Time}\"}}", Encoding.UTF8, "application/json")
					);
					await dbContext.SaveChangesAsync();
					await tran.CommitAsync();

				}
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
