using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using InaManager.Models;
using InaManager.Repositories;

namespace InaManager.ViewModel
{
    // ─── Wrapper de anuncio con propiedades calculadas para la vista ─────────
    public class AnuncioItemVM : ViewModelBase
    {
        public AnuncioModel Model { get; }

        // Color de fondo según el elemento del jugador
        public SolidColorBrush ColorElemento
        {
            get
            {
                return Model.Jugador_afinidad switch
                {
                    "Aire"    => new SolidColorBrush(Color.FromRgb(0x5B, 0xB3, 0xD0)),   // azul cielo
                    "Bosque"  => new SolidColorBrush(Color.FromRgb(0x3A, 0x8F, 0x4B)),   // verde
                    "Fuego"   => new SolidColorBrush(Color.FromRgb(0xC0, 0x39, 0x1B)),   // rojo-naranja
                    "Montaña" => new SolidColorBrush(Color.FromRgb(0x78, 0x5A, 0x3A)),   // marrón-tierra
                    _         => new SolidColorBrush(Color.FromRgb(0x4A, 0x4A, 0x6A)),   // gris neutro
                };
            }
        }

        // Color de borde / acento según elemento
        public SolidColorBrush ColorBorde
        {
            get
            {
                return Model.Jugador_afinidad switch
                {
                    "Aire"    => new SolidColorBrush(Color.FromRgb(0x87, 0xCE, 0xEB)),
                    "Bosque"  => new SolidColorBrush(Color.FromRgb(0x5C, 0xC8, 0x6A)),
                    "Fuego"   => new SolidColorBrush(Color.FromRgb(0xFF, 0x6B, 0x35)),
                    "Montaña" => new SolidColorBrush(Color.FromRgb(0xA0, 0x7B, 0x55)),
                    _         => new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0xAA)),
                };
            }
        }

        public string Emoji
        {
            get
            {
                return Model.Jugador_afinidad switch
                {
                    "Aire"    => "🌪️",
                    "Bosque"  => "🌿",
                    "Fuego"   => "🔥",
                    "Montaña" => "⛰️",
                    _         => "⚽",
                };
            }
        }

        // Nombre completo del jugador
        public string NombreCompleto => $"{Model.Jugador_nombre} {Model.Jugador_apellido}";

        // Precio con formato
        public string PrecioFormato  => Model.Precio.ToString("N0") + " €";
        public string ClausulaFormato => Model.Jugador_clausula.ToString("N0") + " €";

        // Días restantes hasta el fin del anuncio
        public int DiasRestantes => Math.Max(0, (Model.Fecha_fin - DateTime.Today).Days);
        public string DiasTexto  => DiasRestantes == 1 ? "1 día" : $"{DiasRestantes} días";

        public AnuncioItemVM(AnuncioModel model) { Model = model; }
    }

    // ─── ViewModel principal del Mercado de Fichajes ─────────────────────────
    public class MercadoViewModel : ViewModelBase
    {
        private readonly IMercadoRepository _repo;

        // ── Lista maestra y paginada ──────────────────────────────────────────
        private List<AnuncioItemVM> _todosLosAnuncios = new List<AnuncioItemVM>();

        private ObservableCollection<AnuncioItemVM> _anunciosPagina;
        public ObservableCollection<AnuncioItemVM> AnunciosPagina
        {
            get => _anunciosPagina;
            set { _anunciosPagina = value; OnPropertyChanged(nameof(AnunciosPagina)); }
        }

        // ── Paginación ────────────────────────────────────────────────────────
        private const int ItemsPorPagina = 20; // 5 columnas × 4 filas
        private int _paginaActual = 1;
        public int PaginaActual
        {
            get => _paginaActual;
            set { _paginaActual = value; OnPropertyChanged(nameof(PaginaActual)); OnPropertyChanged(nameof(InfoPagina)); }
        }

        private int _totalPaginas = 1;
        public int TotalPaginas
        {
            get => _totalPaginas;
            set { _totalPaginas = value; OnPropertyChanged(nameof(TotalPaginas)); OnPropertyChanged(nameof(InfoPagina)); }
        }

        public string InfoPagina => $"Página {PaginaActual} de {TotalPaginas}";
        public bool PuedePaginaAnterior => PaginaActual > 1;
        public bool PuedePaginaSiguiente => PaginaActual < TotalPaginas;

        // ── Filtros ───────────────────────────────────────────────────────────
        private string _textoBusqueda = string.Empty;
        public string TextoBusqueda
        {
            get => _textoBusqueda;
            set { _textoBusqueda = value; OnPropertyChanged(nameof(TextoBusqueda)); AplicarFiltros(); }
        }

        // Grupos de posición para el ComboBox
        public List<string> GruposPosicion { get; } = new List<string>
        {
            "Todas", "Portero", "Defensas", "Pivotes/Carrileros", "Centrocampistas", "Mediocentros ofensivos", "Delanteros"
        };

        private string _grupoPosicionSeleccionado = "Todas";
        public string GrupoPosicionSeleccionado
        {
            get => _grupoPosicionSeleccionado;
            set { _grupoPosicionSeleccionado = value; OnPropertyChanged(nameof(GrupoPosicionSeleccionado)); AplicarFiltros(); }
        }

        // Elementos (afinidades)
        public List<string> Elementos { get; } = new List<string>
        {
            "Todos", "Aire", "Bosque", "Fuego", "Montaña", "Neutro"
        };

        private string _elementoSeleccionado = "Todos";
        public string ElementoSeleccionado
        {
            get => _elementoSeleccionado;
            set { _elementoSeleccionado = value; OnPropertyChanged(nameof(ElementoSeleccionado)); AplicarFiltros(); }
        }

        // Precio máximo
        private string _precioMaxTexto = string.Empty;
        public string PrecioMaxTexto
        {
            get => _precioMaxTexto;
            set { _precioMaxTexto = value; OnPropertyChanged(nameof(PrecioMaxTexto)); AplicarFiltros(); }
        }

        // Ordenación por fecha (más reciente / más lejana)
        public List<string> OpcionesOrden { get; } = new List<string>
        {
            "Fecha límite (pronto)", "Fecha límite (lejana)", "Precio (menor)", "Precio (mayor)"
        };

        private string _ordenSeleccionado = "Fecha límite (pronto)";
        public string OrdenSeleccionado
        {
            get => _ordenSeleccionado;
            set { _ordenSeleccionado = value; OnPropertyChanged(nameof(OrdenSeleccionado)); AplicarFiltros(); }
        }

        // Contador de resultados
        private int _totalResultados;
        public int TotalResultados
        {
            get => _totalResultados;
            set { _totalResultados = value; OnPropertyChanged(nameof(TotalResultados)); OnPropertyChanged(nameof(TextoResultados)); }
        }
        public string TextoResultados => $"{TotalResultados} jugador{(TotalResultados != 1 ? "es" : "")} en mercado";

        private bool _cargando;
        public bool Cargando
        {
            get => _cargando;
            set { _cargando = value; OnPropertyChanged(nameof(Cargando)); OnPropertyChanged(nameof(NoCargando)); }
        }
        public bool NoCargando => !Cargando;

        private bool _sinResultados;
        public bool SinResultados
        {
            get => _sinResultados;
            set { _sinResultados = value; OnPropertyChanged(nameof(SinResultados)); }
        }

        // ── Comandos ──────────────────────────────────────────────────────────
        public ICommand PaginaAnteriorCommand   { get; }
        public ICommand PaginaSiguienteCommand  { get; }
        public ICommand LimpiarFiltrosCommand   { get; }

        // ── Constructor ───────────────────────────────────────────────────────
        public MercadoViewModel()
        {
            _repo = new MysqlMercadoRepository();

            PaginaAnteriorCommand  = new ViewModelCommand(_ => CambiarPagina(-1), _ => PuedePaginaAnterior);
            PaginaSiguienteCommand = new ViewModelCommand(_ => CambiarPagina(+1), _ => PuedePaginaSiguiente);
            LimpiarFiltrosCommand  = new ViewModelCommand(_ => LimpiarFiltros());

            CargarDatos();
        }

        // ── Carga ─────────────────────────────────────────────────────────────
        public void CargarDatos()
        {
            Cargando = true;
            try
            {
                var lista = _repo.ObtenerAnunciosDisponibles();
                _todosLosAnuncios = lista.Select(a => new AnuncioItemVM(a)).ToList();
                AplicarFiltros();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MercadoViewModel.CargarDatos: {ex.Message}");
            }
            finally
            {
                Cargando = false;
            }
        }

        // ── Filtros y ordenación ──────────────────────────────────────────────
        private void AplicarFiltros()
        {
            var filtrados = _todosLosAnuncios.AsEnumerable();

            // Filtro por nombre
            if (!string.IsNullOrWhiteSpace(TextoBusqueda))
            {
                var q = TextoBusqueda.Trim().ToLower();
                filtrados = filtrados.Where(a =>
                    a.Model.Jugador_nombre.ToLower().Contains(q) ||
                    a.Model.Jugador_apellido.ToLower().Contains(q) ||
                    a.NombreCompleto.ToLower().Contains(q));
            }

            // Filtro por grupo de posición
            if (GrupoPosicionSeleccionado != "Todas")
            {
                var posicionesGrupo = ObtenerPosicionesGrupo(GrupoPosicionSeleccionado);
                filtrados = filtrados.Where(a => posicionesGrupo.Contains(a.Model.Jugador_posicion));
            }

            // Filtro por elemento
            if (ElementoSeleccionado != "Todos")
                filtrados = filtrados.Where(a => a.Model.Jugador_afinidad == ElementoSeleccionado);

            // Filtro por precio máximo
            if (decimal.TryParse(PrecioMaxTexto, out decimal precioMax) && precioMax > 0)
                filtrados = filtrados.Where(a => a.Model.Precio <= precioMax);

            // Ordenación
            filtrados = OrdenSeleccionado switch
            {
                "Fecha límite (lejana)" => filtrados.OrderByDescending(a => a.Model.Fecha_fin),
                "Precio (menor)"        => filtrados.OrderBy(a => a.Model.Precio),
                "Precio (mayor)"        => filtrados.OrderByDescending(a => a.Model.Precio),
                _                       => filtrados.OrderBy(a => a.Model.Fecha_fin)
            };

            var lista = filtrados.ToList();
            TotalResultados = lista.Count;
            SinResultados   = TotalResultados == 0;

            TotalPaginas  = Math.Max(1, (int)Math.Ceiling((double)TotalResultados / ItemsPorPagina));
            PaginaActual  = Math.Min(PaginaActual, TotalPaginas);
            if (PaginaActual < 1) PaginaActual = 1;

            var pagina = lista.Skip((PaginaActual - 1) * ItemsPorPagina).Take(ItemsPorPagina);
            AnunciosPagina = new ObservableCollection<AnuncioItemVM>(pagina);

            ActualizarComandosPaginacion();
        }

        private void CambiarPagina(int delta)
        {
            PaginaActual += delta;
            AplicarFiltros();
        }

        private void LimpiarFiltros()
        {
            _textoBusqueda             = string.Empty;
            _grupoPosicionSeleccionado = "Todas";
            _elementoSeleccionado      = "Todos";
            _precioMaxTexto            = string.Empty;
            _ordenSeleccionado         = "Fecha límite (pronto)";
            _paginaActual              = 1;

            OnPropertyChanged(nameof(TextoBusqueda));
            OnPropertyChanged(nameof(GrupoPosicionSeleccionado));
            OnPropertyChanged(nameof(ElementoSeleccionado));
            OnPropertyChanged(nameof(PrecioMaxTexto));
            OnPropertyChanged(nameof(OrdenSeleccionado));

            AplicarFiltros();
        }

        private void ActualizarComandosPaginacion()
        {
            OnPropertyChanged(nameof(PuedePaginaAnterior));
            OnPropertyChanged(nameof(PuedePaginaSiguiente));
            OnPropertyChanged(nameof(InfoPagina));
        }

        // ── Mapeo de grupos de posición ───────────────────────────────────────
        private static HashSet<string> ObtenerPosicionesGrupo(string grupo) => grupo switch
        {
            "Portero"                  => new HashSet<string> { "PR" },
            "Defensas"                 => new HashSet<string> { "LI", "DFCI", "DFC", "DFCD", "LD" },
            "Pivotes/Carrileros"       => new HashSet<string> { "CI", "MCDI", "MCDC", "MCDD", "CD" },
            "Centrocampistas"          => new HashSet<string> { "MI", "MCI", "MCC", "MCD", "MD" },
            "Mediocentros ofensivos"   => new HashSet<string> { "II", "MCO", "ID" },
            "Delanteros"               => new HashSet<string> { "EI", "DCI", "DC", "DCD", "ED" },
            _                          => new HashSet<string>()
        };
    }
}
