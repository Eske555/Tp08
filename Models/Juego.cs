using Preguntados.Models;
#nullable enable

public class Juego
{
    public string? username { get; set; }
    public int puntuajeActual { get; set; }
    public int cantidadPreguntasCorrectas { get; set; }
    public int contadorNroPreguntaActual { get; set; }
    public Preguntas? PreguntaActual { get; set; }
    public List<Preguntas> ListaPreguntas { get; set; }
    public List<Respuestas> ListaRespuestas { get; set; }

    public Juego()
    {
        username = "";
        puntuajeActual = 0;
        cantidadPreguntasCorrectas = 0;
        contadorNroPreguntaActual = 0;
        PreguntaActual = null;
        ListaPreguntas = new List<Preguntas>();
        ListaRespuestas = new List<Respuestas>();
    }

    private void InicializarJuego()
    {
        puntuajeActual = 0;
        cantidadPreguntasCorrectas = 0;
        contadorNroPreguntaActual = 0;
        PreguntaActual = null;
        ListaPreguntas = new List<Preguntas>();
        ListaRespuestas = new List<Respuestas>();
    }

    public void CargarPartida(string username, int Dificultad, int Categoria)
    {
        this.username = username;
        InicializarJuego();
        this.username = username;
        ListaPreguntas = BD.ObtenerPreguntas(Dificultad, Categoria);
        ListaRespuestas = new List<Respuestas>(); 
        System.Diagnostics.Debug.WriteLine($"Preguntas cargadas: {ListaPreguntas?.Count ?? 0}");
    }

    public List<Categorias> ObtenerCategorias()
    {
        return BD.ObtenerCategorias();
    }

    public List<Dificultades> ObtenerDificultades()
    {
        return BD.ObtenerDificultades();
    }

    public List<Respuestas> ObtenerProximasRespuestas(int idPregunta)
    {
        ListaRespuestas = BD.ObtenerRespuestas(idPregunta);
        return ListaRespuestas ?? new List<Respuestas>();
    }

    public bool VerificarRespuesta(int IDrespuesta)
    {
        bool correcta = false;
        
        if (PreguntaActual != null)
        {
            List<Respuestas> respuestasPreguntaActual = BD.ObtenerRespuestas(PreguntaActual.IdPregunta);
            
            foreach (Respuestas respuesta in respuestasPreguntaActual)
            {
                if (respuesta.IdRespuesta == IDrespuesta)
                {
                    correcta = respuesta.Correcta;
                    break;
                }
            }
        }

        if (correcta)
        {
            puntuajeActual = puntuajeActual + 60;
            cantidadPreguntasCorrectas++;
        }
        
        contadorNroPreguntaActual++;
        return correcta;
    }

    public Preguntas? ObtenerProximaPregunta()
    {
        System.Diagnostics.Debug.WriteLine($"ListaPreguntas es null: {ListaPreguntas == null}");
        System.Diagnostics.Debug.WriteLine($"Contador actual: {contadorNroPreguntaActual}");
        System.Diagnostics.Debug.WriteLine($"Total preguntas: {ListaPreguntas?.Count ?? 0}");
        
        if (ListaPreguntas != null && contadorNroPreguntaActual < ListaPreguntas.Count)
        {
            PreguntaActual = ListaPreguntas[contadorNroPreguntaActual];
            return PreguntaActual;
        }
        return null;
    }

    public string? Username => username;
    public int PuntuajeActual => puntuajeActual;
    public int CantidadPreguntasCorrectas => cantidadPreguntasCorrectas;
    public int ContadorNroPreguntaActual => contadorNroPreguntaActual;
}