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
  partial class CampoSelectVarios : UserControl
  {
    List<Controller.Table> tables;
    Controller.Relationship relationship;    

    public CampoSelectVarios(Controller.Relationship relationship, bool ReadOnly)
    {
      InitializeComponent();

      this.relationship = relationship;
      this.Label.Text = String.Format("{0} :", relationship.typeTable02.Name);

      // Carrega a lista de Tables.
      //this.tables = Controller.DataBase.Read(relationship.typeTable02, new List<Controller.Field>());
    }

    private void Button_Click(object sender, EventArgs e)
    {
    }

    private void Button_Adicionar_Click(object sender, EventArgs e)
    {
      if (tables.Count > 0)
      {
        View.Resources.ItemSelectVarios item =
          new View.Resources.ItemSelectVarios(relationship, this);

        this.FlowLayoutPanel.Height += item.Height + 6;
        this.FlowLayoutPanel.Controls.Add(item);

        this.Atualizar();
      }
      else
        Console.WriteLine("mensagem erro");
    }
    
    public void Atualizar()
    {
      List<Controller.Table> escolhidos = new List<Controller.Table>();

      foreach(Control control in this.FlowLayoutPanel.Controls)
        if(control.GetType() == typeof(ItemSelectVarios))
          escolhidos.Add(((ItemSelectVarios)control).GetSelected());

      foreach (Control control in this.FlowLayoutPanel.Controls)
        if (control.GetType() == typeof(ItemSelectVarios))
        {
          List<Controller.Table> remocao = new List<Controller.Table>(escolhidos.ToArray());
          remocao.Remove(((ItemSelectVarios)control).GetSelected());

          List<Controller.Table> atual = new List<Controller.Table>(this.tables.ToArray());
          foreach(Controller.Table table in remocao)
            atual.Remove(table);

          ((ItemSelectVarios)control).SetSelectedList(atual);
        }
    }

    public void RemoveItem(ItemSelectVarios item)
    {
      this.FlowLayoutPanel.Controls.Remove(item);
      this.Atualizar();
    }
  }
}
