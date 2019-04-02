using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_MMS.Models.Shared {
    public class M_comboBox {


        public M_comboBox(string _tableName, string _fieldDisplay, string _fieldValue) {
            this.tableName = _tableName;
            this.fieldDisplay = _fieldDisplay;
            this.fieldValue = _fieldValue;
        }

        public M_comboBox(string _tableName, string _fieldDisplay, string _fieldValue, string _condition) {
            this.tableName = _tableName;
            this.fieldDisplay = _fieldDisplay;
            this.fieldValue = _fieldValue;
            this.condition = _condition;
        }

        public M_comboBox() {

        }
        public string tableName { get; set; }
        public string fieldDisplay { get; set; }
        public string fieldValue { get; set; }
        public string condition { get; set; }



    }
}