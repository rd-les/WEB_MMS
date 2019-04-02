
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Web_LED.App_Class;
using WEB_MMS.Models.V_QW;

namespace WEB_MMS.DataAccessLayer.V_QW {
    public class Dao_ReportDriver {

        private ClassDataBase classDataBase = new ClassDataBase();


        private DataTable createDataTableDriver(string mainDriverId) {
            
            DataTable dataTable = new DataTable("DT_Driver");

            dataTable.Columns.Add("id", typeof(int));
            dataTable.Columns.Add("po_no", typeof(string));
            dataTable.Columns.Add("code_no", typeof(string));
            dataTable.Columns.Add("date_create", typeof(string));
            dataTable.Columns.Add("driver_count", typeof(string));
            dataTable.Columns.Add("driverOK", typeof(int));
            dataTable.Columns.Add("driverNG", typeof(int));



            string sql = @" SELECT * 
                            , (SELECT COUNT(id) FROM FT_driver_main_detail WHERE FT_driver_main_id = FT_driver_main.id AND action_result = 1) AS driverOK
                            , (SELECT COUNT(id) FROM FT_driver_main_detail WHERE FT_driver_main_id = FT_driver_main.id AND action_result = 0) AS driverNG                            
                            FROM FT_driver_main WHERE id=" + mainDriverId;
            DataRow dataRow = classDataBase.getDataRow(sql);        
            dataTable.Rows.Add(dataRow["id"], dataRow["po_no"] , dataRow["code_no"], dataRow["date_create"], dataRow["driver_count"], dataRow["driverOK"], dataRow["driverNG"]);


            return dataTable; 

        }


        private DataTable createDataTableDriverDetail(string mainDriverId) {

            string sqlDetail = @" SELECT 
                                    ROW_NUMBER() OVER (Order by id) AS RowNumber
                                    , test_id
                                    , convert(varchar, create_datetime, 113)	AS create_datetime
                                    , convert(varchar, data_watt) AS data_watt
                                    , convert(varchar, data_pf) AS data_pf
                                    , convert(varchar, data_volt) AS data_volt
                                    , action_result

                                FROM FT_driver_main_detail
                                WHERE FT_driver_main_id = " + mainDriverId;

            DataTable dataTableDetail = classDataBase.getDataTable(sqlDetail);
            
            return dataTableDetail; 
        }




        public void exportReportExcel(string mainDriverId, string serverPath, string codeNo ,string poNo) {

            LocalReport report = new LocalReport();
            report.ReportPath = HostingEnvironment.MapPath("~/Report/QW/Report1.rdlc");

            ReportDataSource datasourceDriver = new ReportDataSource("DT_Driver", this.createDataTableDriver(mainDriverId));
            ReportDataSource datasourceDriverDetail = new ReportDataSource("DT_DriverDetail", this.createDataTableDriverDetail(mainDriverId));


            report.DataSources.Clear();
            report.DataSources.Add(datasourceDriver);
            report.DataSources.Add(datasourceDriverDetail);
            //report.Refresh();

            //ReportDataSource datasource_1 = new ReportDataSource("DataSetChart_1", dataTable );

            string extension;
            string encoding;
            string mimeType;
            string[] streams;
            Warning[] warnings;
            byte[] reportBytes = report.Render(
                 "EXCEL", null, out mimeType, out encoding,
                  out extension,
                 out streams, out warnings);

            string pathExcel = HostingEnvironment.MapPath("/Report/QW/"+mainDriverId+"/" + string.Format("{0}.{1}", poNo+"-"+codeNo, "xls"));
            string pathDirectory = HostingEnvironment.MapPath("/Report/QW/" + mainDriverId);
            Directory.CreateDirectory(pathDirectory);

            using (FileStream fs = File.Create(pathExcel)) {
                fs.Write(reportBytes, 0, reportBytes.Length );
                //formModel.reportPath = pathPdf + "/ReportWorkStation.pdf";
            }

            /*

            LocalReport report = new LocalReport();
            report.ReportPath = HostingEnvironment.MapPath("~/Report/QW/Report1.rdlc");

            ReportParameter[] reportParams = new ReportParameter[1];
            reportParams[0] = new ReportParameter("ReportParameter1", "XXXXXXXXXXXXXXXXXXXXXXXXXXXX", false);



            report.SetParameters(reportParams);
            report.Refresh();



            string extension;
            string encoding;
            string mimeType;
            string[] streams;
            Warning[] warnings;

            try {
                byte[] reportBytes = report.Render(
                  "Excel", null, out mimeType, out encoding,
                   out extension,
                  out streams, out warnings);


                //string filename = string.Format("{0}.{1}", "ExportToExcel", "xls");

                string pathPdf = HostingEnvironment.MapPath("/Report/" + string.Format("{0}.{1}", "ExportToExcel", "xls"));
                //Directory.CreateDirectory(pathPdf);

                using (FileStream fs = File.Create(pathPdf + "/" + ConfigClass.REPORT_FINISH_SHORT_NAME)) {
                    fs.Write(reportBytes, 0, reportBytes.Length);
                    //formModel.reportPath = pathPdf + "/ReportWorkStation.pdf";
                }
            }
            catch (Exception ex) {
                Debug.WriteLine(ex); 
            }
           
            */
            

            /*
            byte[] bytes = ReportViewer1.LocalReport.Render(
                   "Excel", null, out mimeType, out encoding,
                    out extension,
                   out streamids, out warnings);

            filename = string.Format("{0}.{1}", "ExportToExcel", "xls"); 

            Response.ClearHeaders();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
            Response.ContentType = mimeType;
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();

             */

            //return pathExcel ; 

        }

