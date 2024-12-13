using FastMember;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Models;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnID.Text.Trim() == "")
            {
                IEnumerable<Products> products = await GetList();
                DataTable dt = new DataTable();
                if (products != null)
                {
                    using (var Reader = ObjectReader.Create(products))
                    {
                        dt.Load(Reader);
                    }
                    dgv_Product.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("محصولی وجود ندارد");
                } 
            }
            else
            {
                Products products = await GetListByID(Convert.ToInt32(btnID.Text.Trim()));
                DataTable dt = new DataTable();
                if (products != null)
                {
                    var productsList = new List<Products> { products };
                    using (var Reader = ObjectReader.Create(productsList))
                    {
                        dt.Load(Reader);
                    }
                    dgv_Product.DataSource = dt;
                }
                else
                {
                    MessageBox.Show("محصولی وجود ندارد");
                }
            }
        }

        private async Task<IEnumerable<Products>> GetList()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("Http://127.0.0.1:5000/");
                    string Url = "Products/GetProducts";
                    var Response = client.GetAsync(Url);
                    var Content = await Response.Result.Content.ReadAsStringAsync();
                    var Products = JsonConvert.DeserializeObject<IEnumerable<Products>>(Content);
                    return Products;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private async Task<Products> GetListByID(int ID)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("Http://127.0.0.1:5000/");
                    string Url = $"Products/GetProductsByID?id={ID}";
                    var Response = client.GetAsync(Url);
                    var Content = await Response.Result.Content.ReadAsStringAsync();
                    var Products = JsonConvert.DeserializeObject<Products>(Content);
                    return Products;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
