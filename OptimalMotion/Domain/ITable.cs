using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface ITable
    {
        void AddRow(ITableRow row);
        void RemoveRow(int id);
        void InsertRow(int id);
        void UpdateRow(int id, ITableRow newRow);
    }
}
