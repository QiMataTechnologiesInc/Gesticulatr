using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebApp
{
    public class BandHub : Hub
    {
        private double _y;

        public void UpdateData(double y)
        {
            var moveY = string.Empty;

            if (y > _y)
                moveY = "right";
            else
                moveY = "left";

            _y = y;

            Clients.All.sendData(moveY);
        }
    }
}