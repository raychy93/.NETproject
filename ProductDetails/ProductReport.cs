/*
 *I, Rayhan Chowdhury, 000306853 certify that this material is my original work. No other person's work has been used without due acknowledgement. 
 * 
 * Class:ProductReport.cs
 * By:Rayhan Chowdhury
 * Date:5/2/2021
 * Purpose: The purpose of this class are to define the event handlers and load the form of the ProductReport.cs form.
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



    public partial class ProductReport : Form
    {

        //Connection to SQL database (NorthWind).
        /// <value>Gets the value of Connection.</value>
        public SqlConnection Connection { get; }

        //Connection String for SQL Database.
        String connectionString = @"Data Source =.\sqlexpress; Initial Catalog = Northwind; Integrated Security = True";
        
        //List of order id's.
        List<int> orders = new List<int>();

       
        //Dictionary of string type for keys and short type for values 
        Dictionary<string, short> dict = new Dictionary<string, short>();


        //Form constructor.
        public ProductReport()
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
        /// This method runs when the ProductReport form is loaded. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ProductReport_Load(object sender, EventArgs e)
        {

            try
            {
                //SQL command for all units in stock greater than 30 from products in descending fashion.
                SqlCommand command = new SqlCommand("SELECT * FROM PRODUCTS WHERE UnitsInStock>30 ORDER BY ProductId DESC", Connection);

                //Executes command and reads.
                SqlDataReader reader = command.ExecuteReader(); 
                while (reader.Read())
                {
                    //Add all unit in stock items greater than 30 by product ID inside combobox.
                    comboBox1.Items.Add(reader[0].ToString()); 
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
        /// This method runs when a selection is made on comboBox1.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //List is cleared to ensure results are not appended on one line for different selections in combobox.
            orders.Clear(); 

            try
            {   //Command for deriving dataset from Order Details Extended where any selected item in combobox is equal to ProductId.
                SqlCommand command = new SqlCommand("SELECT * FROM [Order Details Extended] WHERE ProductId = '" + comboBox1.Text + "'", Connection);


                //Executes command and reads.
                SqlDataReader reader = command.ExecuteReader(); 
                while (reader.Read())
                {
                    //Orderid is in index 0 of dataset, since type is int,
                    //reader will parse for equivalent to int which is .GetInt32,
                    //and is thus added to Orders list.
                    orders.Add(reader.GetInt32(0));

                    //Creates enumerable list (via linq) of order by
                    //ascension ( using orderby method via Linq) of existing orders list.
                    var AscendingOrder = orders.OrderBy(i => i);

                    //Product name is in index 2 of dataset, reader parses, returns as string via ToString.
                    string Name = reader[2].ToString();

                    //Product id is in index 1, reader parses, returns as string via ToString. 
                    string ProductId = reader[1].ToString();

                    //String.join concatenates comma in between every value of enumerated list,
                    //orderid's printed in one line in order ID textbox
                    orderID.Text = string.Join(",", AscendingOrder);

                    //productName textbox prints product name.
                    productName.Text = Name;

                    //productID textbox prints product Id.
                    productID.Text = ProductId; 
                    
                    
                }
                //Reader closed.
                reader.Close();

                //Label states success if command succeeds.
                statusLabel.Text = "Database Select Success";



            }
            catch (Exception ex)
            {
                //Label states success if command succeeds.
                statusLabel.Text = "Database operation failed:" + ex.Message;  
            }


        }




        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }


        /// <summary>
        /// This method runs when button1 is pressed. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {       //Command for deriving dataset from Products where any selected item in combobox is equal to ProductId.
                SqlCommand command = new SqlCommand("SELECT * FROM Products WHERE ProductId = '" + comboBox1.Text + "'", Connection);

                //Executes command and reads.
                SqlDataReader reader = command.ExecuteReader(); 
                while (reader.Read())
                {
                    //Calls function AddToDictionary, comboBox1.Text being the selected product ID is in the key field of type string,
                    //while the reader parses the unit stock value and retrieved value is in the value field of type short (since in SQL, it is Int16) in the function,
                    // whereas dict is in the dictionary field of function.
                    
                    AddToDictionary(dict, comboBox1.Text, reader.GetInt16(6));
                    

                    //Sum variable to add for average.
                    int AddedNums = 0; 
                    foreach(var item in dict.Values) 
                    {
                        //Item sums every item in list.
                        AddedNums += item;  
                    }

                    //Average in decimal type calculated by dividing between the sum of values in dictionary
                    //with the amount of values in dictionary.
                    decimal Average = AddedNums / dict.Values.Count();

                    //Average is rounded to two decimal places via Math.Round and utilizes
                    //MidpointRounding to round to the nearest number away from zero,
                    //essentially rounding the number up, and is returned as string
                    //via toString() in updateAverage textbox.
                    updateAverage.Text = Math.Round(Average, 2, MidpointRounding.AwayFromZero).ToString();
                }

                //Reader closed.
                reader.Close(); 

                //Label states success if command succeeds.
                statusLabel.Text = "Database Select Success"; 



            }
            catch (Exception ex)
            {
                //Label states failed if operation fails.
                statusLabel.Text = "Database operation failed:" + ex.Message; 
            }
            
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }



        /// <summary>
        /// The purpose of this method is to ensure there are no duplicate key values in the dictionary.
        /// </summary>
        /// <param name="TheDictionary"></param>
        /// <param name="TheKey"></param>
        /// <param name="TheValue></param>
        public void AddToDictionary(Dictionary<string, short> TheDictionary, string TheKey, short TheValue)
        {
            //If key is not already in the dictionary, then add the key and value to the dictionary.
            if (!TheDictionary.ContainsKey(TheKey))
            {
                TheDictionary.Add(TheKey, TheValue);
            }
            else
            {
                //Otherwise, if key is already in dictionary, and user selects same key, 
                //messagebox will appear to tell user that the key has already been added previously.
                string Message = "This Product ID's Unit Stock Value is already in the average! Please select another Product ID.";
                string Caption = "Error in Selection";
                MessageBoxButtons Buttons = MessageBoxButtons.OK;
                DialogResult Result;

                Result = MessageBox.Show(Message, Caption, Buttons);
               

            }
        }

        /// <summary>
        /// This method runs when button2 is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //Close Form.
            this.Close();
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }
    }
}
