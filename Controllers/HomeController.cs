using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFLShowdown.Services;
using NFLShowdown.Models;
using FantasyData.Api.Client.Model.NFLv3;
using System.Net.Http.Headers;
using static DataLibrary.BusinessLogic.Processor;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace NFLShowdown.Controllers
{
    
    public class HomeController : Controller
    {
        private NFLService nflServ;
        public HomeController()
        {
            nflServ = new NFLService();
        }

        public ActionResult Index()
        {
            // Determine winner of game by gamekey (IF COMPLETED)
            var currSchedule = nflServ.GetUpcomingSchedule();
            
            foreach (var game in currSchedule)
            {
                string gameWinner = null;

                if (game.AwayTeam != "BYE" && game.IsOver)
                {
                    if (game.AwayScore > game.HomeScore)
                        gameWinner = game.AwayTeam;
                    else if (game.AwayScore < game.HomeScore)
                        gameWinner = game.HomeTeam;
                    else
                        gameWinner = "TIE";
                }

                UpdateWinners(game.GameKey, gameWinner);
            }


            //Create leaderboard (IF USER HAS MADE PICKS AND GAMES ARE COMPLETE)
            var usrPickList = LoadPicks();
            if (usrPickList.Any())
            {

                foreach (var weeklyPicks in usrPickList)
                {
                    var usrWin = UsrWins(weeklyPicks.UserID, weeklyPicks.Week);
                    var usrLoss = UsrLosses(weeklyPicks.UserID, weeklyPicks.Week);
                    int countWins = 0;
                    int countLoss = 0;

                    if (usrWin.Any())
                    {
                        countWins = usrWin.Count();
                    }

                    if (usrLoss.Any())
                    {
                        countLoss = usrLoss.Count();
                    }


                    CreateLeaderboard(weeklyPicks.UserID, weeklyPicks.Week, countWins, countLoss);
                }
            }

            return View();
        }


        [Authorize] 
        public ActionResult pickem()
        {
            var seasonDates = nflServ.GetSeasonDates();         //Current week Timeframes
            var currSchedule = nflServ.GetUpcomingSchedule();   //Week game schedule


            //Populate [dbo.gamePicks] database with scheduled games for current week:
            foreach (var game in currSchedule)
            {
                    decimal pS = 0;     //point spread
                    decimal oU = 0;     //over under

                    if (game.Week == seasonDates[0].Week && game.AwayTeam != "BYE")
                    {
                        if(game.PointSpread != null)
                            pS = (decimal)game.PointSpread;

                        if(game.OverUnder != null)
                            oU = (decimal)game.OverUnder;

                            PopulateGames(game.GameKey, game.Week, game.HomeTeam, game.AwayTeam, game.HasStarted, (DateTime)game.DateTime,
                                          game.Channel, pS, oU, game.StadiumDetails.Name);
                    }
            }


            //Send data from [dbo.usrPicks] database to view model: 
            var usrPicks = CheckPickem((int)seasonDates[0].Week, User.Identity.Name);
            List<UserPickem> picks = new List<UserPickem>();

            if (usrPicks.Any())
            {
                //user has already made picks for current Week -- Load picks:
                foreach (var row in usrPicks)
                {
                    picks.Add(new UserPickem
                    {
                        UserID = row.UserID,
                        Week = row.Week,
                        GameKey = row.GameKey,
                        GamePick = row.GamePick
                    });
                }
            }
            else
            {
                //user has not made picks for current week -- load all games:
                foreach (var game in currSchedule)
                {
                    if (game.Week == seasonDates[0].Week && game.AwayTeam != "BYE")
                    {
                        picks.Add(new UserPickem
                        {
                            UserID = User.Identity.Name,
                            Week = game.Week,
                            GameKey = game.GameKey,
                            GamePick = null
                        });

                        AddUsrOptions(User.Identity.Name, game.Week, game.GameKey, null);
                    }
                }
            }


            //create view model of weekly games and picks(if applicable):
            List<LoadPicks> games = new List<LoadPicks>();


            foreach (var game in picks)
            {
                var gameInfo = LoadPickem(game.GameKey, User.Identity.Name);


                if (game.GameKey != null)
                {
                    games.Add(new LoadPicks
                    {
                        GameKey = gameInfo[0].GameKey,
                        Week = gameInfo[0].Week,
                        HomeTeam = gameInfo[0].HomeTeam,
                        AwayTeam = gameInfo[0].AwayTeam,
                        HasStarted = gameInfo[0].HasStarted,
                        Winner = gameInfo[0].Winner,
                        UserID = gameInfo[0].UserID,
                        GamePick = game.GamePick,
                        GameTime = gameInfo[0].GameTime,
                        Channel = gameInfo[0].Channel,
                        PointSpread = gameInfo[0].PointSpread,
                        OverUnder = gameInfo[0].OverUnder,
                        Venue = gameInfo[0].Venue
                    });
                }
            }

            var model = new PickemGames();
            model._Games = games;
            model.Week = (int)seasonDates[0].Week;

            // Return view model to Pickem VIEW:
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult pickem(PickemGames model)
        {
            if (ModelState.IsValid)
            {
                //add user game picks to a list:
                List<string> usrGamePicks = new List<string>();
                usrGamePicks.Add(model.game1);
                usrGamePicks.Add(model.game2);
                usrGamePicks.Add(model.game3);
                usrGamePicks.Add(model.game4);
                usrGamePicks.Add(model.game5);
                usrGamePicks.Add(model.game6);
                usrGamePicks.Add(model.game7);
                usrGamePicks.Add(model.game8);
                usrGamePicks.Add(model.game9);
                usrGamePicks.Add(model.game10);
                usrGamePicks.Add(model.game11);
                usrGamePicks.Add(model.game12);
                usrGamePicks.Add(model.game13);
                usrGamePicks.Add(model.game14);
                usrGamePicks.Add(model.game15);
                usrGamePicks.Add(model.game16);



                //Enter user picks into [dbo.usrPicks] database:
                int index = 0;
                var currSchedule = nflServ.GetUpcomingSchedule(); 
                var seasonDates = nflServ.GetSeasonDates();

                foreach(var game in currSchedule)
                {
                    if (game.Week == seasonDates[0].Week && game.AwayTeam != "BYE")
                    {
                        PopulateUsrPicks(User.Identity.Name, game.Week, game.GameKey, usrGamePicks[index]);
                        index++;
                    }
                }


                return RedirectToAction("Index");
            }

            return View();
        }


        public ActionResult leaderboard(string sortOrder)
        {
            var model = LoadLeaderboard();

            ViewBag.UserSortParm = "UserId";
            ViewBag.WinsSortParm = "Wins";
            ViewBag.LossesSortParm = "Losses";
            ViewBag.RatioSortParm = "Ratio";


            for(int i=0; i < model.Count(); i++)
            {
                if ((model[i].Wins + model[i].Losses) == 0)
                    model[i].Ratio = 0;
                else
                    model[i].Ratio = model[i].Wins / ((double)model[i].Wins + (double)model[i].Losses);
            }

         
            var data = from s in model
                            select s;


            switch (sortOrder)
            {
                case "UserId":
                    data = data.OrderBy(s => s.UserID);
                    break;
                case "Wins":
                    data = data.OrderByDescending(s => s.Wins);
                    break;
                case "Losses":
                    data = data.OrderByDescending(s => s.Losses);
                    break;
                case "Ratio":
                    data = data.OrderByDescending(s => s.Ratio);
                    break;
                default:
                    data = data.OrderByDescending(s => s.Wins);
                    break;
            }

            return View(data);
        }


        [Authorize]
        public ActionResult PickHistory()
        {

            var usrPicks = LoadHistory(User.Identity.Name);

            return View(usrPicks);
        }


        [Authorize]
        public ActionResult ViewPicks(int week)
        {
            var weekPicks = LoadPickHistory(week, User.Identity.Name);

            return View(weekPicks);
        }
    }
}