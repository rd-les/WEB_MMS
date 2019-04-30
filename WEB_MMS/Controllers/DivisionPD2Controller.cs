using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_LED.App_Class;
using WEB_MMS.DataAccessLayer.V_PD2;
using WEB_MMS.Hubs;

namespace WEB_MMS.Controllers
{
    public class DivisionPD2Controller : Controller
    {
        // GET: DivisionPD2
        public ActionResult Index()
        {
            return View("V_Main");
        }

        public ActionResult V_Main() {
            DAO_Dashboard daoDashboard = new DAO_Dashboard();
            DateTime dateNow = DateTime.Now;

            string paramDateStart = SystemClass.returnValue(Request["paramDateStart"]);
            string paramDataEnd = SystemClass.returnValue(Request["paramDataEnd"]);


            if (paramDateStart.Equals("") || paramDataEnd.Equals("") ) {
                string thisYear = dateNow.Year.ToString();
                string thisMonth = dateNow.Month.ToString("00");
                string thisDay = dateNow.Day.ToString("00");

                DateTime startMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(-5); 

                string startDateCurrent = startMonth.Year + "-" + startMonth.Month.ToString("00") + "-01 00:00:00";
                string endDateCurrent = thisYear + "-" + thisMonth + "-" + thisDay + " 23:59:59";

                ViewBag.jsonData = JsonConvert.SerializeObject(Json(daoDashboard.loadStartUpData(startDateCurrent, endDateCurrent)));
                ViewBag.paramDateStart = "01-"+ startMonth.Month+"-"+ startMonth.Year;
                ViewBag.paramDateEnd = thisDay+"-"+thisMonth+"-"+thisYear;

                ViewBag.paramDateStartSearch = startMonth.Year + "-" + startMonth.Month + "-01" ;
                ViewBag.paramDateEndSearch = thisYear+"-"+ thisMonth + "-" + thisDay;

            }
            else {

                ViewBag.jsonData = JsonConvert.SerializeObject(Json(daoDashboard.loadStartUpData(paramDateStart, paramDataEnd)));
                ViewBag.paramDateStart = paramDateStart;
                ViewBag.paramDataEnd = paramDateStart ;
            }
           
            return View(); 
        }

        public JsonResult getDashboardDataByCriteria() {

            string dateStart = Request["txt_date_dashboard_start"];
            string dateEnd = Request["txt_date_dashboard_end"];
            DAO_Dashboard daoDashboard = new DAO_Dashboard();

            if (!dateStart.Equals("") && !dateEnd.Equals("")) {
                return Json(daoDashboard.searchDataDashboard(dateStart, dateEnd));
            }
            else {
                return Json(SystemClass.MS_FAILURE); 
            }
            
        }
        public ActionResult V_ReportAging() {

            ViewBag.Message = "LED Work Station Finish.";
            DAO_ReportAging daoReportAging = new DAO_ReportAging();

            ViewBag.jsonData = JsonConvert.SerializeObject(Json(daoReportAging.loadDataList()));
            return View();
        }

        public ActionResult V_ReportAgingDetail() {

            if (!string.IsNullOrEmpty(Request["workStationId"])) {
                ViewBag.workStationid = Request["workStationId"]; 
                //DAO_ReportAging daoReportAging = new DAO_ReportAging();
                // ViewBag.jsonData = JsonConvert.SerializeObject(Json(daoReportAging.loadDataDetailWorkStation(Request["workStationId"])));

            }
            return View();
        }

        public ActionResult V_ReportFT8() {          

            Dao_ReportFT8 daoReportFT8 = new Dao_ReportFT8();
            ViewBag.jsonData = JsonConvert.SerializeObject(Json(daoReportFT8.loadDataList()))  ;
            return View();

        }

        public ActionResult V_ReportDetailFT8() {

            string mainDataId = Request["mainDataId"];
            string codeNo = Request["codeNo"];
            string workStationNo = Request["workStationNo"];



            if (mainDataId != null && !mainDataId.Equals("")) {

                ViewBag.mainDataId = mainDataId;
            }
            return View(); 
        }


        public JsonResult getReportExcelDetailFT8() {

            string mainDataId = Request["main_data_id"];
            string codeNo = Request["codeNo"];
            string workStationNo = Request["workStationNo"];
            Dao_ReportFT8 daoReportFT8 = new Dao_ReportFT8();
            daoReportFT8.getReportExcelDetailFT8(mainDataId, codeNo, workStationNo);
            string urlReturn = "/Report/PD2";
            urlReturn += "/" + mainDataId + "/" + codeNo + "-" + workStationNo + ".xls";

            // codeNo+"-"+workStationNo , "xls"
            return Json(new { result = true, urlReturn = urlReturn });
        }
        public JsonResult getReportDataDetailFT8() {

            string mainDataId = Request["main_data_id"];
            Dao_ReportFT8 daoReportFT8 = new Dao_ReportFT8();

            return Json(new { result = true, datas = daoReportFT8.getReportDataDetailFT8(mainDataId) });
            //return null; 
        }

        public JsonResult getStartUpDataWorkStation() {

            Dao_ReportFT8 daoReportFT8 = new Dao_ReportFT8();
            return Json(new { datas = daoReportFT8.getStartUpDataWorkStation() });
        }


        public JsonResult getLastDataRealtimeGraph() {
            
            Dao_ReportFT8 daoReportFT8 = new Dao_ReportFT8();
            return Json(new { datas = daoReportFT8.getLastDataRealtimeGraph() });
        }
        public ActionResult DataRealTime() {
            return View(); 
        }

        public void getDataRealTimeFT8() {

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<FT8_Hub>();
            string dateTime = DateTime.Now.ToString(@"dd-MM-yyyy HH:mm:ss");
            string ledCount = Request["ledCount"];
            string dataWatt = Request["dataWatt"];
            string dataPF = Request["dataPF"];
            string dataTHDi = Request["dataTHDi"];            
            string dataResult = Request["dataResult"];
            string ledNo = Request["led_no"];
            string lastSec = Request["lastSec"]; ;
        
            // (ledCount, dateTime  , result , watt , dataPF, dataTHDi ) 
            hubContext.Clients.All.receiveDataRealTimeFT8(ledCount, dateTime, (dataResult.Equals("1") ? "TRUE" : "FALSE"), dataWatt, dataPF , dataTHDi , ledNo , lastSec);

        }

        public ActionResult V_Aging() {
            return View(); 
        }

        public ActionResult V_AgingRealTime() {
            return View(); 
        }

        public ActionResult V_FT8_FINAL() {
            return View();
        }

        public ActionResult V_QualityControl() {
            return View();
        }






    }
}