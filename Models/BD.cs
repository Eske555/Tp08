using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

public static class BD
{
    private const string _connectionString =
            @"Server=localhost\SQLEXPRESS;Database=PreguntadOrt;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true";
    public static List<Categorias> ObtenerCategorias()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            const string sql = @"SELECT IdCategoria, Nombre FROM Categorias ORDER BY IdCategoria";
            return connection.Query<Categorias>(sql).ToList();
        }
    }

    public static List<Dificultades> ObtenerDificultades()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            const string sql = @"SELECT IdDIficultad AS IdDificultad, Nombre FROM Dificultades ORDER BY IdDIficultad";
            return connection.Query<Dificultades>(sql).ToList();
        }
    }

    public static List<Preguntas> ObtenerPreguntas(int dificultad, int categoria)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            const string sql = @"
                SELECT IdPregunta, IdCategoria, IdDificultad, Enunciado FROM Preguntas WHERE (@dificultad = -1 OR IdDificultad = @dificultad)
                  AND (@categoria = -1 OR IdCategoria = @categoria) ORDER BY IdPregunta";
            return connection.Query<Preguntas>(sql, new { dificultad, categoria }).ToList();
        }
    }

    public static List<Respuestas> ObtenerRespuestas(int idPregunta)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            const string sql = @"SELECT IdRespuesta, IdPregunta, Opcion, Contenido, Correcta FROM Respuestas WHERE IdPregunta = @idPregunta
                                 ORDER BY Opcion";
            return connection.Query<Respuestas>(sql, new { idPregunta }).ToList();
        }
    }
}