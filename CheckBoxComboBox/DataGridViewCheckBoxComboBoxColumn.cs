using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace PresentationControls
{
    /// <summary>
    /// TODO: Documentation Class
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(NumericUpDown))]
    public class DataGridViewCheckBoxComboBoxColumn : DataGridViewComboBoxColumn
    {
        #region MEMBERS
        /// <summary>
        /// TODO: Documentation Member
        /// </summary>
        private String _displayMemberSingleItem = String.Empty;
        #endregion

        #region PROPERTIES
        /// <summary>
        /// 
        /// </summary>
        [Category("Aloes")]
        [Description("")]
        [DefaultValue("")]
        public String DisplayMemberSingleItem
        {
            get { return _displayMemberSingleItem; }
            set { _displayMemberSingleItem = value; }
        }
        #endregion

        #region CONSTRUCTORS & FINALIZERS
        /// <summary>
        /// TODO: Documentation Constructor
        /// </summary>
        public DataGridViewCheckBoxComboBoxColumn()
        {
            this.ValueType = typeof(Object);

            this.CellTemplate = new DataGridViewCheckBoxComboBoxCell();
        }

        /// <summary>
        /// Clone override is needed in order to store property set in Designer
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            DataGridViewCheckBoxComboBoxColumn col = base.Clone() as DataGridViewCheckBoxComboBoxColumn;

            col.DisplayMemberSingleItem = this._displayMemberSingleItem;

            return col;
        }
        #endregion

        /// <summary>
        /// TODO: Documentation Class
        /// </summary>
        public class DataGridViewCheckBoxComboBoxCell : DataGridViewComboBoxCell
        {
            #region PROPERTIE
            /// <summary>
            /// Return the entityType of the editing ctl that UpDownCell uses
            /// </summary>
            public override Type EditType
            {
                get { return typeof(DataGridViewCheckBoxComboBoxControl); }
            }
            /// <summary>
            /// Use the DBNull as the default value
            /// </summary>
            public override object DefaultNewRowValue
            {
                get { return Convert.DBNull; }
            }
            #endregion

            #region METHODS
            /// <summary>
            /// TODO: Documentation GetFormattedValue
            /// </summary>
            /// <param name="value"></param>
            /// <param name="rowIndex"></param>
            /// <param name="cellStyle"></param>
            /// <param name="valueTypeConverter"></param>
            /// <param name="formattedValueTypeConverter"></param>
            /// <param name="context"></param>
            /// <returns></returns>
            protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
            {
                String result = String.Empty;

                if (value != Convert.DBNull)
                {
                    DataGridViewCheckBoxComboBoxControl control = this.DataGridView.EditingControl as DataGridViewCheckBoxComboBoxControl;

                    if (control != null)
                    {
                        foreach (CheckBoxComboBoxItem item in control.CheckBoxItems)
                        {
                            if (item.Checked)
                            {
                                if (String.IsNullOrEmpty(result))
                                    result = item.Text;
                                else
                                    result = String.Format(CultureInfo.CurrentCulture,
                                                           "{0}, {1}",
                                                           result, item.Text);
                            }
                        }
                    }
                }

                return result;
            }

            /// <summary>
            /// TODO: Documentation ParseFormattedValue
            /// </summary>
            /// <param name="formattedValue"></param>
            /// <param name="cellStyle"></param>
            /// <param name="formattedValueTypeConverter"></param>
            /// <param name="valueTypeConverter"></param>
            /// <returns></returns>
            public override object ParseFormattedValue(object formattedValue, DataGridViewCellStyle cellStyle, TypeConverter formattedValueTypeConverter, TypeConverter valueTypeConverter)
            {
                DataGridViewCheckBoxComboBoxControl control = this.DataGridView.EditingControl as DataGridViewCheckBoxComboBoxControl;

                var t = new Dictionary<String, Object>();

                foreach (CheckBoxComboBoxItem item in control.CheckBoxItems)
                {
                    if (item.Checked)
                        t.Add(item.Text, item.ComboBoxItem);
                }

                return t;
            }

            /// <summary>
            /// TODO: Documentation InitializeEditingControl
            /// </summary>
            /// <param name="rowIndex"></param>
            /// <param name="initialFormattedValue"></param>
            /// <param name="dataGridViewCellStyle"></param>
            public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
            {
                base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

                DataGridViewCheckBoxComboBoxControl control = this.DataGridView.EditingControl as DataGridViewCheckBoxComboBoxControl;
                DataGridViewCheckBoxComboBoxColumn config = this.OwningColumn as DataGridViewCheckBoxComboBoxColumn;
                control.DisplayMemberSingleItem = config.DisplayMemberSingleItem;

                foreach (CheckBoxComboBoxItem item in control.CheckBoxItems)
                    item.Checked = false;

                if (this.Value != Convert.DBNull)
                {
                    Dictionary<String, Object> t = this.Value as Dictionary<String, Object>;

                    foreach (String key in t.Keys)
                        control.CheckBoxItems[key].Checked = true;
                }

                control.BeginInvoke(new MethodInvoker(control.ShowDropDown));
            }

            /// <summary>
            /// TODO: Documentation DetachEditingControl
            /// </summary>
            public override void DetachEditingControl()
            {
                DataGridViewCheckBoxComboBoxControl control = this.DataGridView.EditingControl as DataGridViewCheckBoxComboBoxControl;

                control.BeginInvoke(new MethodInvoker(control.HideDropDown));

                base.DetachEditingControl();
            }
            #endregion
        }

        /// <summary>
        /// TODO: Documentation Class
        /// </summary>
        private class DataGridViewCheckBoxComboBoxControl : CheckBoxComboBox, IDataGridViewEditingControl
        {
            #region PROPERTIES
            /// <summary>
            /// Implements the IDataGridViewEditingControl.EditingControlDataGridView ctl.
            /// </summary>
            public DataGridView EditingControlDataGridView { get; set; }
            /// <summary>
            /// Implements the IDataGridViewEditingControl.EditingControlFormattedValue ctl.
            /// </summary>
            public object EditingControlFormattedValue
            {
                get { return base.Text; }
                set
                {
                    foreach (CheckBoxComboBoxItem item in base.CheckBoxItems)
                        item.Checked = false;

                    String[] keys = value.ToString().Replace(" ", String.Empty).Split(',');

                    foreach (String key in keys)
                    {
                        if (!String.IsNullOrEmpty(key))
                            base.CheckBoxItems[key].Checked = true;
                    }
                }
            }
            /// <summary>
            /// Implements the IDataGridViewEditingControl.EditingControlRowIndex ctl.
            /// </summary>
            public int EditingControlRowIndex { get; set; }
            /// <summary>
            /// Implements the IDataGridViewEditingControl.EditingControlValueChanged ctl.
            /// </summary>
            public bool EditingControlValueChanged { get; set; }
            /// <summary>
            /// Implements the IDataGridViewEditingControl.EditingPanelCursor ctl.
            /// </summary>
            public Cursor EditingPanelCursor
            {
                get { return Cursors.Default; }
            }
            /// <summary>
            /// Implements the IDataGridViewEditingControl.RepositionEditingControlOnValueChange ctl.
            /// </summary>
            public bool RepositionEditingControlOnValueChange
            {
                get { return false; }
            }
            #endregion

            #region METHODS
            /// <summary>
            /// Notify the DataGridView that the contents of the cell have changed.
            /// </summary>
            /// <param name="e"></param>
            protected override void OnTextChanged(EventArgs e)
            {
                if (this.EditingControlDataGridView != null)
                {
                    this.EditingControlValueChanged = true;
                    this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
                }

                base.OnTextChanged(e);
            }

            /// <summary>
            /// TODO: Documentation OnCreateControl
            /// </summary>
            protected override void OnCreateControl()
            {
                base.OnCreateControl();

                base.CheckBoxProperties.AutoCheck = true;
                base.DropDownStyle = ComboBoxStyle.DropDownList;
            }

            /// <summary>
            /// Implements the IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
            /// </summary>
            /// <param name="dataGridViewCellStyle"></param>
            public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
            {
                this.Font = dataGridViewCellStyle.Font;
            }

            /// <summary>
            /// Implements the IDataGridViewEditingControl.EditingControlWantsInputKey method.
            /// </summary>
            /// <param name="key"></param>
            /// <param name="dataGridViewWantsInputKey"></param>
            /// <returns></returns>
            public bool EditingControlWantsInputKey(Keys key, bool dataGridViewWantsInputKey)
            {
                switch (key & Keys.KeyCode)
                {
                    case Keys.Up:
                    case Keys.Down:
                        return true;

                    default:
                        return !dataGridViewWantsInputKey;
                }
            }

            /// <summary>
            /// Implements the IDataGridViewEditingControl.GetEditingControlFormattedValue method.
            /// </summary>
            /// <param name="context"></param>
            /// <returns></returns>
            public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
            {
                return this.EditingControlFormattedValue;
            }

            /// <summary>
            /// Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit method.
            /// </summary>
            /// <param name="selectAll"></param>
            public void PrepareEditingControlForEdit(bool selectAll)
            { }
            #endregion
        }
    }
}