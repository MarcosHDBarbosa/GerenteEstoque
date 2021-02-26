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
  partial class CampoTexto : UserControl
  {
    Controller.Field field;
    public CampoTexto(Controller.Field field, bool ReadOnly)
    {
      InitializeComponent();

      this.field = field;

      //this.Label.Text = String.Format("{0} :", field.Name());
     // this.TextBox.Text = Convert.ToString(field.Value());

      if (ReadOnly)
        this.TextBox.ReadOnly = ReadOnly;
      else
        this.TextBox.LostFocus += new System.EventHandler(this.TextBox_SetValue);

      this.TextBox.GotFocus += new System.EventHandler(this.TextBox_GotFocus);
      this.TextBox.LostFocus += new System.EventHandler(this.TextBox_LostFocus);
    }

    private void TextBox_GotFocus(object sender, EventArgs e)
    {
      this.TextBox.BackColor = System.Drawing.Color.LemonChiffon;
    }

    private void TextBox_LostFocus(object sender, EventArgs e)
    {
      this.TextBox.BackColor = System.Drawing.SystemColors.Window;
    }

    private void TextBox_SetValue(object sender, EventArgs e)
    {
      //this.field.Value(this.TextBox.Text);
    }
  }
}
