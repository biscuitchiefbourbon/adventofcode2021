using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day8_1
{
    class Program
    {
        static void Main(string[] args) {
            List<Digit[]> allInput = new List<Digit[]>();
            List<Digit[]> allOutput = new List<Digit[]>();

            foreach (String s in File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt")) {
                int inputIndex = 0;                
                Digit[] inputData = new Digit[10];
                Digit[] outputData = new Digit[4];

                foreach (String part in s.Split(new char[] { ' ' })) {
                    if(part != "|") {
                        if (inputIndex < 10) inputData[inputIndex++] = new Digit(part);
                        else outputData[inputIndex++ - 10] = new Digit(part);
                    }
                }
                allInput.Add(inputData);
                allOutput.Add(outputData);
            }

            int partOneAnswer = 0;
            foreach(Digit[] digitList in allOutput) {
                foreach (Digit digit in digitList)
                    partOneAnswer += digit.IsOne || digit.IsFour || digit.IsSeven || digit.IsEight ? 1 : 0;
            }

            Console.WriteLine($"Part one answer is {partOneAnswer}");

            int loop = 0;
            int overallAnswer = 0;
            foreach (Digit[] digitList in allInput) {
                int[] counts = new int[7];

                foreach (Digit digit in digitList) 
                    for (int i = 0; i < digit.Segments.Length; i++) counts[i] += digit.Segments[i];                

                int[] transform = new int[7];

                // Segment b (or index 1) has 6 occurences
                transform[1] = Array.IndexOf(counts, 6);
                // Segment e (or index 4) has 4 occurences
                transform[4] = Array.IndexOf(counts, 4);
                // Segment f (or index 5) has 9 occurences
                transform[5] = Array.IndexOf(counts, 9);

                // Segment a (or index 0) AND Segment c (or index 2) have 8 occurences
                int a0orc2Index = Array.IndexOf(counts, 8);
                // Segment d (or index 3) AND segment g (or index 6) have 7 occurences
                int d3org6Index = Array.IndexOf(counts, 7);

                foreach (Digit digit in digitList) {
                    if (digit.IsOne) {                       
                        if (digit.Segments[a0orc2Index] == 1) {
                            // 1 has c but not a, so a0orc2Index = c2Index
                            transform[2] = a0orc2Index;
                            transform[0] = Array.LastIndexOf(counts, 8);
                        } else {
                            transform[0] = a0orc2Index;
                            transform[2] = Array.LastIndexOf(counts, 8);
                        }
                    } else if (digit.IsFour) {                        
                        if (digit.Segments[d3org6Index] == 1) {
                            // 4 has d but not g, so d3org6Index = d3Index
                            transform[3] = d3org6Index;
                            transform[6] = Array.LastIndexOf(counts, 7);
                        } else {
                            transform[6] = d3org6Index;
                            transform[3] = Array.LastIndexOf(counts, 7);
                        }
                    }
                }

                int answer = 0;
                int multipler = 1000;
                foreach (Digit digit in allOutput[loop]) {
                    answer += (digit.GetNumber(transform) * multipler);
                    multipler /= 10;
                }
                overallAnswer += answer;
                loop++;
            }

            Console.WriteLine($"Part two answer = {overallAnswer}");
            
            
            Console.ReadLine();
        }


        private class Digit {
            private int[] _segments;

            private static int[,] correct = {   {1,1,1,0,1,1,1},
                                                {0,0,1,0,0,1,0 },
                                                {1,0,1,1,1,0,1 },
                                                {1,0,1,1,0,1,1 },
                                                {0,1,1,1,0,1,0 },
                                                {1,1,0,1,0,1,1 },
                                                {1,1,0,1,1,1,1 },
                                                {1,0,1,0,0,1,0 },                                                
                                                {1,1,1,1,1,1,1 },
                                                {1,1,1,1,0,1,1 } };

            public bool IsOne { get { return _segments.Sum() == 2; } }
            public bool IsFour { get { return _segments.Sum() == 4; } }
            public bool IsSeven { get { return _segments.Sum() == 3; } }
            public bool IsEight { get { return _segments.Sum() == 7; } }

            public int[] Segments { get { return _segments; } }
                
            public Digit(String input) {
                //acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf

                _segments = new int[7];
                foreach(char c in input) {
                    _segments[((byte)c) - 97] = 1;
                }
            }

            public int GetNumber(int[] transform) {
                int[] corrected = new int[7];
                for(int i = 0; i < _segments.Length; i++) {
                    corrected[i] = _segments[transform[i]];
                }
                for(int i = 0; i <= correct.GetUpperBound(0); i++) {
                    bool allMatch = true;
                    for(int j = 0; j <= correct.GetUpperBound(1); j++) {
                        if(corrected[j] != correct[i,j]) {
                            allMatch = false;
                            break;
                        }
                    }
                    if (allMatch) return i;
                }
                return -1;
            }
        }

        
    }
}
