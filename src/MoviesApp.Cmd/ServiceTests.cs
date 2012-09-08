using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoviesApp.Core;
using WatTmdb.V3;

namespace MoviesApp.Cmd
{
    [TestFixture]
    public class ServiceTests
    {
        Tmdb api;

        [SetUp]
        public void Setup()
        {
            api = new Tmdb("e7ea08e0ed9aba51ea90d5ffe68fa672");
        }

        [Test]
        public void SearchPerson()
        {
            var result = api.SearchPerson("Cheadle", 1);

            Assert.Greater(result.results.Count, 1);

            foreach (var person in result.results)
            {
                Console.WriteLine(person.name + " - " + person.id + " - " + person.profile_path);
            }
        }

        [Test]
        public void GetPersonImages()
        {
            var result = api.GetPersonImages(1896);

            Assert.Greater(result.profiles.Count, 0);

            foreach (var picture in result.profiles)
            {
                Console.WriteLine(
                    picture.file_path + " - " + picture.width + "x" + picture.height + " - " +
                    picture.aspect_ratio + " - " + picture.iso_639_1);
            }
        }

        [Test]
        public void GetPersonInfo()
        {
            var result = api.GetPersonInfo(1896);

            Assert.IsNotNull(result);

            Console.WriteLine("Id: " + result.id);
            Console.WriteLine("Name: " + result.name);
            Console.WriteLine("Picture: " + result.profile_path);
            Console.WriteLine("Birthday: " + result.birthday + " - " + result.deathday);
            Console.WriteLine("Birthplace: " + result.place_of_birth);
            Console.WriteLine("Bio: " + result.biography);
        }

        [Test]
        public void GetTopRatedMovies()
        {
            var result = api.GetTopRatedMovies(1);

            Assert.Greater(result.results.Count, 1);

            foreach (var movie in result.results)
            {
                Console.WriteLine(movie.title + " - " + movie.id + " - " + movie.poster_path + " - " + movie.release_date);
            }
        }

        [Test]
        public void GetPopularMovies()
        {
            var result = api.GetPopularMovies(1);

            Assert.Greater(result.results.Count, 1);

            foreach (var movie in result.results)
            {
                Console.WriteLine(movie.title + " - " + movie.id + " - " + movie.poster_path + " - " + movie.release_date);
            }
        }

        [Test]
        public void GetMovieTrailers()
        {
            var result = api.GetMovieTrailers(49049);

            Assert.Greater(result.youtube.Count, 0);

            foreach (var youtube in result.youtube)
            {
                Console.WriteLine(youtube.name + " - " + youtube.source);
            }
        }

        [Test]
        public void GetMovieKeywords()
        {
            var result = api.GetMovieKeywords(49049);

            Assert.Greater(result.keywords.Count, 0);

            foreach (var keyword in result.keywords)
            {
                Console.WriteLine(keyword.id + " - " + keyword.name);
            }
        }

        [Test]
        public void GetGenres()
        {
            var result = api.GetGenreList();

            Assert.Greater(result.genres.Count, 0);

            foreach (var genre in result.genres)
            {
                Console.WriteLine(genre.id + " - " + genre.name);
            }
        }

        [Test]
        public void GetConfiguration()
        {
            var result = api.GetConfiguration();
            Assert.IsNotNull(result);
            Console.WriteLine("BaseUrl: " + result.images.base_url);
        }

        [Test]
        public void GetGenreMovies()
        {
            var result = api.GetGenreMovies(9648, 1);

            Assert.Greater(result.results.Count, 0);

            foreach (var movie in result.results)
            {
                Console.WriteLine(movie.id + " - " + movie.title);
            }
        }

        [Test]
        public void GetMovieByImdb()
        {
            var result = api.GetMovieByIMDB("tt1764651");
            Assert.IsNotNull(result);
            PrintMovie(result);
        }

        [Test]
        public void GetMovieCast()
        {
            var result = api.GetMovieCast(49049);

            Assert.Greater(result.cast.Count, 0);

            foreach (var movie in result.cast)
            {
                Console.WriteLine(movie.name + " - " + movie.id + " - " + movie.profile_path);
            }

            Assert.Greater(result.cast.Count, 0);
            Console.WriteLine();
            Console.WriteLine();

            foreach (var movie in result.crew)
            {
                Console.WriteLine(movie.name + " - " + movie.id + " - " + movie.job + " - " + movie.department);
            }
        }

