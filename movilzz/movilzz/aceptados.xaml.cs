using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;
using movilzz.modelo;

namespace movilzz
{
    public partial class aceptados : ContentPage
    {
        public int estudiante_id { get; set; }
        public List<Prestamo> prestamos { get; set; }

        public aceptados(int estudiante_id)
        {
            InitializeComponent();
            this.estudiante_id = estudiante_id;
            prestamos = new List<Prestamo>();
            LoadPrestamos();
        }

        private async void LoadPrestamos()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"http://192.168.56.1/proyectoFinal/api/prestados.php?id_estudiante={estudiante_id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var responseObj = JsonConvert.DeserializeObject<List<Prestamo>>(json);

                    if (responseObj != null)
                    {
                        prestamos = responseObj;
                        PrestamosListView.ItemsSource = prestamos;
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo obtener la lista de préstamos.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Ocurrió un error al cargar los préstamos: " + ex.Message, "OK");
            }
        }

        private void OnPrestamoSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Maneja el evento de selección del elemento aquí
            var selectedPrestamo = (Prestamo)e.SelectedItem;
            // Puedes acceder al objeto Prestamo seleccionado y realizar acciones adicionales
        }
    }
}
