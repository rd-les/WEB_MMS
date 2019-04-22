using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace Web_LED.App_Class{
    public class ClassDataBase    {

        private SqlConnection conn; 
        //private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private SqlDataAdapter dataAdapter;
        //private string connectionString = "Server=192.168.18.100;Uid=sa;PASSWORD=LED123456!;database=LED;Max Pool Size=400;Connect Timeout=600;"; //FOR SERVER 
        private string connectionString = ConfigurationManager.ConnectionStrings["MmsConnectionString"].ToString();  //FOR SERVER 
        //private string connectionStringMMS = "Server=192.168.18.100;Uid=sa;PASSWORD=LED123456!;database=MMS;Max Pool Size=400;Connect Timeout=600;"; //FOR SERVER 
        public ClassDataBase() {

            conn = getConnection();
            try {                
                conn.Open();
            }
            catch (SqlException ex) {
                // output the error to see what's going on
                //MessageBox.Show(ex.Message);
                Debug.WriteLine(ex); 
            }
            
        }

        public void closeConnection() {
            this.conn.Close();
        }
        
        private SqlConnection getConnection() {
            //connectionString = "Server=WIN-E35V46E7IOH;Uid=sa;PASSWORD=Pong1981;database=LED;Max Pool Size=400;Connect Timeout=600;";
            //connectionString = "Server=WIN-E35V46E7IOH;Uid=sa;PASSWORD=Pong1981;database=LED;Max Pool Size=400;Connect Timeout=600;";
            //connectionString = "Server=127.0.0.1;Uid=sa;PASSWORD=LED123456!;database=LED;Max Pool Size=400;Connect Timeout=600;"; //FOR SERVER
            //connectionString = "Server=192.168.18.100;Uid=sa;PASSWORD=LED123456!;database=COUPON_TEST;Max Pool Size=400;Connect Timeout=600;"; //FOR SERVER
            //connectionString = "Server=192.168.18.100;Uid=sa;PASSWORD=LED123456!;database=LED;Max Pool Size=400;Connect Timeout=600;"; //FOR SERVER
            //conn = new SqlConnection(connectionString); 
            //SqlConnection conn = new SqlConnection(connectionString);


            try {
                conn = new SqlConnection(connectionString);
                return conn; 
            }
            catch (SqlException) {
                Console.WriteLine("Can't Connect");
                return null; 
            }
        }

        public void exeCuteCommand(string sql) {

            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
            Debug.WriteLine(sql);
        }

        public void insertCommand(string sqlInsert) {
            
            SqlCommand cmd = new SqlCommand(sqlInsert, conn);
            cmd.ExecuteNonQuery();
            Debug.WriteLine(sqlInsert);
        }


        public void insertCommandCommit(string sql) {
            
            SqlCommand command = conn.CreateCommand();
            SqlTransaction transaction;

            transaction = conn.BeginTransaction("SampleTransaction");

            // Must assign both transaction object and connection
            // to Command object for a pending local transaction
            command.Connection = conn;
            command.Transaction = transaction;

            try {
                command.CommandText = sql;                
                command.ExecuteNonQuery();
                // Attempt to commit the transaction.
                transaction.Commit();
                //Console.WriteLine("Both records are written to database.");
            }
            catch (Exception ex) {
                Debug.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Debug.WriteLine("  Message: {0} : "+sql, ex.Message);

                // Attempt to roll back the transaction.
                try {
                    transaction.Rollback();
                }
                catch (Exception ex2) {
                    // This catch block will handle any errors that may have occurred
                    // on the server that would cause the rollback to fail, such as
                    // a closed connection.
                    Debug.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Debug.WriteLine("  Message: {0}", ex2.Message);
                }
            }
            // Attempt to commit the transaction.
           // Debug.WriteLine("Both records are written to database.");
           
        }

        public void insertData(Dictionary<string, string> fields, string tableName) {
            StringBuilder sqlInsert = new StringBuilder();
            StringBuilder nameFileds = new StringBuilder();
            StringBuilder nameValues = new StringBuilder();

            sqlInsert.Append(" INSERT INTO "+ tableName);

            nameFileds.Append(" ( ");
            nameValues.Append(" ( ");
            int countField = 0;
            foreach (KeyValuePair<string, string> pairKV in fields) {
                if (countField > 0) {
                    nameFileds.Append(" , ");
                    nameValues.Append(" , ");
                }
                nameFileds.Append(pairKV.Key);

                if (!string.IsNullOrEmpty(pairKV.Value) && pairKV.Value.Trim().ToUpper().IndexOf("to_date".ToUpper()) > -1) {
                    nameValues.Append(pairKV.Value.ToString());
                }
                else {
                    nameValues.Append("'" + pairKV.Value + "'");
                }
                countField++;
            }
            nameFileds.Append(" ) ");
            nameValues.Append(" ) ");
            sqlInsert.Append(" " + nameFileds.ToString() + " VALUES " + nameValues);
            Debug.WriteLine(sqlInsert.ToString());
            insertCommand(sqlInsert.ToString());
            

            sqlInsert.Clear();
        }

        public void insertOnceRow(string fieldName, string value, string tableName) {
            StringBuilder sqlInsert = new StringBuilder();

            sqlInsert.Append(" INSERT INTO " + tableName);
            sqlInsert.Append("  (" + fieldName + ") VALUES ('" + value + "')");
            Debug.WriteLine(sqlInsert.ToString());
            this.insertCommand(sqlInsert.ToString());
            sqlInsert.Clear();

        }

        public void updateCommand(string sqlUpdate) {

            SqlCommand cmd = new SqlCommand(sqlUpdate, conn);
            cmd.ExecuteNonQuery();
            Debug.WriteLine(sqlUpdate);
        }

        public void updateData(Dictionary<string, string> fields, string tableName, string whereSql) {

            StringBuilder sqlUpdate = new StringBuilder();

            sqlUpdate.Append(" UPDATE "+ tableName);
            sqlUpdate.Append(" SET ");
            int countField = 0;
            foreach (KeyValuePair<string, string> pairKV in fields) {
                if (countField > 0) sqlUpdate.Append(" , ");
                if (!string.IsNullOrEmpty(pairKV.Value) && pairKV.Value.Trim().ToUpper().IndexOf("to_date".ToUpper()) > -1) {
                    sqlUpdate.Append(pairKV.Key + "=" + pairKV.Value.ToString() + "");
                }
                else {
                    sqlUpdate.Append(pairKV.Key + "='" + pairKV.Value + "'");
                }
                countField++;
            }

            if (!whereSql.Equals("")) {
                sqlUpdate.Append(" WHERE " + whereSql);
            }
            Debug.WriteLine("----->" + sqlUpdate);
            updateCommand(sqlUpdate.ToString());
            sqlUpdate.Clear();
        }

        public void updateOnceRow(string fieldName, string value, string tableName, string whereSql) {
            StringBuilder sqlUpdate = new StringBuilder();

            sqlUpdate.Append(" UPDATE " + tableName);

            sqlUpdate.Append(" SET ");
            if (!string.IsNullOrEmpty(value) && value.Trim().ToUpper().IndexOf("to_date".ToUpper()) > -1) {
                sqlUpdate.Append(fieldName + "=" + value.ToString() + "");
            }
            else {
                sqlUpdate.Append(fieldName + "='" + value + "'");
            }

            //sqlUpdate.Append(" SET " + fieldName + "= '" + value + "'");
            sqlUpdate.Append(" WHERE " + whereSql);

            Debug.WriteLine(sqlUpdate.ToString());
            updateCommand(sqlUpdate.ToString());
            sqlUpdate.Clear();

        }

        public void deleteData(string tableName, string whereSql) {
            StringBuilder sqlDelete = new StringBuilder();
            sqlDelete.Append("DELETE FROM " +tableName);
            sqlDelete.Append(" WHERE " + whereSql);
            this.updateCommand(sqlDelete.ToString());
            sqlDelete.Clear();
        }


        public DataTable getDataTable(string sql) {

            DataTable dataTable = new DataTable();
            
            SqlCommand cmd = new SqlCommand(sql, conn);
            this.dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dataTable);
            dataAdapter.Dispose();
           
            return dataTable;

        }


        public DataRow getDataRowProcedure(string sql, Dictionary<string, string> dataParams) {
            DataTable dt = getDataTableProcedure(sql, dataParams);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        public DataTable getDataTableProcedure(string sql , Dictionary<string, string> dataParams) {

            
            DataTable dataTable = new DataTable();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            foreach (KeyValuePair<string, string> pairKV in dataParams) {

                cmd.Parameters.Add(new SqlParameter(pairKV.Key, SqlDbType.VarChar)).Value = pairKV.Value ; 
             
            }

            //cmd.Parameters.Add("@workstation_no" , SqlDbType.VarChar ); 
            // cmd.Parameters["@workstation_no"].Value = "WO1803512";

            //cmd.Parameters.Add(new SqlParameter("@workstation_no", SqlDbType.VarChar)).Value = "WO1803512"; 

            this.dataAdapter = new SqlDataAdapter(cmd);
            dataAdapter.Fill(dataTable);
            dataAdapter.Dispose();

            return dataTable;
        }

        public DataTable getDataTable(string fileds, string tableName, string whereSql) {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT ");
            strSql.Append(fileds);
            strSql.Append(" FROM ");
            strSql.Append(tableName);
            if (!whereSql.Equals("")) {
                strSql.Append(" WHERE " + whereSql);
            }
            // Debug.Print(strSql.ToString());
            DataTable dataTable = this.getDataTable(strSql.ToString());
            return dataTable;
        }


        public String selectOnceData(String table, String field, String where) {
            String strSql = "SELECT " + field + " FROM " + table + " WHERE  " + where;
            DataRow dr = getDataRow(strSql);

            if (dr == null) { return ""; }
            if (!string.IsNullOrEmpty(dr[field].ToString())) {
                return dr[field].ToString();
            }
            else return "";
        }

        public DataRow getDataRow(string pSql) {
            //Debug.WriteLine("DEBUG SQL : " + pSql);
            DataTable dt = getDataTable(pSql);
            return dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }


        public String nextNumber(string fieldName, string tableName, string whereSql) {

            string sql = "SELECT MAX(" + fieldName + ") + 1 as nextNumber FROM " + tableName + "  ";
            if (!whereSql.Equals("")) {
                sql += " WHERE " + whereSql;
            }
            Debug.WriteLine(sql);
            DataRow dr = getDataRow(sql);

            if (string.IsNullOrEmpty(dr["nextNumber"].ToString()) || dr["nextNumber"].Equals("")) { return ConfigClass.NEXT_NUMBER_START; }
            else { return dr["nextNumber"].ToString(); }
        }

        public string nextId(string tableName) {
            string sql = "SELECT IDENT_CURRENT('"+ tableName + "')+1 as next_id;";
            DataRow dr = getDataRow(sql);
            return dr["next_id"].ToString(); 

        }
        public String getCurrentId(string fieldName, string tableName, string whereSql) {

            string sql = "SELECT MAX(" + fieldName + ") as nextNumber  FROM " + tableName + "  ";
            if (!whereSql.Equals("")) {
                sql += " WHERE " + whereSql;
            }
            //Debug.WriteLine(sql);
            DataRow dr = getDataRow(sql);

            if (string.IsNullOrEmpty(dr["nextNumber"].ToString()) || dr["nextNumber"].Equals("")) { return ConfigClass.NEXT_NUMBER_START_ZERO; }
            else { return dr["nextNumber"].ToString(); }
        }


    }
}