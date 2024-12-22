using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD21B
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD21.txt";

        public AOCD21B()
        {
            Console.WriteLine(Go(2));
            //Console.WriteLine(Go(25));
        }

        static Dictionary<char, (int, int)> keyp = new Dictionary<char, (int, int)>
        {
            {'7', (0, 0)}, {'8', (1, 0)}, {'9', (2, 0)},
            {'4', (0, 1)}, {'5', (1, 1)}, {'6', (2, 1)},
            {'1', (0, 2)}, {'2', (1, 2)}, {'3', (2, 2)},
            {' ', (1, 3)}, {'0', (1, 3)}, {'A', (1, 3)}
        };

        static Dictionary<char, (int, int)> dirp = new Dictionary<char, (int, int)>
        {
            {' ', (0, 0)}, {'^', (0, -1)}, {'A', (0, 0)},
            {'<', (-1, 0)}, {'v', (0, 1)}, {'>', (1, 0)}
        };

        static Counter Steps(Dictionary<char, (int, int)> G, string s, int i = 1)
        {
            var px = G['A'].Item1;
            var py = G['A'].Item2;
            var bx = G[' '].Item1;
            var by = G[' '].Item2;
            var res = new Counter();

            foreach (char c in s)
            {
                var (npx, npy) = G[c];
                bool f = (npx == bx && py == by) || (npy == by && px == bx);
                res.Add((npx - px, npy - py, f), i);
                px = npx;
                py = npy;
            }
            return res;
        }

        static long Go(int n)
        {
            long r = 0;
            var codes = File.ReadAllLines(inputFile);

            foreach (string code in codes)
            {
                var res = Steps(keyp, code);
                for (int j = 0; j <= n; j++)
                {
                    var newRes = new Counter();
                    foreach (var kvp in res)
                    {
                        var (x, y, f) = kvp.Key;
                        int count = kvp.Value;

                        string moves = new string('<', -x) + new string('v', y) + new string('^', -y) + new string('>', x);
                        moves = new string(moves.Reverse().ToArray()) + "A"; // Reverse the moves and add 'A'
                        var stepResult = Steps(dirp, moves, count);

                        foreach (var stepKvp in stepResult)
                        {
                            newRes.Add(stepKvp.Key, stepKvp.Value);
                        }
                    }
                    res = newRes;
                }
                r += res.Total() * int.Parse(code.Substring(0, 3));
            }
            return r;
        }

        internal class Counter : Dictionary<(int, int, bool), int>
        {
            public void Add((int, int, bool) key, int value)
            {
                if (ContainsKey(key))
                {
                    this[key] += value;
                }
                else
                {
                    this[key] = value;
                }
            }

            public long Total()
            {
                return this.Values.Sum();
            }
        }
    }
}
