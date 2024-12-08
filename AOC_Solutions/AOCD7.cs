using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD7
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD7.txt";

        private List<DataHolder> equations = new List<DataHolder>();
        private readonly char[] charChoices = { '+', '*', '|'}; // ADDED '|' for Part 2

        public AOCD7()
        {
            init();
        }

        public void solve12()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            long result = 0;
            int validCount = 0;
            long eqRes = 0;
            for(int i = 0; i < equations.Count; i++)
            {
                eqRes = findValue(equations[i], -1);
                if(eqRes == equations[i].testValue)
                {
                    validCount++;
                    result += equations[i].testValue;
                }
            }
            timer.Stop();
            Console.WriteLine("Final: Valid:[" + validCount + "] FOR Total of [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]" );
        }

        private long findValue(DataHolder equation, int indexModified) // Use recursion 
        {
            long res = 0;
            if(equation.getSetOperatorCount() < equation.operatorList.Count && indexModified < equation.operatorList.Count) // if operator list hasnt been populated yet
            {
                int indexChange = equation.getLatestEmptyOpIndex();
                indexModified = 0;
                equation.modOperator(charChoices[0], indexChange);
                res = findValue(equation, 0);
            }
            else // if populated
            {
                if (indexModified < equation.operatorList.Count)
                {
                    int operatorCount = equation.getSetOperatorCount();
                    if (equation.evaluate() != equation.testValue) // if result is not the testvalue, change a operator
                    {
                        DataHolder eq1 = new DataHolder(equation);
                        DataHolder eq2 = new DataHolder(equation);
                        DataHolder eq3 = new DataHolder(equation); // PART 2
                        eq1.modOperator(charChoices[0], indexModified);
                        eq2.modOperator(charChoices[1], indexModified);
                        eq3.modOperator(charChoices[2], indexModified); // PART 2
                        indexModified++;
                        if (findValue(eq1, indexModified) == equation.testValue)
                        {
                            Console.WriteLine("FOUND AT:" + eq1.toString());
                            res = equation.testValue;
                        }
                        else if (findValue(eq2, indexModified) == equation.testValue)
                        {
                            Console.WriteLine("FOUND AT:" + eq2.toString());
                            res = equation.testValue;
                        }
                        else if (findValue(eq3, indexModified) == equation.testValue)                   //
                        {                                                                               // PART 2
                            Console.WriteLine("FOUND AT:" + eq3.toString());                            //
                            res = equation.testValue;                                                   //
                        }                                                                               //
                    }
                    else
                    {
                        res = equation.evaluate();
                    }
                }
                else
                {
                    res = equation.evaluate();
                }
            }
            return res;
        }

        public void solve2()
        {

        }

        internal class DataHolder
        {
            public long testValue { get; set; }
            List<long> values { get; set; }
            public List<char> operatorList { get; set; }
            public int lastModdedOpIndex;
            private readonly char[] charChoices = { '+', '*', '|' }; // ADDED '|' for Part 2

            public DataHolder(long inVal)
            {
                testValue = inVal;
                values = new List<long>();
                operatorList = new List<char>();
                lastModdedOpIndex = -1;
            }

            public DataHolder(DataHolder copy)
            {
                testValue = copy.testValue;
                values = new List<long>();
                operatorList = new List<char>();
                foreach(long item in copy.values)
                {
                    this.values.Add(item);
                }
                foreach(char item in copy.operatorList)
                {
                    this.operatorList.Add(item);
                }
                lastModdedOpIndex = -1;
            }

            public long evaluate()
            {
                long res = values[0];
                for(int i = 0; i < operatorList.Count; i++)
                {
                    if (operatorList[i] == '+')
                    {
                        res = res + values[i + 1];
                    }
                    else if (operatorList[i] == '*')
                    {
                        res = res * values[i + 1];
                    }
                    else if (operatorList[i] == '|')                                    // 
                    {                                                                   // PART 2
                        res = long.Parse(res.ToString() + values[i + 1].ToString());    //
                    }                                                                   //
                }
                return res;
            }

            public void addValue(long val)
            {
                values.Add(val);
                if(values.Count > 1)
                {
                    addOperator('~'); // Adds in a default operator value
                }
            }

            private bool addOperator(char op)
            {
                bool val = true;
                if(operatorList.Count < values.Count)
                {
                    operatorList.Add(op);
                }
                else
                {
                    Console.WriteLine("WARN:Operator Limit Exceeded:[" + operatorList.Count + " against " + values.Count + "]");
                    val = false;
                }
                return val;
            }

            public void modOperator(char op, int index)
            {
                if (charChoices.Contains(op))
                {
                    operatorList[index] = op;
                    lastModdedOpIndex = index;
                }
                else
                {
                    Console.WriteLine("Invalid Operator Insert");
                }
            }

            public int getSetOperatorCount()
            {
                int count = 0;
                for(int i = 0; i < operatorList.Count; i++)
                {
                    if (charChoices.Contains(operatorList[i]))
                    {
                        count++;
                    }
                }
                return count;
            }

            public int getLatestEmptyOpIndex()
            {
                int index = 0;
                for(int i = 0; i < operatorList.Count; i++)
                {
                    if (charChoices.Contains(operatorList[i]))
                    {
                        index++;
                    }
                    else
                    {
                        break;
                    }
                }
                return index;
            }

            public string toString()
            {
                string val = "TestValue:[" + testValue + "]|CurrentConfig:";

                for(int i = 0; i < values.Count; i++)
                {
                    if(i < operatorList.Count)
                    {
                        val += values[i] + " " + operatorList[i] + " ";
                    }
                    else
                    {
                        val += values[i];
                    }
                }
                return val;
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
                while (line != null)
                {
                    count++;
                    //VVV Do file reading operations here VVV
                    string[] split = line.Split(':');
                    if(split.Length > 0)
                    {
                        DataHolder temp = new DataHolder(long.Parse(split[0]));
                        split[1] = split[1].Trim();
                        string[] split2 = split[1].Split(' ');
                        foreach(string item in split2)
                        {
                            temp.addValue(long.Parse(item.Trim()));
                        }
                        Console.WriteLine(temp.toString());
                        equations.Add(temp);
                    }
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day 7 Resources Initialized! ");
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
