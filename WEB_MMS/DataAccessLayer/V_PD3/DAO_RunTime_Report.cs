using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web_LED.App_Class;
using WEB_MMS.Models.V_PD3;

namespace WEB_MMS.DataAccessLayer.V_PD3 {
    public class DAO_RunTime_Report {


        ClassDataBase classDatabase = new ClassDataBase();


        public List<M_RunTime_Report> loadDataListSearch_vReport(string pd3TypeSlotId , string[] chkGroups) {


            string whereIn = "";
            int count = 0; 
            foreach (string chkGroup in chkGroups) {

                if (count>0) {
                    whereIn += ",";
                }
                whereIn += " "+chkGroup; 
                count++;
            }
            //M_RunTime_Report
            string sql = @" SELECT
	                            TB1.*
                                , TB2.led_type_name
                            FROM
	                            FT_PD3_main TB1
                            INNER JOIN pd3_config_type TB2 ON (TB1.pd3_config_type_id = TB2.id)
                            WHERE 
                                TB2.led_type_slot_id = " + pd3TypeSlotId
                                + " AND TB1.pd3_config_type_id IN ("+whereIn+") " ;

            DataTable dataTable = classDatabase.getDataTable(sql);

            List<M_RunTime_Report> returnLists = new List<M_RunTime_Report>();
            foreach (DataRow dataRow in dataTable.Rows) {

                M_RunTime_Report m_RunTime_Report = new M_RunTime_Report();

                m_RunTime_Report.pd3MainId = dataRow["id"].ToString();
                m_RunTime_Report.pd3ConfigTypeId = dataRow["pd3_config_type_id"].ToString();
                m_RunTime_Report.dateCreate = dataRow["date_create"].ToString();
                m_RunTime_Report.workStationNo = dataRow["work_station_no"].ToString();
                m_RunTime_Report.codeNo = dataRow["code_no"].ToString();
                m_RunTime_Report.workStationCount = dataRow["work_station_count"].ToString();
                m_RunTime_Report.ledTypeName = dataRow["led_type_name"].ToString();

                returnLists.Add(m_RunTime_Report);

            }


            return returnLists;
        }



        public List<M_RunTime_Report> loadDataList_vReport(string pd3TypeSlotId) {


            //M_RunTime_Report
            string sql = @" SELECT
	                            TB1.*
                                , TB2.led_type_name
                            FROM
	                            FT_PD3_main TB1
                            INNER JOIN pd3_config_type TB2 ON (TB1.pd3_config_type_id = TB2.id)
                            WHERE 
                                TB2.led_type_slot_id = " + pd3TypeSlotId;
            DataTable dataTable = classDatabase.getDataTable(sql);

            List<M_RunTime_Report> returnLists = new List<M_RunTime_Report>(); 
            foreach (DataRow dataRow in dataTable.Rows) {

                M_RunTime_Report m_RunTime_Report = new M_RunTime_Report();

                m_RunTime_Report.pd3MainId = dataRow["id"].ToString() ;
                m_RunTime_Report.pd3ConfigTypeId = dataRow["pd3_config_type_id"].ToString();
                m_RunTime_Report.dateCreate = dataRow["date_create"].ToString();
                m_RunTime_Report.workStationNo = dataRow["work_station_no"].ToString();
                m_RunTime_Report.codeNo = dataRow["code_no"].ToString();
                m_RunTime_Report.workStationCount = dataRow["work_station_count"].ToString();
                m_RunTime_Report.ledTypeName = dataRow["led_type_name"].ToString();

                returnLists.Add(m_RunTime_Report); 

            }


            return returnLists; 
        }

