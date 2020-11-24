using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using FantasyData.Api.Client.Model.NFLv3;

namespace NFLShowdown.Models
{
    public class UserPickem
    {
        public string UserID { get; set; }
        public int Week { get; set; }
        public string GameKey { get; set; }
        public string GamePick { get; set; }

    }

    public class PickemGames
    {
        public List<LoadPicks> _Games { get; set; }
        public int Week { get; set; }
        public string game1 { get; set; }
        public string game2 { get; set; }
        public string game3 { get; set; }
        public string game4 { get; set; }
        public string game5 { get; set; }
        public string game6 { get; set; }
        public string game7 { get; set; }
        public string game8 { get; set; }
        public string game9 { get; set; }
        public string game10 { get; set; }
        public string game11 { get; set; }
        public string game12 { get; set; }
        public string game13 { get; set; }
        public string game14 { get; set; }
        public string game15 { get; set; }
        public string game16 { get; set; }
    }

    public class LoadPicks
    {
        public string GameKey { get; set; }
        public int Week { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public bool HasStarted  { get; set; }
        public string Winner { get; set; }
        public string GamePick { get; set; }
        public string UserID { get; set; }
        public DateTime GameTime { get; set; }
        public string Channel { get; set; }
        public decimal PointSpread { get; set; }
        public decimal OverUnder { get; set; }
        public string Venue { get; set; }
    }
}