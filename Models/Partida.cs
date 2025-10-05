public class Partida
{
    public int categoria { get; set; }
    public int dificultad { get; set; }
    public string username { get; set; }

    public Partida(int categoria, int dificultad, string username)
    {
        this.categoria = categoria;
        this.dificultad = dificultad;
        this.username = username;
    }
}