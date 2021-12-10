using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day10
{
    class Program
    {
        static void Main(string[] args) {
            String[] textInput = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt");            
            Dictionary<char, char> pairs = new Dictionary<char, char>() { { '}', '{' }, { ']', '[' }, { '>', '<' }, { ')', '(' } };
            
            Stack<char> openers = new Stack<char>();
            int partOneScore = 0;
            List<long> partTwoScores = new List<long>();

            foreach(String s in textInput) {
                bool corrupt = false;
                openers.Clear();

                foreach(char c in s) {
                    if(c == '[' || c == '{' || c == '(' || c == '<') {                     
                        openers.Push(c);
                    } else {
                        if (pairs[c] != openers.Pop()) {
                            if (c == ')') partOneScore += 3;
                            else if (c == ']') partOneScore += 57;
                            else if (c == '}') partOneScore += 1197;
                            else partOneScore += 25137;
                            corrupt = true;
                            break;
                        }                                               
                    }
                }

                if (!corrupt) {
                    long partTwoScore = 0;
                    while (openers.Count > 0) {
                        partTwoScore *= 5;
                        switch (openers.Pop()) {
                            case '(':partTwoScore += 1;break;
                            case '[':partTwoScore += 2;break;
                            case '{':partTwoScore += 3;break;
                            default:partTwoScore += 4;break;
                        }
                    }
                    partTwoScores.Add(partTwoScore);
                }
            }

            partTwoScores.Sort();

            Console.WriteLine($"Part one answer = {partOneScore} Part two Answer = {partTwoScores[partTwoScores.Count/2]}");
            Console.ReadLine();
        }
    }
}
