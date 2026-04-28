using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using InaManager.Models;
using InaManager.Repositories;
using Microsoft.Win32; // Necesario para OpenFileDialog
using System.IO;       // Necesario para Path y File

namespace InaManager.ViewModel
{
    public class SponsorsViewModel : ViewModelBase
    {
        private MysqlSponsorRepository _sponsorRepository;
        private ObservableCollection<SponsorModel> _sponsors;
        private SponsorModel _selectedSponsor;

        // Control de vistas
        private Visibility _isListVisible = Visibility.Visible;
        private Visibility _isDetailVisible = Visibility.Collapsed;
        private bool _isNewSponsorMode;

        public ObservableCollection<SponsorModel> Sponsors
        {
            get { return _sponsors; }
            set { _sponsors = value; OnPropertyChanged(nameof(Sponsors)); }
        }

        public SponsorModel SelectedSponsor
        {
            get { return _selectedSponsor; }
            set { _selectedSponsor = value; OnPropertyChanged(nameof(SelectedSponsor)); }
        }

        public Visibility IsListVisible
        {
            get { return _isListVisible; }
            set { _isListVisible = value; OnPropertyChanged(nameof(IsListVisible)); }
        }

        public Visibility IsDetailVisible
        {
            get { return _isDetailVisible; }
            set { _isDetailVisible = value; OnPropertyChanged(nameof(IsDetailVisible)); }
        }

        // Comandos
        public ICommand ShowDetailsCommand { get; }
        public ICommand BackToListCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand DeleteSponsorCommand { get; }
        public ICommand NewSponsorCommand { get; }
        public ICommand SelectImageCommand { get; } // <--- NUEVO COMANDO

        public SponsorsViewModel()
        {
            _sponsorRepository = new MysqlSponsorRepository();
            LoadSponsors();

            ShowDetailsCommand = new ViewModelCommand(ExecuteShowDetails);
            BackToListCommand = new ViewModelCommand(ExecuteBackToList);
            SaveChangesCommand = new ViewModelCommand(ExecuteSaveChanges);
            DeleteSponsorCommand = new ViewModelCommand(ExecuteDeleteSponsor);
            NewSponsorCommand = new ViewModelCommand(ExecuteNewSponsor);
            SelectImageCommand = new ViewModelCommand(ExecuteSelectImage); // Inicializar
        }

        private void LoadSponsors()
        {
            var list = _sponsorRepository.GetAll();
            Sponsors = new ObservableCollection<SponsorModel>(list);
        }

        private void ExecuteNewSponsor(object obj)
        {
            _isNewSponsorMode = true;
            SelectedSponsor = new SponsorModel
            {
                // CAMBIO IMPORTANTE: Quitamos la barra inicial '/' para rutas relativas de archivo
                Url_Logo = "\\Images\\Sponsors\\default.png",
                Fecha_Inicio = DateTime.Now,
                Fecha_Fin = DateTime.Now.AddYears(1)
            };

            IsListVisible = Visibility.Collapsed;
            IsDetailVisible = Visibility.Visible;
        }

        // --- LÓGICA PARA SELECCIONAR IMAGEN ---
        private void ExecuteSelectImage(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Imágenes|*.png;*.jpg;*.jpeg;*.bmp";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // 1. Obtener la ruta del archivo seleccionado
                    string sourcePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(sourcePath);

                    // 2. Definir la carpeta de destino dentro del proyecto (relativa al ejecutable)
                    string destFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images", "Sponsors");

                    // Crear carpeta si no existe
                    if (!Directory.Exists(destFolder))
                        Directory.CreateDirectory(destFolder);

                    string destPath = Path.Combine(destFolder, fileName);

                    // 3. Copiar la imagen (si no es la misma)
                    if (!string.Equals(sourcePath, destPath, StringComparison.OrdinalIgnoreCase))
                    {
                        File.Copy(sourcePath, destPath, true); // true = sobreescribir
                    }

                    // 4. Actualizar la ruta en el modelo (ruta relativa para la BD)
                    SelectedSponsor.Url_Logo = $"\\Images\\Sponsors\\{fileName}";

                    // Forzar notificación a la vista (a veces hace falta si la propiedad no es 'Full Property')
                    OnPropertyChanged(nameof(SelectedSponsor));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al copiar la imagen: " + ex.Message);
                }
            }
        }

        private void ExecuteShowDetails(object obj)
        {
            if (obj is SponsorModel sponsor)
            {
                _isNewSponsorMode = false;
                SelectedSponsor = sponsor;
                IsListVisible = Visibility.Collapsed;
                IsDetailVisible = Visibility.Visible;
            }
        }

        private void ExecuteBackToList(object obj)
        {
            LoadSponsors();
            IsListVisible = Visibility.Visible;
            IsDetailVisible = Visibility.Collapsed;
        }

        private void ExecuteSaveChanges(object obj)
        {
            try
            {
                if (SelectedSponsor == null) return;

                if (string.IsNullOrWhiteSpace(SelectedSponsor.Nombre_Empresa))
                {
                    MessageBox.Show("El nombre de la empresa es obligatorio.");
                    return;
                }

                if (_isNewSponsorMode)
                {
                    _sponsorRepository.Add(SelectedSponsor);
                    MessageBox.Show("Patrocinador añadido correctamente.");
                }
                else
                {
                    _sponsorRepository.Edit(SelectedSponsor);
                    MessageBox.Show("Patrocinador actualizado.");
                }

                ExecuteBackToList(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        private void ExecuteDeleteSponsor(object obj)
        {
            if (obj is int id)
            {
                var result = MessageBox.Show("¿Eliminar este patrocinador?", "Confirmar", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _sponsorRepository.Remove(id);
                        LoadSponsors();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al eliminar: " + ex.Message);
                    }
                }
            }
        }
    }
}