using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.IO;

namespace task2__wpf
{
 
    public partial class MainWindow : Window
    {
        string connectionstring;
        SqlDataAdapter adapter;
        DataTable Auto_Table;
        public string marka, body;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                SqlConnectionStringBuilder connect = new SqlConnectionStringBuilder();
                connectionstring = ConfigurationManager.ConnectionStrings["AutoConnectionString"].ConnectionString;

            } 
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("select * from AutoTable");
           
        }

        private void GetDataFromDB(string sql)
        {
           
            Auto_Table = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionstring);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                connection.Open();
                adapter.Fill(Auto_Table);
                this.AutoGrid.ItemsSource = Auto_Table.DefaultView;
                MessageBox.Show("Данные загружены");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("select * from AutoTable");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string model = model_txb.Text;
                int engine = Convert.ToInt32(engine_txb.Text);
                int power = Convert.ToInt32(power_txb.Text);
                int maxspeed = Convert.ToInt32(speed_txb.Text);
                int coast = Convert.ToInt32(cost_txb.Text);
                Insert(model,engine,power,maxspeed,coast,marka,body);
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Insert( string model, int engine, int power, int maxspeed, int cost, string marka, string kuzov)
        {
            SqlConnection connect = null;
            connect = new SqlConnection(connectionstring);
            connect.Open();
            try
            {
                string sql = string.Format("Insert Into AutoTable" +
                      " Values(@model, @marka, @kuzov, @engine,@power,@maxspeed, @cost)");

                using (SqlCommand cmd = new SqlCommand(sql, connect))
                {
                    cmd.Parameters.AddWithValue("@marka", marka);
                    cmd.Parameters.AddWithValue("@model", model);
                    cmd.Parameters.AddWithValue("@engine", engine);
                    cmd.Parameters.AddWithValue("@power", power);
                    cmd.Parameters.AddWithValue("@maxspeed", maxspeed);
                    cmd.Parameters.AddWithValue("@cost", cost);
                    cmd.Parameters.AddWithValue("@kuzov", kuzov);
                    cmd.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            GetDataFromDB("select * from AutoTable");
           
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            SqlConnection connect = null;
            connect = new SqlConnection(connectionstring);
            connect.Open();
            string sql = string.Format("Delete from AutoTable");
            using (SqlCommand cmd = new SqlCommand(sql, connect))
            {
                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            GetDataFromDB("select * from AutoTable");

        }

        private void About_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("База данных Авто. Версия 1.1");
        }

        private void Marki_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from car_brand");
        }

        private void Concern_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from car_manufacturer");
        }

        private void auto_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from autotable");
        }

        private void sitroen_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from autotable where Марка='Ситроен'");

        }

        private void peugeot_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from autotable where Марка='Пежо'");

        }

        private void bentli_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from autotable where Марка='Бентли'");

        }

        private void Audi_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from autotable where Марка='Ауди'");

        }

        private void Buik_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from autotable where Марка='Бьюик'");

        }

        private void Kadila_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from autotable where Марка='Кадилак'");

        }

        private void Shevrole_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from autotable where Марка='Шевроле'");

        }

        private void Bugatti_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from autotable where Марка='Бугатти'");

        }

        private void type_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            body = selectedItem.Content.ToString();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            model_txb.Text = "";
            speed_txb.Text = "";
            engine_txb.Text = "";
            power_txb.Text = "";
            cost_txb.Text = "";

                            
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AutoGrid.SelectedItems.Count == 1)
                {
                    int selectedIndex = AutoGrid.SelectedIndex;
                    var row = Auto_Table.Rows[selectedIndex];
                    row.Delete();

                    adapter.Update(Auto_Table);
                }
                else if (AutoGrid.SelectedItems.Count > 1)
                {
                    while (AutoGrid.SelectedItems.Count > 0)
                    {
                        int selectedIndex = AutoGrid.SelectedIndex;
                        var row = Auto_Table.Rows[selectedIndex];
                        row.Delete();

                        adapter.Update(Auto_Table);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                adapter.UpdateCommand = builder.GetUpdateCommand();
                adapter.Update(Auto_Table);
                GetDataFromDB("select * from AutoTable");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void PSA_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from AutoTable inner join car_brand on AutoTAble.Марка = car_brand.Марка where car_brand.Производитель = 'PSA'");
        }

        private void GM_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from AutoTable inner join car_brand on AutoTAble.Марка = car_brand.Марка where car_brand.Производитель = 'General Motors'");
        }

        private void Wolkswagen_Click(object sender, RoutedEventArgs e)
        {
            GetDataFromDB("Select * from AutoTable inner join car_brand on AutoTAble.Марка = car_brand.Марка where car_brand.Производитель = 'Wolkswagen'");
        }       

        private void marka_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            marka = selectedItem.Content.ToString();
        }
    }
}
