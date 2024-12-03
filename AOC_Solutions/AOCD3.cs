using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD3
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD3.txt";

        List<string> corruptedData = new List<string>();
        readonly char[] validSeq = {'m','u','l','(',')'};
        readonly string validStr = "mul(";

        public AOCD3()
        { 
            init();
        }

        public void solve1()
        {
            int final = 0;
            for(int i = 0; i < corruptedData.Count; i++)
            {
                char[] chars = corruptedData[i].ToCharArray(); 
                final += dataParse(chars);
            }
            Console.WriteLine("Final: " + final);
        }

        public int dataParse(char[] chars)
        {
            int res = 0;
            int found = 0, yoink = 0;
            int trueYoink = 0;
            bool valid = true;
            string yoinked = "";
            for(int i = 0; i < (chars.Length-4); i++)
            {
                yoinked = "";
                valid = false;
                string snapshot = "";
                for(int j = 0; j < 4; j++)
                {
                    snapshot += chars[i+j];
                }
                Console.WriteLine(snapshot);
                if(snapshot == validStr)
                {
                    valid = true;
                    Console.WriteLine("FOUND");
                    found++;
                }

                if (valid)
                {
                    bool notYoinked = true;
                    bool falsePositive = false;
                    bool splitEnc = false;
                    int indC = i+4;
                    int sizeExceed = indC + 8; //largest possible size
                    while (indC < chars.Length && notYoinked && indC < sizeExceed && !falsePositive)
                    {
                        if (chars[indC] == validSeq[4])
                        {
                            notYoinked = false;
                        }
                        else if (chars[indC] == ',')
                        {
                            if (splitEnc)
                            {
                                falsePositive = true;
                            }
                            else
                            {
                                splitEnc = true;
                                yoinked += chars[indC];
                            }
                        }
                        else
                        {
                            yoinked += chars[indC];
                        }
                        Console.WriteLine("PROCESS:Yoinked:[" + yoinked + "]");
                        indC++;
                    }

                    if (!notYoinked && yoinked.Length > 0)
                    {
                        Console.WriteLine("Yoinked:[" + yoinked + "]");
                        yoink++;
                        int output1, output2;
                        string[] split = yoinked.Split(',');
                        if (int.TryParse(split[0], out output1) && int.TryParse(split[1], out output2))
                        {
                            trueYoink++;
                            res += (output1 * output2);
                        }
                    }
                }
            }
            Console.WriteLine("STAT:Found=[" + found + "]|Yoinked=[" + trueYoink + "]|Result=[" + res + "]");
            return res;
        }

        public void solve2()
        {
            
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
                    corruptedData.Add(line);
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day 3 Resources Initialized! ");
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
