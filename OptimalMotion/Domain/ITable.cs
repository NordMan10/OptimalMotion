using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMoving.Domain
{
    public interface ITable
    {
        void AddRow(ITableRowCreationData row);
        void RemoveRow(int id);
        void UpdateRow(int id, ITableRow newRow);
        void Reset();
    }
}
