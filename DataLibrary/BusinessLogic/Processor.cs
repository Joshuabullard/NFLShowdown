using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.BusinessLogic
{
    public static class Processor
    {

        public static int PopulateGames(string gameKey, int week, string homeTeam, string awayTeam, bool hasStarted,
                                        DateTime gameTime, string channel, decimal pointSpread, decimal overUnder, string venue)
        {
            NFLGamePicks data = new NFLGamePicks
            {
                GameKey = gameKey,
                Week = week,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                HasStarted = hasStarted,
                GameTime = gameTime,
                Channel = channel,
                PointSpread = pointSpread,
                OverUnder = overUnder,
                Venue = venue
            };

            string sql = @"IF EXISTS (SELECT * FROM dbo.gamePicks WHERE GameKey = @GameKey)
                             BEGIN
                                UPDATE dbo.gamePicks SET GameKey = @GameKey, Week = @Week, HomeTeam = @HomeTeam, 
                                                         AwayTeam = @AwayTeam, HasStarted = @HasStarted, GameTime = @GameTime,
                                                         Channel = @Channel, PointSpread = @PointSpread, OverUnder = @OverUnder,
                                                         Venue = @Venue
                                                     WHERE GameKey = @GameKey;
                             END
                           ELSE
                             BEGIN
                                INSERT INTO dbo.gamePicks (GameKey, Week, HomeTeam, AwayTeam, HasStarted, GameTime, Channel, PointSpread,
                                                           OverUnder, Venue)
                                VALUES(@GameKey, @Week, @HomeTeam, @AwayTeam, @HasStarted, @GameTime, @Channel, @PointSpread, @OverUnder, @Venue);
                            END";

            return SqlDataAccess.SaveData(sql, data);
        }



        public static List<NFLLoadPickem> LoadPickem(string gKey, string user)
        {
            string sql = @"SELECT games.*, picks.* 
                           FROM dbo.gamePicks as games, dbo.usrPicks as picks
                           WHERE games.GameKey ='" + gKey + "' AND picks.UserID ='" + user + "';";


            return SqlDataAccess.LoadData<NFLLoadPickem>(sql);
        }



        public static List<NFLUsrPicks> CheckPickem(int currWeek, string user)
        {
            string sql = @"SELECT * FROM dbo.usrPicks
                           WHERE Week ='" + currWeek.ToString() + "' AND UserID ='" + user + "';";

            return SqlDataAccess.LoadData<NFLUsrPicks>(sql);
        }



        public static int AddUsrOptions(string user, int week, string gameKey, string gamePick)
        {
            NFLUsrPicks data = new NFLUsrPicks
            {
                UserID = user,
                Week = week,
                GameKey = gameKey,
                GamePick = gamePick
            };

            string sql = @"INSERT INTO dbo.usrPicks (UserID, Week, GameKey, GamePick)
                           VALUES (@UserID, @Week, @GameKey, @GamePick);";


            return SqlDataAccess.SaveData(sql, data);
        }


        public static int PopulateUsrPicks(string userID, int week, string gameKey, string gamePick)
        {
            NFLUsrPicks data = new NFLUsrPicks
            {
                UserID = userID,
                Week = week,
                GameKey = gameKey,
                GamePick = gamePick
            };

            string sql = @"IF EXISTS (SELECT * FROM dbo.usrPicks WHERE GameKey = @GameKey AND UserID = @UserID)
                             BEGIN
                                UPDATE dbo.usrPicks SET UserID = @UserID, Week = @Week, GameKey = @GameKey, GamePick = @GamePick 
                                                    WHERE GameKey = @GameKey AND UserID = @UserID;
                             END
                           ELSE
                             BEGIN
                                INSERT INTO dbo.usrPicks (UserID, Week, GameKey, GamePick)
                                VALUES(@UserID, @Week, @GameKey, @GamePick);
                            END";

            return SqlDataAccess.SaveData(sql, data);
        }



        public static int UpdateWinners(string gameKey, string winner)
        {
            NFLWinners data = new NFLWinners
            {
                Winner = winner,
                GameKey = gameKey
            };

            string sql = @"IF EXISTS (SELECT * FROM dbo.gamePicks WHERE GameKey = @GameKey)
                             BEGIN
                                UPDATE dbo.gamePicks SET Winner = @Winner WHERE GameKey = @GameKey;
                             END";

            return SqlDataAccess.SaveData(sql, data);
        }


        public static int CreateLeaderboard(string userID, int week, int wins, int losses)
        {
            NFLLeaderboard data = new NFLLeaderboard
            {
                UserID = userID,
                Week = week,
                Wins = wins,
                Losses = losses
            };

            string sql = @"IF EXISTS (SELECT * FROM dbo.leaderboard WHERE UserID = @UserID AND Week = @Week)
                             BEGIN
                                UPDATE dbo.leaderboard SET UserID = @UserID, Week = @Week, Wins = @Wins, Losses = @Losses 
                                WHERE UserID = @UserID AND Week = @Week;
                             END
                           ELSE
                             BEGIN
                                INSERT INTO dbo.leaderboard (UserID, Week, Wins, Losses)
                                VALUES (@UserID, @Week, @Wins, @Losses);
                             END";

            return SqlDataAccess.SaveData(sql, data);
        }



        public static List<UsrSelections> UsrWins(string userID, int week)
        {
            string sql = @"SELECT U.UserID, U.Week, U.GameKey, U.GamePick, G.Winner
                           FROM dbo.usrPicks as U, dbo.gamePicks as G
                           WHERE U.GameKey = G.GameKey
                           AND U.GamePick = G.Winner
                           AND U.UserID = '" + userID + "' AND U.Week = '" + week + "';";


            return SqlDataAccess.LoadData<UsrSelections>(sql);
        }


        public static List<UsrSelections> UsrLosses(string userID, int week)
        {
            string sql = @"SELECT U.UserID, U.Week, U.GameKey, U.GamePick, G.Winner
                           FROM dbo.usrPicks as U, dbo.gamePicks as G
                           WHERE U.GameKey = G.GameKey
                           AND U.GamePick <> '" + null + "' AND U.GamePick <> 'TIE' AND U.GamePick <> G.Winner AND U.UserID = '" + userID + "' AND U.Week = '" + week + "';";



            return SqlDataAccess.LoadData<UsrSelections>(sql);
        }



        public static List<NFLUsrPicks> LoadPicks()
        {
            string sql = @"SELECT DISTINCT UserID, Week FROM dbo.usrPicks;";

            return SqlDataAccess.LoadData<NFLUsrPicks>(sql);
        }



        public static List<NFLLeaderboard> LoadLeaderboard()
        {
            string sql = @"SELECT UserID, SUM(Wins) as Wins, SUM(Losses) as Losses FROM dbo.Leaderboard GROUP BY UserID";

            return SqlDataAccess.LoadData<NFLLeaderboard>(sql);

        }


        public static List<NFLLeaderboard> LoadHistory(string id)
        {
            string sql = @"SELECT * FROM dbo.Leaderboard WHERE UserID ='" + id + "';";

            return SqlDataAccess.LoadData<NFLLeaderboard>(sql);

        }

        public static List<NFLHistory> LoadPickHistory(int week, string id)
        {
            string sql = @"SELECT G.HomeTeam, G.AwayTeam, G.Winner, U.GamePick
                           FROM dbo.usrPicks as U, dbo.gamePicks as G
                           WHERE U.GameKey = G.GameKey
                           AND U.UserID ='" + id + "' AND U.Week = '" + week + "';";

            return SqlDataAccess.LoadData<NFLHistory>(sql);

        }

    }
}