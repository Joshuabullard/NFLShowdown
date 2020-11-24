using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class NFLGamePicks
    {
        //dbo.gamePicks Database column headers
        public int Id { get; set; }
        public string GameKey { get; set; }
        public int Week { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public bool HasStarted { get; set; }
        public string Winner { get; set; }
        public DateTime GameTime { get; set; }
        public string Channel { get; set; }
        public decimal PointSpread { get; set; }
        public decimal OverUnder { get; set; }
        public string Venue { get; set; }

    }

    public class NFLLeaderboard
    {
        public int Id { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public int Week { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public double Ratio { get; set; }
    }


    public class NFLUsrPicks 
    {
        //dbo.usrPicks Database column headers
        public int Id { get; set; }
        public int Week { get; set; }
        public string UserID { get; set; }
        public string GameKey { get; set; }
        public string GamePick { get; set; }
    }


    public class NFLLoadPickem
    {
        public int Id { get; set; }
        public string GameKey { get; set; }
        public int Week { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public bool HasStarted { get; set; }
        public string Winner { get; set; }
        public string GamePick { get; set; }
        public string UserID { get; set; }
        public DateTime GameTime { get; set; }
        public string Channel { get; set; }
        public decimal PointSpread { get; set; }
        public decimal OverUnder { get; set; }
        public string Venue { get; set; }
    }


    public class NFLWinners
    {
        public string GameKey { get; set; }
        public string Winner { get; set; }
    }


    public class UsrSelections
    {
        public string UserID { get; set; }
        public string GameKey { get; set; }
        public int Week { get; set; }
        public string Winner { get; set; }
        public string GamePick { get; set; }
    }


    public class NFLHistory
    {
        public string UserID { get; set; }
        public string GameKey { get; set; }
        public string Winner { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string GamePick { get; set; }
    }

}
