using BV3N92_HFT_2021221.Models;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BV3N92_HFT_2021221.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(8000);

            RestService rest = new RestService("http://localhost:41126");

            Menu(rest);

            #region QuickTest Menu options

            //rest.Post<Parliament>(new Parliament()
            //{
            //    ParliamentName = "Parliament of Tests",
            //    RulingParty = "Party of Tests"
            //}, "parliament");

            //rest.Post<Party>(new Party()
            //{
            //    ParliamentID = 2,
            //    PartyName = "Test Party",
            //    Ideology = Ideologies.Conservative.ToString()
            //}, "party");

            //rest.Post<PartyMember>(new PartyMember()
            //{
            //    LastName = "TestName",
            //    Age = 50,
            //    PartyID = 5
            //}, "partymember");

            //rest.Delete(4, "parliament"); ?????
            //rest.Delete(3, "party");
            //rest.Delete(3, "partymember");

            //rest.Put<Parliament>(new Parliament()
            //{
            //    ParliamentID = 1,
            //    ParliamentName = "Bundestag",
            //    RulingParty = "SAP"
            //}, "parliament");

            //rest.Put<Party>(new Party()
            //{
            //    PartyID = 4,
            //    ParliamentID = 3,
            //    PartyName = "Hungarian Conservative Party",
            //    Ideology = Ideologies.Conservative.ToString()
            //}, "party");

            //rest.Put<PartyMember>(new PartyMember()
            //{
            //    MemberID = 6,
            //    LastName = "von Schacht",
            //    Age = 24,
            //    PartyID = 7
            //}, "partymember");

            var representativesperparliament = rest.Get<KeyValuePair<string, int>>("stat/representativesperparliament");
            var shortnamedmembersperparty = rest.Get<KeyValuePair<string, int>>("stat/shortnamedmembersperparty");
            var oldestmembersagesperparty = rest.Get<KeyValuePair<string, int>>("stat/oldestmembersageperparty");
            var juniormembersperparty = rest.Get<KeyValuePair<string, int>>("stat/juniormembersperparty");
            var avgageofmembersperparty = rest.Get<KeyValuePair<string, double>>("stat/avgageofmembersperparty");

            var parliaments = rest.Get<Parliament>("parliament");
            var parties = rest.Get<Party>("party");
            var members = rest.Get<PartyMember>("partymember");

            ;

            #endregion
        }

        private static void Menu(RestService rest)
        {
            #region Menu UI

            Console.WriteLine("Welcome to the DbContext of inner politics!");
            Console.WriteLine("You have the opportunity to manage inner politics of many nations.");
            Console.WriteLine("That means you have a context consisting of parliaments, parties and party members.");
            Console.WriteLine("Each parliament has multiple parties, of which one of them is the ruling party.");
            Console.WriteLine("Each party has an ideology and multiple junior and senior party members.");
            Console.WriteLine("The following actions are availabe:");
            Console.WriteLine("______________________________________________________\n");
            Console.WriteLine("1. Create entities");
            Console.WriteLine("2. Read entities");
            Console.WriteLine("3. Update entities");
            Console.WriteLine("4. Delete entities");
            Console.WriteLine("5. Get number of representatives in each parliament");
            Console.WriteLine("6. Get number of short named members (less than 6 characters) per party");
            Console.WriteLine("7. Get oldest member's age per party");
            Console.WriteLine("8. Get number of junior members (under the age of 30) per party");
            Console.WriteLine("9. Get average age of members per party");
            Console.WriteLine("______________________________________________________\n");
            Console.WriteLine("You can navigate the menu with the corresponding number keys.");
            Console.WriteLine("\nESC: Exit");

            #endregion

            ConsoleKey key = Console.ReadKey().Key;

            do
            {
                if (key == ConsoleKey.D1 || key == ConsoleKey.NumPad1)
                {
                    Console.Clear();
                    Console.WriteLine("1. Create Parliament");
                    Console.WriteLine("2. Create Party");
                    Console.WriteLine("3. Create Party Member");

                    ConsoleKey subkey = Console.ReadKey().Key;

                    if (subkey == ConsoleKey.D1 || subkey == ConsoleKey.NumPad1)
                    {
                        Console.Clear();
                        Console.WriteLine("Parliament must have a unique name!");
                        Console.WriteLine("Please enter data (hit enter after each property)");
                        Console.Write($"Parliament name: ");
                        string name = Console.ReadLine();
                        Console.Write($"Ruling party: ");
                        string rp = Console.ReadLine();

                        rest.Post<Parliament>(new Parliament()
                        {
                            ParliamentName = name,
                            RulingParty = rp
                        }, "parliament");
                        Console.WriteLine("Parliament successfully added to database!");

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }
                    else if (subkey == ConsoleKey.D2 || subkey == ConsoleKey.NumPad2)
                    {
                        Console.Clear();
                        Console.WriteLine("Party must have a unique name and be part of an existing parliament!");
                        Console.WriteLine("Please enter data (hit enter after each property)");
                        Console.Write($"Parliament ID: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write($"Party name: ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Ideology pool: Socialist, Conservative, Nationalist (watch out for capital letters!)");
                        Console.Write($"Party ideology: ");
                        string ideology = Console.ReadLine();

                        rest.Post<Party>(new Party()
                        {
                            ParliamentID = id,
                            PartyName = name,
                            Ideology = ideology
                        }, "party");
                        Console.WriteLine("Party successfully added to database!");

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }
                    else if (subkey == ConsoleKey.D3 || subkey == ConsoleKey.NumPad3)
                    {
                        Console.Clear();
                        Console.WriteLine("Member must have an age value between 18 and 70 years and be part of an existing party!");
                        Console.WriteLine("Please enter data (hit enter after each property)");
                        Console.Write($"Last name: ");
                        string name = Console.ReadLine();
                        Console.Write($"Age: ");
                        int age = int.Parse(Console.ReadLine());
                        Console.Write($"Party ID: ");
                        int id = int.Parse(Console.ReadLine());

                        rest.Post<PartyMember>(new PartyMember()
                        {
                            LastName = name,
                            Age = age,
                            PartyID = id
                        }, "partymember");
                        Console.WriteLine("Party member successfully added to database!");

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }

                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(rest);
                }
                else if (key == ConsoleKey.D2 || key == ConsoleKey.NumPad2)
                {
                    Console.Clear();
                    Console.WriteLine("1. Get Parliament entities");
                    Console.WriteLine("2. Get Party entities");
                    Console.WriteLine("3. Get Party Member entities");
                    Console.WriteLine("4. Get Parliament entity by ID");
                    Console.WriteLine("5. Get Party entity by ID");
                    Console.WriteLine("6. Get Party Member entity by ID");

                    ConsoleKey subkey = Console.ReadKey().Key;

                    if (subkey == ConsoleKey.D1 || subkey == ConsoleKey.NumPad1)
                    {
                        Console.Clear();
                        foreach (var item in rest.Get<Parliament>("parliament"))
                        {
                            Console.WriteLine($"ID: {item.ParliamentID}\nName: {item.ParliamentName}\nRuling Party: {item.RulingParty}\n");
                        }
                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }
                    else if (subkey == ConsoleKey.D2 || subkey == ConsoleKey.NumPad2)
                    {
                        Console.Clear();
                        foreach (var item in rest.Get<Party>("party"))
                        {
                            Console.WriteLine($"Party ID: {item.PartyID}\nParliament ID: {item.ParliamentID}\nName: {item.PartyName}\nIdeology: {item.Ideology}\n");
                        }
                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }
                    else if (subkey == ConsoleKey.D3 || subkey == ConsoleKey.NumPad3)
                    {
                        Console.Clear();
                        foreach (var item in rest.Get<PartyMember>("partymember"))
                        {
                            Console.WriteLine($"ID: {item.MemberID}\nName: {item.LastName}\nAge: {item.Age}\nParty ID: {item.PartyID}\n");
                        }
                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }
                    else if (subkey == ConsoleKey.D4 || subkey == ConsoleKey.NumPad4)
                    {
                        Console.Clear();
                        Console.Write("Parliament ID: ");
                        int id = int.Parse(Console.ReadLine());

                        var item = rest.GetSingle<Parliament>($"parliament/{id}");
                        Console.WriteLine($"\nParliament name: {item.ParliamentName}\nRuling party: {item.RulingParty}\n");

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }
                    else if (subkey == ConsoleKey.D5 || subkey == ConsoleKey.NumPad5)
                    {
                        Console.Clear();
                        Console.Write("Party ID: ");
                        int id = int.Parse(Console.ReadLine());

                        var item = rest.GetSingle<Party>($"party/{id}");
                        Console.WriteLine($"\nParliament ID: {item.ParliamentID}\nParty name: {item.PartyName}\nIdeology: {item.Ideology}\n");

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }
                    else if (subkey == ConsoleKey.D6 || subkey == ConsoleKey.NumPad6)
                    {
                        Console.Clear();
                        Console.Write("Member ID: ");
                        int id = int.Parse(Console.ReadLine());

                        var item = rest.GetSingle<PartyMember>($"partymember/{id}");
                        Console.WriteLine($"\nName: {item.LastName}\nAge: {item.Age}\nParty ID: {item.PartyID}\n");

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }

                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(rest);
                }
                else if (key == ConsoleKey.D3 || key == ConsoleKey.NumPad3)
                {
                    Console.Clear();
                    Console.WriteLine("1. Update Parliament entity");
                    Console.WriteLine("2. Update Party entity");
                    Console.WriteLine("3. Update Party Member entity");

                    ConsoleKey subkey = Console.ReadKey().Key;

                    if (subkey == ConsoleKey.D1 || subkey == ConsoleKey.NumPad1)
                    {
                        Console.Clear();
                        Console.WriteLine("Only existing parliaments can be modified, they must have a unique name!");
                        Console.Write($"Parliament ID: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write($"Parliament name: ");
                        string name = Console.ReadLine();
                        Console.Write($"Ruling party: ");
                        string rp = Console.ReadLine();

                        rest.Put<Parliament>(new Parliament()
                        {
                            ParliamentID = id,
                            ParliamentName = name,
                            RulingParty = rp
                        }, "parliament");
                        Console.WriteLine("Parliament successfully modified!");

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }
                    else if (subkey == ConsoleKey.D2 || subkey == ConsoleKey.NumPad2)
                    {
                        Console.Clear();
                        Console.WriteLine("Only existing parties can be modified, they must have a unique name and be part of an existing parliament!");
                        Console.WriteLine("Please enter data (hit enter after each property)");
                        Console.Write($"Party ID: ");
                        int pid = int.Parse(Console.ReadLine());
                        Console.Write($"Parliament ID: ");
                        int id = int.Parse(Console.ReadLine());
                        Console.Write($"Party name: ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Ideology pool: Socialist, Conservative, Nationalist (watch out for capital letters!)");
                        Console.Write($"Party ideology: ");
                        string ideology = Console.ReadLine();

                        rest.Put<Party>(new Party()
                        {
                            PartyID = pid,
                            ParliamentID = id,
                            PartyName = name,
                            Ideology = ideology
                        }, "party");
                        Console.WriteLine("Party successfully modified!");

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }
                    else if (subkey == ConsoleKey.D3 || subkey == ConsoleKey.NumPad3)
                    {
                        Console.Clear();
                        Console.WriteLine("Only existing members can be modified, they must have an age value between 18 and 70 years and be part of an existing party!");
                        Console.WriteLine("Please enter data (hit enter after each property)");
                        Console.Write($"Member ID: ");
                        int mid = int.Parse(Console.ReadLine());
                        Console.Write($"Last name: ");
                        string name = Console.ReadLine();
                        Console.Write($"Age: ");
                        int age = int.Parse(Console.ReadLine());
                        Console.Write($"Party ID: ");
                        int id = int.Parse(Console.ReadLine());

                        rest.Put<PartyMember>(new PartyMember()
                        {
                            MemberID = mid,
                            LastName = name,
                            Age = age,
                            PartyID = id
                        }, "partymember");
                        Console.WriteLine("Party member successfully modified!");

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }

                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(rest);
                }
                else if (key == ConsoleKey.D4 || key == ConsoleKey.NumPad4)
                {
                    Console.Clear();
                    Console.WriteLine("1. Delete Parliament entity");
                    Console.WriteLine("2. Delete Party entity");
                    Console.WriteLine("3. Delete Party Member entity");

                    ConsoleKey subkey = Console.ReadKey().Key;

                    if (subkey == ConsoleKey.D1 || subkey == ConsoleKey.NumPad1)
                    {
                        Console.Clear();
                        Console.WriteLine("Only an already existing parliament can be deleted!");
                        Console.Write($"Parliament ID: ");
                        int id = int.Parse(Console.ReadLine());

                        rest.Delete(id, "parliament");
                        Console.WriteLine("Parliament successfully deleted!");

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }
                    else if (subkey == ConsoleKey.D2 || subkey == ConsoleKey.NumPad2)
                    {
                        Console.Clear();
                        Console.WriteLine("Only an already existing party can be deleted!");
                        Console.Write($"Party ID: ");
                        int id = int.Parse(Console.ReadLine());

                        rest.Delete(id, "party");
                        Console.WriteLine("Party successfully deleted!");

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }
                    else if (subkey == ConsoleKey.D3 || subkey == ConsoleKey.NumPad3)
                    {
                        Console.Clear();
                        Console.WriteLine("Only an already existing member can be deleted!");
                        Console.Write($"Member ID: ");
                        int id = int.Parse(Console.ReadLine());

                        rest.Delete(id, "partymember");
                        Console.WriteLine("Member successfully deleted!");

                        Console.WriteLine("\nPress any key to return to main menu...");
                        Console.ReadKey();
                        Console.Clear();
                        Menu(rest);
                    }

                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(rest);
                }
                else if (key == ConsoleKey.D5 || key == ConsoleKey.NumPad5)
                {
                    Console.Clear();
                    foreach (var item in rest.Get<KeyValuePair<string, int>>("stat/representativesperparliament"))
                    {
                        Console.WriteLine($"Parliament: {item.Key}\nNumber of representatives: {item.Value}\n");
                    }
                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(rest);
                }
                else if (key == ConsoleKey.D6 || key == ConsoleKey.NumPad6)
                {
                    Console.Clear();
                    foreach (var item in rest.Get<KeyValuePair<string, int>>("stat/shortnamedmembersperparty"))
                    {
                        Console.WriteLine($"Party: {item.Key}\nNumber of short named members: {item.Value}\n");
                    }
                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(rest);
                }
                else if (key == ConsoleKey.D7 || key == ConsoleKey.NumPad7)
                {
                    Console.Clear();
                    foreach (var item in rest.Get<KeyValuePair<string, int>>("stat/oldestmembersageperparty"))
                    {
                        Console.WriteLine($"Party: {item.Key}\nOldest member's age: {item.Value}\n");
                    }
                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(rest);
                }
                else if (key == ConsoleKey.D8 || key == ConsoleKey.NumPad8)
                {
                    Console.Clear();
                    foreach (var item in rest.Get<KeyValuePair<string, int>>("stat/juniormembersperparty"))
                    {
                        Console.WriteLine($"Party: {item.Key}\nNumber of junior members: {item.Value}\n");
                    }
                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(rest);
                }
                else if (key == ConsoleKey.D9 || key == ConsoleKey.NumPad9)
                {
                    Console.Clear();
                    foreach (var item in rest.Get<KeyValuePair<string, double>>("stat/avgageofmembersperparty"))
                    {
                        Console.WriteLine($"Party: {item.Key}\nAverage age of members: {item.Value}\n");
                    }
                    Console.WriteLine("\nPress any key to return to main menu...");
                    Console.ReadKey();
                    Console.Clear();
                    Menu(rest);
                }
                else if (!key.Equals(ConsoleKey.Escape))
                {
                    Console.Clear();
                    Menu(rest);
                }

            } while (!key.Equals(ConsoleKey.Escape));

            Environment.Exit(0);
        }
    }
}
