using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieApp.Controllers
{
    [Route("api/[controller]")]
    public class MoviesController : Controller
    {
        [HttpGet, Route("NowPlaying")]
        public List<GetNowPlaying> NowPlaying()
        {
            List<GetNowPlaying> NowPlaying = new List<GetNowPlaying>();
            NowPlaying = GetNowPlayingData.GetData();
            return NowPlaying;
        }

        [HttpGet, Route("Upcoming")]
        public List<GetUpcoming> Upcoming()
        {
            List<GetUpcoming> UpcomingMovies = new List<GetUpcoming>();
            UpcomingMovies = GetUpcomingData.GetData();
            return UpcomingMovies;
        }

        [HttpGet, Route("Popular")]
        public List<GetPopular> Popular()
        {
            List<GetPopular> PopularMovies = new List<GetPopular>();
            PopularMovies = GetPopularData.GetData();
            return PopularMovies;
        }

        [HttpGet, Route("GetMovieData/{movieId}")]
        public List<string> GetMovieDataList(int movieId)
        {
            List<string> movData = new List<string>();
            movData = GetMovieData.GetData(movieId);
            return movData;
        }

        [HttpGet, Route("GetMovieCast")]
        public List<GetCredit.Cast> GetCast() { return GetMovieData.ActorName; }

        [HttpGet, Route("GetSearchResults/{query}")]
        public List<GetSearchResults> GetSearchResults(string query) { return GetSearchResultsData.GetData(query); }
    }
}
