using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayLife
{
    public class Field
    {
        private const char VerticalBorderSymbol = '║';
        private const char HorizontalBorderSymbol = '=';
        private const char AliveCellSymbol = 'X'; // '¤' '■'
        private const char ImmortalCellSymbol = '#'; //
        private const char EmptyCellSymbol = ' '; //

        private int _rows;
        private int _columns;
        private CellStatus[,] _currentStateOfField;
        private CellStatus[,] _temporaryStateOfField;
        private long _generation = 0;
        public bool allDead;


        public Field(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            _currentStateOfField = new CellStatus[_rows, _columns];
            _temporaryStateOfField = new CellStatus[_rows, _columns];
        }

        public void InitializeLife(string generationZeroPattern)
        {
            var lines = generationZeroPattern.Split("\r\n").ToList();
            lines.RemoveAt(0);
            lines.RemoveAt(lines.Count - 1);
            lines = lines.Take(Math.Min(lines.Count, _rows)).ToList();

            var row = 0;

            foreach (var line in lines)
            {
                var column = 0;
                var cutLine = line.Substring(0, Math.Min(_columns, line.Length));

                foreach (var symbol in cutLine.ToUpper())
                {
                    switch (symbol)
                    {
                        case AliveCellSymbol:
                            _currentStateOfField[row, column] = CellStatus.Alive;
                            break;
                        case ImmortalCellSymbol:
                            _currentStateOfField[row, column] = CellStatus.Immortal;
                            break;
                        default:
                            _currentStateOfField[row, column] = CellStatus.Empty;
                            break;
                    }

                    column++;
                }

                row++;
            }
        }

        public void InitializeLife(params PointOnBoard[] pointWithLife)
        {
            // Заполнение массива принятыми точками при инициализации жизни в главной программе.
            foreach (var point in pointWithLife)
            {
                _currentStateOfField[point.Row, point.Column] = CellStatus.Alive;
            }

            //for (int i = 0; i < 10; i++)
            //{
            //    for (int j = 0; j < 20; j++)
            //    {
            //        _currentStateOfField[i, j] = true;
            //    }
            //}


        }

        public void SurviveDieOrBorn()
        {
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    var numberOfNeighbours = GetNeighboursNumber(i, j);
                    switch (_currentStateOfField[i, j])
                    {
                        case CellStatus.Alive:
                            if (numberOfNeighbours == 2 || numberOfNeighbours == 3)
                            {
                                _temporaryStateOfField[i, j] = CellStatus.Alive;
                            }
                            else
                            {
                                _temporaryStateOfField[i, j] = CellStatus.Empty;
                            }

                            break;

                        case CellStatus.Empty:
                            if (numberOfNeighbours == 3)
                            {
                                _temporaryStateOfField[i, j] = CellStatus.Alive;
                            }

                            break;

                        case CellStatus.Immortal:
                            _temporaryStateOfField[i, j] = CellStatus.Immortal;
                            break;
                    }
                }
            }

            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    _currentStateOfField[i, j] = _temporaryStateOfField[i, j];
                }
            }

            _generation++;
        }

        public void SetImmortalCell(int row, int column)
        {
            _currentStateOfField[row, column] = CellStatus.Immortal;
        }

        private int GetNeighboursNumber(int row, int column)
        {
            int neighbours = 0;
            foreach (var x in new[] { -1, 0, 1 })
            {
                foreach (var y in new[] { -1, 0, 1 })
                {
                    var scanPoint = new PointOnBoard
                    {
                        Column = (column + x + _columns) % _columns,
                        Row = (row + y + _rows) % _rows
                    };

                    if (IsPointValid(scanPoint)) // если проверяемая точка не является равной текущей точке.
                    {
                        if (_currentStateOfField[scanPoint.Row, scanPoint.Column] != CellStatus.Empty)
                        {
                            neighbours++;
                        }
                    }
                }
            }

            return neighbours;

            bool IsPointValid(PointOnBoard scanPoint)
            {
                return !(scanPoint.Column == column && scanPoint.Row == row);
            }
        }

        public void Show()
        {
            Console.Clear();
            PrintHorizontalBorder();

            for (int i = 0; i < _rows; i++)
            {
                PrintLine(i);
            }

            PrintHorizontalBorder();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("---Statistics---");
            Console.WriteLine($"Current generation: {_generation}");
            Console.WriteLine($"Alive cells: {NumberOfObjects()}");

            void PrintLine(int rowNumber)
            {
                Console.Write(VerticalBorderSymbol);

                for (int i = 0; i < _columns; i++)
                {
                    switch (_currentStateOfField[rowNumber, i])
                    {
                        case CellStatus.Alive:
                            Console.Write(AliveCellSymbol);
                            break;
                        case CellStatus.Immortal:
                            Console.Write(ImmortalCellSymbol);
                            break;
                        case CellStatus.Empty:
                            Console.Write(EmptyCellSymbol);
                            break;
                    }
                }

                Console.Write(VerticalBorderSymbol);
                Console.WriteLine();
            }

            void PrintHorizontalBorder()
            {
                Console.WriteLine(new string(HorizontalBorderSymbol, _columns + 2));
            }

            int NumberOfObjects()
            {
                var number = 0;
                foreach (var item in _currentStateOfField)
                {
                    if (item != CellStatus.Empty)
                    {
                        number++;
                    }
                }

                if (number == 0)
                {
                    allDead = true;
                }

                return number;
            }
        }

        public void Center()
        {
            var rightShift = (_columns - GetVerticalBound()) / 2;
            var downShift = (_rows - GetHorizontalBound()) / 2;

            ShiftToRight(rightShift);
            ShiftDown(downShift);

            int GetVerticalBound()
            {
                int verticalBound = 0;
                int row = 0;
                int falses = 0;

                do
                {
                    falses = 0;

                    for (int i = 0; i < _columns; i++) // пройти по всем элементам в ряду.
                    {
                        if (_currentStateOfField[row, i] == CellStatus.Alive)
                        {
                            falses = 0;
                        }
                        else
                        {
                            falses++;
                        }
                    }

                    var oneRowLength = _columns - falses;
                    verticalBound = oneRowLength > verticalBound ? oneRowLength : verticalBound;
                    row++;

                } while (row <= _rows - 1);

                return verticalBound;
            }

            int GetHorizontalBound()
            {
                int horizontalBound = 0;
                int column = 0;
                int falses = 0;

                do
                {
                    falses = 0;

                    for (int i = 0; i < _rows; i++) // пройти по всем элементам в колонке.
                    {
                        if (_currentStateOfField[i, column] == CellStatus.Alive)
                        {
                            falses = 0;
                        }
                        else
                        {
                            falses++;
                        }
                    }

                    var oneRowLength = _rows - falses;
                    horizontalBound = oneRowLength > horizontalBound ? oneRowLength : horizontalBound;
                    column++;

                } while (column <= _columns - 1);

                return horizontalBound;
            }
        }

        public void ShiftToRight(int n)
        {
            for (int i = 0; i < _rows; i++) // очистка временного массива
            {
                for (int j = 0; j < _columns; j++)
                {
                    _temporaryStateOfField[i, j] = CellStatus.Empty;
                }
            }

            for (int i = 0; i < _rows; i++) // Запись во временный массив новых координат с учётом сдвига.
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_currentStateOfField[i, j] != CellStatus.Empty)
                    {
                        _temporaryStateOfField[i, j + n] = _currentStateOfField[i, j];
                    }
                }
            }

            for (int i = 0; i < _rows; i++) // запись из временного массива в текущий.
            {
                for (int j = 0; j < _columns; j++)
                {
                    _currentStateOfField[i, j] = _temporaryStateOfField[i, j];
                }
            }
        }

        public void ShiftDown(int n)
        {
            for (int i = 0; i < _rows; i++) // очистка временного массива
            {
                for (int j = 0; j < _columns; j++)
                {
                    _temporaryStateOfField[i, j] = CellStatus.Empty;
                }
            }

            for (int i = 0; i < _rows; i++) // Запись во временный массив новых координат с учётом сдвига.
            {
                for (int j = 0; j < _columns; j++)
                {
                    if (_currentStateOfField[i, j] != CellStatus.Empty)
                    {
                        _temporaryStateOfField[i + n, j] = _currentStateOfField[i, j];
                    }
                }
            }

            for (int i = 0; i < _rows; i++) // запись из временного массива в текущий.
            {
                for (int j = 0; j < _columns; j++)
                {
                    _currentStateOfField[i, j] = _temporaryStateOfField[i, j];
                }
            }
        }
    }
}
