using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_LED.App_Class;
using WEB_MMS.Models.V_PD3;

namespace WEB_MMS.DataAccessLayer.V_PD3 {
    public class DAO_Dashboard {

        private int[] ledTypeSlotId = {1, 2, 3, 4, 5 ,6 , 7 , 8};
        private string[] ledTypeSlotName = { "MR-16", "GU-10", "E-14", "E-27", "PAR-20", "PAR-30", "PAR-38", "LED-T8" };
        private int minDataTest = 10;
        private int maxDataTest = 100; 

        public Object getDataDashboard(string ledTypeSlotId) {


            Dictionary<string, Object> dataReturn = new Dictionary<string, object>();

            dataReturn.Add("barChart" , getDataDashboardBarChart(ledTypeSlotId) );
            dataReturn.Add("widget", getDataDashboardWidget(ledTypeSlotId));

            return dataReturn; 
        }



        Random randomNumber = new Random()  ;
        private int generateNumber(int min, int max) {
            return randomNumber.Next(min, max);
        }



        public Object searchDataDashboard(string ledTypeSlotId , string startDateCriteria, string endDateCriteria , string[] chkGroups)  {

            Dictionary<string, Object> dataReturn = new Dictionary<string, object>();

            dataReturn.Add("barChart", getDataDashboardBarChart(ledTypeSlotId));
            dataReturn.Add("widget", getDataDashboardWidget(ledTypeSlotId));

            return dataReturn;
        }

        public M_Dashboard_Widget getDataDashboardWidget(string ledTypeSlotId) {

            M_Dashboard_Widget mDashboardWidget = new M_Dashboard_Widget();
            mDashboardWidget.widgetAll = new List<int>();
            mDashboardWidget.widgetOk = new List<int>();
            mDashboardWidget.widgetNg = new List<int>();
            mDashboardWidget.widgetWorkstation = new List<int>();

            for (int index = 0; index <= 6; index++ ) {
                mDashboardWidget.widgetAll.Add(generateNumber(minDataTest , maxDataTest));
                mDashboardWidget.widgetOk.Add(generateNumber(minDataTest, maxDataTest));
                mDashboardWidget.widgetNg.Add(generateNumber(minDataTest, maxDataTest));
                mDashboardWidget.widgetWorkstation.Add(generateNumber(minDataTest, maxDataTest));
            }



            return mDashboardWidget; 
        }

        public Object getDataDashboardBarChart(string ledTypeSlotId) {


            /*
             * //getLastMonthName
            List<string> months = new List<string>();
            months.Add("January");
            months.Add("February");
            months.Add("March");
            months.Add("April");
            months.Add("May");
            months.Add("June");
            months.Add("July");
            */
            List<string> months = SystemClass.getLastMonthName(6); 

            //############################################# datasets OK.
            List<int> okDatas = new List<int>();
            okDatas.Add(generateNumber(minDataTest, maxDataTest));
            okDatas.Add(generateNumber(minDataTest, maxDataTest));
            okDatas.Add(generateNumber(minDataTest, maxDataTest));
            okDatas.Add(generateNumber(minDataTest, maxDataTest));
            okDatas.Add(generateNumber(minDataTest, maxDataTest));
            okDatas.Add(generateNumber(minDataTest, maxDataTest));

            M_Dashboard_Barchart mDashboardOk = new M_Dashboard_Barchart();
            mDashboardOk.data = okDatas; 
            mDashboardOk.label = "OK" ;
            mDashboardOk.borderColor = "rgba(0, 123, 255, 0.9)" ;
            mDashboardOk.borderWidth = "0";
            mDashboardOk.backgroundColor = "rgba(0, 123, 255, 0.5)";
            //############################################# datasets NG.

            List<int> ngDatas = new List<int>();
            ngDatas.Add(generateNumber(minDataTest, maxDataTest));
            ngDatas.Add(generateNumber(minDataTest, maxDataTest));
            ngDatas.Add(generateNumber(minDataTest, maxDataTest));
            ngDatas.Add(generateNumber(minDataTest, maxDataTest));
            ngDatas.Add(generateNumber(minDataTest, maxDataTest));
            ngDatas.Add(generateNumber(minDataTest, maxDataTest));

            M_Dashboard_Barchart mDashboardNg = new M_Dashboard_Barchart();
            mDashboardNg.data = ngDatas;
            mDashboardNg.label = "NG";
            mDashboardNg.borderColor = "rgba(0,0,0,0.09)";
            mDashboardNg.borderWidth = "0";
            mDashboardNg.backgroundColor = "rgba(0,0,0,0.07)";


            List<Object> lists = new List<object>();
            lists.Add(mDashboardOk);
            lists.Add(mDashboardNg);

            Dictionary<string, Object> dataReturn = new Dictionary<string, object>();
            dataReturn.Add("labels", months);
            dataReturn.Add("datasets", lists);


            /*             
                labels: ["January", "February", "March", "April", "May", "June", "July"],
                datasets: [
                            {
                                label: "My First dataset",
                                data: [65, 59, 80, 81, 56, 55, 40],
                                borderColor: "rgba(0, 123, 255, 0.9)",
                                borderWidth: "0",
                                backgroundColor: "rgba(0, 123, 255, 0.5)"
                            },
                            {
                                label: "My Second dataset",
                                data: [28, 48, 40, 19, 86, 27, 90],
                                borderColor: "rgba(0,0,0,0.09)",
                                borderWidth: "0",
                                backgroundColor: "rgba(0,0,0,0.07)"
                            }
                            ]
             */


            return dataReturn; 
        }



    }
}