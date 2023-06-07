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

namespace movilzz
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
		public Login ()
		{
			InitializeComponent ();
		}

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(usuario.Text) || string.IsNullOrWhiteSpace(contraseña.Text))
            {
                await DisplayAlert("Invalido", "Rellena todos los campos.", "Ok");
            }
            else
            {
                // Establece la URL de la API REST para el inicio de sesión
                Uri requestUri = new Uri("http://192.168.56.1/ProyectoFinal/api/usuarios.php");
                var client = new HttpClient();
                var response = await client.GetAsync(requestUri);
                HttpContent content = response.Content;
                if (response.IsSuccessStatusCode)
                {
                    // Lee el contenido como una cadena JSON
                    string json = await content.ReadAsStringAsync();

                    // Si los datos son un array JSON, puedes deserializarlos en una lista de objetos
                    List<Log> datos = JsonConvert.DeserializeObject<List<Log>>(json);

                    var filtroUsu = datos.FirstOrDefault(elemento => elemento.numeroIdentificacion == usuario.Text
                                                           && elemento.contrasena == contraseña.Text);

                    if (filtroUsu != null)
                    {
                        var filtroRol = datos.FirstOrDefault(elemento => elemento.numeroIdentificacion == usuario.Text &&
                                                            elemento.contrasena == contraseña.Text && elemento.tipousuario == "e");
                        if (filtroRol != null)
                        {
                            var numeroidentificacion = Convert.ToInt32(usuario.Text);

                            await Navigation.PushAsync(new izquierda(numeroidentificacion));
                        }
                        else
                        {
                            await DisplayAlert("Lo siento", "No puede ingresar con este usuario", "aceptar");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "Ingrese bien su usuario y contraseña", "aceptar");
                    }
                }
            }
        }

    }
}