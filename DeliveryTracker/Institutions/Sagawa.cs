using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryTracker.Institutions {
	public static class Sagawa {
		public static TrackingType TrackingType = TrackingType.Sagawa;

		public static async Task<bool> IsTrackingNumberAsync(string number) {
			return false;
		}
	}
}