        private DataTable Data() {
            var dataTable = new DataTable("Data");

            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Gender", typeof(string));
            dataTable.Columns.Add("Age", typeof(int));

            dataTable.Rows.Add(1000, "Ahmed", "Male", 22);
            dataTable.Rows.Add(1001, "Mohammed", "Male", 25);
            dataTable.Rows.Add(1002, "Hassan", "Male", 41);
            dataTable.Rows.Add(1003, "Abdullah", "Male", 19);
            dataTable.Rows.Add(1004, "Maryam", "Female", 21);

            return dataTable;
        }

        public string exportExcel(string mainDriverId , string serverPath) {

            /*
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));


            dataTable.Rows.Add(1, "AAA");
            dataTable.Rows.Add(2, "BBB");
            dataTable.Rows.Add(3, "CCC");
            dataTable.Rows.Add(4, "DDD");
            dataTable.Rows.Add(5, "EEE");
            */
            //##################################################################### PACKING DATA.
            //string sql = "SELECT * FROM FT_driver_main WHERE id="+ mainDriverId + " ;";

            //mainDriverId = "33"; 
            string sql = @" SELECT
                                ROW_NUMBER() OVER(ORDER BY TB2.id ASC) AS 'ROW',
	                                TB2.create_datetime AS 'MACHINE_TIME'
                                ,	TB2.data_datetime AS 'SERVER_TIME'
                                , TB2.test_id AS 'DRIVER_NO'
                                , TB2.data_watt AS 'WATT'
                                , TB2.data_volt AS 'VOLT'
                                , TB2.data_pf AS 'PF'
                                , TB2.data_Amp AS 'Amp'
                                , TB2.data_THDi AS 'THDi'
                                , TB2.data_THDv AS 'THDv'
                                , TB2.data_PowerOutLed AS 'PowerOutLed'
                                , TB2.data_VLED AS 'VLED'
                                , TB2.data_ILED AS 'ILED'
                                , TB2.data_Efficiency AS 'Efficiency'
                                , TB2.action_result AS 'Result'
                                -- , TB2.*

                            FROM
	                            FT_driver_main TB1
                            INNER JOIN FT_driver_main_detail TB2 ON (TB1.id = TB2.FT_driver_main_id ) 
                            WHERE
	                            TB1.id = "+ mainDriverId + @"
                            ORDER BY ROW"; 

            DataTable dataTable = classDataBase.getDataTable(sql);

            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook worKbooK;
            Microsoft.Office.Interop.Excel.Worksheet worKsheeT = null;
            Microsoft.Office.Interop.Excel.Range celLrangE;

            string fileName = "ReportIQA_Excel_" + mainDriverId + ".xlsx" ; 
            string fileNamePath = serverPath + @"\"+ fileName; 
            try {

                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                worKbooK = excel.Workbooks.Add(Type.Missing);


                worKsheeT = (Microsoft.Office.Interop.Excel.Worksheet)worKbooK.ActiveSheet;
                worKsheeT.Name = "FT Driver";

                //worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[1, 2]].Merge();
                //worKsheeT.Cells[1, 1] = "Student Report Card";
                //worKsheeT.Cells.Font.Size = 15;

                int headerPosition = 5; 

                int rowcount = 4 ;


                foreach (DataRow datarow in dataTable.Rows) {
                    rowcount += 1;
                    for (int i = 1; i <= dataTable.Columns.Count; i++) {

                        if (rowcount == headerPosition) {                            
                            worKsheeT.Cells[4, i] = dataTable.Columns[i - 1].ColumnName;
                            worKsheeT.Cells.Font.Color = System.Drawing.Color.Black;                           
                        }

                        worKsheeT.Cells[rowcount, i] = datarow[i - 1].ToString();
                        if (rowcount > headerPosition) {
                            if (i == dataTable.Columns.Count) {
                                if (rowcount % 2 == 0) {
                                    celLrangE = worKsheeT.Range[worKsheeT.Cells[rowcount, 1], worKsheeT.Cells[rowcount, dataTable.Columns.Count]];
                                }
                            }
                        }
                    }
                }

                celLrangE = worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[rowcount, dataTable.Columns.Count]];
                celLrangE.EntireColumn.AutoFit();
                Microsoft.Office.Interop.Excel.Borders border = celLrangE.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;

                celLrangE = worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[2, dataTable.Columns.Count]];

