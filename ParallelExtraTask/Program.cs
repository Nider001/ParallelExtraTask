using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelExtraTask
{
    class Program
    {
        static void Main(string[] args)
        {
            int maxNumber = 0; // [1, ..., maxNumber]
            int threadCount = 0;
            int sectionCount = 0;

            do
            {
                Console.Clear();
                Console.Write("Enter max number to process: ");
            }
            while (!int.TryParse(Console.ReadLine(), out maxNumber) || maxNumber < 1) ;

            do
            {
                Console.Clear();
                Console.Write("Enter thread count: ");
            }
            while (!int.TryParse(Console.ReadLine(), out threadCount) || threadCount < 1);

            do
            {
                Console.Clear();
                Console.Write("Enter section count: ");
            }
            while (!int.TryParse(Console.ReadLine(), out sectionCount) || sectionCount < 1);

            Console.Clear();

            SortedList<int, KeyValuePair<bool, int?>?> results = new SortedList<int, KeyValuePair<bool, int?>?>(maxNumber);

            for (int i = 1; i <= maxNumber; i++)
            {
                results.Add(i, null);
            }

            List<List<int>> sections = results.Keys.GroupBy(x => x % sectionCount).Select(x => x.ToList()).ToList();

            ThreadPool.SetMinThreads(threadCount, threadCount); // force exact thread count
            ThreadPool.SetMaxThreads(threadCount, threadCount); // force exact thread count
            Parallel.ForEach(sections,
            new ParallelOptions
            {
                MaxDegreeOfParallelism = threadCount // force exact thread count
            },
            section =>
            {
                foreach (int number in section)
                {
                    results[number] = new KeyValuePair<bool, int?>(number % 3 == 0, Task.CurrentId); // check if the number is divisible by 3
                }
            });

            foreach (var item in results)
            {
                Console.WriteLine("{0, 10} | {1} | {2}", item.Key, item.Value.Value.Key, item.Value.Value.Value.Value);
            }

            Console.ReadKey();
        }
    }
}
