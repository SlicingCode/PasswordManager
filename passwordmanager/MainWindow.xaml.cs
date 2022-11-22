using passwordmanager.ViewModels;
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

namespace passwordmanager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GeneratePW _generatedPW;

        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        private int ManagerId = 0;
        public MainWindow()
        {
            InitializeComponent();
            GetData();
            _generatedPW = new GeneratePW();
            
        }

        public void mycon()
        {
            String Conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new SqlConnection(Conn);
            con.Open();
        }

        private void Button_Click_Generate(object sender, RoutedEventArgs e)
        {
            try
            {
                _generatedPW.CurrentGen.Generated = _generatedPW.CurrentGen.Generated;
                DataContext = _generatedPW;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            try
            {
                // Check validation
                if (txtWebsite.Text == null || txtWebsite.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter a website", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtWebsite.Focus();
                    return;
                }
                else if (txtPassword.Text == null || txtPassword.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter password", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtPassword.Focus();
                    return;
                }
                else
                {
                    //Edit time and set that record Id in ManagerId variable.
                    //Code to Update. If ManagerId greater than zero than it is go for update.
                    if (ManagerId > 0)
                    {
                        //Show the confirmation message
                        if (MessageBox.Show("Are you sure you want to update ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            mycon();
                            DataTable dt = new DataTable();

                            //Update Query Record update using Id
                            cmd = new SqlCommand("UPDATE Manager_Master SET Website = @Website, Password = @Password WHERE Id = @Id", con);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Id", ManagerId);
                            cmd.Parameters.AddWithValue("@Website", txtWebsite.Text);
                            cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Data updated successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        // code to save
                        if (MessageBox.Show("Are you sure you want to save ?", "Information", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            mycon();
                            //Insert query to Save data in the table
                            cmd = new SqlCommand("INSERT INTO Manager_Master(Website, Password) VALUES(@Website, @Password)", con);
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Website", txtWebsite.Text);
                            cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Data saved successfully", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    ClearMaster();
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            
            
        }

        // Bind data to datagrid
        public void GetData()
        {
            // Connect with database
            mycon();

            // Create Datatable object
            DataTable dt = new DataTable();

            // Write SQL query to get the data from database table
            cmd = new SqlCommand("SELECT * FROM Manager_Master", con);

            //CommandType define which type of command will execute like Text, StoredProcedure, TableDirect.
            cmd.CommandType = CommandType.Text;

            // Accept a parameter that contains the command text of the object's SelectCommand property.
            da = new SqlDataAdapter(cmd);

            //The DataAdapter serves as a bridge between a DataSet and a data source for retrieving and saving data. 
            //The fill operation then adds the rows to destination DataTable objects in the DataSet
            da.Fill(dt);

            //dt is not null and rows count greater than 0
            if (dt != null && dt.Rows.Count > 0)
                //Assign DataTable data to datam using item source property.
                datam.ItemsSource = dt.DefaultView;
            else
                datam.ItemsSource = null;

            //Database connection close
            con.Close();
        }

        //Method is used to clear all the input which user entered in currency master tab
        private void ClearMaster()
        {
            try
            {
                txtWebsite.Text = string.Empty;
                txtPassword.Text = string.Empty;
                btnSave.Content = "Save";
                GetData();
                ManagerId = 0;
                txtWebsite.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
