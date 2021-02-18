﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Model
{
  class Categoria : DataBase.Table
  {
    private string nome = "INDEFINIDO";

    // Inicializa a instancia com os atributos padrão
    public Categoria() {}

    // Inicializa a instancia com o dados informados
    public Categoria(string nome) : base()
    {
      this.nome = nome;
    }

    // Função responsavel por adicionar fields relacionadas aos valores persistentes
    protected override void ListFields()
    {
      this.AddField("Nome", GetNome, SetNome);
    }

    // Get and Set do valor Nome.
    public object GetNome() => this.nome;
    public void SetNome(object nome) { this.nome = Convert.ToString(nome); }
  }
}
