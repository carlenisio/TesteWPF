using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TesteWPF
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();
        public MainWindow()
        {
            InitializeComponent();
            client.BaseAddress = new Uri("http://localhost:56141");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            this.Loaded += MainWindow_Loaded;
        }
        async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("/api/analistas");
                response.EnsureSuccessStatusCode(); // Lança um código de erro
                var analistas = await response.Content.ReadAsAsync<IEnumerable<Analista>>();
                analistasListView.ItemsSource = analistas;
            }
            catch (Exception)
            {
                //MessageBox.Show("Erro : " + ex.Message);
            }
        }

        private async void btnNovo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var analista = new Analista()
                {
                    nome = txtNome.Text,
                    sobrenome = txtSobreNome.Text,
                    telefone = int.Parse(txtTelefone.Text)
                };
                var response = await client.PostAsJsonAsync("/api/analistas/", analista);
                response.EnsureSuccessStatusCode(); //lança um código de erro
                MessageBox.Show("Analista incluído com sucesso", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                analistasListView.ItemsSource = await GetAllAnalistas();
                analistasListView.ScrollIntoView(analistasListView.ItemContainerGenerator.Items[analistasListView.Items.Count - 1]);
            }
            catch (Exception)
            {
                MessageBox.Show("O Analista não foi incluído. (Verifique se o ID não esta duplicado)");
            }
        }

        private async void btnAtualiza_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var analista = new Analista()
                {
                    nome = txtNome.Text,
                    sobrenome = txtSobreNome.Text,
                    telefone = int.Parse(txtTelefone.Text)
                };
                var response = await client.PutAsJsonAsync("/api/analistas/", analista);
                response.EnsureSuccessStatusCode(); //lança um código de erro
                MessageBox.Show("Analista atualizado com sucesso", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
                analistasListView.ItemsSource = await GetAllAnalistas();
                analistasListView.ScrollIntoView(analistasListView.ItemContainerGenerator.Items[analistasListView.Items.Count - 1]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void btnDelet_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync("/api/analistas/" + txtID.Text);
                response.EnsureSuccessStatusCode(); //lança um código de erro
                MessageBox.Show("Analista deletado com sucesso");
                analistasListView.ItemsSource = await GetAllAnalistas();
                analistasListView.ScrollIntoView(analistasListView.ItemContainerGenerator.Items[analistasListView.Items.Count - 1]);
            }
            catch (Exception)
            {
                MessageBox.Show("Analista deletado com sucesso");
            }
        }
        private async void btnGetAnalista_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("/api/analistas/" + txtID.Text);
                response.EnsureSuccessStatusCode(); //lança um código de erro
                var analistas = await response.Content.ReadAsAsync<Analista>();
                analistaDetalhesPanel.Visibility = Visibility.Visible;
                analistaDetalhesPanel.DataContext = analistas;
            }
            catch (Exception)
            {
                MessageBox.Show("Analista não localizado");
            }
        }
        public async Task<IEnumerable<Analista>> GetAllAnalistas()
        {
            HttpResponseMessage response = await client.GetAsync("/api/analistas");
            response.EnsureSuccessStatusCode(); //lança um código de erro
            var analistas = await response.Content.ReadAsAsync<IEnumerable<Analista>>();
            return analistas;
        }

       
    }
}
