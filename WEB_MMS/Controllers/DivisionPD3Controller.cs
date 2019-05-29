using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_LED.App_Class;
using WEB_MMS.DataAccessLayer.Shared;
using WEB_MMS.DataAccessLayer.V_PD3;
using WEB_MMS.Models.Shared;
using WEB_MMS.Models.V_PD3;

namespace WEB_MMS.Controllers {
    public class DivisionPD3Controller : Controller {
        // GET: DivisionPD3
        public ActionResult Index() {
            return View("V_Main");
            //return View();
        }

        public ActionResult V_StatShow() {
            return View(); 
        }

        public JsonResult getSearchRunTimeReport(string pd3TypeSlotId, string[] chkGroups) {

            DAO_RunTime_Report dao = new DAO_RunTime_Report();

            return Json(new { result = SystemClass.MS_SUCCESS, datas = dao.loadDataListSearch_vReport(pd3TypeSlotId, chkGroups) });

        }



        public JsonResult getDataLedTypeCheckbox() {

            if (!string.IsNullOrEmpty(Request["led_type_slot_id"])) {
                string ledTypeSlotId = Request["led_type_slot_id"];
                DAO_Shared dao = new DAO_Shared();
                //DAO_ReportAging daoReportAging = new DAO_ReportAging();
                // ViewBag.jsonData = JsonConvert.SerializeObject(Json(daoReportAging.loadDataDetailWorkStation(Request["workStationId"])));
                return Json(new { result = true, datas = dao.getDataLedTypeCheckbox(ledTypeSlotId) });
            }
            else {
                return Json(new { result = SystemClass.MS_FAILURE });
            }

        }

        public JsonResult getDashboardDataByCriteria() {

            string dateStart = Request["txt_date_dashboard_start"];
            string dateEnd = Request["txt_date_dashboard_end"];
            string ledTypeSlotId = Request["led_type_slot_id"];
            string[] chkGroups = Request.Form.GetValues("chkGroups[]");

            DAO_Dashboard daoDashboard = new DAO_Dashboard();
            if (!dateStart.Equals("") && !dateEnd.Equals("")) {
                return Json( new { result = true, datas = daoDashboard.searchDataDashboard(ledTypeSlotId, dateStart, dateEnd , chkGroups)  } );
            }
            else {
                return Json(SystemClass.MS_FAILURE);
            }

        }


        public JsonResult getDataDashboard() {

            DAO_Dashboard dao = new DAO_Dashboard();
            string ledTypeSlotId = Request["led_type_slot_id"];
            return Json(new { result = true, datas = dao.getDataDashboard(ledTypeSlotId)  }); ; 
        }

        public JsonResult getDataDashboardByCriteria() {

            return null; 
        }

        public ActionResult V_Main() {
            return View();
        }

        public ActionResult V_MR16() {
            return View();
        }
        public ActionResult V_E27() {
            return View();


        }

        public ActionResult V_E14() {
            return View();
        }

        public ActionResult V_GU10() {
            return View();


        }

        public ActionResult V_PAR30() {
            return View();


        }

        public ActionResult V_PAR38() {
            return View();


        }

        public ActionResult V_PAR20() {
            return View();


        }

        public ActionResult V_LED_T8() {
            return View();


        }

        public ActionResult V_DRIVER() {
            return View();


        }
        public ActionResult V_Report_MR16() {

            ViewBag.Message = "V_Report_MR16";
            DAO_RunTime_Report dao = new DAO_RunTime_Report();
            ViewBag.jsonData = JsonConvert.SerializeObject(Json(dao.loadDataList_vReport("1")));
            return View();

        }

        public ActionResult V_Report_GU10() {

            ViewBag.Message = "V_Report_GU10";
            DAO_RunTime_Report dao = new DAO_RunTime_Report();
            ViewBag.jsonData = JsonConvert.SerializeObject(Json(dao.loadDataList_vReport("2")));
            return View();

        }

        public ActionResult V_Report_E14() {
            ViewBag.Message = "V_Report_E14";
            DAO_RunTime_Report dao = new DAO_RunTime_Report();
            ViewBag.jsonData = JsonConvert.SerializeObject(Json(dao.loadDataList_vReport("3")));
            return View();
        }
        public ActionResult V_Report_E27() {

            ViewBag.Message = "V_Report_E27";
            DAO_RunTime_Report dao = new DAO_RunTime_Report();
            ViewBag.jsonData = JsonConvert.SerializeObject(Json(dao.loadDataList_vReport("4")));
            return View();

        }

