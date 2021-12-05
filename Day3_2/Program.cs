using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day3_2
{
    class Program
    {
        static void Main(string[] args) {

            String[] inputAsStrings = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt");
            int numBits = inputAsStrings[0].Length;
            uint[] inputAsNumbers = inputAsStrings.Select(s => Convert.ToUInt32(s, 2)).ToArray();
            int[] oneCounts = GetOneCounts(numBits, inputAsNumbers);
                      
            uint[] oxy = inputAsNumbers;
            uint[] co2 = inputAsNumbers;
            int[] oxyOneCounts = oneCounts;
            int[] co2OneCounts = oneCounts;
            uint? oxyRating = null;
            uint? co2Rating = null;

            for (int i = 0; i < oneCounts.Length; i++) {
                uint mask = (uint)(0x00000001 << (oneCounts.Length - i - 1));
                if (!oxyRating.HasValue) {
                    if (oxyOneCounts[i] >= (oxy.Length - oxyOneCounts[i]))
                        oxy = oxy.Where(x => ((x & mask) > 0)).ToArray();
                    else
                        oxy = oxy.Where(x => ((x & mask) == 0)).ToArray();

                    if(oxy.Length == 1) oxyRating = oxy[0];
                    else oxyOneCounts = GetOneCounts(numBits, oxy);
                }
                if (!co2Rating.HasValue) {
                    if (co2OneCounts[i] < (co2.Length - co2OneCounts[i]))
                        co2 = co2.Where(x => ((x & mask) > 0)).ToArray();
                    else
                        co2 = co2.Where(x => ((x & mask) == 0)).ToArray();

                    if (co2.Length == 1) co2Rating = co2[0];
                    else co2OneCounts = GetOneCounts(numBits, co2);
                }

                if (oxyRating.HasValue && co2Rating.HasValue) break;
            }

            Console.WriteLine($"Oxygen rating = {oxyRating} CO2 Rating = {co2Rating} Life support rating = {oxyRating * co2Rating}");
            Console.ReadLine();

           
        }

        static int[] GetOneCounts(int numBits, uint[] input) {
            int[] oneCounts = null;
            foreach (uint num in input) {
                if (oneCounts == null) oneCounts = new int[numBits];
                for (int i = 0; i < oneCounts.Length; i++) {
                    if ((num & (0x00000001 << (oneCounts.Length - i - 1))) > 0) {
                        oneCounts[i]++;
                    }
                }                
            }
            return oneCounts;
        }
    }
}
