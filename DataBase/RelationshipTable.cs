using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.DataBase
{
  internal sealed class RelationshipTable : Table
  {
    private string tableName;
    private Table table01;
    private Table table02;

    public RelationshipTable(Table Table01, Table Table02)
    {
      this.table01 = Table01;
      this.AddForeignKey(this.table01, SetTable01);

      this.table02 = Table02;
      this.AddForeignKey(this.table02, SetTable02);

      this.tableName = String.Format("RelationshipTable_{0}And{1}", table01.GetType().Name, table02.GetType().Name);
    }

    public override string TableName() => this.tableName;
    public void TableName(string tableName)
    {
      this.tableName = tableName;
    }

    // Função responsavel por adicionar fields relacionadas aos valores persistentes.
    protected override void ListFields() { }

    internal Table GetTable01() => table01;
    private void SetTable01(object table)
    {
      if(table.GetType() == typeof(Table))
        this.table01.CopyTo((Table)table);
    }

    internal Table GetTable02() => table02;
    private void SetTable02(object table)
    {
      if (table.GetType() == typeof(Table))
        this.table02.CopyTo((Table)table);
    }
  }
}
