using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace DeliveryTracker.Institutions {
	public static class Sagawa {
		public static TrackingType TrackingType = TrackingType.Sagawa;

		public static async Task<bool> IsTrackingNumberAsync(string number) {
			var html = await HttpClientWrapper.GetDocumentAsync($"https://trackings.post.japanpost.jp/services/srv/search/direct?reqCodeNo1={number}&locale=ja");
			return html.DocumentNode.QuerySelectorAll("#detail1 table")[1].InnerText.Trim() != "";
		}

		public static async Task<DeliveryStatus> GetCurrentStatus(string number) {
			var html = await HttpClientWrapper.GetDocumentAsync($"https://trackings.post.japanpost.jp/services/srv/search/direct?reqCodeNo1={number}&locale=ja");
			var history = html.DocumentNode.QuerySelectorAll("#detail table")[1].QuerySelectorAll("tr");
			var tds = history.Last().QuerySelectorAll("td");

			return new DeliveryStatus(tds[0].InnerText.Trim().Replace("⇒",""), DateTime.Parse($"{DateTime.Now.Year}/{tds[1].InnerText}:00"));
		}
	}
}
