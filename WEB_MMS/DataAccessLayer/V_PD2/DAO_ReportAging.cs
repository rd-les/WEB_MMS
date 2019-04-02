using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Web_LED.App_Class;

namespace WEB_MMS.DataAccessLayer.V_PD2 {
    public class DAO_ReportAging {

        private ClassDataBase classDataBase = new ClassDataBase();
        private string tableName = "work_station_finish";


        public Object loadDataDetailWorkStation(string workStationId) {

            //string filePath = HostingEnvironment.MapPath("~/" + ConfigClass.JSON_FINISH_DETAIL_PATH + "/" + workStationId + "/" + ConfigClass.JSON_FINISH_DETAIL_FILENAME);
            string filePath = ConfigClass.PATH_DATA_SERVER+"/" + ConfigClass.JSON_FINISH_DETAIL_PATH + "/" + workStationId + "/" + ConfigClass.JSON_FINISH_DETAIL_FILENAME ;
            var jsonText = "";
            try {
                jsonText = File.ReadAllText(filePath);

            }
            catch (Exception ex) {
                Debug.WriteLine(ex);
            }

            return JsonConvert.DeserializeObject(jsonText);

        }
        public Object loadDataList() {

            string sql = @" SELECT TB2.*
                                , TB1.led_total AS led_total_finish
                                , TB1.led_good AS led_good_finish
                                , TB1.led_bad AS led_bad_finish
                                , (SELECT personal_name From personal WHERE personal_id = finish_id ) as finish_name 
                            FROM " + tableName + @" TB1
                            INNER JOIN work_station TB2 ON (TB1.work_station_id = TB2.work_station_id)
                            WHERE 1=1  
                                AND work_station_finish = 'Y'  
                            ORDER BY work_station_finish_date DESC ";
            Dictionary<string, object> jsonReturn = new Dictionary<string, object>();
            List<Dictionary<string, object>> lists = new List<Dictionary<string, object>>();

            DataTable dataTable = classDataBase.getDataTable(sql.ToString());

            foreach (DataRow dataRow in dataTable.Rows) {
                Dictionary<string, object> dataList = new Dictionary<string, object>();

                dataList.Add("workStationId", dataRow["work_station_id"]);
                dataList.Add("workNumber", dataRow["work_number"]);
                dataList.Add("rackNumber", dataRow["rack_number"]);
                dataList.Add("led_total", SystemClass.returnValueHyphen(dataRow["led_total_finish"]));
                dataList.Add("led_good", SystemClass.returnValueHyphen(dataRow["led_good_finish"]));
                dataList.Add("led_bad", SystemClass.returnValueHyphen(dataRow["led_bad_finish"]));


                dataList.Add("data_V", dataRow["V_min"] + " - " + dataRow["V_max"]);
                dataList.Add("data_I", dataRow["I_min"] + " - " + dataRow["I_max"]);
                dataList.Add("data_W", dataRow["W_min"] + " - " + dataRow["W_max"]);
                dataList.Add("data_PF", dataRow["PF_min"] + " - " + dataRow["PF_max"]);

                dataList.Add("dateFinish", dataRow["work_station_finish_date"].ToString());
                dataList.Add("finishId", dataRow["finish_id"]);
                dataList.Add("finishName", dataRow["finish_name"]);
               // dataList.Add("reportWorkStationDetail", ConfigClass.PATH_REPORT_FINISH_DETAIL + "/" + dataRow["work_station_id"] + "/" + ConfigClass.REPORT_FINISH_NAME);
               // dataList.Add("reportWorkStationShortDetail", ConfigClass.PATH_REPORT_FINISH_DETAIL + "/" + dataRow["work_station_id"] + "/" + ConfigClass.REPORT_FINISH_SHORT_NAME);


                lists.Add(dataList);
            }
            jsonReturn.Add("dataLists", lists);

            return jsonReturn;

        }






    }
}