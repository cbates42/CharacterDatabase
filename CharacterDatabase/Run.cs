//referenced my Produce app for logic on reading and writing to and from a database
// referenced how to use scope identity and executenonscalar here: https://stackoverflow.com/questions/42648/how-to-get-the-identity-of-an-inserted-row
// and here https://learn.microsoft.com/en-us/sql/t-sql/functions/scope-identity-transact-sql?view=sql-server-ver16&redirectedfrom=MSDN
// rereferenced joins and foreign keys from the class powerpoint

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;

namespace CharacterDatabase
{
    internal class Run
    {
        int newID;
        public Run()
        {//on construction reads the txt file
            Read();
        }

        public string fixString(string val)
        {
            //kept running into an error because of single 's in LeChuck's ship so just taking it out
            if (val != null)
            {
                return val.Replace("'", "");
            }
            else
            {
                return val;
            }
               
        }

        public void Read()
        {
            //build sqlconnection
            SqlConnectionStringBuilder mySqlConBldr = new SqlConnectionStringBuilder();
            mySqlConBldr["server"] = @"(localdb)\MSSQLLocalDB";
            mySqlConBldr["Trusted_Connection"] = true;
            mySqlConBldr["Integrated Security"] = "SSPI";
            mySqlConBldr["Initial Catalog"] = "PROG260FA23";
            string sqlConStr = mySqlConBldr.ToString();

            //get path
            var path = Directory.GetCurrentDirectory() + @"\Chars.csv";
            Console.WriteLine($"reading {path}");
            //streamreader stuff to read the file
            using (StreamReader sr = new StreamReader(path))
            {
                if (path != null)
                {
                    //skip first like
                    var line = sr.ReadLine();
                    line = sr.ReadLine();

                    while (line != null)

                    {
                        //split at the delimiter
                        var stats = line.Split(',');
                        bool.TryParse(stats[3], out bool originalChar);
                        bool.TryParse(stats[4], out bool isSwordFighter);
                        bool.TryParse(stats[5], out bool isMage);

                        Console.WriteLine("stats location: " + stats[2]);

                        Character currentChar =
                            new Character(stats[0], stats[1], stats[2], originalChar, isSwordFighter, isMage);
                        Console.WriteLine(stats[0], stats[1], stats[2], originalChar, isSwordFighter, isMage);

                        Console.WriteLine("character's location: " + currentChar.location);
                        //connect to table
                        using (SqlConnection conn = new SqlConnection(sqlConStr))
                        {
                            conn.Open();

                            //insert the stats first so the statsID int is available
                            string inlineSQL = $@"INSERT INTO [dbo].[CharacterStat] ([Type], [MapLocation], [Original_Character], [Sword_Fighter], [Magic_User]) 
                          VALUES ('{currentChar.type}', '{fixString(currentChar.location)}', '{currentChar.originalchar}', '{currentChar.swordfighter}', '{currentChar.magicuser}');
                          SELECT SCOPE_IDENTITY();";

                            using (var command = new SqlCommand(inlineSQL, conn))
                            {
                                //making absolutely sure its an int was getting errors for some reason before
                                var statsID = command.ExecuteScalar();
                                statsID = Convert.ToInt32(newID);

                                //insert the character into the string using the statsID that was generated before
                                string insertChara = $@"INSERT INTO [dbo].[Character] ([Character], [StatsID]) VALUES ('{currentChar.character}', '{newID}');";

                                using (var insertChar = new SqlCommand(insertChara, conn))
                                {
                                    insertChar.ExecuteNonQuery();
                                }
                            }
                            conn.Close();
                        }


                        //keep looping through 
                        line = sr.ReadLine();
                    }
                    allResults();
                }

                else 
                {
                    Console.WriteLine($"chars.csv not found! Check {path}.");
                }
               
            
            }
        }

