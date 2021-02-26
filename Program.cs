using GerenteEstoque.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GerenteEstoque
{
  static class Program {
    /// <summary>
    /// Ponto de entrada principal para o aplicativo.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      Controller.Controller.Create(new Model.Categoria());
      Controller.Controller.Create(new Model.Fabricante());
      Controller.Controller.Create(new Model.Produto());
      Controller.Controller.Create(new RelationshipTable(new Model.Produto(), new Model.Categoria()));

      Controller.Controller.Insert(new Model.Categoria("Menino"));
      Controller.Controller.Insert(new Model.Categoria("Menina"));
      Controller.Controller.Insert(new Model.Categoria("Shorte"));
      Controller.Controller.Insert(new Model.Categoria("Camisa"));
      Controller.Controller.Insert(new Model.Categoria("Vestido"));
      Controller.Controller.Insert(new Model.Categoria("Sapato"));
      Controller.Controller.Insert(new Model.Categoria("Rasteirinha"));

      Controller.Controller.Insert(new Model.Fabricante("Klin"));
      Controller.Controller.Insert(new Model.Fabricante("Kyly"));
      Controller.Controller.Insert(new Model.Fabricante("Kiko"));

      List<Controller.Table> categoria = Controller.Controller.Read(
        typeof(Model.Categoria),
        new List<Controller.Field>()
        {
        }
        );

      List<Controller.Table> fabricante = Controller.Controller.Read(
        typeof(Model.Fabricante),
        new List<Controller.Field>()
        {
        }
        );

      categoria = Controller.Controller.Read(
        typeof(Model.Categoria),
        new List<Controller.Field>()
        {
        }
        );

      Controller.Controller.Insert(
        new Model.Produto(
            "Shortinho",
            "M",
            "Shorte tamanho M",
            (Model.Fabricante)fabricante[1],
            new List<Model.Categoria>()
            {
              (Model.Categoria)categoria[0],
              (Model.Categoria)categoria[2]
            }

          )
        );

      List<Controller.Table> produto = Controller.Controller.Read(
        typeof(Model.Produto),
        new List<Controller.Field>()
        {
        }
        );

      Console.WriteLine(
        ((Model.Produto)produto[0]).Nome.Value + "=" +
        ((Model.Produto)produto[0]).Fabricante.Value + "=" +
        ((Model.Produto)produto[0]).ListCategoria.ListTables().Count
        );

      //Console.WriteLine(fabricante+""+ categoria + ""+ produto);
      ((Model.Produto)produto[0]).Nome.Value = "Ervilha";
      ((Model.Produto)produto[0]).Fabricante.Value = fabricante[2];
      //((Model.Produto)produto[0]).ListCategoria.RemoveTable(categoria[0]);
      ((Model.Produto)produto[0]).ListCategoria.AddTable(categoria[1]);

      Console.WriteLine(
        ((Model.Produto)produto[0]).Nome.Value + "=" +
        ((Model.Produto)produto[0]).Fabricante.Value + "=" +
        ((Model.Produto)produto[0]).ListCategoria.ListTables().Count
        );


      Controller.Controller.Insert(produto[0]);
      produto = Controller.Controller.Read(
        typeof(Model.Produto),
        new List<Controller.Field>()
        {
        }
        );

      List<RelationshipTable> tb = Controller.Controller.ReadRelationshipTables("ProdutoAndCategoria", typeof(Model.Produto), typeof(Model.Categoria), new List<Field>());

      Console.WriteLine(fabricante + ""+ categoria + ""+ produto + ""+ tb);

      Controller.Controller.Delete(produto[0]);

      produto = Controller.Controller.Read(
        typeof(Model.Produto),
        new List<Controller.Field>()
        {
        }
        );

      Console.WriteLine(produto);

      tb = Controller.Controller.ReadRelationshipTables("ProdutoAndCategoria", typeof(Model.Produto), typeof(Model.Categoria), new List<Field>());

      Console.WriteLine(tb);
      /*
      List<Controller.Table> Categoria = Controller.DataBase.Read(
        typeof(Model.Categoria),
        new List<Controller.Field>(){
          }
        );

      Console.WriteLine(fabricante.Count + "-" + Categoria.Count);

      Controller.DataBase.Insert(
        new Model.Produto(
          "Refrigerante Mate Couro Vidro 250ml",
          "250ml",
          "Melhor que Coca Cola.",
          (Model.Fabricante)fabricante[1],
          new List<Model.Categoria>(){
            (Model.Categoria)Categoria[0],
            (Model.Categoria)Categoria[5]
            }
          )
        );

      List<Controller.Table> Procuto = Controller.DataBase.Read(
        typeof(Model.Produto),
        new List<Controller.Field>(){
          }
        );

      Console.WriteLine(Procuto.Count);

      List<Controller.RelationshipTable> table =
        Controller.DataBase.ReadRelationshipTables(
          "RelationshipTable_ProdutoAndCategoria",
          typeof(Model.Produto),
          typeof(Model.Categoria),
          new List<Controller.Field>()
          );
      
      Console.WriteLine(table);

      //((Model.Produto)Procuto[0]).AddCategoria((Model.Categoria)Categoria[5]);

      Console.WriteLine(
        Controller.DataBase.Delete(
          Procuto[0]
          )
        );

      table = Controller.DataBase.ReadRelationshipTables(
          "RelationshipTable_ProdutoAndCategoria",
          typeof(Model.Produto),
          typeof(Model.Categoria),
          new List<Controller.Field>()
        );

      Console.WriteLine(table);

      Procuto = Controller.DataBase.Read(
        typeof(Model.Produto),
        new List<Controller.Field>(){}
        );

      Console.WriteLine(Procuto.Count);

      /*tabela[1].Fields()[1].Value("Lamina");

      DataBase.DataBase.Update(
        new List<DataBase.Table>(){
          tabela[1],
          }
        );

      tabela = DataBase.DataBase.Read(
        typeof(Model.Categoria),
        new List<DataBase.Field>(){
          }
        );

      Console.WriteLine(tabela.Count);

      Application.Run(new Form1(Procuto[0]));*/
    }
  }
}
