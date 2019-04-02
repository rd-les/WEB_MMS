using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_MMS.Models.V_PD2 {
    public class M_MainDataDetailFT8 {


        public string id { get; set; }
        public string FT8_main_data_id { get; set; }
        public string led_no { get; set; }
        public string data_watt { get; set; }
        public string data_PF { get; set; }
        public string data_THDi { get; set; }
        public string data_volt { get; set; }
        public string data_mA { get; set; }
        public string data_THDv { get; set; }
        public string power_out_watt { get; set; }

        public string VLED { get; set; }
        public string ILED { get; set; }
        public string Efficiency { get; set; }
        public string ActionResult { get; set; }
        public string LowWatt { get; set; }
        public string HighWatt { get; set; }
        public string MaxTHDi { get; set; }
        public string data_datetime { get; set; }
        public string date_create { get; set; }
        public string led_count { get; set; }
        public string led_count_no { get; set; }


    }
}