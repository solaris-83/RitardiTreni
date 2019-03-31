using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace Ritardi_treni.Helpers
{
    public class DisplayAttributeBasedObjectDataProvider : ObjectDataProvider
    {
        public object GetEnumValues(Enum enumObj)
        {
            var attribute = enumObj.GetType().GetRuntimeField(enumObj.ToString()).
                GetCustomAttributes(typeof(DisplayAttribute), false).SingleOrDefault() as DisplayAttribute;
            return attribute == null ? enumObj.ToString() : attribute.Description;
        }
         
        public List<object> GetShortListOfApplicationStations(Type type)
        {
            var shortListOfApplicationStations = Enum.GetValues(type).OfType<Enum>().Select(GetEnumValues).ToList();
            return
                shortListOfApplicationStations;
        }
    }
}
