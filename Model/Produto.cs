using GerenteEstoque.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Model
{
  class Produto : Table
  {

    public override string Name            // Obtem o nome da tabela. Por padrão é o nome da classe.
    {
      get => this.GetType().Name;
    }
    public StringField Nome { get; } =
      new StringField("Nome", 50);
    public StringField Medida { get; } =
      new StringField("Medida", 10);
    public StringField Descricao { get; } =
      new StringField("Descrição", 200, true);
    public ForeignKey Fabricante { get; } =
      new ForeignKey(new Fabricante());

    public Relationship ListCategoria { get; }

    public Produto()
    {
      ListCategoria = new Relationship(this, typeof(Categoria));
    }
    public Produto(string nome,/*Image imagem,*/ string medida, string descricao, Fabricante fabricante, List<Categoria> categorias)
    {
      this.Nome.Value = nome;
      //this.imagem = imagem;
      this.Medida.Value = medida;
      this.Descricao.Value = descricao;
      this.Fabricante.Value = fabricante;

      ListCategoria = new Relationship(this, typeof(Categoria));
      foreach (Categoria categoria in categorias)
        ListCategoria.AddTable(categoria);
    }
  }
}
