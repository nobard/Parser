using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;

namespace Parser
{
    public class BiryuzovayaParser : ParserBase
    {
        public override string ParserName { get; protected set; }
        public override HttpWebRequest Request { get; protected set; }
        public override string Response { get; protected set; }
        public override List<Apartment> Apartments { get; protected set; }
        protected override string URL { get; set; }
        protected override string Page { get; set; }

        public BiryuzovayaParser()
        {
            ParserName = "Бирюзовая Жемчужина";
            URL = "https://2.ac-biryuzovaya-zhemchuzhina.ru";
            Page = "/flats/all?floor=&type=&status=&minArea=&maxArea=&minPrice=&maxPrice=";
            Response = GetResponse();
            Apartments = new List<Apartment>();
        }

        public override List<Apartment> Parse()
        {
            Response.SplitBetweenStrings("<tr style=\"background:", "</tr>")
                .ToList()
                .ForEach(x => Apartments.Add(ParseElement(ParseTable(x))));

            return Apartments;
        }

        private List<string> ParseTable(string table) 
            => table.SplitBetweenStrings("<td>", "/td>").ToList();

        private Apartment ParseElement(List<string> element)
        {
            var flatId = GetStringBetween(element[0], "href=\"http://2.ac-biryuzovaya-zhemchuzhina.ru/flats/", "\"");

            int? flatNum = null;
            if (int.TryParse(GetStringBetween(element[5], "\">", "<"), out var num)) flatNum = num;

            var building = "--";
            var floor = int.Parse(GetStringBetween(element[4], "\">", "<"));
            var area = float.Parse(GetStringBetween(element[1], "<td>", "<"), CultureInfo.InvariantCulture);

            var roomsString = GetStringBetween(element[0], "\">", "<");
            var rooms = roomsString == "Студия" ? 0 : int.Parse(roomsString.Substring(0, 1));

            var price = int.Parse((GetStringBetween(element[2], "\">", "руб.<") ?? "0").Replace(" ", ""));
            var status = GetStringBetween(element[3], "<small>", "</small>") ?? "В продаже";
            var date = DateTime.Now.ToString("yyyy-MM-dd");

            return new Apartment(flatId, flatNum, building, floor, area, rooms, price, price / area, status, date);
        }

        //protected override string GetResponse()
        //{
        //    IWebDriver driver = new EdgeDriver();
        //    driver.Url = URL;
        //    ClickButton(driver, "//a[contains(.,'Выбрать квартиру')]");
        //    ClickButton(driver, "//select[contains(@name,'status')]");


        //    return "";
        //}

        //private void ClickButton(IWebDriver dr, string xPath)
        //{
        //    while (true)
        //    {
        //        try
        //        {
        //            dr.FindElement(By.XPath(xPath)).Click();
        //            break;
        //        }
        //        catch (Exception)
        //        {
        //        }
        //    }
        //}
    }
}