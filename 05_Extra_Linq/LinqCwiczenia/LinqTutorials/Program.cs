using System;
using System.Collections;

namespace LinqTutorials
{
    class Program
    {
        static void Main(string[] args)
        {
            var t1 = LinqTasks.Task1();
            var t2 = LinqTasks.Task2();
            var t3 = LinqTasks.Task3();
            var t4 = LinqTasks.Task4();
            var t5 = LinqTasks.Task5();
            var t6 = LinqTasks.Task6();
            var t7 = LinqTasks.Task7();
            var t8 = LinqTasks.Task8();
            var t9 = LinqTasks.Task9();
            var t10 = LinqTasks.Task10();
            var t11 = LinqTasks.Task11();
            var t12 = LinqTasks.Task12();
            
            int[] testArray = new int[] { 1, 1, 1, 1, 1, 1, 10, 1, 1, 1, 1 };
            var t13 = LinqTasks.Task13(testArray); 
            
            var t14 = LinqTasks.Task14();
            var t15 = LinqTasks.Task15(); 

            PrintResult("Task 1", t1);
            PrintResult("Task 2", t2);
            PrintResult("Task 3", t3);
            PrintResult("Task 4", t4);
            PrintResult("Task 5", t5);
            PrintResult("Task 6", t6);
            PrintResult("Task 7", t7);
            PrintResult("Task 8", t8);
            PrintResult("Task 9", t9);
            PrintResult("Task 10", t10);
            PrintResult("Task 11", t11);
            PrintResult("Task 12", t12);
            PrintResult("Task 13", t13);
            PrintResult("Task 14", t14);
            PrintResult("Task 15", t15);
        }
        
        static void PrintResult(string taskName, object result)
        {
            Console.WriteLine($"=== {taskName} ===");
            
            if (result is IEnumerable collection && !(result is string))
            {
                foreach (var item in collection)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine(result);
            }
            
            Console.WriteLine(); 
        }
    }
}