using System.Collections.Generic;
using System.Linq;

namespace InaManager.Helpers
{
    public static class MapeoPosiciones
    {
        // DICCIONARIO MAESTRO: Clave (BD) -> Valor (Fila, Columna)
        public static readonly Dictionary<string, (int Fila, int Columna)> Coordenadas = new Dictionary<string, (int, int)>()
        {
            // --- FILA 0: DELANTERA (Arriba) ---
            { "EI",   (0, 0) }, // Extremo Izquierdo
            { "DCI",  (0, 1) }, // Delantero Centro Izquierdo
            { "DC",   (0, 2) }, // Delantero Centro
            { "DCD",  (0, 3) }, // Delantero Centro Derecho
            { "ED",   (0, 4) }, // Extremo Derecho

            // --- FILA 1: MEDIAPUNTAS / INTERIORES OFENSIVOS ---
            { "II",   (1, 1) }, // Interior Izquierdo
            { "MCO",  (1, 2) }, // Mediapunta Centro
            { "ID",   (1, 3) }, // Interior Derecho

            // --- FILA 2: MEDIOCENTRO (Línea media) ---
            { "MI",   (2, 0) }, // Medio Izquierdo
            { "MCI",  (2, 1) }, // Medio Centro Izquierdo
            { "MCC",  (2, 2) }, // Medio Centro Centro
            { "MCD",  (2, 3) }, // Medio Centro Derecho
            { "MD",   (2, 4) }, // Medio Derecho

            // --- FILA 3: PIVOTES / CARRILEROS ---
            { "CI",   (3, 0) }, // Carrilero Izquierdo
            { "MCDI", (3, 1) }, // Pivote Izquierdo
            { "MCDC", (3, 2) }, // Pivote Central
            { "MCDD", (3, 3) }, // Pivote Derecho
            { "CD",   (3, 4) }, // Carrilero Derecho

            // --- FILA 4: DEFENSA ---
            { "LI",   (4, 0) }, // Lateral Izquierdo
            { "DFCI", (4, 1) }, // Defensa Central Izquierdo
            { "DFC",  (4, 2) }, // Defensa Central
            { "DFCD", (4, 3) }, // Defensa Central Derecho
            { "LD",   (4, 4) }, // Lateral Derecho

            // --- FILA 5: PORTERÍA (Abajo) ---
            { "PR",   (5, 2) }  // Portero
        };

        // MÉTODO INVERSO: Para saber qué rol asignar cuando haces click en una celda
        public static string ObtenerPosicionDesdeCoordenada(int fila, int columna)
        {
            var match = Coordenadas.FirstOrDefault(x => x.Value.Fila == fila && x.Value.Columna == columna);
            // Si no encuentra nada, devuelve null (Key es null por defecto en struct KeyValuePair si no existe)
            return match.Key;
        }
    }
}