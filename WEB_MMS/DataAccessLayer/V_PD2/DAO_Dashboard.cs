using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using Web_LED.App_Class;
using WEB_MMS.Models.V_PD2;

namespace WEB_MMS.DataAccessLayer.V_PD2 {
    public class DAO_Dashboard {

        ClassDataBase classDatabase = new ClassDataBase(); 


        private string[] borderColor = { "#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#e00d81", "#0de0a0" };



        public M_Dashboard searchDataDashboard(string startDateCriteria, string endDateCriteria) {

            M_Dashboard mDashboard = new M_Dashboard();
            //############################################################################################################  COUNT ALL WORKSTATION.
            string sqlSumCurrent = @"SELECT work_station_no,  COUNT(TB1.work_station_no )  AS COUNT_WORKSTATION
                                    FROM FT8_main_data TB1
                                    WHERE TB1.date_create BETWEEN '" + startDateCriteria + @"' AND '" + endDateCriteria + @"'
                                    GROUP BY TB1.work_station_no ";

            DataTable dataTable = classDatabase.getDataTable(sqlSumCurrent);

            string sqlSumAll = @"SELECT 
                                    SUM(ABC.COUNT_ALL_OK) AS COUNT_ALL_OK
                                    , SUM(ABC.COUNT_ALL_NG) AS COUNT_ALL_NG
                                    , SUM(ABC.COUNT_ALL_OK) +SUM(ABC.COUNT_ALL_NG) AS COUNT_TOTAL
                                FROM (  
                                    SELECT 
                                        TB1.id
                                        , COUNT(TB2.FT8_main_data_id) AS COUNT_MAIN_ID
                                        , (SELECT COUNT(id) FROM FT8_main_data_detail TB3 WHERE  TB3.ActionResult = 1 AND TB3.FT8_main_data_id = TB1.id) AS COUNT_ALL_OK 
                                        , (SELECT COUNT(id) FROM FT8_main_data_detail TB3 WHERE  TB3.ActionResult = 0 AND TB3.FT8_main_data_id = TB1.id) AS COUNT_ALL_NG
                                    FROM
	                                    FT8_main_data TB1
                                    INNER JOIN FT8_main_data_detail TB2 ON (TB1.id = TB2.FT8_main_data_id) 
                                    WHERE
	                                    TB1.date_create BETWEEN '" + startDateCriteria + @"' AND '" + endDateCriteria + @"'
                                    GROUP BY TB1.id

                                ) AS ABC";
            DataRow dataRowSumAll = classDatabase.getDataRow(sqlSumAll);

            mDashboard.led_t8_total = dataRowSumAll["COUNT_TOTAL"].ToString();
            mDashboard.led_t8_ok = dataRowSumAll["COUNT_ALL_OK"].ToString();
            mDashboard.led_t8_ng = dataRowSumAll["COUNT_ALL_NG"].ToString();
            mDashboard.workstation_total = dataTable.Rows.Count.ToString();

            //mDashboard.barChart_labels_month = SystemClass.getLastMonths(3 , endDateCriteria);
            mDashboard.barChart_labels_month = SystemClass.getLastMonths(startDateCriteria , endDateCriteria); 
            //##########################################################    GET MONTH OK/NG.
            //List<string> dataMonths = SystemClass.getLastMonths(setLastMonth); // GET MONTH STRING.
            List<string> dataMonths = SystemClass.getBetweenMonths(startDateCriteria, endDateCriteria); // GET MONTH STRING.
            //List<string> dataNumberMonths = SystemClass.getLastMonths(setLastMonth); // GET MONTH NUMBER.
            StringBuilder sqlAllOkNg = new StringBuilder();
            StringBuilder sqlWidgetAll = new StringBuilder();
            StringBuilder sqlLineChart = new StringBuilder();

            int countMonth = 0;
            foreach (string month in dataMonths) {

                string[] splitYearMonth = month.Split('-');
                int yearInt = int.Parse(splitYearMonth[0]);
                int monthInt = int.Parse(splitYearMonth[1]);

                string startSqlDate = month + "-01";
                string endSqlDate = month + "-" + DateTime.DaysInMonth(yearInt, monthInt);

                if (countMonth == 0 ) { //################################# FIND START DATE.
                    startSqlDate = startDateCriteria; 
                }
                if (countMonth == (dataMonths.Count-1) ) {//################################# FIND END DATE.
                    endSqlDate = endDateCriteria;
                }
                if (countMonth > 0) {
                    sqlAllOkNg.Append(" UNION ALL ");
                    sqlWidgetAll.Append(" UNION ALL ");
                    sqlLineChart.Append(" UNION ALL ");
                }
                sqlAllOkNg.Append(this.getSqlCountOkNg(month, startSqlDate, endSqlDate));
                sqlWidgetAll.Append(this.getSqlCountWidgetAll(month, startSqlDate, endSqlDate));
                sqlLineChart.Append(this.getSqlLineChartCountFT8(month, startSqlDate, endSqlDate));

                countMonth++;

                Debug.WriteLine("----->" + startSqlDate + " - " + endSqlDate);
            }

            //List<string> returnMonths = new List<string>(Enumerable.Range(1, monthRange).Select(i => DateTime.Now.AddMonths(i - monthRange)).Select(date => date.ToString("yyyy-MM-dd")));
            Debug.WriteLine(sqlAllOkNg.ToString());
            DataTable dataTableAllOkNg = classDatabase.getDataTable(sqlAllOkNg.ToString());


            mDashboard.barChart_datas_ok = new List<int>();
            mDashboard.barChart_datas_ng = new List<int>();
            foreach (DataRow dataRowAllOkNg in dataTableAllOkNg.Rows) {

                int countAllOk = int.Parse(SystemClass.returnValueZero(dataRowAllOkNg["COUNT_ALL_OK"].ToString()));
                int countAllNg = int.Parse(SystemClass.returnValueZero(dataRowAllOkNg["COUNT_ALL_NG"].ToString()));
                mDashboard.barChart_datas_ok.Add(countAllOk);
                mDashboard.barChart_datas_ng.Add(countAllNg);
            }


            //###############################################################################   WIDGET.
            Debug.WriteLine(sqlWidgetAll.ToString());
            DataTable dataTableWidgetAll = classDatabase.getDataTable(sqlWidgetAll.ToString());
            mDashboard.widgetAllFT8 = new List<int>();
            mDashboard.widgetOkFT8 = new List<int>();
            mDashboard.widgetNgFT8 = new List<int>();
            mDashboard.widgetWoFT8 = new List<int>();

            foreach (DataRow dataRowWidgetAll in dataTableWidgetAll.Rows) {
                int countAll = int.Parse(SystemClass.returnValueZero(dataRowWidgetAll["COUNT_ALL"].ToString()));
                int countAllOk = int.Parse(SystemClass.returnValueZero(dataRowWidgetAll["COUNT_ALL_OK"].ToString()));
                int countAllNg = int.Parse(SystemClass.returnValueZero(dataRowWidgetAll["COUNT_ALL_NG"].ToString()));
                int countAllWo = int.Parse(SystemClass.returnValueZero(dataRowWidgetAll["COUNT_ALL_WO"].ToString()));

                mDashboard.widgetAllFT8.Add(countAll);
                mDashboard.widgetOkFT8.Add(countAllOk);
                mDashboard.widgetNgFT8.Add(countAllNg);
                mDashboard.widgetWoFT8.Add(countAllWo);
            }




            return mDashboard;
        }


