using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD6
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD6_Example.txt";

        private List<List<char>> grid = new List<List<char>>();
        private int[] guardStartPos = new int[2];
        private int[] guardCurrPos = new int[2];
        private readonly char[] guard = {'^','>','v','<'};

        // For Part 2
        private List<List<int>> positionLog = new List<List<int>>();

        public AOCD6()
        {
            init();
        }

        public void solve1()
        {
            bool isFinished = false;
            findGuard();
            Console.WriteLine("COMMENCE:StartPos:[" + guardStartPos[0] + "," + guardStartPos[1] + "]");
            while (!isFinished)
            {
                isFinished = moveGuard();
                //printGrid();
            }
            printGrid();
            Console.WriteLine("Final Marked: " + countMarked());
        }

        public void solve2()
        {

        }

        private void infiniteLoopFinder()
        {
            List<int> possibleObstacleSpots = new List<int>();
            
        }

        private bool moveGuard()
        {
            bool isGone = false;
            if (grid[guardCurrPos[0]][guardCurrPos[1]] == guard[0]) // If guard is facing up
            {
                if (guardCurrPos[0] != 0) // If guard is not facing emptiness | X value is not 0
                {
                    if (grid[guardCurrPos[0] - 1][guardCurrPos[1]] == '#') // If guard is facing a barrier block
                    {
                        grid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("right");
                    }
                    else // if facing empty space
                    {
                        guardCurrPos[0]--;
                        grid[guardCurrPos[0] + 1][guardCurrPos[1]] = 'X';

                        // Log position of Guard - LEFT OFF HERE
                        List<int> log = new List<int>();
                        log.Add(guardCurrPos[0]);
                        log.Add(guardCurrPos[1]);
                        positionLog.Add(log);

                        grid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("up");
                    }
                }
                else
                {
                    // End
                    grid[guardCurrPos[0]][guardCurrPos[1]] = 'X';
                    isGone = true;
                }
            }
            else if (grid[guardCurrPos[0]][guardCurrPos[1]] == guard[1]) //If guard is facing right
            {
                if (guardCurrPos[0] != (grid[0].Count-1)) // If guard is not facing emptiness | Y value is not COUNT
                {
                    if (grid[guardCurrPos[0]][guardCurrPos[1] + 1] == '#') // If guard is facing a barrier block
                    {
                        grid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("down");
                    }
                    else // if facing empty space
                    {
                        guardCurrPos[1]++;
                        grid[guardCurrPos[0]][guardCurrPos[1] - 1] = 'X';
                        grid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("right");
                    }
                }
                else
                {
                    // End
                    grid[guardCurrPos[0]][guardCurrPos[1]] = 'X';
                    isGone = true;
                }
            }
            else if (grid[guardCurrPos[0]][guardCurrPos[1]] == guard[2]) // If guard is facing down
            {
                if (guardCurrPos[0] != (grid.Count-1)) // If guard is not facing emptiness | X value is not COUNT
                {
                    if (grid[guardCurrPos[0] + 1][guardCurrPos[1]] == '#') // If guard is facing a barrier block
                    {
                        grid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("left");
                    }
                    else // if facing empty space
                    {
                        guardCurrPos[0]++;
                        grid[guardCurrPos[0] - 1][guardCurrPos[1]] = 'X';
                        grid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("down");
                    }
                }
                else
                {
                    // End
                    grid[guardCurrPos[0]][guardCurrPos[1]] = 'X';
                    isGone = true;
                }
            }
            else if(grid[guardCurrPos[0]][guardCurrPos[1]] == guard[3]) // If guard is facing left
            {
                if (guardCurrPos[1] != 0) // If guard is not facing emptiness | Y value is not 0
                {
                    if (grid[guardCurrPos[0]][guardCurrPos[1] - 1] == '#') // If guard is facing a barrier block
                    {
                        grid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("up");
                    }
                    else // if facing empty space
                    {
                        guardCurrPos[1]--;
                        grid[guardCurrPos[0]][guardCurrPos[1] + 1] = 'X';
                        grid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("left");
                    }
                }
                else
                {
                    // End
                    grid[guardCurrPos[0]][guardCurrPos[1]] = 'X';
                    isGone = true;
                }
            }
            else
            {
                Console.WriteLine("ERROR:INVALID GUARD DIR");
            }
            return isGone;
        }

        private char faceGuard(string dir)
        {
            char val = 'E';
            switch (dir)
            {
                case "up":
                    val = guard[0];
                    break;
                case "right":
                    val = guard[1];
                    break;
                case "down":
                    val= guard[2];
                    break;
                case "left":
                    val= guard[3];
                    break;
                default:
                    break;
            }
            return val;
        }

        private int countMarked()
        {
            int count = 0;
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j] == 'X')
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private void findGuard()
        {
            for(int i = 0; i < grid.Count; i++)
            {
                for(int j = 0; j < grid[i].Count; j++)
                {
                    if (guard.Contains(grid[i][j]))
                    {
                        guardStartPos[0] = i;
                        guardStartPos[1] = j;
                        guardCurrPos[0] = i;
                        guardCurrPos[1] = j;
                    }
                }
            }
        }

        private void printGrid()
        {
            Console.WriteLine("----------------------");
            for(int i = 0; i < grid.Count; i++)
            {
                for(int j = 0; j < grid[i].Count; j++)
                {
                    Console.Write(grid[i][j]);
                }
                Console.WriteLine();
            }
        }

        private void printGrid(List<List<char>> inGrid)
        {
            Console.WriteLine("----------------------");
            for (int i = 0; i < inGrid.Count; i++)
            {
                for (int j = 0; j < inGrid[i].Count; j++)
                {
                    Console.Write(inGrid[i][j]);
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
                    List<char> list = new List<char>();
                    for(int i = 0; i < chars.Length; i++)
                    {
                        list.Add(chars[i]);
                    }
                    grid.Add(list);
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                printGrid();
                Console.WriteLine("| AOC Day 6 Resources Initialized! ");
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
