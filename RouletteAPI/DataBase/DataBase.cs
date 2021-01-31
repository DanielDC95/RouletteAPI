using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;

namespace RouletteAPI.DataBase
{
    public class DataBase
    {
        private static NpgsqlConnection dataBaseConnection = new NpgsqlConnection("Server = localhost; User Id = postgres; Password = 1234; DataBase = roulette_db");

        public static bool connect()
        {
            try
            {
                dataBaseConnection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool desconnect()
        {
            try
            {
                dataBaseConnection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
