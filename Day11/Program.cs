using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day11
{
    class Program
    {
        static readonly int TOP = 0;
        static readonly int BOTTOM = 1;
        static readonly int LEFT = 2;
        static readonly int RIGHT = 3;
        static readonly int TOPLEFT = 4;
        static readonly int TOPRIGHT = 5;
        static readonly int BOTTOMLEFT = 6;
        static readonly int BOTTOMRIGHT = 7;

        static readonly int STEPS = 100;

        static void Main(string[] args) {
            String[] textInput = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt");
            Octopus[,] octopuses = new Octopus[textInput[0].Length, textInput.Length];
            for (int y = 0; y < textInput.Length; y++)
                for (int x = 0; x < textInput[y].Length; x++)
                    octopuses[x, y] = new Octopus(x, y, int.Parse(textInput[y][x].ToString()),1000);

            
            int numFlashes = 0;
            int flashesPerStep = 0;
            int firstSynchronisedStepFlash = 0;
            
            for(int i = 0; i < 1000; i++) {
                flashesPerStep = 0;
                Stack<Octopus> octopusesToIncrease = new Stack<Octopus>();
                
                // Add every octopus for energy increase
                for (int x = 0; x <= octopuses.GetUpperBound(0); x++) 
                    for(int y = 0; y <= octopuses.GetUpperBound(1); y++) 
                        octopusesToIncrease.Push(octopuses[x, y]);
                    
                while(octopusesToIncrease.Count > 0) {
                    Octopus o = octopusesToIncrease.Pop();
                    if (o.IncreaseEnergy(i)) {
                        numFlashes++; flashesPerStep++;
                        foreach (Octopus adjacent in getAdjacent(octopuses, o.X, o.Y)) {
                            if (adjacent != null) octopusesToIncrease.Push(adjacent);
                        }
                    }
                }

                if (flashesPerStep == (octopuses.GetUpperBound(0) + 1) * (octopuses.GetUpperBound(1) + 1)) {
                    firstSynchronisedStepFlash = i + 1;
                    break;
                }
                if(i+1 == STEPS) Console.WriteLine($"Part one answer = {numFlashes}");
            }

            Console.WriteLine($"Part two answer = {firstSynchronisedStepFlash} steps");
            Console.ReadLine();


        }

        static Octopus[] getAdjacent(Octopus[,] octupuses, int x, int y) {
            Octopus[] adjacent = new Octopus[8];
            if (y > 0) adjacent[TOP] = octupuses[x, y - 1];
            if (y < octupuses.GetUpperBound(1)) adjacent[BOTTOM] = octupuses[x, y + 1];
            if (x > 0) adjacent[LEFT] = octupuses[x - 1, y];
            if (x < octupuses.GetUpperBound(0)) adjacent[RIGHT] = octupuses[x + 1, y];
            if (x > 0 && y > 0) adjacent[TOPLEFT] = octupuses[x - 1, y - 1];
            if (y > 0 && x < octupuses.GetUpperBound(0)) adjacent[TOPRIGHT] = octupuses[x + 1, y - 1];
            if (y < octupuses.GetUpperBound(1) && x > 0) adjacent[BOTTOMLEFT] = octupuses[x - 1, y + 1];
            if (y < octupuses.GetUpperBound(1) && x < octupuses.GetUpperBound(0)) adjacent[BOTTOMRIGHT] = octupuses[x + 1, y + 1];

            return adjacent;
        }

        private class Octopus
        {
            public int X;
            public int Y;
            public int Energy;
            public bool[] StepFlashes;

            public Octopus(int x, int y, int energy, int steps) {
                X = x;
                Y = y;
                Energy = energy;
                StepFlashes = new bool[steps];
            }

            public bool IncreaseEnergy(int step) {
                if (!StepFlashes[step]) {
                    // We haven't flashed yet on this step
                    Energy++;
                    if( Energy > 9) {
                        // Save flash state for this step, reset energy, return true to let the caller know we have flashed
                        StepFlashes[step] = true;
                        Energy = 0;
                        return true;
                    }
                }
                // Return false to tell the caller we haven't flashed on this step (or we have already flashed on this step)
                return false;
            }
           
        }
    }
}
