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
			var html = await HttpClientWrapper.GetDocumentAsync($"http://jizen.kuronekoyamato.co.jp/jizen/servlet/crjz.b.NQ0010?id={number}");
			return html.DocumentNode.QuerySelector("table.meisai") != null;
		}

		public static async Task<DeliveryStatus> GetCurrentStatus(string number) {
			var html = await HttpClientWrapper.GetDocumentAsync($"http://jizen.kuronekoyamato.co.jp/jizen/servlet/crjz.b.NQ0010?id={number}");
			var history = html.DocumentNode.QuerySelector("table.meisai").QuerySelectorAll("tr");
			var tds = history.Last().QuerySelectorAll("td");

			return new DeliveryStatus(tds[1].InnerText, DateTime.Parse($"{DateTime.Now.Year}/{tds[2].InnerText} {tds[3].InnerText}:00"));
		}
	}
}
