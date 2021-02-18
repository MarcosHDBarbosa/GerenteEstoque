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

      DataBase.DataBase.Insert(
        new Model.Fabricante("Pepsi")
        );

      List<DataBase.Table> fabricante = DataBase.DataBase.Read(
        typeof(Model.Fabricante),
        new List<DataBase.Field>(){
          }
        );

      List<DataBase.Table> Categoria = DataBase.DataBase.Read(
        typeof(Model.Categoria),
        new List<DataBase.Field>(){
          }
        );

      Console.WriteLine(fabricante.Count + "-" + Categoria.Count);

      DataBase.DataBase.Insert(
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

      List<DataBase.Table> Procuto = DataBase.DataBase.Read(
        typeof(Model.Produto),
        new List<DataBase.Field>(){
          }
        );

      Console.WriteLine(Procuto.Count);

      List<DataBase.RelationshipTable> table =
        DataBase.DataBase.ReadRelationshipTables(
          "RelationshipTable_ProdutoAndCategoria",
          typeof(Model.Produto),
          typeof(Model.Categoria),
          new List<DataBase.Field>()
          );
      
      Console.WriteLine(table);

      //((Model.Produto)Procuto[0]).AddCategoria((Model.Categoria)Categoria[5]);

      Console.WriteLine(
        DataBase.DataBase.Delete(
          Procuto[0]
          )
        );

      table =DataBase.DataBase.ReadRelationshipTables(
          "RelationshipTable_ProdutoAndCategoria",
          typeof(Model.Produto),
          typeof(Model.Categoria),
          new List<DataBase.Field>()
        );

      Console.WriteLine(table);

      Procuto = DataBase.DataBase.Read(
        typeof(Model.Produto),
        new List<DataBase.Field>(){}
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

      Console.WriteLine(tabela.Count);*/

      Application.Run(new Form1());
    }
  }
}
