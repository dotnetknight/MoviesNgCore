using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class GetSearchResults
    {
        public int vote_count { get; set; }
        public int id { get; set; }
        public bool video { get; set; }
        public double vote_average { get; set; }
        public string title { get; set; }
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public List<int> genre_ids { get; set; }
        public string backdrop_path { get; set; }
        public bool adult { get; set; }
        public string overview { get; set; }
        public string release_date { get; set; }
    }

    public class SearchResultRootObject
    {
        public int page { get; set; }
        public int total_results { get; set; }
        public int total_pages { get; set; }
        public List<GetSearchResults> results { get; set; }
    }

    public class GetSearchResultsData
    {
        public static List<GetSearchResults> SearchResults = new List<GetSearchResults>();
        public static List<GetSearchResults> GetData(string query)
        {
            string url = @"https://api.themoviedb.org/3/search/movie?api_key=01eb700d027839712c6317e7a1a9bc32&language=en-US&query=" + query + "&page=1&include_adult=false";
            string JSonString = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) { using (Stream stream = response.GetResponseStream()) { using (StreamReader reader = new StreamReader(stream)) { JSonString = reader.ReadToEnd(); reader.Close(); stream.Close(); } } }

            var list = JsonConvert.DeserializeObject<SearchResultRootObject>(JSonString);
            SearchResults = list.results.ToList();
            return SearchResults;
        }
    }
}
