using System;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ValuteConverter
{
    /// <summary>
    /// Class for SOAP request to www.cbr.ru
    /// </summary>
    internal static class SoapRequest
    {
        /// <summary>
        /// Wrapper for <see cref="M:ValuteConverter.SoapRequest.GetCursOnDate(System.DateTime)"/> to asynchronous call
        /// </summary>
        /// <param name="dateTime">Parameter for getting exchange rates for a given <see cref="System.DateTime"/></param>
        /// <returns>An asynchronous operation that can return a value</returns>
        public static async Task<DataTable> GetCursOnDateAsync(DateTime dateTime)
        {
            return await Task.Run(() => GetCursOnDate(dateTime));
        }
        /// <summary>
        /// Get exchange rates from <see href="https://cbr.ru/"/>
        /// </summary>
        /// <param name="dateTime">Parameter for getting exchange rates for a given <see cref="System.DateTime"/></param>
        /// <returns>DataTable with exchange rates</returns>
        public static DataTable GetCursOnDate(DateTime dateTime)
        {
            string requestText = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
  <soap:Body>
    <GetCursOnDate xmlns=""http://web.cbr.ru/"">
      <On_date>" + dateTime.ToString("yyyy-MM-ddThh:mm:ss") + @"</On_date>
    </GetCursOnDate>
  </soap:Body>
</soap:Envelope>";
            byte[] postData = Encoding.UTF8.GetBytes(requestText);

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://www.cbr.ru/DailyInfoWebServ/DailyInfo.asmx");
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.ContentLength = postData.Length;
            request.Headers.Add("SOAPAction", "http://web.cbr.ru/GetCursOnDate");

            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.WriteAsync(postData, 0, postData.Length);
            }

            DataSet ds = new DataSet("CursValute");
            try
            {
                WebResponse response = request.GetResponseAsync().Result;
                using (Stream responseStream = response.GetResponseStream())
                using (XmlReader xmlReader = XmlReader.Create(responseStream))
                {
                    xmlReader.ReadToFollowing("schema", "http://www.w3.org/2001/XMLSchema");
                    ds.ReadXmlSchema(xmlReader);
                    ds.ReadXml(xmlReader);
                }
            }
            catch (Exception ex)
            {

            }
            return ds?.Tables[0] ?? new DataTable();
        }
    }
}
