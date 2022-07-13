using System;
using System.Threading;

namespace ConwayLife
{
    public class Program
    {
        static void Main(string[] args)
        {
            IRules rules = new RulesFor4();

            var size = 31;
            Field life = new Field(size, size, rules);
            life.InitializeLife(Patterns.ObliqueCross(15));
            //life.SetImmortalCell(4, 4);

            life.Center();
            life.Show();
            
            Console.ReadKey();

            while (life.AllAreDead != true)
            {
                if (life.CycleAchieved)
                {
                    Console.WriteLine($"The life got cycled on generation №{life.Generation}.");
                    Console.WriteLine($"The length of the cycle: {life.CycleLength}.");
                }

                Thread.Sleep(100);

                life.MakeMove();
            } 

            if (life.AllAreDead)
            {
                Console.WriteLine($"The life got extinct on generation №{life.Generation}.");
                Console.Write("Press any key...");
                Console.ReadKey();
            }
        }
    }
}
