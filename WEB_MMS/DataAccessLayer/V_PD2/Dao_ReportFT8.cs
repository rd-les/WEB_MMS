using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using Web_LED.App_Class;
using WEB_MMS.Models.V_PD2;

namespace WEB_MMS.DataAccessLayer.V_PD2 {
    public class Dao_ReportFT8 {


        private ClassDataBase classDataBase = new ClassDataBase();

        private string getCurrentWorkstation() {
            string sqlMainId = "SELECT TOP 1 id FROM FT8_main_data ORDER BY id DESC ";
            DataRow dataRowId = classDataBase.getDataRow(sqlMainId);

            string ft8MainDataId = dataRowId["id"].ToString();
            return ft8MainDataId; 
        }
            
        public Object getStartUpDataWorkStation() {

            List<Dictionary<string, string>> dataReturns = new List<Dictionary<string, string>>();
            Dictionary<string, string> dataList = new Dictionary<string, string>();
            string sqlWhere = " ( SELECT id FROM FT8_main_data WHERE work_station_no=TB1.work_station_no  )  ";
            string sqlMainData = @"SELECT TOP 1 
                                    *
                                     , (SELECT SUM(TOTAL_LED_GOOD) AS TOTAL_LED_GOOD
                                        FROM(   (SELECT COUNT(id) AS TOTAL_LED_GOOD FROM FT8_main_data_detail
                                                    WHERE 1 = 1 AND FT8_main_data_id IN ("+ sqlWhere + @") AND led_count_no = 1 AND ActionResult = 1
                                                    GROUP BY led_no
                                                 )
			                                 ) AS TOTAL_LED_GOOD
		                                )AS TOTAL_REAL_LED_GOOD
                                    , (SELECT SUM(TOTAL_LED_BAD) AS TOTAL_LED_BAD
                                        FROM(   (SELECT COUNT(id) AS TOTAL_LED_BAD FROM FT8_main_data_detail
                                                    WHERE 1 = 1 AND FT8_main_data_id IN (" + sqlWhere + @") AND led_count_no = 1 AND ActionResult = 0
                                                    GROUP BY led_no
                                                )
		                                ) AS TOTAL_LED_BAD
	                                )AS TOTAL_REAL_LED_BAD
                                    , (SELECT   ROUND(AVG(LowWatt), 2) FROM  FT8_main_data_detail TB_MAIN WHERE  TB_MAIN.FT8_main_data_id IN (" + sqlWhere + @") ) AS LOW_WATT
                                    , (SELECT   ROUND(AVG(HighWatt), 2) FROM  FT8_main_data_detail TB_MAIN WHERE  TB_MAIN.FT8_main_data_id IN (" + sqlWhere + @") ) AS HIGH_WATT
                                    ,(  SELECT SUM(work_station_count_real) AS COUNT_ALL_CURRENT_LED
                                        FROM FT8_main_data
                                        WHERE work_station_no = TB1.work_station_no
                                    ) AS COUNT_ALL_CURRENT_LED
                                    FROM FT8_main_data  TB1
                                    ORDER BY id DESC ";

            DataRow dataRow = classDataBase.getDataRow(sqlMainData);

            dataList.Add("WORK_STATION_NO", dataRow["work_station_no"].ToString());
            dataList.Add("CODE_NO", dataRow["code_no"].ToString());
            dataList.Add("WORK_STATION_COUNT", dataRow["work_station_count"].ToString());
            dataList.Add("COUNT_ALL_CURRENT_LED", dataRow["COUNT_ALL_CURRENT_LED"].ToString()); 
           // dataList.Add("TOTAL_TARGET", dataRow["TOTAL_TARGET"].ToString());
            dataList.Add("TOTAL_REAL_LED_GOOD", (dataRow["TOTAL_REAL_LED_GOOD"].ToString().Equals("") ? "0": dataRow["TOTAL_REAL_LED_GOOD"].ToString()) );
            dataList.Add("TOTAL_REAL_LED_BAD", (dataRow["TOTAL_REAL_LED_BAD"].ToString().Equals("")? "0" : dataRow["TOTAL_REAL_LED_BAD"].ToString()));
            dataList.Add("LOW_WATT", dataRow["LOW_WATT"].ToString());
            dataList.Add("HIGH_WATT", dataRow["HIGH_WATT"].ToString());

            //################################################################################# TIME DIFF.
            Dictionary<string, string> dataParams = new Dictionary<string, string>();
            dataParams.Add("@workstation_no", dataRow["work_station_no"].ToString());


            //DataTable dataTableProcedure = classDataBase.getDataTableProcedure("sp_getTimeDiffFT8", dataParams);
            DataRow dataRowProcedure = classDataBase.getDataRowProcedure("sp_getTimeDiffFT8",dataParams);

            Debug.WriteLine(((int)dataRowProcedure["DATEDIFF_HOUR"]).ToString("00"));
            Debug.WriteLine(((int)dataRowProcedure["DATEDIFF_MINUTE"]).ToString("00"));
            Debug.WriteLine(((int)dataRowProcedure["DATEDIFF_SECOND"]).ToString("00"));

            dataList.Add("WORKSTATION_TOTAL_TIME" , ((int)dataRowProcedure["DATEDIFF_HOUR"]).ToString("00.##") + ":"+ ((int)dataRowProcedure["DATEDIFF_MINUTE"]).ToString("00.##")+ ":" + ((int)dataRowProcedure["DATEDIFF_SECOND"]).ToString("00.##"));

            //################################################################################# GET DATA FROM WEBSERVICE.
            var client = new WebClient();
            var content = client.DownloadString(ConfigClass.URL_WEB_LES_SERVICE_WO_DETAIL+ "?workStationId="+ dataRow["work_station_no"].ToString());
            content = content.Replace(@"\", "");
            var JSONObj = new JavaScriptSerializer().Deserialize<Dictionary<string, string>>(content);


            dataList.Add("WORKSTATION_ALL_TOTAL",Math.Round( Double.Parse(JSONObj["workStationItem"].ToString()) ).ToString());

            dataReturns.Add(dataList); 
            return dataReturns; 
        }

        public Object getLastDataRealtimeGraph() {

            //###################################################################   GET CURRENT WorkStation.
            string ft8MainDataId = getCurrentWorkstation();
            string sql = @" SELECT TOP 100 
                                id
                                , led_no
                                , led_count
                                , data_watt
                                , data_PF
                                , data_THDi
                                , ActionResult
                                , CONVERT (VARCHAR (10),date_create,105) AS createDate
                                , CONVERT (VARCHAR (10),date_create,108) AS createTime
                            FROM [dbo].[FT8_main_data_detail]
                            WHERE FT8_main_data_id = " + ft8MainDataId + @" 
                            ORDER BY id DESC
                            ;";
            DataTable dataTable = classDataBase.getDataTable(sql);
            //List<string> dataReturns = new List<string>();
            List<Dictionary<string, string>> dataReturns = new List<Dictionary<string, string>>();

            int arrayCount = 100 - dataTable.Rows.Count;

            for (int index = 0; index < arrayCount; index++) {
                Dictionary<string, string> dataList = new Dictionary<string, string>();
                dataList.Add("id", "");
                dataList.Add("led_no" , "-"); 
                dataList.Add("led_count", "0");
                dataList.Add("data_watt", "0");
                dataList.Add("data_PF", "0");
                dataList.Add("data_THDi", "0");
                dataList.Add("ActionResult", "0");
                dataList.Add("datetime_create", "");

                dataReturns.Add(dataList);
            }

            foreach (DataRow row in dataTable.Rows) {
                Dictionary<string, string> dataList = new Dictionary<string, string>();
                dataList.Add("id", row["id"].ToString());
                dataList.Add("led_no", row["led_no"].ToString());
                dataList.Add("led_count" , row["led_count"].ToString() ); 
                dataList.Add("data_watt", row["data_watt"].ToString());
                dataList.Add("data_PF", row["data_PF"].ToString());
                dataList.Add("data_THDi", row["data_THDi"].ToString());
                dataList.Add("ActionResult", row["ActionResult"].ToString());
                dataList.Add("datetime_create", row["createDate"].ToString()+" "+ row["createTime"].ToString());

                dataReturns.Add(dataList); 
            }

            return dataReturns; 
        }


        public void getReportExcelDetailFT8(string mainDataId , string codeNo , string workStationNo) {


            string sqlMainData = @"SELECT 
                                    *
                                    , (SELECT	COUNT (id) FROM	FT8_main_data_detail TB1 WHERE	TB1.FT8_main_data_id = TB_MAIN.id AND TB1.ActionResult = 1 ) AS LED_GOOD
                                    , (SELECT	COUNT (id) FROM	FT8_main_data_detail TB1 WHERE	TB1.FT8_main_data_id = TB_MAIN.id AND TB1.ActionResult = 0 ) AS LED_BAD
                                    , (SELECT	ROUND(AVG(LowWatt),2) FROM	FT8_main_data_detail TB1 WHERE	TB1.FT8_main_data_id = TB_MAIN.id ) AS LOW_WATT
                                    , (SELECT	ROUND(AVG(HighWatt) ,2  ) FROM	FT8_main_data_detail TB1 WHERE	TB1.FT8_main_data_id = TB_MAIN.id ) AS HIGH_WATT
                                FROM FT8_main_data AS TB_MAIN
                                WHERE  id = " + mainDataId;
            DataTable dataTable = classDataBase.getDataTable(sqlMainData);


            string sqlDetail = @"SELECT 
                                    ROW_NUMBER() OVER (Order by id) AS RowNumber
                                    , led_no
                                    , convert(varchar, date_create, 113)	AS date_create
                                    , convert(varchar, data_watt) AS data_watt
                                    , convert(varchar, data_PF) AS data_pf
                                    , convert(int, led_count) AS led_count
                                    , convert(int, led_count_no) AS led_count_no
                                    , ActionResult 
                                FROM FT8_main_data_detail 
                                WHERE 1=1 AND FT8_main_data_id =" + mainDataId + "  ORDER BY led_count , led_no ASC , led_count_no ASC ";
            DataTable dataTableDetail = classDataBase.getDataTable(sqlDetail);


            LocalReport report = new LocalReport();
            report.ReportPath = HostingEnvironment.MapPath("~/Report/PD2/Report_FT8.rdlc");

            ReportDataSource datasourceMain =  new ReportDataSource("DataSet_FT8", dataTable);
            ReportDataSource datasourceMainDetail = new ReportDataSource("DataTable_FT8_Detail", dataTableDetail);


            report.DataSources.Clear();
            report.DataSources.Add(datasourceMain);
            report.DataSources.Add(datasourceMainDetail); 

            string extension;
            string encoding;
            string mimeType;
            string[] streams;
            Warning[] warnings;
            byte[] reportBytes = report.Render(
                 "EXCEL", null, out mimeType, out encoding,
                  out extension,
                 out streams, out warnings);

            string pathExcel = HostingEnvironment.MapPath("/Report/PD2/" + mainDataId + "/" + string.Format("{0}.{1}", codeNo+"-"+workStationNo , "xls"));
            string pathDirectory = HostingEnvironment.MapPath("/Report/PD2/" + mainDataId);
            Directory.CreateDirectory(pathDirectory);

            using (FileStream fs = File.Create(pathExcel)) {
                fs.Write(reportBytes, 0, reportBytes.Length);
                //formModel.reportPath = pathPdf + "/ReportWorkStation.pdf";
            }

        }

        public M_MainDataFT8 getReportDataDetailFT8(string mainDataId) {

            M_MainDataFT8 mMainDataFT8 = new M_MainDataFT8();
            mMainDataFT8.mMainDataDetailFT8 = new List<M_MainDataDetailFT8>();

            string sqlMainData = @"SELECT 
                                    *
                                    , (SELECT	COUNT (id) FROM	FT8_main_data_detail TB1 WHERE	TB1.FT8_main_data_id = TB_MAIN.id AND TB1.ActionResult = 1 ) AS LED_GOOD
                                    , (SELECT	COUNT (id) FROM	FT8_main_data_detail TB1 WHERE	TB1.FT8_main_data_id = TB_MAIN.id AND TB1.ActionResult = 0 ) AS LED_BAD
                                    , (SELECT	ROUND(AVG(LowWatt),2) FROM	FT8_main_data_detail TB1 WHERE	TB1.FT8_main_data_id = TB_MAIN.id ) AS LOW_WATT
                                    , (SELECT	ROUND(AVG(HighWatt) ,2  ) FROM	FT8_main_data_detail TB1 WHERE	TB1.FT8_main_data_id = TB_MAIN.id ) AS HIGH_WATT
                                FROM FT8_main_data AS TB_MAIN
                                WHERE  id = " + mainDataId;
            DataRow dataRowMain = classDataBase.getDataRow(sqlMainData);
            mMainDataFT8.codeNo = dataRowMain["code_no"].ToString();
            mMainDataFT8.workStationNo = dataRowMain["work_station_no"].ToString();
            mMainDataFT8.dateCreate = dataRowMain["date_create"].ToString();
            mMainDataFT8.ledTotal= dataRowMain["work_station_count"].ToString();
            mMainDataFT8.ledGood = dataRowMain["LED_GOOD"].ToString();
            mMainDataFT8.ledBad = dataRowMain["LED_BAD"].ToString();
            mMainDataFT8.ledLowWatt = dataRowMain["LOW_WATT"].ToString();
            mMainDataFT8.ledHighWatt = dataRowMain["HIGH_WATT"].ToString(); 

            string sql = "SELECT * FROM FT8_main_data_detail WHERE 1=1 AND FT8_main_data_id =" + mainDataId + " ORDER BY  led_count , led_no ASC , led_count_no ASC  ";
            DataTable dataTable = classDataBase.getDataTable(sql); 

            foreach (DataRow dataRow in dataTable.Rows) {

                M_MainDataDetailFT8 mMainDataDetailFT8 = new M_MainDataDetailFT8();

                mMainDataDetailFT8.id = dataRow["id"].ToString();
                mMainDataDetailFT8.led_no = dataRow["led_no"].ToString();
                mMainDataDetailFT8.data_watt = dataRow["data_watt"].ToString();
                mMainDataDetailFT8.data_PF = dataRow["data_PF"].ToString();
                mMainDataDetailFT8.data_THDi = dataRow["data_THDi"].ToString();
                mMainDataDetailFT8.data_volt = dataRow["data_volt"].ToString(); 
                mMainDataDetailFT8.data_mA = dataRow["data_mA"].ToString();
                mMainDataDetailFT8.data_THDv = dataRow["data_THDv"].ToString();
                mMainDataDetailFT8.power_out_watt = dataRow["power_out_watt"].ToString();
                mMainDataDetailFT8.VLED = dataRow["VLED"].ToString();
                mMainDataDetailFT8.ILED = dataRow["ILED"].ToString();
                mMainDataDetailFT8.Efficiency = dataRow["Efficiency"].ToString();
                mMainDataDetailFT8.ActionResult = dataRow["ActionResult"].ToString();
                mMainDataDetailFT8.LowWatt = dataRow["LowWatt"].ToString();
                mMainDataDetailFT8.HighWatt = dataRow["HighWatt"].ToString();
                mMainDataDetailFT8.MaxTHDi = dataRow["MaxTHDi"].ToString();
                mMainDataDetailFT8.data_datetime = dataRow["data_datetime"].ToString();
                mMainDataDetailFT8.date_create = dataRow["date_create"].ToString();
                mMainDataDetailFT8.led_count = dataRow["led_count"].ToString();
                mMainDataDetailFT8.led_count_no = dataRow["led_count_no"].ToString();

                mMainDataFT8.mMainDataDetailFT8.Add(mMainDataDetailFT8); 

            }

            return mMainDataFT8;
        }

        public string getMainDataMinMax() {
            

            return "";
        }

        public M_ReportFT8 loadDataList() {


            M_ReportFT8 mReportFT8 = new M_ReportFT8();

            mReportFT8.mainDataFT8 = new List<M_MainDataFT8>();


            //string sql = @"SELECT * ,  CONVERT(VARCHAR(10), date_create, 105) AS dateCreateFormat  FROM FT8_main_data WHERE 1=1 ORDER BY id DESC";
            string sql = @"SELECT *
                                , CONVERT(VARCHAR (10),date_create,105) AS dateCreateFormat
                                , ( SELECT SUM (TOTAL_LED) AS TOTAL_LED
                                    FROM(
				                          ( SELECT COUNT (id) AS TOTAL_LED FROM FT8_main_data_detail
					                        WHERE 1 = 1 AND FT8_main_data_id = TB1.id AND led_count_no = 1			
					                        GROUP BY led_no
				                          )
			                            ) AS TOTAL_REAL_LED
		                            )AS TOTAL_REAL_LED
                                    , (SELECT SUM (TOTAL_LED_GOOD) AS TOTAL_LED_GOOD
		                                FROM(
				                            (SELECT COUNT (id) AS TOTAL_LED_GOOD FROM FT8_main_data_detail
					                            WHERE 1 = 1 AND FT8_main_data_id = TB1.id AND led_count_no = 1 AND ActionResult = 1 	
					                            GROUP BY led_no
				                            )
			                            ) AS TOTAL_LED_GOOD
		                            )AS TOTAL_REAL_LED_GOOD
                                    , (SELECT SUM (TOTAL_LED_BAD) AS TOTAL_LED_BAD
		                                FROM (
				                            (SELECT COUNT (id) AS TOTAL_LED_BAD FROM FT8_main_data_detail
					                            WHERE 1 = 1 AND FT8_main_data_id = TB1.id AND led_count_no = 1 AND ActionResult = 0	
					                            GROUP BY led_no
				                            )
			                            ) AS TOTAL_LED_BAD
		                            )AS TOTAL_REAL_LED_BAD
                            FROM
	                            FT8_main_data TB1
                            WHERE
	                            1 = 1
                            ORDER BY
	                            id DESC"; 
            DataTable dataTable = classDataBase.getDataTable(sql);

            foreach (DataRow dataRow in dataTable.Rows) {

                M_MainDataFT8 mMainDataFT8 = new M_MainDataFT8();

                mMainDataFT8.FT8_MainDataId = dataRow["id"].ToString();
                mMainDataFT8.codeNo = dataRow["code_no"].ToString();
                mMainDataFT8.workStationNo = dataRow["work_station_no"].ToString();
                mMainDataFT8.dateCreate = dataRow["dateCreateFormat"].ToString();


                mMainDataFT8.ledTotal = dataRow["TOTAL_REAL_LED"].ToString() ;
                mMainDataFT8.ledGood = dataRow["TOTAL_REAL_LED_GOOD"].ToString();
                mMainDataFT8.ledBad = dataRow["TOTAL_REAL_LED_BAD"].ToString() ;


                //mMainDataFT8.dateCreate = dataRow["date_create"].ToString();

                mReportFT8.mainDataFT8.Add(mMainDataFT8);
            }
            return mReportFT8;
        }



    }
}