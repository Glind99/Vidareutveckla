namespace Vidareutveckla
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    namespace VarmDrinkStation
    {
        // Definierar ett interface för varma drycker
        public interface IWarmDrink
        {
            void Consume(); // Metod för att konsumera drycken
            string GetName(); // Metod för att få dryckens namn

        }


        // Implementerar en specifik varm dryck, i detta fall vatten
        internal class Water : IWarmDrink
        {
            public void Consume()
            {
                Console.WriteLine("Warm water is served."); // Utskrift vid konsumtion av vatten
            }
            public string GetName()
            {
                return "Warm Water";
            }
        }
        internal class Coffee : IWarmDrink
        {
            public void Consume()
            {
                Console.WriteLine("Hot Coffee is served."); // Utskrift vid konsumtion av vatten
            }
            public string GetName()
            {
                return "Coffee";
            }
        }
        internal class Chocolate : IWarmDrink
        {
            public void Consume()
            {
                Console.WriteLine("Hot Chocolate is served."); // Utskrift vid konsumtion av vatten
            }
            public string GetName()
            {
                return "Hot Chocolate";
            }
        }

        // Definierar ett interface för fabriker som kan skapa varma drycker
        public interface IWarmDrinkFactory
        {
            IWarmDrink Prepare(int total); // Metod för att förbereda drycken med en specifik mängd
        }

        // Implementerar en specifik fabrik som förbereder varmt vatten
        internal class HotWaterFactory : IWarmDrinkFactory
        {
            public IWarmDrink Prepare(int total)
            {
                Console.WriteLine($"Pour {total} ml hot water in your cup"); // Utskrift av mängden vatten som hälls upp
                return new Water(); // Returnerar en ny instans av Water
            }
        }
        internal class CoffeeFactory : IWarmDrinkFactory
        {
            public IWarmDrink Prepare(int total)
            {
                Console.WriteLine($"Brew {total} ml coffee");
                return new Coffee();
            }
        }
        internal class ChocolateFactory : IWarmDrinkFactory
        {
            public IWarmDrink Prepare(int total)
            {
                Console.WriteLine($"Pour {total} ml Hot Chocolate");
                return new Chocolate();
            }
        }

        // Maskin som hanterar skapandet av varma drycker
        public class WarmDrinkMachine
        {
            private readonly List<Tuple<string, IWarmDrinkFactory>> namedFactories; // Lista över fabriker med deras namn

            public WarmDrinkMachine()
            {
                namedFactories = new List<Tuple<string, IWarmDrinkFactory>>(); // Initierar listan över fabriker

                // Registrerar fabriker explicit
                RegisterFactory<HotWaterFactory>("Hot Water"); // Registrerar fabriken för varmt vatten
                RegisterFactory<CoffeeFactory>("Coffee"); // Registrerar fabriken för kaffe
                RegisterFactory<ChocolateFactory>("Hot Chocolate"); // Registrerar fabriken för kaffe
            }


            // Metod för att registrera en fabrik
            private void RegisterFactory<T>(string drinkName) where T : IWarmDrinkFactory, new()
            {
                namedFactories.Add(Tuple.Create(drinkName, (IWarmDrinkFactory)Activator.CreateInstance(typeof(T)))); // Lägger till fabriken i listan
            }

            // Metod för att skapa en varm dryck
            public IWarmDrink MakeDrink()
            {
                Console.WriteLine("This is what we serve today:");
                for (var index = 0; index < namedFactories.Count; index++)
                {
                    var tuple = namedFactories[index];
                    Console.WriteLine($"{index}: {tuple.Item1}"); // Skriver ut tillgängliga drycker
                }
                Console.WriteLine("Select a number to continue:");
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out int i) && i >= 0 && i < namedFactories.Count) // Läser och validerar användarens val
                    {
                        Console.Write("How much: ");
                        if (int.TryParse(Console.ReadLine(), out int total) && total > 0) // Läser och validerar mängden
                        {
                            return namedFactories[i].Item2.Prepare(total); // Förbereder och returnerar drycken
                        }
                    }
                    Console.WriteLine("Something went wrong with your input, try again."); // Meddelande vid felaktig inmatning
                }
            }
        }

        class Program
        {
            static void Main(string[] args)
            {
                
                Console.WriteLine("Hey! Welcome to our virtual CoffeeShop");
                List<IWarmDrink> orderedDrinks = new List<IWarmDrink>(); // Lista för att lagra beställda drycker

                while (true)
                {
                    Console.WriteLine("1: Order a drink");
                    Console.WriteLine("2: Show ordered drinks");
                    Console.WriteLine("3: Exit");
                    Console.Write("Choose an option: ");
                    
                    string input = Console.ReadLine();

                    if (input == "3")
                    {
                        Console.WriteLine("Thank you for visiting! Goodbye!");
                        break;
                    }
                    else if (input == "1")
                    {
                        Console.Clear();
                        var machine = new WarmDrinkMachine(); // Skapar en instans av WarmDrinkMachine
                        IWarmDrink drink = machine.MakeDrink(); // Skapar en dryck
                        orderedDrinks.Add(drink); // Lägger till den skapade drycken i listan
                        drink.Consume(); // Konsumerar drycken
                    }
                    else if (input == "2")
                    {
                        if (orderedDrinks.Count == 0)
                        {
                            Console.Clear();
                            Console.WriteLine("No drinks ordered yet.");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Ordered drinks:");
                            foreach (var drink in orderedDrinks)
                            {
                                Console.WriteLine(drink.GetName()); // Visar namnet på varje beställd dryck
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please try again.");
                    }
                }
            }
        }
    }

}
