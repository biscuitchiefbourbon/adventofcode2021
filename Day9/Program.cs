using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day9
{
    class Program
    {
        static int ABOVE = 0;
        static int BELOW = 1;
        static int LEFT = 2;
        static int RIGHT = 3;

        static void Main(string[] args) {
            String[] textInput = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt");

            int[,] heightMap = new int[textInput[0].Length, textInput.Length];
            for (int y = 0; y < textInput.Length; y++)
                for (int x = 0; x < textInput[y].Length; x++)
                    heightMap[x, y] = (int)textInput[y][x] - 48;

            List<int> riskLevels = new List<int>();
            List<int> basinSizes = new List<int>();

            for (int y = 0; y < textInput.Length; y++)
                for (int x = 0; x < textInput[y].Length; x++) {
                    Point?[] adjacent = getAdjacent(heightMap, x, y);
                    int numMoreThanCurrent = adjacent.Where(p => p.HasValue && p.Value.Height > heightMap[x, y]).Count();
                    int total = adjacent.Where(p => p.HasValue).Count();
                    if (numMoreThanCurrent == total) { 
                        riskLevels.Add(1 + heightMap[x, y]);
                        Point[] basin = getBasin(heightMap, new Point(x, y, heightMap[x, y]));
                        basinSizes.Add(basin.Length);
                    }
                }


            basinSizes.Sort();
            int partTwoAnswer = basinSizes[basinSizes.Count - 1] * basinSizes[basinSizes.Count - 2] * basinSizes[basinSizes.Count - 3];

            Console.WriteLine($"Part one answer is {riskLevels.Sum()} Part two answer is {partTwoAnswer}");
            Console.ReadLine();

            

        }


        static Point?[] getAdjacent(int[,] heightMap, int x, int y) {
            Point?[] adjacent = new Point?[4];
            if (y > 0) adjacent[ABOVE] = new Point(x, y - 1, heightMap[x, y - 1]);
            if (y < heightMap.GetUpperBound(1)) adjacent[BELOW] = new Point(x, y + 1, heightMap[x, y + 1]);
            if (x > 0) adjacent[LEFT] = new Point(x - 1, y, heightMap[x - 1, y]);
            if (x < heightMap.GetUpperBound(0)) adjacent[RIGHT] = new Point(x + 1, y, heightMap[x + 1, y]);            
            return adjacent;
        }

        static Point[] getBasin(int[,] heightMap, Point lowest) {
            List<Point> basin = new List<Point>();
            basin.Add(lowest);
            
            Stack<Point> basinStack = new Stack<Point>();
            foreach (Point? p in getAdjacent(heightMap, lowest.X, lowest.Y)) {
                if (p.HasValue && p.Value.Height > heightMap[lowest.X,lowest.Y]) basinStack.Push(p.Value);
            }

            while(basinStack.Count > 0) {
                Point newPoint = basinStack.Pop();
                if (newPoint.Height < 9 && !basin.Contains(newPoint)) basin.Add(newPoint);
                foreach (Point? p in getAdjacent(heightMap, newPoint.X, newPoint.Y)) {
                    if (p.HasValue && p.Value.Height > heightMap[newPoint.X, newPoint.Y] && p.Value.Height < 9) basinStack.Push(p.Value);
                }
            }
            return basin.ToArray();
        }

        private struct Point
        {
            public int X;
            public int Y;
            public int Height;

            public Point(int x, int y, int height) {
                X = x;
                Y = y;
                Height = height;
            }

            public override bool Equals(object obj) {
                Point other = (Point)obj;
                return this.X == other.X && this.Y == other.Y && this.Height == other.Height;
            }


        }

    }

   
}
