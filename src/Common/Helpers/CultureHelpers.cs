using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;

namespace Tiveria.Common.Helpers
{
    public static class CultureHelpers
    {
        public static IEnumerable<Country> GetCountriesWithId()
        {
            return from ri in
                       from ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                       select new RegionInfo(ci.LCID)
                   group ri by ri.TwoLetterISORegionName into g
                   select new Country
                   {
                       CountryId = g.Key,
                       Title = g.First().DisplayName
                   };
        }

        public static IEnumerable<string> GetCountries()
        {
            return from ri in
                       from ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                       select new RegionInfo(ci.LCID)
                   group ri by ri.TwoLetterISORegionName into g
                   select g.First().DisplayName;
        }


        public class Country
        {
            public string CountryId { get; set; }
            public string Title { get; set; }
        }
    }
}
