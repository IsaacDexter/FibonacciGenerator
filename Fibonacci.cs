using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;

namespace FibonacciGenerator
{
    /// <summary><para>This class will generate a fibonacci sequence up to a specified size starting at 0. <br/> 
    /// It has methods to generate nth numbers in the sequence as well. This uses the formula Fn = (phi^n - (1-phi)^n) / 5^(1/2), rounding the result to an integer. <br/>
    /// The program spins up a specified number of threads and spreads the workload evenly among them, meaning the program can cover more ground in less time. <br/>
    /// accuracy is only limited by the Math.Round method and the BigInteger type, so the numbers can become quite large indeed.
    /// </para></summary>
    public class FibonacciGenerator
    {
        private BigInteger[] sequence;
        private double size;
        private double threads;
        private List<Thread> threadsList;
        /// <summary>
        /// The golden ratio, used to calculate fibonacci numbers
        /// </summary>
        private double goldenRatio;

        /// <summary> <para>
        /// Spins up Threads threads of near equal size that cover the range defined in size. <br/> 
        /// These threads all calculate the fibonacci sequence within their respective boundaries.
        /// </para></summary>
        /// <param name="threads">The number of threads to spin up. Default = 10.</param>
        /// <param name="size">The total of number of figures of the fibonacci sequence to be calculated across all threads. Default = 30</param>
        public FibonacciGenerator(int threads = 3, int size = 30)
        {
            sequence = new BigInteger[size];
            this.size = size;
            this.threads = threads;
            threadsList = new List<Thread>();

            //Calculate the golden ratio, given by 1/2 + root(5)/2
            goldenRatio = ((Math.Sqrt(5) / 2) + 0.5);

            int interval;
            int startPoint = 0;

            //Wind up this.threads threads. 
            for (int i = 0; i < this.threads; i++)
            {
                //Set the intervals. Each thread calculates an interval for it to work on, to evenly distribute the workload and allow for sizes that do not have threads as common denominator
                interval = (int)Math.Round(this.size / (this.threads - i));
                //reduce this.size so the interval can be recalculated correctly
                this.size -= interval;

                //Add the new thread starting at start point and ending and start point + interval
                threadsList.Add(new Thread(new ThreadStart(() => GenerateFibonacciSequence(startPoint, interval))));
                threadsList.Last<Thread>().Start();
                threadsList.Last<Thread>().Join();

                //Increase the start point so the next thread winds up where the other left off.
                startPoint += interval;
            }
            //Output the fibonacci sequence now the threads have done their thing
            PrintSequence();
        }
        /// <summary>
        /// Generates the fibonnacci between startingnum2 and size
        /// </summary>
        public void GenerateFibonacciSequence(int startPos, int size)
        {
            sequence[startPos] = GenerateFibonacciNumber(startPos);
            sequence[startPos + 1] = GenerateFibonacciNumber(startPos + 1);
            for (int i = startPos + 2; i < startPos + size; i++)
            {
                sequence[i] = sequence[i - 1] + sequence[i - 2];
            }
        }
        /// <summary>
        /// Used to find a specific (n+1th) number in the fibonacci sequence. Uses BASE 0 for n
        /// </summary>
        /// <param name="n">The index of the number you want from the fibonacci sequence, AS BASE ZERO! For example, to get the 7th number (13), n would be 6.</param>
        /// <returns>The n+1th number of the fibonacci sequence.</returns>
        public long GenerateFibonacciNumber(int n)
        {
            //Find the nth fibonacci number using the formula Fn = (phi^n - (1-phi)^n) / 5^(1/2) and rounding it.
            return (long)Math.Round((Math.Pow(goldenRatio, n + 1) - Math.Pow(1 - goldenRatio, n + 1)) / Math.Sqrt(5));

        }

        /// <summary>
        /// Outputs the fibonacci sequence generated
        /// </summary>
        public void PrintSequence()
        {
            string outString = "";
            foreach (BigInteger item in sequence)
            {
                outString += item + ", ";
            }
            Console.WriteLine(outString);
        }
    }
}
