using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD5
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD5.txt";

        private List<List<int>> ruleset = new List<List<int>>();
        private List<List<int>> pages = new List<List<int>>();

        // 50|60 = 50 MUST come before 60

        //For part 2 
        private List<List<int>> violated = new List<List<int>>(); // [0] = violated ruleset index | [1] = violating page list index

        public AOCD5()
        {
            init();
        }

        public void solve1()
        {
            int result = 0;
            result = midPointSum(validCheck());
            Console.WriteLine("Final 1: " + result);
        }

        private List<int> validCheck()
        {
            int passed= 0;
            List<int> passedPageLists = new List<int>();
            for(int i = 0; i < pages.Count; i++)
            {
                for(int j = 0; j < pages[i].Count; j++)
                {
                    int pass = 0;
                    bool valid = false;
                    //Console.WriteLine("Value Taken:[" + pages[i][j] + "]");
                    for(int k = 0; k < ruleset.Count; k++)
                    {
                        if (ruleset[k][0] == pages[i][j] && numExists(pages[i], ruleset[k][1]))
                        {
                            for(int l = j; l < pages[i].Count; l++)
                            {
                                if (ruleset[k][1] == pages[i][l])
                                {
                                    Console.WriteLine("VALID:[" + ruleset[k][0] + "|" + ruleset[k][1] + "] FOR [" + getString(pages[i]) + "]");
                                    pass++;
                                    valid = true;
                                    break;
                                }
                            }
                            if (!valid)
                            {
                                List<int> viol = new List<int>();
                                Console.WriteLine("INVALID:[" + ruleset[k][0] + "|" + ruleset[k][1] + "] FOR [" + getString(pages[i]) + "]");
                                viol.Add(k);
                                viol.Add(i);
                                violated.Add(viol);
                            }
                            valid = false;
                        }
                        else
                        {
                            //Console.WriteLine("DEFAULT VALID");
                            pass++;
                        }
                    }
                    if(pass == ruleset.Count)
                    {
                        Console.WriteLine("PASS:[" + pass + "/" + ruleset.Count + "]");
                        passed++;
                    }
                    else
                    {
                        Console.WriteLine("FAIL:[" + pass + "/" + ruleset.Count + "]");
                    }
                }
                if(passed == pages[i].Count)
                {
                    Console.WriteLine("Page List: [" + getString(pages[i]) + "] | PASSED");
                    passedPageLists.Add(i);
                    Console.WriteLine("ADDED INDEX:[" + i + "]");
                }
                else
                {
                    Console.WriteLine("Page List: [" + getString(pages[i]) + "] | FAILED");
                }
                passed = 0;
            }
            return passedPageLists;
        }

        private int midPointSum(List<int> passedList)
        {
            int res = 0;
            for(int i = 0; i < passedList.Count; i++)
            {
                res += pages[passedList[i]][pages[passedList[i]].Count/2];
            }
            return res;
        }

        private bool numExists(List<int> list, int match)
        {
            bool val = false;
            for(int i = 0; i < list.Count; i++)
            {
                if (list[i] == match)
                {
                    val = true;
                }
            }
            return val;
        }

        public void solve2()
        {
            List<List<int>> failedList = new List<List<int>>();
            List<int> passed = validCheck();
            for(int i = 0; i < pages.Count; i++)
            {
                for(int j = 0; j < passed.Count; j++)
                {
                    if(!numExists(passed, i))
                    {
                        failedList.Add(pages[i]);
                    }
                }
            }
        }

        private List<int> listRepair(List<int> list)
        {
            
        }

        private bool checkIfValid(List<int> list)
        {
            for (int j = 0; j < list.Count; j++)
            {
                int pass = 0;
                bool valid = false;
                Console.WriteLine("Checker:ValueTaken:[" + list[j] + "]");
                for (int k = 0; k < ruleset.Count; k++)
                {
                    if (ruleset[k][0] == list[j] && numExists(list, ruleset[k][1]))
                    {
                        for (int l = j; l < list.Count; l++)
                        {
                            if (ruleset[k][1] == list[l])
                            {
                                Console.WriteLine("Checker:VALID:[" + ruleset[k][0] + "|" + ruleset[k][1] + "] FOR [" + getString(list) + "]");
                                pass++;
                                valid = true;
                                break;
                            }
                        }
                        if (!valid)
                        {
                            Console.WriteLine("Checker:INVALID:[" + ruleset[k][0] + "|" + ruleset[k][1] + "] FOR [" + getString(list) + "]");
                        }
                        valid = false;
                    }
                    else
                    {
                        pass++;
                    }
                }
            }
        }

        public void printData()
        {
            for(int i = 0; i < ruleset.Count; i++)
            {
                Console.WriteLine(ruleset[i][0] + "|" + ruleset[i][1]);
            }
            for (int i = 0; i < pages.Count; i++)
            {
                for(int j = 0; j < pages[i].Count; j++)
                {
                    Console.Write(pages[i][j] + ",");
                }
                Console.WriteLine();
            }
        }

        private string getString(List<int> nums)
        {
            string val = "";
            for(int i = 0; i < nums.Count; i++)
            {
                val += (nums[i] + ",");
            }
            return val;
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
                    string[] split = line.Split('|');
                    if(split.Length == 2) // ruleset
                    {
                        List<int> rule = new List<int>();
                        rule.Add(int.Parse(split[0]));
                        rule.Add(int.Parse(split[1]));
                        ruleset.Add(rule);
                    }
                    else
                    {
                        string[] split2 = line.Split(',');
                        if(split2.Length > 1) // pages
                        {
                            List<int> temppage = new List<int>();
                            for(int i = 0; i < split2.Length; i++)
                            {
                                temppage.Add(int.Parse(split2[i]));
                            }
                            pages.Add(temppage);
                        }
                    }
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                printData();
                Console.WriteLine("| AOC Day 5 Resources Initialized! ");
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
