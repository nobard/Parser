using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class BakeevoparkParser : ParserBase
    {
        public override string ParserName { get; protected set; }
        protected override string URL { get; set; }
        public override string Response { get; protected set; }
        public override HttpWebRequest Request { get; protected set; }
        public override List<Apartment> Apartments { get; protected set; }
        protected override string Page { get; set; }

        public BakeevoparkParser()
        {
            ParserName = "Бакеево-Парк";
            URL = "http://bakeevopark.ru/";
            Response = GetResponse();
            Apartments = new List<Apartment>();
        }

        public override List<Apartment> Parse()
        {
            Response.SplitBetweenStrings("<div class=\"flat-item row", "<div class=\"moreinfo\"><a href=")
                .ToList()
                .ForEach(x =>
                {
                    if (!x.Contains("data[key]")) Apartments.Add(ParseElement(x));
                });

            return Apartments;
        }

        private Apartment ParseElement(string element)
        {
            var flatId = GetStringBetween(element, "<div class=\"flat-item row hidden-xs hidden-sm\" data-id=\"", "\"");

            int? flatNum = null;
            if (int.TryParse(GetStringBetween(element, "Квартира №", "<"), out var num)) flatNum = num;

            var building = GetStringBetween(element, "<p class=\"flat-location\">", " /");
            var floor = int.Parse(GetStringBetween(
                GetStringBetween(element, "<div class=\"flat-mobile-props-head\">Этаж</div>", "/div>"),
                "<div class=\"flat-mobile-props-val\">", "<"));
            var area = float.Parse(GetStringBetween(
                GetStringBetween(element, "<div class=\"flat-mobile-props-head\">Площадь</div>", "<sup>2</sup></div>"),
                "<div class=\"flat-mobile-props-val\">", " м"), CultureInfo.InvariantCulture);
            var rooms = int.Parse(GetStringBetween(element, "<div class=\"flat-item-name\" >", " - комнатная квартира") ?? "0");
            var price = int.Parse(GetStringBetween(element, "<div class=\"price\">", "руб.</div>").Replace(" ", ""));
            var status = GetStringBetween(element,
                $"<a href=\"#reserve-form\" class=\"btn-popup-round-inverse reserve-popup\" data-flat-id=\"{flatId}\">",
                "</a>") == "Забронировать" ? "В продаже" : "";
            var date = DateTime.Now.ToString("yyyy-MM-dd");

            return new Apartment(flatId, flatNum, building, floor, area, rooms, price, price / area, status, date);
        }
    }
}