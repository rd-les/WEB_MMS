using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web_LED.App_Class;

namespace WEB_MMS.DataAccessLayer.V_PD3 {
    public class Dao_LedSlotType {

        private ClassDataBase classDataBase = new ClassDataBase();


        public Object loadDataList() {

            string sql = " SELECT * FROM led_type_slot ORDER BY sort_no ASC "; 
            Dictionary<string, object> jsonReturn = new Dictionary<string, object>();
            List<Dictionary<string, object>> lists = new List<Dictionary<string, object>>();

            DataTable dataTable = classDataBase.getDataTable(sql.ToString());

            foreach (DataRow dataRow in dataTable.Rows) {

                Dictionary<string, object> dataList = new Dictionary<string, object>();
                dataList.Add("led_type_slot_id", dataRow["led_type_slot_id"]);
                dataList.Add("codex", dataRow["codex"]);
                dataList.Add("led_type_slot_name", dataRow["led_type_slot_name"]);
                dataList.Add("usable", dataRow["usable"]);
                dataList.Add("sort_no" , dataRow["sort_no"]); 

                lists.Add(dataList);
            }

            jsonReturn.Add("dataLists", lists);
            return jsonReturn;


        }




    }
}