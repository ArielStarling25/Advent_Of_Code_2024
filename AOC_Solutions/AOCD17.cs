using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD17
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD17.txt";

        private int[] operandContain = new int[] { 0, 1, 2, 3, 0, 0, 0, -1 }; // 4 = A, 5 = B, 6 = C, 7 = -1
        private List<Instruction> instructions = new List<Instruction>(); // flawed approach
        private List<int> operands = new List<int>();

        private string oriInst = "";
        private List<long> program = new List<long>();

        public AOCD17()
        {
            init();
            Console.WriteLine();
            Console.WriteLine("Register A: " + operandContain[4]);
            Console.WriteLine("Register B: " + operandContain[5]);
            Console.WriteLine("Register C: " + operandContain[6]);
            foreach(Instruction inst in instructions)
            {
                Console.WriteLine(inst);
            }
            foreach(int num in operands)
            {
                Console.Write(num + ",");
                oriInst += (num + ",");
            }
        }

        public void solve1()
        {
            long[] operandCon = makeLongCopy(operandContain);
            string res = compute(operands, operandCon);
            Console.WriteLine("Final 1: " + res);
        }

        public void solve2()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            long[] operandCon = makeLongCopy(operandContain);
            long result = computeMatcher(operands, operandCon);
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2b()
        {
            Console.WriteLine();
            Console.WriteLine(Find(makeLongCopy(program.ToArray()), 0));
        }

        private long computeMatcher(List<int> instruct, long[] operandContainer)
        {
            string output = "";
            long aVal = 0;
            while(output != oriInst)
            {
                if (output.Length < oriInst.Length)
                {
                    aVal += ((output.Length + oriInst.Length) * (output.Length * 1000));
                }
                else
                {
                    aVal++;
                }
                operandContainer[4] = aVal;
                output = compute(instruct, makeLongCopy(operandContainer));
                Console.WriteLine("OUTPUT:[" + output + "]|aVal: " + aVal);
                
            }
            Console.WriteLine(output + " <--- Computed ");
            Console.WriteLine(oriInst + " <--- Original");
            Console.WriteLine("A Val: " + aVal);
            return aVal;
        }

        private string compute(List<int> instruct, long[] operandContainer)
        {
            string res = "";
            for(int i = 0; i < instruct.Count-1; i = i + 2)
            {
                Instruction process = new Instruction(instruct[i], instruct[i+1]);
                switch (process.opcode)
                {
                    case 0: // adv
                        long numerator = operandContainer[4];
                        long denominator = (int)Math.Pow(2, operandContainer[process.operand]);
                        operandContainer[4] = numerator / denominator;
                        break;
                    case 1: // bxl
                        UInt64 n1 = Convert.ToUInt64(operandContainer[5]);
                        UInt64 n2 = Convert.ToUInt64(process.operand); // literal operand or value?
                        UInt64 n3 = n1 ^ n2;
                        operandContainer[5] = (long)Convert.ToInt64(n3);
                        break;
                    case 2: // bst
                        operandContainer[5] = operandContainer[process.operand] % 8;
                        break;
                    case 3: // jnz
                        if (operandContainer[4] != 0)
                        {
                            i = process.operand - 2; // the -2 is to mimic the no changes on the next iteration
                        }
                        break;
                    case 4: // bxc
                        UInt64 n4 = Convert.ToUInt64(operandContainer[5]);
                        UInt64 n5 = Convert.ToUInt64(operandContainer[6]);
                        UInt64 n6 = n4 ^ n5;
                        operandContainer[5] = (long)Convert.ToInt64(n6);
                        break;
                    case 5: // out
                        res += (operandContainer[process.operand] % 8) + ",";
                        break;
                    case 6: // bdv
                        long numerator2 = operandContainer[4];
                        long denominator2 = (int)Math.Pow(2, operandContainer[process.operand]);
                        operandContainer[5] = numerator2 / denominator2;
                        break;
                    case 7: // cdv
                        long numerator3 = operandContainer[4];
                        long denominator3 = (int)Math.Pow(2, operandContainer[process.operand]);
                        operandContainer[6] = numerator3 / denominator3;
                        break;
                    default:
                        Console.Error.WriteLine("INVALID OPCODE: " + process.opcode);
                        break;
                }
            }   
            return res;
        }

        private long? Find(long[] target, long ans)
        {
            if (target.Length == 0) return ans;

            for (long t = 0; t < 8; t++)
            {
                long a = (ans << 3) | t;
                long b = 0;
                long c = 0;
                long? output = null;
                bool adv3 = false;

                long Combo(long operand)
                {
                    if (operand >= 0 && operand <= 3) return operand;
                    if (operand == 4) return a;
                    if (operand == 5) return b;
                    if (operand == 6) return c;
                    throw new ArgumentException($"unrecognized combo operand {operand}");
                }

                for (int pointer = 0; pointer < program.Count - 2; pointer += 2)
                {
                    long ins = program[pointer];
                    long operand = program[pointer + 1];

                    if (ins == 0)
                    {
                        if (adv3) throw new InvalidOperationException("program has multiple ADVs");
                        if (operand != 3) throw new InvalidOperationException("program has ADV with operand other than 3");
                        adv3 = true;
                    }
                    else if (ins == 1)
                    {
                        b ^= operand;
                    }
                    else if (ins == 2)
                    {
                        b = Combo(operand) % 8;
                    }
                    else if (ins == 3)
                    {
                        throw new InvalidOperationException("program has JNZ inside expected loop body");
                    }
                    else if (ins == 4)
                    {
                        b ^= c;
                    }
                    else if (ins == 5)
                    {
                        if (output.HasValue) throw new InvalidOperationException("program has multiple OUT");
                        output = Combo(operand) % 8;
                    }
                    else if (ins == 6)
                    {
                        long numerator2 = a;
                        long denominator2 = (int)Math.Pow(2, Combo(operand));
                        b = numerator2 / denominator2;
                    }
                    else if (ins == 7)
                    {
                        long numerator3 = a;
                        long denominator3 = (int)Math.Pow(2, Combo(operand));
                        c = numerator3 / denominator3;
                    }

                    if (output == target[target.Length - 1])
                    {
                        long? sub = Find(target[0..^1], a);
                        if (sub == null) continue;
                        return sub;
                    }
                }
            }
            return null;
        }

        internal class Instruction
        {
            public int opcode { get; set; }
            public int operand { get; set; }
            public Instruction(int opcode, int operand)
            {
                this.opcode = opcode;
                this.operand = operand;
            }

            public override string ToString()
            {
                return "OPCODE:[" + opcode + "]|OPERAND:[" + operand + "]";
            }
        }

        private int[] makeCopy(int[] input)
        {
            int[] newArr = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                newArr[i] = input[i];   
            }
            return newArr;
        }

        private long[] makeLongCopy(int[] input)
        {
            long[] newArr = new long[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                newArr[i] = input[i];
            }
            return newArr;
        }

        private long[] makeLongCopy(long[] input)
        {
            long[] newArr = new long[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                newArr[i] = input[i];
            }
            return newArr;
        }

        private void printArr(string[] arr)
        {
            Console.WriteLine("|");
            foreach (string str in arr)
            {
                Console.Write(str + "|");
            }
        }

        private void init()
        {
            string line;
            int count = 0;
            bool newSect = false;
            try
            {
                StreamReader sr = new StreamReader(inputFile);
                line = sr.ReadLine();
                while (line != null)
                {
                    count++;
                    //VVV Do file reading operations here VVV
                    string[] str = line.Split(new char[] {' ', ':', ','});
                    if (str.Length < 2)
                    {
                        newSect = true;
                    }
                    if (!newSect)
                    {
                        operandContain[4] = int.Parse(str[3]);
                        printArr(str);

                        line = sr.ReadLine();
                        string[] str2 = line.Split(new char[] { ' ', ':', ',' });
                        operandContain[5] = int.Parse(str2[3]);
                        printArr(str2);

                        line = sr.ReadLine();
                        string[] str3 = line.Split(new char[] { ' ', ':', ',' });
                        operandContain[6] = int.Parse(str3[3]);
                        printArr(str3);
                    }
                    else
                    {
                        printArr(str);
                        for(int i = 2; i < str.Length-1; i = i + 2)
                        {
                            Instruction inst = new Instruction(int.Parse(str[i]), int.Parse(str[i+1]));
                            instructions.Add(inst);
                        }
                        for (int i = 2; i < str.Length; i++)
                        {
                            operands.Add(int.Parse(str[i]));
                            program.Add(long.Parse(str[i]));
                        }
                    }
                    //printArr(str);
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day {DayNum} Resources Initialized! ");
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.Message);
            }
        }
    }
}
