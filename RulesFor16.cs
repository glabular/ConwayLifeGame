using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayLife
{
    public class RulesFor16 : IRules
    {
        public string Description => "Sixteen neighbours.";

        public bool IsCalculationAsync { get; set; }

        public CellStatus[,] SurviveDieOrBorn(
            int totalRows,
            int totalColumns,
            CellStatus[,] currentStateOfField)
        {
            CellStatus[,] temporaryStateOfField = new CellStatus[totalRows, totalColumns];

            for (int currentRowIndex = 0; currentRowIndex < totalRows; currentRowIndex++)
            {
                for (int currentColumnIndex = 0; currentColumnIndex < totalColumns; currentColumnIndex++)
                {
                    var numberOfNeighbours = GetNeighboursNumber(currentRowIndex, currentColumnIndex, totalRows, totalColumns, currentStateOfField);

                    if (numberOfNeighbours > 5 && numberOfNeighbours < 10)
                    {
                        temporaryStateOfField[currentRowIndex, currentColumnIndex] = CellStatus.Alive;
                    }
                    else if (numberOfNeighbours > 9 && numberOfNeighbours < 13)
                    {
                        temporaryStateOfField[currentRowIndex, currentColumnIndex] = currentStateOfField[currentRowIndex, currentColumnIndex];
                    }
                    else
                    {
                        temporaryStateOfField[currentRowIndex, currentColumnIndex] = CellStatus.Empty;
                    }
                }
            }

            return temporaryStateOfField;
        }

        public int GetNeighboursNumber(int currentRowIndex, int currentColumnIndex, int totalRows, int totalColumns, CellStatus[,] currentStateOfField)
        {
            int neighbours = 0;
            foreach (var x in new[] { -2, -1, 0, 1, 2 })
            {
                foreach (var y in new[] { -2, -1, 0, 1, 2 })
                {
                    var scanPoint = new PointOnBoard
                    {
                        Row = (currentRowIndex + y + totalRows) % totalRows,
                        Column = (currentColumnIndex + x + totalColumns) % totalColumns                        
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
