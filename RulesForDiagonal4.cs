using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayLife
{
    public class RulesForDiagonal4 : IRules
    {
        public string Description => "Four diagonal neighbours.";

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

                    switch (numberOfNeighbours)
                    {
                        case 3: // Если у клетки 3 соседа
                            // Оставить состояние клетки таким же, как и было.
                            temporaryStateOfField[currentRowIndex, currentColumnIndex] = currentStateOfField[currentRowIndex, currentColumnIndex];
                            break;
                        case 2: // Если у клетки 2 соседа
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

        private int GetNeighboursNumber(int currentRowIndex, int currentColumnIndex, int totalRows, int totalColumns, CellStatus[,] currentStateOfField)
        {
            int neighbours = 0;
            foreach (var x in new[] { -1, 1 })
            {
                foreach (var y in new[] { -1, 1 })
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

