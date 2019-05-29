using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_MMS.DataAccessLayer.V_QW;

namespace WEB_MMS.Controllers
{
    public class DivisionQWController : Controller
    {
        // GET: DivisionQW
        public ActionResult Index(){
            return View();
        }


        public ActionResult V_FunctionIQA() {
            return View(); 
        }

        public ActionResult V_FunctionQA() {
            return View(); 
        }

        public ActionResult V_StatShow() {
            return View(); 
        }
        public ActionResult V_ReportIQA() {

            Dao_ReportDriver daoReportDriver = new Dao_ReportDriver();
            ViewBag.jsonData = JsonConvert.SerializeObject(Json(daoReportDriver.loadDataList())); 

            return View();
        }



        public ActionResult testExcel() {
            Dao_ReportDriver daoReportDriver = new Dao_ReportDriver();
            //urlReturn = daoReportDriver.exportExcel(mainDriverId , serverPath); 

            //string urlReturn = daoReportDriver.exportReportExcel("", "");

            return null; 
        }

        public JsonResult getReportExcelDetailIQA() {

            string mainDriverId = Request["mainDriverId"];
            string codeNo = Request["codeNo"];
            string poNo = Request["poNo"];

            string urlReturn = "/Report/QW";
            string reportPath = "/Uploads/DivisionQW/REPORT_IQA" ; 
            string serverPath = Server.MapPath(reportPath); 
            if (mainDriverId != null && !mainDriverId.Equals("")) {
                Dao_ReportDriver daoReportDriver = new Dao_ReportDriver();
                //urlReturn = daoReportDriver.exportExcel(mainDriverId , serverPath); 
                daoReportDriver.exportReportExcel(mainDriverId, serverPath , codeNo , poNo) ;
                urlReturn += "/" + mainDriverId + "/"+poNo + "-" + codeNo+".xls"; 

            }
            //return File(urlReturn, "application/ms-excel");
            return Json(new { result = true , urlReturn= urlReturn } , JsonRequestBehavior.AllowGet);
        }


        public ActionResult V_ReportDetailIQA() {

            string mainDriverId = Request["mainDriverId"];
            string codeNo = Request["codeNo"];
            string poNo = Request["poNo"];

            if (mainDriverId != null && !mainDriverId.Equals("")) {
                ViewBag.mainDriverId = mainDriverId;
            }
            return View();
        }

        public JsonResult getReportDataDetailDriver() {

            string mainDriverId = Request["mainDriverId"];
            Dao_ReportDriver daoReportDriver = new Dao_ReportDriver();            
            return Json(new { result = true, datas = daoReportDriver.getReportDataDetaiDriver(mainDriverId) });
           
        }


        public ActionResult V_ReportQA() {
            return View();
        }


    }
}