using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace GameForum
{
    public class Game
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int postCount { get; set; }


        public Game()
        {

        }

        public static Game Select(int id)
        {
            string sql = $"SELECT * FROM game WHERE id = {id}";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();
            mda.Dispose();
            if (dt.Rows.Count == 0)
                return null;
            var row = dt.Rows[0];

            Game game = new Game
            {
                id = Convert.ToInt32(row["id"]),
                title = Convert.ToString(row["title"]),
                description = Convert.ToString(row["description"]),
                postCount = Convert.ToInt32(row["postcount"])
            };

            return game;
        }
        public static bool Create(Game game)
        {
            string sql = $"INSERT INTO `game` (`title`, `description`, `postcount`) VALUES ('{game.title}', '{game.description}', '{game.postCount}')";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return true;
        }
        public static List<Game> selectAll()
        {
            string sql = $"SELECT * FROM `game`";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            Debug.WriteLine(conn);
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();
            mda.Dispose();

            List<Game> list = new List<Game>();
            foreach (DataRow row in dt.Rows)
            {
                Game thread = new Game
                {
                    id = Convert.ToInt32(row["id"]),
                    title = Convert.ToString(row["title"]),
                    description = Convert.ToString(row["description"]),
                    postCount = Convert.ToInt32(row["postcount"])
                };

                list.Add(thread);
            }
            return list;
        }
        public static bool CheckExists(int id)
        {
            string sql = $"SELECT COUNT(id) FROM `game` WHERE `id` = {id}";

            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();
            mda.Dispose();

            var row = dt.Rows[0];
            int count = Convert.ToInt32(row[0]);

            if (count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool Update(int id, Game game)
        {
            string sql = $"UPDATE `game` SET `title` = '{game.title}', `description` = '{game.description}', `postcount` = '{game.postCount}' WHERE `game`.`id` = {id}";

            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return true;
        }
        public static bool Delete(int id)
        {
            string sqlquery = $"DELETE FROM `game` WHERE `game`.`id` = {id}";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return true;
        }
    }
}
