using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD16
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD16.txt";

        private List<List<char>> mainGrid = new List<List<char>>();

        private List<int[]> bestRoute = new List<int[]>();
        private int bestRouteScore;

        public AOCD16()
        {
            init();
            printGrid(mainGrid);
        }

        // DFS Algorithm, keep track of actions done, 
        public void solve1()
        {
            List<int[]> route = new List<int[]>();
            int result = 0;
            bool comp = false;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Deer deer = new Deer(locateDeer());
            Console.WriteLine("Attempt...");
            bestRouteScore = 125000;
            //route = dfs_MazeRunner(route, deer.initPos, out comp);
            route = dfs_MazeRunnerOP(route, deer.initPos, 0, '>');
            if (!comp)
            {
                Console.Error.WriteLine("FAILED TO FIND ROUTE");
            }
            placeRouteAndPrint(bestRoute);
            result = deer.travel(bestRoute);
            timer.Stop();
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2()
        {

        }

        private List<int[]> dfs_MazeRunner(List<int[]> routeTraveled, int[] pos, out bool completed)
        {
            //routeTraveled.Add(pos);
            List<int[]> routeCopy = makeCopy(routeTraveled);
            if (mainGrid[pos[0]][pos[1]] == 'E')
            {
                completed = true;
                routeCopy.Add(pos);
                bestRoute = routeCopy;
                bestRouteScore = getScore(bestRoute);
                Console.WriteLine("New Best:" + bestRouteScore);
                return routeCopy;
            }
            routeCopy.Add(pos);
            if (getScore(routeCopy) < bestRouteScore)
            {
                //routeTraveled.Add(pos);
                //placeRouteAndPrintAndClear(routeTraveled);
                List<int[]> nextRoutes = grabNextAvailableRoutes(pos[0], pos[1]);
                List<List<int[]>> completedRoutes = new List<List<int[]>>();
                foreach (int[] next in nextRoutes)
                {
                    if (!alreadyTraversed(routeCopy, next)) // take the results of all branches and compare them, if more than 1 route has completed a route, compare their lengths (reject longer)
                    {
                        bool comp = false;
                        //List<int[]> cpy = makeCopy(routeTraveled);
                        List<int[]> temp = dfs_MazeRunner(routeCopy, next, out comp);
                        if (comp)
                        {
                            completedRoutes.Add(temp);
                        }
                    }
                }

                if (completedRoutes.Count > 1)
                {
                    completed = true;
                    return getShortest(completedRoutes);
                }
                else if (completedRoutes.Count == 1)
                {
                    completed = true;
                    return completedRoutes[0];
                }
                else
                {
                    completed = false;
                    routeCopy.RemoveAt(routeCopy.Count - 1);
                    return routeCopy;
                }
            }
            else
            {
                completed = false;
                routeCopy.RemoveAt(routeCopy.Count - 1);
                return routeCopy;
            }
        }

        private List<int[]> dfs_MazeRunnerOP(List<int[]> routeTraveled, int[] pos, int score, char dir)
        {
            List<int[]> routeCopy = makeCopy(routeTraveled);
            if (mainGrid[pos[0]][pos[1]] == 'E')
            {
                if(score < bestRouteScore)
                {
                    routeCopy.Add(pos);
                    bestRoute = routeCopy;
                    bestRouteScore = getScore(bestRoute);
                    Console.WriteLine("New Best:" + bestRouteScore);
                    return routeCopy;
                }
                else
                {
                    //Console.WriteLine("Best Exceeded");
                    routeCopy.RemoveAt(routeCopy.Count - 1);
                    return routeCopy;
                }
            }
            routeCopy.Add(pos);
            if (score < bestRouteScore)
            {
                List<int[]> nextRoutes = grabNextAvailableRoutes(pos[0], pos[1]);
                foreach (int[] next in nextRoutes)
                {
                    if (!alreadyTraversed(routeCopy, next)) // take the results of all branches and compare them, if more than 1 route has completed a route, compare their lengths (reject longer)
                    {
                        bool change = false;
                        char newDir = step(new int[] { pos[0], pos[1] }, new int[] { next[0], next[1] }, dir, out change);
                        if (change)
                        {
                            dfs_MazeRunnerOP(routeCopy, next, score+1000, newDir);
                        }
                        else
                        {
                            dfs_MazeRunnerOP(routeCopy, next, score+1, newDir);
                        }
                    }
                }
                routeCopy.RemoveAt(routeCopy.Count - 1);
                return routeCopy;
            }
            else
            {
                //Console.WriteLine("Best Exceeded");
                routeCopy.RemoveAt(routeCopy.Count - 1);
                return routeCopy;
            }
        }

        private List<int[]> getShortest(List<List<int[]>> compRoutes)
        {
            List<int> counts = new List<int>();
            foreach (List<int[]> comp in compRoutes)
            {
                counts.Add(getScore(comp));
            }
            int min = counts.Min();
            foreach (List<int[]> comp in compRoutes)
            {
                if(getScore(comp) == min)
                {
                    return comp;
                }
            }
            return compRoutes[0];
        }

        /*
        private List<int[]> getLeast(List<int[]> currBest, List<int[]> challenger)
        {
       
        }
        */

        private int getScore(List<int[]> route)
        {
            char face = '>';
            int score = 0;
            for(int i = 0; i < route.Count-1; i++)
            {
                bool change = false;
                face = step(route[i], route[i+1], face, out change);
                if (change)
                {
                    score += 1000;
                    i--;
                }
                else
                {
                    score++;
                }
            }
            return score;
        }

        private char step(int[] currPos, int[] nextPos, char face, out bool change)
        {
            change = false;
            if (currPos[0] - 1 == nextPos[0]) // if next pos is up
            {
                if (face != '^')
                {
                    change = true;
                }
                face = '^';
            }
            else if (currPos[0] + 1 == nextPos[0]) //if next pos is down
            {
                if (face != 'v')
                {
                    change = true;
                }
                face = 'v';
            }
            else if (currPos[1] - 1 == nextPos[1]) // if next pos is left
            {
                if (face != '<')
                {
                    change = true;
                }
                face = '<';
            }
            else if (currPos[1] + 1 == nextPos[1]) // if next pos is right
            {
                if (face != '>')
                {
                    change = true;
                }
                face = '>';
            }
            return face;
        }

        private bool alreadyTraversed(List<int[]> routes, int[] route)
        {
            foreach (int[] route2 in routes)
            {
                if (route[0] == route2[0] && route[1] == route2[1])
                {
                    return true;
                }
            }
            return false;
        }

        private List<int[]> grabNextAvailableRoutes(int in1, int in2)
        {
            List<int[]> availableRoutes = new List<int[]>();
            if(in1 > 0)
            {
                if (mainGrid[in1-1][in2] != '#') // if up is not a wall
                {
                    availableRoutes.Add(new int[] { in1-1, in2 });
                }
            }
            if(in1 < mainGrid.Count-1)
            {
                if (mainGrid[in1 + 1][in2] != '#') // if down is not a wall
                {
                    availableRoutes.Add(new int[] { in1 + 1, in2 });
                }
            }
            if(in2 > 0)
            {
                if (mainGrid[in1][in2 - 1] != '#') // if left is not a wall
                {
                    availableRoutes.Add(new int[] { in1, in2 - 1 });
                }   
            }
            if(in2 < mainGrid[0].Count-1)
            {
                if (mainGrid[in1][in2 + 1] != '#') // if right is not a wall
                {
                    availableRoutes.Add(new int[] { in1, in2 + 1 });
                }
            }
            return availableRoutes;
        }

        private void placeRouteAndPrint(List<int[]> route)
        {
            List<List<char>> grid = new List<List<char>>();
            for(int i = 0; i < mainGrid.Count; i++)
            {
                List<char> ln = new List<char>();
                for(int j = 0; j < mainGrid[i].Count; j++)
                {
                   ln.Add(mainGrid[i][j]);
                }
                grid.Add(ln);
            }

            foreach (int[] rout in route)
            {
                grid[rout[0]][rout[1]] = 'o';
            }

            printGrid(grid);
        }

        private void placeRouteAndPrintAndClear(List<int[]> route)
        {
            Thread.Sleep(3);
            Console.Clear();
            List<List<char>> grid = new List<List<char>>();
            for (int i = 0; i < mainGrid.Count; i++)
            {
                List<char> ln = new List<char>();
                for (int j = 0; j < mainGrid[i].Count; j++)
                {
                    ln.Add(mainGrid[i][j]);
                }
                grid.Add(ln);
            }

            foreach (int[] rout in route)
            {
                grid[rout[0]][rout[1]] = 'o';
            }

            printGrid(grid);
        }

        private int[]? locateDeer()
        {
            for(int i = 0; i < mainGrid.Count; i++)
            {
                for(int j = 0; j < mainGrid[i].Count; j++)
                {
                    if (mainGrid[i][j] == 'S')
                    {
                        return new int[] { i, j };
                    }
                }
            }
            return null;
        }

        private List<int[]> makeCopy(List<int[]> input)
        {
            List<int[]> newCopy = new List<int[]>();
            foreach (int[] inp in input)
            {
                newCopy.Add(new int[] { inp[0], inp[1] });
            }
            return newCopy;
        }

        private void printGrid(List<List<char>> grid)
        {
            Console.WriteLine("---");
            foreach (List<char> sect in grid)
            {
                foreach (char c in sect)
                {
                    Console.Write(c);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        internal class Deer
        {
            public int[] initPos { get; set; }
            public int[] currPos { get; set; }
            public char facing { get; set; }
            public List<char> dirs { get; set; }

            public Deer(int[] pos)
            {
                initPos = pos;
                currPos = new int[] { pos[0], pos[1] };
                facing = '>';
                dirs = new List<char>();
            }

            public int travel(List<int[]> route)
            {
                int score = 0;  
                for(int i = 0; i < route.Count-1; i++)
                {
                    if (step(route[i], route[i + 1]))
                    {
                        score += 1000;
                        i--;
                    }
                    else
                    {
                        score++;
                    }
                }
                return score;
            }

            private bool step(int[] currPos, int[] nextPos)
            {
                bool change = false;
                if (currPos[0]-1 == nextPos[0]) // if next pos is up
                {
                    if(facing != '^')
                    {
                        change = true;
                    }
                    facing = '^';
                }
                else if (currPos[0]+1 == nextPos[0]) //if next pos is down
                {
                    if (facing != 'v')
                    {
                        change = true;
                    }
                    facing = 'v';
                }
                else if (currPos[1]-1 == nextPos[1]) // if next pos is left
                {
                    if (facing != '<')
                    {
                        change = true;
                    }
                    facing = '<';
                }
                else if (currPos[1]+1 == nextPos[1]) // if next pos is right
                {
                    if (facing != '>')
                    {
                        change = true;
                    }
                    facing = '>';
                }
                char cpy = facing;
                dirs.Add(cpy);
                return change;
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
                    List<char> ln = line.ToCharArray().ToList();
                    mainGrid.Add(ln);
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day 16 Resources Initialized! ");
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
