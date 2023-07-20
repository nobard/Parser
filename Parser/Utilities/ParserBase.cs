using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    public abstract class ParserBase
    {
        public abstract string ParserName { get; protected set; }
        public abstract HttpWebRequest Request { get; protected set; }
        public abstract string Response { get; protected set; }
        protected abstract string URL { get; set; }
        protected abstract string Page { get; set; }

        protected string FullURL
        {
            get => $"{URL}{Page}";
        }
        public abstract List<Apartment> Apartments { get; protected set; }

        public abstract List<Apartment> Parse();

        protected virtual string GetResponse()
        {
            Request = (HttpWebRequest)WebRequest.Create(FullURL);
            Request.Method = "Get";

            try
            {
                HttpWebResponse response = (HttpWebResponse)Request.GetResponse();

                var stream = response.GetResponseStream();

                if (stream != null) return new StreamReader(stream).ReadToEnd();
            }
            catch (Exception)
            {
            }

            return null;
        }

        protected string GetStringBetween(string str, string startString, string endString)
        {
            if(!str.Contains(startString)) return null;

            var startIndex = str.IndexOf(startString) + startString.Length;
            var endIndex = str.IndexOf(endString, startIndex);

            if (startIndex == -1 || endIndex == -1) return string.Empty;

            return str.Substring(startIndex, endIndex - startIndex).Trim();
        }
    }
}