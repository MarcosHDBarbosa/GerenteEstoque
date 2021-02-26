using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GerenteEstoque
{
  partial class Form1 : Form
  {
    public Form1(Controller.Table table)
    {
      InitializeComponent();
      //List<View.Resources.CampoTexto> campos = new List<View.Resources.CampoTexto>();
      /*Controller.Field[] fields = table.Fields();
      for (int index = 0; index < fields.Length; index++)
      {
        UserControl campo;

        if (fields[index].GetType() == typeof(Controller.PrimaryKey))
          campo = new View.Resources.CampoTexto(fields[index], true);
        else if (fields[index].GetType() == typeof(Controller.ForeignKey))
          campo = new View.Resources.CampoSelect((Controller.ForeignKey)fields[index], false);
        else if (fields[index].GetType() == typeof(Controller.Relationship))
          campo = new View.Resources.CampoSelectVarios((Controller.Relationship)fields[index], true);
        else
          campo = new View.Resources.CampoTexto(fields[index], true);

        campo.Location = new System.Drawing.Point(50, 50 + (index * 35));

        this.Controls.Add(campo);
      }*/
    }
  }
}