        [Test]
        public void GetMovieImages()
        {
            var result = api.GetMovieImages(49049);

            Assert.Greater(result.posters.Count, 0);

            foreach (var picture in result.posters)
            {
                Console.WriteLine(
                    picture.file_path + " - " + picture.width + "x" + picture.height + " - " +
                    picture.aspect_ratio + " - " + picture.iso_639_1);
            }

            Console.WriteLine();
            Console.WriteLine();

            foreach (var picture in result.backdrops)
            {
                Console.WriteLine(
                    picture.file_path + " - " + picture.width + "x" + picture.height + " - " +
                    picture.aspect_ratio + " - " + picture.iso_639_1);
            }
        }

        [Test]
        public void GetMovieInfo()
        {
            var result = api.GetMovieInfo(49049);
            Assert.IsNotNull(result);
            PrintMovie(result);
        }

        private static void PrintMovie(TmdbMovie result)
        {
            Console.WriteLine("Id: " + result.id);
            Console.WriteLine("Title: " + result.title);
            Console.WriteLine("BackdropPath: " + result.backdrop_path);
            Console.WriteLine("Budget: " + result.budget);
            Console.WriteLine("Genres: " + result.genres.JoinStrings(g => g.name, ","));
            Console.WriteLine("Homepage: " + result.homepage);
            Console.WriteLine("Imdb: " + result.imdb_id);
            Console.WriteLine("Overview: " + result.overview);
            Console.WriteLine("Popularity: " + result.popularity);
            Console.WriteLine("PosterPath: " + result.poster_path);
            Console.WriteLine("Companies: " + result.production_companies.JoinStrings(p => p.name, ","));
            Console.WriteLine("Countries: " + result.production_countries.JoinStrings(p => p.name, ","));
            Console.WriteLine("ReleaseDate: " + result.release_date);
            Console.WriteLine("Revenue: " + result.revenue);
            Console.WriteLine("Runtime: " + result.runtime);
            Console.WriteLine("Title: " + result.spoken_languages.JoinStrings(s => s.name, ","));
            Console.WriteLine("Tagline: " + result.tagline);
            Console.WriteLine("VoteAverage: " + result.vote_average);
            Console.WriteLine("VoteCount: " + result.vote_count);
            Console.WriteLine("Adult: " + result.adult);
            Console.WriteLine("OriginalTitle: " + result.original_title);
        }

        [Test]
        public void GetPersonCredits()
        {
            var result = api.GetPersonCredits(1896);

            Assert.Greater(result.cast.Count, 0);

            foreach (var movie in result.cast)
            {
                Console.WriteLine(movie.title + " - " + movie.id + " - " + movie.poster_path + " - " + movie.release_date);
            }

            Assert.Greater(result.cast.Count, 0);
            Console.WriteLine();
            Console.WriteLine();

            foreach (var movie in result.crew)
            {
                Console.WriteLine(movie.title + " - " + movie.id + " - " + movie.job + " - " + movie.department);
            }
        }

        [Test]
        public void GetUpcomingMovies()
        {
            var result = api.GetUpcomingMovies(1);

            Assert.Greater(result.results.Count, 1);

            foreach (var movie in result.results)
            {
                Console.WriteLine(movie.title + " - " + movie.id + " - " + movie.poster_path + " - " + movie.release_date);
            }
        }

        [Test]
        public void GetNowPlayingMovies()
        {
            var result = api.GetNowPlayingMovies(1);

            Assert.Greater(result.results.Count, 1);

            foreach (var movie in result.results)
            {
                Console.WriteLine(movie.title + " - " + movie.id + " - " + movie.poster_path + " - " + movie.release_date);
            }
        }

        [Test]
        public void GetSimilarMovies()
        {
            var result = api.GetSimilarMovies(603, 1);

            Assert.Greater(result.results.Count, 1);

            foreach (SimilarMovie movie in result.results)
            {
                Console.WriteLine(movie.title + " - " + movie.id + " - " + movie.poster_path + " - " + movie.release_date);
            }
        }

        [Test]
        public void SearchMovie()
        {
            var result = api.SearchMovie("Redemption", 1);

            Assert.Greater(result.results.Count, 1);

            foreach (var movie in result.results)
            {
                Console.WriteLine(movie.title + " - " + movie.id + " - " + movie.poster_path + " - " + movie.release_date);
            }
        }
    }
}
