using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebApp
{
    public class BandHub : Hub
    {
        public void UpdateData(double x, double y, double z)
        {
            var _x = x;
            var _y = y;
            var _z = z;

            Clients.All.sendData(x, y, z);
        }
    }
}