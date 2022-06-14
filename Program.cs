using System;
using System.Threading;

namespace ConwayLife
{
    public class Program
    {
        static void Main(string[] args)
        {
            var size = 25;
            Field life = new Field(25, 100);
            life.InitializeLife(Patterns.GliderGun);

            //life.SetImmortalCell(4, 4);

            life.Center();
            life.Show();
            Console.ReadKey();


            do
            {
                life.Show();
                life.SurviveDieOrBorn();
                Thread.Sleep(150);

            } while (life.allDead != true);
        }
    }
}
