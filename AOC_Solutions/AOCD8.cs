using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD8
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD8.txt";

        private List<List<char>> mainGrid = new List<List<char>>();
        // Part 1 solution approach: mark/records antinodes in a List
        private List<int[]> antinodes = new List<int[]>(); // int[] is to be of only size 2, to hold coordinates
        private List<Tower> towers = new List<Tower>();

        public AOCD8()
        {
            init();
        }

        public void solve1()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            scanGridForTowers();
            for (int i = 0; i < towers.Count; i++)
            {
                createAntinodes(towers[i]);
            }
            timer.Stop();
            //placeAntiNodeAndPrintGrid();
            Console.WriteLine("Final 1: [" + antinodes.Count + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
            antinodes.Clear();
            towers.Clear();
        }

        private void createAntinodes(Tower tower)
        {
            for (int i = 0; i < towers.Count; i++)
            {
                if (!towers[i].Equals(tower))
                {
                    if (towers[i].identity == tower.identity)
                    {
                        int[] antiNodeLoc = new int[2];
                        int antiDX = (towers[i].x - tower.x);
                        int antiDY = (towers[i].y - tower.y);
                        antiNodeLoc[0] = towers[i].x + antiDX;
                        antiNodeLoc[1] = towers[i].y + antiDY;
                        if (antiNodeLoc[0] >= 0 && antiNodeLoc[0] < mainGrid[0].Count && antiNodeLoc[1] >= 0 && antiNodeLoc[1] < mainGrid.Count)
                        {
                            if (!doesAntinodeAlreadyExist(antiNodeLoc))
                            {
                                antinodes.Add(antiNodeLoc);
                                Console.WriteLine("ADDED Antinode At:[" + antiNodeLoc[0] + "," + antiNodeLoc[1] + "]");
                            }
                            else
                            {
                                Console.WriteLine("Duplicate at:[" + antiNodeLoc[0] + "," + antiNodeLoc[1] + "]");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Out Of bounds bruh:[" + antiNodeLoc[0] + "," + antiNodeLoc[1] + "]");
                        }
                    }
                }
            }
        }

        private void placeAntiNodeAndPrintGrid()
        {
            List<List<char>> copyGrid = makeCopy(mainGrid);
            for (int i = 0; i < antinodes.Count; i++)
            {
                copyGrid[antinodes[i][1]][antinodes[i][0]] = '#';
            }
            printGrid(copyGrid);
        }

        private void scanGridForTowers() // X+ is right, Y+ is down, and rmb first list is Y, not X so its like [Y][X]
        {
            for (int y = 0; y < mainGrid.Count; y++)
            {
                for (int x = 0; x < mainGrid[0].Count; x++)
                {
                    if (mainGrid[y][x] != '.')
                    {
                        Tower tower = new Tower(mainGrid[y][x], x, y);
                        Console.WriteLine("Tower Added:[" + tower.ToString() + "]");
                        towers.Add(tower);
                    }
                }
            }
        }

        private bool doesAntinodeAlreadyExist(int[] antinode)
        {
            bool val = false;
            for (int i = 0; i < antinodes.Count; i++)
            {
                if (antinodes[i][0] == antinode[0] && antinodes[i][1] == antinode[1])
                {
                    val = true;
                    break;
                }
            }
            return val;
        }

        public void solve2()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            scanGridForTowers();
            for (int i = 0; i < towers.Count; i++)
            {
                createAntinodes2(towers[i]);
            }
            timer.Stop();
            //placeAntiNodeAndPrintGrid();
            int result = calcTowersAndAntinodes();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
            antinodes.Clear();
            towers.Clear();
        }

        private void createAntinodes2(Tower tower)
        {
            for (int i = 0; i < towers.Count; i++)
            {
                if (!towers[i].Equals(tower))
                {
                    if (towers[i].identity == tower.identity)
                    {
                        int antiDX = (towers[i].x - tower.x);
                        int antiDY = (towers[i].y - tower.y);
                        int nodeLocX = towers[i].x + antiDX;
                        int nodeLocY = towers[i].y + antiDY;
                        do
                        {
                            int[] antiNodeLoc = new int[2];
                            antiNodeLoc[0] = nodeLocX;
                            antiNodeLoc[1] = nodeLocY;
                            if (antiNodeLoc[0] >= 0 && antiNodeLoc[0] < mainGrid[0].Count && antiNodeLoc[1] >= 0 && antiNodeLoc[1] < mainGrid.Count)
                            {
                                if (!doesAntinodeAlreadyExist(antiNodeLoc))
                                {
                                    antinodes.Add(antiNodeLoc);
                                    Console.WriteLine("ADDED Antinode At:[" + antiNodeLoc[0] + "," + antiNodeLoc[1] + "]");
                                }
                                else
                                {
                                    Console.WriteLine("Duplicate at:[" + antiNodeLoc[0] + "," + antiNodeLoc[1] + "]");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Out Of bounds bruh:[" + antiNodeLoc[0] + "," + antiNodeLoc[1] + "]");
                            }
                            nodeLocX = nodeLocX + antiDX;
                            nodeLocY = nodeLocY + antiDY;
                        } while (nodeLocX >= 0 && nodeLocX < mainGrid[0].Count && nodeLocY >= 0 && nodeLocY < mainGrid.Count);
                    }
                }
            }
        }

        private int calcTowersAndAntinodes()
        {
            int result = antinodes.Count + towers.Count;
            foreach (int[] antinode in antinodes)
            {
                foreach(Tower tower in towers)
                {
                    if (antinode[0] == tower.x && antinode[1] == tower.y)
                    {
                        result--;
                    }
                }
            }
            return result;
        }

        private List<List<char>> makeCopy(List<List<char>> grid)
        {
            List<List<char>> newGrid = new List<List<char>>();
            foreach(List<char> line in grid)
            {
                List<char> newLine = new List<char>();
                foreach(char c in line)
                {
                    newLine.Add(c);
                }
                newGrid.Add(newLine);
            }
            return newGrid;
        }

        private void printGrid()
        {
            foreach(List<char> list in mainGrid)
            {
                foreach(char item in list)
                {
                    Console.Write(item);
                }
                Console.WriteLine();
            }
        }

        private void printGrid(List<List<char>> grid)
        {
            foreach (List<char> list in grid)
            {
                foreach (char item in list)
                {
                    Console.Write(item);
                }
                Console.WriteLine();
            }
        }

        internal class Tower
        {
            public int x {  get; set; }
            public int y { get; set; }
            public char identity { get; set; }

            public Tower(char inChar, int x, int y)
            {
                identity = inChar;
                this.x = x;
                this.y = y;
            }

            public override string ToString()
            {
                string val = "ID:[" + identity + "] Location:[" + x + "," + y + "]";
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
                    char[] temp = line.ToCharArray();
                    List<char> chars = new List<char>();
                    foreach(char item in temp)
                    {
                        chars.Add(item);
                    }
                    mainGrid.Add(chars);
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                printGrid();
                sr.Close();
                Console.WriteLine("| AOC Day 8 Resources Initialized! ");
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
