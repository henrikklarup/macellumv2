using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using HtmlAgilityPack;
using MoreLinq;

namespace Macellum_Remake.Models
{
    /// <summary>
    /// Database functions using linq
    /// </summary>
    public class DatabaseRepo
    {
        /// <summary>
        /// Database repo
        /// </summary>
        readonly DBConnectionContainer _repo = new DBConnectionContainer();

        //Session methods
        #region Session
        /// <summary>
        /// Get Session Id by Username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>The users current session id</returns>
        public string GetSessionId(string username)
        {
            var usrId = GetUserByName(username).Id.ToString(CultureInfo.InvariantCulture);
            var activeSessionId = _repo.ActiveSessionIds.FirstOrDefault(s => s.UserId == usrId);
            return activeSessionId != null ? activeSessionId.SessionId : null;
        }

        /// <summary>
        /// Save session id to database
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="sessionId">Session id to save</param>
        /// <returns>True</returns>
        public bool SaveSessionId(string username, string sessionId)
        {
            var usrId = GetUserByName(username).Id.ToString(CultureInfo.InvariantCulture);
            if (_repo.ActiveSessionIds.Any(s => s.UserId == usrId))
            {
                var activeSessionId = _repo.ActiveSessionIds.FirstOrDefault(s => s.UserId == usrId);
                if (activeSessionId != null)
                    activeSessionId.SessionId = sessionId;
            }
            else
            {
                _repo.ActiveSessionIds.Add(new ActiveSessionId { SessionId = sessionId, UserId = usrId });
            }
            Save();
            return true;
        }
        #endregion

        //User methods
        #region User
        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>The user with the given username</returns>
        public User GetUserByName(string username)
        {
            var returnUser = _repo.Users.FirstOrDefault(s => username.Equals(s.Username));
            return returnUser;
        }

