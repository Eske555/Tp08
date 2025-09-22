using System.Collections.Generic;
using System.Linq;

public class Juego
{
    private string Username;
    private int PuntajeActual;
    private int CantidadPreguntasCorrectas;
    private int ContadorNroPreguntaActual;
    private Preguntas PreguntaActual;
    private List<Preguntas> ListaPreguntas = new List<Preguntas>();
    private List<Respuestas> ListaRespuestas = new List<Respuestas>();
    private void InicializarJuego()
    {
        Username = "";
        PuntajeActual = 0;
        CantidadPreguntasCorrectas = 0;
        ContadorNroPreguntaActual = 0;
        PreguntaActual = new Preguntas();
        ListaPreguntas = new List<Preguntas>();
        ListaRespuestas = new List<Respuestas>();
    }

    public List<Categorias> ObtenerCategorias()
    {
        return BD.ObtenerCategorias();
    }

    public List<Dificultades> ObtenerDificultades()
    {
        return BD.ObtenerDificultades();
    }

    public void CargarPartida(string username, int dificultad, int categoria)
    {
        InicializarJuego();
        Username = username;
        ListaPreguntas = BD.ObtenerPreguntas(dificultad, categoria);
        ContadorNroPreguntaActual = 0;

        if (ListaPreguntas.Count > 0)
        {
            PreguntaActual = ListaPreguntas[0];
            ListaRespuestas = BD.ObtenerRespuestas(PreguntaActual.IdPregunta);
        }
    }

    public Preguntas ObtenerProximaPregunta()
    {
        if (ListaPreguntas == null || ContadorNroPreguntaActual >= ListaPreguntas.Count)
            return null;

        PreguntaActual = ListaPreguntas[ContadorNroPreguntaActual];
        return PreguntaActual;
    }

    public List<Respuestas> ObtenerProximasRespuestas(int idPregunta)
    {
        ListaRespuestas = BD.ObtenerRespuestas(idPregunta);
        return ListaRespuestas;
    }

    public bool VerificarRespuesta(int idRespuesta)
    {
        if (ListaRespuestas == null) return false;

        var respuesta = ListaRespuestas.FirstOrDefault(r => r.IdRespuesta == idRespuesta);

        if (respuesta == null) return false;

        bool esCorrecta = respuesta.Correcta;

        if (esCorrecta)
        {
            PuntajeActual += 10;
            CantidadPreguntasCorrectas++;
        }

        ContadorNroPreguntaActual++;

        if (ListaPreguntas != null && ContadorNroPreguntaActual < ListaPreguntas.Count)
        {
            PreguntaActual = ListaPreguntas[ContadorNroPreguntaActual];
        }
        else
        {
            PreguntaActual = null;
        }

        return esCorrecta;
    }
    public string GetUsername() => Username;
    public int GetPuntaje() => PuntajeActual;
    public int GetNumeroPregunta() => ContadorNroPreguntaActual + 1;
}
