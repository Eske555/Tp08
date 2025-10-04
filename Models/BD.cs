using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;

public static class BD
{ 
      private static string _connectionString =
        @"Server=localhost\SQLEXPRESS;Database=PreguntadOrt;Integrated Security=True;TrustServerCertificate=True;";    
    public static List<Categorias> ObtenerCategorias()
    {
    using var connection = new SqlConnection(_connectionString);
    const string query = "SELECT * FROM Categorias ORDER BY IdCategoria";
    return connection.Query<Categorias>(query).ToList();
    }
    public static List<Dificultades> ObtenerDificultades()
    {
    using var connection = new SqlConnection(_connectionString);
    const string query = "SELECT * FROM Dificultades ORDER BY IdDificultad";
    return connection.Query<Dificultades>(query).ToList();
    }
    public static List<Preguntas> ObtenerPreguntas(int dificultad, int categoria)
    {
        using var connection = new SqlConnection(_connectionString);
        var parametros = new DynamicParameters();
        string query = "SELECT * FROM Preguntas WHERE 1=1";
        if (dificultad != -1)
        {
            query += " AND IdDificultad = @Dificultad";
            parametros.Add("Dificultad", dificultad);
        }
        if (categoria != -1)
        {
            query += " AND IdCategoria = @Categoria";
            parametros.Add("Categoria", categoria);
        }
        return connection.Query<Preguntas>(query, parametros).ToList();
    }
    public static List<Respuestas> ObtenerRespuestas(int idPregunta)
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT * FROM Respuestas WHERE IdPregunta = @IdPregunta";
        return connection.Query<Respuestas>(query, new { IdPregunta = idPregunta }).ToList();
    }
}
