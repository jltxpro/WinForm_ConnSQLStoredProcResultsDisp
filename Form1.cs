using Utilities;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using WinForm_ConnSQLStoredProcResultsDisp.Services;

namespace WinForm_ConnSQLStoredProcResultsDisp
{
    public partial class Form1 : Form
    {
        //constructor 
        public Form1()
        {
            InitializeComponent();
            this.lblMessage.Visible = false;

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string connString;
            this.lblMessage.Visible = false;

            // Verify that the fields are  not empty 
            if (string.IsNullOrWhiteSpace(this.txtServer.Text) ||
                string.IsNullOrWhiteSpace(this.txtDatabase.Text) ||
                string.IsNullOrWhiteSpace(this.txtUser.Text) ||
                string.IsNullOrWhiteSpace(this.txtPassword.Text) ||
                string.IsNullOrWhiteSpace(this.txtStoredProcedure.Text))
            {
                showError("Please complete all SQL Server Connection Settings");
                return;
            }

            // Set the wait cursor
            Cursor.Current = Cursors.WaitCursor;

            // Create the SQL connection string 
            connString = String.Format("server={0};user id={1}; password={2}; database={3}; port={4}; SslMode={5}",
                 this.txtServer.Text, this.txtUser.Text, this.txtPassword.Text, this.txtDatabase.Text, this.txtPort.Text, this.txtSSL.Text);

            try
            {
                ConnectionFactory cF = new ConnectionFactory();
                IDbConnection connection = cF.CreateConnection(connString);

                // Run the stored procedure 
                SQLService sqlService = new SQLService();
                DataTable result = sqlService.GetAll(connection, this.txtStoredProcedure.Text);
                // Populate the grid with the data  
                this.dataGridView1.DataSource = result;

            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }

            // Set the default cursor
            Cursor.Current = Cursors.Default;
        }


        private void showError(string message)
        {
            this.lblMessage.ForeColor = Color.Red;
            this.lblMessage.Text = $"Error: {message}";
            this.lblMessage.Visible = true;
        }

        private void btnViewPwd_Click(object sender, EventArgs e)
        {
            // show or hide the password textbox 
            if(this.txtPassword.UseSystemPasswordChar == true)
            {
                this.txtPassword.UseSystemPasswordChar = false;
                this.btnViewPwd.Text = "Hide";
            }
            else
            {
                this.txtPassword.UseSystemPasswordChar = true;
                this.btnViewPwd.Text = "Show";
            }
                
        }
    }
}
