using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LogOff
{
    public class Program
    {
        static void AudioEventDetected(object source, AudioChangedEventArgs e)
        {
            Console.WriteLine("We are going to logoff!");
            Console.WriteLine(e.GetInfo());
            var logoff = new LogoffWindows();
            logoff.Run();
        }

        static void Main(string[] args)
        {
            var checkAudioChanged = new CheckAudioChanged();
            checkAudioChanged.OnAudioChangedEvent += new AudioChangedEvent(AudioEventDetected);
            Thread thread = new Thread(new ThreadStart(checkAudioChanged.Run));
            thread.Start();
            thread.Join();
        }
    }
}
