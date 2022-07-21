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
            IRules diagonalRules4 = new RulesForDiagonal4();

            const int StepsPerSecond = 25;

            var size = 25;
            var shift = size + 2 + 20;

            //Field life8 = new Field(size, size, rules8);
            //life8.InitializeLife(Patterns.HollowSquare(31, 2));

            Field life4 = new Field(size, size, rules4);
            life4.InitializeLife(Patterns.Square(15));

            Field life4diag = new Field(size, size, diagonalRules4);
            life4diag.InitializeLife(Patterns.Square(15));


            //life8.Center();
            //Render.Show(life8);

            life4.Center();
            Render.Show(life4, shift);

            life4diag.Center();
            Render.Show(life4diag);

            Console.ReadKey();

            while (life4diag.AllAreDead != true )
            {
                //life8.MakeMove();
                //Render.Show(life8);

                life4.MakeMove();
                Render.Show(life4, shift);

                life4diag.MakeMove();
                Render.Show(life4diag);

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
