using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryTracker.Database.Tables {
	public class TrackingNumber {
		/// <summary>
		/// 追跡番号
		/// </summary>
		public string Number {
			get;
			set;
		}

		/// <summary>
		/// 状態
		/// </summary>
		public string State {
			get;
			set;
		}

		/// <summary>
		/// 配送機関
		/// </summary>
		public int Institution {
			get;
			set;
		}
	}
}
