using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2024_Day1.AOC_Solutions
{
    public class AOCD9
    {
        private static readonly string inputFile = "C:\\Users\\user\\Desktop\\Ariel's Folder\\Training\\AdventOfCode_2024\\AOC_2024_Day1\\AOC_Resources\\AOCD9.txt";

        // Part 1
        List<int> rawInput = new List<int>();
        List<InDiskMap> diskMap = new List<InDiskMap>();
        List<DiskBlock> diskBlocks = new List<DiskBlock>();

        // Part 2
        List<DiskFile> diskFiles = new List<DiskFile>();

        public AOCD9()
        {
            init();
        }

        public void solve1()
        {
            long result = 0;
            int idAssign = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            for(int i = 0; i < rawInput.Count-1; i = i + 2)
            {
                InDiskMap diskm = new InDiskMap(idAssign, rawInput[i], rawInput[i+1]);
                Console.WriteLine(diskm.genDiskMapping());
                diskMap.Add(diskm);
                idAssign++;
            }
            if (!isEven(rawInput.Count))
            {
                InDiskMap diskm = new InDiskMap(idAssign, rawInput[rawInput.Count - 1], 0);
                Console.WriteLine(diskm.genDiskMapping());
                diskMap.Add(diskm);
            }
            diskBlocks = constructDiskBlocks();
            printDiskBlocks(diskBlocks);
            diskBlocks = diskBlockCompactor(diskBlocks);
            printDiskBlocks(diskBlocks);
            result = checkSum(diskBlocks);
            timer.Stop();
            Console.WriteLine("Final 1: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        public void solve2()
        {
            long result = 0;
            int idAssign = 0;
            Stopwatch timer = new Stopwatch();
            timer.Start();
            for (int i = 0; i < rawInput.Count - 1; i = i + 2)
            {
                InDiskMap diskm = new InDiskMap(idAssign, rawInput[i], rawInput[i + 1]);
                Console.WriteLine(diskm.genDiskMapping());
                diskMap.Add(diskm);
                idAssign++;
            }
            if (!isEven(rawInput.Count))
            {
                InDiskMap diskm = new InDiskMap(idAssign, rawInput[rawInput.Count - 1], 0);
                Console.WriteLine(diskm.genDiskMapping());
                diskMap.Add(diskm);
            }
            //diskBlocks = constructDiskBlocks();
            diskFiles = constructDiskFiles();
            printDiskFiles(diskFiles);
            diskBlocks = diskFileCompactor(diskFiles);
            Console.WriteLine("DiskBlocksVV");
            printDiskBlocks(diskBlocks);
            result = checkSum2(diskBlocks);
            //diskBlocks = diskBlockCompactor(diskBlocks);
            //printDiskBlocks(diskBlocks);
            //result = checkSum(diskBlocks);
            timer.Stop();
            Console.WriteLine("Final 2: [" + result + "] Finished in:[" + timer.ElapsedMilliseconds + "ms]");
        }

        private List<DiskBlock> constructDiskBlocks()
        {
            List<DiskBlock> construct = new List<DiskBlock>();
            foreach(InDiskMap dm in diskMap)
            {
                List<DiskBlock> temp = dm.genDiskBlocks();
                foreach(DiskBlock bk in temp)
                {
                    construct.Add(bk);
                }
            }
            return construct;
        }

        private List<DiskFile> constructDiskFiles()
        {
            List<DiskFile> construct = new List<DiskFile>();
            foreach (InDiskMap dm in diskMap)
            {
                List<DiskFile> temp = dm.genDiskFiles();
                foreach (DiskFile bf in temp)
                {
                    construct.Add(bf);
                }
            }
            return construct;
        }

        private List<DiskBlock> diskBlockCompactor(List<DiskBlock> construct)
        {
            while (leftMostEmpty(construct) < rightMostAvailable(construct))
            {
                int? obtained = construct[rightMostAvailable(construct)].fileID;
                construct[rightMostAvailable(construct)].fileID = null;
                construct[leftMostEmpty(construct)].fileID = obtained;
            }
            return construct;
        }

        private List<DiskBlock> diskFileCompactor(List<DiskFile> construct)
        {
            List<DiskBlock> res = new List<DiskBlock>();
            //printDiskFiles(construct);
            for(int i = 0; i < construct.Count; i++)
            {
                for(int j = construct.Count-1; j >= 0; j--)
                {
                    if (i <= j)
                    {
                        if (construct[i].fileID == null && construct[j].fileID != null)
                        {
                            if (construct[i].fileID != construct[j].fileID)
                            {
                                if (construct[i].spaceOccupied > construct[j].spaceOccupied) // Place disk at j to i with additional insertion
                                {
                                    DiskFile df1 = new DiskFile(construct[j]);
                                    DiskFile df2 = new DiskFile(null, construct[i].spaceOccupied - construct[j].spaceOccupied);
                                    construct.RemoveAt(i);
                                    construct.Insert(i, df2);
                                    construct[j].fileID = null;
                                    construct.Insert(i, df1);
                                    //printDiskFiles(construct);
                                }
                                else if (construct[i].spaceOccupied == construct[j].spaceOccupied) // Place disk at j to i
                                {
                                    DiskFile df1 = new DiskFile(construct[j]);
                                    construct.RemoveAt(i);
                                    construct.Insert(i, df1);
                                    construct[j].fileID = null;
                                    //printDiskFiles(construct);
                                }
                            }
                        }
                    }
                }
            }
            foreach(DiskFile f in construct)
            {
                List<DiskBlock> bs = f.getDiskBlocks();
                foreach(DiskBlock b in bs)
                {
                    res.Add(b);
                }
            }
            return res;
        }

        private long checkSum(List<DiskBlock> construct)
        {
            long result = 0;
            List<long> longs = new List<long>();
            foreach(DiskBlock item in construct)
            {
                if(item.fileID != null)
                {
                    longs.Add((long)item.fileID);
                }
            }
            for (int i = 0; i < longs.Count; i++)
            {
                Console.Write(longs[i]);
                result += (i * longs[i]);
            }
            Console.WriteLine();
            return result;
        }

        private long checkSum2(List<DiskBlock> construct)
        {
            long result = 0;
            for(int i = 0; i < construct.Count; i++)
            {
                if (construct[i].fileID != null)
                {
                    result += (long)(i * construct[i].fileID);
                }
            }
            Console.WriteLine();
            return result;
        }

        private int leftMostEmpty(List<DiskBlock> construct)
        {
            int val = construct.Count;
            for (int i = 0; i < construct.Count; i++)
            {
                if (construct[i].fileID == null)
                {
                    val = i;
                    break;
                }
            }
            return val;
        }

        private int leftMostEmpty(List<DiskFile> construct)
        {
            int val = construct.Count;
            for (int i = 0; i < construct.Count; i++)
            {
                if (construct[i].fileID == null)
                {
                    val = i;
                    break;
                }
            }
            return val;
        }

        private int rightMostAvailable(List<DiskBlock> construct)
        {
            int val = -1;
            for (int i = construct.Count - 1; i >= 0; i--)
            {
                if (construct[i].fileID != null)
                {
                    val = i;
                    break;
                }
            }
            return val;
        }

        private int rightMostAvailable(List<DiskFile> construct)
        {
            int val = -1;
            for (int i = construct.Count - 1; i >= 0; i--)
            {
                if (construct[i].fileID != null)
                {
                    val = i;
                    break;
                }
            }
            return val;
        }

        private void printDiskBlocks(List<DiskBlock> db)
        {
            foreach (DiskBlock block in db)
            {
                if(block.fileID != null) Console.Write(block.fileID);
                if (block.fileID == null) Console.Write('.');
            }
            Console.WriteLine();
        }

        private void printDiskFiles(List<DiskFile> df)
        {
            foreach(DiskFile f in df)
            {
                Console.Write(f.ToString());
            }
            Console.WriteLine();
        }

        internal class InDiskMap
        {
            public int fileID {  get; set; }
            public int files {  get; set; }
            public int free { get; set; }

            public InDiskMap(int inid, int infiles, int infree)
            {
                fileID = inid;
                files = infiles;
                free = infree;
            }

            public string genDiskMapping()
            {
                string val = "";
                for(int i = 0; i < files; i++)
                {
                    val += fileID + "";
                }
                for(int i = 0;i < free; i++)
                {
                    val += ".";
                }
                return val;
            }

            public List<DiskBlock> genDiskBlocks()
            {
                string str = genDiskMapping();
                List<DiskBlock> blockList = new List<DiskBlock>();
                for(int i = 0; i < files; i++)
                {
                    DiskBlock block = new DiskBlock(fileID);
                    blockList.Add(block);   
                }
                for(int i = 0; i < free; i++)
                {
                    DiskBlock block = new DiskBlock(null);
                    blockList.Add(block);
                }
                return blockList;
            }

            public List<DiskFile> genDiskFiles()
            {
                List<DiskFile> diskFiles = new List<DiskFile>();
                DiskFile diskf1 = new DiskFile(fileID, files);
                DiskFile diskf2 = new DiskFile(null, free);
                diskFiles.Add(diskf1);
                diskFiles.Add(diskf2);
                return diskFiles;
            }
        }

        internal class DiskBlock
        {
            public int? fileID { get; set; }

            public DiskBlock(int? in1)
            {
                fileID = in1;
            }
        }

        internal class DiskFile
        {
            public int? fileID { get; set; }
            public int spaceOccupied { get; set; }

            public DiskFile(int? in1, int spaceOccupied)
            {
                fileID = in1;
                this.spaceOccupied = spaceOccupied;
            }

            public DiskFile(DiskFile copy)
            {
                fileID = copy.fileID; 
                spaceOccupied = copy.spaceOccupied;
            }

            public List<DiskBlock> getDiskBlocks()
            {
                List<DiskBlock> bk = new List<DiskBlock>();
                for(int i = 0; i < spaceOccupied; i++)
                {
                    DiskBlock bl = new DiskBlock(fileID);
                    bk.Add(bl);
                }
                return bk;
            }

            public override string ToString()
            {
                string str = "|";
                for(int i = 0; i < spaceOccupied; i++)
                {
                    if(fileID == null)
                    {
                        str += ".";
                    }
                    else
                    {
                        str += fileID;
                    }
                }
                return str;
            }
        }

        private bool isEven(int n)
        {
            return (n % 2 == 0);
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
                    foreach(char num in chars)
                    {
                        rawInput.Add(num-'0'); // WTH IS THIS C#??
                    }
                    line = sr.ReadLine();
                }
                Console.WriteLine("| " + count + " lines read");
                sr.Close();
                Console.WriteLine("| AOC Day 9 Resources Initialized! ");
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
