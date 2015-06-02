
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

        private string device = null;
        public bool stop = false;
        public event AudioChangedEvent OnAudioChangedEvent;

        public void Run()
        {
            while (!stop)
            {
                var enumerator = new MMDeviceEnumerator();
                var d = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                Console.WriteLine(d.DeviceFriendlyName);
                var friendlyName = d.DeviceFriendlyName;
                if (device == null)
                {
                    device = d.DeviceFriendlyName;
                }
                else
                {
                    if (device.Equals(friendlyName))
                    {
                        Console.WriteLine("Device has not changed...");
                    }
                    else
                    {
                        OnAudioChangedEvent(this, new AudioChangedEventArgs("Device has changed"));
                        stop = true;
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