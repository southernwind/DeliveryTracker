using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace DeliveryTracker.Institutions {
	public static class Yamato {
		public static TrackingType TrackingType = TrackingType.Yamato;

		public static async Task<bool> IsTrackingNumberAsync(string number) {
			return false;
			var html = await HttpClientWrapper.GetDocumentAsync($"https://trackings.post.japanpost.jp/services/srv/search/direct?reqCodeNo1={number}&locale=ja");
			return html.DocumentNode.QuerySelector("table[summary='履歴情報']") != null;
		}

		public static async Task<DeliveryStatus> GetCurrentStatus(string number) {
			var html = await HttpClientWrapper.GetDocumentAsync($"https://trackings.post.japanpost.jp/services/srv/search/direct?reqCodeNo1={number}&locale=ja");
			var history = html.DocumentNode.QuerySelector("table[summary='履歴情報']").QuerySelectorAll("tr");
			var current = history.Reverse().Skip(1).FirstOrDefault();
			var tds = current.QuerySelectorAll("td");

			return new DeliveryStatus(tds[1].InnerText, DateTime.Parse(tds[0].InnerText));
		}
	}
}
