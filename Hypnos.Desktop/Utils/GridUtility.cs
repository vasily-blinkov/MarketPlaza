using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Wholesale.Desktop.Models.Administration;

namespace Wholesale.Desktop.Utils
{
    public static class GridUtility
    {
        public static void Setup(this DataGridView grid)
        {
            grid.Setup(new[] { nameof(Entity.ID) });
        }

        public static void Setup(this DataGridView grid, IEnumerable<string> hiddenColumnsNames)
        {
            var columns = grid.Columns;
            columns.Hide(hiddenColumnsNames);

            DataGridViewColumn col;

            for (int index = 0; index < columns.Count; index++)
            {
                col = columns[index];

                if (hiddenColumnsNames.Contains(col.Name))
                {
                    continue;
                }

                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        public static void Hide(this DataGridViewColumnCollection columns, IEnumerable<string> names)
        {
            foreach (var col in names)
            {
                if (columns.Contains(col))
                {
                    columns[col].Visible = false;
                }
            }
        }
        
        /// <returns>
        /// Returns row which column called <paramref name="name"/> value is <paramref name="value"/>.
        /// Returns <c>null</c> if the grid hasn't got the column with <paramref name="name"/>
        /// or there is no row with the <paramref name="value"/> in such column.
        /// </returns>
        public static DataGridViewRow Find(
            this DataGridViewRowCollection rows,
            string name,
            object value,
            IEqualityComparer comparer)
        {
            DataGridViewRow row;

            for (int index = 0; index < rows.Count; index++)
            {
                row = rows[index];
                
                if (comparer.Equals(value, row.Cells[name].Value))
                {
                    return row;
                }
            }

            return null;
        }
    }
}
