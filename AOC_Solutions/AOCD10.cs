using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD10
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD10.txt";

        List<List<int>> topographyGrid = new List<List<int>>();
        List<List<int>> trailheads = new List<List<int>>();
        List<List<Trail>> vTrails = new List<List<Trail>>();

        List<List<Trail>> rTrails = new List<List<Trail>>();

        public AOCD10()
        {
            init();
            findTrailHeads();
            foreach (List<int> ints in trailheads)
            {
                Console.WriteLine("TrailHead: " + ints[0] + "," + ints[1]);
            }
        }

        public void solve1()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            foreach(List<int> heads in trailheads)
            {
                List<Trail> trails = new List<Trail>();
                Trail trail = new Trail(heads[0], heads[1], topographyGrid[heads[0]][heads[1]]);
                trails.AddRange(trailBlazer(trail, trails));
                trails = removeDupes(trails);
                vTrails.Add(trails);
            }
            timer.Stop();
            int result = 0;
            Console.Write("Score:[");
            foreach (List<Trail> trails in vTrails)
            {
                Console.Write(trails.Count + ",");
                result += trails.Count;
            }
            Console.WriteLine("]");
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");

        }

        public void solve2()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            foreach (List<int> heads in trailheads)
            {
                List<Trail> trails = new List<Trail>();
                Trail trail = new Trail(heads[0], heads[1], topographyGrid[heads[0]][heads[1]]);
                trails.AddRange(trailBlazer2(trail, trails));
                rTrails.Add(trails);
            }
            timer.Stop();
            int result = 0;
            Console.Write("Score:[");
            foreach (List<Trail> trails in rTrails)
            {
                Console.Write((trails.Count/2) + ",");
                result += (trails.Count/2);
            }
            Console.WriteLine("]");
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        private List<Trail> trailBlazer(Trail trail, List<Trail> validTrail) // recursive function
        {
            if(trail.currTopography == 9)
            {
                //validTrail.Add(trail);
                validTrail = addTrail(validTrail, trail);
                //Console.WriteLine("VALID:" + trail.ToString());
            }
            else
            {
                List<List<int>> gridGrab = grid3Grabber(trail.currTrail[0], trail.currTrail[1]);
                List<List<int>> nextSteps = possibleTrails(gridGrab, trail.currTopography);
                if(nextSteps.Count > 0)
                {
                    for(int i = 0; i < nextSteps.Count; i++)
                    {
                        Trail newTrail = new Trail(trail);
                        int newTopo = topographyGrid[newTrail.currTrail[0] + nextSteps[i][0]][newTrail.currTrail[1] + nextSteps[i][1]];
                        newTrail.movement(nextSteps[i][0], nextSteps[i][1], newTopo);
                        //Console.WriteLine(newTrail.ToString());
                        List<Trail> trailRecord = new List<Trail>();
                        //validTrail.AddRange(trailBlazer(newTrail, trailRecord));
                        validTrail = addTrails(validTrail, trailBlazer(newTrail, trailRecord));
                    }
                }
                else
                {
                    // Invalid Trail
                }
            }
            return validTrail;
        }

        private List<Trail> trailBlazer2(Trail trail, List<Trail> validTrail) // recursive function
        {
            if (trail.currTopography == 9)
            {
                //validTrail.Add(trail);
                validTrail = addTrail2(validTrail, trail);
                //Console.WriteLine("VALID:" + trail.ToString());
            }
            else
            {
                List<List<int>> gridGrab = grid3Grabber(trail.currTrail[0], trail.currTrail[1]);
                List<List<int>> nextSteps = possibleTrails(gridGrab, trail.currTopography);
                if (nextSteps.Count > 0)
                {
                    for (int i = 0; i < nextSteps.Count; i++)
                    {
                        Trail newTrail = new Trail(trail);
                        int newTopo = topographyGrid[newTrail.currTrail[0] + nextSteps[i][0]][newTrail.currTrail[1] + nextSteps[i][1]];
                        newTrail.movement(nextSteps[i][0], nextSteps[i][1], newTopo);
                        //Console.WriteLine(newTrail.ToString());
                        List<Trail> trailRecord = new List<Trail>();
                        //validTrail.AddRange(trailBlazer(newTrail, trailRecord));
                        validTrail = addTrails2(validTrail, trailBlazer2(newTrail, trailRecord));
                    }
                }
                else
                {
                    // Invalid Trail
                }
            }
            return validTrail;
        }

        private List<Trail> addTrails(List<Trail> trailsParent, List<Trail> trailsNew)
        {
            for(int i = 0; i < trailsNew.Count; i++)
            {
                trailsParent = addTrail(trailsParent, trailsNew[i]);
            }
            return trailsParent;
        }

        private List<Trail> addTrails2(List<Trail> trailsParent, List<Trail> trailsNew)
        {
            for (int i = 0; i < trailsNew.Count; i++)
            {
                trailsParent = addTrail2(trailsParent, trailsNew[i]);
            }
            return trailsParent;
        }

        private List<Trail> addTrail(List<Trail> trailsParent, Trail newTrail)
        {
            bool dupeExists = false;
            for(int i = 0; i < trailsParent.Count; i++)
            {
                if (trailsParent[i].currTrail[0] == newTrail.currTrail[0] && trailsParent[i].currTrail[1] == newTrail.currTrail[1])
                {
                    dupeExists = true;
                    //Console.WriteLine("Dupe Exists!");
                }
            }
            if (!dupeExists)
            {
                //Console.WriteLine("ADDING");
                trailsParent.Add(newTrail);
            }
            return trailsParent;
        }

        private List<Trail> addTrail2(List<Trail> trailsParent, Trail newTrail)
        {
            bool dupeExists = false;
            for (int i = 0; i < trailsParent.Count; i++)
            {
                int simcounter = 0;
                for(int j = 0; j < trailsParent[i].trailTaken.Count; j++)
                {
                    if (trailsParent[i].trailTaken[j][0] == newTrail.trailTaken[j][0] && trailsParent[i].trailTaken[j][1] == newTrail.trailTaken[j][1])
                    {
                        simcounter++;
                    }
                }
                if(simcounter == trailsParent[i].trailTaken.Count)
                {
                    dupeExists = true;
                    //Console.WriteLine("Dupe Exists!");
                }
            }

            if (!dupeExists)
            {
                //Console.WriteLine("ADDING");
                trailsParent.Add(newTrail);
            }
            return trailsParent;
        }

        private List<Trail> removeDupes(List<Trail> list)
        {
            for(int i = 0; i < list.Count; i++)
            {
                Trail temp = list[i];
                for(int j = 0; j < list.Count; j++)
                {
                    if(j != i)
                    {
                        if (temp.trailHead[0] == list[j].trailHead[0] && temp.trailHead[1] == list[j].trailHead[1])
                        {
                            if (temp.currTrail[0] == list[j].currTrail[0] && temp.currTrail[1] == list[j].currTrail[1])
                            {
                                list.RemoveAt(j);
                                if (j == 0)
                                {
                                    j = 0;
                                }
                                else
                                {
                                    j--;
                                }
                            }
                        }
                    }
                }
            }
            return list;
        }

        private List<List<int>> grid3Grabber(int in1, int in2)
        {
            List<List<int>> grid = new List<List<int>>();
            for(int i = in1-1; i <= in1+1 && i < topographyGrid.Count; i++)
            {
                if(i >= 0)
                {
                    List<int> line = new List<int>();
                    for (int j = in2 - 1; j <= in2 + 1 && j < topographyGrid[i].Count; j++)
                    {
                        if (j >= 0)
                        {
                            line.Add(topographyGrid[i][j]);
                        }
                        else
                        {
                            line.Add(-1);
                        }
                    }
                    grid.Add(line);
                }
                else
                {
                    List<int> line = new List<int>();
                    for (int j = 0; j < 3; j++)
                    {
                        line.Add(-1);
                    }
                    grid.Add(line);
                }
            }
            //printgrid(grid);
            return grid;
        }

        private List<List<int>> possibleTrails(List<List<int>> grid, int currTopo) // a 3x3 grid input, outputs a list of possible directions: left, right, up, down
        {
            List<List<int>> pos = new List<List<int>>();
            for(int i = 0; i < grid.Count; i++)
            {
                for(int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j] == (currTopo + 1))
                    {
                        List<int> p = new List<int>();
                        if(i == 0 && j == 1) // up
                        {
                            p.Add(-1);
                            p.Add(0);
                            pos.Add(p);
                        }
                        else if(i == 2 && j == 1) // down
                        {
                            p.Add(1); 
                            p.Add(0);
                            pos.Add(p);
                        }
                        else if(i == 1 && j == 0) //left
                        {
                            p.Add(0);
                            p.Add(-1);
                            pos.Add(p);
                        }
                        else if(i == 1 && j == 2) //right
                        {
                            p.Add(0);
                            p.Add(1);
                            pos.Add(p);
                        }
                    }
                }
            }
            return pos;
        }

        private void findTrailHeads()
        {
            for(int i = 0; i < topographyGrid.Count; i++)
            {
                for(int j = 0; j < topographyGrid[i].Count; j++)
                {
                    if (topographyGrid[i][j] == 0)
                    {
                        List<int> head = new List<int>();
                        head.Add(i);
                        head.Add(j);
                        trailheads.Add(head);
                    }
                }
            }
        }

        private List<Trail> makeCopy(List<Trail> input)
        {
            List<Trail> output = new List<Trail>();
            foreach(Trail trail in input)
            {
                output.Add(trail);
            }
            return output;
        }

        internal class Trail
        {
            public List<List<int>> trailTaken { get; set; }
            public int[] trailHead {  get; set; }
            public int[] currTrail { get; set; }
            public int currTopography {  get; set; }

            public Trail(int t1, int t2, int topo)
            {
                trailTaken = new List<List<int>>();
                trailHead = new int[2];
                currTrail = new int[2];
                trailHead[0] = t1;
                trailHead[1] = t2;
                currTrail[0] = t1;
                currTrail[1] = t2;
                currTopography = topo;
            }

            public Trail(Trail copy)
            {
                trailTaken = new List<List<int>>();
                foreach(List<int> item in copy.trailTaken)
                {
                    List<int> pos = new List<int>();
                    pos.Add(item[0]);
                    pos.Add(item[1]);
                    trailTaken.Add(pos);
                }
                trailHead = new int[2];
                currTrail = new int[2];
                currTrail[0] = copy.currTrail[0];
                currTrail[1] = copy.currTrail[1];
                trailHead[0] = copy.trailHead[0];
                trailHead[1] = copy.trailHead[1];
                currTopography = copy.currTopography;
            }

            public void movement(int in1, int in2, int topo)
            {
                List<int> pos = new List<int>();
                pos.Add(in1 + currTrail[0]);
                pos.Add(in2 + currTrail[1]);
                trailTaken.Add(pos);
                currTrail[0] = pos[0];
                currTrail[1] = pos[1];
                currTopography = topo;
            }

            public override string ToString()
            {
                string val = "TRAIL|currTopo:[" + currTopography + "]|trailHead:[" + trailHead[0] + "," + trailHead[1] + "]|currentPos:[" + currTrail[0] + "," + currTrail[1] + "]";
                foreach(List<int> pos in trailTaken)
                {
                    val += ("\n =>[" + pos[0] + "," + pos[1] + "]");
                }
                return val;
            }

        }

        private void printgrid(List<List<int>> grid)
        {
            Console.WriteLine("-----");
            foreach(List<int> line in grid)
            {
                foreach(int item in line)
                {
                    Console.Write(item);
                }
                Console.WriteLine();
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
                    char[] chars = line.ToCharArray();
                    List<int> gridpart = new List<int>();
                    foreach(char c in chars)
                    {
                        gridpart.Add(c-'0');
                    }
                    topographyGrid.Add(gridpart);
                    line = sr.ReadLine();
                }
                printgrid(topographyGrid);
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day 10 Resources Initialized! ");
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
