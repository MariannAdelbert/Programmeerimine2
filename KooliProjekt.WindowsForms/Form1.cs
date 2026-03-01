using KooliProjekt.WindowsForms.Api;
using System.Net.Http.Json;

namespace KooliProjekt.WindowsForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override async void OnLoad(EventArgs e)
        {
            await LoadData();

            base.OnLoad(e);
        }

        private async Task LoadData()
        {
            var url = "http://localhost:5086/api/Projects/List";
            url += "?page=1&pageSize=10";

            using var client = new HttpClient();
            var response = await client.GetFromJsonAsync<OperationResult<PagedResult<ProjectDetailsDto>>>(url);

            dataGridView1.DataSource = response.Value.Results;
        }
    }
}