        public Object getReportDetailPd3(string mainDataId) {

            string sql = @" SELECT * 
                                , ( SELECT COUNT(id) FROM FT_PD3_main_detail WHERE FT_PD3_datas_id =  5 AND dataResult = 1	) AS dataResultOK
                                , ( SELECT COUNT(id) FROM FT_PD3_main_detail WHERE FT_PD3_datas_id =  5 AND dataResult = 0	) AS dataResultNG
                                , ( SELECT COUNT(id) FROM FT_PD3_main_detail WHERE FT_PD3_datas_id =  5 AND pairResult = 1	) AS pairResultOK
                                , ( SELECT COUNT(id) FROM FT_PD3_main_detail WHERE FT_PD3_datas_id =  5 AND pairResult = 0	) AS pairResultNG
                            FROM FT_PD3_main TB1
                            INNER JOIN pd3_config_type TB2 ON ( TB1.pd3_config_type_id = TB2.id)
                            Where TB1.id = " + mainDataId;

            DataRow dataRow = classDatabase.getDataRow(sql);

            M_RunTime_Report m_RunTime_Report = new M_RunTime_Report();

            m_RunTime_Report.pd3MainId = mainDataId;
            m_RunTime_Report.pd3ConfigTypeId = dataRow["pd3_config_type_id"].ToString();
            m_RunTime_Report.dateCreate = dataRow["date_create"].ToString();
            m_RunTime_Report.workStationNo = dataRow["work_station_no"].ToString();
            m_RunTime_Report.codeNo = dataRow["code_no"].ToString();
            m_RunTime_Report.workStationCount = dataRow["work_station_count"].ToString();
            m_RunTime_Report.ledTypeName = dataRow["led_type_name"].ToString();


            m_RunTime_Report.dataResultOK = dataRow["dataResultOK"].ToString();
            m_RunTime_Report.dataResultNG = dataRow["dataResultNG"].ToString();
            m_RunTime_Report.pairResultOK = dataRow["pairResultOK"].ToString();
            m_RunTime_Report.pairResultNG = dataRow["pairResultNG"].ToString();

            m_RunTime_Report.dataMinW = dataRow["w_min"].ToString();
            m_RunTime_Report.dataMaxW = dataRow["w_max"].ToString();



            m_RunTime_Report.pd3Details = new List<M_RunTime_Report_Detail>(); 
            string sqlDetail = "SELECT * FROM FT_PD3_main_detail WHERE FT_PD3_datas_id = "+mainDataId ;
            DataTable dataTableDetail = classDatabase.getDataTable(sqlDetail);
            foreach (DataRow dataRowDetail in dataTableDetail.Rows ) {

                M_RunTime_Report_Detail m_RunTime_Report_Detail = new M_RunTime_Report_Detail();
                m_RunTime_Report_Detail.pd3DetailId = dataRowDetail["id"].ToString();
                m_RunTime_Report_Detail.pd3MainId = dataRowDetail["FT_PD3_datas_id"].ToString();
                m_RunTime_Report_Detail.lotNo = dataRowDetail["lot_no"].ToString();
                m_RunTime_Report_Detail.ledNo = dataRowDetail["led_no"].ToString();
                m_RunTime_Report_Detail.dataWatt = dataRowDetail["dataWatt"].ToString();
                m_RunTime_Report_Detail.dataVolt = dataRowDetail["dataVolt"].ToString();
                m_RunTime_Report_Detail.dataCurrent = dataRowDetail["dataCurrent"].ToString();
                m_RunTime_Report_Detail.dataPower = dataRowDetail["dataPower"].ToString();
                m_RunTime_Report_Detail.dataResult = dataRowDetail["dataResult"].ToString();
                m_RunTime_Report_Detail.pairResult = dataRowDetail["pairResult"].ToString();
                m_RunTime_Report_Detail.createDatetime = dataRowDetail["create_datetime"].ToString();
                m_RunTime_Report_Detail.dataDatetime = dataRowDetail["data_datetime"].ToString();
                m_RunTime_Report_Detail.workStationCount = dataRowDetail["work_station_count"].ToString();

                m_RunTime_Report.pd3Details.Add(m_RunTime_Report_Detail);
            }




            return m_RunTime_Report ; 
        }

    }
}