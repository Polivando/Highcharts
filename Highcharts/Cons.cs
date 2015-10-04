using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Chrome;
using System.IO;

namespace Highcharts
{
    public static class Cons
    {
        public static IWebDriver driver;

        //public static readonly string path = "http://www.highcharts.com/samples/view.php?path=highcharts/demo/combo-timeline";
        public static readonly string path = "http://www.highcharts.com/component/content/article/2-articles/news/146-highcharts-5th-anniversary";
        public static string output = Directory.GetCurrentDirectory() + "\\log.txt";

        public static void log(string text)
        {
            string[] arr = new String[] { text };
            File.AppendAllLines(output, arr);
        }
    }
}
