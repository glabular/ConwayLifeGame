using System;
using System.Threading;

namespace ConwayLife
{
    public class Program
    {
        static void Main(string[] args)
        {
            
            IRules ruleOf16 = new RulesFor16();
            var renderObject3 = new Render();
            const int StepsPerSecond = 25;

            var size = 25;
            var shift = size + 2 + 20;

            Field neighbours16 = new Field(size, size, ruleOf16);
            neighbours16.InitializeLife(Patterns.Square(3));

            neighbours16.Center();
            renderObject3.Show(neighbours16);

            Console.ReadKey();

            while (neighbours16.AllAreDead != true )
            {
                neighbours16.MakeMove();
                renderObject3.Show(neighbours16);

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
