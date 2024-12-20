using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD18
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD18.txt";

        private List<int[]> fallingBytes = new List<int[]>(); // X,Y format where X = j and Y = i 
        private List<List<int>> repGrid = new List<List<int>>();
        private List<int[]> pathTaken = new List<int[]>();
        private readonly int ROWSIZE = 71;
        private readonly int COLSIZE = 71;

        public AOCD18()
        {
            init();
            repGrid = initGrid(ROWSIZE, COLSIZE);
            foreach (int[] pairs in fallingBytes)
            {
                Console.WriteLine(pairs[0] + "," + pairs[1]);
            }
        }

        public void solve1()
        {
            Stopwatch timer = new Stopwatch();
            List<List<int>> gridCpy = makeCopy(repGrid);
            gridCpy = placeObs(gridCpy, fallingBytes, 1024);
            printGrid(gridCpy);

            timer.Start();
            //Pair source = new Pair(0, 0);
            //Pair destination = new Pair(ROWSIZE-1, COLSIZE-1);
            //AStar(gridCpy, source, destination);
            int result = BFS(gridCpy);
            timer.Stop();
            Console.WriteLine("\nFinal 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
            //printAndPlacePath(gridCpy);
        }

        public void solve2()
        {
            int result = 0;
            int resIndex = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            for(int i = 0; i < fallingBytes.Count; i++)
            {
                List<List<int>> gridCpy = makeCopy(repGrid);
                gridCpy = placeObs(gridCpy, fallingBytes, i+1);
                result = BFS(gridCpy);
                if(result == -1)
                {
                    resIndex = i;
                    break;
                }
            }
            timer.Stop();
            //printGrid(gridCpy);
            Console.WriteLine("\nFinal 2: [" + fallingBytes[resIndex][0] + "," + fallingBytes[resIndex][1] + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        private int BFS(List<List<int>> grid)
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
            if (grid[0][0] == 1 || grid[row - 1][col - 1] == 1)
            {
                return -1;
            }

            Queue<(int x, int y, int distance)> queue = new Queue<(int, int, int)>();
            bool[,] visited = new bool[row, col];

            queue.Enqueue((0, 0, 0));
            visited[0, 0] = true;

            while (queue.Count > 0)
            {
                var (x, y, distance) = queue.Dequeue();

                if (x == row - 1 && y == col - 1)
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

            // If we reach here, it means we cannot reach the destination
            return -1;
        }

        private void AStar(List<List<int>> grid, Pair source, Pair destination)
        {
            int row = grid.Count;
            int col = grid[0].Count;

            if(!isValid(source.x, source.y, row, col) || !isValid(destination.x, destination.y, row, col))
            {
                Console.Error.WriteLine("Either source or destination is invalid!");
                return;
            }

            if(!isUnblocked(grid, source.x, source.y) || !isUnblocked(grid, destination.x, destination.y))
            {
                Console.Error.WriteLine("Source or destination is blocked");
                return;
            }

            if(source.x == destination.x && source.y == destination.y)
            {
                Console.WriteLine("Source is the same as the destination");
            }

            bool[,] closedList = new bool[row, col]; // boolean grid to represent either visited or unvisited
            Cell[,] cellInfo = new Cell[row, col]; // holding cell information
            for(int i = 0; i < row; i++)
            {
                for(int j = 0; j < col; j++)
                {
                    cellInfo[i,j] = new Cell();
                    cellInfo[i,j].f = double.MaxValue;
                    cellInfo[i,j].g = double.MaxValue;
                    cellInfo[i,j].h = double.MaxValue;
                    cellInfo[i,j].parent_i = -1;
                    cellInfo[i,j].parent_j = -1;
                    closedList[i,j] = false;
                }
            }

            int x = source.x, y = source.y;
            cellInfo[x,y].f = 0.0;
            cellInfo[x,y].g = 0.0;
            cellInfo[x,y].h = 0.0;
            cellInfo[x,y].parent_i = x;
            cellInfo[x,y].parent_j = y;
            // sorted set containing sorted tuples based on f = g + h formula
            SortedSet<(double, Pair)> openList = new SortedSet<(double, Pair)>(Comparer<(double, Pair)>.Create((a, b) => a.Item1.CompareTo(b.Item1)));
            openList.Add((0.0, new Pair(x, y))); // tuple go brrr
            bool foundDest = false;

            int[,] directions = new int[,]
            {
                { -1, 0 }, // up
                { 1, 0 }, // down
                { 0, -1 }, // left
                { 0, 1 }, // right
                { -1, -1},
                { 1, 1 },
                { 1, -1},
                { -1, 1 },
            };

            while (openList.Count > 0)
            {
                (double f, Pair pair) p = openList.Min;
                openList.Remove(p);

                x = p.pair.x;
                y = p.pair.y;
                closedList[x, y] = true;

                for(int i = 0; i < directions.GetLength(0); i++)
                {
                    int newX = x + directions[i,0];
                    int newY = y + directions[i,1];
                    if(isValid(newX, newY, row, col))
                    {
                        if(isDestination(newX, newY, destination))
                        {
                            cellInfo[newX, newY].parent_i = x;
                            cellInfo[newX, newY].parent_j = y;
                            Console.WriteLine("Destination reached at: [" + newX + "," + newY + "]");
                            tracePath(cellInfo, destination);
                            foundDest = true;
                            return;
                        }

                        if (!closedList[newX,newY] && isUnblocked(grid, newX, newY))
                        {
                            double gNew = cellInfo[x, y].g + 1.0;
                            double hNew = calculateHValue(newX, newY, destination);
                            double fNew = gNew + hNew;

                            if (cellInfo[newX,newY].f == double.MaxValue || cellInfo[newX,newY].f > fNew)
                            {
                                openList.Add((fNew, new Pair(newX, newY)));
                                cellInfo[newX, newY].f = fNew;
                                cellInfo[newX, newY].g = gNew;
                                cellInfo[newX, newY].h = hNew;
                                cellInfo[newX, newY].parent_i = x;
                                cellInfo[newX, newY].parent_j = y;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid:[" + newX + "," + newY + "]");
                    }
                    
                }
            }
            if (!foundDest)
            {
                Console.WriteLine("Failed Searching at:[" + x + "," + y + "]");
            }
        }

        private void tracePath(Cell[,] cellDetails, Pair dest)
        {
            Console.WriteLine("\nThe Path is ");
            int ROW = cellDetails.GetLength(0);
            int COL = cellDetails.GetLength(1);

            int row = dest.x;
            int col = dest.y;

            Stack<Pair> Path = new Stack<Pair>();

            while (!(cellDetails[row, col].parent_i == row && cellDetails[row, col].parent_j == col))
            {
                Path.Push(new Pair(row, col));
                int temp_row = cellDetails[row, col].parent_i;
                int temp_col = cellDetails[row, col].parent_j;
                row = temp_row;
                col = temp_col;
            }

            Path.Push(new Pair(row, col));
            int counter = 0;
            pathTaken.Clear();
            while (Path.Count > 0)
            {
                counter++;
                Pair p = Path.Peek();
                Path.Pop();
                pathTaken.Add(new int[] {p.x, p.y});
                Console.Write(" -> ({0},{1}) ", p.x, p.y);
            }
            Console.WriteLine("\nSteps Taken: [" + (counter-1) + "]");
        }

        private static bool isValid(int row, int col, int rowRange, int colRange)
        {
            return (row >= 0) && (row < rowRange) && (col >= 0) && (col < colRange);
        }

        // Returns true if the cell is not blocked else false
        private static bool isUnblocked(List<List<int>> grid, int row, int col)
        {
            return grid[row][col] == 1;
        }

        // Checks whether destination cell has been reached or not
        private static bool isDestination(int row, int col, Pair destination)
        {
            return (row == destination.x && col == destination.y);
        }

        // Return using the distance formula
        private static double calculateHValue(int row, int col, Pair dest)
        {
            return Math.Sqrt(Math.Pow(row - dest.x, 2) + Math.Pow(col - dest.y, 2));
        }

        internal class Pair
        {
            public int x { get; set; }
            public int y { get; set; }
            public Pair( int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        internal class Cell
        {
            public int parent_i { get; set; }
            public int parent_j { get; set; }
            public double f { get; set; }
            public double g { get; set; }
            public double h { get; set; }
        }

        private List<List<int>> initGrid(int row, int col)
        {
            List<List<int>> newGrid = new List<List<int>>();
            for(int i = 0; i < row; i++)
            {
                List<int> newLine = new List<int>();
                for(int j = 0; j < col; j++)
                {
                    newLine.Add(0);
                }
                newGrid.Add(newLine);
            }
            return newGrid;
        }

        private List<List<int>> placeObs(List<List<int>> grid, List<int[]> bytes, int range)
        {
            for(int i = 0; i < range && i < bytes.Count; i++)
            {
                grid[bytes[i][1]][bytes[i][0]] = 1;
            }
            return grid;
        }

        private List<List<int>> makeCopy(List<List<int>> input)
        {
            List<List<int>> newCopy = new List<List<int>>();
            foreach(List<int> line in input)
            {
                List<int> lineCopy = new List<int>();
                foreach(int val in line)
                {
                    lineCopy.Add(val);
                }
                newCopy.Add(lineCopy);
            }
            return newCopy;
        }

        private void printGrid(List<List<int>> grid)
        {
            foreach(List<int> line in grid)
            {
                Console.WriteLine();
                foreach(int val in line)
                {
                    if(val == 1)
                    {
                        Console.Write('.');
                    }
                    else if(val == 2)
                    {
                        Console.Write('O');
                    }
                    else
                    {
                        Console.Write('#');
                    }
                }
            }
        }

        private void printAndPlacePath(List<List<int>> grid)
        {
            foreach (int[] path in pathTaken)
            {
                grid[path[0]][path[1]] = 2;
            }

            printGrid(grid);
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
                    string[] split = line.Split(',');
                    fallingBytes.Add(new int[] { int.Parse(split[0]), int.Parse(split[1])});
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day {DayNum} Resources Initialized! ");
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
