using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTracker {
	public class DeliveryStatus {
		public string Name {
			get;
		}

		public DateTime Time {
			get;
		}

		public DeliveryStatus(string name, DateTime time) {
			this.Name = name;
			this.Time = time;
		}
	}
}
