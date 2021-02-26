using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Model
{
  class Fabricante : Controller.Table
  {
    public Controller.StringField Nome { get; } =
      new Controller.StringField("Nome", 50);

    // Inicializa a instancia com os atributos padrão
    public Fabricante() {}

    public Fabricante(string nome) : base()
    {
      this.Nome.Value = nome;
    }

    public override string ToString()
    {
      return Convert.ToString(this.Nome.Value);
    }
  }
}
