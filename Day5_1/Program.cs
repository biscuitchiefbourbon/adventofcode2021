using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day5_1
{
    class Program
    {
        static void Main(string[] args) {

            int[,] ventPositions;

            List<int> x1 = new List<int>();
            List<int> x2 = new List<int>();
            List<int> y1 = new List<int>();
            List<int> y2 = new List<int>();
            bool partTwo = true;

            foreach (String s in File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt")) {
                String[] parts = s.Split(new char[] { ' ' });
                String[] fromCoords = parts[0].Split(new char[] { ',' });
                x1.Add(int.Parse(fromCoords[0]));
                y1.Add(int.Parse(fromCoords[1]));
                fromCoords = parts[2].Split(new char[] { ',' });
                x2.Add(int.Parse(fromCoords[0]));
                y2.Add(int.Parse(fromCoords[1]));
            }
            ventPositions = new int[x1.Concat(x2).Max() + 1, y1.Concat(y2).Max() + 1];

            for (int i = 0; i < x1.Count; i++) {
                if (x1[i] == x2[i]) {
                    for (int j = (y1[i]> y2[i] ? y2[i] : y1[i]); j <= (y1[i] > y2[i] ? y1[i] : y2[i]); j++) {
                        ventPositions[x1[i], j] ++;
                    }
                } else if (y1[i] == y2[i]) {
                    for (int j = (x1[i] > x2[i] ? x2[i] : x1[i]); j <= (x1[i] > x2[i] ? x1[i] : x2[i]); j++) {
                        ventPositions[j, y1[i]]++;
                    }
                } else {               
                    if (partTwo) {
                        int yOffset = 0;
                        if (x1[i] > x2[i]) {
                            for (int x = x1[i]; x >= x2[i]; x--) {                                
                                ventPositions[x, y1[i] > y2[i] ? (y1[i] - yOffset++):(y1[i] + yOffset++)]++;                                
                            }
                        } else {
                            for (int x = x1[i]; x <= x2[i]; x++) {
                                ventPositions[x, y1[i] > y2[i] ? (y1[i] - yOffset++) : (y1[i] + yOffset++)]++;
                            }
                        }
                    }
                }
            }

            int count = 0;
            for(int i = 0; i <= ventPositions.GetUpperBound(0); i++) {
                for(int j = 0; j <= ventPositions.GetUpperBound(1); j++) {
                    if (ventPositions[i, j] >= 2) count++;
                }
            }

            Console.WriteLine($"Answer is {count}");
            Console.ReadLine();

        }


        
    }
}
