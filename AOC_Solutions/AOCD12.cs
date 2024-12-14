using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD12
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD12_Example.txt";

        private List<List<char>> areaGrid = new List<List<char>>();
        private List<Plot> plots = new List<Plot>();

        public AOCD12()
        {
            init();
        }

        public void solve1()
        {
            long result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            plotDiscovery();
            foreach(Plot plot in plots)
            {
                result += plot.getArea() * plot.getPerimeter();
            }
            timer.Stop();
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
            plots.Clear();
        }

        public void solve2()
        {
            long result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            plotDiscovery();
            foreach (Plot plot in plots)
            {
                result += plot.getArea() * plot.getSides();
            }
            timer.Stop();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
            plots.Clear();
        }

        private void plotDiscovery()
        {
            for(int i = 0; i < areaGrid.Count; i++)
            {
                for(int j = 0; j < areaGrid[i].Count; j++)
                {
                    if(!plantAccountedFor(i, j))
                    {
                        Plot newPlot = new Plot(areaGrid[i][j]);
                        plotDiscover(newPlot, i, j);
                        plots.Add(newPlot);
                    }
                }
            }
            Console.WriteLine("Plot Count:" + plots.Count);
            foreach(Plot plot in plots)
            {
                //plot.genSides();
                plot.genSides();
                Console.WriteLine(plot);
            }
        }

        private void plotDiscover(Plot plot, int i1, int i2)
        {
            int[] pos = { i1, i2 };
            plot.addNewPos(pos);
            List<List<char>> gridGrab = grid3Grabber(i1, i2);
            //printgrid(gridGrab);
            List<List<int>> nextPlots = possibleTrails(plot, gridGrab, plot.plantType, pos);
            if(nextPlots.Count > 0)
            {
                for (int i = 0; i < nextPlots.Count; i++)
                {
                    int[] temp = { i1 + nextPlots[i][0], i2 + nextPlots[i][1]};
                    if (!plot.plantPosExists(temp))
                    {
                        plotDiscover(plot, i1 + nextPlots[i][0], i2 + nextPlots[i][1]);
                    }
                }
            }
        }

        private bool plantAccountedFor(int i1, int i2)
        {
            int[] temp = { i1, i2 };
            foreach(Plot plot in plots)
            {
                if (plot.plantPosExists(temp))
                {
                    return true;
                }
            }
            return false;
        }

        private List<List<char>> grid3Grabber(int in1, int in2)
        {
            List<List<char>> grid = new List<List<char>>();
            for (int i = in1 - 1; i <= in1 + 1; i++)
            {
                if (i >= 0 && i < areaGrid.Count)
                {
                    List<char> line = new List<char>();
                    for (int j = in2 - 1; j <= in2 + 1; j++)
                    {
                        if (j >= 0 && j < areaGrid[i].Count)
                        {
                            line.Add(areaGrid[i][j]);
                        }
                        else if (j == areaGrid[i].Count-1)
                        {
                            line.Add('#');
                        }
                        else
                        {
                            line.Add('#');
                        }
                    }
                    grid.Add(line);
                }
                else if(i == areaGrid.Count-1)
                {
                    List<char> line = new List<char>();
                    for (int j = 0; j < 3; j++)
                    {
                        line.Add('#');
                    }
                    grid.Add(line);
                }
                else
                {
                    List<char> line = new List<char>();
                    for (int j = 0; j < 3; j++)
                    {
                        line.Add('#');
                    }
                    grid.Add(line);
                }
            }
            //printgrid(grid);
            return grid;
        }

        private List<List<int>> possibleTrails(Plot plot, List<List<char>> grid, char plantType, int[] currPos) // a 3x3 grid input, outputs a list of possible directions: left, right, up, down
        {
            List<List<int>> pos = new List<List<int>>();
            int record = 0;
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j] == plantType)
                    {
                        List<int> p = new List<int>();
                        if (i == 0 && j == 1) // up
                        {
                            p.Add(-1);
                            p.Add(0);
                            pos.Add(p);
                        }
                        else if (i == 2 && j == 1) // down
                        {
                            p.Add(1);
                            p.Add(0);
                            pos.Add(p);
                        }
                        else if (i == 1 && j == 0) //left
                        {
                            p.Add(0);
                            p.Add(-1);
                            pos.Add(p);
                        }
                        else if (i == 1 && j == 2) //right
                        {
                            p.Add(0);
                            p.Add(1);
                            pos.Add(p);
                        }
                    }
                    else
                    {
                        int[] temp = new int[2];
                        if (i == 0 && j == 1) // up
                        {
                            temp[0] = currPos[0] - 1;
                            temp[1] = currPos[1];
                            plot.addPerimeter(temp);
                        }
                        else if (i == 2 && j == 1) // down
                        {
                            temp[0] = currPos[0] + 1;
                            temp[1] = currPos[1];
                            plot.addPerimeter(temp);
                        }
                        else if (i == 1 && j == 0) //left
                        {
                            temp[0] = currPos[0];
                            temp[1] = currPos[1] - 1;
                            plot.addPerimeter(temp);
                        }
                        else if (i == 1 && j == 2) //right
                        {
                            temp[0] = currPos[0];
                            temp[1] = currPos[1] + 1;
                            plot.addPerimeter(temp);
                        }
                    }
                }
            }
            return pos;
        }

        private void printgrid(List<List<int>> grid)
        {
            Console.WriteLine("-----");
            foreach (List<int> line in grid)
            {
                foreach (int item in line)
                {
                    Console.Write(item);
                }
                Console.WriteLine();
            }
        }

        private void printgrid(List<List<char>> grid)
        {
            Console.WriteLine("-----");
            foreach (List<char> line in grid)
            {
                foreach (char item in line)
                {
                    Console.Write(item);
                }
                Console.WriteLine();
            }
        }

        internal class Plot
        {
            public List<int[]> plants {  get; set; }
            public List<int[]> perimeterPos { get; set; }
            public Dictionary<int, int> keyValuePairs { get; set; }
            //private List<Side> sides { get; set; }
            List<(int, List<int>)> periCoordinatesX { get; set; }
            List<(int, List<int>)> periCoordinatesY { get; set; }
            private int sides {  get; set; }
            public char plantType { get; set; }
            public int perimeter { get; set; }

            public Plot(char plantType)
            {
                this.plantType = plantType;
                plants = new List<int[]>();
                perimeterPos = new List<int[]>();
                keyValuePairs = new Dictionary<int, int>();
                periCoordinatesX = new List<(int, List<int>)>();
                periCoordinatesY = new List<(int, List<int>)>();
                sides = 0;
                perimeter = 0;
            }

            public void addNewPos(int[] newPos)
            {
                plants.Add(newPos);
            }

            public void addPerimeter(int[] periPos)
            {
                perimeterPos.Add(periPos);
                perimeter++;
            }

            public bool plantPosExists(int[] pos)
            {
                foreach(int[] p in plants)
                {
                    if (pos[0] == p[0] && pos[1] == p[1])
                    {
                        return true;
                    }
                }
                return false;
            }

            public void genSides()
            {
                foreach (int[] item in perimeterPos)
                {
                    updateRecord(periCoordinatesX, item[0], item[1]);
                }
                foreach (int[] item in perimeterPos)
                {
                    updateRecord(periCoordinatesY, item[1], item[0]);
                }
            }

            private void updateRecord(List<(int, List<int>)> record, int key, int value)
            {
                bool changes = false;
                for(int i = 0; i < record.Count; i++)
                {
                    if (record[i].Item1 == key)
                    {
                        record[i].Item2.Add(value);
                        changes = true;
                        break;
                    }
                }
                if (!changes)
                {
                    (int, List<int>) newRec = (key, new List<int>());
                    newRec.Item2.Add(value);
                    record.Add(newRec);
                }
            } 

            private Dictionary<int, int> updateRecord(Dictionary<int, int> record, int key, int value)
            {
                if (record.ContainsKey(key))
                {
                    record[key] += value;
                }
                else
                {
                    record.Add(key, value);
                }
                return record;
            }

            private void printPerimeters()
            {
                Console.WriteLine("Perimeters");
                foreach (var (int1, int2) in keyValuePairs)
                {
                    Console.WriteLine("[" + int1 + "," + int2 + "]");
                }
            }

            public int getArea()
            {
                return plants.Count;
            }

            public int getPerimeter()
            {
                return perimeter;
            }

            public int getSides()
            {
                return 0;
            }

            public override string ToString()
            {
                string val = "PLOT:[" + plantType + "]|COUNT:[" + plants.Count + "]";
                foreach(int[] p in plants)
                {
                    val += "\n[" + p[0] + "," + p[1] + "]";
                }
                val += "\n PERIMETERS:[" + perimeter + "]|SIDES:[" + 0 + "]";
                val += "\nX:";
                foreach((int, List<int>) temp in periCoordinatesX)
                {
                    val += "\n[" + temp.Item1 + "|";
                    foreach(int inte in temp.Item2)
                    {
                        val += inte + ",";
                    }
                    val += "]";
                }
                val += "\nY:";
                foreach ((int, List<int>) temp in periCoordinatesY)
                {
                    val += "\n[" + temp.Item1 + "|";
                    foreach (int inte in temp.Item2)
                    {
                        val += inte + ",";
                    }
                    val += "]";
                }
                return val;
            }
        }

        internal class Side
        {
            public char side {  get; set; }
            public int indexAtI { get; set; } // will be -1 if its along this axis
            public int indexAtJ { get; set; } // will be -1 if its along this axis
            public int sideLen { get; set; }
            public bool isParent { get; set; }

            public Side(char input, int index)
            {
                side = input;
                sideLen = 0;
                if(input == '-')
                {
                    indexAtJ = index;
                    indexAtI = -1;
                }
                else if(input == '|')
                {
                    indexAtI = index;
                    indexAtJ = -1;
                }
                else
                {
                    Console.WriteLine("INVLAUID");
                }
            }

            public bool addSide(Side input)
            {
                if(input.indexAtI == indexAtI && input.indexAtJ == indexAtJ)
                {
                    if (isParent)
                    {
                        sideLen++;
                    }
                    else if (input.isParent)
                    {
                        input.sideLen++;
                    }
                    return true;
                }
                return false;
            }

            public bool addSide(int i1, int i2)
            {
                if(indexAtI != -1)
                {
                    if(i1 == indexAtI)
                    {
                        sideLen++;
                        return true;
                    }
                }
                else if(indexAtJ != -1)
                {
                    if(i2 == indexAtJ)
                    {
                        sideLen++;
                        return true;
                    }
                }
                return false;
            }

            public bool isEqual(Side input)
            {
                if(indexAtI == input.indexAtI && indexAtJ == input.indexAtJ && side == input.side && isParent == input.isParent)
                {
                    return true;
                }
                return false;
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
                    List<char> chars1 = new List<char>();
                    foreach(char c in chars)
                    {
                        chars1.Add(c);
                    }
                    areaGrid.Add(chars1);
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day 12 Resources Initialized! ");
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