        public string getSqlCountWidgetAll(string monthhName, string startDateCurrent, string endDateCurrent) {

            string sql = @"SELECT 
		                             '" + monthhName + @"' AS MONTH_NAME
		                            , SUM (ABC.COUNT_ALL) AS COUNT_ALL
                                    , SUM (ABC.COUNT_ALL_OK) AS COUNT_ALL_OK
                                    , SUM (ABC.COUNT_ALL_NG) AS COUNT_ALL_NG
                                    , SUM (ABC.COUNT_ALL_WO) AS COUNT_ALL_WO
                            FROM (  
		                            SELECT
			                            TB1.work_station_count_real AS COUNT_ALL
                                        ,TB1.work_station_count_real_ok AS COUNT_ALL_OK
                                        ,TB1.work_station_count_real_ng AS COUNT_ALL_NG
                                        , 1 AS COUNT_ALL_WO
		                            FROM
			                            FT8_main_data TB1		
		                            WHERE
			                            TB1.date_create BETWEEN '" + startDateCurrent + @" 00:00:00' AND '" + endDateCurrent + @" 23:59:59'
                            ) AS ABC
                            ";
            return sql;
        }

        private string getSqlLineChartCountFT8(string monthhName, string startDateCurrent, string endDateCurrent) {

            string sql = @"
                            SELECT
                            '"+monthhName+ @"'  AS 'MONTH'
                            , sum(case when datepart(hour, TB2.data_datetime) >= 8 AND datepart(hour, TB2.data_datetime) < 9  then 1 else 0 end) as '08:00-09:00'
                            ,sum(case when datepart(hour, TB2.data_datetime) >= 9 AND datepart(hour, TB2.data_datetime) < 10  then 1 else 0 end) as '09:00-10:00'
                            ,sum(case when datepart(hour, TB2.data_datetime) >= 10 AND datepart(hour, TB2.data_datetime) < 11  then 1 else 0 end) as '10:00-11:00'
                            ,sum(case when datepart(hour, TB2.data_datetime) >= 11 AND datepart(hour, TB2.data_datetime) < 12  then 1 else 0 end) as '11:00-12:00'
                            -- ,sum(case when datepart(hour, TB2.data_datetime) >= 12 AND datepart(hour, TB2.data_datetime) < 13  then 1 else 0 end) as '12:00-13:00'
                            ,sum(case when datepart(hour, TB2.data_datetime) >= 13 AND datepart(hour, TB2.data_datetime) < 14  then 1 else 0 end) as '13:00-14:00'
                            ,sum(case when datepart(hour, TB2.data_datetime) >= 14 AND datepart(hour, TB2.data_datetime) < 15  then 1 else 0 end) as '14:00-15:00'
                            ,sum(case when datepart(hour, TB2.data_datetime) >= 15 AND datepart(hour, TB2.data_datetime) < 16  then 1 else 0 end) as '15:00-16:00'
                            ,sum(case when datepart(hour, TB2.data_datetime) >= 16 AND datepart(hour, TB2.data_datetime) < 17  then 1 else 0 end) as '16:00-17:00'
                            ,sum(case when datepart(hour, TB2.data_datetime) >= 17 AND datepart(hour, TB2.data_datetime) < 18  then 1 else 0 end) as '17:00-18:00'
                            ,sum(case when datepart(hour, TB2.data_datetime) >= 18 AND datepart(hour, TB2.data_datetime) < 19  then 1 else 0 end) as '18:00-19:00'
                            ,sum(case when datepart(hour, TB2.data_datetime) >= 19 AND datepart(hour, TB2.data_datetime) < 20  then 1 else 0 end) as '19:00-20:00'
                            ,sum(case when datepart(hour, TB2.data_datetime) >= 20 AND datepart(hour, TB2.data_datetime) < 21  then 1 else 0 end) as '20:00-21:00'
                            ,sum(case when datepart(hour, TB2.data_datetime) >= 21 AND datepart(hour, TB2.data_datetime) < 22  then 1 else 0 end) as '21:00-22:00'

                            FROM
	                            FT8_main_data TB1
                            INNER JOIN FT8_main_data_detail TB2 ON (TB1.id = TB2.FT8_main_data_id)


                            WHERE
                            TB1.date_create BETWEEN '" + startDateCurrent + @"' AND '" + endDateCurrent + @"'"; 

            return sql ; 
        }


