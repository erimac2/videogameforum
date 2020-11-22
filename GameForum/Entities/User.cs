using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Text.Json.Serialization;
using System.Diagnostics;

namespace GameForum.Entities
{
    public class User
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public UserRole role { get; set; }
        public string email { get; set; }
        public DateTime dateofbirth { get; set; }
        public string token { get; set; }

        public User()
        {
            token = "";
        }
        public static bool Create(User user)
        {
            string sql = $"INSERT INTO `user` (`username`, `password`, `email`, `dateofbirth`, `fk_role`) VALUES ('{user.username}', '{user.password}', '{user.email}', '{user.dateofbirth.ToString("yyyy-MM-dd HH:mm:ss.fff")}', '{(int)user.role}')";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return true;
        }
        public static bool Login(int id)
        {
            string sql = $"UPDATE `user` SET `online` = '{1}' WHERE `user`.`id` = {id}";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return true;
        }
        public static bool Logout(int id)
        {
            string sql = $"UPDATE `user` SET `online` = '{0}' WHERE `user`.`id` = {id}";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return true;
        }
        public static bool IsOnline(int id)
        {
            string sql = $"SELECT online FROM `user` WHERE `user`.`id` = {id}";

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
            int online = Convert.ToInt32(row[0]);
            Debug.WriteLine("This is status: " + online);

            if(online == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool Delete(int id)
        {
            string sqlquery = $"DELETE FROM `user` WHERE `user`.`id` = {id}";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sqlquery, mySqlConnection);
            mySqlConnection.Open();
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

            return true;
        }
        public static User Select(int id)
        {
            string sql = $"SELECT * FROM user WHERE id = {id}";
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

            User user = new User
            {
                id = Convert.ToInt32(row["id"]),
                username = Convert.ToString(row["username"]),
                password = Convert.ToString(row["password"]),
                email = Convert.ToString(row["email"]),
                dateofbirth = Convert.ToDateTime(row["dateofbirth"]),
                role = (UserRole)Convert.ToInt32(row["fk_role"])
            };

            return user;
        }
        public static List<User> selectAll()
        {
            string sql = $"SELECT * FROM user";
            string conn = ConfigurationManager.ConnectionStrings["MysqlConnection"].ConnectionString;
            MySqlConnection mySqlConnection = new MySqlConnection(conn);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, mySqlConnection);
            mySqlConnection.Open();
            MySqlDataAdapter mda = new MySqlDataAdapter(mySqlCommand);
            DataTable dt = new DataTable();
            mda.Fill(dt);
            mySqlConnection.Close();
            mda.Dispose();

            List<User> list = new List<User>();
            foreach (DataRow row in dt.Rows)
            {
                User user = new User
                {
                    id = Convert.ToInt32(row["id"]),
                    username = Convert.ToString(row["username"]),
                    password = Convert.ToString(row["password"]),
                    email = Convert.ToString(row["email"]),
                    dateofbirth = Convert.ToDateTime(row["dateofbirth"]),
                    role = (UserRole)Convert.ToInt32(row["fk_role"])
                };

                list.Add(user);
            }
            return list;
        }
        public static bool ChangeRole(int id, UserRole role)
        {
            string sql = $"UPDATE `user` SET  `fk_role` = '{role}' WHERE `user`.`id` = {id}";
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
            string sql = $"SELECT COUNT(id) FROM `user` WHERE `id` = {id}";

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
        public static bool Update(int id, User user)
        {
            string sql = $"UPDATE `user` SET `username` = '{user.username}', `password` = '{user.password}', `email` = '{user.email}'," +
                $" `dateofbirth` = '{user.dateofbirth.ToString("yyyy-MM-dd HH:mm:ss.fff")}', `fk_role` = '{(int)user.role}' WHERE `user`.`id` = {id}";

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
