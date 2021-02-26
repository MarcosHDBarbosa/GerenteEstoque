using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Controller
{
  class IntergeField : Field 
  {
    private Type _typeValue; // Propriedade usada para delimitar os tipos de dados aceitos.
    private readonly long _minimum;
    private readonly long _maximum;

    public IntergeField(string name, long minimum, long maximum, bool nullable = false)
      : base(name, 0, nullable)
    {
      // O valor Minimum não pode ser maior do que o Maximum.
      _maximum = maximum;
      if (Minimum < maximum)
        _minimum = minimum;
      else
        _minimum = maximum;

      // Identifica o tipo de valor conforme as limitações Maximum e Minimum
      if (_maximum <= Int16.MaxValue && _minimum >= Int16.MinValue)
        this._typeValue = typeof(Int16);
      else if (_maximum <= Int32.MaxValue && _minimum >= Int32.MinValue)
        this._typeValue = typeof(Int32);
      else
        this._typeValue = typeof(Int64);
    }

    // Delimitadores do valor.
    public long Minimum
    {
      get => _minimum;
    }
    public long Maximum
    {
      get => _maximum;
    }

    // Atualiza o valor da Field caso seja valido.
    public override object Value
    {
      set
      {
        if (_typeValue.IsAssignableFrom(value.GetType()))
        {
          long newValue = Convert.ToInt64(value);
          if (newValue > this.Maximum)
            base.Value = this.Maximum;
          else if (newValue < this.Minimum)
            base.Value = this.Minimum;
          else
            base.Value = value;
        }
        else
          Console.WriteLine("Mensagem  de Erro");
      }
    }

    // Resgata a assinatura da Field na tabela.
    internal override string Signature
    {
      get
      {
        string signature = "INT";
        if (_typeValue == typeof(Int16))
          signature = "SMALLINT";
        else if (_typeValue == typeof(Int64))
          signature = "BIGINT";

        return String.Format("{0} {1} {2} NULL", this.Name, signature, Nullable ? "" : "NOT");
      }
    }
  }
}
