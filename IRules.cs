using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayLife
{
    public interface IRules
    {
        string Description { get; }


        /// <summary>
        /// Определяет, как вычисляется количество соседей. При синхронном способе расчёт ведётся на основе исходного неизменённого массива, 
        /// а результат записывается в новый временный.
        /// При асинхронном подсчёт соседей происходит с учётом ВСЕХ изменений текущего поколения. Каждое изменение записывается в массив и используется при расчёте соседей следующей клетки.
        /// </summary>
        bool IsCalculationAsync { get; set; } 


        CellStatus[,] SurviveDieOrBorn(int totalRows, int totalColumns, CellStatus[,] currentStateOfField);
    }
}
