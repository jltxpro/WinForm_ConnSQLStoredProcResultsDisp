using Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

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
/*
        private void label1_Click(object sender, EventArgs e)
        {

        }
*/
        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.lblMessage.Visible = false;

            // Verify that the fields are  not empty 
            if (string.IsNullOrWhiteSpace(this.txtServer.Text) ||
                string.IsNullOrWhiteSpace(this.txtDatabase.Text) ||
                string.IsNullOrWhiteSpace(this.txtUser.Text) ||
                string.IsNullOrWhiteSpace(this.txtPassword.Text) ||
                string.IsNullOrWhiteSpace(this.txtStoredProcedure.Text))
            {
                showError("Error: Please complete all SQL Server Connection Settings");
                return;
            }

            // Set the wait cursor
            Cursor.Current = Cursors.WaitCursor;

            // Create the SQL connection string 
            string connString = String.Format("server={0};user id={1}; password={2}; database={3}",
                    this.txtServer.Text, this.txtUser.Text, this.txtPassword.Text, this.txtDatabase.Text);

            ConnectionFactory cF = new ConnectionFactory();
            SqlConnection connection = cF.CreateConnection(connString);

            // Run the stored procedure 
            var result = GetAll(connection, this.txtStoredProcedure.Text);
            // Populate the grid with the data  
            this.dataGridView1.DataSource = result;

            // Set the default cursor
            Cursor.Current = Cursors.Default;
        }


        /*
         *  GetAll()
         *  Returns a DataTable for the specified storedProcedureName by parsing through the columns returned by 
         *  then, creating the columns on the DataTable and lastly adding rows with the data 
         *  returned by the stored proc. 
        */
        private DataTable GetAll(SqlConnection connection, string storedProcedureName)
        {
            DataTable outputDataTable = new DataTable();

            try
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storedProcedureName;

                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    //Get the schema definition for the columns returned by the Stored Procedure
                    DataTable schemaTable = reader.GetSchemaTable();

                    // Create a Dictionary with the Col. Name, Col. Type for each column returned by the Stored Procedure
                    Dictionary<string, string> storedProcedureColumns = 
                                    new Dictionary<string, string>(); 

                    //Read each column definition from the schemaTable
                    for (int i = 0; i < schemaTable.Rows.Count; i++) 
                    {
                        DataRow row = schemaTable.Rows[i];
                        string colName=string.Empty, colDataType=string.Empty;

                        //Retrieve the Column Name and DataType for each column  
                        foreach (DataColumn col in schemaTable.Columns)
                        {
                            if (col.ColumnName == "ColumnName")
                            {
                                colName = row[col.Ordinal].ToString();
                                Debug.WriteLine("{0}", colName);
                            }

                            if (col.ColumnName == "DataType")
                            {
                                colDataType = row[col.Ordinal].ToString();
                                Debug.WriteLine("{0}", colDataType);
                            }
                        }

                        // add Column Name and DataType 
                        storedProcedureColumns.Add(colName, colDataType);
                        outputDataTable.Columns.Add(colName, typeof(string));

                    }

                    while (reader.Read())
                    {
                        // Create a new Row on the DataTable 
                        DataRow dr = outputDataTable.NewRow();

                        // Loops through each column on the storedProcedureColumns 
                        foreach (var item in storedProcedureColumns)
                        {
                            var dbColumnName = item.Key;
                            var dbColumnDataType = item.Value;
                            dr[dbColumnName] = reader[dbColumnName].ToString();
                        }

                        //Add new row to the DataTable
                        outputDataTable.Rows.Add(dr);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
            finally
            {
                if (connection is IDisposable)
                {
                    var disposableConnection = connection as IDisposable;
                    disposableConnection.Dispose();
                }
            }

            return outputDataTable;
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
