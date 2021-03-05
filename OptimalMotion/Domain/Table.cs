using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace OptimalMoving.Domain
{
    public class Table : ITable
    {
        public Table(DataGridView graphicBase)
        {
            this.graphicBase = graphicBase;
            this.graphicBase.DataBindingComplete += GraphicBaseOnDataBindingComplete;
            data = new BindingList<ITableRow>();

            this.graphicBase.DataSource = data;
        }

        private readonly DataGridView graphicBase;
        private readonly BindingList<ITableRow> data;

        private void GraphicBaseOnDataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (var property in typeof(TableRow).GetProperties())
            {
                var displayNameAttribute = property.GetCustomAttribute(typeof(DisplayNameAttribute));
                if (displayNameAttribute != null)
                {
                    var propDisplayName = (displayNameAttribute as DisplayNameAttribute).DisplayName;
                    graphicBase.Columns[property.Name].HeaderText = propDisplayName;
                }

                graphicBase.Columns[property.Name].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
        }

        /// <summary>
        /// Добавление строки: (ИНТЕРФЕЙС) (И.1)
        /// </summary>
        /// <param name="rowCreationData"></param>
        public void AddRow(ITableRowCreationData rowCreationData)
        {
            // Принимаем данные, необходимые для создания строки;
            var tableRowCreationData = rowCreationData;

            // Создаем { Строку таблицы} через метод(1);
            var tableRow = GetTableRow(tableRowCreationData);

            // Добавляем ее в список;
            data.Add(tableRow);
        }

        /// <summary>
        /// Удаление строки: (ИНТЕРФЕЙС) (И.2)
        /// </summary>
        /// <param name="id"></param>
        public void RemoveRow(int id)
        {
            // Принимаем Id {Строки таблицы};
            var rowId = id;

            // Получаем индекс {Строки таблицы} в списке;
            var rowIndex = GetTableRowIndexById(rowId);

            // Удаляем строку из списка;
            data.RemoveAt(rowIndex);
        }

        /// <summary>
        /// Изменение значения строки: (ИНТЕРФЕЙС) (И.4)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newRow"></param>
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

        /// <summary>
        /// Очищает таблицу, удаляя все сохраненные значения
        /// </summary>
        public void Reset()
        {
            data.Clear();
        }

        /// <summary>
        /// Создает {Строку таблицы}: (1)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private ITableRow GetTableRow(ITableRowCreationData data)
        {
            // Получаем все данные, кроме Id строки;
            var creationData = data;

            // Получаем Id как: количество строк в таблице + 1;
            var rowId = this.data.Count + 1;

            // Создаем { Строку таблицы};
            var tableRow = new TableRow(rowId, creationData);

            // Возвращаем;
            return tableRow;
        }

        /// <summary>
        /// Получить индекс строки по Id строки: (2)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private int GetTableRowIndexById(int id)
        {
            // Получаем Id {Строки таблицы};
            var rowId = id;

            // Возвращаем Id -1;
            return id - 1;
        }
    }
}
