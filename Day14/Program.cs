using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day14
{
    class Program
    {
        static void Main(string[] args) {
            String[] textInput = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt");


            string input = textInput[0];
            Dictionary<string, string> lookups = new Dictionary<string, string>();
            for (int i = 2; i < textInput.Length; i++) {
                //CH -> B
                string[] parts = textInput[i].Split(new char[] { ' ' });
                lookups.Add(parts[0], parts[0][0] + parts[2]);
            }

            Dictionary<String, long> countsByKey = new Dictionary<string, long>();
            for (int j = 0; j < input.Length - 1; j++) {
                string key = input.Substring(j, 2);
                if (countsByKey.ContainsKey(key)) countsByKey[key]++;
                else countsByKey.Add(key, 1);
            }


            int steps = 40;            

            for (int i = 0; i < steps; i++) {

                Dictionary<String, long> tempCountsByKey = new Dictionary<string, long>();

                foreach (KeyValuePair<string,long> count in countsByKey) {
                    String key = count.Key;
                    if (tempCountsByKey.ContainsKey(lookups[key])) tempCountsByKey[lookups[key]]+=count.Value;
                    else tempCountsByKey.Add(lookups[key], count.Value);

                    key = lookups[key][1].ToString() + count.Key[1].ToString();
                    if (tempCountsByKey.ContainsKey(key)) tempCountsByKey[key]+=count.Value;
                    else tempCountsByKey.Add(key, count.Value);
                }

                countsByKey = tempCountsByKey;

            }


           
            Dictionary<char, long> countsByChar = new Dictionary<char, long>();
            // Preload the last char, to 'close off' the final non overlapping pair
            countsByChar.Add(input[input.Length - 1], 1);

            foreach(KeyValuePair<string,long> count in countsByKey) {
                if (countsByChar.ContainsKey(count.Key[0])) countsByChar[count.Key[0]] += count.Value;
                else countsByChar.Add(count.Key[0], count.Value);                
            }

            long max = countsByChar.Max(x => x.Value);
            long min = countsByChar.Min(x => x.Value);

            Console.WriteLine($"Answer = {max - min}");
            Console.ReadLine();
                        
        }
    }
}
