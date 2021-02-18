using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.DataBase
{
  internal class ForeignKey : Field
  {
    // Retem o nome do tipo de classe da chave estrangeira.
    internal Type ForeignTableType { get; }
    public ForeignKey(Table foreignTable, SetValue setValue)
      : base (new List<Field>(foreignTable.Fields()).Find(x => x.GetType() == typeof(PrimaryKey)).Name(), foreignTable.GetID, setValue)
    {
      this.ForeignTableType = foreignTable.GetType();
    }

    // Apenas atribui ao valor caso o mesmo seja uma instância com o mesmo tipo 
    public override void Value(object foreignTable)
    {
      if (this.ForeignTableType == foreignTable.GetType())
        base.Value(foreignTable);
      else
        Console.WriteLine("Mensagem de erro");
    }
  }
}