        private string getSqlCountOkNg(string monthhName ,string startDateCurrent, string endDateCurrent) {

            string sql = @"SELECT 
		                             '"+ monthhName + @"' AS MONTH_NAME
		                            , SUM(ABC.COUNT_ALL_OK) AS COUNT_ALL_OK
		                            , SUM(ABC.COUNT_ALL_NG) AS COUNT_ALL_NG
                            FROM (  
		                            SELECT 
				                            TB1.id
				                            , COUNT(TB2.FT8_main_data_id) AS COUNT_MAIN_ID
				                            , (SELECT COUNT(id) FROM FT8_main_data_detail TB3 WHERE  TB3.ActionResult = 1 AND TB3.FT8_main_data_id = TB1.id) AS COUNT_ALL_OK 
				                            , (SELECT COUNT(id) FROM FT8_main_data_detail TB3 WHERE  TB3.ActionResult = 0 AND TB3.FT8_main_data_id = TB1.id) AS COUNT_ALL_NG
		                            FROM
			                            FT8_main_data TB1
		                            INNER JOIN FT8_main_data_detail TB2 ON (TB1.id = TB2.FT8_main_data_id) 
		                            WHERE
			                            TB1.date_create BETWEEN '"+ startDateCurrent + @" 00:00:00' AND '"+ endDateCurrent + @" 23:59:59'
		                            GROUP BY TB1.id

                            ) AS ABC
                            "; 
            return sql; 
        }


