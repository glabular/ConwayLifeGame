using System;
using System.Threading;

namespace ConwayLife
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            IRules rules = new RulesFor8();
            rules.IsCalculationAsync = true;
            var renderObject3 = new Render();
            const int StepsPerSecond = 25;

            var size = 51;
            var shift = size + 2 + 20;

            Field neighbours4 = new Field(size, size, rules);
            neighbours4.InitializeLife(Patterns.ObliqueCross(25));

            neighbours4.Center();
            renderObject3.Show(neighbours4);

            Console.ReadKey();

            while (neighbours4.AllAreDead != true )
            {
                neighbours4.MakeMove();
                renderObject3.Show(neighbours4);

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
