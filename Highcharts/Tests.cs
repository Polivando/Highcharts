using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Firefox;
using System.Threading;
using System.Timers;
using OpenQA.Selenium.Support.UI;

namespace Highcharts
{
    class Tests
    {
        static ChartsPage page;

        static Chart chart;

        void Initialize()
        {
            Cons.driver = new ChromeDriver();
            Cons.driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            Cons.driver.Navigate().GoToUrl(Cons.path);
            page = new ChartsPage();
            Cons.driver.SwitchTo().Frame(page.WrappedFrame);
            chart = new Chart();
        }
         
        void Clean()
        {
            Cons.driver.Close();
        }

        [Test]
        public void TestGreenGraph()
        {
            Cons.log(DateTime.Now+" executing test for green graph");
            Initialize();
            chart = chart.ToggleGraph(chart.GoogleControl).ToggleGraph(chart.RevenueControl);
            chart.makeDictionary();
            var graphPath = chart.ParsePath(chart.EmployeesGraph);
            //var offset = chart.ParseOffset(chart.EmployeesGraph);
            chart.moveTo(chart.Graphs);
            chart.moveTo(graphPath[0][0], graphPath[0][1]);
            for (int i = 0; i < graphPath.Count - 1; i++)
            {
                int offsetX=(graphPath[i + 1][0] - graphPath[i][0])/2-1;
                chart.moveTo(offsetX, 0);
                var message = chart.GetEmployeeMessage();
                if (!chart.PositionCheck(message, graphPath[i][1]))
                {
                    Cons.log("Position error at level " + message + "\nTest failed");
                    throw new Exception();
                }
                Cons.log(message);
                chart.moveTo(graphPath[i + 1][0] - graphPath[i][0] - offsetX, graphPath[i + 1][1] - graphPath[i][1]);
            }
            Cons.log("Test passed");
            Clean();
        }
    }
}
