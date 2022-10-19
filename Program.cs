using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FibonacciGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int size = 0;
            int threads = 0;
            string input;

            Console.WriteLine("Welcome to the Fibonacci Generator.");
            Console.WriteLine("\nHow many numbers in the sequence would you like to generate?");
            input = Console.ReadLine();
            while (!Int32.TryParse(input, out size) || size <= 3)
            {
                Console.WriteLine("Please enter an integer larger than 3.");
                input = Console.ReadLine();
            }

            Console.WriteLine("\nHow many threads would you like to use to generate those numbers?");
            input = Console.ReadLine();
            while (!Int32.TryParse(input, out threads) || threads < 1)
            {
                Console.WriteLine("Please enter an integer of at least 1.");
                input = Console.ReadLine();
            }

            //Generate the numbers according to the specifications
            FibonacciGenerator fibonacciGenerator = new FibonacciGenerator(threads, size);

            Console.WriteLine("\nPress the enter key to exit.");
            Console.ReadLine();
        }
    }
}
