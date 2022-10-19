# FibonacciGenerator
This project generates a fibonacci sequence up to a specified size using threading.
This program will generate a fibonacci sequence up to a specified size starting at 0. 
It has methods to generate nth numbers in the sequence as well. This uses the formula Fn = (phi^n - (1-phi)^n) / 5^(1/2), rounding the result to an integer.
The program spins up a specified number of threads and spreads the workload evenly among them, meaning the program can cover more ground in less time. 
accuracy is only limited by the Math.Round method and the BigInteger type, so the numbers can become quite large indeed.

Two of the main challenges faced in this project were accuracy loss and spreading the load evenly over the different threads.
The accuracy loss was initially though to be because of the Math.Round() method, which utilises doubles, but was in fact due to the long integer type. 
The long was subsequently replaced with Numeric's BigInteger.

The even spreading accross the threads was a simple logical wall i had trouble passing:
I could not figure out how to split sizes accross a number of threads that was not a multiple of size.
I evntually opted for an option that calculates an interval for each thread out of the remaining size to be covered and the remaining number of threads to be spun up.
There is a seperate start point variable that tells the program where in the sequence to start calculating.
``` csharp
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
```
