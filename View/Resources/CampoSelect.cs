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
  partial class CampoSelect : UserControl
  {
    Controller.ForeignKey foreignKey;
    public CampoSelect(Controller.ForeignKey foreignKey, bool ReadOnly = false)
    {
      InitializeComponent();

      this.foreignKey = foreignKey;

      //this.Label.Text = String.Format("{0} :", foreignKey.ForeignTableType.Name);

      if (ReadOnly)
      {
        this.ComboBox.KeyDown += this.ComboBox_SuppressKeyPress;
        this.ComboBox.DropDownStyle = ComboBoxStyle.Simple;
      }
      else
      {
        this.ComboBox.LostFocus += new System.EventHandler(this.ComboBox_SetValue);
      }

      this.ComboBox.GotFocus += new System.EventHandler(this.ComboBox_GotFocus);
      this.ComboBox.LostFocus += new System.EventHandler(this.ComboBox_LostFocus);

      // Cria uma lista de Table.
      List<Controller.Table> tables = new List<Controller.Table>();
      // Busca e adiciona todas as tabelas do tipo  pela ForeignKey.
      /*foreach (Controller.Table table in Controller.DataBase.Read(foreignKey.ForeignTableType, new List<Controller.Field>()))
        tables.Add(table);*/

      // Adiciona as opções ao ComboBox.
      this.ComboBox.Items.AddRange(tables.ToArray());

      // Seta o valor do ComboBox de acordo com o identificador fornecido pelo ForeignKey.
      //this.ComboBox.SelectedItem = tables.Find(x => Convert.ToInt32(x.GetID()) == Convert.ToInt32(foreignKey.Value()));
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
      /*else
        this.foreignKey.Value(this.ComboBox.SelectedItem);*/
    }

    private void Button_Click(object sender, EventArgs e)
    {

    }
  }
}
