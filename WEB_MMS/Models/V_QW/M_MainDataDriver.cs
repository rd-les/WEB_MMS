using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_MMS.Models.V_QW {
    public class M_MainDataDriver {


        public string id { get; set; }

        public string poNo { get; set; }
        public string codeNo { get; set; }
        public string dateCreate { get; set; }
        public string driverCount { get; set; }

        public string driverOK { get; set; }
        public string driverNG { get; set; }

        public List<M_MainDataDetailDriver> mMainDataDetailDriver { get; set; }


    }
}