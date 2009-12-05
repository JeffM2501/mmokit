using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using System.Data;

using ServerConfigurator;
using MySql.Data.MySqlClient;

namespace UserDatabase
{
    public class Users
    {
        protected ServerConfig config;
        protected MySqlConnection database;

        public Users ( string configFile )
        {
            FileInfo file = new FileInfo(configFile);
            config = new ServerConfig(configFile);
            if (!file.Exists)
                ConfigDefaults();

            string connStr = String.Format("server={0};user id={1}; password={2}; database=mysql; pooling=false",
                config.GetItem("UserDatabaseHost"), config.GetItem("UserDatabaseUser"), config.GetItem("UserDatabasePassword"));

            database = new MySqlConnection(connStr);
            try
            {
                database.Open();
            }
            catch (System.Exception ex)
            {
                database.Close();
                database = null;
            }

            if (database != null && database.State != ConnectionState.Open)
            {
                database.Close();
                database = null;
            }
            if (database != null)
                database.ChangeDatabase(config.GetItem("UserDatabaseDatabase"));
        }

        protected void checkDatabase()
        {
            if (database.State != ConnectionState.Open)
            {
                database.Open();
                database.ChangeDatabase(config.GetItem("UserDatabaseDatabase"));
            }
        }

        public bool Valid ()
        {
            return database != null;
        }

        public Int64 AuthUser ( string username, string password, ref UInt64 UID )
        {
            checkDatabase();

            String query = String.Format("SELECT ID, Password, Access FROM users WHERE Username = @Username");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@Username",username));

            MySqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return 0;
            }

            reader.Read();
            UID = reader.GetUInt64(0);
            String passHash = reader.GetString(1);
            int access = reader.GetInt32(2);
            reader.Close();

            if (access < 0)
                return -2;

            MD5 md5 = MD5.Create();
            byte[] inputHash = md5.ComputeHash(new ASCIIEncoding().GetBytes(password));

            string inputHashString = "";
            foreach(byte b in inputHash)
            {
                inputHashString += b.ToString("x2");
            }

            if (passHash != inputHashString)
                return -3;

            // valid login, make a new token
            Random rand = new Random();
            Int64 token = Math.Abs(rand.Next());

            query = String.Format("UPDATE users SET Token=@Token, Heartbeat=@Now WHERE ID=@UID");
            command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@UID", UID));
            command.Parameters.Add(new MySqlParameter("@Token", token));
            command.Parameters.Add(new MySqlParameter("@Now", DateTime.Now));

            command.ExecuteNonQuery();

            return token;
        }

        public bool CheckToken(UInt64 UID, Int64 token)
        {
            if (token < 0)
                return false;

            String query = String.Format("SELECT Token FROM users WHERE ID = @UID");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@UID", UID));

            MySqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
                return false;

            reader.Read();
            Int64 theToken = reader.GetInt64(0);
            reader.Close();

            return token == theToken;
        }

        protected void ConfigDefaults ()
        {
            config.SetItem("UserDatabaseHost", "localhost");
            config.SetItem("UserDatabaseUser", "username");
            config.SetItem("UserDatabasePassword", "password");
            config.SetItem("UserDatabaseDatabase", "database");

            config.Save();
        }

        public int UserCount ()
        {
            String query = String.Format("SELECT ID FROM users");
            MySqlCommand command = new MySqlCommand(query, database);

            MySqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return 0;
            }

            int count = 0;
            while (reader.Read())
                count++;
            reader.Close();

            return count;
        }

        public void CheckDeadUsers ( double age )
        {
         //   String query = String.Format("SELECT ID FROM users WHERE Heartbeat < '@DeadDate'");

            String query = String.Format("SELECT ID FROM users WHERE Heartbeat < @DeadDate");
            MySqlCommand command = new MySqlCommand(query, database);
            DateTime deadDate = DateTime.Now.AddSeconds(age*-1.0);

            command.Parameters.Add(new MySqlParameter("@DeadDate", deadDate));

            MySqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return;
            }

            List<UInt64> ItemsToKill = new List<UInt64>();

            while (reader.Read())
                ItemsToKill.Add(reader.GetUInt64(0));
            reader.Close();

            foreach(UInt64 id in ItemsToKill)
            {
                query = String.Format("UPDATE users SET Token=NULL, Heartbeat=NULL WHERE ID=@UID");
                command = new MySqlCommand(query, database);
                command.Parameters.Add(new MySqlParameter("@UID", id));
                command.ExecuteNonQuery();
            }
        }
    }
}
