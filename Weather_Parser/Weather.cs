using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ForXml
{
    [Serializable]
    public class Weather
    {
        public ArrayList Time = new ArrayList();
        public ArrayList Cloud = new ArrayList();
        public ArrayList MinTemp = new ArrayList();
        public ArrayList MaxTemp = new ArrayList();
        public ArrayList Wind = new ArrayList();
        public ArrayList Humid = new ArrayList();

        public void WeatherEngine()
        {
            string url = "http://api.pogoda.com/index.php?api_lang=ru&localidad=13088&affiliate_id=4v7j6at7rkya&v=2";
            XmlTextReader Xdoc = new XmlTextReader(url);
            bool flag = false;
            int count = 0;
            DateTime time = DateTime.Now;
            while (Xdoc.Read())
            {
                if (Xdoc.Name.Contains("day") && time.ToString("yyyyMMdd") == Xdoc.GetAttribute("value") ||
                        Xdoc.Name.Contains("day") && time.AddDays(1).ToString("yyyyMMdd") == Xdoc.GetAttribute("value") ||
                            Xdoc.Name.Contains("day") && time.AddDays(2).ToString("yyyyMMdd") == Xdoc.GetAttribute("value"))
                {
                    if (count == 0)
                    {
                        Console.WriteLine(time.ToShortDateString() + ": ");
                        Time.Add(time.ToShortDateString() + ": ");
                    }
                    if (count == 1)
                    {
                        Console.WriteLine(time.AddDays(1).ToString("dd.MM.yyyy" + ": "));
                        Time.Add(time.AddDays(1).ToString("dd.MM.yyyy" + ": "));
                    }
                    if (count == 2)
                    {
                        Console.WriteLine(time.AddDays(2).ToString("dd.MM.yyyy" + ": "));
                        Time.Add(time.AddDays(2).ToString("dd.MM.yyyy" + ": "));
                    }
                    flag = false;
                    while (Xdoc.Read())
                    {
                        if (Xdoc.Name.Contains("symbol"))
                        {
                            Console.WriteLine("Cloud cover: " + Xdoc.GetAttribute("desc"));
                            Cloud.Add("Cloud cover: " + Xdoc.GetAttribute("desc"));
                        }
                        if (Xdoc.Name.Contains("tempmin"))
                        {
                            Console.WriteLine("Minimum temperature: " + Xdoc.GetAttribute("value") + Xdoc.GetAttribute("unit"));
                            MinTemp.Add("Minimum temperature: " + Xdoc.GetAttribute("value") + Xdoc.GetAttribute("unit"));
                        }
                        if (Xdoc.Name.Contains("tempmax"))
                        {
                            Console.WriteLine("Maximum temperature: " + Xdoc.GetAttribute("value") + Xdoc.GetAttribute("unit"));
                            MaxTemp.Add("Maximum temperature: " + Xdoc.GetAttribute("value") + Xdoc.GetAttribute("unit"));
                        }
                        if (Xdoc.Name == "wind")
                        {
                            Console.WriteLine("Wind: " + Xdoc.GetAttribute("value") + Xdoc.GetAttribute("unit"));
                            Wind.Add("Wind: " + Xdoc.GetAttribute("value") + Xdoc.GetAttribute("unit"));
                        }
                        if (Xdoc.Name.Contains("humidity"))
                        {
                            Console.WriteLine("Humidity: " + Xdoc.GetAttribute("value") + "\n");
                            Humid.Add("Humidity: " + Xdoc.GetAttribute("value") + "\n");
                        }
                        if (Xdoc.Name == "local_info")
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag == true)
                    {
                        Xdoc.MoveToNextAttribute();
                        count++;
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            Weather tttWeather = new Weather();
            tttWeather.WeatherEngine();
            XmlSerializer sw = new XmlSerializer(typeof(Weather));
            var file = File.Create(@"C:\Users\Alice\Documents\Visual Studio 2015\Projects C#\Weather_Parser\Weather.txt");
            sw.Serialize(file, tttWeather);
            file.Close();
        }
    }
}