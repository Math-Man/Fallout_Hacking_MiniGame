using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace Fallout_Hacking_MiniGame
{
    class Program
    {
        static void Main(string[] args)
        {


            //Enable1 dictionary https://github.com/dolph/dictionary/blob/master/enable1.txt
            //slowPrint(AppDomain.CurrentDomain.BaseDirectory + "enable1.txt");

            slowPrint("Input 'help' for more information", 5);
            play(12, 6);

        }

        static int terminalsHacked = 0;
        static int score = 0;
        static double scoremult = 1;

        //Choose words
        static ArrayList getWords()
        {
            slowPrint("Choose Terminal difficulty: Novice, Advanced, Expert, Master", 10);
            bool passWord = false;

            string diff = "";
            while (!passWord)
            {
                diff = (Console.ReadLine()).ToLower();
                if (diff.Equals("novice") || diff.Equals("advanced") || diff.Equals("expert") || diff.Equals("master")) { passWord = true; }
                else if (diff.Equals("help")) { slowPrint("This is a silly recration of the hacking minigame from fallout series.\nGame generates a random set of words with a secret word you are trying to guess\nIf you pick a wrong word game will show how close you are to secret word", 5); }
                else { slowPrint("Invalid Selection!", 5); }
            }


            StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "enable1.txt");

            ArrayList words = new ArrayList();
            switch (diff)
            {
                case "novice":
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine().TrimEnd();
                            if (line.Length == 4)
                            {
                                scoremult = 1;
                                words.Add(line);
                            }
                        }
                        break;
                    }
                case "advanced":
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine().TrimEnd();
                            if (line.Length == 5)
                            {
                                scoremult = 1.5;
                                words.Add(line);
                            }
                        }
                        break;
                    }
                case "expert":
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine().TrimEnd();
                            if (line.Length == 6)
                            {
                                scoremult = 2;
                                words.Add(line);
                            }
                        }
                        break;
                    }
                case "master":
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine().TrimEnd();
                            if (line.Length == 7)
                            {
                                scoremult = 2.5;
                                words.Add(line);
                            }
                        }
                        break;
                    }
                default:
                    {
                        slowPrint("Invalid, defaulting to novice", 15);
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine().TrimEnd();
                            if (line.Length == 4)
                            {

                                words.Add(line);
                            }
                        }
                        break;
                    }
            }
            return words;
        }

        static ArrayList printWords(ArrayList words, string secret, int amount) //Returns printed words
        {
            ArrayList writtenWords = new ArrayList();
            Random rnd = new Random();
            int secretWordPos = rnd.Next(1, amount);

            for (int i = 0; i < amount; i++)
            {

                string fakeRegisterID = ("0x" + rnd.Next(0, 8192).ToString("X4"));
                string fakeAffix = (char)(rnd.Next(35, 47)) + "";
                string fakePrefix = (char)(rnd.Next(35, 47)) + "";
                string nextWord = (words[rnd.Next(0, words.Count)] + "").ToUpper();

                if (i == 0) { slowPrint(fakeRegisterID + " :  " + fakePrefix + nextWord + fakeAffix, 10); writtenWords.Add(nextWord.ToLower()); }
                else if (i == secretWordPos)
                {
                    slowPrint(fakeRegisterID + " :  " + fakePrefix + secret.ToUpper() + fakeAffix, 10);
                    writtenWords.Add(secret.ToLower());
                }
                else
                {
                    foreach (string s in writtenWords)
                    {

                        if (!s.Equals(nextWord))
                        {
                            slowPrint(fakeRegisterID + " :  " + fakePrefix + nextWord + fakeAffix, 10);
                            break;
                        }
                    }
                    writtenWords.Add(nextWord.ToLower());
                }
            }

            return writtenWords;
        }

        static bool guess(string secret, ArrayList enabledWords)
        {
            string input = "";
            bool passWord = false;
            while (!passWord)
            {
                Console.Write("@>");
                input = Console.ReadLine().ToLower();
                if (!enabledWords.Contains(input) || secret.Length != input.Length) { slowPrint("> Invalid input", 5); passWord = false; }
                else { passWord = true; }
            }

            char[] secretA = secret.ToCharArray();
            char[] wordA = input.ToCharArray();
            int score = 0;
            for (int l = 0; l < wordA.Length; l++)
            {
                if (secretA[l].Equals(wordA[l]))
                {
                    score++;
                }

            }


            slowPrint(score + "/" + secret.Length + " Correct", 5);
            if (score == secret.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        static void play(int amount, int tries)
        {


            ArrayList words = getWords();
            Random rnd = new Random();
            string secretWord = (string)words[rnd.Next(0, words.Count)];
            ArrayList wordsPrinted = printWords(words, secretWord, amount);
            int triesA = tries;

            while (true)
            {

                if (guess(secretWord, wordsPrinted))
                {
                    terminalsHacked++;
                    score += (int)(10 * scoremult);
                    slowPrint("Correct!\n--------------\nYou hacked: " + terminalsHacked + "\nYour Score: " + score + "\nKeep Going?(Y/N)", 15);

                    while (true)
                    {
                        string c = Console.ReadLine().ToLower();
                        if (c.Equals("y"))
                        {
                            tries = triesA;
                            play(amount, tries);
                        }
                        else if (c.Equals("n")) { slowPrint("Shutting down......", 20); System.Environment.Exit(0); }
                    }

                }
                else
                {
                    tries--;
                    slowPrint("Tries Left " + tries, 15);
                    if (tries == 0)
                    {
                        slowPrint("Failed!\nPlay again?(Y/N)", 15);

                        while (true)
                        {
                            string c = Console.ReadLine().ToLower();
                            if (c.Equals("y"))
                            {
                                score = 0;
                                tries = triesA;
                                play(amount, tries);
                            }
                            else if (c.Equals("n")) { slowPrint("Shutting down......",20); System.Environment.Exit(0); }
                        }
                    }
                }
            }
        }

        static void slowPrint(string line, int speed)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.DarkGreen;

            for (int i = -1; i < line.Length; i++)
            {
                Task.Delay(speed).Wait();

                Task.Factory.StartNew(() =>
                {
                    if (i < line.Length)
                    {
                        Console.Write(line[i]);
                    }

                });

            }
            Console.Write("\n");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }



    }
}
