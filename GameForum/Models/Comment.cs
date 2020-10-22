using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace GameForum
{
    public class Comment
    {
        public int id { get; set; }
        public string text { get; set; }
        public int likeCount { get; set; }
        public int fk_user { get; set; }
        public int fk_post { get; set; }
        public DateTime timeofcreation { get; set; }

        public Comment()
        {

        }

        public static Comment Select(int id)
        {
            string sql = $"SELECT * FROM comment WHERE id = {id}";
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

            Comment comment = new Comment
            {
                id = Convert.ToInt32(row["id"]),
                text = Convert.ToString(row["text"]),
                likeCount = Convert.ToInt32(row["likecount"]),
                fk_user = Convert.ToInt32(row["fk_user"]),
                fk_post = Convert.ToInt32(row["fk_post"]),
                timeofcreation = Convert.ToDateTime(row["timeofcreation"])
            };

            return comment;
        }
        public static List<Comment> SelectAll()
        {
            string sql = $"SELECT * FROM comment";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();
            mda.Dispose();


            List<Comment> list = new List<Comment>();
            foreach (DataRow row in dt.Rows)
            {
                Comment comment = new Comment
                {
                    id = Convert.ToInt32(row["id"]),
                    text = Convert.ToString(row["text"]),
                    likeCount = Convert.ToInt32(row["likecount"]),
                    fk_user = Convert.ToInt32(row["fk_user"]),
                    fk_post = Convert.ToInt32(row["fk_post"]),
                    timeofcreation = Convert.ToDateTime(row["timeofcreation"])
                };
                list.Add(comment);
            }

            return list;
        }
        public static bool Delete(int id)
        {
            string sqlquery = $"DELETE FROM `comment` WHERE `comment`.`id` = {id}";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return true;
        }
        public static bool Create(Comment comment)
        {
            string sql = $"INSERT INTO `comment` (`text`, `likecount`, `fk_user`, `fk_post`, `timeofcreation`) VALUES ('{comment.text}', '{comment.likeCount}', '{comment.fk_user}', '{comment.fk_post}', '{comment.timeofcreation.ToString("yyyy-MM-dd HH:mm:ss.fff")}')";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return true;
        }
        public static bool CheckExists(int id)
        {
            string sql = $"SELECT COUNT(id) FROM `comment` WHERE `id` = {id}";

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
        public static bool Update(int id, Comment comment)
        {
            string sql = $"UPDATE `comment` SET `text` = '{comment.text}', `likecount` = '{comment.likeCount}', `fk_user` = '{comment.fk_user}'," +
                $" `fk_post` = '{comment.fk_post}', `timeofcreation` = '{comment.timeofcreation.ToString("yyyy-MM-dd HH:mm:ss.fff")}' WHERE `comment`.`id` = {id}";

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
