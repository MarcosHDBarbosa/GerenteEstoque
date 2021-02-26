using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Controller
{
  class ForeignKey : Field
  {
    private Type _typeForingn;

    public ForeignKey(Table foreignTable)
      : base (foreignTable.Identifier.Name, foreignTable)
    {
      _typeForingn = foreignTable.GetType();
    }

    public override object Value        // Valor deste field.
    {
      set
      {
        if (_typeForingn == value.GetType())
          base.Value = (Table)value;
      }
    }

    internal override string Signature  // Resgata a assinatura da Field na tabela.
    {
      get => String.Format("{0} BIGINT NOT NULL", this.Name);
    }

    internal Type TypeForingn { get => _typeForingn;  }
  }
}
