using Windows.Devices.Gpio;

namespace IoTApp.Drivers
{
    public class LedDriver
    {
        private GpioPin _ledPin;

        public LedDriver(int ledPin)
        {
            _ledPin = GpioController.GetDefault().OpenPin(ledPin);
        }

        public LedDriver(GpioPin ledPin)
        {
            _ledPin = ledPin;
        }

        public bool IsOn()
        {
            return _ledPin.Read() == GpioPinValue.High;
        }

        public bool IsOff()
        {
            return !IsOn();
        }

        public void Toggle()
        {
            if (IsOn())
            {
                Off();
            }
            else
            {
                On();
            }
        }

        public void On()
        {
            _ledPin.Write(GpioPinValue.Low);
        }

        public void Off()
        {
            _ledPin.Write(GpioPinValue.High);
        }
    }
}