        public ActionResult V_Report_PAR20() {

            ViewBag.Message = "V_Report_PAR20";
            DAO_RunTime_Report dao = new DAO_RunTime_Report();
            ViewBag.jsonData = JsonConvert.SerializeObject(Json(dao.loadDataList_vReport("5")));
            return View();

        }

        public ActionResult V_Report_PAR30() {

            ViewBag.Message = "V_Report_PAR30";
            DAO_RunTime_Report dao = new DAO_RunTime_Report();
            ViewBag.jsonData = JsonConvert.SerializeObject(Json(dao.loadDataList_vReport("6")));
            return View();

        }
        public ActionResult V_Report_PAR38() {
            ViewBag.Message = "V_Report_PAR38";
            DAO_RunTime_Report dao = new DAO_RunTime_Report();
            ViewBag.jsonData = JsonConvert.SerializeObject(Json(dao.loadDataList_vReport("7")));
            return View();

        }

        public ActionResult V_Report_LEDT8() {
            ViewBag.Message = "V_Report_LEDT8";
            DAO_RunTime_Report dao = new DAO_RunTime_Report();
            ViewBag.jsonData = JsonConvert.SerializeObject(Json(dao.loadDataList_vReport("8")));
            return View();

        }
        public ActionResult V_Report_Driver() {

            return View();

        }



        public Object getReportDetailPd3(string mainDataId) {

            DAO_RunTime_Report dao = new DAO_RunTime_Report();
            return Json(dao.getReportDetailPd3(mainDataId));
        }

        public ActionResult V_ReportDetail_MR16() {
            return View();
        }

        public ActionResult V_ReportDetail_GU10() {


            return View();
        }
        public ActionResult V_ReportDetail_E27() {

            string mainDataId = Request["mainDataId"];
            if (mainDataId != null && !mainDataId.Equals("")) {
                ViewBag.mainDataId = mainDataId;
                ViewBag.jsonData = JsonConvert.SerializeObject(this.getReportDetailPd3(mainDataId));
            }

            return View();
        }
        public ActionResult V_ReportDetail_E14() {


            return View();
        }
        public ActionResult V_ReportDetail_PAR30() {


            return View();
        }
        public ActionResult V_ReportDetail_PAR38() {


            return View();
        }
        public ActionResult V_ReportDetail_PAR20() {


            return View();
        }
        public ActionResult V_ReportDetail_LEDT8() {


            return View();
        }
        public ActionResult V_ReportDetail_Driver() {


            return View();
        }

        public ActionResult V_SettingSlotType() {

            ViewBag.Message = "PD3 LED CONFIG TYPE.";
            Dao_LedSlotType dao = new Dao_LedSlotType();
            ViewBag.jsonData = JsonConvert.SerializeObject(Json(dao.loadDataList()));

            return View();
        }

        public ActionResult V_SettingProductTypeDetail(M_ConfigType model) {

            string configId = Request["configId"];
            Dao_Shared mShared = new Dao_Shared();
            M_comboBox combo = new M_comboBox("led_type_slot", "led_type_slot_name", "led_type_slot_id", " AND usable = '1'");
            ViewBag.jsonComboType = JsonConvert.SerializeObject(Json(mShared.getJsonListComboBox(combo)));

            if (configId != null && !configId.Equals("")) {
                ViewBag.mainDataId = configId;
                Dao_LedConfigs daoLedConfigs = new Dao_LedConfigs();
                ViewBag.jsonData = JsonConvert.SerializeObject(Json(new { result = true, datas = daoLedConfigs.loadData(configId) }));

            }
            else if (!string.IsNullOrEmpty(Request["saveData"])) {
                Dao_LedConfigs daoLedConfigs = new Dao_LedConfigs();
                return Json(daoLedConfigs.saveData(model));
            }
            else {
                ViewBag.jsonData = JsonConvert.SerializeObject(Json(new { result = "FAILURE", codex = SystemClass.generateString(20) }));
            }


            return View();
        }

        public ActionResult V_SettingProductType() {


            ViewBag.Message = "PD3 LED CONFIG TYPE.";
            Dao_LedConfigs dao_LedConfigs = new Dao_LedConfigs();
            ViewBag.jsonData = JsonConvert.SerializeObject(Json(dao_LedConfigs.loadDataList()));

            return View();
        }
    }
}