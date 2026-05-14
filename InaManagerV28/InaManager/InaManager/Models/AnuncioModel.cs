using System;

namespace InaManager.Models
{
    public class AnuncioModel
    {
        // --- CAMPOS TABLA Mercado ---
        public int      Id_anuncio  { get; set; }
        public int      Id_jugador  { get; set; }
        public int      Id_equipo   { get; set; }
        public decimal  Precio      { get; set; }
        public DateTime Fecha_fin   { get; set; }
        public string   Estado      { get; set; }

        // --- DATOS DEL JOIN (Jugadores) ---
        public string   Jugador_nombre   { get; set; }
        public string   Jugador_apellido { get; set; }
        public string   Jugador_posicion { get; set; }
        public string   Jugador_afinidad { get; set; }
        public int      Jugador_nivel    { get; set; }
        public decimal  Jugador_clausula { get; set; }
        public string   Jugador_imagen   { get; set; }

        // --- DATOS DEL JOIN (Equipos) ---
        public string   Equipo_nombre    { get; set; }
        public string   Equipo_escudo    { get; set; }

        // --- CONSTRUCTORES ---
        public AnuncioModel() { }

        public AnuncioModel(
            int id_anuncio, int id_jugador, int id_equipo,
            decimal precio, DateTime fecha_fin, string estado,
            string jugador_nombre, string jugador_apellido,
            string jugador_posicion, string jugador_afinidad,
            int jugador_nivel, decimal jugador_clausula, string jugador_imagen,
            string equipo_nombre, string equipo_escudo)
        {
            Id_anuncio       = id_anuncio;
            Id_jugador       = id_jugador;
            Id_equipo        = id_equipo;
            Precio           = precio;
            Fecha_fin        = fecha_fin;
            Estado           = estado;
            Jugador_nombre   = jugador_nombre;
            Jugador_apellido = jugador_apellido;
            Jugador_posicion = jugador_posicion;
            Jugador_afinidad = jugador_afinidad;
            Jugador_nivel    = jugador_nivel;
            Jugador_clausula = jugador_clausula;
            Jugador_imagen   = jugador_imagen;
            Equipo_nombre    = equipo_nombre;
            Equipo_escudo    = equipo_escudo;
        }
    }
}
