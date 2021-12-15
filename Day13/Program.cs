using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day13
{
    class Program
    {
        static void Main(string[] args) {
            String[] textInput = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt");
            List<int> xValues = new List<int>();
            List<int> yValues = new List<int>();
            List<string> folds = new List<string>();           

            foreach (String s in textInput) {
                if (s.Contains(",")) {
                    string[] parts = s.Split(new char[] { ',' });
                    xValues.Add(int.Parse(parts[0]));
                    yValues.Add(int.Parse(parts[1]));
                } else if (s.StartsWith("fold along")) {
                    folds.Add(s);
                }
            }

            int[,] grid = new int[xValues.Max() + 1, yValues.Max() + 1];
            for (int i = 0; i < xValues.Count; i++) {
                grid[xValues[i], yValues[i]] = 1;
            }

            bool first = true;
            int[,] newGrid = null;

            foreach (String fold in folds) {
                int foldNum = int.Parse(fold.Split(new char[] { '=' })[1]);


                if (fold.StartsWith("fold along y")) {
                    int top = foldNum;
                    int bottom = grid.GetUpperBound(1) - top;

                    newGrid = new int[grid.GetUpperBound(0) + 1, top > bottom ? top : bottom];

                    for (int x = 0; x <= grid.GetUpperBound(0); x++) {
                        for (int y = 0; y <= grid.GetUpperBound(1); y++) {
                            if (top >= bottom) {
                                if (y < top) newGrid[x, y] |= grid[x, y];
                                else if (y > top) newGrid[x, 2 * top - y] |= grid[x, y];
                            } else {
                                if (y > top) newGrid[x, y - bottom - 1] |= grid[x, y];
                                else if (y < top) newGrid[x, top - 1 + y] |= grid[x, y];
                            }
                        }
                    }

                    int totalDots = (from int val in newGrid select val).Sum();

                    if (first) {
                        Console.WriteLine($"Part one answer = {totalDots}");                        
                    }

                    grid = newGrid;
                } else if (fold.StartsWith("fold along x")) {
                    int left = foldNum;
                    int right = grid.GetUpperBound(0) - left;

                    newGrid = new int[left > right ? left : right, grid.GetUpperBound(1) + 1];

                    for (int x = 0; x <= grid.GetUpperBound(0); x++) {
                        for (int y = 0; y <= grid.GetUpperBound(1); y++) {
                            if (left >= right) {
                                if (x < left) newGrid[x, y] |= grid[x, y];
                                else if (x > left) newGrid[2 * left - x, y] |= grid[x, y];
                            } else {
                                if (x > left) newGrid[x - right - 1, y] |= grid[x, y];
                                else if (x < left) newGrid[left - 1 + x, y] |= grid[x, y];
                            }
                        }
                    }

                    int totalDots = (from int val in newGrid select val).Sum();

                    if (first) {
                        Console.WriteLine($"Part one answer = {totalDots}");                       
                    }
                    grid = newGrid;
                }

                first = false;
            }

            for(int y = 0; y <= newGrid.GetUpperBound(1); y++) {
                for(int x = 0; x <= newGrid.GetUpperBound(0); x++) {
                    Console.Write("{0}", newGrid[x, y]==1?"#":" ");
                }
                Console.WriteLine();
            }

            Console.ReadLine();





        }
    }
}
