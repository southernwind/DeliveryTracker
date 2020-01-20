using Microsoft.EntityFrameworkCore;

using DeliveryTracker.Database.Tables;

namespace DeliveryTracker.Database {
	public enum DbType {
		SqlServer
	}
	public class DeliveryTrackerDbContext : DbContext {
		private readonly string _dbConnectionString;
		private readonly DbType _dbType;

		public DbSet<TrackingNumber> TrackingNumbers {
			get;
			set;
		}
		
		public DeliveryTrackerDbContext(DbType dbType, string dbConnectionString) {
			this._dbConnectionString = dbConnectionString;
			this._dbType = dbType;
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			// Primary Keys
			modelBuilder.Entity<TrackingNumber>().HasKey(x => x.Number);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			switch (this._dbType) {
				case DbType.SqlServer:
					optionsBuilder.UseSqlServer(this._dbConnectionString);
					break;
			}
		}
	}
}