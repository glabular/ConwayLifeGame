using System;
using System.Threading;

namespace ConwayLife
{
    public class Program
    {
        static void Main(string[] args)
        {
            IRules rules = new RulesFor8();
            var shift = 0;
            const int StepsPerSecond = 25;

            var size = 30;
            Field life = new Field(size, size * 7, rules);
            life.InitializeLife(Patterns.GliderGun);
            //life.SetImmortalCell(4, 4);

            life.Center();
            Render.Show(life, shift);
            Console.ReadKey();

            while (life.AllAreDead != true)
            {
                life.MakeMove();
                Render.Show(life, shift);
                Sleep(StepsPerSecond, life);
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
