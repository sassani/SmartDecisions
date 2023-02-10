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

		/// <summary>
		/// Map dto object to an entity
		/// entity must has same properties in dto
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="dto"></param>
		public static void PatchEntityByDto(object entity, object dto)
		{
			foreach (var item in dto.GetType().GetProperties())
			{
				var itemValue = dto.GetType().GetProperty(item.Name)!.GetValue(dto);
				if (itemValue != null && item.PropertyType.Namespace != "System.Collections.Generic")
				{
					entity.GetType().GetProperty(item.Name)!.SetValue(entity, itemValue);
				}
			}
		}
	}
}
