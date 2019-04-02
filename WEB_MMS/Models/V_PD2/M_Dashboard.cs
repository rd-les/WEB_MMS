using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_MMS.Models.V_PD2 {
    public class M_Dashboard {


        public string led_t8_total { get; set;  }
        public string led_t8_ok { get; set; }
        public string led_t8_ng { get; set; }
        public string workstation_total { get; set; }


        public List<string> barChart_labels_month { get; set; }
        public List<int> barChart_datas_ok { get; set; }
        public List<int> barChart_datas_ng { get; set; }


        //######################################################### WIDGET.

        public List<int> widgetAllFT8 { get; set; }

        public List<int> widgetOkFT8 { get; set; }

        public List<int> widgetNgFT8 { get; set; }

        public List<int> widgetWoFT8 { get; set; }

        public List<LineChartDashboardDataSet > lineChart { get; set; }





    }

    public class LineChartDashboardDataSet {
        public List<int> data { get; set; }
        public string label { get; set; }        
        public string borderColor { get; set; }
        public bool fill { get; set; }

    }
}