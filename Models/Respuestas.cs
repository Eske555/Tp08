public class Respuestas
{
    public int IdRespuesta { get; set; }
    public int IdPregunta { get; set; }
    public int Opcion { get; set; }
    public string Contenido { get; set; } = string.Empty;
    public bool Correcta { get; set; }
}