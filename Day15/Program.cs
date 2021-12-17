using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Day15
{
    class Program
    {

       



        static void Main(string[] args) {


            String[] textInput = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt");
            bool partTwo = true;

            int[,] risk = new int[textInput[0].Length, textInput.Length];
            int[,] minDist = new int[textInput[0].Length, textInput.Length];
            for (int y = 0; y < textInput.Length; y++) {
                for (int x = 0; x < textInput[y].Length; x++) {
                    risk[x, y] = int.Parse(textInput[y][x].ToString());
                    minDist[x, y] = int.MaxValue;
                }

            }

            if (partTwo) {
                int[,] expanded = new int[(risk.GetUpperBound(0) + 1) * 5, (risk.GetUpperBound(1) + 1) * 5];
                int[,] expandedDist = new int[(risk.GetUpperBound(0) + 1) * 5, (risk.GetUpperBound(1) + 1) * 5];

                for (int tileX = 0; tileX < 5; tileX++) {
                    for (int tileY = 0; tileY < 5; tileY++) {
                        for (int oldX = 0; oldX <= risk.GetUpperBound(0); oldX++) {
                            for (int oldY = 0; oldY <= risk.GetUpperBound(1); oldY++) {
                                if (tileX == 0 && tileY == 0) {
                                    // Copy original
                                    expanded[oldX, oldY] = risk[oldX, oldY];
                                    expandedDist[oldX, oldY] = 4000;
                                } else {
                                    if (tileX == 0) {
                                        // Copy and adjust from above
                                        int newX = tileX * (risk.GetUpperBound(0) + 1) + oldX;
                                        int newY = tileY * (risk.GetUpperBound(1) + 1) + oldY;
                                        int copyFromY = (tileY - 1) * (risk.GetUpperBound(1) + 1) + oldY;
                                        int newRisk = expanded[newX, copyFromY] == 9 ? 1 : expanded[newX, copyFromY] + 1;
                                        expanded[newX, newY] = newRisk;
                                        expandedDist[newX, newY] = 4000;
                                    } else {
                                        // Copy and adjust from left
                                        int newX = tileX * (risk.GetUpperBound(0) + 1) + oldX;
                                        int newY = tileY * (risk.GetUpperBound(1) + 1) + oldY;
                                        int copyFromX = (tileX - 1) * (risk.GetUpperBound(0) + 1) + oldX;
                                        int newRisk = expanded[copyFromX, newY] == 9 ? 1 : expanded[copyFromX, newY] + 1;
                                        expanded[newX, newY] = newRisk;
                                        expandedDist[newX, newY] = 4000;
                                    }
                                }
                            }
                        }
                    }
                }
                risk = expanded;
                minDist = expandedDist;
            }


            int[] point = new int[2] { 0, 0 };
            Stack<int[]> nodes = new Stack<int[]>();
            nodes.Push(point);
            

            while (nodes.Count > 0) {
                point = nodes.Pop();
                foreach (int[] newPoint in findNeighbours(point, risk)) {
                    int dist = point[0] == 0 && point[1] == 0 ? risk[newPoint[0], newPoint[1]] : minDist[point[0], point[1]] + risk[newPoint[0], newPoint[1]];
                    if (dist < minDist[newPoint[0], newPoint[1]]) {
                        minDist[newPoint[0], newPoint[1]] = dist;
                        nodes.Push(newPoint);                        
                    }
                }
            }

            Console.WriteLine($"Answer is {minDist[minDist.GetUpperBound(0), minDist.GetUpperBound(1)]}");
            Console.ReadLine();
        }


        static List<int[]> findNeighbours(int[] point, int[,] grid) {
            List<int[]> points = new List<int[]>();
            if (point[1] < grid.GetUpperBound(1))
                points.Add(new int[] { point[0], point[1] + 1 });
            // Right
            if (point[0] < grid.GetUpperBound(0))
                points.Add(new int[] { point[0] + 1, point[1] });
            // Left
            if (point[0] > 0)
                points.Add(new int[] { point[0] - 1, point[1] });
            // Above
            if (point[1] > 0)
                points.Add(new int[] { point[0], point[1] - 1 });
            return points;
        }
    }
}
