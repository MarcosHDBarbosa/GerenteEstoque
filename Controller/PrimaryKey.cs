using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Controller
{
  internal class PrimaryKey : Field
  {
    public PrimaryKey(string name) 
      : base (name, 0) { }
    public override object Value      // Atualiza o valor da PrimaryKey o convetendo para o tipo long.
    {
      set
      {
        base.Value = long.TryParse(Convert.ToString(value), out long outInt) ? outInt : 0;
        //if (typeof(Int64).IsAssignableFrom(value.GetType()))
          //base.Value = Convert.ToInt64(value);
      }
    }

    internal override string Signature // Resgata a assinatura da Field na tabela.
    {
      get => String.Format("{0} BIGINT IDENTITY (1, 1) PRIMARY KEY", this.Name);
    }
  }
}
