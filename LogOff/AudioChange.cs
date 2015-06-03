
using NAudio.CoreAudioApi;
using System;
using System.Threading;

namespace LogOff
{
    public delegate void AudioChangedEvent(object source, AudioChangedEventArgs e);

    public class AudioChangedEventArgs : EventArgs
    {
        private string EventInfo;
        public AudioChangedEventArgs(string Text)
        {
            EventInfo = Text;
        }
        public string GetInfo()
        {
            return EventInfo;
        }
    }

    public class CheckAudioChanged
    {

        private string device = "-1";
        private string connectedDevice = "0";
        private string unConnectedDevice = "1";
        public bool stop = false;
        public event AudioChangedEvent OnAudioChangedEvent;

        public void Run()
        {
            while (!stop)
            {
                var enumerator = new MMDeviceEnumerator();
                var d = new MMDeviceEnumerator().GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
                for (var i = 0; i < d.Properties.Count; i++)
                {
                    try
                    {
                        if (d.Properties[i].Key.formatId.Equals(new Guid("3ba0cd54-830f-4551-a6eb-f3eab68e3700")))
                        {
                            var newDevice = d.Properties[i].Value.ToString();
                            if (newDevice.Equals(unConnectedDevice))
                            {
                                if (device.Equals(connectedDevice))
                                {
                                    Console.WriteLine("Device has changed state...");
                                    OnAudioChangedEvent(this, new AudioChangedEventArgs("Device has changed"));
                                    stop = true;
                                }
                                else
                                {
                                    Console.WriteLine("Device has not changed...");
                                }
                            }
                            else
                            {
                                device = newDevice;
                                Console.WriteLine("Device is connected...");
                            }
                        }
                    }
                    catch {
                        Console.WriteLine("Failed to retrieve key...");
                    }
                }
                Thread.Sleep(100);
            }
        }

        public void Stop()
        {
            stop = true;
        }

    }
}