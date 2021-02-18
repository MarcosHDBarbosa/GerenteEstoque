using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Model
{
  class Produto : DataBase.Table
  {
    private string nome = "INDEFINIDO";
    // private  imagem
    private string medida = "0kg/0ml";
    private string descricao = "INDEFINIDO";
    private Fabricante fabricante = new Fabricante();
    private List<Categoria> categorias = new List<Categoria>();

    public Produto() { }
    public Produto(string nome,/*Image imagem,*/ string medida, string descricao, Fabricante fabricante, List<Categoria> categorias)
    {
      this.nome = nome;
      //this.imagem = imagem;
      this.medida = medida;
      this.descricao = descricao;
      this.fabricante.CopyTo(fabricante);
      foreach(Categoria categoria in categorias)
        this.categorias.Add(categoria);

    }

    // Função responsavel por adicionar fields relacionadas aos valores persistentes
    protected override void ListFields()
    {
      this.AddField("Nome", GetNome, SetNome);
      //this.AddField("Imagem", GetImagem, GetImagen);
      this.AddField("Medida", GetMedida, SetMedida);
      this.AddField("Descricao", GetDescricao, SetDescricao);
      this.AddForeignKey(fabricante, SetFabricante);
      this.AddRelationship(this, typeof(Categoria), GetCategorias, AddCategoria);
    }

    // Get and Set do valor Nome.
    public object GetNome() => this.nome;
    public void SetNome(object nome) { this.nome = Convert.ToString(nome); }

    // Get and Set do valor Medida.
    public object GetMedida() => this.medida;
    public void SetMedida(object medida) { this.medida = Convert.ToString(medida); }

    // Get and Set do valor Descricao.
    public object GetDescricao() => this.descricao;
    public void SetDescricao(object descricao) { this.descricao = Convert.ToString(descricao); }

    // Get and Set do valor Fabricante.
    public object GetFabricante() => this.fabricante;
    public void SetFabricante(object fabricante)
    {
      if(fabricante.GetType() == typeof(Fabricante))
        this.fabricante.CopyTo((Fabricante)fabricante);
    }

    // Get and Set do valor Descricao.
    public IReadOnlyCollection<Categoria> GetCategorias() => this.categorias.AsReadOnly();
    public void AddCategoria(object categoria)
    {
      if (categoria.GetType() == typeof(Categoria))
        this.categorias.Add((Categoria)categoria);
    }
    public void RemoveCategoria(Categoria categoria)
    {
      this.categorias.Remove(categoria);
    }
  }
}
