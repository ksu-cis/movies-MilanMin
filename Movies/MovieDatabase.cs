using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace Movies
{
    /// <summary>
    /// A class representing a database of movies
    /// </summary>
    public class MovieDatabase
    {
        private List<Movie> movies = new List<Movie>();

        /// <summary>
        /// Loads the movie database from the JSON file
        /// </summary>
        public MovieDatabase() {
            
            using (StreamReader file = System.IO.File.OpenText("movies.json"))
            {
                string json = file.ReadToEnd();
                movies = JsonConvert.DeserializeObject<List<Movie>>(json);
            }
        }

        public List<Movie> All { get { return movies; } }

        public List<Movie> SearchAndFilter(string searchString, List<string> rating)
        {
            // Case 0: No search string, no ratings
            if (searchString == null && rating.Count == 0) return All;

            List<Movie> results = new List<Movie>();
            
            foreach(Movie movie in movies)
            {
                // Case 1: Search string AND ratings
                if(rating.Count > 0 && searchString != null)
                {
                    if(movie.Title != null
                        && movie.Title.Contains(searchString, StringComparison.InvariantCultureIgnoreCase)
                        && rating.Contains(movie.MPAA_Rating))
                    {
                        results.Add(movie);
                    }
                }

                // Case 2: Search string only
                else if (searchString != null)
                {
                    if (movie.Title != null && movie.Title.Contains(searchString, StringComparison.InvariantCultureIgnoreCase))
                    {
                        results.Add(movie);
                    }
                }
                
                // Case 3: Ratings only
                else if (rating.Count > 0)
                {
                    if (rating.Contains(movie.MPAA_Rating))
                    {
                        results.Add(movie);
                    }
                }
            }

            /*foreach(Movie movie in movies)
            {
                string release = movie.Release_Date;
                release = release.Split(" ")[2];

                if (Convert.ToInt32(release) < 1930)
                {
                    results.Add(movie);
                }
            }*/

            return results;
        }
    }
}
