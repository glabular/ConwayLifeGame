using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConwayLife
{
    public interface IRules
    {
        CellStatus[,] SurviveDieOrBorn(int totalRows, int totalColumns, CellStatus[,] currentStateOfField);
    }
}
