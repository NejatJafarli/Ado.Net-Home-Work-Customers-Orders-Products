using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ado.Net_Home_Work_Customers_Orders_Products
{
    /// <summary>
    /// Interaction logic for OrderDetailWindow.xaml
    /// </summary>
    public partial class OrderDetailWindow : Window
    {
        public OrderDetailWindow()
        {
            InitializeComponent();
        }
        public OrderDetailWindow(DataTable dt)
        {
            InitializeComponent();
            
            CompTxt.Text=dt.Rows[0][0].ToString();
            ProdTxt.Text = dt.Rows[0][1].ToString();
            QuanityTxt.Text = dt.Rows[0][2].ToString();
            PriceTxt.Text = dt.Rows[0][3].ToString();
            DisTxt.Text = dt.Rows[0][4].ToString();
            TotalPriceTxt.Text = dt.Rows[0][5].ToString();

        }
    }
}
