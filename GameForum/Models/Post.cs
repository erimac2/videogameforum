using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GameForum
{
    public class Post
    {
        public int id { get; private set; }
        public string title { get; private set; }
        public int likeCount { get; private set; }
        public int commentCount { get; private set; }
        public int fk_user { get; private set; }
        public int fk_game { get; private set; }
        public DateTime timeofcreation { get; private set; }

        public Post()
        {

        }

        public static Post Select(int id)
        {
            string sql = $"SELECT * FROM post WHERE id = {id}";
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

            Post post = new Post
            {
                id = Convert.ToInt32(row["id"]),
                title = Convert.ToString(row["title"]),
                likeCount = Convert.ToInt32(row["likecount"]),
                commentCount = Convert.ToInt32(row["commentcount"]),
                fk_user = Convert.ToInt32(row["fk_user"]),
                timeofcreation = Convert.ToDateTime(row["timeofcreation"])
            };

            return post;
        }
        public static List<Post> SelectAll()
        {
            string sql = $"SELECT * FROM post";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();
            mda.Dispose();


            List<Post> list = new List<Post>();
            foreach (DataRow row in dt.Rows)
            {
                Post post = new Post
                {
                    id = Convert.ToInt32(row["id"]),
                    title = Convert.ToString(row["title"]),
                    likeCount = Convert.ToInt32(row["likecount"]),
                    commentCount = Convert.ToInt32(row["commentcount"]),
                    fk_user = Convert.ToInt32(row["fk_user"]),
                    timeofcreation = Convert.ToDateTime(row["timeofcreation"])
                };
                list.Add(post);
            }

            return list;
        }
        public static List<Post> SelectAllInGame(int id)
        {
            string sql = $"SELECT * FROM post WHERE fk_game = {id}";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();
            mda.Dispose();


            List<Post> list = new List<Post>();
            foreach (DataRow row in dt.Rows)
            {
                Post post = new Post
                {
                    id = Convert.ToInt32(row["id"]),
                    title = Convert.ToString(row["title"]),
                    likeCount = Convert.ToInt32(row["likecount"]),
                    commentCount = Convert.ToInt32(row["commentcount"]),
                    fk_user = Convert.ToInt32(row["fk_user"]),
                    timeofcreation = Convert.ToDateTime(row["timeofcreation"])
                };
                list.Add(post);
            }

            return list;
        }
        public static bool Delete(int id)
        {
            string sqlquery = $"DELETE FROM `post` WHERE `post`.`id` = {id}";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return true;
        }
        public static bool CheckExists(int id)
        {
            string sql = $"SELECT COUNT(id) FROM `post` WHERE `id` = {id}";

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
        public static bool Create(Post post)
        {
            string sql = $"INSERT INTO `post` (`title`, `likecount`, `commentcount`, `fk_user`, `fk_game`, `timeofcreation`) VALUES ('{post.title}', '{post.likeCount}', '{post.commentCount}', '{post.fk_user}', '{post.fk_game}', '{post.timeofcreation}')";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return true;
        }
        public static bool Update(int id, Post post)
        {
            string sql = $"UPDATE `post` SET `title` = '{post.title}', `likecount` = '{post.likeCount}', `commentcount` = '{post.commentCount}', `fk_user` = '{post.fk_user}'," +
                $" `fk_game` = '{post.fk_game}', `timeofcreation` = '{post.timeofcreation}' WHERE `post`.`id` = {id}";

            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return true;
        }
    }
}
