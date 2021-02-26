using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace GerenteEstoque.Controller
{

  static class Controller
  {
    public static void Create(Table table)
    {
      // Cria a query de .
      string query = String.Format(
        @"if object_id('{0}') is null begin CREATE {1} end",
        table.Name,
        table.Signature
        );

      // Cria o SqlCommand com a query gerada.
      SqlCommand command = new SqlCommand(query);

      try
      {
        SQL_DataBase.Create(command);
      }
      catch (Exception e)
      {
        // retorna o exeção de erro gerada.
        throw e;
      }
    }

    // Função destinada a Leitura de dados
    // typeTable = O tipo de classe derivada de Table que determinará a tabela acesada e a lista retornada.
    // searchConditions = Lista de finelds que serão usadas como limitadores de busca. Seu valor SetValue não pode ser nulo.
    public static List<Table> Read(Type typeTable, List<Field> searchConditions)
    {
      List<Table> tables = new List<Table>();
      // Limita a busca aos tipos derivados da classe Table.
      if (typeTable.IsSubclassOf(typeof(Table)))
      {
        // Cria a string de busca.
        string query = String.Format(
          @"SELECT * FROM {0}",
          typeTable.Name
          );

        // Incere as condições caso existam.
        if (searchConditions?.Count > 0)
        {
          query = String.Concat(
            query,
            String.Format(@" WHERE {0} = @{1}",
              searchConditions[0].Name,
              searchConditions[0].Name
              )
            );

          for (int index = 1; index < searchConditions.Count; index++)
            query = String.Concat(
              query,
              String.Format(@" AND {0} = @{1}",
                searchConditions[index].Name,
                searchConditions[index].Name
                )
              );
        }

        SqlCommand command = new SqlCommand(query);

        // Adiciona parametros ao SqlCommand referenciando a string values
        foreach (Field field in searchConditions)
        {
          command.Parameters.AddWithValue(
            String.Format("@{0}", field.Name),
            field.Value
            );
        }

        try
        {
          // Obtem o DataTabre como a operação reader seja bem sussedida e itera para cada Linha.
          foreach (DataRow row in SQL_DataBase.Read(command).Rows)
          {
            // Cria uma instância do typo fornecido pelo typeTable recebido por parâmetro.
            Table table = (Table)typeTable.GetConstructor(new Type[0]).Invoke(new object[0]);

            // Para cada Field da nova tabela criada itera.
            foreach(Field field in table.Fields)
            {
              // Caso a field seja do tipo ForeignKey faz.
              if (field.GetType() == typeof(ForeignKey))
              {
                // Faz uma busca na tabela do tipo fornecido pela ForeingKey com o identificador resgatado pela ultima busca.
                List<Table> ForeignTable = Controller.Read(
                  ((ForeignKey)field).TypeForingn,
                  new List<Field>(){
                        new Field(
                          field.Name,
                          row?[field.Name]
                          )
                    }
                  );
                if (ForeignTable.Count > 0)
                  field.Value = ForeignTable[0];
                // Caso não tenha encontrado nada, interrompe o ciclo atual.
                else
                  goto end;
              }
              // Se não apenas adiciona o valor.
              else if (field.GetType() != typeof(Relationship))
                field.Value = row?[field.Name];
            }

            // Para cada elemento Field dotipo Relationship faz.
            foreach (Field field in table.Fields.FindAll(x => x.GetType() == typeof(Relationship)))
            {
              // Resgata a chave primária da instância primária do Relationship.
              Field primaryKey = new List<Field>(
                ((Relationship)field).Table01.Fields
                ).Find(x => x.GetType() == typeof(PrimaryKey));

              // Busca todas as RelationshipTables atreladas a chave primaria resgatada anteriormente.
              // O typeTable01 representa a própria instâncias buscada, e é definida como Type do tipo Table pois assim a função Read rertorna uma tabela vazia. 
              List<RelationshipTable> relationshipTables = Controller.ReadRelationshipTables(
                ((Relationship)field).Name,
                typeof(Table),
                ((Relationship)field).typeTable02,
                new List<Field>() { primaryKey }
                );

              // Atualiza o Relationship com as RelationshipTables resgatadas
              ((Relationship)field).Value = relationshipTables;
            }

            // Adiciona a nova tabela a lista.
            tables.Add(table);
          end:;
          }
        }
        catch (Exception e)
        {
          // retorna o exeção de erro gerada.
          throw e;
        }
      }
      else if (typeTable == typeof(Table))
        tables.Add(new Table());

      return tables;
    }

    public static List<RelationshipTable> ReadRelationshipTables(string RalationshipName, Type table01, Type table02, List<Field> searchConditions)
    {
      List<RelationshipTable> relationshipTables = new List<RelationshipTable>();

      // Cria a string de busca.
      string query = String.Format(
        @"SELECT * FROM {0}",
        RalationshipName
        );

      // Incere as condições caso existam.
      if (searchConditions?.Count > 0)
      {
        query = String.Concat(
          query,
          String.Format(@" WHERE {0} = @{1}",
            searchConditions[0].Name,
            searchConditions[0].Name
            )
          );

        for (int index = 1; index < searchConditions.Count; index++)
          query = String.Concat(
            query,
            String.Format(@" AND {0} = @{1}",
              searchConditions[index].Name,
              searchConditions[index].Name
              )
            );
      }

      SqlCommand command = new SqlCommand(query);

      // Adiciona parametros ao SqlCommand referenciando a string values
      foreach (Field field in searchConditions)
      {
        command.Parameters.AddWithValue(
          String.Format("@{0}", field.Name),
          field.Value
          );
      }

      try
      {
        DataTable data = SQL_DataBase.Read(command);

        // Obtem o nome das Colunas, que são os nomes da chaves estrageiras.
        List<string> colunName = new List<string>();
        for (int index = 0; index < data.Columns.Count; index++)
          colunName.Add(data.Columns[index].ColumnName);

        foreach (DataRow row in data.Rows)
        {
          // Cria uma instância RelationshipTable buscando as duas tabelas relacioandas filtras pelas ForeignKeys resgatadas.
          RelationshipTable table = new RelationshipTable(
            Controller.Read(table01, new List<Field>() { new Field(colunName[0], row[colunName[0]]) })[0],
            Controller.Read(table02, new List<Field>() { new Field(colunName[1], row[colunName[1]]) })[0]
            );
          table.Identifier.Value = row[colunName[2]];
          table.SetName = RalationshipName;

          // Adiciona a nova tabela a lista.
          relationshipTables.Add(table);
        }
      }
      catch (Exception e)
      {
        // retorna o exeção de erro gerada.
        throw e;
      }

      return relationshipTables;
    }

    public static void Update(Table table)
    {
      // Resgata a lista de instâncias Field da instância Table corrente.
      List<Field> fields = new List<Field>(table.Fields);

      // Recupera a Field relacionada a primaryKey.
      Field primaryKey = fields.Find(x => x.GetType() == typeof(PrimaryKey));

      // Recupera as Fields relacionada a Relationship.
      List<Field> relationships = fields.FindAll(x => x.GetType() == typeof(Relationship));

      // Remove a primaryKey da list de Fields.
      fields.Remove(primaryKey);
      // Remove a primaryKey da list de Fields.
      fields.RemoveAll(x => x.GetType() == typeof(Relationship));

      // Inicializa a string de inserção para cada item da lista.
      string query = @"";
      // Não gera uma quere para uma lista de fields fazia.
      if (fields.Count > 0)
      {
        // Cria uma lista de string com base nas fields da instância Tabela
        List<string> ParameterValue = new List<string>();
        foreach (Field field in fields)
          ParameterValue.Add(
            String.Format(
              " {0} = @{0} ",
              field.Name
              )
            );

        query += String.Format(
          @"UPDATE {0} SET {1} WHERE {2} = @{2}",
          table.Name,
          String.Join(", ", ParameterValue),
          primaryKey.Name
          );
      }

      // Cria o SqlCommand com a query gerada.
      SqlCommand command = new SqlCommand(query);

      // Adiciona a chave primária a lista de fildes para a devida atribuição de valor pela adição de parametros do SqlCommand.
      fields.Add(primaryKey);
      // Adiciona parametros ao SqlCommand referenciando a string values
      foreach (Field field in fields)
      {
        if(field.GetType() == typeof(ForeignKey))
          command.Parameters.AddWithValue(
            String.Format("@{0}", field.Name),
            ((Table)((ForeignKey)field).Value).Identifier.Value
            );
        else
          command.Parameters.AddWithValue(
            String.Format("@{0}", field.Name),
            field.Value
            );
      }

      try
      {
        SQL_DataBase.Update(command);

        // Usa as fields resgatadas do tipo Relationship e faz uma iteração para cada uma.
        foreach (Relationship relationship in relationships)
        {
          // Carrega todas as RelationshipTable relacionadas a Relationship corrente.
          // Como o objetivo é apenas resgatar o ID da RelationshipTable, os tipos da tabela são informados como Table para resgatarem uma tabela vazia. 
          List<RelationshipTable> relationshipTablesDataBase = Controller.ReadRelationshipTables(
            relationship.Name,
            typeof(Table),
            typeof(Table),
            new List<Field>(){
                primaryKey
              }
            );
          // Resgata as RelationshipTable do Relatioanship da tabela corrente.
          List<RelationshipTable> relationshipTablesCurrent = (List<RelationshipTable>)relationship.Value;

          // Atualiza, Deleta ou Cria RelationshipTable conforme as condições abaixo
          for (int index = 0; (index < relationshipTablesDataBase.Count || index < relationshipTablesCurrent.Count); index++)
          {
            // Caso o numero de RelationshipTable o BANCO e no CORRENTE sejam menores que index:
            // Atualiza RelationshipTable do BANCO com os valores do CORRENTE.
            if (index < relationshipTablesDataBase.Count && index < relationshipTablesCurrent.Count)
            {
              // Recupera a Field da relationshipTablesDataBase relacionada a primaryKey.
              Field primaryKeyRelationship =
                new List<Field>(relationshipTablesDataBase[index].Fields)
                .Find(x => x.GetType() == typeof(PrimaryKey));

              // Atualiza o identificador da relationshipTablesCurrent
              relationshipTablesCurrent[index].Identifier.Value = primaryKeyRelationship.Value;

              // Atualiza a RelationshipTable
              Controller.Update(relationshipTablesCurrent[index]);
            }

            // Caso o numero de RelationshipTable do BANCO seja menor do que o index:
            // Insere RelationshipTable ao BANCO com os valores do CORRENTE.
            else if (index >= relationshipTablesDataBase.Count && index < relationshipTablesCurrent.Count)
              Controller.Insert(relationshipTablesCurrent[index]);

            // Caso o numero de RelationshipTable do CORRENTE seja menor do que o index:
            // Deleta RelationshipTable do BANCO.
            else if (index < relationshipTablesDataBase.Count && index >= relationshipTablesCurrent.Count)
              Controller.Delete(relationshipTablesDataBase[index]);
          }
        }
      }
      catch (Exception e)
      {
        // retorna o exeção de erro gerada.
        throw e;
      }
    }

    public static void Delete(Table table)
    {
      // Recupera a Field relacionada a primaryKey.
      Field primaryKey = new List<Field>(table.Fields)
        .Find(x => x.GetType() == typeof(PrimaryKey));

      // Cria a quere de Remoção.
      string query = @"";
      query += String.Format(
        @"DELETE FROM {0} WHERE {1} = @{1}",
        table.Name,
        primaryKey.Name
        );

      // Cria o SqlCommand com a query gerada.
      SqlCommand command = new SqlCommand(query);

      // Adiciona o parametro, referente a PrimaryKey, ao comando.
      command.Parameters.AddWithValue(
        String.Format("@{0}", primaryKey.Name),
        primaryKey.Value
        );

      try
      {
        // deleta as RelationshipTables de cada field do tipo Relatingship
        foreach (Field field in table.Fields.FindAll(x => x.GetType() == typeof(Relationship)))
        {
          // Resgata todas as RelationshipTable que possuem como chave estrangeira a chava primária da instancia atual.
          // Os tipos das tabelas não são especificador pois o objetivo é obter apenas o identificador da RelationshipTable.
          List<RelationshipTable> relationshipTables = Controller.ReadRelationshipTables(
            field.Name,
            typeof(Table),
            typeof(Table),
            new List<Field>() {
                  primaryKey
              }
            );

          // Chama a função Deleta para cada RelationshipTable encontrada.
          foreach (RelationshipTable relationshipTable in relationshipTables)
            Delete(relationshipTable);
        }

        SQL_DataBase.Delete(command);
      }
      catch (Exception e)
      {
        // retorna o exeção de erro gerada.
        throw e;
      }

      
    }

    public static void Insert(Table table)
    {
      // Resgata a lista de instâncias Field da instância Table corrente.
      List<Field> fields = new List<Field>(table.Fields);

      // Recupera a Field relacionada a primaryKey.
      Field primaryKey = fields.Find(x => x.GetType() == typeof(PrimaryKey));
      // Remove a primaryKey da list de Fields.
      fields.Remove(primaryKey);

      // Cria a quere de inserção.
      string queue = @"";

      //Cria uma lista com os nomes das Fields da tabela
      List<string> names = new List<string>();
      foreach (Field field in fields)
        if (field.GetType() != typeof(Relationship))
          names.Add(field.Name);

      // Lista os parâmetros da tabela com base na lista Field da instância Table.
      string parameters = String.Join(", ", names);
      // Lista os valores da tabela com base na lista Field da instância Table.
      string values = String.Concat("@", String.Join(", @", names));

      // Finaliza a string de busca para este item
      queue += String.Format(
        @"INSERT INTO {0} ({1}) values ({2})
            SELECT {3} FROM {0} WHERE {3} = @@Identity",
        table.Name,
        parameters,
        values,
        primaryKey.Name
        );

      // Cria a instância SqlCommand.
      SqlCommand command = new SqlCommand(queue);

      // Adiciona parametros ao SqlCommand referenciando a string values
      foreach (Field field in fields)
        if (field.GetType() == typeof(ForeignKey))
          command.Parameters.AddWithValue(
            String.Format("@{0}", field.Name),
            ((Table)field.Value).Identifier.Value
            );
        else if (field.GetType() != typeof(Relationship))
          command.Parameters.AddWithValue(
            String.Format("@{0}", field.Name),
            field.Value
            );
      try
      {
        foreach (DataRow row in SQL_DataBase.Insert(command).Rows)
        {
          // Atualiza a PrimaryKey da tabela corrente
          table.Identifier.Value = row[primaryKey.Name];

          // Insere RelationshipTables para cada field do tipo Relatingship
          foreach (Field field in table.Fields.FindAll(x => x.GetType() == typeof(Relationship)))
            foreach (RelationshipTable relationshipTable in (List<RelationshipTable>)field.Value)
              Controller.Insert(relationshipTable);
        }

        
      }
      catch (Exception e)
      {
        // retorna o exeção de erro gerada.
        throw e;
      }
    }
  }
}
