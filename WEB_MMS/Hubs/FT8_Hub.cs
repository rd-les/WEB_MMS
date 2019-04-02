using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WEB_MMS.Hubs {
    public class FT8_Hub : Hub {
        public void Hello() {
            Clients.All.hello();
        }


    }
}