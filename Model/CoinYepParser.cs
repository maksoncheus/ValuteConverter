using System;
using System.Collections.Generic;
using System.Data;

namespace ValuteConverter
{
    internal static class CoinYepParser
    {
        public static IEnumerable<ValuteEntry> GetValuteEntries(DataTable table)
        {
            List<ValuteEntry> res = new()
            {
                new ValuteEntry("Российский рубль", "RUB", 1)
            };
            foreach (DataRow row in table.Rows)
            {
                res.Add(new ValuteEntry(row["Vname"].ToString().Trim(), row["Vchcode"].ToString().Trim(), Convert.ToDouble(row["Vcurs"])));
            }
            return res;
        }
    }
}
