using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.DataBase
{
  internal class Relationship : Field
  {
    internal Table Table01 { get; }
    internal Type typeTable02 { get; }

    internal Relationship(Table table01, Type typeTable02, GetValue getValue, SetValue addTable) 
      : base(String.Format("RelationshipTable_{0}And{1}", table01.GetType().Name, typeTable02.Name), getValue, addTable)
    {
      this.Table01 = table01;
      this.typeTable02 = typeTable02;
    }

    // Retorna uma lista de instâncias RelationshipTable referenciando as duas tabelas relacionada
    public override object Value()
    {
      IReadOnlyCollection<Table> secundaryTables = (IReadOnlyCollection<Table>)base.Value();
      List<RelationshipTable> relationshipTables = new List<RelationshipTable>();

      foreach (Table secundaryTable in secundaryTables)
        relationshipTables.Add(new RelationshipTable(Table01, secundaryTable));

      return relationshipTables;
    }

    // recebe uma lista somente leitura de RelationshipTable e os adiciona pela função delegate.
    public override void Value(object relationshipTables)
    {
      foreach (RelationshipTable relationshipTable in (IReadOnlyCollection<RelationshipTable>)relationshipTables)
        base.Value(relationshipTable.GetTable02());
    }
  }
}
