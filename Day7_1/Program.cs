using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day7_1
{
    class Program
    {
        static void Main(string[] args) {
            int[] crabPositions = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt").Split(new char[] { ',' }).Select(x => int.Parse(x)).ToArray();
            int[] fuelRequired = new int[crabPositions.Max()+1];
            int min = int.MaxValue;

            bool partTwo = true;
            int[] partTwoCosts = new int[fuelRequired.Length];
            partTwoCosts[1] = 1;
            for(int i = 2; i< partTwoCosts.Length; i++) {
                partTwoCosts[i] = partTwoCosts[i - 1] + i;
            }
         
            for(int i = 0; i < fuelRequired.Length; i++) {
                for(int j = 0; j < crabPositions.Length; j++) {
                    if (partTwo) fuelRequired[i] += partTwoCosts[crabPositions[j] > i ? crabPositions[j] - i : (crabPositions[j] < i ? i - crabPositions[j] : 0)];
                    else fuelRequired[i] += crabPositions[j] > i ? crabPositions[j] - i : (crabPositions[j] < i ? i - crabPositions[j] : 0);                    
                }
                if (fuelRequired[i] < min) min = fuelRequired[i];
            }

            Console.WriteLine($"Answer is {min}");
            Console.ReadLine();
        }
    }
}
