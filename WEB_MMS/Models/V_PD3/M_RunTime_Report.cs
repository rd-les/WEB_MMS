using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_MMS.Models.V_PD3 {
    public class M_RunTime_Report {


        public string pd3MainId { get; set; }
        public string pd3ConfigTypeId { get; set; }
        public string pd3ConfigTypeName { get; set; }
        public string dateCreate { get; set; }
        public string workStationNo { get; set; }
        public string codeNo { get; set; }
        public string workStationCount { get; set; }
        public string ledTypeName { get; set; }
        public string dataMinW { get; set;  }
        public string dataMaxW { get; set;  }
        public string dataResultOK { get; set; }
        public string dataResultNG { get; set; }
        public string pairResultOK { get; set; }
        public string pairResultNG { get; set; }


        public List<M_RunTime_Report_Detail> pd3Details { get; set; }

    }

    public class M_RunTime_Report_Detail {

        public string pd3DetailId { get; set; }
        public string pd3MainId { get; set; }
        public string lotNo { get; set; }
        public string ledNo { get; set; }
        public string dataWatt { get; set; }
        public string dataVolt { get; set; }
        public string dataCurrent { get; set; }
        public string dataPower { get; set; }
        public string dataResult { get; set; }
        public string pairResult { get; set; }
        public string createDatetime { get; set; }
        public string dataDatetime { get; set; }
        public string workStationCount { get; set; }
       
    }
}