        /// <summary>
        /// Get user by Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>The user with the given user id</returns>
        public User GetUserById(int id)
        {
            return _repo.Users.FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// Add a user to the database
        /// </summary>
        /// <param name="user">The user object to add</param>
        /// <returns></returns>
        public bool AddUser(User user)
        {
            var isUserInDb = _repo.Users.Any(s => s.Username.Equals(user.Username.ToLower()));
            if (isUserInDb) return false;
            user.Role = _repo.Roles.FirstOrDefault(s => s.Name.Equals("User"));
            user.Username = user.Username.ToLower();
            //HASH PASSWORD
            var hashedPas = Helper.HashPassword(user.Password.pass);
            user.Password.pass = hashedPas;
            user.Password.ConfirmPassword = hashedPas;

            _repo.Users.Add(user);
            Save();
            return true;
        }

        /// <summary>
        /// Authinticate user with database
        /// </summary>
        /// <param name="user">The user to be authenticated</param>
        /// <returns>True if user is in database</returns>
        public bool AuthenticateUser(User user)
        {
            var passHashed = Helper.HashPassword(user.Password.pass);
            return _repo.Users.Any(s => s.Username.Equals(user.Username) && s.Password.pass.Equals(passHashed));
        }
        #endregion

        //Role methods
        #region Role
        /// <summary>
        /// Get role by username
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns>String with role name of the given user</returns>
        public string GetRoleByUsername(string userName)
        {
            return GetUserByName(userName).Role.Name;
        }
        #endregion

        //Fish methods
        #region Fish

        /// <summary>
        /// Add fish (single)
        /// </summary>
        /// <param name="fish">Fish object to add</param>
        public void AddFisk(Fisk fish)
        {
            _repo.Fisks.Add(fish);
            Save();
        }

        /// <summary>
        /// Add fish (range)
        /// </summary>
        /// <param name="fish">Fish list</param>
        public void AddFisk(IEnumerable<Fisk> fish)
        {
            _repo.Fisks.AddRange(fish);
            Save();
        }

        /// <summary>
        /// Get the last know date where fish where added to the database
        /// </summary>
        /// <returns>String in date format</returns>
        public string GetLastDate()
        {
            var firstOrDefault = _repo.Fisks.OrderByDescending(s => s.Date).FirstOrDefault();
            if (firstOrDefault == null) return null;
            var lastdate = firstOrDefault.Date.ToString(CultureInfo.InvariantCulture);
            return lastdate;
        }

        /// <summary>
        /// Get the newst fish from the database, one of each 'art'
        /// </summary>
        /// <param name="havneId">The current habour id</param>
        /// <returns>IEnumerable of Fisk with the newst fish from the database</returns>
        public IEnumerable<Fisk> GetAllFishNewest(int havneId)
        {
            var fish = _repo.Fisks.Where(s => s.Havn.Id.Equals(havneId)).OrderByDescending(s => s.Date);
            var groupFish = fish.GroupBy(s => new {s.Arter.Name, s.Sort});
            var newestFish = groupFish.Select(s => s.FirstOrDefault());
            return newestFish;
        }

        /// <summary>
        /// Get the fish prices avg price
        /// </summary>
        /// <param name="fangst">The catch</param>
        /// <param name="havneId">The current habour id</param>
        /// <returns>Avg fish price of catch</returns>
        public decimal GetFishPrices(List<Fisk> fangst, int havneId)
        {
            var newestFish = GetAllFishNewest(havneId);

            //Select fish by sort and art id
            var fishByIdAndSort =
                newestFish.Where(newest => fangst.Any(fangstLinq => newest.Arter.Id.Equals(fangstLinq.Arter.Id)
                                                                      && newest.Sort.Equals(fangstLinq.Sort)));
            //Edit the amount on the fish selected
            var selectFish = fishByIdAndSort.Select(s =>
            {
                var firstOrDefault = fangst.FirstOrDefault(x => x.Arter.Id.Equals(s.Arter.Id));
                if (firstOrDefault != null)
                    s.Amount = firstOrDefault.Amount;
                return s;
            });

            //The 'normnal' fish return price
            var returnPrice = selectFish.Sum(s => s.Amount*s.AvgPrice);

            //Turbis fish, when a fish isn't found
            var turbis = fangst.ExceptBy(fishByIdAndSort, s => s.Arter.Id);

// ReSharper disable PossibleNullReferenceException
            var turbisAvgPrice = GetArtFromName("SkidtfiskLUNDE").Fisks.FirstOrDefault().AvgPrice;
// ReSharper restore PossibleNullReferenceException
            var turbisPrice = turbis.Sum(s => s.Amount)*turbisAvgPrice;

            //Final Price
            return returnPrice + turbisPrice;
        }

        /// <summary>
        /// Get the fish prices max price
        /// </summary>
        /// <param name="fangst">The catch</param>
        /// <param name="havneId">The current habour id</param>
        /// <returns>Max fish price of catch</returns>
        public decimal GetFishPricesMax(List<Fisk> fangst, int havneId)
        {
            var newestFish = GetAllFishNewest(havneId);

            //Select fish by sort and art id
            var fishByIdAndSort =
                newestFish.Where(newest => fangst.Any(fangstLinq => newest.Arter.Id.Equals(fangstLinq.Arter.Id)
                                                                      && newest.Sort.Equals(fangstLinq.Sort)));
            //Edit the amount on the fish selected
            var selectFish = fishByIdAndSort.Select(s =>
            {
                var firstOrDefault = fangst.FirstOrDefault(x => x.Arter.Id.Equals(s.Arter.Id));
                if (firstOrDefault != null)
                    s.Amount = firstOrDefault.Amount;
                return s;
            });

            //The 'normnal' fish return price
            var returnPrice = selectFish.Sum(s => s.Amount * s.MaxPrice);

            //Turbis fish, when a fish isn't found
            var turbis = fangst.ExceptBy(fishByIdAndSort, s => s.Arter.Id);

            var turbisAvgPrice = GetArtFromName("SkidtfiskLUNDE").Fisks.FirstOrDefault().AvgPrice;
            var turbisPrice = turbis.Sum(s => s.Amount) * turbisAvgPrice;

            //Final Price
            return returnPrice + turbisPrice;
        }
        #endregion

        //Arter methods
        #region Arter 

        /// <summary>
        /// Return all 'arter' from the database
        /// </summary>
        /// <returns>IEnumerable of arter from the database</returns>
        public IEnumerable<Arter> GetAllArter()
        {
            return _repo.Arters;
        }

        /// <summary>
        /// Get art from name
        /// </summary>
        /// <param name="art">Art name</param>
        /// <returns>Arter object from database</returns>
        /// TODO REGEX
        public Arter GetArtFromName(string art)
        {
            var artTest = art.Trim().ToLower().Substring(0, art.Trim().Length < 8 ? art.Trim().Length : 7);

            var artElements = new List<ArtTableElement>();

            _repo.Arters.ToList().ForEach(x => artElements.Add(new ArtTableElement(x.Id, x.Name)));

            var k = artElements.FirstOrDefault(s => s.Name.Trim().ToLower().Substring(0, s.Name.Trim().Length < 8 ? s.Name.Trim().Length : 7) == artTest);
            return k == null ? null : _repo.Arters.FirstOrDefault(s => s.Id == k.Id);
        }

        #region ArtTableElement Class
        /// <summary>
        /// ArtTableElement Helper class
        /// </summary>
        internal class ArtTableElement
        {
            public int Id;
            public string Name;

            public ArtTableElement(int id, string name)
            {
                Id = id;
                Name = name;
            }
        }
        #endregion

        /// <summary>
        /// Get art from id
        /// </summary>
        /// <param name="id">Art id</param>
        /// <returns>Arter object from database</returns>
        public Arter GetArtFromId(int id)
        {
            return _repo.Arters.FirstOrDefault(s => s.Id == id);
        }

        /// <summary>
        /// Add new Art to database
        /// </summary>
        /// <param name="art">Art to add</param>
        /// <returns>The art which was added</returns>
        public Arter AddArt(Arter art)
        {
            var retArt = _repo.Arters.Add(art);
            Save();
            return retArt;
        }
        #endregion

        //Trip methods
        #region Trip
        /// <summary>
        /// Add a trip to database
        /// </summary>
        /// <param name="trip">The trip to add</param>
        public void AddTrip(Trip trip)
        {
            _repo.Trips.Add(trip);
            Save();
        }

        /// <summary>
        /// Check if a user has open trips
        /// </summary>
        /// <param name="user">User name</param>
        /// <returns>True if the user has open trips</returns>
        public bool UserHasOpenTrip(string user)
        {
            if (string.IsNullOrEmpty(user)) return false;
            var usr = _repo.Users.FirstOrDefault(s => s.Username.Equals(user));
            return usr != null && usr.Trips.Any(s => s.Open);
        }

        /// <summary>
        /// Get the current trip from the username
        /// </summary>
        /// <param name="user">Username</param>
        /// <returns>The current open trip of the user</returns>
        public Trip GetCurrentTrip(string user)
        {
            var dbUser = GetUserByName(user);
            return dbUser.Trips.FirstOrDefault(s => s.Open);
        }

        /// <summary>
        /// Update the trip with a new fishlist
        /// </summary>
        /// <param name="trip">The trip to update</param>
        /// <returns>The trip from the database</returns>
        public Trip UpdateTrip(Trip trip)
        {
            var dbTrip = _repo.Trips.FirstOrDefault(s => s.Id == trip.Id);
            if (dbTrip != null) dbTrip.FishList = trip.FishList;
            Save();
            return dbTrip;
        }

        /// <summary>
        /// Get all trips of a user
        /// </summary>
        /// <param name="user">Username</param>
        /// <returns>List of users trips</returns>
        public List<Trip> GetAllTrips(string user)
        {
            var dbUsr = GetUserByName(user);
            return dbUsr.Trips.ToList();
        }

        /// <summary>
        /// Get trip by trip id
        /// </summary>
        /// <param name="id">Trip id</param>
        /// <returns>Trip which matches the trip id</returns>
        public Trip GetTripById(int id)
        {
            return _repo.Trips.FirstOrDefault(s => s.Id == id);
        }

        #endregion

        //Havne methods
        #region Havn
        /// <summary>
        /// Get the habour id by name
        /// </summary>
        /// <param name="name">Habour name</param>
        /// <returns>Habour id matching the given name</returns>
        public int GetHavnId(string name)
        {
            var firstOrDefault = _repo.Havns.FirstOrDefault(s => s.Name == name);
            if (firstOrDefault != null)
                return firstOrDefault.Id;
            return -1;
        }

        /// <summary>
        /// Get habour from id
        /// </summary>
        /// <param name="id">Habour id</param>
        /// <returns>The habour object matching the id</returns>
        public Havn GetHavnFromId(int id)
        {
            return _repo.Havns.FirstOrDefault(s => s.Id == id);
        }
        #endregion

        //Data methods
        #region Data
        /// <summary>
        /// Get a list of fish from a website
        /// </summary>
        /// <param name="url">The website url</param>
        /// <param name="havnId">Habour id</param>
        /// <returns>A list of fish</returns>
        public IEnumerable<Fisk> GetContent(string url, int havnId)
        {
            var allFish = new List<Fisk>();
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var nodes = doc.DocumentNode.SelectNodes("//table/tr");
            var table = new DataTable("MyTable");

            table.Columns.Add("Art");
            table.Columns.Add("Sort");
            table.Columns.Add("Gns.Pris");
            table.Columns.Add("Max.Kr");
            table.Columns.Add("Mængde");


            var rows = nodes.Skip(8).Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToArray());

            foreach (var row in rows.TakeWhile(row => row.Count() >= 5))
            {
                table.Rows.Add(row);
                var art = GetArtFromName(HttpUtility.HtmlDecode(row[0]).Normalize().Trim());
                if (art == null)
                {
                    var artToAdd = new Arter { Name = HttpUtility.HtmlDecode(row[0]).Normalize().Trim() };
                    art = AddArt(artToAdd);
                }
                var havn = GetHavnFromId(havnId);
                var dato = DateTime.UtcNow;
                var addFish = new Fisk
                {
                    Arter = art,
                    Havn = havn,
                    Date = dato,
                    AvgPrice =
                        decimal.Parse(
                            HttpUtility.HtmlDecode(row[2]).Normalize(NormalizationForm.FormKC).Split(' ')[0].Replace(
                                ',', '.')),
                    MaxPrice =
                        decimal.Parse(
                            HttpUtility.HtmlDecode(row[3]).Normalize(NormalizationForm.FormKC).Split(' ')[0].Replace(
                                ',', '.')),
                    Amount =
                        decimal.Parse(
                            HttpUtility.HtmlDecode(row[4]).Normalize(NormalizationForm.FormKC).Split(' ')[0].Replace(
                                '.', ',')),
                    Sort = int.Parse(HttpUtility.HtmlDecode(row[1]).Normalize(NormalizationForm.FormKC))
                };
                allFish.Add(addFish);
            }

            return allFish;
        }

