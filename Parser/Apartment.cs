using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public class Apartment
    {
        public string FlatId { get; set; }
        public int? FlatNum { get; set; }
        public string Building { get; set; }
        public int Floor { get; set; }
        public float Area { get; set; }
        public int Rooms { get; set; }
        public int Price { get; set; }
        public float SPrice { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }

        public Apartment(string flatId, int? flatNum, string building, int floor, float area, int rooms, int price, float sPrice, string status, string date)
        {
            FlatId = flatId;
            FlatNum = flatNum;
            Building = building;
            Floor = floor;
            Area = area;
            Rooms = rooms;
            Price = price;
            SPrice = sPrice;
            Status = status;
            Date = date;
        }

        public override string ToString() 
            => $"{FlatId} | {FlatNum} | {Building} | {Floor} | {Area} | {Rooms} | {Price} | {string.Format("{0:0.00}", SPrice)} | {Status} | {Date}";
    }
}