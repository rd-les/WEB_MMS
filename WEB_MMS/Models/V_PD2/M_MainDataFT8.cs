using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_MMS.Models.V_PD2 {
    public class M_MainDataFT8 {
        public string FT8_MainDataId { get; set; }
        public string workStationNo { get; set; }
        public string codeNo { get; set; }
        public string dateCreate { get; set; }
        public string ledTotal { get; set; }
        public string ledGood { get; set; }
        public string ledBad { get; set; }

        public string ledLowWatt { get; set;  }
        public string ledHighWatt { get; set;  }
        public List<M_MainDataDetailFT8> mMainDataDetailFT8 { get; set;  }

    }
}