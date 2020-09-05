#region License
/// Scraps - Scrap.TF Raffle Bot
/// Copyright(C) 2020  Caprine Logic

/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.

/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
/// GNU General Public License for more details.

/// You should have received a copy of the GNU General Public License
/// along with this program. If not, see <https://www.gnu.org/licenses/>.
#endregion License

using System;

namespace Scraps.Common.Models
{
	public class LatestRelease
	{
		public string url { get; set; }
		public string assets_url { get; set; }
		public string upload_url { get; set; }
		public string html_url { get; set; }
		public int id { get; set; }
		public string node_id { get; set; }
		public string tag_name { get; set; }
		public string target_commitish { get; set; }
		public string name { get; set; }
		public bool draft { get; set; }
		public Author author { get; set; }
		public bool prerelease { get; set; }
		public DateTime created_at { get; set; }
		public DateTime published_at { get; set; }
		public Asset[] assets { get; set; }
		public string tarball_url { get; set; }
		public string zipball_url { get; set; }
		public string body { get; set; }
	}

	public class Author
	{
		public string login { get; set; }
		public int id { get; set; }
		public string node_id { get; set; }
		public string avatar_url { get; set; }
		public string gravatar_id { get; set; }
		public string url { get; set; }
		public string html_url { get; set; }
		public string followers_url { get; set; }
		public string following_url { get; set; }
		public string gists_url { get; set; }
		public string starred_url { get; set; }
		public string subscriptions_url { get; set; }
		public string organizations_url { get; set; }
		public string repos_url { get; set; }
		public string events_url { get; set; }
		public string received_events_url { get; set; }
		public string type { get; set; }
		public bool site_admin { get; set; }
	}

	public class Asset
	{
		public string url { get; set; }
		public int id { get; set; }
		public string node_id { get; set; }
		public string name { get; set; }
		public object label { get; set; }
		public Uploader uploader { get; set; }
		public string content_type { get; set; }
		public string state { get; set; }
		public int size { get; set; }
		public int download_count { get; set; }
		public DateTime created_at { get; set; }
		public DateTime updated_at { get; set; }
		public string browser_download_url { get; set; }
	}

	public class Uploader
	{
		public string login { get; set; }
		public int id { get; set; }
		public string node_id { get; set; }
		public string avatar_url { get; set; }
		public string gravatar_id { get; set; }
		public string url { get; set; }
		public string html_url { get; set; }
		public string followers_url { get; set; }
		public string following_url { get; set; }
		public string gists_url { get; set; }
		public string starred_url { get; set; }
		public string subscriptions_url { get; set; }
		public string organizations_url { get; set; }
		public string repos_url { get; set; }
		public string events_url { get; set; }
		public string received_events_url { get; set; }
		public string type { get; set; }
		public bool site_admin { get; set; }
	}

}
