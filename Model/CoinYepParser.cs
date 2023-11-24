using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValuteConverter
{
    internal static class CoinYepParser
    {
        public static IEnumerable<ValuteEntry> GetValuteEntries(DataTable table)
        {
            List<ValuteEntry> res = new List<ValuteEntry>
            {
                new ValuteEntry("Российский рубль", "RUB", 1)
            };
            foreach (DataRow row in table.Rows)
            {
                res.Add(new ValuteEntry(row["Vname"].ToString(), row["Vchcode"].ToString(), Convert.ToDouble(row["Vcurs"])));
            }
            return res;
        }
    }
}
