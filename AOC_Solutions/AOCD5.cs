using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD5
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD5_Example.txt";

        private List<List<int>> ruleset = new List<List<int>>();
        private List<List<int>> pages = new List<List<int>>();

        // 50|60 = 50 MUST come before 60

        //For part 2 
        private List<List<int>> violated = new List<List<int>>(); // [0] = violated ruleset index | [1] = violating page list index
        List<List<int>> fixedListRef = new List<List<int>>();

        //For part 2 rework
        private List<Violated> listOfViolations = new List<Violated>();

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
                Violated violate = new Violated(pages[i]);
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
                                violate.addViolatedRuleSet(ruleset[k]);
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
                listOfViolations.Add(violate);
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

        private int numFind(List<int> list, int match)
        {
            int val = -1;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == match)
                {
                    val = i;
                }
            }
            return val;
        }

        private bool doesListMatch(List<int> list, List<int> matchList)
        {
            bool val = true;
            if(list.Count == matchList.Count)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] != matchList[i])
                    {
                        val = false;
                    }
                }
            }
            else
            {
                val = false;
            }
            return val;
        }

        private bool doesContainListMatch(List<List<int>> list, List<int> matchList)
        {
            bool val = false;
            for(int i = 0; i < list.Count; i++)
            {
                if (doesListMatch(list[i], matchList))
                {
                    val = true;
                }
            }
            return val;
        }

        private void printViolated()
        {
            for(int i = 0; i < violated.Count; i++)
            {
                Console.WriteLine("VIOLATED:[" + ruleset[violated[i][0]][0] + "|" + ruleset[violated[i][0]][1] + "]");
            }
        }

        public void solve2()
        {
            List<List<int>> failedList = new List<List<int>>();
            List<List<int>> fixedList = new List<List<int>>();
            List<int> passed = validCheck();
            for(int i = 0; i < pages.Count; i++)
            {
                if(!numExists(passed, i))
                {
                    failedList.Add(pages[i]);
                }
            }
            printViolated();
            Console.WriteLine("FailedList:[" + failedList.Count + "]");

            for(int i = 0; i < failedList.Count; i++)
            {
                fixedList.Add(listRepair(failedList[i]));
            }
        }

        // ATTEMPT 2 - BRUH

        public void solve22()
        {
            List<int> passed = validCheck();
            List<List<int>> fixedPageNums = new List<List<int>>();
            for(int i = 0; i < listOfViolations.Count; i++)
            {
                if (listOfViolations[i].isViolating())
                {
                    listOfViolations[i].printData();
                    List<int> fixedPages = listRepair2(listOfViolations[i]);
                }
            }
        }

        // 
        private List<int> listRepair2(Violated violatedList)
        {
            List<List<int>> selectMover = new List<List<int>>(); // [0] = index of the chosen pageNum , [1] = furthest index to satisfy violated rulesets 
            for(int i = 0; i < violatedList.getViolatingPageNums().Count; i++)
            {
                // Blep
            }
            return null;
        }

        internal class Violated
        {
            List<int> violatingPageNums;
            List<List<int>> violatedRulesets = new List<List<int>>();

            public Violated(List<int> input)
            {
                violatingPageNums = new List<int>(input);
            }

            public void addViolatedRuleSet(List<int> rule)
            {
                violatedRulesets.Add(rule);
            }

            public List<int> getViolatingPageNums()
            {
                return violatingPageNums;
            }

            public List<List<int>> getViolatedRulesets()
            {
                return violatedRulesets;
            }

            public void printData()
            {
                Console.WriteLine("|" + getString() + "| FOR RULESETS:[" + getRuleSetStr() + "]");
            }

            private string getString()
            {
                string val = "";
                for (int i = 0; i < violatingPageNums.Count; i++)
                {
                    val += (violatingPageNums[i] + ",");
                }
                return val;
            }

            private string getRuleSetStr()
            {
                string val = "";
                for(int i = 0; i < violatedRulesets.Count; i++)
                {
                    val += (violatedRulesets[i][0] + "|" + violatedRulesets[i][1] + ",");
                }
                return val;
            }

            public bool isViolating()
            {
                bool val = false;
                if(violatedRulesets.Count > 0)
                {
                    val = true;
                }
                return val;
            }
        }

        private List<int> listRepair(List<int> list)
        {
            bool nowValid = false;
            for(int i = 0; i < violated.Count; i++)
            {
                if(doesContainListMatch(fixedListRef, list))
                {
                    break;
                }
                if(numExists(list, ruleset[violated[i][0]][0]) && numExists(list, ruleset[violated[i][0]][1]))
                {
                    int ind1 = numFind(list, ruleset[violated[i][0]][0]);
                    int ind2 = numFind(list, ruleset[violated[i][0]][1]);
                    Console.WriteLine("Violated RuleSet:[" + ruleset[violated[i][0]][0] + "|" + ruleset[violated[i][0]][1] + "] FOR [" + getString(list) + "] | IND1:[" + ind1 + "] IND2:[" + ind2 + "]");
                    // ind2 must be placed behind ind1 somewhere
                    Console.WriteLine("1st Attempt");
                    if (checkIfValid(list))
                    {
                        nowValid = true;
                    }
                    // 1st att => move ind2 behind ind1
                    if (!nowValid)
                    {
                        for (int j = ind1; j > 0; j--)
                        {
                            List<int> tempList = new List<int>(list);
                            tempList = swap(tempList, ind2, j);
                            Console.WriteLine("SWAPPING:[" + list[ind2] + "][" + list[j] + "]");
                            Console.WriteLine("Attempt1:[" + ruleset[violated[i][0]][0] + "|" + ruleset[violated[i][0]][1] + "] FOR [" + getString(tempList) + "]");
                            if (checkIfValid(tempList))
                            {
                                nowValid = true;
                                fixedListRef.Add(list);
                                list = tempList;
                                Console.WriteLine("List:[" + getString(list) + "] now valid :) | 1st Att");
                                break;
                            }
                        }
                    }
                    // 2nd att => move ind1 infront of ind2
                    if (!nowValid)
                    {
                        Console.WriteLine("2nd Attempt");
                        for (int j = ind2; j < list.Count; j++)
                        {
                            List<int> tempList = new List<int>(list);
                            tempList = swap(tempList, ind1, j);
                            Console.WriteLine("SWAPPING:[" + list[ind1] + "][" + list[j] + "]");
                            Console.WriteLine("Attempt2:[" + ruleset[violated[i][0]][0] + "|" + ruleset[violated[i][0]][1] + "] FOR [" + getString(tempList) + "]");
                            if (checkIfValid(tempList))
                            {
                                nowValid = true;
                                fixedListRef.Add(list);
                                list = tempList;
                                Console.WriteLine("List:[" + getString(list) + "] now valid :) | 2nd Att");
                                break;
                            }
                        }
                    }
                    // 3rd att => move ind2 behind ind1 in bubble sort manner
                    if (!nowValid)
                    {
                        Console.WriteLine("3rd Attempt");
                        for (int j = ind1; j > 0; j--)
                        {
                            List<int> tempList = new List<int>(list);
                            tempList = bubbleStyleMove(list, ind2, j);
                            Console.WriteLine("Attempt3:[" + ruleset[violated[i][0]][0] + "|" + ruleset[violated[i][0]][1] + "] FOR [" + getString(tempList) + "]");
                            if (checkIfValid(tempList))
                            {
                                nowValid = true;
                                fixedListRef.Add(list);
                                list = tempList;
                                Console.WriteLine("List:[" + getString(list) + "] now valid :) | 3st Att");
                                break;
                            }
                        }
                    }
                    // 4th att => move ind1 infront of ind2 in bubble sort manner
                    if (!nowValid)
                    {
                        Console.WriteLine("4th Attempt");
                        for (int j = ind2; j < list.Count; j++)
                        {
                            List<int> tempList = new List<int>(list);
                            tempList = bubbleStyleMove(list, ind1, j);
                            Console.WriteLine("Attempt4:[" + ruleset[violated[i][0]][0] + "|" + ruleset[violated[i][0]][1] + "] FOR [" + getString(tempList) + "]");
                            if (checkIfValid(tempList))
                            {
                                nowValid = true;
                                fixedListRef.Add(list);
                                list = tempList;
                                Console.WriteLine("List:[" + getString(list) + "] now valid :) | 4th Att");
                                break;
                            }
                        }
                    }
                    // 5th att => place ind2 anywhere behind ind1 and shuffle everything to make room
                    if (!nowValid)
                    {
                        Console.WriteLine("5th Attempt");
                        for (int j = ind1; j > 0; j--)
                        {
                            List<int> tempList = new List<int>(list);
                            tempList = shufflePlace(tempList, ind2, j);
                            Console.WriteLine("Attempt5:[" + ruleset[violated[i][0]][0] + "|" + ruleset[violated[i][0]][1] + "] FOR [" + getString(tempList) + "]");
                            if (checkIfValid(tempList))
                            {
                                nowValid = true;
                                fixedListRef.Add(list);
                                list = tempList;
                                Console.WriteLine("List:[" + getString(list) + "] now valid :) | 5th Att");
                                break;
                            }
                        }
                    }
                    //6th att => place ind1 anywhere infront of ind2 and shuffle everything to make room
                    if (!nowValid)
                    {
                        Console.WriteLine("6th Attempt");
                        for (int j = ind2; j < list.Count; j++)
                        {
                            List<int> tempList = new List<int>(list);
                            tempList = shufflePlace(tempList, ind1, j);
                            Console.WriteLine("Attempt6:[" + ruleset[violated[i][0]][0] + "|" + ruleset[violated[i][0]][1] + "] FOR [" + getString(tempList) + "]");
                            if (checkIfValid(tempList))
                            {
                                nowValid = true;
                                fixedListRef.Add(list);
                                list = tempList;
                                Console.WriteLine("List:[" + getString(list) + "] now valid :) | 6th Att");
                                break;
                            }
                        }
                    }
                    if (nowValid)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("LIST REPAIR : FAILED");
                    }
                }
            }
            return list;
        }

        private List<int> shufflePlace(List<int> list, int source, int dest)
        {
            int temp = list[source];
            if (source > dest) // if source index is to the right of the destination index // 0 D 0 0 S 0
            {
                for (int i = source; i > dest; i--)
                {
                    list[i] = list[i - 1];
                }
                list[dest] = temp;
            }
            else // if source index is to the left of the destination index // 0 S 0 0 D 0
            {
                for(int i = source; i < dest; i++)
                {
                    list[i] = list[i + 1];
                }
                list[dest] = temp;
            }
            return list;
        }

        private List<int> bubbleStyleMove(List<int> list, int ind1, int ind2)
        {
            Console.WriteLine("Bubble:IND1:[" + ind1 + "]|IND2:[" + ind2 + "]");
            int counter = 1;
            if(ind1 > ind2) // if ind1 is to the right of ind2 = ind1 go left
            {
                list = swap(list, ind1, list.Count-1);
                for(int i = ind1; i > ind2; i--)
                {
                    list = swap(list, ind1-counter, i);
                    counter++;
                    Console.WriteLine("Bubble:[" + getString(list) + "]");
                }
            }
            else if(ind2 > ind1) // if ind1 is to the left of ind2 == ind1 go right
            {
                list = swap(list, ind1, 0);
                for(int i = ind1; i < ind2; i++)
                {
                    list = swap(list, ind1+counter, i);
                    counter++;
                    Console.WriteLine("Bubble:[" + getString(list) + "]");
                }
            }
            return list;
        }

        private List<int> swap(List<int> list, int ind1, int ind2)
        {
            //Console.WriteLine("SWP:[" + list[ind1] + "][" + list[ind2] + "]");
            int value = list[ind1];
            list[ind1] = list[ind2];
            list[ind2] = value;
            return list;
        }

        private bool checkIfValid(List<int> list)
        {
            bool validFull = false;
            int passed = 0;
            for (int j = 0; j < list.Count; j++)
            {
                int pass = 0;
                bool valid = false;
                //Console.WriteLine("Checker:ValueTaken:[" + list[j] + "]");
                for (int k = 0; k < ruleset.Count; k++)
                {
                    if (ruleset[k][0] == list[j] && numExists(list, ruleset[k][1]))
                    {
                        for (int l = j; l < list.Count; l++)
                        {
                            if (ruleset[k][1] == list[l])
                            {
                                //Console.WriteLine("Checker:VALID:[" + ruleset[k][0] + "|" + ruleset[k][1] + "] FOR [" + getString(list) + "]");
                                pass++;
                                valid = true;
                                break;
                            }
                        }
                        if (!valid)
                        {
                            //Console.WriteLine("Checker:INVALID:[" + ruleset[k][0] + "|" + ruleset[k][1] + "] FOR [" + getString(list) + "]");
                        }
                        valid = false;
                    }
                    else
                    {
                        pass++;
                    }
                }
                if (pass == ruleset.Count)
                {
                    Console.WriteLine("Checker:PASS:[" + pass + "/" + ruleset.Count + "]");
                    passed++;
                }
                else
                {
                    Console.WriteLine("Checker:FAIL:[" + pass + "/" + ruleset.Count + "]");
                }
            }
            if (passed == list.Count)
            {
                Console.WriteLine("Checker:PageList: [" + getString(list) + "] | PASSED");
                validFull = true;
            }
            else
            {
                Console.WriteLine("Checker:PageList: [" + getString(list) + "] | FAILED");
            }
            return validFull;
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