        /// <summary>
        /// Get a list of fish from a website
        /// </summary>
        /// <param name="url">The website url</param>
        /// <param name="havnId">Habour id</param>
        /// <returns>A list of fish</returns>
        public IEnumerable<Fisk> GetContentFiskeContent(string url, int havnId)
        {
            var allFish = new List<Fisk>();
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var nodes = doc.DocumentNode.SelectNodes("//table/tr");
            var table = new DataTable("MyTable");

            table.Columns.Add("Havn");
            table.Columns.Add("Art");
            table.Columns.Add("Sort");
            table.Columns.Add("Gns.Pris");
            table.Columns.Add("Max.Kr");
            table.Columns.Add("Mængde");


            var rows = nodes.Skip(5).Select(tr => tr.Elements("td").Select(td => td.InnerText.Trim()).ToArray());

            foreach (var row in rows.TakeWhile(row => row.Count() >= 3))
            {
                table.Rows.Add(row);
                var art = GetArtFromName(HttpUtility.HtmlDecode(row[1]));
                if (art == null)
                {
                    var artToAdd = new Arter { Name = HttpUtility.HtmlDecode(row[1]).Normalize().Trim() };
                    art = AddArt(artToAdd);
                }
                var havn = GetHavnFromId(havnId);
                var dato = DateTime.UtcNow;
                var addFish = new Fisk
                {
                    Arter = art,
                    Havn = havn,
                    Date = dato,

                };
                addFish.AvgPrice =
                    decimal.Parse(
                        HttpUtility.HtmlDecode(row[3]).Normalize(NormalizationForm.FormKC).Split(' ')[1].Replace(
                            ',', '.'));
                addFish.MaxPrice =
                    decimal.Parse(
                        HttpUtility.HtmlDecode(row[4]).Normalize(NormalizationForm.FormKC).Split(' ')[1].Replace(
                            ',', '.'));
                addFish.Amount =
                    decimal.Parse(
                        HttpUtility.HtmlDecode(row[5]).Normalize(NormalizationForm.FormKC).Split(' ')[0].Replace(
                            '.', ','));
                addFish.Sort = int.Parse(HttpUtility.HtmlDecode(row[2]).Normalize(NormalizationForm.FormKC));
                allFish.Add(addFish);
            }

            return allFish;
        }
        #endregion

        //Save database method
        #region Save Datebase
        /// <summary>
        /// Method to save database changes
        /// </summary>
        public void Save()
        {
            _repo.SaveChanges();
        }
        #endregion
    }
}