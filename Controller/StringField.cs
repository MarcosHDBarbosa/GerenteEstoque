using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Controller
{
  class StringField : Field
  {
    private readonly long _maximum;

    public StringField(string name, uint Maximum, bool nullable = false)
       : base(name, "UNDEFINED", nullable)
    {
      _maximum = Maximum;
    }


    // Delimitadores do valor.
    public long Maximum
    {
      get => _maximum;
    }

    // Atualiza o valor da Field caso seja valido.
    public override object Value
    {
      set
      {
        if (typeof(String).IsAssignableFrom(value.GetType()))
        {
          string newValue = Convert.ToString(value);
          if (newValue.Length > Maximum)
            newValue = newValue.Substring(0, Convert.ToInt32(Maximum));
          base.Value = newValue;
        }
        else
          Console.WriteLine("Mensagem  de Erro");
      }
    }

    // Resgata a assinatura da Field na tabela.
    internal override string Signature
    {
      get => String.Format("{0} VARCHAR ({1}) {2} NULL", Name, Maximum, Nullable ? "" : "NOT");
    }
  }
}
