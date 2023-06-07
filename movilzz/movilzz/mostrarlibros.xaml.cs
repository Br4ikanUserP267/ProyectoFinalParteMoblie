using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace movilzz
{
    public partial class mostrarlibros : ContentPage
    {
        private ObservableCollection<Book> books;
        private ObservableCollection<Book> filteredBooks;
        private List<Book> selectedBooks;

        public int estudiante_id { get; set; }

        public mostrarlibros(int estudiante_id)
        {
            InitializeComponent();

            this.estudiante_id = estudiante_id;

            books = new ObservableCollection<Book>();
            filteredBooks = new ObservableCollection<Book>();
            selectedBooks = new List<Book>();

            BooksListView.ItemsSource = filteredBooks;

            LoadBooks();
            BindingContext = this;
        }

        private async void LoadBooks()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync("http://192.168.56.1/proyectoFinal/api/consultarautor_libro.php");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var bookList = JsonConvert.DeserializeObject<List<Book>>(json);
                    foreach (var book in bookList)
                    {
                        if (book.Disponibilidad)
                        {
                            books.Add(book);
                            filteredBooks.Add(book);
                            CargarImagen(book);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private async void CargarImagen(Book book)
        {
            try
            {
                HttpClient client = new HttpClient();
                var imageUrl = "http://192.168.56.1/proyectoFinal/api/" + book.Imagen;
                var response = await client.GetAsync(imageUrl);
                if (response.IsSuccessStatusCode)
                {
                    var imageData = await response.Content.ReadAsByteArrayAsync();
                    ImageSource imagenSource = ImageSource.FromStream(() => new MemoryStream(imageData));
                    book.ImagenCargada = imagenSource;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al cargar la imagen: " + ex.Message);
            }
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            string searchTerm = SearchBar.Text;

            // Filtrar los libros según el término de búsqueda
            filteredBooks.Clear();
            foreach (var book in books)
            {
                if (book.Nombre.Contains(searchTerm))
                {
                    filteredBooks.Add(book);
                }
            }
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var selectedBook = (Book)button.BindingContext;

            selectedBooks.Add(selectedBook);

            // Guardar los libros seleccionados en las preferencias compartidas
            await SaveSelectedBooks(selectedBooks);

            await DisplayAlert("Éxito", "El libro ha sido añadido al carrito.", "OK");
        }

        private async Task SaveSelectedBooks(List<Book> books)
        {
            var json = JsonConvert.SerializeObject(books);

            await SecureStorage.SetAsync("SelectedBooks", json);
        }

        private async Task<List<Book>> LoadSelectedBooks()
        {
            var json = await SecureStorage.GetAsync("SelectedBooks");

            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject<List<Book>>(json);
            }

            return new List<Book>();
        }

        private async void irAlcarro(object sender, EventArgs e)
        {
            await SaveSelectedBooks(selectedBooks);

            // Navegar a la página del carroCompra y pasar la lista de libros seleccionados como parámetro
            await Navigation.PushAsync(new carroCompra(selectedBooks, estudiante_id));
        }

        private async void irAPendientes(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new pendientes(estudiante_id));
        }
        private async void irAceptados(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new aceptados(estudiante_id));
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int EditorialesId { get; set; }
        public string Imagen { get; set; }
        public int TemasId { get; set; }
        public float Valor { get; set; }
        public bool Disponibilidad { get; set; }
        public int NumeroUnidades { get; set; }
        public string Resumen { get; set; }
        public string Autores { get; set; }
        public string NombreEditorial { get; set; }
        public ImageSource ImagenCargada { get; set; }
    }
}
