using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Controller
{
  class Table
  {
    private readonly PrimaryKey _identifier;

    public Table()
    {
      _identifier = new PrimaryKey(String.Format("ID_{0}", this.GetType().Name));
    }
    public virtual string Name    // Obtem o nome da tabela. Por padrão é o nome da classe.
    {
      get => this.GetType().Name;
    }
    public PrimaryKey Identifier  // Obtem a Field do tipo PrimaryKey da tabela.
    {
      get => _identifier;
    } 

    public List<Field> Fields     // Obtem todas as Field da tabela.         
    {
      get
      {
        // Lista a ser retornada.
        List<Field> fields = new List<Field>();

        // Recupera todas as propriedade da Classe.
        List<PropertyInfo> properties = new List<PropertyInfo>(this.GetType().GetProperties());

        // Itera para cada propriedade da classe.
        foreach (PropertyInfo property in properties)
          if (property.PropertyType.IsSubclassOf(typeof(Field)))
            fields.Add((Field)property.GetValue(this));

        return fields;
      }
    }

    internal string Signature             // Resgata a assinatura do elemento Table.
    {
      get
      {
        // Obtem uma lista de todos as assinaturas das fields da tabela.
        List<string> signatureFields = new List<string>();

        foreach (Field field in this.Fields)
        {
          if (field.GetType() != typeof(Relationship))
            signatureFields.Add(field.Signature);

          if (field.GetType() == typeof(ForeignKey))
          {
            signatureFields.Add(
                String.Format("CONSTRAINT {0} FOREIGN KEY ({1}) REFERENCES {2}({1})",
                  String.Format("FK_{0}_{1}",
                    this.Name,
                    ((Table)field.Value).Name
                  ),
                  field.Name,
                  ((Table)field.Value).Name
                  )
                );
          }
        }

        return String.Format(
          "TABLE {0} ({1})",
          this.Name,
          String.Join(
            ", ",
            signatureFields
            )
          );
      }
    }

    /*public abstract Table NewTable()
    { 
      return new Table();
    }*/
  }
}
