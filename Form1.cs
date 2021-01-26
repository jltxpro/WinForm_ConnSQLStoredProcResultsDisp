using CobraCore.Utilities;
using Entities.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            this.lblMessage.Visible = false;

            // Verify that the fields are  not empty 
            if (string.IsNullOrWhiteSpace(this.txtServer.Text) ||
                string.IsNullOrWhiteSpace(this.txtDatabase.Text) ||
                string.IsNullOrWhiteSpace(this.txtUser.Text) ||
                string.IsNullOrWhiteSpace(this.txtPassword.Text) ||
                string.IsNullOrWhiteSpace(this.txtPort.Text) ||
                string.IsNullOrWhiteSpace(this.txtSSL.Text) ||
                string.IsNullOrWhiteSpace(this.txtStoredProcedure.Text))
            {
                this.lblMessage.ForeColor = Color.Red;
                this.lblMessage.Text = "Error: Please complete all SQL Server Connection Settings.";
                this.lblMessage.Visible = true;

                return;
            }

            string connString = String.Format("server={0};port={1};user id={2}; password={3}; database={4}; SslMode={5}",
                    this.txtServer.Text, this.txtPort.Text, this.txtUser.Text, this.txtPassword.Text, this.txtDatabase.Text, this.txtSSL.Text);

            ConnectionFactory cF = new ConnectionFactory();
            MySqlConnection connection = cF.CreateConnection(connString);
            StoredProcedureSchema spSchema = new StoredProcedureSchema();

            var result = GetAll(connection, spSchema, this.txtStoredProcedure.Text);

            /*
            try
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = this.txtStoredProcedure.Text; //obj.GetCommandText_GetAll();

                // Let's run the stored proc to get the column names, 
                //DataTable table = connection.GetSchema("Procedures");
                //DisplayData(table);

                //DbParameterService.AddWithValue(command, "_subscriberid", subscriberId);

                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    bool hasRows = false;

                    while (reader.Read())
                    {
                        if (!hasRows)
                            hasRows = true;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                this.lblMessage.ForeColor = Color.Red;
                this.lblMessage.Text = $"Error: {ex.Message}";
                this.lblMessage.Visible = true;
                //throw new Exception(ex.Message);
            }
            finally
            {
                if (connection is IDisposable)
                {
                    var disposableConnection = connection as IDisposable;
                    disposableConnection.Dispose();
                }
            }
            */


            // if connection tested OK, then proceed with running stored proc
            //var companies = new Interactor<Company>(new ConnectionFactory()).GetAll(subscriberId, new Company());
            //var result = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            //

        }



        public IEnumerable<StoredProcedureSchema> GetAll(MySqlConnection connection, 
            StoredProcedureSchema obj, string storedProcedureName)
        {
            var objList = new List<StoredProcedureSchema>();

            try
            {
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storedProcedureName;

                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    bool hasRows = false;

                    while (reader.Read())
                    {
                        var tmpObj = Activator.CreateInstance(typeof(StoredProcedureSchema));

                        foreach (var item in obj.GetFieldMapper())
                        {
                            var propertyName = item.Key;
                            var propertyInfo = tmpObj.GetType().GetProperty(propertyName);

                            // make sure object has the property 
                            if (propertyInfo != null)
                            {
                                string dbField = item.Value;
                                if (propertyInfo.PropertyType == typeof(System.DateTime) ||
                                            propertyInfo.PropertyType == typeof(System.Int32))
                                    propertyInfo.SetValue(tmpObj, reader[dbField]);
                                else
                                    propertyInfo.SetValue(tmpObj, reader[dbField].ToString());
                            }
                        }

                        objList.Add((StoredProcedureSchema)tmpObj);

                        if (!hasRows)
                            hasRows = true;
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // throw new Exception(ex.Message);
                this.lblMessage.ForeColor = Color.Red;
                this.lblMessage.Text = $"Error: {ex.Message}";
                this.lblMessage.Visible = true;
            }
            finally
            {
                if (connection is IDisposable)
                {
                    var disposableConnection = connection as IDisposable;
                    disposableConnection.Dispose();
                }
            }

            return objList;
        }








        private static void DisplayData(System.Data.DataTable table)
        {
            foreach (System.Data.DataRow row in table.Rows)
            {
                foreach (System.Data.DataColumn col in table.Columns)
                {
                    Debug.WriteLine("{0} = {1}", col.ColumnName, row[col]);
                }
                Debug.WriteLine("============================");
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
