using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DeliveryTracker.Institutions;

namespace DeliveryTracker {
	public enum TrackingType {
		Unknown,
		Yamato,
		Sagawa,
		JapanPost
	}
	public static class Tracking {
		public static async Task<TrackingType> GetTrackingTypeAsync(string number) {
			if (await JapanPost.IsTrackingNumberAsync(number)) {
				return JapanPost.TrackingType;
			}
			if (await Yamato.IsTrackingNumberAsync(number)) {
				return Yamato.TrackingType;
			}
			if (await Sagawa.IsTrackingNumberAsync(number)) {
				return Sagawa.TrackingType;
			}

			return default;
		}

		public static async Task<DeliveryStatus> GetCurrentStatus(string number,TrackingType institution) {
			switch (institution) {
				case TrackingType.Yamato:
					return await Yamato.GetCurrentStatus(number);
				case TrackingType.Sagawa:
					return await Sagawa.GetCurrentStatus(number);
				case TrackingType.JapanPost:
					return await JapanPost.GetCurrentStatus(number);
				default:
					return null;
			}
		}
	}
}
