using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Macellum_Remake.Models;
using Newtonsoft.Json;

namespace Macellum_Remake.Controllers
{
    /// <summary>
    /// Home Controller
    /// </summary>
    [Authorize(Roles = "Admin,User")]
    public class HomeController : BaseController
    {
        private readonly DatabaseRepo _repo = new DatabaseRepo();

        // GET: Home
        public ActionResult Index()
        {
            var gfa = new GraphArray();

            var userFish = _repo.GetCurrentTrip(SimpleSessionPersister.Username);
            if (userFish != null)
            {
                var roll = userFish.FishList; //For First Way
                var fangst = JsonConvert.DeserializeObject<List<Fisk>>(roll);
                gfa.GraphNumbers.Add(new GraphInfo("Tyborøn", _repo.GetFishPrices(fangst, 1), _repo.GetFishPricesMax(fangst, 1)));
                gfa.GraphNumbers.Add(new GraphInfo("Hvide Sande", _repo.GetFishPrices(fangst, 2), _repo.GetFishPricesMax(fangst, 2)));
                gfa.GraphNumbers.Add(new GraphInfo("Thorsminde", _repo.GetFishPrices(fangst, 3), _repo.GetFishPricesMax(fangst, 3)));
                gfa.GraphNumbers.Add(new GraphInfo("Hirtshals", _repo.GetFishPrices(fangst, 4), _repo.GetFishPricesMax(fangst, 4)));
                gfa.GraphNumbers.Add(new GraphInfo("Hanstholm", _repo.GetFishPrices(fangst, 5), _repo.GetFishPricesMax(fangst, 5)));
                gfa.GraphNumbers.Add(new GraphInfo("Strandby", _repo.GetFishPrices(fangst, 6), _repo.GetFishPricesMax(fangst, 6)));
                gfa.GraphNumbers = gfa.GraphNumbers.OrderByDescending(s => (s.Value - (s.Value2 - s.Value))).ToList();

                gfa.Date = "'" + userFish.Date + "'";
            }
            else
            {
                gfa.GraphNumbers.Add(new GraphInfo("INTET AT VISE!", 0, 0));
                gfa.Date = "''";
            }
            return View(gfa);
        }

        public ActionResult Help()
        {
            return View();
        }
    }
}