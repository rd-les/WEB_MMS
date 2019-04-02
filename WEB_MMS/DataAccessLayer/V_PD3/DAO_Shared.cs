using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web_LED.App_Class;

namespace WEB_MMS.DataAccessLayer.V_PD3 {
    public class DAO_Shared {


        ClassDataBase classDatabase = new ClassDataBase();

        public Object getDataLedTypeCheckbox(string ledTypeSlotId) {

            string sql = @" SELECT
                                id, 
	                            code_no
                            FROM
	                            pd3_config_type
                            WHERE led_type_usable = 1 AND 
	                            led_type_slot_id =" + ledTypeSlotId;

            DataTable dataTable = classDatabase.getDataTable(sql);
            List<Dictionary<string, string>> lists = new List<Dictionary<string, string>>();

            foreach (DataRow dataRow in dataTable.Rows) {

                Dictionary<string, string> data = new Dictionary<string, string>();
                data["led_type_slot_id"] = dataRow["id"].ToString();
                data["code_no"] = dataRow["code_no"].ToString();

                lists.Add(data); 

            }
            return lists; 

        }




    }
}