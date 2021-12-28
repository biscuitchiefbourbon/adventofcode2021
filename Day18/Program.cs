using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day18
{
    class Program
    {
        static void Main(string[] args) {
            String[] textInput = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt");
            bool partOne = false;

            if (partOne) {
                String number1Str = textInput[0];
                for (int i = 1; i < textInput.Length; i++) {
                    number1Str = $"[{number1Str},{textInput[i]}]";

                    String newStr = Reduce(number1Str);

                    while (newStr != number1Str) {
                        number1Str = newStr;
                        newStr = Reduce(number1Str);
                    }
                }

                long mag = Magnitude(number1Str);
                Console.WriteLine($"Addition result = {number1Str} Magnitude = {mag}");
            } else {
                long largestMag = 0;

                for(int i = 0; i < textInput.Length; i++) {
                    for(int j = 0; j < textInput.Length; j++) {
                        if (i != j) {
                            String iPlusJ = $"[{textInput[i]},{textInput[j]}]";
                            String newStr = Reduce(iPlusJ);

                            while (newStr != iPlusJ) {
                                iPlusJ = newStr;
                                newStr = Reduce(iPlusJ);
                            }

                            long iPlusJMag = Magnitude(iPlusJ);
                            if (iPlusJMag > largestMag) largestMag = iPlusJMag;


                            String jPlusI = $"[{textInput[j]},{textInput[i]}]";
                            newStr = Reduce(jPlusI);

                            while (newStr != jPlusI) {
                                jPlusI = newStr;
                                newStr = Reduce(jPlusI);
                            }

                            long jPlusIMag = Magnitude(jPlusI);
                            if (jPlusIMag > largestMag) largestMag = jPlusIMag;
                        }
                    }

                }

                Console.WriteLine($"Largest Mag={largestMag}");
            }
            Console.ReadLine();



        }

        static long Magnitude(String number) {

            List<RegularNumber> numbers = new List<RegularNumber>();
            String numStr = "";
            StringBuilder formatSB = new StringBuilder();


            int currentDepth = 0;
            foreach (char c in number) {
                if (c == '[') {
                    formatSB.Append(c);
                    currentDepth++;
                } else if (c == ',' || c == ']') {

                    if (numStr.Length > 0) {
                        long newNum = long.Parse(numStr);
                        formatSB.Append($"<{numbers.Count}>");
                        numbers.Add(new RegularNumber(newNum, currentDepth));
                        numStr = "";
                    }
                    if (c == ']') {
                        currentDepth--;

                    }
                    formatSB.Append(c);
                } else {
                    numStr += c;
                }

            }

            String formatStr = formatSB.ToString();

            do {
                for (int i = 1; i < numbers.Count; i++) {
                    if (numbers[i].Depth == numbers[i - 1].Depth) {
                        long mag = (3 * numbers[i - 1].Number) + (2 * numbers[i].Number);
                        RegularNumber rn = new RegularNumber(mag, numbers[i].Depth - 1);
                        numbers.RemoveAt(i - 1);
                        numbers.RemoveAt(i - 1);
                        numbers.Insert(i - 1, rn);
                        break;
                    }
                }
            } while (numbers.Count > 2);

            return numbers[0].Number * 3 + numbers[1].Number * 2;
        }

        static string Reduce(String number) {
            List<RegularNumber> numbers = new List<RegularNumber>();
            String numStr = "";
            StringBuilder formatSB = new StringBuilder();


            int currentDepth = 0;
            foreach (char c in number) {
                if (c == '[') {
                    formatSB.Append(c);
                    currentDepth++;
                } else if (c == ',' || c == ']') {

                    if (numStr.Length > 0) {
                        int newNum = int.Parse(numStr);
                        formatSB.Append($"<{numbers.Count}>");
                        numbers.Add(new RegularNumber(newNum, currentDepth));
                        numStr = "";
                    }
                    if (c == ']') {
                        currentDepth--;

                    }
                    formatSB.Append(c);
                } else {
                    numStr += c;
                }

            }

            String formatStr = formatSB.ToString();

            if (numbers.Where(n => n.Depth >= 5).Count() > 0) {
                // Explosion!
                int firstExplosionIndex = -1;
                for (int i = 0; i < numbers.Count; i++) {
                    if (numbers[i].Depth >= 5) {
                        firstExplosionIndex = i;
                        if (i > 0) {
                            // There is a number to the left
                            numbers[i - 1].Number += numbers[i].Number;
                        }
                        if ((i + 2) < numbers.Count) {
                            // There is a number to the right
                            numbers[i + 2].Number += numbers[i + 1].Number;
                        }
                        break;
                    }
                }
              
                formatStr = formatStr.Replace($"[<{firstExplosionIndex}>,<{firstExplosionIndex + 1}>]", "0");



            } else if (numbers.Where(n => n.Number >= 10).Count() > 0) {
                int firstSplitIndex = -1;
                for (int i = 0; i < numbers.Count; i++) {
                    if (numbers[i].Number >= 10) {
                        firstSplitIndex = i;
                        break;
                    }
                }
                int half = (int)((double)numbers[firstSplitIndex].Number / 2.0);
                int remainder = (int)((double)numbers[firstSplitIndex].Number % 2.0);
     
            }

            for (int i = 0; i < numbers.Count; i++) {
                formatStr = formatStr.Replace($"<{i}>", $"{numbers[i].Number}");
            }

      
            return formatStr;
        }

     

        class RegularNumber
        {
            public long Number;
            public int Depth;
            public int PairIndex;

            public RegularNumber(long number, int depth) {
                Number = number;
                Depth = depth;
                PairIndex = -1;
            }

            public override string ToString() {
                return $"{Number} ({Depth})";
            }
        }


    }
}
