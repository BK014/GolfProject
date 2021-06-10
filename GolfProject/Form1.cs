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

namespace GolfProject
{
    public partial class Form1 : Form
    {
        //connection string
        private string connectionString = @"Data Source=LAPTOP-AAPS5QSM\SQLEXPRESS;Initial Catalog=Golf;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;";
        SqlConnection Con = new SqlConnection();
        DataTable GolfTable = new DataTable();
        public Form1()
        {
            InitializeComponent();
            Con.ConnectionString = connectionString;
        }

        private void btnLoadDB_Click(object sender, EventArgs e)
        {
            loaddb();

        }

        private void loaddb()
        {
            //load datatable colums
            datatablecolumns();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string QueryString = @"SELECT * FROM Golf order by ID";
                //open your connection 
                connection.Open();

                SqlCommand Command = new SqlCommand(QueryString, connection);
                //start your DB reader

                SqlDataReader reader = Command.ExecuteReader();

                while (reader.Read()) //looping through data
                {
                    //add in each row to the datatable
                    GolfTable.Rows.Add(
                        reader["ID"],
                        reader["Title"],
                        reader["Firstname"],
                        reader["Surname"],
                        reader["Gender"],
                        reader["DOB"],
                        reader["Street"],
                        reader["Suburb"],
                        reader["City"],
                        reader["Available week days"],
                        reader["Handicap"]);

                }
                reader.Close();//close reader
                connection.Close();//close connection
                // add the datatable into your data grid view
                dgvGolf.DataSource = GolfTable;

            }
        }

        private void datatablecolumns()
        {

            //clear the old data
            GolfTable.Clear();
            //add in the column titlees to the datable
            try
            {
                GolfTable.Columns.Add("ID");
                GolfTable.Columns.Add("Title");
                GolfTable.Columns.Add("Firstname");
                GolfTable.Columns.Add("Surname");
                GolfTable.Columns.Add("Gender");
                GolfTable.Columns.Add("DOB");
                GolfTable.Columns.Add("Street");
                GolfTable.Columns.Add("Suburb");
                GolfTable.Columns.Add("City");
                GolfTable.Columns.Add("Availavle week days");
                GolfTable.Columns.Add("Handicap");

            }
            catch (Exception s)
            {
                //MessageBox.Show("Data Table not loading"+ s.Message);
            }
        }

        private void dgvGolf_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //get the value of clicked cell
                string newvalue = dgvGolf.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                //show the value on the header
                this.Text = "Row :" + e.RowIndex.ToString() + " Col : " + e.ColumnIndex.ToString() + " Value =" + newvalue;

                //fill textboxes with the data
                txtID.Text = dgvGolf.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtTitle.Text = dgvGolf.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtFirstname.Text = dgvGolf.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtSurname.Text = dgvGolf.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtGender.Text = dgvGolf.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtDOB.Text = dgvGolf.Rows[e.RowIndex].Cells[5].Value.ToString();
                txtStreet.Text = dgvGolf.Rows[e.RowIndex].Cells[6].Value.ToString();
                txtSuburb.Text = dgvGolf.Rows[e.RowIndex].Cells[7].Value.ToString();
                txtCity.Text = dgvGolf.Rows[e.RowIndex].Cells[8].Value.ToString();
                txtAvailable.Text = dgvGolf.Rows[e.RowIndex].Cells[9].Value.ToString();
                txtHandicap.Text = dgvGolf.Rows[e.RowIndex].Cells[10].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // this puts the parameters into the code so that the data in the text boxes is added to the database
            string NewEntry = "INSERT INTO Golf (Title, Firstname, Surname, Gender, DOB, Street, Suburb, City, [Available week days], Handicap) VALUES(@Title, @Firstname, @Surname, @Gender, @DOB, @Street, @Suburb, @City, @Available,@Handicap)";
            using (SqlCommand newdata = new SqlCommand(NewEntry, Con))
            {

                newdata.Parameters.AddWithValue("@Title", txtTitle.Text);
                newdata.Parameters.AddWithValue("@Firstname", txtFirstname.Text);
                newdata.Parameters.AddWithValue("@Surname", txtSurname.Text);
                newdata.Parameters.AddWithValue("@Street", txtStreet.Text);
                newdata.Parameters.AddWithValue("@Suburb", txtSuburb.Text);
                newdata.Parameters.AddWithValue("@City", txtCity.Text);
                newdata.Parameters.AddWithValue("@Gender", txtGender.Text);
                newdata.Parameters.Add("@DOB", SqlDbType.DateTime).Value =txtDOB.Text;
                newdata.Parameters.AddWithValue("@Handicap", txtHandicap.Text);
                newdata.Parameters.AddWithValue("@Available", txtAvailable.Text);
                Con.Open(); //open a connection to the database
                            //its a NONQuery as it doesn't return any data its only going up to the server
                newdata.ExecuteNonQuery(); //Run the Query
                Con.Close(); //Close a connection to the database
                             //a happy message box23 May 2019 Introduction to C#.Net
                MessageBox.Show("Data has been Inserted !! ");
            }
            //Run the LoadDatabase method we made earler to see the new data.
            loaddb();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // this puts the parameters into the code so that the data in the text boxes is added to the database
            
            string updatestatement = "UPDATE Golf set Title=@Title, Firstname=@Firstname, Surname=@Surname, Gender = @Gender, DOB = @DOB, Street = @Street, Suburb = @Suburb, City = @City, [Available week days]= @Available, Handicap = @Handicap where ID = @ID";


            using (SqlCommand update = new SqlCommand(updatestatement, Con))
            {
                update.Parameters.AddWithValue("@ID", txtID.Text);

                update.Parameters.AddWithValue("@Title", txtTitle.Text);
                update.Parameters.AddWithValue("@Firstname", txtFirstname.Text);
                update.Parameters.AddWithValue("@Surname", txtSurname.Text);
                update.Parameters.AddWithValue("@Street", txtStreet.Text);
                update.Parameters.AddWithValue("@Suburb", txtSuburb.Text);
                update.Parameters.AddWithValue("@City", txtCity.Text);
                update.Parameters.AddWithValue("@Gender", txtGender.Text);
                update.Parameters.Add("@DOB", SqlDbType.DateTime).Value = txtDOB.Text;
                update.Parameters.AddWithValue("@Handicap", txtHandicap.Text);
                update.Parameters.AddWithValue("@Available", txtAvailable.Text);
                Con.Open(); //open a connection to the database
                            //its a NONQuery as it doesn't return any data its only going up to the server
                update.ExecuteNonQuery(); //Run the Query
                Con.Close(); //Close a connection to the database
                             //a happy message box23 May 2019 Introduction to C#.Net
                MessageBox.Show("Data has been updated!! ");
            }
            //Run the LoadDatabase method we made earler to see the new data.
            loaddb();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string DeleteCommand = "Delete Golf where ID = @ID";
            SqlCommand DeleteData = new SqlCommand(DeleteCommand, Con);
            DeleteData.Parameters.AddWithValue("@ID", txtID.Text);
            Con.Open();
            DeleteData.ExecuteNonQuery();
            Con.Close();
            MessageBox.Show("Data has been deleted!! ");
            loaddb();

        }
    }
       
}
