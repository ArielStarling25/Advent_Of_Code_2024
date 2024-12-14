using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD13
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD13.txt";

        private List<ClawMachine> clawMachines = new List<ClawMachine>();

        private readonly long adjust = 10000000000000; // 10 trillion

        public AOCD13()
        {
            init();
        }

        public void solve1()
        {
            long result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            foreach(ClawMachine machine in clawMachines)
            {
                machine.minTokens = minTokensToWin(machine);
                Console.WriteLine(machine);
                if(machine.minTokens > 0)
                {
                    result += machine.minTokens;
                }
            }
            timer.Stop();
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2()
        {
            long result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            foreach (ClawMachine machine in clawMachines)
            {
                long[] prizeAdjust = machine.prize;
                prizeAdjust[0] = prizeAdjust[0] + adjust;
                prizeAdjust[1] = prizeAdjust[1] + adjust;
                machine.setPrize(prizeAdjust);
                machine.minTokens = minTokensToWinEnhanced3(machine);
                Console.WriteLine(machine);
                if (machine.minTokens > 0)
                {
                    result += machine.minTokens;
                }
            }
            timer.Stop();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        private int minTokensToWin(ClawMachine machine)
        {
            int minTokens = int.MaxValue;
            for (int aPresses = 0; aPresses <= 100; aPresses++)
            {
                for (int bPresses = 0; bPresses <= 100; bPresses++)
                {
                    if (aPresses * machine.buttonA.valueX + bPresses * machine.buttonB.valueX == machine.prize[0] && aPresses * machine.buttonA.valueY + bPresses * machine.buttonB.valueY == machine.prize[1])
                    {
                        int tokens = (aPresses * 3) + bPresses;
                        minTokens = Math.Min(minTokens, tokens);
                    }
                }
            }
            return minTokens == int.MaxValue ? -1 : minTokens;
        }

        private long minTokensToWinExtended(ClawMachine machine) // Possibly the slowest possible ever
        {
            long minTokens = int.MaxValue;
            for (int aPresses = 0; aPresses <= adjust; aPresses++)
            {
                Console.WriteLine("Press:" + aPresses);
                for (int bPresses = 0; bPresses <= adjust; bPresses++)
                {
                    if (aPresses * machine.buttonA.valueX + bPresses * machine.buttonB.valueX == machine.prize[0] && aPresses * machine.buttonA.valueY + bPresses * machine.buttonB.valueY == machine.prize[1])
                    {
                        long tokens = (aPresses * 3) + bPresses;
                        minTokens = Math.Min(minTokens, tokens);
                    }
                }
            }
            return minTokens == int.MaxValue ? -1 : minTokens;
        }

        private long minTokensToWinEnhanced(ClawMachine machine)
        {
            int Ax = machine.buttonA.valueX, Ay = machine.buttonA.valueY;
            int Bx = machine.buttonB.valueX, By = machine.buttonB.valueY;
            long prize_x = machine.prize[0], prize_y = machine.prize[1];

            long gcdA = gcd(Ax, Ay);
            long gcdB = gcd(Bx, By);

            Console.WriteLine("GCDA:[" + gcdA + "]GCDB[" + gcdB + "]");
            // Check if the prizes are multiples of the gcd of A or B
            if (prize_x % gcdA == 0 && prize_y % gcdA == 0 && gcdA != 1)
            {
                long a_presses = prize_x / Ax;
                long b_presses = prize_y / By;
                long tokens = a_presses * 3 + b_presses;
                return tokens;
            }
            else if (prize_x % gcdB == 0 && prize_y % gcdB == 0 && gcdB != 1)
            {
                long a_presses = prize_x / Bx;
                long b_presses = prize_y / By;
                long tokens = a_presses * 3 + b_presses;
                return tokens;
            }
            else
            {
                return -1;
            }
        }

        private long minTokensToWinEnhanced2(ClawMachine machine)
        {
            if (machine.prize[0] % gcd(machine.buttonA.valueX, machine.buttonB.valueX) != 0 || machine.prize[1] % gcd(machine.buttonA.valueY, machine.buttonB.valueY) != 0)
            {
                return -1;
            }

            long lcmX = lcm(machine.buttonA.valueX, machine.buttonB.valueX);
            long lcmY = lcm(machine.buttonA.valueY, machine.buttonB.valueY);

            long aPresses = (machine.prize[0] * lcmY - machine.prize[1] * lcmX) / (machine.buttonA.valueX * lcmY - machine.buttonA.valueY * lcmX);
            long bPresses = (machine.prize[0] - aPresses * machine.buttonA.valueX) / machine.buttonB.valueX;

            if(aPresses < 0 || bPresses < 0)
            {
                return -1;
            }

            return (aPresses * 3) + bPresses;
        }

        private long minTokensToWinEnhanced3(ClawMachine machine) // Edison big brein moment, i no brein, i smooth brein
        {
            long ax = machine.buttonA.valueX, ay = machine.buttonA.valueY;
            long bx = machine.buttonB.valueX, by = machine.buttonB.valueY;
            long px = machine.prize[0], py = machine.prize[1];
            if(!isWholeNumber((ay * px) - (ax * py), (bx * ay) - (by * ax)))
            {
                return -1;
            }
            long bPresses = ((ay * px) - (ax * py))/((bx * ay) - (by * ax));
            if(!isWholeNumber(px - (bx * bPresses), ax))
            {
                return -1;
            }
            long aPresses = (px - (bx * bPresses)) / ax;
            long res = (aPresses * 3) + bPresses;
            if(bPresses < 0 || aPresses < 0)
            {
                return -1;
            }
            else
            {
                return res;
            }
        }

        private bool isWholeNumber(double num)
        {
            return num % 1 == 0;
        }

        private bool isWholeNumber(long num, long num2)
        {
            return (num % num2) == 0;
        }

        private long lcm(long in1, long in2)
        {
            return (in1 / gcd(in1, in2)) * in2;
        }

        private long gcd(long in1, long in2) // greatest common denominator
        {
            if(in1 == 0)
            {
                return in2;
            }
            else
            {
                return gcd(in2 % in1, in1);
            }
        }

        internal class ClawMachine
        {
            public Button buttonA {  get; set; }
            public Button buttonB { get; set; }
            public long[] prize {  get; set; }
            public long minTokens { get; set; }
            public ClawMachine(long[] prize, int bAX, int bAY, int bBX, int bBY)
            {
                this.prize = prize;
                buttonA = new Button(bAX, bAY);
                buttonB = new Button(bBX, bBY);
                minTokens = -1;
            }
            public ClawMachine()
            {
                prize = new long[2];
                buttonA = new Button(0, 0);
                buttonB = new Button(0, 0);
                minTokens = -1;
            }
            public void setPrize(long[] prize)
            {
                this.prize = prize;
            }
            public void setButtonA(int bAX, int bAY)
            {
                buttonA.valueX = bAX;
                buttonA.valueY = bAY;
            }
            public void setButtonB(int bBX, int bBY)
            {
                buttonB.valueX = bBX;
                buttonB.valueY = bBY;
            }
            public override string ToString()
            {
                string val = "Button A: X+" + buttonA.valueX + ", Y+" + buttonA.valueY;
                val += "\nButton B: X+" + buttonB.valueX + ", Y+" + buttonB.valueY;
                val += "\nPrize: X=" + prize[0] + ", Y=" + prize[1];
                val += "\nMin Tokens: " + minTokens + "";
                return val;
            }
        }

        internal class Button
        {
            public int valueX { get; set; }
            public int valueY { get; set; }
            public Button(int x, int y)
            {
                valueX = x;
                valueY = y;
            }
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
            try
            {
                StreamReader sr = new StreamReader(inputFile);
                line = sr.ReadLine();
                int claw = 0;
                bool createdClaw = false;
                while (line != null)
                {
                    count++;
                    //VVV Do file reading operations here VVV
                    string[] strings = line.Split(new char[] {' ', ':', '+', '=', ','});
                    //printArr(strings);
                    if(strings.Length > 2)
                    {
                        if (strings[0] == "Button" && strings[1] == "A")
                        {
                            ClawMachine newClaw = new ClawMachine();
                            newClaw.setButtonA(int.Parse(strings[4]), int.Parse(strings[7]));
                            line = sr.ReadLine();
                            string[] strings2 = line.Split(new char[] { ' ', ':', '+', '=', ',' });
                            newClaw.setButtonB(int.Parse(strings2[4]), int.Parse(strings2[7]));
                            line = sr.ReadLine();
                            string[] strings3 = line.Split(new char[] { ' ', ':', '+', '=', ',' });
                            newClaw.setPrize(new long[] { int.Parse(strings3[3]), int.Parse(strings3[6])});
                            clawMachines.Add(newClaw);
                        }
                    }
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