                worKbooK.SaveAs(fileNamePath); ;
                worKbooK.Close();
                excel.Quit();



            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
                fileName = ex.ToString() ; 
            }
            finally {
                worKsheeT = null;
                celLrangE = null;
                worKbooK = null;
            }
            /*
                id
                po_no
                code_no
                date_create
                driver_count
             */


            //##################################################################### END PACKING DATA.
            /*

            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook worKbooK;
            Microsoft.Office.Interop.Excel.Worksheet worKsheeT = null;
            Microsoft.Office.Interop.Excel.Range celLrangE;
            try {
                
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                worKbooK = excel.Workbooks.Add(Type.Missing);


                worKsheeT = (Microsoft.Office.Interop.Excel.Worksheet)worKbooK.ActiveSheet;
                worKsheeT.Name = "REPORT EXCEL";

                worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[1, 2]].Merge();
                worKsheeT.Cells[1, 1] = "Student Report Card";
                worKsheeT.Cells.Font.Size = 15;


                int rowcount = 2;


                foreach (DataRow datarow in dataTable.Rows) {
                    rowcount += 1;
                    for (int i = 1; i <= dataTable.Columns.Count; i++) {

                        
                        if (rowcount == 3) {
                            worKsheeT.Cells[2, i] = dataTable.Columns[i - 1].ColumnName;
                            worKsheeT.Cells.Font.Color = System.Drawing.Color.Black;

                        }
                        

                        worKsheeT.Cells[rowcount, i] = datarow[i - 1].ToString();

                        if (rowcount > 3) {
                            if (i == dataTable.Columns.Count) {
                                if (rowcount % 2 == 0) {
                                    celLrangE = worKsheeT.Range[worKsheeT.Cells[rowcount, 1], worKsheeT.Cells[rowcount, dataTable.Columns.Count]];
                                }

                            }
                        }

                    }

                }

                celLrangE = worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[rowcount, dataTable.Columns.Count]];
                celLrangE.EntireColumn.AutoFit();
                Microsoft.Office.Interop.Excel.Borders border = celLrangE.Borders;
                border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
                border.Weight = 2d;

                celLrangE = worKsheeT.Range[worKsheeT.Cells[1, 1], worKsheeT.Cells[2, dataTable.Columns.Count]];

                worKbooK.SaveAs(@"D:\Users\pongsathorn.ro\Desktop\Shared\xxx.xls"); ;
                worKbooK.Close();
                excel.Quit();



            }
            catch (Exception ex) {
                Debug.WriteLine(ex); 
            }
            finally {
                worKsheeT = null;
                celLrangE = null;
                worKbooK = null;
            }
            */
            return fileName; 
        }




        public M_ReportIQA loadDataList() {


            M_ReportIQA mReportIQA = new M_ReportIQA();

            mReportIQA.mainDataDriver = new List<M_MainDataDriver>();

            string sql = @" SELECT * 
                                , CONVERT (VARCHAR (10),date_create,105) AS dateCreateFormat
                                , (SELECT COUNT(id) FROM FT_driver_main_detail WHERE FT_driver_main_id = FT_driver_main.id AND action_result = 1) AS COUNT_OK
                                , (SELECT COUNT(id) FROM FT_driver_main_detail WHERE FT_driver_main_id = FT_driver_main.id AND action_result = 0) AS COUNT_NG
                            FROM FT_driver_main  
                            WHERE 1 = 1 ORDER BY id DESC ";
            DataTable dataTable = classDataBase.getDataTable(sql);

            foreach (DataRow dataRow in dataTable.Rows) {

                M_MainDataDriver mMainDataDriver = new M_MainDataDriver();
                mMainDataDriver.id = dataRow["id"].ToString();
                mMainDataDriver.poNo = dataRow["po_no"].ToString();
                mMainDataDriver.codeNo = dataRow["code_no"].ToString();
                mMainDataDriver.dateCreate = dataRow["dateCreateFormat"].ToString();
                mMainDataDriver.driverCount = dataRow["driver_count"].ToString();

                mMainDataDriver.driverOK = dataRow["COUNT_OK"].ToString(); ;
                mMainDataDriver.driverNG = dataRow["COUNT_NG"].ToString();  ; 

                mReportIQA.mainDataDriver.Add(mMainDataDriver);

            }

            return mReportIQA; 
        }

        public M_MainDataDriver getReportDataDetaiDriver(string mainDriverId) {

            M_MainDataDriver mMainDataDriver = new M_MainDataDriver();

            string sql = @" SELECT * 
                            , (SELECT COUNT(id) FROM FT_driver_main_detail WHERE FT_driver_main_id = FT_driver_main.id AND action_result = 1) AS COUNT_OK
                            , (SELECT COUNT(id) FROM FT_driver_main_detail WHERE FT_driver_main_id = FT_driver_main.id AND action_result = 0) AS COUNT_NG                            
                            FROM FT_driver_main WHERE id=" + mainDriverId;
            DataRow dataRow = classDataBase.getDataRow(sql);
            mMainDataDriver.id = dataRow["id"].ToString() ;
            mMainDataDriver.poNo = dataRow["po_no"].ToString();
            mMainDataDriver.codeNo = dataRow["code_no"].ToString();
            mMainDataDriver.dateCreate = dataRow["date_create"].ToString();
            mMainDataDriver.driverCount = dataRow["driver_count"].ToString();
            mMainDataDriver.driverOK = dataRow["COUNT_OK"].ToString();
            mMainDataDriver.driverNG = dataRow["COUNT_NG"].ToString();


            mMainDataDriver.mMainDataDetailDriver = new List<M_MainDataDetailDriver>();
            string sqlDetail = @" SELECT * 
                            FROM FT_driver_main_detail
                            WHERE FT_driver_main_id = "+ mainDriverId;
            DataTable dataTableDetail = classDataBase.getDataTable(sqlDetail);
            int driverCount = 1; 
            foreach (DataRow dataRowDetail in dataTableDetail.Rows) {

                M_MainDataDetailDriver mMainDataDetailDriver = new M_MainDataDetailDriver();
                mMainDataDetailDriver.driver_count = driverCount.ToString(); 
                mMainDataDetailDriver.id = dataRowDetail["id"].ToString();
                mMainDataDetailDriver.FT_driver_main_id = dataRowDetail["FT_driver_main_id"].ToString();
                mMainDataDetailDriver.create_datetime = dataRowDetail["create_datetime"].ToString();
                mMainDataDetailDriver.data_datetime = dataRowDetail["data_datetime"].ToString();
                mMainDataDetailDriver.test_id = "Driver No." + dataRowDetail["test_id"].ToString();
                mMainDataDetailDriver.data_watt = dataRowDetail["data_watt"].ToString();
                mMainDataDetailDriver.data_pf = dataRowDetail["data_pf"].ToString();
                mMainDataDetailDriver.data_THDi = dataRowDetail["data_THDi"].ToString();
                mMainDataDetailDriver.data_volt = dataRowDetail["data_volt"].ToString();
                mMainDataDetailDriver.data_Amp = dataRowDetail["data_Amp"].ToString();
                mMainDataDetailDriver.data_THDv = dataRowDetail["data_THDv"].ToString();
                mMainDataDetailDriver.data_PowerOutLed = dataRowDetail["data_PowerOutLed"].ToString();
                mMainDataDetailDriver.data_VLED = dataRowDetail["data_VLED"].ToString();
                mMainDataDetailDriver.data_ILED = dataRowDetail["data_ILED"].ToString();
                mMainDataDetailDriver.data_Efficiency = dataRowDetail["data_Efficiency"].ToString();
                mMainDataDetailDriver.action_result = dataRowDetail["action_result"].ToString();

                mMainDataDriver.mMainDataDetailDriver.Add(mMainDataDetailDriver);
                driverCount++; 
            }
            return mMainDataDriver; 
        }



    }
}