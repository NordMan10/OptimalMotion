using System.ComponentModel;
using System.Windows.Forms;

namespace OptimalMoving.Domain
{
    public class Table : ITable
    {
        public Table(DataGridView graphicBase)
        {
            this.graphicBase = graphicBase;
            data = new BindingList<ITableRow>();

            this.graphicBase.DataSource = data;
        }

        private readonly DataGridView graphicBase;
        private readonly BindingList<ITableRow> data;

        public void AddRow(ITableRowCreationData rowCreationData)
        {
            // Принимаем данные, необходимые для создания строки;
            var tableRowCreationData = rowCreationData;

            // Создаем { Строку таблицы} через метод(1);
            var tableRow = GetTableRow(tableRowCreationData);

            // Добавляем ее в список;
            data.Add(tableRow);
        }

        public void RemoveRow(int id)
        {
            // Принимаем Id {Строки таблицы};
            var rowId = id;

            // Получаем индекс {Строки таблицы} в списке;
            var rowIndex = GetTableRowIndexById(rowId);

            // Удаляем строку из списка;
            data.RemoveAt(rowIndex);
        }

        public void UpdateRow(int id, ITableRow newRow)
        {
            // Принимаем Id {Строки таблицы};
            var rowId = id;

            // Принимаем новую строку с новыми значениями;
            var updatedRow = newRow;

            // Получаем индекс {Строки таблицы} в списке;
            var rowIndex = GetTableRowIndexById(rowId);

            // Удаляем старую строку;
            data.RemoveAt(rowIndex);

            // Заменяем старую строку на новую;
            data.Insert(rowIndex, updatedRow);
        }

        public void Reset()
        {
            data.Clear();
        }

        private ITableRow GetTableRow(ITableRowCreationData data)
        {
            // Получаем все данные, кроме Id строки;
            var creationData = data;

            // Получаем Id как: количество строк в таблице + 1;
            var rowId = this.data.Count + 1;

            // Создаем { Строку таблицы};
            var tableRow = new TableRow(rowId, creationData.AircraftId, 
                creationData.EngineStartMoment, creationData.IsPlannedMomentFeasible);

            // Возвращаем;
            return tableRow;
        }

        private int GetTableRowIndexById(int id)
        {
            // Получаем Id {Строки таблицы};
            var rowId = id;

            // Возвращаем Id -1;
            return id - 1;
        }
    }
}
