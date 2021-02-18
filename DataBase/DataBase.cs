using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace GerenteEstoque.DataBase
{

  static class DataBase
  {
    /*private static DataTable GetDataTable(SqlDataReader reader)
    {
      DataTable schema = reader.GetSchemaTable();
      DataTable dataTable = new DataTable();

      foreach (DataRow row in schema.Rows)
      {
        if (!dataTable.Columns.Contains(row["ColumnName"].ToString()))
        {
          DataColumn col = new DataColumn()
          {
            ColumnName = row["ColumnName"].ToString(),
            Unique = Convert.ToBoolean(row["IsUnique"]),
            AllowDBNull = Convert.ToBoolean(row["AllowDBNull"]),
            ReadOnly = Convert.ToBoolean(row["IsReadOnly"])
          };
          dataTable.Columns.Add(col);
        }
      }

      while (reader.Read())
      {
        DataRow novaLinha = dataTable.NewRow();
        for (int i = 0; i < dataTable.Columns.Count; i++)
        {
          novaLinha[i] = reader.GetValue(i);
        }
        dataTable.Rows.Add(novaLinha);
      }

      return dataTable;
    }

    /*
    private static DataTable ForeignKeyDataTable(SqlConnection connection)
    {
      SqlCommand command = new SqlCommand(
        String.Format(@"
        SELECT
            OBJECT_NAME(fk.parent_object_id) 'Parent table',
            c1.name 'Parent column',
            OBJECT_NAME(fk.referenced_object_id) 'Referenced table'
        FROM 
            sys.foreign_keys fk
        INNER JOIN 
            sys.foreign_key_columns fkc ON fkc.constraint_object_id = fk.object_id
        INNER JOIN
            sys.columns c1 ON fkc.parent_column_id = c1.column_id AND fkc.parent_object_id = c1.object_id
        INNER JOIN
            sys.columns c2 ON fkc.referenced_column_id = c2.column_id AND fkc.referenced_object_id = c2.object_id
        "),
        connection
        );
      SqlDataReader reader = command.ExecuteReader();

      return GetDataTable(reader);
    }
    */
    public static void Create(List<Table> tables)
    {

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
        // Cria a instância SqlConnection e sua respectiva string de conexão.
        using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
        {
          // Solicida a abertura da conexão.
          connection.Open();

          // Cria a string de busca.
          string queue = String.Format(@"SELECT * FROM {0}", typeTable.Name);
          // Incere as condições caso existam. 
          if (searchConditions?.Count > 0)
          {
            queue = String.Concat(
              queue,
              String.Format(@" WHERE {0} IN ({1})",
                searchConditions[0].Name(),
                searchConditions[0].Value()
                )
              );
            for (int index = 1; index < searchConditions.Count; index++)
              queue = String.Concat(
                queue,
                String.Format(@" AND {0} IN ({1})",
                  searchConditions[index].Name(),
                  searchConditions[index].Value()
                  )
                );
          }

          // Cria a instância SqlCommand.
          SqlCommand command = new SqlCommand(
            queue,
            connection
            );

          try
          {
            // Inicializa o reader com a resposta da instância SqlCommand
            SqlDataReader reader = command.ExecuteReader();

            // Por meio da instância SqlDataReader escanea cada linha de acesso.
            while (reader.Read())
            {
              // Cria uma instância do typo fornecido pelo typeTable recebido por parâmetro.
              Table table = (Table)typeTable.GetConstructor(new Type[0]).Invoke(new object[0]);

              // Escaneia cada elemento field da isntância Table e atualiza os valores da mesma.
              foreach (Field field in table.Fields())
              {
                // Caso a field seja do tipo ForeignKey faz.
                if (field.GetType() == typeof(ForeignKey))
                {
                  // Faz uma busca na tabela do tipo fornecido pela ForeingKey com o identificador resgatado pela ultima busca.
                  List<Table> ForeignTable = DataBase.Read(
                    ((ForeignKey)field).ForeignTableType,
                    new List<Field>(){
                        new Field(
                          field.Name(),
                          () => reader?[field.Name()]
                          )
                      }
                    );
                  // Adiciona o item regatado
                  if (ForeignTable.Count > 0)
                    field.Value(ForeignTable[0]);
                  // Caso não tenha encontrado nada, interrompe o ciclo atual.
                  else
                    goto end;
                }
                // Caso a field seja do tipo Relationship faz.
                else if (field.GetType() == typeof(Relationship))
                {
                  // Resgata a chave primária da instância primária do Relationship.
                  Field primaryKey = new List<Field>(
                    ((Relationship)field).Table01.Fields()
                    ).Find(x => x.GetType() == typeof(PrimaryKey));

                  // Busca todas as RelationshipTables atreladas a chave primaria resgatada anteriormente.
                  // O typeTable01 representa a própria instâncias buscada, e é definida como Type do tipo Table pois assim a função Read rertorna uma tabela vazia. 
                  List<RelationshipTable> relationshipTables = DataBase.ReadRelationshipTables(
                    ((Relationship)field).Name(),
                    typeof(Table),
                    ((Relationship)field).typeTable02,
                    new List<Field>() { primaryKey }
                    );

                  // Atualiza o Relationship com as RelationshipTables resgatadas
                  ((Relationship)field).Value(relationshipTables);
                }
                // Se não apenas adiciona o valor.
                else
                  field.Value(reader?[field.Name()]);
              }

              // Adiciona a nova tabela a lista.
              tables.Add(table);
            end:;
            }
          }
          catch (Exception e)
          {
            string message = e.Message;
          }
          finally
          {
            // Finaliza a conexão
            Dispose(connection);
          }
        }
      }
      else if (typeTable == typeof(Table))
        tables.Add(Table.GetTable());

      return tables;
    }
    public static List<RelationshipTable> ReadRelationshipTables(string RalationshipName, Type table01, Type table02, List<Field> searchConditions)
    {
      List<RelationshipTable> relationshipTables = new List<RelationshipTable>();

      using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
      {
        // Solicida a abertura da conexão.
        connection.Open();
        // Cria a string de busca.
        string queue = String.Format(@"SELECT * FROM {0}", RalationshipName);
        // Incere as condições caso existam. 
        if (searchConditions?.Count > 0)
        {
          queue = String.Concat(
            queue,
            String.Format(@" WHERE {0} IN ({1})",
              searchConditions[0].Name(),
              searchConditions[0].Value()
              )
            );
          for (int index = 1; index < searchConditions.Count; index++)
            queue = String.Concat(
              queue,
              String.Format(@" AND {0} IN ({1})",
                searchConditions[index].Name(),
                searchConditions[index].Value()
                )
              );
        }

        // Cria a instância SqlCommand.
        SqlCommand command = new SqlCommand(
          queue,
          connection
          );

        try
        {
          // Inicializa o reader com a resposta da instância SqlCommand.
          SqlDataReader reader = command.ExecuteReader();
          
          // Obtem o nome das Colunas, que são os nomes da chaves estrageiras.
          List<string> colunName = new List<string>();
          for (int index = 0; index < reader.FieldCount; index++)
            colunName.Add(reader.GetName(index));


          // Por meio da instância SqlDataReader escanea cada linha de acesso.
          while (reader.Read())
          {
            // Cria uma instância RelationshipTable buscando as duas tabelas relacioandas filtras pelas ForeignKeys resgatadas.
            RelationshipTable table = new RelationshipTable(
              DataBase.Read(table01, new List<Field>() { new Field(colunName[1], () => reader[colunName[1]]) })[0],
              DataBase.Read(table02, new List<Field>() { new Field(colunName[2], () => reader[colunName[2]]) })[0]
              );
            table.SetID(reader[colunName[0]]);
            table.TableName(RalationshipName);

            // Adiciona a nova tabela a lista.
            relationshipTables.Add(table);
          }
        }
        catch (Exception e)
        {
          string message = e.Message;
        }
        finally
        {
          // Finaliza a conexão
          Dispose(connection);
        }
      }

      return relationshipTables;
    }

    public static int Update(Table table)
    {
      int response = 0;
      // Cria a instância SqlConnection e sua respectiva string de conexão.
      using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
      {
        // Solicida a abertura da conexão.
        connection.Open();

        // Resgata a lista de instâncias Field da instância Table corrente.
        List<Field> fields = new List<Field>(table.Fields());
        // Recupera a Field relacionada a primaryKey.
        Field primaryKey = fields.Find(x => x.GetType() == typeof(PrimaryKey));
        // Recupera as Fields relacionada a Relationship.
        List<Field> relationships = fields.FindAll(x => x.GetType() == typeof(Relationship));
        // Remove a primaryKey da list de Fields.
        fields.Remove(primaryKey);
        // Remove a primaryKey da list de Fields.
        fields.RemoveAll(x => x.GetType() == typeof(Relationship));

        // Inicializa a string de inserção para cada item da lista.
        string queue = @"";
        // Não gera uma quere para uma lista de fields fazia.
        if (fields.Count > 0)
        {
          // Cria uma lista de string com base nas fields da instância Tabela
          List<string> ParameterValue = new List<string>();
          foreach (Field field in fields)
            ParameterValue.Add(
              String.Format(
                " {0} = @{0} ",
                field.Name()
                )
              );

          queue += String.Format(
            @"UPDATE {0} SET {1} WHERE {2} = @{2}",
            table.TableName(),
            String.Join(", ", ParameterValue),
            primaryKey.Name()
            );
        }

        // Cria a instância SqlCommand.
        SqlCommand command = new SqlCommand(
          queue,
          connection
          );

        // Adiciona a chave primária a lista de fildes para a devida atribuição de valor pela adição de parametros do SqlCommand.
        fields.Add(primaryKey);
        // Adiciona parametros ao SqlCommand referenciando a string values
        foreach (Field field in fields)
        {
          command.Parameters.AddWithValue(
            String.Format("@{0}", field.Name()),
            field.Value()
            );
        }

        try
        {
          // Executa comando e obtem o numero de instâncias Atualizadas.
          response += command.ExecuteNonQuery();

          // Usa as fields resgatadas do tipo Relationship e faz uma iteração para cada uma.
          foreach (Relationship relationship in relationships)
          {
            // Carrega todas as RelationshipTable relacionadas a Relationship corrente.
            // Como o objetivo é apenas resgatar o ID da RelationshipTable, os tipos da tabela são informados como Table para resgatarem uma tabela vazia. 
            List<RelationshipTable> relationshipTablesDataBase = DataBase.ReadRelationshipTables(
              relationship.Name(),
              typeof(Table),
              typeof(Table),
              new List<Field>(){
                primaryKey
                }
              );
            // Resgata as RelationshipTable do Relatioanship da tabela corrente.
            List<RelationshipTable> relationshipTablesCurrent = (List<RelationshipTable>) relationship.Value();

            // Atualiza, Deleta ou Cria RelationshipTable conforme as condições abaixo
            for(int index = 0; (index < relationshipTablesDataBase.Count || index < relationshipTablesCurrent.Count); index++)
            {
              // Caso o numero de RelationshipTable o BANCO e no CORRENTE sejam menores que index:
              // Atualiza RelationshipTable do BANCO com os valores do CORRENTE.
              if (index < relationshipTablesDataBase.Count && index < relationshipTablesCurrent.Count)
              {
                // Recupera a Field da relationshipTablesDataBase relacionada a primaryKey.
                Field primaryKeyRelationship =
                  new List<Field>(relationshipTablesDataBase[index].Fields())
                  .Find(x => x.GetType() == typeof(PrimaryKey));

                // Atualiza o identificador da relationshipTablesCurrent
                relationshipTablesCurrent[index].SetID(primaryKeyRelationship.Value());

                // Atualiza a RelationshipTable
                response += DataBase.Update(relationshipTablesCurrent[index]);
              }

              // Caso o numero de RelationshipTable do BANCO seja menor do que o index:
              // Insere RelationshipTable ao BANCO com os valores do CORRENTE.
              else if (index >= relationshipTablesDataBase.Count && index < relationshipTablesCurrent.Count)
                response += DataBase.Insert(relationshipTablesCurrent[index]);

              // Caso o numero de RelationshipTable do CORRENTE seja menor do que o index:
              // Deleta RelationshipTable do BANCO.
              else if (index < relationshipTablesDataBase.Count && index >= relationshipTablesCurrent.Count)
                response += DataBase.Delete(relationshipTablesDataBase[index]);
            }
          }
        }
        catch (Exception e)
        {
          string message = e.Message;
        }
        finally
        {
          // Finaliza a conexão
          Dispose(connection);
        }
      }
      return response;
    }
    public static int Delete(Table table)
    {
      int response = 0;
      // Cria a instância SqlConnection e sua respectiva string de conexão.
      using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
      {
        // Solicida a abertura da conexão.
        connection.Open();

        // Recupera a Field relacionada a primaryKey.
        Field primaryKey = new List<Field>(table.Fields())
          .Find(x => x.GetType() == typeof(PrimaryKey));

        // Cria a quere de inserção.
        string queue = @"";
        queue += String.Format(
          @"DELETE FROM {0} WHERE {1} = @{1} ",
          table.TableName(),
          primaryKey.Name()
          );

        // Cria a instância SqlCommand.
        SqlCommand command = new SqlCommand(
          queue,
          connection
          );

        // Adiciona o parametro, referente a PrimaryKey, ao comando.
        command.Parameters.AddWithValue(
          String.Format("@{0}", primaryKey.Name()),
          primaryKey.Value()
          );

        try
        {
          // deleta as RelationshipTables de cada field do tipo Relatingship
          foreach (Field field in table.Fields())
            if (field.GetType() == typeof(Relationship))
            {
              // Resgata todas as RelationshipTable que possuem como chave estrangeira a chava primária da instancia atual.
              // Os tipos das tabelas não são especificador pois o objetivo é obter apenas o identificador da RelationshipTable.
              List<RelationshipTable> relationshipTables = DataBase.ReadRelationshipTables(
                field.Name(),
                typeof(Table),
                typeof(Table),
                new List<Field>() {
                  primaryKey
                  }
                );

              // Chama a função Deleta para cada RelationshipTable encontrada.
              foreach (RelationshipTable relationshipTable in relationshipTables)
                response += DataBase.Delete(relationshipTable);
            }
              

          // Executa comando e obtem o numero de instâncias deletadas.
          response += command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
          string message = e.Message;
        }
        finally
        {
          // Finaliza a conexão
          Dispose(connection);
        }
      }
      return response;
    }

    public static int Insert(Table table)
    {
      int response = 0;
      // Cria a instância SqlConnection e sua respectiva string de conexão.
      using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
      {
        // Solicida a abertura da conexão.
        connection.Open();

        // Resgata a lista de instâncias Field da instância Table corrente.
        List<Field> fields = new List<Field>(table.Fields());
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
            names.Add(field.Name());

        // Lista os parâmetros da tabela com base na lista Field da instância Table.
        string parameters = String.Join(", ", names);
        // Lista os valores da tabela com base na lista Field da instância Table.
        string values = String.Concat("@", String.Join(", @", names));

        // Finaliza a string de busca para este item
        queue += String.Format(
          @"INSERT INTO {0} ({1}) values ({2})
            SELECT {3} FROM {0} WHERE {3} = @@Identity",
          table.TableName(),
          parameters,
          values,
          primaryKey.Name()
          );

        // Cria a instância SqlCommand.
        SqlCommand command = new SqlCommand(
          queue,
          connection
          );

        // Adiciona parametros ao SqlCommand referenciando a string values
        foreach (Field field in fields)
          if (field.GetType() != typeof(Relationship))
            command.Parameters.AddWithValue(
              String.Format("@{0}", field.Name()),
              field.Value()
              );

        try
        {
          // Inicializa o reader com a resposta da instância SqlCommand
          SqlDataReader reader = command.ExecuteReader();
          while(reader.Read())
          {
            response = 1;
            // Atualiza a PrimaryKey da tabela corrente
            table.SetID(reader[primaryKey.Name()]);
            // Insere RelationshipTables para cada field do tipo Relatingship
            foreach (Field field in table.Fields())
              if (field.GetType() == typeof(Relationship))
                foreach (RelationshipTable relationshipTable in (List<RelationshipTable>)field.Value())
                  response += DataBase.Insert(relationshipTable);
          }
        }
        catch (Exception e)
        {
          string message = e.Message;
        }
        finally
        {
          // Finaliza a conexão
          Dispose(connection);
        }
      }
      return response;
    }

    private static void Dispose(SqlConnection connection)
    {
      try
      {
        if (connection.State != ConnectionState.Closed)
        {
          connection.Close();
          connection.Dispose();
        }
      }
      catch (Exception e)
      {
        throw e;
      }
    }
  }
}
