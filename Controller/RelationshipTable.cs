using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Controller
{
  sealed class RelationshipTable : Table
  {
    private string _tableName;
    private ForeignKey _table01;
    private ForeignKey _table02;

    public RelationshipTable(Table table01, Table table02)
    {
      _table01 = new ForeignKey(table01);
      _table02 = new ForeignKey(table02);

      this._tableName = String.Format("{0}And{1}", table01.Name, table02.Name);
    }

    public override string Name
    {
      get => this._tableName;
    }

    internal string SetName
    {
      set
      {
        this._tableName = value;
      }
    }

    public ForeignKey Table01
    {
      get => _table01;
    }

    public ForeignKey Table02
    {
      get => _table02;
    }
  }
}
