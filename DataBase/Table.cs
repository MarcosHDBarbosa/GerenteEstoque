using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.DataBase
{
  class Table
  {
    private int id = 0;

    // Cria e inicializa a lista de colunas da tabela.
    protected internal List<Field> fields = new List<Field>();

    protected Table()
    {
      // Por padrão, toda instancia table possue uma chave primária.
      try
      {
        Field newField = new PrimaryKey(String.Format("ID_{0}", base.GetType().Name), GetID, SetID);
        this.fields.Add(newField);
      }
      catch
      {
        Console.WriteLine("Mensagem de erro");
      }
      this.ListFields();
    }

    // Retorna uma novo instância Table
    static internal Table GetTable() => new Table();

    public virtual string TableName() => this.GetType().Name;

    // Copia os valores de uma instância de mesmo tipo.
    public void CopyTo(Table table)
    {
      if(this.GetType() == table.GetType())
      {
        Field[] newParametes = table.Fields();
        for (int index = 0; index < newParametes.Length; index++)
          this.fields[index].Value(newParametes[index].Value());
      }
    }

    // Função responsavel por adicionar fields relacionadas aos valores persistentes.
    protected virtual void ListFields() { }

    // Função responsavel por gerir a adição da nova Field
    protected void AddField(string name, GetValue getValue, SetValue setValue)
    {
      try
      {
        Field newField = new Field(name, getValue, setValue);
        this.fields.Add(newField);
      }
      catch
      {
        Console.WriteLine("Mensagem de erro");
      }
    }

    // Função responsavel por gerir a adição da chaves estrangeiras
    protected void AddForeignKey(Table foreignTable, SetValue setValue)
    {
      try
      {
        ForeignKey foreignKey = new ForeignKey(foreignTable, setValue);
        this.fields.Add(foreignKey);
      }
      catch
      {
        Console.WriteLine("Mensagem de erro");
      }
    }

    // Funções responsavel por gerir a adição das tabelas de relacionamento
    protected void AddRelationship(Table primaryTable, Type secondaryType, GetValue getValue, SetValue addTable)
    {
      try
      {
        Relationship relationship = new Relationship(
          primaryTable, 
          secondaryType, 
          getValue, 
          addTable
          );
        this.fields.Add(relationship);
      }
      catch
      {
        Console.WriteLine("Mensagem de erro");
      }
    }
   

    // Get and Set do identificador.
    public object GetID() => this.id;
    internal void SetID(object id) { this.id = Convert.ToInt32(id); }

    // Replica a lista de Fields e a retorna.
    internal Field[] Fields() => fields.ToArray();
  }
}
