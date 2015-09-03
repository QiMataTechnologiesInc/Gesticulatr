using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using IoTApp.Drivers;
using IoTApp.Drivers.Models;
using Microsoft.AspNet.SignalR.Client;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IoTApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ServoDriver _servoDriver;
        private LedDriver _ledDriver;
        private readonly HubConnection _connection;
        private readonly IHubProxy _hubProxy;

        public MainPage()
        {
            this.InitializeComponent();
            _servoDriver = new ServoDriver(18, new ServoPulseModel
            {
                ForwardPulseWidth = 2,
                BackwardPulseWidth = 1,
                PulseFrequency = 20
            });
            _ledDriver = new LedDriver(4);
            _connection = new HubConnection("http://ipwebapp.azurewebsites.net/");
            _hubProxy = _connection.CreateHubProxy("bandHub");
            _hubProxy.On<string>("sendData", x =>
             {
                 if (x == "left")
                 {
                     _servoDriver.Clockwise();
                 }
                 else if (x == "right")
                 {
                     _servoDriver.CounterClockwise();
                 }
             });
            _connection.Start().Wait();
        }

        private void Button_Click_Left(object sender, RoutedEventArgs e)
        {
            _servoDriver.Clockwise();
        }

        private void Button_Click_Right(object sender, RoutedEventArgs e)
        {
            _servoDriver.CounterClockwise();
        }

        private void Button_Click_Stop(object sender, RoutedEventArgs e)
        {
            _servoDriver.StopServo();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            _ledDriver.Toggle();
        }
    }
}
