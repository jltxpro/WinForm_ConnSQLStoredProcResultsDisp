using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace WinForm_ConnSQLStoredProcResultsDisp.Services
{
    public class SQLService
    {
        /*
         *  GetAll()
         *  Returns a DataTable for the specified storedProcedureName by parsing through the columns returned by 
         *  then, creating the columns on the DataTable and lastly adding rows with the data 
         *  returned by the stored proc. 
        */
        public DataTable GetAll(IDbConnection connection, string storedProcedureName)
        {
            DataTable outputDataTable = new DataTable();

            try
            {
                //Open connection, and prepare the Command to execute 
                connection.Open();
                IDbCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = storedProcedureName;

                using (var reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    //Load the reader results into the DataTable 
                    outputDataTable.Load(reader);
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                // throw exception to caller 
                throw new Exception(ex.Message);
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


    }



}
