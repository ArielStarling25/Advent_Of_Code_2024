using AOC_2024_Day1.AOC_Solutions;

namespace AOC_2024_Day1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Select Day:");
            string daySelect = Console.ReadLine();
            switch (daySelect)
            {
                case "D1":
                    day1();
                    break;
                case "D2":
                    day2();
                    break;
                case "D3":
                    day3();
                    break;
                case "D4":
                    day4();
                    break;
                case "D5":
                    day5();
                    break;
                case "D6":
                    day6();
                    break;
                case "D7":
                    day7();
                    break;
                case "D8":
                    day8();
                    break;
                case "D9":
                    day9();
                    break;
                case "D10":
                    day10();
                    break;
                case "D11":
                    day11();
                    break;
                case "D12":
                    day12();
                    break;
                case "D13":
                    day13();
                    break;
                case "D14":
                    day14();
                    break;
                case "D15":
                    day15();
                    break;
                case "D16":
                    day16();
                    break;
                case "D17":
                    day17();
                    break;
                case "D18":
                    day18();
                    break;
                case "D19":
                    day19();
                    break;
                case "D20":
                    day20();
                    break;
                default:
                    Console.WriteLine("Invalid Code");
                    break;
            }
        }

        static void day1()
        {
            AOCD1 day1 = new AOCD1();
            day1.solve2();
        }

        static void day2()
        {
            AOCD2 day2 = new AOCD2();
            day2.solve2();
        }

        static void day3()
        {
            AOCD3 day3 = new AOCD3();
            day3.solve2();
        }

        static void day4()
        {
            AOCD4 day4 = new AOCD4();
            day4.solve2();
        }

        static void day5()
        {
            AOCD5 day5 = new AOCD5();
            //day5.solve1();
            day5.solve2();
            //day5.solve22();
        }

        static void day6()
        {
            AOCD6 day6 = new AOCD6();
            //day6.solve1();
            day6.solve2();
            //day6.test();
        }

        static void day7()
        {
            AOCD7 day7 = new AOCD7();   
            day7.solve12();
            //day7.solve2();
        }

        static void day8()
        {
            AOCD8 day8 = new AOCD8();
            //day8.solve1();
            day8.solve2();
        }

        static void day9()
        {
            AOCD9 day9 = new AOCD9();
            day9.solve1();
            day9.solve2();
            //day9.differenceCheck();
        }

        static void day10()
        {
            AOCD10 day10 = new AOCD10();
            day10.solve1();
            day10.solve2();
        }

        static void day11()
        {
            AOCD11 day11 = new AOCD11();
            //day11.solve1();
            //day11.solve2();
            //day11.solve2Optimised();
            day11.solve2Blazing();
        }

        static void day12()
        {
            AOCD12 day12 = new AOCD12();
            day12.solve1();
        }

        static void day13()
        {
            AOCD13 day13 = new AOCD13();
            //day13.solve1();
            day13.solve2();
        }

        static void day14()
        {
            AOCD14 day14 = new AOCD14();
            //day14.solve1();
            day14.solve2();
        }

        static void day15()
        {
            AOCD15 aOCD15 = new AOCD15(); 
            //aOCD15.solve1();
            aOCD15.solve2();
        }

        static void day16()
        {
            AOCD16 aOCD16 = new AOCD16();
            aOCD16.solve1();
        }

        static void day17()
        {
            AOCD17 aOCD17 = new AOCD17();
            //aOCD17.solve1();
            //aOCD17.solve2();
            aOCD17.solve2b();
        }

        static void day18()
        {
            AOCD18 aOCD18 = new AOCD18();
            //aOCD18.solve1();
            aOCD18.solve2();
        }

        static void day19()
        {
            AOCD19 aOCD19 = new AOCD19();
            aOCD19.solve1();
            aOCD19.solve2();
        }

        static void day20()
        {
            AOCD20 aOCD20 = new AOCD20();
            //aOCD20.solve1();
            //aOCD20.solve2();
            aOCD20.solve2b();
        }
    }
}
