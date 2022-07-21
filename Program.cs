using System;
using System.Threading;

namespace ConwayLife
{
    public class Program
    {
        static void Main(string[] args)
        {
            IRules rules8 = new RulesFor8();
            IRules rules4 = new RulesFor4();

            const int StepsPerSecond = 25;

            var size = 51;
            var shift = size + 2 + 20;

            Field life8 = new Field(size, size, rules8);
            life8.InitializeLife(Patterns.HollowSquare(31, 2));

            Field life4 = new Field(size, size, rules4);
            life4.InitializeLife(Patterns.HollowSquare(31, 2));
            //life.SetImmortalCell(4, 4);

            life8.Center();
            Render.Show(life8);

            life4.Center();
            Render.Show(life4, shift);

            Console.ReadKey();

            while (life8.AllAreDead != true )
            {
                life8.MakeMove();
                Render.Show(life8);

                life4.MakeMove();
                Render.Show(life4, shift);

                Thread.Sleep(200);
                //Sleep(StepsPerSecond, life8);
            }
        }

        private static void Sleep(int stepsPerSecond, Field life)
        {
            const int MicrosecondsPerCell = 61;

            var oneStepDuratonMicroseconds = 1000 * 1000 / stepsPerSecond;
            var renderTimeMicroseconds = MicrosecondsPerCell * life.ChangedCellsConut;
            var sleepTimeMicroseconds = oneStepDuratonMicroseconds - renderTimeMicroseconds;

            if (sleepTimeMicroseconds > 0)
            {
                long sleepTimeTicks = sleepTimeMicroseconds * 10;
                var sleepDuration = new TimeSpan(sleepTimeTicks);
                Thread.Sleep(sleepDuration);
            }
        }
    }
}
