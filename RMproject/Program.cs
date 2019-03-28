using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using RestSharp;
using System.Xml.Linq;
using System.IO;

namespace ConsoleApp1
{
    class Program
    {
        

        public class Result
        {
            public string name { get; set; }
            public string status { get; set; }
            public string species { get; set; }
            public string gender { get; set; }

            public Result()
            {

            }
            public Result(string name, string status, string species, string gender)
            {
                this.name = name;
                this.status = status;
                this.species = species;
                this.gender = gender;
            }
        }

        public class RootObject
        {
            public List<Result> results { get; set; }
        }
                     

        static void Main(string[] args)
        {

            int index;


            var envPath = @"%userprofile%\Downloads\Data.txt";
            var filePath = Environment.ExpandEnvironmentVariables(envPath);

            

            string content;
            Boolean exists;
            exists = File.Exists(filePath);
            
            if (exists)
            {

                 content = File.ReadAllText(filePath);
            }
            else
            {
                
                var client = new RestClient("https://rickandmortyapi.com/api/character/?page=1");
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                content = response.Content;
                
            }


            RootObject res = SimpleJson.DeserializeObject<RootObject>(content);



            int userInput = 0;
            do
            {
                userInput = DisplayMenu();

                switch (userInput)
                {
                    case 1:
                        AddCharacter(res);                     
                        break;
                        //method to add character, prompts questions one at a time
                    case 2:
                        int counter = 0;
                        Console.Clear();
                        Console.WriteLine("Here are all the characters\n");
                        foreach (var item in res.results)
                        {
                            Console.WriteLine("The characters name is {0} with an ID of {1}", item.name, counter);
                            counter++;
                        }
                        Console.WriteLine("\nPress Enter to continue back to the menu");
                        Console.ReadLine();

                        //method to display list of all characters with there ID #

                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Please enter a character ID between 0 and {0}",res.results.Count-1);
                        index = Convert.ToInt32(Console.ReadLine());


                        Console.WriteLine(res.results[index].name);
                        Console.WriteLine(res.results[index].status);
                        Console.WriteLine(res.results[index].species);
                        Console.WriteLine(res.results[index].gender);

                        Console.WriteLine("\nPress Enter to continue back to the menu");
                        Console.ReadLine();

                        //call method to display chosen character by ID and any information on them.
                        break;
                    case 4:
                        Console.WriteLine("Which ID would you like to delete?");
                        Console.WriteLine("Please enter a character ID between 0 and {0}", res.results.Count - 1);

                        //Fix this to test parse for a number
                        bool ifsuccess;
                        
                        ifsuccess = Int32.TryParse(Console.ReadLine(), out index);
                 //       index = Convert.ToInt32(Console.ReadLine());
                        res.results.RemoveAt(index);

                        //call method for to enter ID# and delete it from list
                        break;


                }
            }
            while (userInput != 5);

            //saves any changes inputed through method and updates file.

            string data = SimpleJson.SerializeObject(res);
            File.WriteAllText(filePath, data);

        }

        static public int DisplayMenu()
        { 
            //basic menu, directs to correct methods
            Console.Clear();
            Console.WriteLine("Rick and Morty Character list!!");
            Console.WriteLine();
            Console.WriteLine("1. Add a new Character");
            Console.WriteLine("2. List the all the Characters");
            Console.WriteLine("3. Search for a Character by Id");
            Console.WriteLine("4. Delete a Character");
            Console.WriteLine("5. Exit\n");
            Console.Write("Input: ");
            var result = Console.ReadLine();
            return Convert.ToInt32(result);
        }

        static public void AddCharacter(RootObject rooty)
        {
            // promps questions, adds response into new item in array.
            Console.Write("What's the Characters Name? ");
            string name = Console.ReadLine();
            Console.Write("Is the character Alive or Dead? ");
            string status = Console.ReadLine();
            Console.Write("What Species is the Character? ");
            string species = Console.ReadLine();
            Console.Write("What gender is the Character? ");
            string gender = Console.ReadLine();

            Result newPerson = new Result(name, status, species, gender);
            rooty.results.Add(newPerson);


        }
    }
}
