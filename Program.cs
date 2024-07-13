using System.Management;
using System.Media;
using NAudio;
using NAudio.Wave;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            float temperature = GetCPUTemperature();
            Console.WriteLine("Mevcut CPU Sıcaklığı: " + temperature + "°C");

            if (temperature > 51)
            {
                Console.WriteLine("sıcaklık kötü");
                PlayAlertSound();
            }

            Thread.Sleep(5000);
        }
    }

    private static float GetCPUTemperature()
    {
        float temperature = 0;
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM MSAcpi_ThermalZoneTemperature");

        foreach (ManagementObject queryObj in searcher.Get())
        {
            temperature = (Convert.ToSingle(queryObj["CurrentTemperature"].ToString()) - 2732) / 10.0f;
        }

        return temperature;
    }

    private static void PlayAlertSound()
    {
        using (var audioFile = new AudioFileReader("burning sound effect.mp4"))
        using (var outputDevice = new WaveOutEvent())
        {
            outputDevice.Init(audioFile);
            outputDevice.Play();
            while (outputDevice.PlaybackState == PlaybackState.Playing)
            {
                Thread.Sleep(1000);
            }
        }
    }
}