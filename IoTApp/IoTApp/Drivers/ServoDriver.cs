using System;
using System.Diagnostics;
using Windows.Devices.Gpio;
using Windows.Foundation;
using IoTApp.Drivers.Models;

namespace IoTApp.Drivers
{
    public class ServoDriver : IDisposable
    {
        private GpioPin _servoPin;
        private ServoPulseModel _servoPulseModel;
        private IAsyncAction _servoBackgroundTask;
        private bool _servoOn;
        private long _currentPulseWidth = -1;

        public ServoDriver(int servoPin, ServoPulseModel servoPulseModel)
            : this(GpioController.GetDefault().OpenPin(servoPin),servoPulseModel)
        {
        }

        public ServoDriver(GpioPin servoPin, ServoPulseModel servoPulseModel)
        {
            _servoPin = servoPin;
            _servoPin.SetDriveMode(GpioPinDriveMode.Output);
            _servoPulseModel = servoPulseModel;
            _servoBackgroundTask = Windows.System.Threading.ThreadPool.RunAsync(this.ServoBackgrounTask, Windows.System.Threading.WorkItemPriority.High);
        }

        public void Clockwise()
        {
            _currentPulseWidth = _servoPulseModel.ForwardPulseWidth;
        }

        public void CounterClockwise()
        {
            _currentPulseWidth = _servoPulseModel.BackwardPulseWidth;
        }

        public void StopServo()
        {
            _currentPulseWidth = -1;
        }

        private void ServoBackgrounTask(IAsyncAction action)
        {
            //This motor thread runs on a high priority task and loops forever to pulse the motor as determined by the drive buttons
            while (true)
            {
                if (_currentPulseWidth >= 0)
                {
                    //If a button is pressed the pulsewidth is changed to cause the motor to spin in the appropriate direction
                    //Write the pin high for the appropriate length of time
                    if (_currentPulseWidth != 0)
                    {
                        _servoPin.Write(GpioPinValue.High);
                    }
                    //Use the wait helper method to wait for the length of the pulse
                    Wait(_currentPulseWidth);
                    //The pulse if over and so set the pin to low and then wait until it's time for the next pulse
                    _servoPin.Write(GpioPinValue.Low);
                    Wait(_servoPulseModel.PulseFrequency - _currentPulseWidth);
                }
            }
        }

        //A synchronous wait is used to avoid yielding the thread 
        //This method calculates the number of CPU ticks will elapse in the specified time and spins
        //in a loop until that threshold is hit. This allows for very precise timing.
        private void Wait(double milliseconds)
        {
            double desiredTicks = milliseconds / 1000 * Stopwatch.Frequency;
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.ElapsedTicks < desiredTicks)
            {

            }
        }

        public void Dispose()
        {
            _servoBackgroundTask.Cancel();
        }
    }
}
