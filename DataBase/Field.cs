using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.DataBase
{
  delegate object GetValue();
  delegate void SetValue(object value);
  internal class Field
  {
    // Nome do Campo da Tabela
    private string name;
    // Delegate direcionado para a função que recupera o valor da instancia
    private GetValue getValue;
    // Delegate direcionado para a função que atualiza o valor da instancia
    private SetValue setValue;

    // Inicializa os atributos com os valores recebidos como parametro 
    public Field(string name, GetValue getValue = null, SetValue setValue = null)
    {
      this.name = name;
      this.getValue = getValue;
      this.setValue = setValue;
    }

    // Recupera o nome dado ao campo
    public string Name() => this.name;
    // Recupera o valor da instancia pelo delegate
    public virtual object Value() => this.getValue();
    // Atualiza o valor da instancia pelo delegate
    public virtual void Value(object value) { this.setValue(value); }
  }
}
