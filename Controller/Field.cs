using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Controller
{
  class Field
  {
    private readonly string _nome;
    private object _value;
    private readonly bool _nullable;

    public Field(string name, object value, bool nullable = false)
    {
      _nome = name;
      _value = value;
      _nullable = nullable;
    }

    public string Name                // Nome usado como identificador da Field.
    {
      get => _nome;
    }
    public virtual object Value       // Valor deste field.
    {
      get => _value;
      set
      {
        _value = value;
      }
    }
    protected bool Nullable           // Expecifica se a filde pode ser nula.
    {
      get => _nullable;
    }
    internal virtual string Signature // Resgata a assinatura da Field na tabela.
    {
      get => String.Format("{0} VARCHAR(10) {1} NULL", Name, Nullable ? "" : "NOT");
    }
  }
}
