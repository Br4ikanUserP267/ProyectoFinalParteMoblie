using movilzz.modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static movilzz.mostrarlibros;

namespace movilzz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class carroCompra : ContentPage
    {
        public List<Book> SelectedBooks { get; set; }

        public int estudiante_id { get; set; }

        public carroCompra(List<Book> selectedBooks, int estudiante_id)
        {
            InitializeComponent();

            SelectedBooks = selectedBooks;
            this.estudiante_id = estudiante_id;
            BindingContext = this;
        }

        private async void OnEnviarClicked(object sender, EventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();
                DateTime fechaInicio = DateTime.Now;
                DateTime fechaFinal = fechaInicio.AddDays(7);

                int estudianteId = ObtenerEstudianteId();

                List<int> libroIds = SelectedBooks.Select(book => book.Id).ToList();

                var postData = new
                {
                    fechaInicio = fechaInicio.ToString("yyyy-MM-dd"),
                    fechaFinal = fechaFinal.ToString("yyyy-MM-dd"),
                    prestado = true,
                    Estudiantes_id = estudianteId,
                    libros = libroIds
                };

                string jsonData = JsonConvert.SerializeObject(postData);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("http://192.168.56.1/proyectoFinal/api/prestamos.php", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);
                    string message = "Status: " + responseObject["status"] + "\n" +
                                     "Message: " + responseObject["message"] + "\n" +
                                     "Data: " + JsonConvert.SerializeObject(responseObject["data"]);

                    await DisplayAlert("Success", message, "OK");

                    SelectedBooks.Clear();
                    // Refresh the ListView or any other UI component displaying the selected books

                    await Navigation.PushAsync(new mostrarlibros(estudiante_id));
                }
                else
                {
                    await DisplayAlert("Error", "Failed to send the loan request. Please try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private int ObtenerEstudianteId()
        {
            return this.estudiante_id;
        }

        private string ObtenerRegistrosJson()
        {
            // Implementar la lógica para obtener los registros en formato JSON
            // Puedes utilizar la lista de libros seleccionados o cualquier otra fuente de datos
            string json = JsonConvert.SerializeObject(SelectedBooks);
            return json;
        }
    }
}
