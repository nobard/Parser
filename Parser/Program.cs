using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var parsers = new List<ParserBase>
            {
                new BakeevoparkParser(),
                new BiryuzovayaParser()
            };

            parsers.ForEach(parser =>
            {
                var apps = parser.Parse()
                    .OrderBy(x => x.Building)
                    .ThenBy(x => x.Floor)
                    .ThenBy(x => x.Rooms)
                    .ThenBy(x => x.FlatNum)
                    .ToList();

                CreateAppsFile(parser.ParserName, apps);
            });
        }

        private static void CreateAppsFile(string name, List<Apartment> apps)
        {
            var valuesString = "FlatId | FlatNum | Building | Floor | Area | Rooms | Price | SPrice | Status | Date";
            var b = apps.Select(x => x.ToString()).ToList().Prepend(valuesString);

            File.WriteAllLines($"{name}.txt", b);
        }
    }
}
