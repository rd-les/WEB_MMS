using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web_LED.App_Class;
using WEB_MMS.Models.V_PD3;

namespace WEB_MMS.DataAccessLayer.V_PD3 {
    public class Dao_LedConfigs {




        private ClassDataBase classDataBase = new ClassDataBase();
        
        public Object loadData(string configId) {


            string sql = @" SELECT * 
                            FROM pd3_config_type
                            INNER JOIN led_type_slot ON (pd3_config_type.led_type_slot_id = led_type_slot.led_type_slot_id) 
                            WHERE pd3_config_type.id = "+configId+@"
                            ";

            //DataTable dataTable = classDataBase.getDataTable(sql.ToString());
            DataRow dataRow = classDataBase.getDataRow(sql); 
            M_ConfigType m_ConfigType = new M_ConfigType();
            m_ConfigType.configTypeId = dataRow["id"].ToString();
            m_ConfigType.codex = dataRow["codex"].ToString();
            m_ConfigType.ledTypeCodeNo = dataRow["code_no"].ToString(); 
            m_ConfigType.ledTypeName = dataRow["led_type_name"].ToString();
            m_ConfigType.ledTypeSlotId = dataRow["led_type_slot_id"].ToString();
            m_ConfigType.ledTypeSlotName = dataRow["led_type_slot_name"].ToString();
            m_ConfigType.usable = dataRow["led_type_usable"].ToString();
            m_ConfigType.wMin = dataRow["w_min"].ToString();
            m_ConfigType.wMax = dataRow["w_max"].ToString();
            m_ConfigType.pfMin = dataRow["pf_min"].ToString();
            m_ConfigType.pfMax = dataRow["pf_max"].ToString();


            return m_ConfigType;
        }



        public Object saveData(M_ConfigType model) {

            Dictionary<string, string> dataFields = new Dictionary<string, string>();
            dataFields.Add("code_no", model.ledTypeCodeNo);
            dataFields.Add("led_type_name", model.ledTypeName);
            dataFields.Add("led_type_slot_id", model.ledTypeSlotId);
            dataFields.Add("w_min", model.wMin);
            dataFields.Add("w_max", model.wMax);
            dataFields.Add("pf_min", model.pfMin);
            dataFields.Add("pf_max", model.pfMax);
            dataFields.Add("led_type_usable", (model.usable==null? "0": "1"));
            
            
            if (model.action.Equals("ADD")) {
                dataFields.Add("codex", model.codex);
                classDataBase.insertData(dataFields, "pd3_config_type");

            }
            else if (model.action.Equals("EDIT")) {
                classDataBase.updateData(dataFields, "pd3_config_type", "id=" + model.configTypeId);
            }

            return SystemClass.returnResultJsonSuccess();  ; 
        }


        public Object loadDataList() {

            string sql = @" SELECT * 
                            FROM pd3_config_type
                            INNER JOIN led_type_slot ON (pd3_config_type.led_type_slot_id = led_type_slot.led_type_slot_id) 
                            WHERE led_type_usable = 1 
                            ORDER BY led_type_slot.led_type_slot_id ASC
                            "; 
            Dictionary<string, object> jsonReturn = new Dictionary<string, object>();
            List<Dictionary<string, object>> lists = new List<Dictionary<string, object>>();

            DataTable dataTable = classDataBase.getDataTable(sql.ToString());

            foreach (DataRow dataRow in dataTable.Rows ) {

                Dictionary<string, object> dataList = new Dictionary<string, object>();
                dataList.Add("id", dataRow["id"]);
                dataList.Add("code", dataRow["codex"]);
                dataList.Add("ledTypeCodeNo" , dataRow["code_no"]); 
                dataList.Add("led_type_name", dataRow["led_type_name"]);
                dataList.Add("led_type_slot_name", dataRow["led_type_slot_name"]); 
                dataList.Add("usable", dataRow["led_type_usable"]);

                dataList.Add("wMin", dataRow["w_min"]);
                dataList.Add("wMax", dataRow["w_max"]);
                dataList.Add("pfMin", dataRow["pf_min"]);
                dataList.Add("pfMax", dataRow["pf_max"]);
                dataList.Add("code_no" ,dataRow["code_no"]); 
                lists.Add(dataList); 
            }

            jsonReturn.Add("dataLists", lists);
            return jsonReturn; 


        }



    }
}