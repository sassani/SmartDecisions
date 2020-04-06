using System.Collections.Generic;
using System.Reflection;

namespace Helpers
{
	public static class Mapper
	{

		public static List<PropertyInfo> MapDbModelToClassModel(object clsModel, object dbModel, object? mapperItems = null)
		{
			int missedPropCount = 0;
			List<PropertyInfo> missedProps = new List<PropertyInfo>();
			foreach (var clsProp in clsModel.GetType().GetProperties())
			{
				var dbProp = dbModel.GetType().GetProperty(clsProp.Name);
				if (dbProp != null)
				{
					clsProp.SetValue(clsModel, dbProp.GetValue(dbModel, null));
				}
				else
				{
					missedPropCount++;
					missedProps.Add(clsProp);
				}
			}
			return missedProps;
		}
	}
}
