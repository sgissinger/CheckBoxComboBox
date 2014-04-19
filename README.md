CheckBoxComboBox
================

Forked from Martin Lottering project (visible on codeproject.com)

The main addition is a DataGridViewCheckBoxComboBoxColumn which allows the use of this control in a...DataGridView.
And some minor corrections in order to make the control work smoothly.

It's designed to use DataSource capabilities. If the column is bound to a Dataset, the linked property must be set to Object type.

```
List<Status> statuses = new List<Status>();

statuses.Add(new Status(1, "New"));
statuses.Add(new Status(2, "Loaded"));
statuses.Add(new Status(3, "Inserted"));
statuses.Add(new Status(4, "Updated"));
statuses.Add(new Status(5, "Deleted"));


DataGridViewCheckBoxComboBoxColumn comboboxColumn = new DataGridViewCheckBoxComboBoxColumn();

comboboxColumn.DataSource = new ListSelectionWrapper<Status>(statuses);
comboboxColumn.ValueMember = "Selected";
comboboxColumn.DisplayMemberSingleItem = "Name";
comboboxColumn.DisplayMember = "NameConcatenated";


DataGridView dataGridView = new DataGridView();
dataGridView.Columns.Add(comboboxColumn);

...

var values = dataGridView[0,0].Value as Dictionary<String, Object>;

foreach (var val in values)
{
  var wrappedVal = val.Value as ObjectSelectionWrapper<Status>;

  Status finalVal = wrappedVal.Item;
}
```
