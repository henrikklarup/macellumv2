using System;
using System.Collections.Generic;

namespace Macellum_Remake.Models
{
    public class TripClass
    {
        private DateTime _turTime;

        public TripClass()
        { }

        public TripClass(List<Fisk> fangst, DateTime turTime, List<Arter> alleArters)
        {
            Fangst = fangst;
            TurTime = turTime;
            AlleArters = alleArters;
        }

        public List<Fisk> Fangst { get; set; }
        public string CurFangst { get; set; }
        public string CurFishId { get; set; }
        public string CurFishSort { get; set; }
        public string CurFishAmount { get; set; }
        public bool TripOpen { get; set; }
        public Trip CurrentTrip { get; set; }
        public string SubmitVal { get; set; }


        public DateTime TurTime
        {
            get { return _turTime; }
            set { _turTime = value; }
        }

        public List<Arter> AlleArters { get; set; }
    }
}