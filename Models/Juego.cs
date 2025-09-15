using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;

public class Juego
{
    private string username;
    private int PuntajeActual;
    private int CantidadPreguntasCorrectas;
    private int ContadorNroPreguntaActual;
    private Preguntas PreguntaActual;
    private List<Preguntas> ListaPreguntas;
    private List<Respuestas> ListaRespuestas;

    public Juego()
    {
        string username;
        int PuntajeActual;
        int CantidadPreguntasCorrectas;
        int ContadorNroPreguntaActual;
        Preguntas PreguntaActual = new Preguntas();
        List<Preguntas> ListaPreguntas = new List<Preguntas>();
        List<Respuestas> ListaRespuestas = new List<Respuestas>();
    }
    private void InicializarJuego()
    {
        username = "";
        PuntajeActual = 0;
        CantidadPreguntasCorrectas = 0;
        ContadorNroPreguntaActual = 0;
        PreguntaActual = null;
        ListaPreguntas = null;
        ListaRespuestas = null;
    }

}