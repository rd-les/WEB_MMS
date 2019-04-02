using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Web_LED.App_Class;
using WEB_MMS.Models.Shared;

namespace WEB_MMS.DataAccessLayer.Shared {
    public class Dao_Shared {


        private ClassDataBase classDataBase = new ClassDataBase();

        public Object getJsonListComboBox(M_comboBox formModel) {
            string whereCondition = " 1 = 1 ";
            if (!string.IsNullOrEmpty(formModel.condition)) {
                whereCondition += " " + formModel.condition;
            }

            DataTable dataTable = classDataBase.getDataTable(formModel.fieldDisplay + " , " + formModel.fieldValue, formModel.tableName, whereCondition);
            Dictionary<string, object> jsonList = new Dictionary<string, object>();
            Dictionary<string, object> dataList = new Dictionary<string, object>();
            //List<Object> returnData = new List<Object>();
            //JavaScriptSerializer serializer = new JavaScriptSerializer();

            foreach (DataRow dataRow in dataTable.Rows) {
                dataList[dataRow[formModel.fieldValue].ToString()] = dataRow[formModel.fieldDisplay];
            }

            //Debug.WriteLine("----->" + SystemClass.getDateNow());
            //Debug.WriteLine("----->"+SystemClass.getDateNow("yyyy-MM-dd HH:mm:ss")) ; 

            //returnData.Add(dataList);
            jsonList.Add("comboData", dataList);

            return jsonList;
        }




    }
}