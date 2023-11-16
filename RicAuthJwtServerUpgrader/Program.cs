using System;
using System.Configuration;
using System.Reflection;
using DbUp;
using Microsoft.Data.SqlClient;
using RicAuthJwtServerUpgrader.Properties;

namespace RicAuthJwtServerUpgrader
{
    class Program
    {
        private static string _connectionString;

        static int Main(string[] args)
        {
            var keySelection = string.Empty;
            int resultCode = 0;
            _connectionString = GetConnectionString();

            if (args.Length > 0)
                keySelection = args[0];

            if (args.Length > 1)
                keySelection = args[1];

            while (string.Compare(keySelection, "Exit", StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                if (!string.IsNullOrEmpty(keySelection))
                {
                    if (keySelection == DbActionConstant.Create)
                    {
                        Console.WriteLine(Resources.Program_Main_Creating_database);

                        resultCode = CreateDatabase();

                        keySelection = "Exit";
                    }
                    else if (keySelection == DbActionConstant.Update)
                    {
                        Console.WriteLine(Resources.Program_Main_Updating_database);

                        resultCode = UpdateDatabase();

                        keySelection = "Exit";
                    }
                }
                else
                {
                    DrawMenu();

                    var key = Console.ReadKey(true);
                    keySelection = GetVerbForKeySelection(key);
                }

            }

            return resultCode;
        }

        private static string GetVerbForKeySelection(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    return "Update";
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    return "Create";
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    return "TestData";
                case ConsoleKey.D4:
                case ConsoleKey.NumPad4:
                    return "Exit";
                default:
                    return string.Empty;
            }
        }

        private static void DrawMenu()
        {
            Console.Clear();

            Console.WriteLine(Resources.Program_DrawMenu__1__Update);
            Console.WriteLine(Resources.Program_DrawMenu__2__Create);
            Console.WriteLine(Resources.Program_DrawMenu__3__Exit_program);
        }

        private static int UpdateDatabase()
        {
            //run scripts
            return RunScript(DbActionConstant.Update);
        }

        private static int CreateDatabase()
        {
            //create new database base on the connection string
            EnsureDatabase.For.SqlDatabase(_connectionString);
            SqlConnection.ClearAllPools();
            //run scripts
            return RunScript(DbActionConstant.Create);
        }

        private static int RunScript(string folderName)
        {
            var upgrader =
                DeployChanges.To
                    .SqlDatabase(_connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), (string s) => s.Contains(folderName))
                    .WithExecutionTimeout(TimeSpan.FromMinutes(10))
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();

                Console.Read();
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.ResetColor();

            return 0;
        }

        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["RicAuthServerDb"].ConnectionString;
        }

    }
}
