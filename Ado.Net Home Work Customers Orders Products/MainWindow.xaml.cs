using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ado.Net_Home_Work_Customers_Orders_Products
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string Connection { get; set; } = ConfigurationManager.ConnectionStrings["ConnectionStringToDB"].ConnectionString;
        public MainWindow()
        {
            InitializeComponent();

            using (SqlConnection Conn=new SqlConnection(Connection))
            {
                Conn.Open();


                SqlDataAdapter adapter = new SqlDataAdapter("SELECT OrderID FROM Orders",Conn);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                for (int i = 0; i < dataTable.Rows.Count; i++)
                    CBOrderId.Items.Add(dataTable.Rows[i][0]);
                CBOrderId.SelectedIndex = 0;
            }
        }

        private void CBOrderId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection Conn=new SqlConnection(Connection))
            {
                Conn.Open();

                string Query = @"
SELECT Customers.CompanyName,Products.ProductName
FROM Orders 
JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID
JOIN Products ON Products.ProductIDD = [Order Details].ProductID 
JOIN Customers ON Customers.CustomerID = Orders.CustomerID 
WHERE Orders.OrderID = @Id";

                SqlDataAdapter Adapter = new SqlDataAdapter(Query, Conn);
                Adapter.SelectCommand.Parameters.Add("@Id",SqlDbType.Int).Value=CBOrderId.SelectedItem.ToString();
             
                DataTable dataTable = new DataTable();

                Adapter.Fill(dataTable);
                DG.ItemsSource = dataTable.DefaultView;

            }
        }

        private void DG_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DG.SelectedItem == null) return;
            DataRowView Data = (DataRowView)DG.SelectedItem;

            string Query = @"
SELECT Customers.CompanyName,
Products.ProductName,[Order Details].Quantity,
[Order Details].UnitPrice, [Order Details].Discount,
(Quantity * [Order Details].UnitPrice) AS 'Total Price'
FROM Orders 
JOIN [Order Details] ON Orders.OrderID = [Order Details].OrderID
JOIN Products ON Products.ProductIDD = [Order Details].ProductID 
JOIN Customers ON Customers.CustomerID = Orders.CustomerID 
WHERE Products.ProductName = @Name";

            using (SqlConnection Conn = new SqlConnection(Connection))
            {
                Conn.Open ();

                SqlDataAdapter adapter = new SqlDataAdapter(Query,Conn);
                adapter.SelectCommand.Parameters.Add("@Name",SqlDbType.NVarChar).Value=Data.Row[1].ToString();
                DataTable DataTable = new DataTable();

                adapter.Fill (DataTable);

                OrderDetailWindow orderDetailWindow = new OrderDetailWindow(DataTable);
                orderDetailWindow.ShowDialog();
            }
        }
    }
}
