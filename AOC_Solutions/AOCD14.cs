using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static AOC_2024_Day1.AOC_Solutions.AOCD14;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD14
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD14.txt";

        private List<Robot> robots = new List<Robot>();
        private readonly int setILimitExample = 7; // Example
        private readonly int setJLimitExample = 11; // Example

        private readonly int setILimit = 103;
        private readonly int setJLimit = 101;

        private bool isOn = true;

        public AOCD14()
        {
            init();
        }

        public void solve1()
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            for(int i = 0; i < robots.Count; i++)
            {
                robots[i].xLimit = setJLimit;
                robots[i].yLimit = setILimit;
                Console.WriteLine("Robot:" + (i+1));
                Console.WriteLine(robots[i]);
                robots[i].moveAll(100);
                Console.WriteLine(robots[i]);
            }

            placeRobotsOnMap(robots);

            int q1 = 0, q2 = 0, q3 = 0, q4 = 0;
            for(int i = 0; i < robots.Count; i++)
            {
                if (robots[i].currPosition[0] < robots[i].xLimit/2 && robots[i].currPosition[1] < robots[i].yLimit/2)
                {
                    q1++;
                }
                else if (robots[i].currPosition[0] < robots[i].xLimit/2 && robots[i].currPosition[1] > robots[i].yLimit / 2)
                {
                    q2++;
                }
                else if (robots[i].currPosition[0] > robots[i].xLimit/2 && robots[i].currPosition[1] < robots[i].yLimit / 2)
                {
                    q3++;
                }
                else if (robots[i].currPosition[0] > robots[i].xLimit/2 && robots[i].currPosition[1] > robots[i].yLimit / 2)
                {
                    q4++;
                }
            }

            int result = q1 * q2 * q3 * q4;
            timer.Stop();
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
            /*
            robots[10].xLimit = setJLimitExample;
            robots[10].yLimit = setILimitExample;
            robots[10].moveAll(5);
            Console.WriteLine(robots[10]);
            placeRobotOnMap(robots[10]);
            */
        }

        public void solve2()
        {
            int counter = 0;
            for (int i = 0; i < robots.Count; i++)
            {
                robots[i].xLimit = setJLimit;
                robots[i].yLimit = setILimit;
                //Console.WriteLine("Robot:" + (i + 1));
                //Console.WriteLine(robots[i]);
                robots[i].moveAll(7750);
                //Console.WriteLine(robots[i]);
            }
            counter = 7750;
            while (isOn)
            {
                for (int i = 0; i < robots.Count; i++)
                {
                    robots[i].xLimit = setJLimit;
                    robots[i].yLimit = setILimit;
                    //Console.WriteLine("Robot:" + (i + 1));
                    robots[i].moveAll(1);
                }
                placeRobotsOnMap(robots);
                counter++;
                Console.WriteLine("Steps Taken:[" + counter + "]| Press W to progress, Press X to stop");
                setReadKey();
            }
            Console.WriteLine("Final 2: [" + counter + "] steps");
        }

        private void setReadKey()
        {
            char input = Console.ReadKey().KeyChar;
            Console.Clear();
            switch (input)
            {
                case 'w':
                case 'W':
                    
                    break;
                case 'x':
                case 'X':
                    isOn = false;
                    break;
                default:
                    //isOn = false;
                    break;
            }
        }

        private void placeRobotsOnMap(List<Robot> bots)
        {
            List<List<int>> grid = new List<List<int>>();
            for(int i = 0; i < setILimit; i++)
            {
                List<int> gridPart = new List<int>();
                for(int j = 0; j < setJLimit; j++)
                {
                    gridPart.Add(0);
                }
                grid.Add(gridPart);
            }

            foreach (Robot robot in bots)
            {
                grid[robot.currPosition[1]][robot.currPosition[0]]++;
            }

            printGrid(grid);
        }

        private void placeRobotOnMap(Robot bot)
        {
            List<List<int>> grid = new List<List<int>>();
            for (int i = 0; i < setILimitExample; i++)
            {
                List<int> gridPart = new List<int>();
                for (int j = 0; j < setJLimitExample; j++)
                {
                    gridPart.Add(0);
                }
                grid.Add(gridPart);
            }

            grid[bot.currPosition[1]][bot.currPosition[0]]++;

            printGrid(grid);
        }

        internal class Robot
        {
            public int[] initPosition {  get; set; }
            public int[] currPosition { get; set; }
            public long numOfSecondsElapsed { get; set; }
            public int[] velocity {  get; set; }
            public int yLimit {  get; set; }
            public int xLimit { get; set; }

            public Robot(int[] initPos, int[] velocity, int xLimit, int yLimit)
            {
                initPosition = initPos;
                currPosition = initPosition;
                numOfSecondsElapsed = 0;
                this.velocity = velocity;
                this.yLimit = yLimit;
                this.yLimit = xLimit;
            }

            public Robot(int[] initPos, int[] velocity)
            {
                initPosition = initPos;
                currPosition = new int[] { initPos[0], initPos[1] };
                numOfSecondsElapsed = 0;
                this.velocity = velocity;
                this.xLimit = 0;
                this.yLimit = 0;
            }

            public void move()
            {

            }

            public void moveAll(long secondsPassed)
            {
                for (long i = 0; i < secondsPassed; i++)
                {
                    numOfSecondsElapsed++;
                    currPosition[0] = currPosition[0] + velocity[0]; // j
                    currPosition[1] = currPosition[1] + velocity[1]; // i
                    if (currPosition[1] < 0 && currPosition[0] >= 0 && currPosition[0] < xLimit) // if robot exited out the top , 
                    {
                        // shift everything down
                        currPosition[1] += yLimit;
                        //Console.WriteLine("Shifting down");
                    }
                    else if (currPosition[1] >= yLimit && currPosition[0] >= 0 && currPosition[0] < xLimit) // if robot exited out the bottom
                    {
                        // shift everthing up
                        currPosition[1] -= yLimit;
                        //Console.WriteLine("Shifting up");
                    }
                    else if (currPosition[0] < 0 && currPosition[1] >= 0 && currPosition[1] < yLimit) // if robot exited out the left
                    {
                        // shift everything right
                        currPosition[0] += xLimit;
                        //Console.WriteLine("Shifting right");
                    }
                    else if (currPosition[0] >= xLimit && currPosition[1] >= 0 && currPosition[1] < yLimit) // if robot exited out the right
                    {
                        //shift everything left
                        currPosition[0] -= xLimit;
                        //Console.WriteLine("Shifting left");
                    }
                    else if (currPosition[1] < 0 && currPosition[0] < 0) // if robot exited out the top left
                    {
                        currPosition[1] += yLimit;
                        currPosition[0] += xLimit;
                    }
                    else if (currPosition[1] < 0 && currPosition[0] >= xLimit) // if robot exited out the top right
                    {
                        currPosition[1] += yLimit;
                        currPosition[0] -= xLimit;
                    }
                    else if (currPosition[1] >= yLimit && currPosition[0] < 0) // if robot exited out the bottom left
                    {
                        currPosition[1] -= yLimit;
                        currPosition[0] += xLimit;
                    }
                    else if (currPosition[1] >= yLimit && currPosition[0] >= xLimit) // if robot exited out the bottom right
                    {
                        currPosition[1] -= yLimit;
                        currPosition[0] -= xLimit;
                    }
                }
            }

            public override string ToString()
            {
                string val = "ROBOT:INIT[" + initPosition[0] + "," + initPosition[1] + "]|VELO:[" + velocity[0] + "," + velocity[1] + "]|CURRPOS:[" + currPosition[0] + "," + currPosition[1] + "]|SECONDSPASS:[" + numOfSecondsElapsed + "]|SETLIMITS:[" + xLimit + "," + yLimit + "]";
                return val;
            }
        }

        private void printArr(string[] arr)
        {
            Console.WriteLine("|");
            foreach (string str in arr)
            {
                Console.Write(str + "|");
            }
        }

        private void printGrid(List<List<int>> grid)
        {
            Console.WriteLine("---");
            foreach(List<int> piece in grid)
            {
                Console.WriteLine();
                foreach(int item in piece)
                {
                    if(item != 0)
                    {
                        Console.Write(item);
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
            }
            Console.WriteLine();
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
                    string[] split = line.Split(new char[] {'=',',',' '});
                    printArr(split);
                    Robot newRobot = new Robot(new int[] {int.Parse(split[1]), int.Parse(split[2])}, new int[] {int.Parse(split[4]), int.Parse(split[5])});
                    robots.Add(newRobot);
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day 14 Resources Initialized! ");
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
