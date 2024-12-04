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

        }
    }
}
