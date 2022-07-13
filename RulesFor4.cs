using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayLife
{
    public class RulesFor4 : IRules
    {
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

            if (HasNeighbourRow(-1))
            {
                neighbours++;
            }

            if (HasNeighbourRow(1))
            {
                neighbours++;
            }

            if (HasNeighbourColumn(-1))
            {
                neighbours++;
            }

            if (HasNeighbourColumn(1))
            {
                neighbours++;
            }


            return neighbours;

            bool HasNeighbourRow(int index)
            {
                var scanPoint = new PointOnBoard
                {
                    Column = (currentColumnIndex + index + totalColumns) % totalColumns,
                    Row = currentRowIndex
                };

                return currentStateOfField[scanPoint.Row, scanPoint.Column] != CellStatus.Empty;
            }

            bool HasNeighbourColumn(int index)
            {
                var scanPoint = new PointOnBoard
                {
                    Column = currentColumnIndex,
                    Row = (currentRowIndex + index + totalRows) % totalRows
                };

                return currentStateOfField[scanPoint.Row, scanPoint.Column] != CellStatus.Empty;
            }
        }
    }
}
