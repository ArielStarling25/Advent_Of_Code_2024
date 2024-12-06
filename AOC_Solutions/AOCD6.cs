using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD6
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD6.txt";

        private List<List<char>> grid = new List<List<char>>();
        private int[] guardStartPos = new int[2];
        private int[] guardCurrPos = new int[2];
        private readonly char[] guard = {'^','>','v','<'};

        // For Part 2
        private List<List<char>> gridCopy;
        private List<List<int>> positionLog = new List<List<int>>();
        private int traversedBefore = 0;

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
            bool isFinished = false;
            int result = 0;
            findGuard();
            Console.WriteLine("COMMENCE:StartPos:[" + guardStartPos[0] + "," + guardStartPos[1] + "]");
            //gridCopy = new List<List<char>>(grid);
            gridCopy = copyGrid(grid);
            printGrid(gridCopy);
            while (!isFinished)
            {
                isFinished = moveGuard();
            }
            //printGrid(grid);
            countMarked();
            foreach (List<int> log in positionLog)
            {
                Console.WriteLine("PositionLog:[" + log[0] + "," + log[1] + "]");
            }
            Console.WriteLine("PosLog: " + positionLog.Count);
            result = infiniteLoopFinder(gridCopy);
            Console.WriteLine("Final: " + result);
        }

        private int infiniteLoopFinder(List<List<char>> gridCopy)
        {
            int res = 0;
            List<List<int>> possibleObstacleSpots = new List<List<int>>();

            for(int i = 0; i < positionLog.Count; i++)
            {
                bool isGone = false;
                int passThreshold = gridCopy.Count * gridCopy[0].Count;
                List<List<char>> gridCpy = copyGrid(gridCopy);
                gridCpy[positionLog[i][0]][positionLog[i][1]] = 'O'; // Place an obstacle along the original path
                guardCurrPos[0] = guardStartPos[0];
                guardCurrPos[1] = guardStartPos[1];
                //printGrid(gridCpy);
                Console.WriteLine("------------------ Iter:[" + i + "]----------------------");
                while (!isGone && passThreshold > traversedBefore)
                {
                    //Console.WriteLine("GuardPos:[" + guardCurrPos[0] + "," + guardCurrPos[1] + "]");
                    //Console.WriteLine("Traverse:[" + traversedBefore + "/" + passThreshold + "]");
                    gridCpy = traverseGuard(gridCpy, out isGone);
                    //printGrid(gridCpy);
                }
                if(traversedBefore >= passThreshold)
                {
                    Console.WriteLine("ADDEDTOPossibleObs:[" + positionLog[i][0] + "," + positionLog[i][1] + "]");
                    //printGrid(gridCpy);
                    possibleObstacleSpots.Add(positionLog[i]);
                }
                else
                {
                    //printGrid(gridCpy);
                    Console.WriteLine("FAILED TO TRAP GUARD For Obs at:[" + positionLog[i][0] + "," + positionLog[i][1] + "]");
                }
                traversedBefore = 0;
            }
            res = possibleObstacleSpots.Count;
            return res;
        }

        private List<List<char>> traverseGuard(List<List<char>> localGrid, out bool isGone)
        {
            isGone = false;
            if (localGrid[guardCurrPos[0]][guardCurrPos[1]] == guard[0]) // If guard is facing up
            {
                if (guardCurrPos[0] != 0) // If guard is not facing emptiness | X value is not 0
                {
                    if (localGrid[guardCurrPos[0] - 1][guardCurrPos[1]] == '#' || localGrid[guardCurrPos[0] - 1][guardCurrPos[1]] == 'O') // If guard is facing a barrier block
                    {
                        localGrid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("right");
                    }
                    else if(localGrid[guardCurrPos[0] - 1][guardCurrPos[1]] == 'X') // if encountering a previously traversed spot
                    {
                        guardCurrPos[0]--;
                        localGrid[guardCurrPos[0] + 1][guardCurrPos[1]] = 'X';
                        localGrid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("up");
                        traversedBefore++;
                    }
                    else // if facing empty space
                    {
                        guardCurrPos[0]--;
                        localGrid[guardCurrPos[0] + 1][guardCurrPos[1]] = 'X';
                        localGrid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("up");
                        traversedBefore = 0;
                    }
                }
                else
                {
                    // End
                    localGrid[guardCurrPos[0]][guardCurrPos[1]] = 'X';
                    isGone = true;
                }
            }
            else if (localGrid[guardCurrPos[0]][guardCurrPos[1]] == guard[1]) //If guard is facing right
            {
                if (guardCurrPos[1] != (localGrid[0].Count - 1)) // If guard is not facing emptiness | Y value is not COUNT
                {
                    if (localGrid[guardCurrPos[0]][guardCurrPos[1] + 1] == '#' || localGrid[guardCurrPos[0]][guardCurrPos[1] + 1] == 'O') // If guard is facing a barrier block
                    {
                        localGrid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("down");
                    }
                    else if(localGrid[guardCurrPos[0]][guardCurrPos[1] + 1] == 'X')
                    {
                        guardCurrPos[1]++;
                        localGrid[guardCurrPos[0]][guardCurrPos[1] - 1] = 'X';
                        localGrid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("right");
                        traversedBefore++;
                    }
                    else // if facing empty space
                    {
                        guardCurrPos[1]++;
                        localGrid[guardCurrPos[0]][guardCurrPos[1] - 1] = 'X';
                        localGrid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("right");
                        traversedBefore = 0;
                    }
                }
                else
                {
                    // End
                    localGrid[guardCurrPos[0]][guardCurrPos[1]] = 'X';
                    isGone = true;
                }
            }
            else if (localGrid[guardCurrPos[0]][guardCurrPos[1]] == guard[2]) // If guard is facing down
            {
                if (guardCurrPos[0] != (localGrid.Count - 1)) // If guard is not facing emptiness | X value is not COUNT
                {
                    if (localGrid[guardCurrPos[0] + 1][guardCurrPos[1]] == '#' || localGrid[guardCurrPos[0] + 1][guardCurrPos[1]] == 'O') // If guard is facing a barrier block
                    {
                        localGrid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("left");
                    }
                    else if (localGrid[guardCurrPos[0] + 1][guardCurrPos[1]] == 'X')
                    {
                        guardCurrPos[0]++;
                        localGrid[guardCurrPos[0] - 1][guardCurrPos[1]] = 'X';
                        localGrid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("down");
                        traversedBefore++;
                    }
                    else // if facing empty space
                    {
                        guardCurrPos[0]++;
                        localGrid[guardCurrPos[0] - 1][guardCurrPos[1]] = 'X';
                        localGrid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("down");
                        traversedBefore = 0;
                    }
                }
                else
                {
                    // End
                    localGrid[guardCurrPos[0]][guardCurrPos[1]] = 'X';
                    isGone = true;
                }
            }
            else if (localGrid[guardCurrPos[0]][guardCurrPos[1]] == guard[3]) // If guard is facing left
            {
                if (guardCurrPos[1] != 0) // If guard is not facing emptiness | Y value is not 0
                {
                    if (localGrid[guardCurrPos[0]][guardCurrPos[1] - 1] == '#' || localGrid[guardCurrPos[0]][guardCurrPos[1] - 1] == 'O') // If guard is facing a barrier block
                    {
                        localGrid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("up");
                    }
                    else if(localGrid[guardCurrPos[0]][guardCurrPos[1] - 1] == 'X')
                    {
                        guardCurrPos[1]--;
                        localGrid[guardCurrPos[0]][guardCurrPos[1] + 1] = 'X';
                        localGrid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("left");
                        traversedBefore++;
                    }
                    else // if facing empty space
                    {
                        guardCurrPos[1]--;
                        localGrid[guardCurrPos[0]][guardCurrPos[1] + 1] = 'X';
                        localGrid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("left");
                        traversedBefore = 0;
                    }
                }
                else
                {
                    // End
                    localGrid[guardCurrPos[0]][guardCurrPos[1]] = 'X';
                    isGone = true;
                }
            }
            else
            {
                Console.WriteLine("ERROR:INVALID GUARD DIR:[" + localGrid[guardCurrPos[0]][guardCurrPos[1]] + "]");
            }
            return localGrid; 
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
                        logGuardPos(guardCurrPos[0], guardCurrPos[1]);
                    }
                    else // if facing empty space
                    {
                        logGuardPos(guardCurrPos[0], guardCurrPos[1]);
                        guardCurrPos[0]--;
                        grid[guardCurrPos[0] + 1][guardCurrPos[1]] = 'X';
                        grid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("up");

                        // Log position of Guard 
                        logGuardPos(guardCurrPos[0], guardCurrPos[1]);
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
                        logGuardPos(guardCurrPos[0], guardCurrPos[1]);
                    }
                    else // if facing empty space
                    {
                        logGuardPos(guardCurrPos[0], guardCurrPos[1]);
                        guardCurrPos[1]++;
                        grid[guardCurrPos[0]][guardCurrPos[1] - 1] = 'X';
                        grid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("right");

                        // Log position of Guard 
                        logGuardPos(guardCurrPos[0], guardCurrPos[1]);
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
                        logGuardPos(guardCurrPos[0], guardCurrPos[1]);
                    }
                    else // if facing empty space
                    {
                        logGuardPos(guardCurrPos[0], guardCurrPos[1]);
                        guardCurrPos[0]++;
                        grid[guardCurrPos[0] - 1][guardCurrPos[1]] = 'X';
                        grid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("down");

                        // Log position of Guard 
                        logGuardPos(guardCurrPos[0], guardCurrPos[1]);
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
                        logGuardPos(guardCurrPos[0], guardCurrPos[1]);
                    }
                    else // if facing empty space
                    {
                        logGuardPos(guardCurrPos[0], guardCurrPos[1]);
                        guardCurrPos[1]--;
                        grid[guardCurrPos[0]][guardCurrPos[1] + 1] = 'X';
                        grid[guardCurrPos[0]][guardCurrPos[1]] = faceGuard("left");

                        // Log position of Guard 
                        logGuardPos(guardCurrPos[0], guardCurrPos[1]);
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

        private List<List<char>> copyGrid(List<List<char>> oriList)
        {
            List<List<char>> newGrid = new List<List<char>>();
            for(int i = 0; i < oriList.Count; i++)
            {
                List<char> temp = new List<char>();
                for (int j = 0; j < oriList[i].Count; j++)
                {
                    char c = oriList[i][j];
                    temp.Add(c);
                }
                newGrid.Add(temp);
            }
            return newGrid;
        }

        private void logGuardPos(int ind1, int ind2)
        {
            // Log position of Guard 
            List<int> log = new List<int>();
            log.Add(ind1);
            log.Add(ind2);
            if (!logExists(positionLog, log) && ind1 != guardStartPos[0] && ind2 != guardStartPos[1])
            {
                positionLog.Add(log);
            }
        }

        private bool logExists(List<List<int>> parent, List<int> match)
        {
            bool val = false;
            for (int i = 0; i < parent.Count; i++){
                if (parent[i][0] == match[0] && parent[i][1] == match[1])
                {
                    val = true;
                    break;
                }
            }
            return val;
        }

        private int countMarked()
        {
            positionLog = new List<List<int>>();
            int count = 0;
            for (int i = 0; i < grid.Count; i++)
            {
                for (int j = 0; j < grid[i].Count; j++)
                {
                    if (grid[i][j] == 'X')
                    {
                        List<int> log = new List<int>();
                        log.Add(i);
                        log.Add(j);
                        if(!(i == guardStartPos[0] && j == guardStartPos[1]))
                        {
                            positionLog.Add(log);
                        }
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

        private int max(int i1, int i2)
        {
            return i1 > i2 ? i1 : i2;
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
