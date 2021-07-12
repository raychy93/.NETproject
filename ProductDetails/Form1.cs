/*
 *I, Rayhan Chowdhury, 000306853 certify that this material is my original work. No other person's work has been used without due acknowledgement. 
 * 
 * Class:Form1.cs
 * By:Rayhan Chowdhury
 * Date:5/2/2021
 * Purpose: The purpose of this class are to define the event handlers and load the form of the Form1.cs form
 * 
*/




using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Product_Details
{
    /// <summary>
    /// This class dictates the event handlers of the properties of the form.
    /// </summary>

    public partial class Form1 : Form
    {
        // Connection to SQL database (NorthWind).
        /// <value>Gets the value of Connection.</value>
        public SqlConnection Connection { get; }

        // Connection String for SQL Database.
        String connectionString = @"Data Source =.\sqlexpress; Initial Catalog = Northwind; Integrated Security = True";

        // Form constructor.
        public Form1()
        {
            InitializeComponent();

            try
            {
                Connection = new SqlConnection();
                Connection.ConnectionString = connectionString;
                Connection.Open();
                //Label outputs successful database connection confirmation.
                statusLabel.Text = "Connected to Database Successfully"; 
            }
            catch (Exception ex)
            {
                //Label outputs failure to connect to database.
                statusLabel.Text = "Database Connection failed - check Connection String : " + 
                ex.Message;                                             
            }


        }

        /// <summary>
        /// This method runs when the Form1 is loaded. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {

            try
            {   //SQL command, select query of all items from table products.
                SqlCommand command = new SqlCommand("SELECT * FROM PRODUCTS", Connection);

                //Executes command.
                SqlDataReader reader = command.ExecuteReader(); 
                while (reader.Read()) 
                {
                    //Combobox appends data in index 0, which is product id, returns as string.
                    comboBox1.Items.Add(reader[0].ToString()); 
                }
                //Reader closed.
                reader.Close();
                //Label states success if command succeeds.
                statusLabel.Text = "Database Select Success";



            } catch (Exception ex)
            {
                //Label states failure if operation has error.
                statusLabel.Text = "Database operation failed:" + ex.Message;
            }
        }

        /// <summary>
        /// This method runs when there is a selection made on comboBox1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                //SQL command for select query of all items in products table where product id
                //is equal to the text in selection in combobox.
                SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE ProductId = '" + comboBox1.Text + "'", Connection);

                //Command executed.
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //Reader parses for product name in index 1 and returns as string, stored in string variable.
                    string ProductName = reader[1].ToString();
                    //Reader parses for quantity unit in index 4 and returns as string, stored in string variable.
                    string QuantityUnit = reader[4].ToString();
                    //Reader parses for unit price in index 5 and returns as string, stored in string variable.
                    string UnitPrice = reader[5].ToString();
                    //Reader parses for unit stock in index 6 and returns as string, stored in string variable.
                    string UnitStock = reader[6].ToString(); 
                    //Textbox1 labels product name variable as text.
                    productName.Text = ProductName;
                    //Textbox2 labels unit price variable as text.
                    productPrice.Text = UnitPrice;
                    //Textbox3 labels quantity unit variable as text.
                    quantityUnit.Text = QuantityUnit;
                    //Textbox4 labels unit price variable as text.
                    unitStock.Text = UnitStock;
                    



                }
                //Reader closed.
                reader.Close();
                //Label states success if command succeeds.
                statusLabel.Text = "Database Select Success";




            }
            catch (Exception ex)
            {
                //Label states failure if operation has error.
                statusLabel.Text = "Database operation failed:" + ex.Message;
            }






        }

        /// <summary>
        /// This method runs when button1 is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button1_Click(object sender, EventArgs e)
        {

            //New ProductReport form object created.
            ProductReport ReportForm = new ProductReport();
            //Form is shown as a model dialog box.
            ReportForm.ShowDialog();
            



        }
    }

       
    }

