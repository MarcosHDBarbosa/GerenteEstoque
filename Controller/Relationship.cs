using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Controller
{
  delegate IReadOnlyCollection<Table> GetTables();
  delegate void SetTable(Table table);
  internal class Relationship : Field
  {
    private readonly Table _table01;
    private readonly Type _typeTable02;
    private List<Table> _tables = new List<Table>();

    internal Relationship(Table table01, Type typeTable02) 
      : base(String.Format("{0}And{1}", table01.Name, typeTable02.Name), null)
    {
      this._table01 = table01;
      this._typeTable02 = typeTable02;
    }

    internal Table Table01
    {
      get => _table01;
    }

    internal Type typeTable02
    {
      get => _typeTable02;
    }

    public override object Value
    {
      get // Retorna uma lista de instâncias RelationshipTable referenciando as duas tabelas relacionada
      {
        List<RelationshipTable> relationshipTables = new List<RelationshipTable>();

        foreach (Table secundaryTable in _tables)
          relationshipTables.Add(new RelationshipTable(Table01, secundaryTable));

        return relationshipTables;
      }
      set // recebe uma lista somente leitura de RelationshipTable e os adiciona pela função delegate.
      {
        if (value?.GetType() == typeof(List<RelationshipTable>))
          foreach (RelationshipTable relationshipTable in (List<RelationshipTable>)value)
            AddTable((Table)relationshipTable.Table02.Value);
      }
    }

    internal override string Signature { get => null; }

    public IReadOnlyCollection<Table> ListTables() => _tables.AsReadOnly();

    public void AddTable(Table table)
    {
      if (_typeTable02 == table.GetType())
        if (!_tables.Exists(x => Convert.ToInt64(x.Identifier.Value) == Convert.ToInt64(table.Identifier.Value)))
          _tables.Add(table);
    }

    public void RemoveTable(Table table)
    {
      _tables.RemoveAll(x => Convert.ToInt64(x.Identifier.Value) == Convert.ToInt64(table.Identifier.Value));
    }
  }
}
