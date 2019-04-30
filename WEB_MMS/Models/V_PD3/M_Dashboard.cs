using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_MMS.Models.V_PD3 {
    public class M_Dashboard {



     

    }

    public class M_Dashboard_Widget {

        public List<int> widgetAll { get; set; }
        public List<int> widgetOk { get; set; }
        public List<int> widgetNg { get; set; }
        public List<int> widgetWorkstation { get; set; }

    }


    public class M_Dashboard_Barchart {

        //######################################################### Barchart.
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string borderWidth { get; set; }
        public List<int> data { get; set; }


        //######################################################### Barchart.


        /*
    label: "My First dataset",
    data: [65, 59, 80, 81, 56, 55, 40],
    borderColor: "rgba(0, 123, 255, 0.9)",
    borderWidth: "0",
    backgroundColor: "rgba(0, 123, 255, 0.5)"
    */


    }
}