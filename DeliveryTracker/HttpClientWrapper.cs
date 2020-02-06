using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace DeliveryTracker {
	internal static class HttpClientWrapper {
		/// <summary>
		/// HttpClient
		/// </summary>
		private static readonly HttpClient _hc;

		static HttpClientWrapper() {
			_hc = new HttpClient();
		}

		/// <summary>
		/// 引数で渡されたURLのHTMLDocumentを取得する(GET)
		/// </summary>
		/// <param name="url">URL</param>
		/// <returns>取得したHTMLDocument</returns>
		public static async Task<HtmlDocument> GetDocumentAsync(string url) {
			var uri = new Uri(url);
			var request = new HttpRequestMessage {
				Method = HttpMethod.Get,
				RequestUri = uri
			};

			var response = await _hc.SendAsync(request);
			var html = await response.Content.ReadAsStringAsync();

			var hd = new HtmlDocument();
			hd.LoadHtml(html);
			return hd;
		}

		public static async Task<HtmlDocument> GetDocumentAsync(string url, HttpContent content) {
			var uri = new Uri(url);
			var request = new HttpRequestMessage {
				Method = HttpMethod.Post,
				RequestUri = uri,
				Content = content
			};

			var response = await _hc.SendAsync(request);
			var html = await response.Content.ReadAsStringAsync();

			var hd = new HtmlDocument();
			hd.LoadHtml(html);
			return hd;
		}

		/// <summary>
		/// 引数で渡されたURLのHTMLDocumentを取得する(POST)
		/// </summary>
		/// <param name="url">URL</param>
		/// <param name="content">要求本文</param>
		public static async Task PostAsync(string url, HttpContent content) {
			var uri = new Uri(url);
			var request = new HttpRequestMessage {
				Method = HttpMethod.Post,
				RequestUri = uri,
				Content = content
			};

			request.Headers.Add("Referer", "https://moneyforward.com/users/sign_in");
			var response = await _hc.SendAsync(request);
			response.EnsureSuccessStatusCode();
		}
	}
}
