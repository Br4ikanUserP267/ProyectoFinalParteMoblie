using movilzz.modelo;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Common;
using ZXing.Net.Mobile.Forms;

namespace movilzz
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class izquierda : ContentPage
    {
        Estudiante estudiante { get; set; }
        Carrera carrera { get; set; }
        public int numeroidentificacion { get; set; }
        public string BarcodeValue { get; set; }


        public izquierda(int numeroidentificacion)
        {
            InitializeComponent();

            this.numeroidentificacion = numeroidentificacion;
            estudiante = new Estudiante();
            carrera = new Carrera();

            CargarDatos();
            CargarDatosCarrera();


            BarcodeValue = numeroidentificacion.ToString(); 
            BindingContext = this;








        }


        private async void CargarDatos()
        {



            Uri requestUri = new Uri($"http://192.168.56.1/ProyectoFinal/api/mostrardatoscarnet.php?numeroIdentificacion={numeroidentificacion}");
            var client = new HttpClient();
            var response = await client.GetAsync(requestUri);
            HttpContent content = response.Content;

            if (response.IsSuccessStatusCode)
            {

                string json = await content.ReadAsStringAsync();
                // Si los datos son un array JSON, puedes deserializarlos en una lista de objetos
                Estudiante datos = JsonConvert.DeserializeObject<Estudiante>(json);

                estudiante = new Estudiante
                {
                    nombres = datos.nombres,
                    apellidos = datos.apellidos,
                    celular = datos.celular,
                    ciudadnacimiento = datos.ciudadnacimiento,
                    correoelectronico = datos.correoelectronico,
                    departamentonacimiento = datos.departamentonacimiento,
                    fechanacimiento = datos.fechanacimiento,
                    foto = datos.foto, // Convertir la foto de base64 a byte[]
                    numeroIdentificacion = datos.numeroIdentificacion,
                    tipoIdentificacion = datos.tipoIdentificacion,
                    id = datos.id,
                    paisnacimiento = datos.paisnacimiento,
                    tiposagre = datos.tiposagre,
                    barrio = datos.barrio,
                    calle = datos.calle,
                    estudiantes_id = datos.estudiantes_id,
                    ciudad = datos.ciudad,
                    departamento = datos.departamento,
                    numero = datos.numero,
                    pais = datos.pais
                };

                SpnNombre.Text = estudiante.nombres;
                SpnApellido.Text = estudiante.apellidos;
                SpnTipoIdentificacion.Text = estudiante.tipoIdentificacion.ToUpper();
                SpnNumeroIdentificacion.Text = estudiante.numeroIdentificacion.ToString();
                SpnTipoSangre.Text = estudiante.tiposagre;
                SpnDireccion.Text = $"{estudiante.calle} {estudiante.ciudad} {estudiante.barrio}";
                SpnCorreo.Text = estudiante.correoelectronico;
                SpnTelefono.Text = estudiante.celular;
                CargarImagen(estudiante.foto); // Llamada a CargarImagen después de obtener el valor de estudiante.foto


            }
        }

        private void CargarImagen(string foto)
        {
            try
            {
                // Construir la URL de la imagen
                string url = $"http://192.168.56.1/proyectoFinal/api/{foto}";

                // Crear un objeto UriImageSource con la URL de la imagen
                FotoMateo.Source = new UriImageSource
                {
                    Uri = new Uri(url),
                    CachingEnabled = false // Esto evita que se guarde en caché la imagen para evitar problemas de actualización
                };
            }
            catch (Exception ex)
            {
                // Manejar cualquier error que ocurra durante la carga de la imagen
                Console.WriteLine($"Error al cargar la imagen: {ex.Message}");
            }
        }


        private async void CargarDatosCarrera()
        {
            Uri requestUri = new Uri($"http://192.168.56.1/ProyectoFinal/api/mostrardatoscarnetparteinscripciones.php?numeroIdentificacion={numeroidentificacion}");
            var client = new HttpClient();
            var response = await client.GetAsync(requestUri);
            HttpContent content = response.Content;

            if (response.IsSuccessStatusCode)
            {

                string json = await content.ReadAsStringAsync();
                // Si los datos son un array JSON, puedes deserializarlos en una lista de objetos
                List<Carrera> datos = JsonConvert.DeserializeObject<List<Carrera>>(json);

                carrera = new Carrera
                {
                    Carrera_id = datos[0].Carrera_id,
                    descripcion = datos[0].descripcion,
                    estudiantes_id = datos[0].estudiantes_id,
                    fecha = datos[0].fecha,
                    id = datos[0].id,
                    nombreCarrera = datos[0].nombreCarrera,
                    Semestre_numero = datos[0].Semestre_numero
                };

                LblCarrera.Text = carrera.nombreCarrera;

            }

        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            //Enviar el id por el contructor

            int estudiante_id = Convert.ToInt32(estudiante.estudiantes_id);

            await Navigation.PushAsync(new mostrarlibros(estudiante_id));
        }


    }
}