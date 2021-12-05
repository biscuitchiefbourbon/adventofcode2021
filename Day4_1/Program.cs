using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day4_1
{
    class Program
    {
        static void Main(string[] args) {
            String[] inputAsStrings = File.ReadAllLines(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt");
            int startIndex = 0;

            int[] callNumbers = inputAsStrings[startIndex++].Split(new char[] { ',' }).Select(x => int.Parse(x)).ToArray();

            List<BingoCard> cards = new List<BingoCard>();
            while (startIndex < inputAsStrings.Length) {
                cards.Add(new BingoCard(inputAsStrings, ref startIndex));
            }


            PartTwo(callNumbers,cards);
        }

        private static void PartOne(int[] callNumbers, List<BingoCard> cards) {
            
            int? answer = null;
            foreach (int callNumber in callNumbers) {
                foreach (BingoCard card in cards) {
                    if (card.CallNumber(callNumber)) {
                        answer = card.UnmarkedSum * callNumber;
                        break;
                    }
                }
                if (answer.HasValue) break;
            }

            Console.WriteLine($"Answer is {answer}");
            Console.ReadLine();
        }

        private static void PartTwo(int[] callNumbers, List<BingoCard> cards) {

            List<BingoCard> winners = new List<BingoCard>();
            foreach (int callNumber in callNumbers) {
                foreach (BingoCard card in cards) {                    
                    if (!card.IsWinner && card.CallNumber(callNumber)) {
                        winners.Add(card);
                    }
                }                
            }
            int answer = winners[winners.Count - 1].UnmarkedSum * winners[winners.Count - 1].WinningNumber.Value;
            Console.WriteLine($"Answer is {answer}");
            Console.ReadLine();
        }



        private class BingoCard
        {
            public const int HORIZONTAL_SIZE = 5;
            public const int VERTICAL_SIZE = 5;
            private BingoNumber[,] _numbers;
            public bool IsWinner { get; set; }
            public int? WinningNumber { get; set; }


            public BingoCard(String[] input, ref int startIndex) {
                _numbers = new BingoNumber[HORIZONTAL_SIZE, VERTICAL_SIZE];                
                int row = 0, column = 0;
                while (String.IsNullOrWhiteSpace(input[startIndex])) { startIndex++; }

                do {
                    foreach (int number in input[startIndex].Split(new char[] { ' ' }).Where(x => !String.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x.Trim())).ToArray()) {
                        _numbers[row++, column] = new BingoNumber(number);
                    }
                    column++;
                    row = 0;
                    startIndex++;
                } while (!(startIndex < input.Length && String.IsNullOrWhiteSpace(input[startIndex])) && startIndex < input.Length);
            }

            public bool CallNumber(int number) {
                for(int i = 0; i < HORIZONTAL_SIZE; i++) {
                    for(int j = 0; j < VERTICAL_SIZE; j++) {
                        if(_numbers[i,j].Number== number) {
                            _numbers[i, j].IsCalled = true;
                            IsWinner = checkHorizontal(j) || checkVertical(i);
                            if (IsWinner) WinningNumber = number;
                            return IsWinner;
                        }
                    }
                }
                return false;
            }

            public int UnmarkedSum {
                get {
                    int sum = 0;
                    for(int i = 0; i < HORIZONTAL_SIZE; i++) {
                        for(int j = 0; j < VERTICAL_SIZE; j++) {
                            sum += _numbers[i, j].IsCalled ? 0 : _numbers[i, j].Number;
                        }
                    }
                    return sum;
                }
            }

            private bool checkHorizontal(int y) {                
                for (int i = 0; i < HORIZONTAL_SIZE; i++) {
                    if (!_numbers[i, y].IsCalled) return false;                    
                }
                return true;
            }

            private bool checkVertical(int x) {
                for (int i = 0; i < VERTICAL_SIZE; i++) {
                    if (!_numbers[x, i].IsCalled) return false;
                }
                return true;
            }

            private struct BingoNumber
            {
                public int Number;
                public bool IsCalled;

                public BingoNumber(int number) {
                    Number = number;
                    IsCalled = false;
                }
            }
        }
    }
}
