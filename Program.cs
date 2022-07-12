using System;
using System.Threading;

namespace ConwayLife
{
    public class Program
    {
        static void Main(string[] args)
        {
            var size = 51;
            Field life = new Field(size, size, SurviveDieOrBorn8, GetNeighboursNumber8);
            life.InitializeLife(Patterns.ObliqueCross(25));
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

        public static CellStatus[,] SurviveDieOrBorn8(
            int totalRows,
            int totalColumns,
            CellStatus[,] currentStateOfField,
            Func<int, int, int, int, CellStatus[,], int> getNumberOfNeighbours)
        {
            CellStatus[,] temporaryStateOfField = new CellStatus[totalRows, totalColumns];

            for (int currentRowIndex = 0; currentRowIndex < totalRows; currentRowIndex++)
            {
                for (int currentColumnIndex = 0; currentColumnIndex < totalColumns; currentColumnIndex++)
                {
                    var numberOfNeighbours = getNumberOfNeighbours(currentRowIndex, currentColumnIndex, totalRows, totalColumns, currentStateOfField);

                    switch (numberOfNeighbours)
                    {
                        case 2: // Если у клетки 2 соседа
                            // Оставить состояние клетки таким же, как и было.
                            temporaryStateOfField[currentRowIndex, currentColumnIndex] = currentStateOfField[currentRowIndex, currentColumnIndex]; 
                            break;
                        case 3: // Если у клетки 3 соседа
                            // Сделать или оставить клетку живой.
                            temporaryStateOfField[currentRowIndex, currentColumnIndex] = CellStatus.Alive;
                            break;
                        default: // Во всех остальных случаях - убить клетку или оставить её пустой.
                            temporaryStateOfField[currentRowIndex, currentColumnIndex] = CellStatus.Empty;
                            break;
                    }
                }
            }

            return temporaryStateOfField;
        }

        private static int GetNeighboursNumber8(int currentRowIndex, int currentColumnIndex, int totalRows, int totalColumns, CellStatus[,] currentStateOfField)
        {
            int neighbours = 0;
            foreach (var x in new[] { -1, 0, 1 })
            {
                foreach (var y in new[] { -1, 0, 1 })
                {
                    var scanPoint = new PointOnBoard
                    {
                        Column = (currentColumnIndex + x + totalColumns) % totalColumns,
                        Row = (currentRowIndex + y + totalRows) % totalRows
                    };

                    if (IsPointValid(scanPoint)) // если проверяемая точка не является равной текущей точке.
                    {
                        if (currentStateOfField[scanPoint.Row, scanPoint.Column] != CellStatus.Empty)
                        {
                            neighbours++;
                        }
                    }
                }
            }

            return neighbours;

            bool IsPointValid(PointOnBoard scanPoint)
            {
                return !(scanPoint.Column == currentColumnIndex && scanPoint.Row == currentRowIndex);
            }
        }
    }
}
