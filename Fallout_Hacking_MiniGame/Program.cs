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
            Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory + "enable1.txt");
         
            play(12, 6);

        }

        static int terminalsHacked = 0;
        static int score = 0;
        static double scoremult = 1;

        //Choose words
        static ArrayList getWords()
        {
            Console.WriteLine(@"Choose difficulty: Novice, Advanced, Expert, Master");
            string diff = (Console.ReadLine()).ToLower();

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
                        Console.WriteLine("Invalid, defaulting to novice");
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
            int secretWordPos = rnd.Next(1,amount);

            for(int i = 0; i < amount; i++)
            {
                string nextWord = (words[rnd.Next(0, words.Count)] + "").ToUpper();

                if (i == 0) { Console.WriteLine(nextWord); writtenWords.Add(nextWord.ToLower()); }
                else if (i == secretWordPos)
                {
                    Console.WriteLine(secret.ToUpper());
                    writtenWords.Add(secret.ToLower());
                }
                else
                {
                    foreach (string s in writtenWords)
                    {

                        if (s.Equals(nextWord))
                        {

                        }
                        else
                        {
                            Console.WriteLine(nextWord);
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
            Console.Write("> ");
            string input = Console.ReadLine().ToLower();

            if (!enabledWords.Contains(input) || secret.Length != input.Length) {Console.WriteLine("> Invalid input\n");  return false; }

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

            
            Console.WriteLine(score + "/" + secret.Length);
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
                    score += (int)(10* scoremult);
                    Console.WriteLine("Correct!\nYou hacked: " + terminalsHacked + "\nYour Score: "+ score + "\n Keep Going?(Y)");

                    if (Console.ReadLine().ToLower().Equals("y"))
                    {
                        tries = triesA;
                        play(amount, tries);
                    }
                    else { System.Environment.Exit(0); }
                }
                else
                {
                    tries--;
                    Console.WriteLine("Tries Left "  + tries);
                    if (tries == 0)
                    {
                        Console.WriteLine("Failed!\n Play again?(Y)");

                        if (Console.ReadLine().ToLower().Equals("y"))
                        {
                            tries = triesA;
                            play(amount, tries);
                        }
                        else { System.Environment.Exit(0); }
                    }
                }

                
            }



            
        }



    }
}
