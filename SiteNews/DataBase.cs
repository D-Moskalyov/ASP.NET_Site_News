using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace SiteNews
{
    public class DataBase
    {
        static SqlConnection sql;
        static MD5 md5Hash = MD5.Create();

        public static MD5 Md5Hash
        {
            get { return DataBase.md5Hash; }
        }

        public static SqlConnection Sql
        {
            get { return sql; }
        }

        public static string GetConnectionString()
        {

            return ConfigurationManager.ConnectionStrings["NewsSiteConnection"].ConnectionString;
        }

        public static SqlConnection GetSqlConnection()
        {

            sql = new SqlConnection(GetConnectionString());
            try
            {
                sql.Open();
            }
            catch (System.Data.SqlClient.SqlException e)
            {

            }
            return sql;
        }

        public static void BeforeClosing()
        {
            if (sql != null && sql.State == ConnectionState.Open)
                sql.Close();
        }

        public static bool GetStateConnection()
        {
            if (sql != null && sql.State == ConnectionState.Open)
                return true;
            return false;
        }


        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}