using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieApp.Models
{
    public class BelongsToCollection
    {
        public int id { get; set; }
        public string name { get; set; }
        public string poster_path { get; set; }
        public string backdrop_path { get; set; }
    }

    public class Genre
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class ProductionCompany
    {
        public string name { get; set; }
        public int id { get; set; }
    }

    public class ProductionCountry
    {
        public string iso_3166_1 { get; set; }
        public string name { get; set; }
    }

    public class SpokenLanguage
    {
        public string iso_639_1 { get; set; }
        public string name { get; set; }
    }

    public class GetMovieDataRootObject
    {
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public BelongsToCollection belongs_to_collection { get; set; }
        public int budget { get; set; }
        public List<Genre> genres { get; set; }
        public string homepage { get; set; }
        public int id { get; set; }
        public string imdb_id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public List<ProductionCompany> production_companies { get; set; }
        public List<ProductionCountry> production_countries { get; set; }
        public string release_date { get; set; }
        public int revenue { get; set; }
        public int runtime { get; set; }
        public List<SpokenLanguage> spoken_languages { get; set; }
        public string status { get; set; }
        public string tagline { get; set; }
        public string title { get; set; }
        public bool video { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
    }
    public static class GetMovieData
    {
        public static string MovieTitle;
        public static string ReleaseDate;
        public static string PosterPath;
        public static string Overview;
        public static string TagLine;
        public static string Lines;
        public static string Genres;
        public static string Runtime;
        public static string DirectorName = "";
        public static string ScreenPlay = "";
        public static string Composer = "";
        public static string Writer = "";
        public static int CastCount = 0;
        public static int CrewCount = 0;
        public static int ScreenPlaysCount = 0;
        public static int WritersCount = 0;
        public static int DirectorJobCount = 0;
        public static int ScreenPlayJobCount = 0;
        public static int ComposerJobCount = 0;
        public static int WriterJobCount = 0;
        public static int Revenue;
        public static int Budget;
        public static List<GetCredit.Cast> ActorName;
        public static List<GetCredit.Crew> FullCrewData;
        public static List<string> GetData(int MovieId)
        {
            string TrailerVideoUrl = "";
            string DirectorName = "";
            string ScreenPlay = "";
            string Composer = "";
            string Writer = "";

            string url = "https://api.themoviedb.org/3/movie/" + MovieId + "?api_key=01eb700d027839712c6317e7a1a9bc32&language=en-US";
            string JSonString = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) { using (Stream stream = response.GetResponseStream()) { using (StreamReader reader = new StreamReader(stream)) { JSonString = reader.ReadToEnd(); reader.Close(); stream.Close(); } } }

            var MovieData = JsonConvert.DeserializeObject<GetMovieDataRootObject>(JSonString);

            string CastUrl = "https://api.themoviedb.org/3/movie/" + MovieId + "/credits?api_key=01eb700d027839712c6317e7a1a9bc32";
            string CastJSonString = "";
            HttpWebRequest CastRequest = (HttpWebRequest)WebRequest.Create(CastUrl);
            CastRequest.Method = "GET";

            using (HttpWebResponse CastResponse = (HttpWebResponse)CastRequest.GetResponse()) { using (Stream CastStream = CastResponse.GetResponseStream()) { using (StreamReader CastDataReader = new StreamReader(CastStream)) { CastJSonString = CastDataReader.ReadToEnd(); } } }

            var CastData = JsonConvert.DeserializeObject<GetCredit.CreditRootObject>(CastJSonString);

            string TrailerUrl = "https://api.themoviedb.org/3/movie/" + MovieId + "/videos?api_key=01eb700d027839712c6317e7a1a9bc32&language=en-US";
            string TrailerJSon = "";

            HttpWebRequest VideoRequest = (HttpWebRequest)WebRequest.Create(TrailerUrl);
            VideoRequest.Method = "GET";

            using (HttpWebResponse VideoResponse = (HttpWebResponse)VideoRequest.GetResponse()) { using (Stream stream = VideoResponse.GetResponseStream()) { using (StreamReader reader = new StreamReader(stream)) { TrailerJSon = reader.ReadToEnd(); reader.Close(); stream.Close(); } } }
            var TrailerData = JsonConvert.DeserializeObject<TrailerRootObject>(TrailerJSon);
            if (TrailerData.results != null && TrailerData.results.Count != 0)
            {
                List<GetTrailer> TrailerDataList = new List<GetTrailer>();
                TrailerDataList = TrailerData.results.ToList();
                int RandomTrailerIndex = 0;
                if (TrailerDataList.Count == 1) { RandomTrailerIndex = 0; }
                if (TrailerDataList.Count > 1) { Random RandomTrailer = new Random(); RandomTrailerIndex = RandomTrailer.Next(0, TrailerDataList.Count); }
                TrailerVideoUrl = "https://www.youtube.com/embed/" + TrailerDataList[RandomTrailerIndex].key;
            }

            string j = "";
            for (int i = 0; i < MovieData.genres.Count; i++) { j += MovieData.genres[i].name.ToString() + ", "; }
            string myString = j;
            myString = myString.Remove(myString.Length - 2);
            Genres = myString;

            DirectorJobCount = 0;
            ScreenPlayJobCount = 0;
            ComposerJobCount = 0;
            WriterJobCount = 0;
            for (int i = 0; i < CastData.crew.Count; i++)
            {
                if (CastData.crew[i].job == "Director") { DirectorJobCount++; DirectorName += CastData.crew[i].name + ", "; }
                if (CastData.crew[i].job == "Screenplay") { ScreenPlayJobCount++; ScreenPlay += CastData.crew[i].name + ", "; }
                if (CastData.crew[i].job == "Original Music Composer") { ComposerJobCount++; Composer += CastData.crew[i].name + ", "; }
                if (CastData.crew[i].job == "Writer") { WriterJobCount++; Writer += CastData.crew[i].name + ", "; }
            }
            ScreenPlaysCount = ScreenPlayJobCount;
            WritersCount = WriterJobCount;

            if (DirectorJobCount >= 1) { DirectorName = DirectorName.Remove(DirectorName.Length - 2); }
            if (ScreenPlayJobCount >= 1) { ScreenPlay = ScreenPlay.Remove(ScreenPlay.Length - 2); }
            if (ComposerJobCount >= 1) { Composer = Composer.Remove(Composer.Length - 2); }
            if (WriterJobCount >= 1) { Writer = Writer.Remove(Writer.Length - 2); }

            int Hours;
            int Minutes;
            int SumMins;
            int MinutesToConvert = MovieData.runtime;
            Hours = MinutesToConvert / 60;
            Minutes = MovieData.runtime;
            SumMins = Minutes - (Hours * 60);
            Runtime = Hours.ToString() + "h " + SumMins.ToString() + "m";

            PosterPath = "http://image.tmdb.org/t/p/original" + MovieData.poster_path;

            List<string> GetMovieList = new List<string> { MovieData.title, PosterPath, MovieData.original_language, Runtime, MovieData.release_date, DirectorName, ScreenPlay, Writer, Composer, MovieData.budget.ToString(), MovieData.revenue.ToString(), Genres, MovieData.overview, TrailerVideoUrl };
            for (int i = 0; i < GetMovieList.Count; i++) { if (GetMovieList[i] == "" || GetMovieList[i] == null) { GetMovieList[i] = "Unknown"; } }
            ActorName = CastData.cast.ToList();

            return GetMovieList;
        }
    }
}