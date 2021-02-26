using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenteEstoque.Controller
{
  static class SQL_DataBase
  {
    public static void Create(SqlCommand command)
    {
      // Avalia se o CommandText da instância command não corresponde a expressão regular do função.
      /*if (System.Text.RegularExpressions.Regex.Match(command.CommandText, "if object_id('{0}') is null begin CREATE {1} end"))
        throw new ArgumentException(
          String.Format(
            "Formatação da QUERY não coresponde com o expressão regular do metodo Create:\n{0}",
            command.CommandText
            )
          );*/

      using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
      {
        // Solicida a abertura da conexão e a atribui ao command.
        connection.Open();
        command.Connection = connection;

        try
        {
          // Executa o comando.
          command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
          // retorna o exeção de erro gerada.
          throw e;
        }
        finally
        {
          // Finaliza coneção.
          Dispose(connection);
        }
      }
    }

    public static DataTable Read(SqlCommand command)
    {
      DataTable response = new DataTable();

      // Avalia se o CommandText da instância command não corresponde a expressão regular do função.
      /*if (System.Text.RegularExpressions.Regex.Match(command.CommandText, "SELECT * FROM {0}"))
        throw new ArgumentException(
          String.Format(
            "Formatação da QUERY não coresponde com o expressão regular do metodo Read:\n{0}",
            command.CommandText
            )
          );*/

      using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
      {
        // Solicida a abertura da conexão e a atribui ao command.
        connection.Open();
        command.Connection = connection;

        try
        {
          // Cria um DataTable por meio da função CreateDataTable com a referência da intância reader
          response = CreateDataTable(
            command.ExecuteReader() // Executa o comando
            );
        }
        catch (Exception e)
        {
          // retorna o exeção de erro gerada.
          throw e;
        }
        finally
        {
          // Finaliza coneção.
          Dispose(connection);
        }
      }

      return response;
    }

    public static void Update(SqlCommand command)
    {
      // Avalia se o CommandText da instância command não corresponde a expressão regular do função.
      /*if (System.Text.RegularExpressions.Regex.Match(command.CommandText, "UPDATE {0} SET {1} WHERE {2} = @{2}"))
        throw new ArgumentException(
          String.Format(
            "Formatação da QUERY não coresponde com o expressão regular do metodo Update:\n{0}",
            command.CommandText
            )
          );*/

      using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
      {
        // Solicida a abertura da conexão e a atribui ao command.
        connection.Open();
        command.Connection = connection;

        try
        {
          // Executa o comando.
          command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
          // retorna o exeção de erro gerada.
          throw e;
        }
        finally
        {
          // Finaliza coneção.
          Dispose(connection);
        }
      }
    }

    public static void Delete(SqlCommand command)
    {
      // Avalia se o CommandText da instância command não corresponde a expressão regular do função.
      /*if (System.Text.RegularExpressions.Regex.Match(command.CommandText, "DELETE FROM {0} WHERE {1} = @{1}"))
        throw new ArgumentException(
          String.Format(
            "Formatação da QUERY não coresponde com o expressão regular do metodo Delete:\n{0}",
            command.CommandText
            )
          );*/

      using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
      {
        // Solicida a abertura da conexão e a atribui ao command.
        connection.Open();
        command.Connection = connection;

        try
        {
          // Executa o comando.
          command.ExecuteNonQuery();
        }
        catch (Exception e)
        {
          // retorna o exeção de erro gerada.
          throw e;
        }
        finally
        {
          // Finaliza coneção.
          Dispose(connection);
        }
      }
    }

    public static DataTable Insert(SqlCommand command)
    {
      DataTable response = new DataTable();

      // Avalia se o CommandText da instância command não corresponde a expressão regular do função.
      /*if (System.Text.RegularExpressions.Regex.Match(command.CommandText,
        "INSERT INTO {0} ({1}) values ({2}) SELECT {3} FROM {0} WHERE {3} = @@Identity"))
        throw new ArgumentException(
          String.Format(
            "Formatação da QUERY não coresponde com o expressão regular do metodo Insert:\n{0}",
            command.CommandText
            )
          );*/

      using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.EstoqueDatabaseConnectionString))
      {
        // Solicida a abertura da conexão e a atribui ao command.
        connection.Open();
        command.Connection = connection;

        try
        {
          // Cria um DataTable por meio da função CreateDataTable com a referência da intância reader
          response = CreateDataTable(
            command.ExecuteReader() // Executa o comando
            );
        }
        catch (Exception e)
        {
          // retorna o exeção de erro gerada.
          throw e;
        }
        finally
        {
          // Finaliza coneção.
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
        // retorna o exeção de erro gerada.
        throw e;
      }
    }

    private static DataTable CreateDataTable(SqlDataReader reader)
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
  }
}