        public void allResults()
        {
            SqlConnectionStringBuilder mySqlConBldr = new SqlConnectionStringBuilder();
            mySqlConBldr["server"] = @"(localdb)\MSSQLLocalDB";
            mySqlConBldr["Trusted_Connection"] = true;
            mySqlConBldr["Integrated Security"] = "SSPI";
            mySqlConBldr["Initial Catalog"] = "PROG260FA23";
            string sqlConStr = mySqlConBldr.ToString();

    

            using (SqlConnection conn = new SqlConnection(sqlConStr))
            {
                conn.Open();

                //join tables where the Character.StatsID and CharacterStat.StatsID match. character cant exist without a statsID, so there has to be a match for all of them
                string inlineSQL = @"Select * 
                              FROM dbo.Character 
                              INNER JOIN CharacterStat ON Character.StatsID = CharacterStat.StatsID;";

                using (var command = new SqlCommand(inlineSQL, conn))
                {
                    var reader = command.ExecuteReader();
                        using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"\Full Report.txt", true))
                    {
                        Console.WriteLine("Wrote to Full Report.txt");
                        while (reader.Read())
                        {
                            //write to text file for every column that was selected
                            for (int currentColumn = 0; currentColumn < reader.FieldCount; currentColumn++)
                            {
                                sw.Write(reader.GetValue(currentColumn));
                            }
                            sw.WriteLine();
                        }
                
                    }
                    reader.Close();
                    conn.Close();
                }
            }

            //run noMaps
            noMaps();
        }

        public void noMaps()
        {
            SqlConnectionStringBuilder mySqlConBldr = new SqlConnectionStringBuilder();
            mySqlConBldr["server"] = @"(localdb)\MSSQLLocalDB";
            mySqlConBldr["Trusted_Connection"] = true;
            mySqlConBldr["Integrated Security"] = "SSPI";
            mySqlConBldr["Initial Catalog"] = "PROG260FA23";
            string sqlConStr = mySqlConBldr.ToString();

            using (SqlConnection conn = new SqlConnection(sqlConStr))
            {
                conn.Open();

                string inlineSQL = "SELECT * FROM Character " +
                                   "INNER JOIN CharacterStat ON Character.StatsID = CharacterStat.StatsID " +
                                   "WHERE CharacterStat.MapLocation = '';";

                using (var command = new SqlCommand(inlineSQL, conn))
                {
                    var reader = command.ExecuteReader();

                    Console.WriteLine("Wrote to Lost.txt");
                    using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"\Lost.txt"))
                    {
                        while (reader.Read())
                        {
                            for (int currentColumn = 0; currentColumn < reader.FieldCount; currentColumn++)
                            {
                                var value = reader.GetValue(currentColumn);
                                sw.Write(value);
                            }
                            sw.WriteLine();
                        }
                    }
                    reader.Close();
                }
                conn.Close();
            }

            swordNonHuman();
        }

        public void swordNonHuman()
        {
            SqlConnectionStringBuilder mySqlConBldr = new SqlConnectionStringBuilder();
            mySqlConBldr["server"] = @"(localdb)\MSSQLLocalDB";
            mySqlConBldr["Trusted_Connection"] = true;
            mySqlConBldr["Integrated Security"] = "SSPI";
            mySqlConBldr["Initial Catalog"] = "PROG260FA23";
            string sqlConStr = mySqlConBldr.ToString();

            using (SqlConnection conn = new SqlConnection(sqlConStr))
            {
                conn.Open();

                //join and select all where the type isn't equal to human and where swordfighter is true
                string inlineSQL = @"SELECT * " +
                                   "FROM Character " + 
                                   "INNER JOIN CharacterStat ON Character.StatsID = CharacterStat.StatsID " +
                                   "WHERE CharacterStat.Type != 'Human' AND CharacterStat.Sword_Fighter = 1; ";

                using (var command = new SqlCommand(inlineSQL, conn))
                {
                    var reader = command.ExecuteReader();
                    Console.WriteLine("Wrote to SwordNonHuman.txt");
                    using (StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"\SwordNonHuman.txt"))
                    {
                        while (reader.Read())
                        {
                            for (int currentColumn = 0; currentColumn < reader.FieldCount; currentColumn++)
                            {
                                sw.Write(reader.GetValue(currentColumn));
                            }
                            sw.WriteLine();
                        }
                    }
                    reader.Close();
                }
                conn.Close();
            }
            }
        }
    }

