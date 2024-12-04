using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD4
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD4.txt";

        List<string> rawGrid = new List<string>();
        List<List<char>> charGrid = new List<List<char>>();

        private readonly char[] validChars = {'X','M','A','S'};

        // SAMXMAS
        //    M
        //    A
        //    S
        // 1234567
        // 7 - 3 = 4
        // 7 x 7 search grid

        public AOCD4()
        {
            init();
        }

        public void solve1()
        {
            int result = 0;
            for(int x = 0; x < charGrid.Count; x++)
            {
                for(int y = 0; y < charGrid[x].Count; y++)
                {
                    if (charGrid[x][y] == validChars[0])
                    {
                        result += xmasFinder(x, y);
                    }
                }
            }
            Console.WriteLine("Final 1: " + result);
        }

        private int xmasFinder(int indX, int indY)
        {
            int res = 0;
            int pass = 0;
            //Look W
            if(indX >= 3)
            {
                for(int c = 0; c < 4; c++)
                {
                    if (charGrid[indX - c][indY] == validChars[c])
                    {
                        pass++;
                    }
                }
                if(pass == 4)
                {
                    Console.WriteLine("FOUND AT:[" + indX + "][" + indY + "]| WEST");
                    res++;
                }
                pass = 0;
            }
            //Look NW
            if(indX >= 3 && indY >= 3)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (charGrid[indX - c][indY - c] == validChars[c])
                    {
                        pass++;
                    }
                }
                if (pass == 4)
                {
                    Console.WriteLine("FOUND AT:[" + indX + "][" + indY + "]| NORTHWEST");
                    res++;
                }
                pass = 0;
            }
            //Look N
            if(indY >= 3)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (charGrid[indX][indY - c] == validChars[c])
                    {
                        pass++;
                    }
                }
                if (pass == 4)
                {
                    Console.WriteLine("FOUND AT:[" + indX + "][" + indY + "]| NORTH");
                    res++;
                }
                pass = 0;
            }
            //Look NE
            if(indY >= 3 && indX <= charGrid[0].Count - 4)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (charGrid[indX + c][indY - c] == validChars[c])
                    {
                        pass++;
                    }
                }
                if (pass == 4)
                {
                    Console.WriteLine("FOUND AT:[" + indX + "][" + indY + "]| NORTHEAST");
                    res++;
                }
                pass = 0;
            }
            //Look E
            if(indX <= charGrid[0].Count - 4) // XMAS|   5678| Length 9  | 9 - 4 = 5
            {
                for (int c = 0; c < 4; c++)
                {
                    if (charGrid[indX + c][indY] == validChars[c])
                    {
                        pass++;
                    }
                }
                if (pass == 4)
                {
                    Console.WriteLine("FOUND AT:[" + indX + "][" + indY + "]| EAST");
                    res++;
                }
                pass = 0;
            }
            //Look SE
            if(indX <= charGrid[0].Count - 4 && indY <= charGrid.Count - 4)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (charGrid[indX + c][indY + c] == validChars[c])
                    {
                        pass++;
                    }
                }
                if (pass == 4)
                {
                    Console.WriteLine("FOUND AT:[" + indX + "][" + indY + "]| SOUTHEAST");
                    res++;
                }
                pass = 0;
            }
            //Look S
            if(indY <= charGrid.Count - 4)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (charGrid[indX][indY + c] == validChars[c])
                    {
                        pass++;
                    }
                }
                if (pass == 4)
                {
                    Console.WriteLine("FOUND AT:[" + indX + "][" + indY + "]| SOUTH");
                    res++;
                }
                pass = 0;
            }
            //Look SW
            if (indY <= charGrid.Count - 4 && indX >= 3)
            {
                for (int c = 0; c < 4; c++)
                {
                    if (charGrid[indX - c][indY + c] == validChars[c])
                    {
                        pass++;
                    }
                }
                if (pass == 4)
                {
                    Console.WriteLine("FOUND AT:[" + indX + "][" + indY + "]| SOUTHWEST");
                    res++;
                }
                pass = 0;
            }
            return res;
        }

        public void solve2()
        {
            int result = 0;
            for (int x = 1; x < charGrid.Count-1; x++)
            {
                for (int y = 1; y < charGrid[x].Count-1; y++)
                {
                    if (charGrid[x][y] == validChars[2])
                    {
                        result += literalXMASFinder(x,y);
                    }
                }
            }
            Console.WriteLine("Final 2: " + result);
        }


        // M S | M M | S M | S S
        //  A  |  A  |  A  |  A
        // M S | S S | S M | M M
        private int literalXMASFinder(int indX, int indY)
        {
            int res = 0;
            char[,] validSeqs = {
                {'M','S','S','M'},
                {'M','M','S','S'},
                {'S','M','M','S'},
                {'S','S','M','M'}
            };
            int[,] checkInd = {
                {-1,-1},
                {-1,1},
                {1,1},
                {1,-1}
            };

            int pass = 0;
            for(int i = 0; i < 4; i++)
            {
                Console.WriteLine("AT:[" + indX + "][" + indY + "]");
                for(int j = 0; j < 4; j++)
                {
                    int diffX = checkInd[j,0];
                    int diffY = checkInd[j,1];
                    Console.Write("Checker:[" + charGrid[indX + diffX][indY + diffY] + "]|Compare:[" + validSeqs[i,j] + "]");
                    if (charGrid[indX + diffX][indY + diffY] == validSeqs[i, j])
                    {
                        Console.WriteLine("| TRUE");
                        pass++;
                    }
                    else
                    {
                        Console.WriteLine("| FALSE");
                    }
                }
                Console.WriteLine("PASS_VALUE:[" + pass + "]");
                if(pass == 4)
                {
                    Console.WriteLine("FOUNDAT:[" + indX + "][" + indY + "]");
                    res++;
                    break;
                }
                pass = 0;
            }
            return res;
        }

        public void printRawString()
        {
            foreach (string str in rawGrid) { 
                Console.WriteLine(str);
            }
        }

        public void printCharGrid()
        {
            for(int i = 0; i < charGrid.Count; i++)
            {
                for(int j = 0; j < charGrid[i].Count; j++)
                {
                    Console.Write(charGrid[i][j]);
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
                    List<char> tempChars = new List<char>();
                    rawGrid.Add(line);
                    char[] chars = line.ToCharArray();
                    for(int i = 0; i < chars.Length; i++)
                    {
                        tempChars.Add(chars[i]);
                    }
                    charGrid.Add(tempChars);
                    line = sr.ReadLine();
                }
                //printRawString();
                //printCharGrid();
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day 4 Resources Initialized! ");
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