        public M_Dashboard loadStartUpData(string startDateCurrent , string endDateCurrent) {
            M_Dashboard mDashboard = new M_Dashboard();
            int setLastMonth = 6; 

            //############################################################################################################  COUNT ALL WORKSTATION.
            string sqlSumCurrent = @"SELECT work_station_no,  COUNT(TB1.work_station_no )  AS COUNT_WORKSTATION
                                    FROM FT8_main_data TB1
                                    WHERE TB1.date_create BETWEEN '"+ startDateCurrent + @"' AND '" + endDateCurrent + @"'
                                    GROUP BY TB1.work_station_no ";

            DataTable dataTable = classDatabase.getDataTable(sqlSumCurrent);

            string sqlSumAll = @"SELECT 
                                    SUM(ABC.COUNT_ALL_OK) AS COUNT_ALL_OK
                                    , SUM(ABC.COUNT_ALL_NG) AS COUNT_ALL_NG
                                    , SUM(ABC.COUNT_ALL_OK) +SUM(ABC.COUNT_ALL_NG) AS COUNT_TOTAL
                                FROM (  
                                    SELECT 
                                        TB1.id
                                        , COUNT(TB2.FT8_main_data_id) AS COUNT_MAIN_ID
                                        , (SELECT COUNT(id) FROM FT8_main_data_detail TB3 WHERE  TB3.ActionResult = 1 AND TB3.FT8_main_data_id = TB1.id) AS COUNT_ALL_OK 
                                        -- , (SELECT COUNT(id) FROM FT8_main_data_detail TB3 WHERE  TB3.ActionResult = 0 AND TB3.FT8_main_data_id = TB1.id) AS COUNT_ALL_NG
                                        ,(
                                            SELECT SUM (TOTAL_LED_BAD) AS TOTAL_LED_BAD
				                            FROM (
						                            (
						                                SELECT COUNT(id) AS TOTAL_LED_BAD 
						                                FROM FT8_main_data_detail TB3 
						                                WHERE  TB3.ActionResult = 0 AND TB3.FT8_main_data_id = TB1.id AND led_count_no = 1
						                                GROUP BY TB3.led_no
					                                ) 
                                                ) AS TOTAL_LED_BAD
                                            ) AS COUNT_ALL_NG
                                    FROM
	                                    FT8_main_data TB1
                                    INNER JOIN FT8_main_data_detail TB2 ON (TB1.id = TB2.FT8_main_data_id) 
                                    WHERE
	                                    TB1.date_create BETWEEN '" + startDateCurrent +@"' AND '"+ endDateCurrent + @"'
                                    GROUP BY TB1.id

                                ) AS ABC";
            DataRow dataRowSumAll = classDatabase.getDataRow(sqlSumAll);

            mDashboard.led_t8_total = dataRowSumAll["COUNT_TOTAL"].ToString();
            mDashboard.led_t8_ok = dataRowSumAll["COUNT_ALL_OK"].ToString();
            mDashboard.led_t8_ng = dataRowSumAll["COUNT_ALL_NG"].ToString(); 
            mDashboard.workstation_total = dataTable.Rows.Count.ToString();

            //##########################################################    SET MONTH NAME.

            mDashboard.barChart_labels_month = SystemClass.getLastMonthName(setLastMonth);


            //##########################################################    GET MONTH OK/NG.
            List<string> dataMonths = SystemClass.getLastMonths(setLastMonth); // GET MONTH STRING.
            //List<string> dataNumberMonths = SystemClass.getLastMonths(setLastMonth); // GET MONTH NUMBER.
            StringBuilder sqlAllOkNg = new StringBuilder() ;
            StringBuilder sqlWidgetAll = new StringBuilder();
            StringBuilder sqlLineChart = new StringBuilder(); 

            int countMonth = 0; 
            foreach (string month in dataMonths) {

                string[] splitYearMonth = month.Split('-');
                int yearInt = int.Parse(splitYearMonth[0]) ;
                int monthInt = int.Parse(splitYearMonth[1]); 

                string startSqlDate = month+"-01";
                string endSqlDate = month+"-"+DateTime.DaysInMonth(yearInt, monthInt );

                if (countMonth>0) {
                    sqlAllOkNg.Append(" UNION ALL ");
                    sqlWidgetAll.Append(" UNION ALL ");
                    sqlLineChart.Append(" UNION ALL "); 
                }
                sqlAllOkNg.Append(this.getSqlCountOkNg(month , startSqlDate, endSqlDate) );
                sqlWidgetAll.Append(this.getSqlCountWidgetAll(month, startSqlDate, endSqlDate) );
                sqlLineChart.Append(this.getSqlLineChartCountFT8(month, startSqlDate, endSqlDate)); 

                countMonth++;

                Debug.WriteLine("----->"+ startSqlDate+" - "+ endSqlDate); 
            }

            //List<string> returnMonths = new List<string>(Enumerable.Range(1, monthRange).Select(i => DateTime.Now.AddMonths(i - monthRange)).Select(date => date.ToString("yyyy-MM-dd")));
            Debug.WriteLine(sqlAllOkNg.ToString());
            DataTable dataTableAllOkNg = classDatabase.getDataTable(sqlAllOkNg.ToString());
            

            mDashboard.barChart_datas_ok = new List<int>();
           
            mDashboard.barChart_datas_ng = new List<int>();
            foreach (DataRow dataRowAllOkNg in dataTableAllOkNg.Rows) {

                int countAllOk = int.Parse(SystemClass.returnValueZero(dataRowAllOkNg["COUNT_ALL_OK"].ToString()) ) ;
                int countAllNg = int.Parse(SystemClass.returnValueZero(dataRowAllOkNg["COUNT_ALL_NG"].ToString()));
                mDashboard.barChart_datas_ok.Add(countAllOk);
                mDashboard.barChart_datas_ng.Add(countAllNg);
            }

            //###############################################################################   LINE CHART.
            Debug.WriteLine(sqlLineChart.ToString());
            DataTable dataTableLineChart = classDatabase.getDataTable(sqlLineChart.ToString());
            mDashboard.lineChart = new List<LineChartDashboardDataSet>();
            int countLineChart = 0; 
            foreach (DataRow dataRowLineChart in dataTableLineChart.Rows) {
               // Dictionary<string, LineChartDashboardDataSet> dataPack = new Dictionary<string, LineChartDashboardDataSet>();

                LineChartDashboardDataSet lineChartDashboardDataSet = new LineChartDashboardDataSet();

                List<int> countDatas = new List<int>();
                Debug.Write(dataRowLineChart); 

                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[1].ToString())));
                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[2].ToString())));
                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[3].ToString())));
                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[4].ToString())));
                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[5].ToString())));
                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[6].ToString())));
                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[7].ToString())));
                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[8].ToString())));
                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[9].ToString())));
                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[10].ToString())));
                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[11].ToString())));
                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[12].ToString())));
                countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[13].ToString())));
                //countDatas.Add(int.Parse(SystemClass.returnValueZero(dataRowLineChart[14].ToString())));
             
                lineChartDashboardDataSet.label = dataRowLineChart["MONTH"].ToString() ;
                lineChartDashboardDataSet.data = countDatas;
                lineChartDashboardDataSet.fill = false;                
                lineChartDashboardDataSet.borderColor= this.borderColor[countLineChart] ; 


                //dataPack.Add( lineChartDashboardDataSet); 
                mDashboard.lineChart.Add(lineChartDashboardDataSet);
                countLineChart++;


            }
                //###############################################################################   WIDGET.
            Debug.WriteLine(sqlWidgetAll.ToString());
            DataTable dataTableWidgetAll = classDatabase.getDataTable(sqlWidgetAll.ToString());
            mDashboard.widgetAllFT8 = new List<int>();
            mDashboard.widgetOkFT8 = new List<int>();
            mDashboard.widgetNgFT8 = new List<int>();
            mDashboard.widgetWoFT8 = new List<int>(); 

            foreach (DataRow dataRowWidgetAll in dataTableWidgetAll.Rows) {
                int countAll = int.Parse(SystemClass.returnValueZero(dataRowWidgetAll["COUNT_ALL"].ToString()));
                int countAllOk = int.Parse(SystemClass.returnValueZero(dataRowWidgetAll["COUNT_ALL_OK"].ToString()));
                int countAllNg = int.Parse(SystemClass.returnValueZero(dataRowWidgetAll["COUNT_ALL_NG"].ToString()));
                int countAllWo = int.Parse(SystemClass.returnValueZero(dataRowWidgetAll["COUNT_ALL_WO"].ToString()));

                mDashboard.widgetAllFT8.Add(countAll);
                mDashboard.widgetOkFT8.Add(countAllOk);
                mDashboard.widgetNgFT8.Add(countAllNg);
                mDashboard.widgetWoFT8.Add(countAllWo);
            }

            return mDashboard; 

        }





    }
}