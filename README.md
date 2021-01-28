# WinForm_ConnSQLStoredProcResultsDisp
WinForm_ConnSQLStoredProcResultsDisp

01/28/2021: Code refactoring, optimized Data Reader logic, reorganized UI. I will run tests tonight on MS SQL server. 

01/27/2021: Future upgrades. 
* Add a limit on the output rows ?. Since this App is able to connect to any stored procedure.

01/26/2021: This Windows Forms App calls a SQL server stored procedure that SELECTS data from a Database and displays the results in a DataGrid control.
The user is able to configure the Connection String, and specify the Stored Procedure to call. The app does not currently accept parametized SPs, but can be modified in the future to do so. 


