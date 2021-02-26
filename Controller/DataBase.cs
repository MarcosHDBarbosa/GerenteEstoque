using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GerenteEstoque.Controller
{
  internal class DataBase6
  {
    public static int Create(Table table)
    {
      int response = 0;

      // Cria a query de .
      string query = String.Format(
        @"if object_id('{0}') is null begin CREATE {1} end",
        table.Name,
        table.Signature
        );

      using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
      using (SqlCommand command = new SqlCommand(query, connection ))
      {
        // Solicida a abertura da conexão.
        connection.Open();

        try
        {
          command.ExecuteNonQuery();
          response = 1;
        }
        catch(Exception e)
        {
          MessageBox.Show(
            String.Format(
              "DataBase.Create Erro: {0}",
              e.Message
              )
            );
          response = -1;
        }
        finally
        {
          // Finaliza coneção.
          Dispose(connection);
        }
      }

      return response;
    }
    
    public static List<Table> Read(Type typeTable, Field[] searchConditions = null)
    {
      List<Table> response = new List<Table>();

      // Cria a string de busca.
      string query = String.Format(
        @"SELECT * FROM {0}",
        typeTable.Name
        );

      // Incere as condições caso existam.
      if (searchConditions?.Length > 0)
      {
        query = String.Concat(
          query,
          String.Format(@" WHERE {0} = @{1}",
            searchConditions[0].Name,
            searchConditions[0].Name
            )
          );

        for (int index = 1; index < searchConditions.Length; index++)
          query = String.Concat(
            query,
            String.Format(@" AND {0} = @{1}",
              searchConditions[index].Name,
              searchConditions[index].Name
              )
            );
      }

      using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
      using (SqlCommand command = new SqlCommand(query, connection))
      {
        // Solicida a abertura da conexão.
        connection.Open();

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
          // Seta como resposta um DataTable criado a partir da instância sqlDataReader retornada pela função ExecuteReader.
          //response = DataBase6.CreatDataTable(command.ExecuteReader());
          //response.TableName = typeTable.Name;
        }
        catch (Exception e)
        {
          MessageBox.Show(
            String.Format(
              "DataBase.Read Erro: {0}",
              e.Message
              )
            );
        }
        finally
        {
          // Finaliza coneção.
          Dispose(connection);
        }
      }

      return response;
    }

    public static int Update(Table table)
    {
      int response = 0;

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
          String.Join( 
            ", ",
            ParameterValue
            ),
          primaryKey.Name
          );
      }

      using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
      using (SqlCommand command = new SqlCommand(query, connection))
      {
        // Solicida a abertura da conexão.
        connection.Open();

        // Adiciona a chave primária a lista de fildes para a devida atribuição de valor pela adição de parametros do SqlCommand.
        fields.Add(primaryKey);
        // Adiciona parametros ao SqlCommand referenciando a string values
        foreach (Field field in fields)
        {
          command.Parameters.AddWithValue(
            String.Format("@{0}", field.Name),
            field.Value
            );
        }

        try
        {
          response = command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
          MessageBox.Show(
            String.Format(
              "DataBase.Create Erro: {0}",
              e.Message
              )
            );
          response = -1;
        }
        finally
        {
          // Finaliza coneção.
          Dispose(connection);
        }
      }

      return response;
    }

    private static DataTable CreatDataTable(SqlDataReader reader)
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
