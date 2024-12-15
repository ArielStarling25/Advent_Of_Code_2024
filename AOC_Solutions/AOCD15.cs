using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;
using static AOC_2024_Day1.AOC_Solutions.AOCD14;
using static AOC_2024_Day1.AOC_Solutions.AOCD15;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD15
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD15.txt";

        private List<char> movementSeq = new List<char>();
        private List<List<char>> mainGrid = new List<List<char>>();
        private List<Entity> entities = new List<Entity>();

        private List<List<char>> updatedGrid = new List<List<char>>();

        public AOCD15()
        {
            init();
            updateGrid();
        }

        // Approach: if the robot encounters a block, the robot will contact the block to call blocks(if any) in the way, and recursively makes that block 
        // contact the next block(if any), until it reaches a wall which then it cannot move
        public void solve1()
        {
            locateEntities();
            foreach (Entity entity in entities)
            {
                Console.WriteLine(entity);
            }
            int result = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            foreach(char move in movementSeq)
            {
                moveRobot(move);
                //printGrid(entities);
            }
            foreach(Entity entity in entities)
            {
                if(entity.type == 'O')
                {
                    result += 100 * entity.currPos[0] + entity.currPos[1];
                }
            }
            timer.Stop();
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2()
        {
            locateEntities2();
            foreach (Entity entity in entities)
            {
                Console.WriteLine(entity);
            }
            int result = 0;

            Stopwatch timer = new Stopwatch();
            timer.Start();
            foreach (Entity entity in entities)
            {
                if(entity.type != '@')
                {
                    entity.setFat(true);
                }
            }
            foreach (char move in movementSeq)
            {
                moveRobot2(move);
                //printFatGrid(entities);
                //Console.WriteLine(getRobot() + " | " + move);
                //Console.ReadLine();
            }
            printGrid(updatedGrid);
            Console.WriteLine("^^^ Before | After vvv");
            printFatGrid(entities);
            foreach (Entity entity in entities)
            {
                if (entity.type == 'O')
                {
                    result += 100 * entity.fatCurrPos[0][0] + entity.fatCurrPos[0][1];
                }
            }
            timer.Stop();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        private void moveRobot(char moveDir)
        {
            Entity? robot = getRobot();
            if(robot != null)
            {
                switch (moveDir)
                {
                    case '<':
                        robot = buildRecursiveTreeLeft(robot);
                        break;
                    case '^':
                        robot = buildRecursiveTreeUp(robot);
                        break;
                    case '>':
                        robot = buildRecursiveTreeRight(robot);
                        break;
                    case 'v':
                        robot = buildRecursiveTreeDown(robot);
                        break;
                    default:
                        Console.Error.WriteLine("Encountered an invalid character:[" + moveDir + "]");
                        break;
                }
                Console.WriteLine("Move Robot");
                robot.move(moveDir);
            }
            else
            {
                Console.Error.WriteLine("ROBOT is NULL!");
            }
        }

        private void moveRobot2(char moveDir)
        {
            Entity? robot = getRobot();
            if (robot != null)
            {
                switch (moveDir)
                {
                    case '<':
                        robot = buildRecursiveTreeLeft2(robot);
                        break;
                    case '^':
                        robot = buildRecursiveTreeUp2(robot, 0);
                        break;
                    case '>':
                        robot = buildRecursiveTreeRight2(robot);
                        break;
                    case 'v':
                        robot = buildRecursiveTreeDown2(robot, 0);
                        break;
                    default:
                        Console.Error.WriteLine("Encountered an invalid character:[" + moveDir + "]");
                        break;
                }
                Console.WriteLine("Move Robot:[" + moveDir + "]");
                Console.WriteLine(robot);
                Console.WriteLine("------------VV------------");
                bool potMove = robot.move2(moveDir);
                if (potMove)
                {
                    Console.WriteLine("Moving...");
                    robot.fatMove(moveDir);
                }
                else
                {
                    if(robot.obstacle != null)
                    {
                        Console.WriteLine("Cannot move cuz of " + robot.obstacle.ToString());
                        robot.obstacle = null;
                    }
                    if(robot.obstacle2 != null)
                    {
                        Console.WriteLine("Cannot move cuz of " + robot.obstacle2.ToString());
                        robot.obstacle2 = null;
                    }
                }
                clearAllObstacles();
            }
            else
            {
                Console.Error.WriteLine("ROBOT is NULL!");
            }
        }

        private void clearAllObstacles()
        {
            foreach(Entity entity in entities)
            {
                entity.obstacle = null;
                entity.obstacle2 = null;
            }
        }

        private Entity buildRecursiveTreeLeft2(Entity ent) //recursively build the recursion tree LEFT
        {
            if (ent.isFat)
            {
                if(ent.type == '#')
                {
                    return ent;
                }
                else
                {
                    if (getFatEntityByPos(ent.currPos[0], ent.currPos[1] - 1) != null)
                    {
                        ent.obstacle = buildRecursiveTreeLeft2(getFatEntityByPos(ent.currPos[0], ent.currPos[1] - 1));
                        return ent;
                    }
                    else // encountered empty space
                    {
                        return ent;
                    }
                }
            }
            else
            {
                if(getFatEntityByPos(ent.currPos[0], ent.currPos[1] - 1) != null)
                {
                    ent.obstacle = buildRecursiveTreeLeft2(getFatEntityByPos(ent.currPos[0], ent.currPos[1] - 1));
                    return ent;
                }
                else
                {
                    return ent;
                }
            }
        }

        private Entity buildRecursiveTreeUp2(Entity ent, int side) //recursively build the recursion tree UP
        {
            if (ent.isFat)
            {
                if (ent.typeFat[side] == '#')
                {
                    return ent;
                }
                else
                {
                    int idRec = -1;
                    if (getFatEntityByPos(ent.fatCurrPos[0][0] - 1, ent.fatCurrPos[0][1]) != null)
                    {
                        ent.obstacle = buildRecursiveTreeUp2(getFatEntityByPos(ent.fatCurrPos[0][0] - 1, ent.fatCurrPos[0][1]), 0);
                        idRec = ent.obstacle.id;
                    }

                    if (getFatEntityByPos(ent.fatCurrPos[1][0] - 1, ent.fatCurrPos[1][1]) != null)
                    {
                        ent.obstacle2 = buildRecursiveTreeUp2(getFatEntityByPos(ent.fatCurrPos[1][0] - 1, ent.fatCurrPos[1][1]), 1);
                        if(idRec == ent.obstacle2.id)
                        {
                            ent.obstacle2 = null;
                        }
                    }
                    return ent;
                }
            }
            else
            {
                if (getFatEntityByPos(ent.currPos[0] - 1, ent.currPos[1]) != null)
                {
                    ent.obstacle = buildRecursiveTreeUp2(getFatEntityByPos(ent.currPos[0] - 1, ent.currPos[1]), 0);
                    return ent;
                }
                else
                {
                    return ent;
                }
            }
        }

        private Entity buildRecursiveTreeRight2(Entity ent) //recursively build the recursion tree RIGHT
        {
            Console.WriteLine(ent);
            if (ent.isFat)
            {
                if (ent.type == '#')
                {
                    return ent;
                }
                else
                {
                    if (getFatEntityByPos(ent.fatCurrPos[1][0], ent.fatCurrPos[1][1] + 1) != null)
                    {
                        ent.obstacle = buildRecursiveTreeRight2(getFatEntityByPos(ent.fatCurrPos[1][0], ent.fatCurrPos[1][1] + 1));
                        return ent;
                    }
                    else // encountered empty space
                    {
                        return ent;
                    }
                }
            }
            else
            {
                if (getFatEntityByPos(ent.fatCurrPos[0][0], ent.fatCurrPos[0][1] + 1) != null)
                {
                    ent.obstacle = buildRecursiveTreeRight2(getFatEntityByPos(ent.fatCurrPos[0][0], ent.fatCurrPos[0][1] + 1));
                    return ent;
                }
                else
                {
                    return ent;
                }
            }
        }

        private Entity buildRecursiveTreeDown2(Entity ent, int side) //recursively build the recursion tree DOWN
        {
            if (ent.isFat)
            {
                if (ent.typeFat[side] == '#')
                {
                    return ent;
                }
                else
                {
                    int idRec = -1;
                    if (getFatEntityByPos(ent.fatCurrPos[0][0] + 1, ent.fatCurrPos[0][1]) != null)
                    {
                        ent.obstacle = buildRecursiveTreeDown2(getFatEntityByPos(ent.fatCurrPos[0][0] + 1, ent.fatCurrPos[0][1]), 0);
                        idRec = ent.obstacle.id;
                    }

                    if (getFatEntityByPos(ent.fatCurrPos[1][0] + 1, ent.fatCurrPos[1][1]) != null)
                    {
                        ent.obstacle2 = buildRecursiveTreeDown2(getFatEntityByPos(ent.fatCurrPos[1][0] + 1, ent.fatCurrPos[1][1]), 1);
                        if (idRec == ent.obstacle2.id)
                        {
                            ent.obstacle2 = null;
                        }
                    }
                    return ent;
                }
            }
            else
            {
                if (getFatEntityByPos(ent.currPos[0] + 1, ent.currPos[1]) != null)
                {
                    ent.obstacle = buildRecursiveTreeDown2(getFatEntityByPos(ent.currPos[0] + 1, ent.currPos[1]), 0);
                    return ent;
                }
                else
                {
                    return ent;
                }
            }
        }

        private Entity buildRecursiveTreeLeft(Entity robot) // robot - block - wall
        {
            int count = 1;
            bool walledOrEmpty = false;
            List<Entity> recurseList = new List<Entity>();
            if (getEntityByPos(robot.currPos[0], robot.currPos[1] - count) != null)
            {
                while (!walledOrEmpty)
                {
                    Console.WriteLine("LoopLeft");
                    if (getEntityByPos(robot.currPos[0], robot.currPos[1] - count) != null)
                    {
                        if (getEntityByPos(robot.currPos[0], robot.currPos[1] - count).type == '#')
                        {
                            //stop
                            walledOrEmpty = true;
                            recurseList.Add(getEntityByPos(robot.currPos[0], robot.currPos[1] - count));
                        }
                        else if (getEntityByPos(robot.currPos[0], robot.currPos[1] - count).type == 'O')
                        {
                            recurseList.Add(getEntityByPos(robot.currPos[0], robot.currPos[1] - count));
                        }
                    }
                    else
                    {
                        walledOrEmpty = true;
                    }
                    count++;
                }

                if(recurseList.Count > 1)
                {
                    for (int i = 0; i < recurseList.Count - 1; i++)
                    {
                        recurseList[i].setNext(recurseList[i + 1]);
                    }
                    robot.setNext(recurseList[0]);
                }
                else
                {
                    robot.setNext(recurseList[0]);
                }
            }
            else
            {
                robot.setNext(null);
            }
            return robot;
        }

        private Entity buildRecursiveTreeUp(Entity robot) // robot - block - wall
        {
            int count = 1;
            bool walledOrEmpty = false;
            List<Entity> recurseList = new List<Entity>();
            if (getEntityByPos(robot.currPos[0] - count, robot.currPos[1]) != null)
            {
                while (!walledOrEmpty)
                {
                    Console.WriteLine("LoopUp");
                    if (getEntityByPos(robot.currPos[0] - count, robot.currPos[1]) != null)
                    {
                        if (getEntityByPos(robot.currPos[0] - count, robot.currPos[1]).type == '#')
                        {
                            //stop
                            walledOrEmpty = true;
                            recurseList.Add(getEntityByPos(robot.currPos[0] - count, robot.currPos[1]));
                        }
                        else if (getEntityByPos(robot.currPos[0] - count, robot.currPos[1]).type == 'O')
                        {
                            recurseList.Add(getEntityByPos(robot.currPos[0] - count, robot.currPos[1]));
                        }
                    }
                    else
                    {
                        walledOrEmpty = true;
                    }
                    count++;
                }

                if (recurseList.Count > 1)
                {
                    for (int i = 0; i < recurseList.Count - 1; i++)
                    {
                        recurseList[i].setNext(recurseList[i + 1]);
                    }
                    robot.setNext(recurseList[0]);
                }
                else
                {
                    robot.setNext(recurseList[0]);
                }
            }
            else
            {
                robot.setNext(null);
            }
            return robot;
        }

        private Entity buildRecursiveTreeRight(Entity robot) // robot - block - wall
        {
            int count = 1;
            bool walledOrEmpty = false;
            List<Entity> recurseList = new List<Entity>();
            if (getEntityByPos(robot.currPos[0], robot.currPos[1] + count) != null)
            {
                while (!walledOrEmpty)
                {
                    Console.WriteLine("LoopRight");
                    if (getEntityByPos(robot.currPos[0], robot.currPos[1] + count) != null)
                    {
                        if (getEntityByPos(robot.currPos[0], robot.currPos[1] + count).type == '#')
                        {
                            //stop
                            walledOrEmpty = true;
                            recurseList.Add(getEntityByPos(robot.currPos[0], robot.currPos[1] + count));
                        }
                        else if (getEntityByPos(robot.currPos[0], robot.currPos[1] + count).type == 'O')
                        {
                            recurseList.Add(getEntityByPos(robot.currPos[0], robot.currPos[1] + count));
                        }
                    }
                    else
                    {
                        walledOrEmpty = true;
                    }
                    count++;
                }

                if (recurseList.Count > 1)
                {
                    for (int i = 0; i < recurseList.Count - 1; i++)
                    {
                        recurseList[i].setNext(recurseList[i + 1]);
                    }
                    robot.setNext(recurseList[0]);
                }
                else
                {
                    robot.setNext(recurseList[0]);
                }
            }
            else
            {
                robot.setNext(null);
            }
            return robot;
        }

        private Entity buildRecursiveTreeDown(Entity robot) // robot - block - wall
        {
            int count = 1;
            bool walledOrEmpty = false;
            List<Entity> recurseList = new List<Entity>();
            if (getEntityByPos(robot.currPos[0] + count, robot.currPos[1]) != null)
            {
                while (!walledOrEmpty)
                {
                    Console.WriteLine("LoopDown");
                    if(getEntityByPos(robot.currPos[0] + count, robot.currPos[1]) != null)
                    {
                        if (getEntityByPos(robot.currPos[0] + count, robot.currPos[1]).type == '#')
                        {
                            //stop
                            walledOrEmpty = true;
                            recurseList.Add(getEntityByPos(robot.currPos[0] + count, robot.currPos[1]));
                        }
                        else if (getEntityByPos(robot.currPos[0] + count, robot.currPos[1]).type == 'O')
                        {
                            recurseList.Add(getEntityByPos(robot.currPos[0] + count, robot.currPos[1]));
                        }
                    }
                    else
                    {
                        walledOrEmpty = true;
                    }
                    count++;
                }

                if (recurseList.Count > 1)
                {
                    for (int i = 0; i < recurseList.Count - 1; i++)
                    {
                        recurseList[i].setNext(recurseList[i + 1]);
                    }
                    robot.setNext(recurseList[0]);
                }
                else
                {
                    robot.setNext(recurseList[0]);
                }
            }
            else
            {
                robot.setNext(null);
            }
            return robot;
        }

        private Entity? getEntityByPos(int in1, int in2)
        {
            for(int i = 0; i < entities.Count; i++)
            {
                if (entities[i].currPos[0] == in1 && entities[i].currPos[1] == in2)
                {
                    return entities[i];
                }
            }
            return null;
        }

        private Entity? getFatEntityByPos(int in1, int in2)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if ((entities[i].fatCurrPos[0][0] == in1 && entities[i].fatCurrPos[0][1] == in2) || (entities[i].fatCurrPos[1][0] == in1 && entities[i].fatCurrPos[1][1] == in2))
                {
                    return entities[i];
                }
            }
            return null;
        }

        private Entity? getRobot()
        {
            for(int i = 0; i < entities.Count; i++)
            {
                if (entities[i].type == '@')
                {
                    return entities[i];
                }
            }
            return null;
        }

        private void locateEntities()
        {
            int counter = 1;
            for(int i = 0; i < mainGrid.Count; i++)
            {
                for(int j = 0; j < mainGrid[i].Count; j++)
                {
                    if (mainGrid[i][j] == '#')
                    {
                        Entity newEnt = new Entity(new int[] {i, j}, '#', counter);
                        entities.Add(newEnt);
                        counter++;
                    }
                    else if (mainGrid[i][j] == 'O')
                    {
                        Entity newEnt = new Entity(new int[] { i, j }, 'O', counter);
                        entities.Add(newEnt);
                        counter++;
                    }
                    else if (mainGrid[i][j] == '@')
                    {
                        Entity newEnt = new Entity(new int[] { i, j }, '@', counter);
                        entities.Add(newEnt);
                        counter++;
                    }
                }
            }
        }

        private void locateEntities2()
        {
            int counter = 1;
            for (int i = 0; i < updatedGrid.Count; i++)
            {
                for (int j = 0; j < updatedGrid[i].Count-1; j = j + 2)
                {
                    if (updatedGrid[i][j] == '#')
                    {
                        Entity newEnt = new Entity(new int[] { i, j }, '#', counter);
                        entities.Add(newEnt);
                        counter++;
                    }
                    else if (updatedGrid[i][j] == '[')
                    {
                        Entity newEnt = new Entity(new int[] { i, j }, 'O', counter);
                        entities.Add(newEnt);
                        counter++;
                    }
                    else if (updatedGrid[i][j] == '@')
                    {
                        Entity newEnt = new Entity(new int[] { i, j }, '@', counter);
                        entities.Add(newEnt);
                        counter++;
                    }
                    else if (updatedGrid[i][j+1] == '@')
                    {
                        Entity newEnt = new Entity(new int[] { i, j+1 }, '@', counter);
                        entities.Add(newEnt);
                        counter++;
                    }
                }
            }
        }

        internal class Entity
        {
            public int id { get; set; }
            public int[] currPos { get; set; }
            public List<int[]> fatCurrPos { get; set; }
            public int[] initPos { get; set; }
            public char type { get; set; } // could either be '@', 'O', '#'
            public char[] typeFat { get; set; } // could either be '[]' or '##'
            public Entity? obstacle { get; set; }
            public Entity? obstacle2 { get; set; }
            public bool isFat { get; set; }
            private bool moved { get; set; }
            
            public Entity(int[] setPos, char type, int inId)
            {
                initPos = setPos;
                currPos = new int[] { setPos[0], setPos[1] };
                this.type = type;
                fatVer();
                id = inId;
                obstacle = null;
                obstacle2 = null;
                isFat = false;
                moved = false;
            }

            private void fatVer()
            {
                if(type == 'O')
                {
                    typeFat = new char[] { '[', ']' };
                }
                else if(type == '#')
                {
                    typeFat = new char[] { '#', '#' };
                }
                else
                {
                    typeFat = new char[] { '@', '.' };
                }

                fatCurrPos = new List<int[]>();
                if(type == '@')
                {
                    int[] side1 = new int[] { currPos[0], currPos[1] };
                    int[] side2 = new int[] { currPos[0], currPos[1] };
                    fatCurrPos.Add(side1);
                    fatCurrPos.Add(side2);
                }
                else
                {
                    int[] side1 = new int[] { currPos[0], currPos[1] };
                    int[] side2 = new int[] { currPos[0], currPos[1] + 1 };
                    fatCurrPos.Add(side1);
                    fatCurrPos.Add(side2);
                }
            }

            public void setFat(bool input)
            {
                isFat = input;
            }

            public void setNext(Entity? ent)
            {
                obstacle = ent;
            }

            public bool move(char moveDir) // returns false if it cannot be moved, aka, encounters a wall
            {
                if(type == '#') // if the entity is a wall, it cannot be moved
                {
                    return false;
                }

                if(obstacle != null)
                {
                    if (obstacle.move(moveDir))
                    {
                        switch (moveDir)
                        {
                            case '<':
                                currPos[1]--;
                                break;
                            case '^':
                                currPos[0]--;
                                break;
                            case '>':
                                currPos[1]++;
                                break;
                            case 'v':
                                currPos[0]++;
                                break;
                            default:
                                Console.Error.WriteLine("Encountered an invalid character:[" + moveDir + "]");
                                break;
                        }
                        obstacle = null;
                        return true;
                    }
                    else
                    {
                        obstacle = null;
                        return false;
                    }
                }
                else
                {
                    switch (moveDir)
                    {
                        case '<':
                            currPos[1]--;
                            break;
                        case '^':
                            currPos[0]--;
                            break;
                        case '>':
                            currPos[1]++;
                            break;
                        case 'v':
                            currPos[0]++;
                            break;
                        default:
                            Console.Error.WriteLine("Encountered an invalid character:[" + moveDir + "]");
                            break;
                    }
                    obstacle = null;
                    return true;
                }
            }

            public bool move2(char moveDir)
            {
                moved = false;
                if (type == '#') // if the entity is a wall, it cannot be moved
                {
                    return false;
                }

                if(moveDir == '^' || moveDir == 'v')
                {
                    if (obstacle == null && obstacle2 == null)
                    {
                        return true;
                    }
                    else
                    {
                        bool rec1 = true;
                        bool rec2 = true;
                        if (obstacle != null)
                        {
                            rec1 = obstacle.move2(moveDir);
                        }
                        if (obstacle2 != null)
                        {
                            rec2 = obstacle2.move2(moveDir);
                        }
                        if (rec1 == true && rec2 == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if(obstacle == null)
                    {
                        return true;
                    }
                    else
                    {
                        bool rec = true;
                        if(obstacle != null)
                        {
                            rec = obstacle.move2(moveDir);
                        }
                        if(rec == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                
            }

            public void fatMove(char moveDir)
            {
                if (obstacle != null)
                {
                    obstacle.fatMove(moveDir);
                }
                if (obstacle2 != null)
                {
                    obstacle2.fatMove(moveDir);
                }
                obstacle = null;
                obstacle2 = null;
                if (!moved)
                {
                    if (type != '#')
                    {
                        switch (moveDir)
                        {
                            case '<':
                                currPos[1]--;
                                fatCurrPos[0][1]--;
                                fatCurrPos[1][1]--;
                                break;
                            case '^':
                                currPos[0]--;
                                fatCurrPos[0][0]--;
                                fatCurrPos[1][0]--;
                                break;
                            case '>':
                                currPos[1]++;
                                fatCurrPos[0][1]++;
                                fatCurrPos[1][1]++;
                                break;
                            case 'v':
                                currPos[0]++;
                                fatCurrPos[0][0]++;
                                fatCurrPos[1][0]++;
                                break;
                            default:
                                Console.Error.WriteLine("Encountered an invalid character:[" + moveDir + "]");
                                break;
                        }
                    }
                }
            }

            private string getObs(int num)
            {
                if(num == 1)
                {
                    if(obstacle != null)
                    {
                        return obstacle.type.ToString();
                    }
                    else
                    {
                        return "NULL";
                    }
                }
                else
                {
                    if(obstacle2 != null)
                    {
                        return obstacle2.type.ToString();
                    }
                    else
                    {
                        return "NULL";
                    }
                }
            }

            public override string ToString()
            {
                string val = "ID:[" + id + "]|TYPE:[" + type + "]|OBS1:[" + getObs(1) + "]|OBS2:[" + getObs(2) + "]|CURRPOS:[" + currPos[0] + "," + currPos[1] + "]|FATPOS:[" + fatCurrPos[0][0] + "," + fatCurrPos[0][1] + "][" + fatCurrPos[1][0] + "," + fatCurrPos[1][1] + "]";
                return val;
            }
        }

        private void printGrid(List<Entity> ents)
        {
            List<List<char>> grid = new List<List<char>>();
            for(int i = 0; i < mainGrid.Count; i++)
            {
                List<char> chars = new List<char>();
                for(int j = 0; j < mainGrid[i].Count; j++)
                {
                    chars.Add('.');
                }
                grid.Add(chars);
            }

            foreach(Entity ent in ents)
            {
                grid[ent.currPos[0]][ent.currPos[1]] = ent.type;
            }

            printGrid(grid);
        }

        private void printFatGrid(List<Entity> ents)
        {
            List<List<char>> grid = new List<List<char>>();
            for (int i = 0; i < updatedGrid.Count; i++)
            {
                List<char> chars = new List<char>();
                for (int j = 0; j < updatedGrid[i].Count; j++)
                {
                    chars.Add('.');
                }
                grid.Add(chars);
            }

            foreach (Entity ent in ents)
            {
                if (ent.type == '@')
                {
                    grid[ent.currPos[0]][ent.currPos[1]] = ent.type;
                }
                else
                {
                    grid[ent.fatCurrPos[0][0]][ent.fatCurrPos[0][1]] = ent.typeFat[0];
                    grid[ent.fatCurrPos[1][0]][ent.fatCurrPos[1][1]] = ent.typeFat[1];
                }
            }

            printGrid(grid);
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

        private void printList(List<char> sequence)
        {
            Console.WriteLine("---");
            foreach(char c in sequence)
            {
                Console.Write(c);
            }
            Console.WriteLine();
        }

        private void updateGrid()
        {
            for(int i = 0; i < mainGrid.Count; i++)
            {
                List<char> chars = new List<char>();
                for(int j = 0; j < mainGrid[i].Count; j++)
                {
                    if (mainGrid[i][j] == '#')
                    {
                        chars.Add('#');
                        chars.Add('#');
                    }
                    else if (mainGrid[i][j] == 'O')
                    {
                        chars.Add('[');
                        chars.Add(']');
                    }
                    else if (mainGrid[i][j] == '@')
                    {
                        chars.Add('@');
                        chars.Add('.');
                    }
                    else
                    {
                        chars.Add('.');
                        chars.Add('.');
                    }
                }
                updatedGrid.Add(chars);
            }
            printGrid(updatedGrid);
        }

        private void init()
        {
            string line;
            int count = 0;
            bool seqScan = false;
            try
            {
                StreamReader sr = new StreamReader(inputFile);
                line = sr.ReadLine();
                while (line != null)
                {
                    count++;
                    //VVV Do file reading operations here VVV
                    char[] charArr = line.ToCharArray();
                    if(charArr.Length < 1 && seqScan == false)
                    {
                        seqScan = true;
                        continue;
                    }
                    if (!seqScan) //scan grid
                    {
                        List<char> grid = new List<char>();
                        for (int i = 0; i < charArr.Length; i++)
                        {
                            grid.Add(charArr[i]);
                        }
                        mainGrid.Add(grid);
                    }
                    else // seq scan
                    {
                        for (int i = 0; i < charArr.Length; i++)
                        {
                            movementSeq.Add(charArr[i]);
                        }
                    }
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                printGrid(mainGrid);
                printList(movementSeq);
                sr.Close();
                Console.WriteLine("| AOC Day 15 Resources Initialized! ");
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
