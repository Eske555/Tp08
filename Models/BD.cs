using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;

public static class BD
{ 
      private static string _connectionString =
        @"Server=localhost;Database=Tp06;Integrated Security=True;TrustServerCertificate=True;";    
    public static List<Categorias> ObtenerCategorias()
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT * FROM Categorias";
        return connection.Query<Categorias>(query).ToList();
    }
    public static List<Dificultades> ObtenerDificultades()
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT * FROM Dificultades";
        return connection.Query<Dificultades>(query).ToList();
    }
    public static List<Preguntas> ObtenerPreguntas(int dificultad, int categoria)
    {
        using var connection = new SqlConnection(_connectionString);

        string query = "SELECT * FROM Preguntas WHERE 1=1";

        if (dificultad != -1)
            query += " AND IdDificultad = @Dificultad";

        if (categoria != -1)
            query += " AND IdCategoria = @Categoria";

        return connection.Query<Preguntas>(query, new { Dificultad = dificultad, Categoria = categoria }).ToList();
    }
    public static List<Respuestas> ObtenerRespuestas(int idPregunta)
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT * FROM Respuestas WHERE IdPregunta = @IdPregunta";
        return connection.Query<Respuestas>(query, new { IdPregunta = idPregunta }).ToList();
    }
}
