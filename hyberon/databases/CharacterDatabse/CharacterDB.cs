using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Data;

using ServerConfigurator;
using MySql.Data.MySqlClient;
using Characters;

namespace CharacterDatabse
{
    public class CharacterDB
    {
        protected ServerConfig config;
        protected MySqlConnection database;

        public CharacterDB(string configFile)
        {
            FileInfo file = new FileInfo(configFile);
            config = new ServerConfig(configFile);
            if (!file.Exists)
                ConfigDefaults();

            string connStr = String.Format("server={0};user id={1}; password={2}; database=mysql; pooling=false",
            config.GetItem("CharacterDatabaseHost"), config.GetItem("CharacterDatabaseUser"), config.GetItem("CharacterDatabasePassword"));

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
                database.ChangeDatabase(config.GetItem("CharacterDatabase"));
        }

        public bool Valid()
        {
            return database != null;
        }

        protected void ConfigDefaults()
        {
            config.SetItem("CharacterDatabaseHost", "localhost");
            config.SetItem("CharacterDatabaseUser", "username");
            config.SetItem("CharacterDatabasePassword", "password");
            config.SetItem("CharacterDatabase", "database");

            config.Save();
        }

        protected void checkDatabase()
        {
            if (database.State != ConnectionState.Open)
            {
                database.Open();
                database.ChangeDatabase(config.GetItem("CharacterDatabase"));
            }
        }

        public int ChracterCount()
        {
            checkDatabase();
            String query = String.Format("SELECT CID FROM characters WHERE UID NOT NULL AND active is 1");
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

        public List<UInt64> GetCharacterList ( UInt64 UID )
        {
            checkDatabase();
            List<UInt64> characterList = new List<UInt64>();

            String query = String.Format("SELECT CID FROM characters WHERE UID is @UID AND active is 1");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@UID", UID));

            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
                characterList.Add(reader.GetUInt64(0));

            reader.Close();
            return characterList;
        }

        public Character GetCharacter(UInt64 CID, UInt64 UID)
        {
            checkDatabase();

            if (CharacterUser(CID) != UID)
                return new Character();

            String query = String.Format("SELECT UID, name, race, class, gender, experience, level FROM characters WHERE CID is @CID AND active is 1");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@CID", CID));
            
            MySqlDataReader reader = command.ExecuteReader();

            if (!reader.HasRows)
            {
                reader.Close();
                return new Character();
            }

            reader.Read();
            Character character = new Character();

            character.CharacterID = CID;
            character.PlayerID = reader.GetUInt64(0);
            character.Name = reader.GetString(1);
            character.Race = (CharacterRace)Enum.ToObject(typeof(CharacterRace),reader.GetInt32(2));
            character.Class = (CharacterClass)Enum.ToObject(typeof(CharacterClass), reader.GetInt32(3));
            character.Gender = (CharacterGender)Enum.ToObject(typeof(CharacterGender), reader.GetInt32(4));
            character.Experience = reader.GetInt32(5);
            character.Level = reader.GetInt32(6);

            return character;
        }

        public UInt64 AddCharacter ( ref Character character )
        {
            checkDatabase();

            String query = String.Format("INSERT INTO characters SET name=@name");
            MySqlCommand command = new MySqlCommand(query, database);

            string name = new Random().NextDouble().ToString();
            command.Parameters.Add(new MySqlParameter("@name", name));
            if (command.ExecuteNonQuery() == 0)
                return 0;

            query = String.Format("SELECT CID FROM characters WHERE name is @name");
            command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@name", name));

            MySqlDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
            {
                reader.Close();
                return 0;
            }

            character.CharacterID = reader.GetUInt64(0);

            if (!UpdateCharacter(character))
                return 0;

            return character.CharacterID;
        }

        public bool DeleteCharacter ( UInt64 CID, UInt64 UID )
        {
            checkDatabase();
            if (!CharacterExists(CID) || CharacterUser(CID) != UID)
                return false;

            String query = String.Format("UPDATE characters SET active=0 WHERE CID=@CID");
            MySqlCommand command = new MySqlCommand(query, database);

            command.Parameters.Add(new MySqlParameter("@CID", CID));
            return command.ExecuteNonQuery() != 0;
        }

        public bool CharacterExists ( UInt64 CID )
        {
            checkDatabase();
            
            String query = String.Format("SELECT CID FROM characters WHERE CID is @CID");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@CID", CID));
            
            MySqlDataReader reader = command.ExecuteReader();

            bool valid = reader.HasRows;
            reader.Close();
            return valid;
        }

        public UInt64 CharacterUser(UInt64 CID)
        {
            checkDatabase();

            String query = String.Format("SELECT UID FROM characters WHERE CID is @CID");
            MySqlCommand command = new MySqlCommand(query, database);
            command.Parameters.Add(new MySqlParameter("@CID", CID));

            MySqlDataReader reader = command.ExecuteReader();

            UInt64 uid = 0;
            if (reader.Read())
                uid = reader.GetUInt64(0);
            reader.Close();
            return uid;
        }

        public bool UpdateCharacter ( Character character )
        {
            checkDatabase();
            
            if (!CharacterExists(character.CharacterID))
                return false;
            String query = String.Format("UPDATE characters SET UID=@UID, name=@name, race=@race, class=@class, gender=@gender, experience=@experience, level=@level active=1 WHERE CID=@CID");
            MySqlCommand command = new MySqlCommand(query, database);
           
            command.Parameters.Add(new MySqlParameter("@CID", character.CharacterID));
            command.Parameters.Add(new MySqlParameter("@UID", character.PlayerID));
            command.Parameters.Add(new MySqlParameter("@name", character.Name));
            command.Parameters.Add(new MySqlParameter("@race", (int)character.Race));
            command.Parameters.Add(new MySqlParameter("@class", (int)character.Class));
            command.Parameters.Add(new MySqlParameter("@gender", (int)character.Gender));
            command.Parameters.Add(new MySqlParameter("@experience", character.Experience));
            command.Parameters.Add(new MySqlParameter("@level", character.Level));
            
            return command.ExecuteNonQuery() != 0;
        }
    }
}
