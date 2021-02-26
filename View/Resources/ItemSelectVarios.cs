using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GerenteEstoque.View.Resources
{
  partial class ItemSelectVarios : UserControl
  {
    Controller.Relationship relationship;
    CampoSelectVarios campo;
    public ItemSelectVarios(Controller.Relationship relationship, CampoSelectVarios campo, bool ReadOnly = false)
    {
      InitializeComponent();

      this.relationship = relationship;
      this.campo = campo;

      if (ReadOnly)
      {
        this.ComboBox.KeyDown += this.ComboBox_SuppressKeyPress;
        this.ComboBox.DropDownStyle = ComboBoxStyle.Simple;
      }
      else
      {
        this.ComboBox.LostFocus += new System.EventHandler(this.ComboBox_SetValue);
        this.ComboBox.SelectionChangeCommitted += new System.EventHandler(this.ComboBox_Cahnge);
      }

      this.ComboBox.GotFocus += new System.EventHandler(this.ComboBox_GotFocus);
      this.ComboBox.LostFocus += new System.EventHandler(this.ComboBox_LostFocus);
    }

    private void ComboBox_SuppressKeyPress(object sender, KeyEventArgs e)
    {
      e.SuppressKeyPress = true;
    }

    private void ComboBox_GotFocus(object sender, EventArgs e)
    {
      this.ComboBox.BackColor = System.Drawing.Color.LemonChiffon;
    }

    private void ComboBox_LostFocus(object sender, EventArgs e)
    {
      this.ComboBox.BackColor = System.Drawing.SystemColors.Window;
    }

    private void ComboBox_SetValue(object sender, EventArgs e)
    {
      if (!this.ComboBox.CanSelect)
        this.ComboBox.SelectedIndex = 0;
      //else
        //campo.Atualizar();
    }

    
    private void ComboBox_Cahnge(object sender, EventArgs e)
    {
       campo.Atualizar();
    }

    public Controller.Table GetSelected() => (Controller.Table)this.ComboBox.SelectedItem;
    public void SetSelectedList(List<Controller.Table> tables)
    {
      Controller.Table table = null;
      if (this.ComboBox.CanSelect)
        table = this.GetSelected();

      this.ComboBox.Items.Clear();
      this.ComboBox.Items.AddRange(tables.ToArray());
      if(tables.Contains(table))
        this.ComboBox.SelectedItem = table;
      else
        this.ComboBox.SelectedItem = tables[0];
    }

    private void Button_Click(object sender, EventArgs e)
    {
      campo.RemoveItem(this);
    }
  }
}
