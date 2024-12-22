using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD21
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD21.txt";
        // ME> robot keypad >ROBOT(-40C)> robot keypad >ROBOT(Radiation)> robot keypad >ROBOT(Depressurised)> door keypad
        private List<List<char>> codes = new List<List<char>>();

        public AOCD21()
        {
            init();
        }

        public void solve1()
        {
            long result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Code
            
            foreach(List<char> code in codes)
            {
                result += typeCode(code);
            }
            
            //result = typeCode(codes[4]);
            timer.Stop();
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2()
        {
            int result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            //Code

            timer.Stop();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        private long typeCode(List<char> code)
        {
            Robot final = new Robot(true);
            Robot radRobot = new Robot(false);
            Robot coldRobot = new Robot(false);

            coldRobot.setRobot(radRobot);
            radRobot.setRobot(final);

            List<char> seq = coldRobot.type(code);
            printList(seq);
            long seqLen = seq.Count();
            long numVal = retriveNumValue(code);
            long res = seqLen * numVal;
            Console.WriteLine(seqLen + " * " + numVal + " = " + res);
            Console.WriteLine("-------------");
            return res;
        }

        private long retriveNumValue(List<char> code) 
        {
            char[] arr = code.ToArray();
            string str = new string(arr);
            str = str.TrimStart(new char[] {'0'});
            str = str.TrimEnd(new char[] { 'A' });
            int value = int.Parse(str);
            Console.WriteLine("Constructed Value: " + value);
            return value;
        }

        private void printList(List<char> presses)
        {
            Console.WriteLine("---");
            foreach(char c in presses)
            {
                Console.Write(c);
            }
            Console.WriteLine();
        }

        private void printList(List<List<char>> input)
        {
            Console.WriteLine("---");
            foreach (List<char> line in input)
            {
                foreach(char c in line)
                {
                    Console.Write("[" + c + "]");
                }
                Console.WriteLine();
            }
        }

        // Each robot will have its own BFS to pathfind to the next button from its current location
        internal class Robot
        {
            public char[,] numPad {  get; set; }
            public char[,] robotPad { get; set; }
            public (int, int) armPos { get; set; }
            public bool isControllingNumPad { get; set; }
            public Robot? target { get; set; }

            public Robot(bool isControllingNumPad)
            {
                numPad = new char[,] {
                    { '7', '8', '9'},
                    { '4', '5', '6'},
                    { '1', '2', '3'},
                    { 'N', '0', 'A'}
                };
                robotPad = new char[,]
                {
                    {'N', '^', 'A'},
                    {'<', 'v', '>'}
                };
                this.isControllingNumPad = isControllingNumPad;
                if (isControllingNumPad)
                {
                    armPos = findChar(numPad,'A');
                }
                else
                {
                    armPos = findChar(robotPad,'A');
                }
            }

            public void setRobot(Robot robot)
            {
                this.target = robot;
            }

            public List<char>? type(List<char> targetChars)
            {
                if (isControllingNumPad) 
                {
                    List<char> directions = new List<char>();
                    foreach (char targetChar in targetChars)
                    {
                        //Console.WriteLine("findChar for:[" + targetChar + "|" + findChar(numPad, targetChar));
                        List<(int, int)> coor = getArmRoute(convertCharToIntGrid(numPad), armPos, findChar(numPad, targetChar));
                        foreach((int, int) val in coor)
                        {
                            Console.Write(val + "|");
                        }
                        directions.AddRange(convertCoorToChars(coor));
                        armPos = coor[coor.Count - 1]; // coordinates
                    }
                    printList(directions);
                    return directions;
                }
                else
                {
                    List<char> directions = new List<char>(); // output
                    List<char> targets = new List<char>(); // retrieved input
                    if(target != null)
                    {
                        targets = target.type(targetChars); // recursive call
                        foreach (char targ in targets)
                        {
                            //Console.WriteLine("findChar for:[" + targ + "|" + findChar(robotPad, targ));
                            List<(int, int)> coor = getArmRoute(convertCharToIntGrid(robotPad), armPos, findChar(robotPad, targ));
                            directions.AddRange(convertCoorToChars(coor));
                            //Console.WriteLine("findChar for:[" + targ + "|" + findChar(robotPad,targ));
                            armPos = coor[coor.Count - 1];
                        }
                        printList(directions);
                        return directions;
                    }
                    else
                    {
                        Console.Error.WriteLine("Target is NULL!");
                        return null;
                    }
                }
            }

            private List<(int, int)> getArmRoute(int[,] grid, (int, int) start, (int, int) end) // 1 = wall, 0 = empty
            {
                List<(int, int)> route = new List<(int, int)>();
                int row = grid.GetLength(0);
                int col = grid.GetLength(1);

                int[,] directions = new int[,]
                {
                    { -1, 0 }, // up
                    { 1, 0 }, // down
                    { 0, 1 }, // right
                    { 0, -1 } // left
                };

                // Check if the starting or ending cell is blocked
                if (grid[start.Item1,start.Item2] == 1 || grid[end.Item1,end.Item2] == 1)
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
                    (int x, int y, int distance) = queue.Dequeue();

                    if (x == end.Item1 && y == end.Item2) 
                    {
                        var current = (x, y);
                        while (current != start)
                        {
                            route.Add(current);
                            current = parent[current];
                        }
                        route.Add(start);
                        route.Reverse();
                        return route;
                    }
                    
                    /*
                    if (visited[x, y])
                    {
                        continue;
                    }
                    visited[x, y] = true;
                    */

                    for (int i = 0; i < directions.GetLength(0); i++)
                    {
                        int newX = x + directions[i, 0];
                        int newY = y + directions[i, 1];

                        // Check if the new position is valid and not visited
                        if (isValid(newX, newY, row, col) && grid[newX, newY] == 0 && !visited[newX, newY])
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

            private (int, int) findChar(char[,] grid, char target)
            {
                for (int i = 0; i < grid.GetLength(0); i++)
                {
                    for (int j = 0; j < grid.GetLength(1); j++)
                    {
                        if (grid[i, j] == target)
                        {
                            return (i, j); //tuples are cool
                        }
                    }
                }
                return (-1, -1);
            }

            private int[,] convertCharToIntGrid(char[,] grid)
            {
                int[,] newGrid = new int[grid.GetLength(0), grid.GetLength(1)];
                for(int i = 0; i < grid.GetLength(0); i++)
                {
                    for(int j = 0; j < grid.GetLength(1); j++)
                    {
                        if (grid[i,j] == 'N') 
                            newGrid[i, j] = 1;
                        else 
                            newGrid[i, j] = 0;
                    }
                }
                return newGrid;
            }

            private List<char> convertCoorToChars(List<(int, int)> route)
            {
                List<char> directions = new List<char>();
                for (int i = 0; i < route.Count - 1; i++) // 0 1
                {
                    if (route[i].Item1 - 1 == route[i + 1].Item1 && route[i].Item2 == route[i + 1].Item2) // up
                    {
                        directions.Add('^');
                    }
                    else if (route[i].Item1 + 1 == route[i + 1].Item1 && route[i].Item2 == route[i + 1].Item2) // down
                    {
                        directions.Add('v');
                    }
                    else if (route[i].Item1 == route[i + 1].Item1 && route[i].Item2 - 1 == route[i + 1].Item2) // left
                    {
                        directions.Add('<');
                    }
                    else if (route[i].Item1 == route[i + 1].Item1 && route[i].Item2 + 1 == route[i + 1].Item2) //right
                    {
                        directions.Add('>');
                    }
                    else
                    {
                        Console.WriteLine("INVALID COORDINATE TO CHAR");
                    }
                }
                directions.Add('A');
                return directions;
            }

            private void printList(List<char> presses)
            {
                Console.WriteLine("---");
                foreach (char c in presses)
                {
                    Console.Write(c);
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
                    List<char> code = new List<char>();
                    foreach(char c in chars)
                    {
                        code.Add(c);
                    }
                    codes.Add(code);
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                printList(codes);
                Console.WriteLine("| AOC Day 21 Resources Initialized! ");
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
