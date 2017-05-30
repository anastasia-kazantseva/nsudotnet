using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Kazantseva.Nsudotnet.NumberGuesser
{
    class Program
    {
        private static String[] _swearings =
        {
            "You're adopted, {0}! How do you like this news, you bastard?",
            "{0}, seems like you haven't been fuked for a year",
            "I'd cry in my room all days long if I were as retarted as {0}"
        };

        private static String[] _history = new string[1000];
        private static String _nickname;
        private static int _iteration = 1;
        private static int _random;
        private static DateTime _startTime;

        static void Main(string[] args)
        {
            try
            {
                Random random = new Random();
                Console.WriteLine(
                    "Hey there! Let's play a funny game!\nI think of a random number from 0 to 100 and you try to guess it!\nIf you're too chicken for this game, press 'q' to stop.\nNow enter your name.");
                _nickname = Console.ReadLine();
                _random = random.Next(100);
                _startTime = DateTime.Now;

                Console.WriteLine("Let's get it started! Waiting for your first try!");

                String input = Console.ReadLine();
                int guess;

                while (!input.Equals("q"))
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        //Console.WriteLine("\nIteration: " + (_iteration + i));
                        //Console.ReadLine();
                        if (!int.TryParse(input, out guess))
                        {
                            Console.WriteLine("OMG! I only asked you for a number! Don't make it hard for both of us!");
                            _history[_iteration + i] = input;
                            continue;
                        }

                        switch (Math.Sign(guess.CompareTo(_random)))
                        {
                            case 0:
                                Console.WriteLine(
                                    "Finally! I thought I'd die before you guessed this damn number correctly!");
                                Console.WriteLine(
                                    String.Format(
                                        "You tried {0} times during {1} minutes\nAnd that's what you came up with: ",
                                        _iteration + i, (DateTime.Now - _startTime).TotalMinutes.ToString()));
                                for (int j = 0; j < _iteration + i; ++j)
                                {
                                    Console.WriteLine(_history[j]);
                                }
                                Console.ReadLine();
                                return;
                            case -1:
                                Console.WriteLine(
                                    "Come on, we're not messing with small numbers here! Try something bigger!\n");
                                _history[_iteration + i] = String.Format("{0} - <", guess);
                                break;
                            case 1:
                                Console.WriteLine("Who the hell do you think you are? Try something smaller!\n");
                                _history[_iteration + i] = String.Format("{0} - >", guess);
                                break;
                            default:
                                Console.WriteLine("WTF???");
                                break;
                        }

                        input = Console.ReadLine();
                    }

                    _iteration += 4;

                    Console.WriteLine(_swearings[random.Next(_swearings.Length)], _nickname);
                }

                Console.WriteLine("Sorry not sorry. Run away, you loser!");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
