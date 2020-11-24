using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using FantasyData.Api.Client;
using FantasyData.Api.Client.Model.NFLv3;
using NFLShowdown.Models;

namespace NFLShowdown.Services
{
    public class NFLService
    {
        //Retrieve API Key from SportsData.IO
        private readonly string SportsDataioKey = WebConfigurationManager.AppSettings["SportsDataioKey"];


        //Returns a list of all games in the current NFL season - DataType = Score
        public List<Score> GetUpcomingSchedule()
        {
            NFLv3StatsClient client = new NFLv3StatsClient(SportsDataioKey);
            int? currentSeason = client.GetCurrentSeason();
            return client.GetScores(currentSeason.ToString());
        }

        
        //Returns current week, Current season, week start, and week end
        public List<Timeframe> GetSeasonDates()
        {
            NFLv3StatsClient client = new NFLv3StatsClient(SportsDataioKey);
            return client.GetTimeframes("upcoming");
        }

    }
}