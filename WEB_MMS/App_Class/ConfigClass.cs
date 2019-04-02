using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LED.App_Class {
    public class ConfigClass {

        public static String NEXT_NUMBER_START = "1";
        public static String NEXT_NUMBER_START_ZERO = "0";


        //public static readonly String PATH_DATA_SERVER = @"D:/WEB_MMS"; 
        public static readonly String PATH_DATA_SERVER = @"D:\Documents\Visual Studio 2017\Projects\WEB_LED\WEB_LED";
        
        public static readonly String PATH_REPORT_FINISH_DETAIL = "Report/ReportWorkStationFinish/WorkStationDetail";
        public static readonly String REPORT_FINISH_NAME = "ReportWorkStationDetail.pdf";
        public static readonly String REPORT_FINISH_SHORT_NAME = "ReportWorkStationDetailShort.pdf";
        public static readonly String JSON_FINISH_DETAIL_PATH = "RESTful/WorkStationFinish";
        public static readonly String JSON_FINISH_DETAIL_FILENAME = "workStationFinishDetail.json";
        public static readonly String JSON_FINISH_ALL_DETAIL_FILENAME = "workStationAllDetail.json";


        public static readonly String URL_WEB_LES_SERVICE_WO_DETAIL = "http://192.168.18.100:88/Service/GetWorkStationDetail";

    }
}