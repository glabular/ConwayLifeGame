using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayLife
{
    public class Render
    {
        private bool _areBordersDrawn = false;
        public void Show(Field field, int horizontalShift = 0)
        {
            Console.CursorVisible = false;

            if (!_areBordersDrawn)
            {
                PrintBorders(field.Rows, field.Columns, horizontalShift); 
            }

            for (int i = 0; i < field.Rows; i++)
            {
                for (int j = 0; j < field.Columns; j++)
                {
                    char cellSymbol = ' ';

                    if (field.ChangedCells[i, j])
                    {
                        switch (field.CurrentStateOfField[i, j])
                        {
                            case CellStatus.Alive:
                                cellSymbol = Field.AliveCellSymbol;
                                break;
                            case CellStatus.Immortal:
                                cellSymbol = Field.ImmortalCellSymbol;
                                break;
                            case CellStatus.Empty:
                                cellSymbol = Field.EmptyCellSymbol;
                                break;
                        }

                        WriteAt(cellSymbol, i + 1, j + 1, horizontalShift);
                    }
                }
            }

            var indent = 3;
            WriteAt($"{field.RulesDescription}", field.Rows + indent++, 0, horizontalShift);
            WriteAt("---Statistics---", field.Rows + indent++, 0, horizontalShift);
            WriteAt($"Current generation: {field.Generation}", field.Rows + indent++, 0, horizontalShift);
            WriteAt($"Alive cells: {field.AliveCells}     ", field.Rows + indent++, 0, horizontalShift);

            if (field.CycleAchieved)
            {
                WriteAt($"The life got cycled on generation №{field.Generation}.", field.Rows + indent++, 0, horizontalShift);
                WriteAt($"The length of the cycle: {field.CycleLength}.", field.Rows + indent++, 0, horizontalShift);
            }

            if (field.AllAreDead)
            {
                WriteAt($"The life got extinct on generation №{field.Generation}.", field.Rows + indent++, 0, horizontalShift);
            }
        }

        private void WriteAt(char c, int row, int column, int horizontalShift = 0)
        {
            WriteAt($"{c}", row, column, horizontalShift);            
        }

        private void WriteAt(string s, int row, int column, int horizontalShift = 0)
        {
            try
            {
                Console.SetCursorPosition(column + horizontalShift, row); // TODO обработать исключение.
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.Clear();
                Console.WriteLine("The console window must be fullscreen. ");
                Console.WriteLine("Maximize the window and press Enter.");
                Console.ReadKey();
                Console.Clear();
                _areBordersDrawn = false;
            }
        }

        private void PrintBorders(int rows, int columns, int horizontalShift = 0, Field field = null)
        {
            _areBordersDrawn = true;
            var horizontalBorder = new string(Field.HorizontalBorderSymbol, columns + 2);

            WriteAt(horizontalBorder, 0, 0, horizontalShift);

            for (int i = 1; i < rows + 1; i++)
            {
                WriteAt('|', i, 0, horizontalShift);
                WriteAt('|', i, columns + 1, horizontalShift);
            }

            WriteAt(horizontalBorder, rows + 1, 0, horizontalShift);                        
        }
    }
}
