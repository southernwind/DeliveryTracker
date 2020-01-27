using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace DeliveryTracker.Institutions {
	public static class JapanPost {
		public static TrackingType TrackingType = TrackingType.JapanPost;

		static JapanPost() {
		}

		public static async Task<bool> IsTrackingNumberAsync(string number) {
			var html = await HttpClientWrapper.GetDocumentAsync($"https://trackings.post.japanpost.jp/services/srv/search/direct?reqCodeNo1={number}&locale=ja");
			return html.DocumentNode.QuerySelector("table[summary='履歴情報']") != null;
		}
	}
}
