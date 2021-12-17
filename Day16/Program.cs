using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Day16
{
    class Program
    {

        static int versionCount = 0;
        static void Main(string[] args) {

            String textInput = File.ReadAllText(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Input.txt");

            BitArray ba = ConvertHexToBitArray(textInput);
            int index = 0;
            long subLiteral = 0;

            while (index < ba.Length-6) {
                byte version = ByteFromBitArray(ba, ref index, 3);
                byte type = ByteFromBitArray(ba, ref index, 3);
                versionCount += version;
                if (type == 4) {
                    long literal = LiteralFromBitArray(ba, ref index);
                } else {
                    // Operator 
                    if (ba[index++]) {
                        //11 - bit number
                        ushort count = UShortFromBitArray(ba, ref index, 11);
                        subLiteral = SubPacketsByCount(type, ba, ref index, count);
                    } else {
                        //15-bit number
                        ushort len = UShortFromBitArray(ba, ref index, 15);
                        subLiteral = SubPackets(type, ba, ref index, len);
                    }
                }

               

            }

            Console.WriteLine($"Part one Answer = {versionCount} Part twoAnswer = {subLiteral}");
            Console.ReadLine();



        }

        static long ProcessLiterals(int type, List<long> literals) {
            long answer = 0;
            switch (type) {
                case 0: answer = literals.Sum();
                    break;   
                case 1: 
                    answer = literals[0];
                    for (int i = 1; i < literals.Count; i++)
                        answer *= literals[i];
                    break;
                case 2: answer = literals.Min();
                    break;
                case 3: answer = literals.Max();
                    break;
                case 5:
                    answer = literals[0] > literals[1] ? 1 : 0;
                    break;
                case 6:
                    answer = literals[0] < literals[1] ? 1 : 0;
                    break;
                case 7:
                    answer = literals[0] == literals[1] ? 1 : 0;
                    break;
            }
            Console.WriteLine($"Answer = {answer}");
            if(answer < 0) {
                Debugger.Break();
            }
            return answer;
        }

        private static long SubPackets(int subType, BitArray ba, ref int index, int length) {
            
            int end = index + length;
            List<long> literals = new List<long>();
            while (index < end) {
                byte version = ByteFromBitArray(ba, ref index, 3);
                byte type = ByteFromBitArray(ba, ref index, 3);
                versionCount += version;
                if (type == 4) {
                    if (subType == 5 && literals.Count == 1)
                        Debugger.Break();
                    long newLiteral = LiteralFromBitArray(ba, ref index);
                    if(newLiteral < 0) {
                        Debugger.Break();
                    }
                    literals.Add(newLiteral);
                } else {
                    if (ba[index++]) {
                        //11 - bit number
                        ushort count = UShortFromBitArray(ba, ref index, 11);
                        literals.Add(SubPacketsByCount(type, ba, ref index, count));
                    } else {
                        //15-bit number
                        ushort len = UShortFromBitArray(ba, ref index, 15);
                        literals.Add(SubPackets(type, ba, ref index, len));
                    }
                }
            }
            return ProcessLiterals(subType,literals);
        }

        private static long SubPacketsByCount(int subType, BitArray ba, ref int index, int count) {

            List<long> literals = new List<long>();
            for (int i=0; i < count;i++) {
                byte version = ByteFromBitArray(ba, ref index, 3);
                byte type = ByteFromBitArray(ba, ref index, 3);
                versionCount += version;
                if (type == 4) {
                    long newLiteral = LiteralFromBitArray(ba, ref index);
                    if (newLiteral < 0) {
                        Debugger.Break();
                    }
                    literals.Add(newLiteral);
                } else {
                    if (ba[index++]) {
                        //11 - bit number
                        ushort newCount = UShortFromBitArray(ba, ref index, 11);
                        literals.Add(SubPacketsByCount(type, ba, ref index, newCount));
                    } else {
                        //15-bit number
                        ushort len = UShortFromBitArray(ba, ref index, 15);
                        literals.Add(SubPackets(type, ba, ref index, len));
                    }
                }
            }
            return ProcessLiterals(subType, literals);
        }

        private static long LiteralFromBitArray(BitArray ba, ref int index) {
            bool moreChunks = true;
            List<byte> buffer = new List<byte>();
            do {
                moreChunks = ba[index++];
                buffer.Add(ByteFromBitArray(ba, ref index, 4));
            } while (moreChunks);
            return LiteralFromByteBuffer(buffer);
        }

        private static long LiteralFromByteBuffer(List<byte> buffer) {
            long retVal = 0;
            for (int i = 0; i < buffer.Count; i++) {
                int upshiftBits = (buffer.Count - i - 1) * 4;
                retVal |= (long)((long)buffer[i] << upshiftBits);
            }
            return retVal;
        }

        private static byte ByteFromBitArray(BitArray bits, ref int startIndex, int length) {
            //int index = startIndex;
            int end = startIndex + length;
            byte retVal = bits[startIndex++] ? (byte)0x01 : (byte)0x00;
            while (startIndex < end) {
                retVal <<= 1;
                retVal |= bits[startIndex++] ? (byte)0x01 : (byte)0x00;
            }
            return retVal;
        }

        private static ushort UShortFromBitArray(BitArray bits, ref int startIndex, int length) {
            //int index = startIndex;
            int end = startIndex + length;
            ushort retVal = bits[startIndex++] ? (ushort)0x0001 : (ushort)0x0000;
            while (startIndex < end) {
                retVal <<= 1;
                retVal |= bits[startIndex++] ? (ushort)0x0001 : (ushort)0x0000;
            }
            return retVal;
        }

        public static BitArray ConvertHexToBitArray(string hexData) {

            BitArray ba = new BitArray(4 * hexData.Length);
            for (int i = 0; i < hexData.Length; i++) {
                byte b = byte.Parse(hexData[i].ToString(), NumberStyles.HexNumber);
                for (int j = 0; j < 4; j++) {
                    ba.Set(i * 4 + j, (b & (1 << (3 - j))) != 0);
                }
            }
            return ba;
        }

      
        
     
    }

   
}
