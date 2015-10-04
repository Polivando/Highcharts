using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Highcharts
{
    class Chart
    {
        [FindsBy(How = How.XPath, Using = "//*[@class='highcharts-legend-item']//*[contains(text(),'Google')]")]
        public IWebElement GoogleControl { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@class='highcharts-legend-item']//*[contains(text(),'Revenue')]")]
        public IWebElement RevenueControl { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@class='highcharts-legend-item']//*[contains(text(),'employees')]")]
        public IWebElement EmployeesControl { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@class='highcharts-series' and @visibility='visible']")]
        public IWebElement EmployeesGraph { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@class='highcharts-tooltip' and @visibility='visible']")]
        public IWebElement Tooltip { get; set; }

        [FindsBy(How = How.ClassName, Using = "highcharts-series-group")]
        public IWebElement Graphs { get; set; }
                
        public List<List<int>> Dictionary {get; set;}
        

        public Chart()
        {
            PageFactory.InitElements(Cons.driver,this);
        }

        public Chart ToggleGraph(IWebElement graph)
        {
            graph.Click();
            return new Chart();
        }

        public string GetEmployeeMessage()
        {
            WebDriverWait wait = new WebDriverWait(Cons.driver, TimeSpan.FromSeconds(10));
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@class='highcharts-tooltip']//*[contains(text(),'employees')]")));
            return Tooltip.FindElement(By.XPath(".//*[contains(text(),'employees')]")).Text;
        }

        public List<int[]> ParsePath(IWebElement graph)
        {
            var temp = graph.FindElement(By.XPath(".//*[@fill='none' and 1]")).GetAttribute("d");
            temp = Regex.Replace(temp,"\\s[A-Z]\\s"," ");
            temp = Regex.Replace(temp, "[A-Z]\\s", "");
            temp = Regex.Replace(temp, "\\.", ",");
            var tempArr = temp.Split(' ');

            List<int[]> tempList=new List<int[]>();
            for (int i = 0; i < tempArr.Length; i += 4)
            {
                var aux = new int[2];
                double auxdouble;
                Double.TryParse(tempArr[i], out auxdouble);
                aux[0] = Convert.ToInt32(auxdouble);
                Double.TryParse(tempArr[i+1], out auxdouble);
                aux[1] = Convert.ToInt32(auxdouble);
                tempList.Add(aux);
            }
            return tempList;
        }

        public void moveTo(int x, int y)
        {
            int effectiveOffset = 0;
            int step = 1;
            int a = x > 0 ? step : -step;
            int b = y > 0 ? step : -step;
            
            while (effectiveOffset < Math.Abs(x))
            {
                var build = new Actions(Cons.driver);
                build.MoveByOffset(a, 0).ContextClick().Build().Perform();
                effectiveOffset += step;
            }
            effectiveOffset = 0;
            while (effectiveOffset < Math.Abs(y))
            {
                var build = new Actions(Cons.driver);
                build.MoveByOffset(0, b).ContextClick().Build().Perform();
                effectiveOffset += step;
            }            
        }

        public void moveTo(IWebElement item)
        {
            var build = new Actions(Cons.driver);
            build.MoveToElement(item, 0, 0).Build().Perform();
        }

        public void makeDictionary()
        {
            var size = Graphs.Size.Height;
            var quantityElem = Cons.driver.FindElements(By.XPath("//*[contains(@class,'highcharts-axis-labels') and contains(@class, 'highcharts-yaxis-labels')]/*"));
            int max = 0;
            int temp;
            foreach (var item in quantityElem)
            {
                temp = Convert.ToInt32(item.Text);
                if (temp > max)
                    max = temp;
            }
            var step = size / max;
            var res = new List<List<int>>(max+1);
            for (int i=0; i<res.Capacity; i++)
            {
                res.Add(new List<int>());
                res[i].Add(i);
                res[i].Add(i>0 ? res[i - 1][1] - step : size);
            }
            Dictionary = res;
        }

        public bool PositionCheck(string message, int position)
        {
            foreach (var item in Dictionary)
            {
                var text = Regex.Match(message, "\\d+").Value;
                if (item[0] == Convert.ToInt32(text))
                    if (position >= item[1] - 5 && position < item[1] + 3)
                        return true;
            }
            return false;
        }
    }
}