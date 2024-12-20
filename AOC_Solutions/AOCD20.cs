using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD20
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD20.txt";

        private List<List<char>> mainGrid = new List<List<char>>();
        private static List<(int, int)> possibleCheats = new List<(int, int)>();

        private static List<(int, int)> cheatStartPoint = new List<(int, int)>();
        private static List<List<(int, int)>> possible20Cheat = new List<List<(int, int)>>();

        public AOCD20()
        {
            init();
        }

        public void solve1()
        {
            int result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Code
            result = findShortestRoutes(mainGrid, 100, false);
            timer.Stop();
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2() // alone attempt
        {
            int result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Code
            result = findShortestRoutes2(mainGrid, 100, true);
            timer.Stop();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2b() // help from internet :P 
        {
            int result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Code
            result = floodFillRoutes(mainGrid);
            timer.Stop();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public int floodFillRoutes(List<List<char>> inputGrid)
        {
            char[,] grid;
            int sr = -1, sc = -1, er = -1, ec = -1;
            int rows = inputGrid.Count;
            int cols = inputGrid[0].Count;
            grid = new char[rows, cols];

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    char character = inputGrid[r][c];
                    if (character == 'S')
                    {
                        sr = r;
                        sc = c;
                        character = '.';
                    }
                    if (character == 'E')
                    {
                        er = r;
                        ec = c;
                        character = '.';
                    }
                    grid[r, c] = character;
                }
            }

            int[,] distanceFromStart = new int[rows, cols];
            int[,] distanceFromEnd = new int[rows, cols];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    distanceFromStart[r, c] = -1;
                    distanceFromEnd[r, c] = -1;
                }
            }
            distanceFromStart[sr, sc] = 0;
            distanceFromEnd[er, ec] = 0;

            // Basically a mini BFS from the start
            Queue<(int, int)> cellsFromStart = new Queue<(int, int)>();
            cellsFromStart.Enqueue((sr, sc));
            while (cellsFromStart.Count > 0)
            {
                var cell = cellsFromStart.Dequeue();
                int distance = distanceFromStart[cell.Item1, cell.Item2];
                foreach (var (nr, nc) in GetNeighbors(cell.Item1, cell.Item2, rows, cols))
                {
                    if (grid[nr, nc] == '.' && (distanceFromStart[nr, nc] == -1 || distanceFromStart[nr, nc] > distance + 1))
                    {
                        distanceFromStart[nr, nc] = distance + 1;
                        cellsFromStart.Enqueue((nr, nc));
                    }
                }
            }

            // Mini BFS from the end
            Queue<(int, int)> cellsFromEnd = new Queue<(int, int)>();
            cellsFromEnd.Enqueue((er, ec));
            while (cellsFromEnd.Count > 0)
            {
                var cell = cellsFromEnd.Dequeue();
                int distance = distanceFromEnd[cell.Item1, cell.Item2];
                foreach (var (nr, nc) in GetNeighbors(cell.Item1, cell.Item2, rows, cols))
                {
                    if (grid[nr, nc] == '.' && (distanceFromEnd[nr, nc] == -1 || distanceFromEnd[nr, nc] > distance + 1))
                    {
                        distanceFromEnd[nr, nc] = distance + 1;
                        cellsFromEnd.Enqueue((nr, nc));
                    }
                }
            }

            // Determining the number of cheat paths that would give picosecond savings of at least 100 picoseconds
            int total = 0;
            int nonCheatTime = distanceFromStart[er, ec];
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (grid[r, c] == '.')
                    {
                        for (int dr = -20; dr <= 20; dr++)
                        {
                            for (int dc = -20; dc <= 20; dc++)
                            {
                                if (Math.Abs(dr) + Math.Abs(dc) <= 20 && (dr != 0 || dc != 0))
                                {
                                    int nr = r + dr;
                                    int nc = c + dc;
                                    if (nr >= 0 && nr < rows && nc >= 0 && nc < cols && grid[nr, nc] == '.')
                                    {
                                        int savings = nonCheatTime - (distanceFromStart[r, c] + Math.Abs(dr) + Math.Abs(dc) + distanceFromEnd[nr, nc]);
                                        if (savings >= 100)
                                        {
                                            total++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return total;
        }

        private static IEnumerable<(int, int)> GetNeighbors(int r, int c, int rows, int cols) // for 2b
        {
            if (r > 0) yield return (r - 1, c); // Up
            if (r < rows - 1) yield return (r + 1, c); // Down
            if (c > 0) yield return (r, c - 1); // Left
            if (c < cols - 1) yield return (r, c + 1); // Right
        }

        private int findShortestRoutes(List<List<char>> grid, int picosSaved, bool exact)
        {
            int result = 0;
            (int, int) start = findChar(grid,'S');
            (int, int) end = findChar(grid,'E');
            List<List<int>> gridInt = convertToIntGrid(grid);

            int initRes = BFS_Detector(makeCopy(gridInt), start, end);
            Console.WriteLine("InitResult: " + initRes);
            foreach((int, int) n in possibleCheats)
            {
                Console.WriteLine("Possible Cheat At:[" + n.Item1 + "," + n.Item2 + "]");
            }
            Console.WriteLine("Finding Savings...");
            foreach((int, int) cheat in possibleCheats)
            {
                int newRes = BFS_Runner(makeCopy(gridInt, cheat), start, end);
                if (exact)
                {
                    if((initRes - newRes) == picosSaved)
                    {
                        result++;
                    }
                }
                else
                {
                    if ((initRes - newRes) >= picosSaved)
                    {
                        result++;
                    }
                }
            }
 
            return result;
        }

        private int findShortestRoutes2(List<List<char>> grid, int picosSaved, bool exact)
        {
            int result = 0;
            (int, int) start = findChar(grid, 'S');
            (int, int) end = findChar(grid, 'E');
            List<List<int>> gridInt = convertToIntGrid(grid);

            int initRes = BFS_Detector2(makeCopy(gridInt), start, end);
            Console.WriteLine("InitResult: " + initRes);
            Console.WriteLine("Finding Possible Cheat Routes...");
            foreach ((int, int) s in cheatStartPoint)
            {
                //Console.WriteLine("Cheat Start Point At:[" + s.Item1 + "," + s.Item2 + "]");
                List<(int, int)> cheatEndPoints = getCheatEndpoints(s, invertIntGrid(makeCopy(gridInt)));
                if(cheatEndPoints.Count > 0)
                {
                    foreach ((int, int) e in cheatEndPoints)
                    {
                        //Console.WriteLine("Cheat End Point At:[" + e.Item1 + "," + e.Item2 + "]");
                        List<(int, int)> route = getRoute(invertIntGrid(makeCopy(gridInt)), s, e);
                        if(route.Count > 0)
                        {
                            possible20Cheat.Add(route);
                        }
                    }
                }
            }
            Console.WriteLine("Size: " + possible20Cheat.Count + "| Finding Savings...");
            foreach(List<(int, int)> cheats in possible20Cheat)
            {
                //printGrid(makeCopy(gridInt, cheats));
                int newRes = BFS_Runner(makeCopy(gridInt, cheats), start, end);
                if (exact)
                {
                    if ((initRes - newRes) == picosSaved)
                    {
                        result++;
                    }
                }
                else
                {
                    if ((initRes - newRes) >= picosSaved)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        private int BFS_Runner(List<List<int>> grid, (int, int) start, (int, int) end)
        {
            int row = grid.Count;
            int col = grid[0].Count;

            int[,] directions = new int[,]
            {
                { -1, 0 }, // up
                { 1, 0 }, // down
                { 0, -1 }, // left
                { 0, 1 } // right
            };

            // Check if the starting or ending cell is blocked
            if (grid[start.Item1][start.Item2] == 1 || grid[end.Item1][end.Item2] == 1)
            {
                return -1;
            }

            Queue<(int x, int y, int distance)> queue = new Queue<(int, int, int)>();
            bool[,] visited = new bool[row, col];

            queue.Enqueue((start.Item1, start.Item2, 0));
            visited[start.Item1, start.Item2] = true;

            while (queue.Count > 0)
            {
                var (x, y, distance) = queue.Dequeue();

                if (x == end.Item1 && y == end.Item2)
                {
                    return distance;
                }

                for (int i = 0; i < directions.GetLength(0); i++)
                {
                    int newX = x + directions[i, 0];
                    int newY = y + directions[i, 1];

                    // Check if the new position is valid and not visited
                    if (isValid(newX, newY, row, col) && grid[newX][newY] == 0 && !visited[newX, newY])
                    {
                        queue.Enqueue((newX, newY, distance + 1));
                        visited[newX, newY] = true;
                    }
                }
            }

            return -1;
        }

        private int BFS_Detector(List<List<int>> grid, (int, int) start, (int, int) end)
        {
            int row = grid.Count;
            int col = grid[0].Count;

            int[,] directions = new int[,]
            {
                { -1, 0 }, // up
                { 1, 0 }, // down
                { 0, -1 }, // left
                { 0, 1 } // right
            };

            // Check if the starting or ending cell is blocked
            if (grid[start.Item1][start.Item2] == 1 || grid[end.Item1][end.Item2] == 1)
            {
                return -1;
            }

            Queue<(int x, int y, int distance)> queue = new Queue<(int, int, int)>();
            bool[,] visited = new bool[row, col];

            queue.Enqueue((start.Item1, start.Item2, 0));
            visited[start.Item1, start.Item2] = true;

            while (queue.Count > 0)
            {
                var (x, y, distance) = queue.Dequeue();

                Console.WriteLine("Travel:[" + x + "," + y + "]");
                detectAndFindPossibleCheats((x, y), grid);

                if (x == end.Item1 && y == end.Item2)
                {
                    return distance;
                }

                for (int i = 0; i < directions.GetLength(0); i++)
                {
                    int newX = x + directions[i, 0];
                    int newY = y + directions[i, 1];

                    // Check if the new position is valid and not visited
                    if (isValid(newX, newY, row, col) && grid[newX][newY] == 0 && !visited[newX, newY])
                    {
                        queue.Enqueue((newX, newY, distance + 1));
                        visited[newX, newY] = true;
                    }
                }
            }

            Console.WriteLine("Could Not traverse");
            return -1;
        }

        private int BFS_Detector2(List<List<int>> grid, (int, int) start, (int, int) end)
        {
            int row = grid.Count;
            int col = grid[0].Count;

            int[,] directions = new int[,]
            {
                { -1, 0 }, // up
                { 1, 0 }, // down
                { 0, -1 }, // left
                { 0, 1 } // right
            };

            // Check if the starting or ending cell is blocked
            if (grid[start.Item1][start.Item2] == 1 || grid[end.Item1][end.Item2] == 1)
            {
                return -1;
            }

            Queue<(int x, int y, int distance)> queue = new Queue<(int, int, int)>();
            bool[,] visited = new bool[row, col];

            queue.Enqueue((start.Item1, start.Item2, 0));
            visited[start.Item1, start.Item2] = true;

            while (queue.Count > 0)
            {
                var (x, y, distance) = queue.Dequeue();

                //Console.WriteLine("Travel:[" + x + "," + y + "]");

                if (x == end.Item1 && y == end.Item2)
                {
                    return distance;
                }

                detectAndFindPossibleCheatsExtended((x, y), grid);

                for (int i = 0; i < directions.GetLength(0); i++)
                {
                    int newX = x + directions[i, 0];
                    int newY = y + directions[i, 1];

                    // Check if the new position is valid and not visited
                    if (isValid(newX, newY, row, col) && grid[newX][newY] == 0 && !visited[newX, newY])
                    {
                        queue.Enqueue((newX, newY, distance + 1));
                        visited[newX, newY] = true;
                    }
                }
            }

            Console.WriteLine("Could Not traverse");
            return -1;
        }

        private List<(int, int)> getCheatEndpoints((int, int) start, List<List<int>> invertedGrid)
        {
            int row = invertedGrid.Count;
            int col = invertedGrid[0].Count;
            List<(int, int)> endpoints = new List<(int, int)>();    

            int[,] directions = new int[,]
            {
                { -1, 0 }, // up
                { 1, 0 }, // down
                { 0, -1 }, // left
                { 0, 1 } // right
            };

            // Check if the starting or ending cell is blocked
            if (invertedGrid[start.Item1][start.Item2] == 1)
            {
                return endpoints;
            }

            Queue<(int x, int y, int distance)> queue = new Queue<(int, int, int)>();
            bool[,] visited = new bool[row, col];

            queue.Enqueue((start.Item1, start.Item2, 0));
            visited[start.Item1, start.Item2] = true;

            while (queue.Count > 0)
            {
                var (x, y, distance) = queue.Dequeue();

                if(distance <= 20 && distance > 0)
                {
                    if(outPointAvailable((x, y), invertedGrid, endpoints))
                    {
                        endpoints.Add((x,y));
                    }
                    if(distance == 20)
                    {
                        continue; // dont make anymore paths after 20 steps
                    }
                }

                for (int i = 0; i < directions.GetLength(0); i++)
                {
                    int newX = x + directions[i, 0];
                    int newY = y + directions[i, 1];

                    // Check if the new position is valid and not visited
                    if (isValid(newX, newY, row, col) && invertedGrid[newX][newY] == 0 && !visited[newX, newY])
                    {
                        queue.Enqueue((newX, newY, distance + 1));
                        visited[newX, newY] = true;
                    }
                }
            }

            return endpoints;
        } 

        private List<(int, int)> getRoute(List<List<int>> invertedGrid, (int, int) start, (int, int) end)
        {
            List<(int, int)> route = new List<(int, int)>();
            int row = invertedGrid.Count;
            int col = invertedGrid[0].Count;

            int[,] directions = new int[,]
            {
                { -1, 0 }, // up
                { 1, 0 }, // down
                { 0, -1 }, // left
                { 0, 1 } // right
            };

            // Check if the starting or ending cell is blocked
            if (invertedGrid[start.Item1][start.Item2] == 1 || invertedGrid[end.Item1][end.Item2] == 1)
            {
                return route;
            }

            Queue<(int x, int y, int distance)> queue = new Queue<(int, int, int)>();
            bool[,] visited = new bool[row, col];
            Dictionary<(int, int), (int, int)> parent = new Dictionary<(int, int), (int, int)>();

            queue.Enqueue((start.Item1, start.Item2, 0));
            visited[start.Item1, start.Item2] = true;

            while (queue.Count > 0)
            {
                var (x, y, distance) = queue.Dequeue();

                //Console.WriteLine("Travel:[" + x + "," + y + "]");

                if (x == end.Item1 && y == end.Item2)
                {
                    var current = (x, y);
                    while(current != start)
                    {
                        route.Add(current);
                        current = parent[current];
                    }
                    route.Add(start);
                    route.Reverse();
                    return route;
                }

                for (int i = 0; i < directions.GetLength(0); i++)
                {
                    int newX = x + directions[i, 0];
                    int newY = y + directions[i, 1];

                    // Check if the new position is valid and not visited
                    if (isValid(newX, newY, row, col) && invertedGrid[newX][newY] == 0 && !visited[newX, newY])
                    {
                        queue.Enqueue((newX, newY, distance + 1));
                        visited[newX, newY] = true;
                        parent[(newX, newY)] = (x, y); // recording the parent
                    }
                }
            }

            Console.WriteLine("Could Not traverse");
            return route;
        }

        private bool isValid(int row, int col, int rowRange, int colRange)
        {
            return (row >= 0) && (row < rowRange) && (col >= 0) && (col < colRange);
        }

        private void detectAndFindPossibleCheats((int, int) location, List<List<int>> numGrid)
        {
            if(isValid(location.Item1 - 2, location.Item2, numGrid.Count, numGrid[0].Count)) //up
            {
                if(!existsPossibleCheat((location.Item1 - 1, location.Item2)) && numGrid[location.Item1-1][location.Item2] == 1 && numGrid[location.Item1 - 2][location.Item2] == 0)
                {
                    possibleCheats.Add((location.Item1 - 1, location.Item2));
                }
            }
            if(isValid(location.Item1 + 2, location.Item2, numGrid.Count, numGrid[0].Count)) //down
            {
                if (!existsPossibleCheat((location.Item1 + 1, location.Item2)) && numGrid[location.Item1 + 1][location.Item2] == 1 && numGrid[location.Item1 + 2][location.Item2] == 0)
                {
                    possibleCheats.Add((location.Item1 + 1, location.Item2));
                }
            }
            if(isValid(location.Item1, location.Item2 - 2, numGrid.Count, numGrid[0].Count)) //left
            {
                if (!existsPossibleCheat((location.Item1, location.Item2 - 1)) && numGrid[location.Item1][location.Item2 - 1] == 1 && numGrid[location.Item1][location.Item2 - 2] == 0)
                {
                    possibleCheats.Add((location.Item1, location.Item2 - 1));
                }
            }
            if(isValid(location.Item1, location.Item2 + 2, numGrid.Count, numGrid[0].Count)) //right
            {
                if (!existsPossibleCheat((location.Item1, location.Item2 + 1)) && numGrid[location.Item1][location.Item2 + 1] == 1 && numGrid[location.Item1][location.Item2 + 2] == 0)
                {
                    possibleCheats.Add((location.Item1, location.Item2 + 1));
                }
            }
        }

        private void detectAndFindPossibleCheatsExtended((int, int) location, List<List<int>> numGrid) // max of 20 
        {
            if (isValid(location.Item1 - 1, location.Item2, numGrid.Count, numGrid[0].Count)) //up
            {
                if (!existsCheatStart((location.Item1 - 1, location.Item2)) && numGrid[location.Item1 - 1][location.Item2] == 1)
                {
                    cheatStartPoint.Add((location.Item1 - 1, location.Item2));
                }
            }
            if (isValid(location.Item1 + 1, location.Item2, numGrid.Count, numGrid[0].Count)) //down
            {
                if (!existsCheatStart((location.Item1 + 1, location.Item2)) && numGrid[location.Item1 + 1][location.Item2] == 1)
                {
                    cheatStartPoint.Add((location.Item1 + 1, location.Item2));
                }
            }
            if (isValid(location.Item1, location.Item2 - 1, numGrid.Count, numGrid[0].Count)) //left
            {
                if (!existsCheatStart((location.Item1, location.Item2 - 1)) && numGrid[location.Item1][location.Item2 - 1] == 1)
                {
                    cheatStartPoint.Add((location.Item1, location.Item2 - 1));
                }
            }
            if (isValid(location.Item1, location.Item2 + 1, numGrid.Count, numGrid[0].Count)) //right
            {
                if (!existsCheatStart((location.Item1, location.Item2 + 1)) && numGrid[location.Item1][location.Item2 + 1] == 1)
                {
                    cheatStartPoint.Add((location.Item1, location.Item2 + 1));
                }
            }
        }

        private bool outPointAvailable((int, int) location, List<List<int>> numGrid, List<(int, int)> outpoints) // numGrid here is an inverted grid
        {
            if (isValid(location.Item1 - 1, location.Item2, numGrid.Count, numGrid[0].Count) && numGrid[location.Item1 - 1][location.Item2] == 1) //up
            {
                if(!outpoints.Contains((location.Item1, location.Item2)))
                {
                    return true;
                }
            }
            if (isValid(location.Item1 + 1, location.Item2, numGrid.Count, numGrid[0].Count) && numGrid[location.Item1 + 1][location.Item2] == 1) //down
            {
                if(!outpoints.Contains((location.Item1, location.Item2)))
                {
                    return true;
                }
            }
            if (isValid(location.Item1, location.Item2 - 1, numGrid.Count, numGrid[0].Count) && numGrid[location.Item1][location.Item2 - 1] == 1) //left
            {
                if(!outpoints.Contains((location.Item1, location.Item2)))
                {
                    return true;
                }
            }
            if (isValid(location.Item1, location.Item2 + 1, numGrid.Count, numGrid[0].Count) && numGrid[location.Item1][location.Item2 + 1] == 1) //right
            {
                if(!outpoints.Contains((location.Item1, location.Item2)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool existsPossibleCheat((int, int) possibleCheat)
        {
            return possibleCheats.Contains(possibleCheat);
        }

        private bool existsCheatStart((int, int) possibleCheat)
        {
            return cheatStartPoint.Contains(possibleCheat);
        }

        private void printGrid(List<List<char>> grid)
        {
            Console.WriteLine();
            foreach (List<char> row in grid)
            {
                foreach (char c in row)
                {
                    Console.Write(c);
                }
                Console.WriteLine();
            }
        }

        private void printGrid(List<List<int>> grid)
        {
            Console.WriteLine();
            foreach (List<int> row in grid)
            {
                foreach (int c in row)
                {
                    Console.Write(c);
                }
                Console.WriteLine();
            }
        }

        private (int, int) findChar(List<List<char>> grid, char target)
        {
            for(int i = 0; i < grid.Count; i++)
            {
                for(int j = 0; j < grid[0].Count; j++)
                {
                    if (grid[i][j] == target)
                    {
                        return (i, j); //tuples are cool
                    }
                }
            }
            return (-1, -1);
        }

        private List<List<int>> convertToIntGrid(List<List<char>> grid)
        {
            List<List<int>> newGrid = new List<List<int>>();
            foreach(List<char> line in grid)
            {
                List<int> gridpiece = new List<int>();
                foreach(char c in line)
                {
                    if(c == '#')
                    {
                        gridpiece.Add(1);
                    }
                    else
                    {
                        gridpiece.Add(0);
                    }
                }
                newGrid.Add(gridpiece);
            }
            return newGrid;
        }

        private List<List<int>> invertIntGrid(List<List<int>> grid)
        {
            List<List<int>> newGrid = new List<List<int>>();
            foreach (List<int> line in grid)
            {
                List<int> gridpiece = new List<int>();
                foreach (int c in line)
                {
                    if (c == 0)
                    {
                        gridpiece.Add(1);
                    }
                    else
                    {
                        gridpiece.Add(0);
                    }
                }
                newGrid.Add(gridpiece);
            }
            return newGrid;
        }

        private List<List<int>> makeCopy(List<List<int>> input)
        {
            List<List<int>> newCopy = new List<List<int>>();
            foreach (List<int> line in input)
            {
                List<int> lineCopy = new List<int>();
                foreach (int val in line)
                {
                    lineCopy.Add(val);
                }
                newCopy.Add(lineCopy);
            }
            return newCopy;
        }

        private List<List<int>> makeCopy(List<List<int>> input, (int, int) removeBarrier)
        {
            List<List<int>> newCopy = new List<List<int>>();
            foreach (List<int> line in input)
            {
                List<int> lineCopy = new List<int>();
                foreach (int val in line)
                {
                    lineCopy.Add(val);
                }
                newCopy.Add(lineCopy);
            }
            newCopy[removeBarrier.Item1][removeBarrier.Item2] = 0;
            return newCopy;
        }

        private List<List<int>> makeCopy(List<List<int>> input, List<(int, int)> remBarrier)
        {
            List<List<int>> newCopy = new List<List<int>>();
            foreach (List<int> line in input)
            {
                List<int> lineCopy = new List<int>();
                foreach (int val in line)
                {
                    lineCopy.Add(val);
                }
                newCopy.Add(lineCopy);
            }
            
            foreach((int, int) rem in remBarrier)
            {
                newCopy[rem.Item1][rem.Item2] = 0;
            }

            return newCopy;
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
                    List<char> lineG = new List<char>();
                    foreach(char c in chars)
                    {
                        lineG.Add(c);
                    }
                    mainGrid.Add(lineG);
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                printGrid(mainGrid);
                sr.Close();
                Console.WriteLine("| AOC Day 20 Resources Initialized! ");
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
