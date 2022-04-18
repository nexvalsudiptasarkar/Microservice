using ObjectExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Shared.Data
{
	public partial class DAL
	{
		public static PropertyInfo[] GetEntityProperties(Type type, BulkInsertType bulkType)
		{
			PropertyInfo[] properties = type.GetProperties();
			switch (bulkType)
			{
				case BulkInsertType.DBInsertAttribute :
					properties = type.GetProperties().Where(prop => Attribute.IsDefined(prop, typeof(CanDbInsertAttribute))).ToArray();
					break;

				case BulkInsertType.HaveSetMethod :
					properties = type.GetProperties().Where(prop => prop.GetSetMethod() != null).ToArray();
					break;
			}
			return properties;
		}
		public static PropertyInfo[] GetEntityProperties<T>(T entity, BulkInsertType bulkType)
		{
			PropertyInfo[] properties = entity.GetType().GetProperties();
			properties = DAL.GetEntityProperties(properties, bulkType);
			return properties;
		}
		public static PropertyInfo[] GetEntityProperties(PropertyInfo[] properties, BulkInsertType bulkType)
		{
			switch (bulkType)
			{
				case BulkInsertType.ALL:
					break;

				case BulkInsertType.DBInsertAttribute:
					properties = properties.Where(prop => Attribute.IsDefined(prop, typeof(CanDbInsertAttribute))).ToArray();
					break;

				case BulkInsertType.HaveSetMethod:
					properties = properties.Where(prop => prop.SetMethod != null).ToArray();
					break;
			};
			return properties;
		}

		public static MySqlParameter[] MakeSqlParameters<T>(T entity, BulkInsertType bulkType, ParamPrecedence precedence = ParamPrecedence.None, string propertyName = "MySqlParameters")
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new InvalidOperationException("It makes no sense to specify a null propertyName. Ever.");
			}

			MySqlParameter[] parameters = null;
			PropertyInfo[] properties = entity.GetType().GetProperties();

			PropertyInfo sqlParams = sqlParams = properties.FirstOrDefault(x => x.PropertyType.Name == "MySqlParameter[]" && x.Name == propertyName);
			if (sqlParams != null && precedence != ParamPrecedence.UseBulkType)
			{
				parameters = (MySqlParameter[])sqlParams.GetValue(entity);
			}
			else
			{
				List<MySqlParameter> list = new List<MySqlParameter>();
				properties = DAL.GetEntityProperties(properties, bulkType);
				foreach(PropertyInfo property in properties)
				{
					list.Add(new MySqlParameter(string.Format("@{0}", property.Name), property.GetValue(entity)));	
				}
				parameters = list.ToArray();
			}

			Global.WriteLine("-----------------------------"); 
			if (properties.Length == 0)
			{
				Global.WriteLine("No properties found.");
			}
			else
			{
				int length = parameters.Max(x=>x.ParameterName.Length) + 1;
				string format = string.Concat("    {0,-", length.ToString(), "}{1}");

				Global.WriteLine("Resulting parameters:");
				foreach(MySqlParameter item in parameters)
				{
					string text = string.Format(format, item.ParameterName, item.Value);
					Global.WriteLine(text);
				}
			}
			return parameters;
		}
	}
}
