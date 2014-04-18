using System.Data;
using PresentationControls;

namespace DemoApp
{
    public class DataTableWrapper : ListSelectionWrapper<DataRow>
    {
        public DataTableWrapper(DataTable dataTable, string usePropertyAsDisplayName) : base(dataTable.Rows, false, usePropertyAsDisplayName) { }
    }
}
