using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using OpenQA.Selenium.Internal;
using OpenQA.Selenium.Support.PageObjects;


namespace Highcharts
{
    class ChartsPage
    {
        [FindsBy(How = How.XPath, Using = "//*[@id='hs-component']/div/div/p[2]/iframe")]
        public IWebElement innerFrame { get; set; }

        public IWebElement WrappedFrame { get; set; }

        public ChartsPage()
        {
            PageFactory.InitElements(Cons.driver,this);
            WrappedFrame = (innerFrame as IWrapsElement).WrappedElement;
        }

        //void FindGraphs()
        //{
        //    var temp = Cons.driver.FindElements(By.ClassName("highcharts-legend-item"));
        //    foreach (var item in temp) Graphs.Add(item);
        //}
        
    }
}
