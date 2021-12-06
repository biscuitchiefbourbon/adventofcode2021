using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day6_1
{
    class Program
    {
        static void Main(string[] args) {
            bool partTwo = false;
            int[] initialAges = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt").Split(new char[] { ',' }).Select(x => int.Parse(x)).ToArray();

            // First attempt below blows up as it uses to much memory

            // List<int> newFish = new List<int>();
            //for (int i = 0; i < 256; i++) {
            //    for (int j = 0; j < initialAges.Length; j++) {
            //        if (initialAges[j] > 0) initialAges[j]--;
            //        else {
            //            initialAges[j] = 6;
            //            newFish.Add(8);
            //        }
            //    }
            //    initialAges = initialAges.Concat(newFish.ToArray()).ToArray();
            //    newFish.Clear();
            //}

            //Console.WriteLine($"Answer is {initialAges.Length}");
            //Console.ReadLine();


            long[] startingCounts = new long[] { 0, 0, 0, 0, 0, 0, 0 };
            foreach (int startingAge in initialAges) {
                startingCounts[startingAge]++;
            }

            long[] newFishStartingCounts = new long[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };


            for (int i = 0; i < (partTwo ? 256 : 80); i++) {
                long numResetting = startingCounts[i % 7];
                long numNewFishResetting = newFishStartingCounts[i % 9];
                startingCounts[i % 7] += numNewFishResetting;
                newFishStartingCounts[i % 9] = numResetting + numNewFishResetting;
            }

            long answer = startingCounts.Concat(newFishStartingCounts).Sum();
            Console.WriteLine($"Answer is {answer}");
            Console.ReadLine();


            
        }


    }
}
