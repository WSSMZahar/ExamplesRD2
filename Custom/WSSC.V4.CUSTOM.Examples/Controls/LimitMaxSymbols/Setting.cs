using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WSSC.V4.CUSTOM.Examples;
using WSSC.V4.SYS.DBFramework;

namespace WSSC.V4.DMS.SLD.Controls.LimitMaxSymbols
{
	public class Setting
	{
		private DBItem _item;
		private XDocument _xmlDocument;
		public Setting(DBItem item)
		{
			_item = item ?? throw new Exception($"В настройку '{GetType().FullName}' не передана карточка");

			_xmlDocument = _item.Site.ConfigParams.GetXDocument(Consts.ConfigParams.LimitMaxSymbols)
				?? throw new Exception($"Не удалось получить настройку из константы '{Consts.ConfigParams.LimitMaxSymbols}'");

			XElement listSettings = GetSettingsList(_xmlDocument);
			IEnumerable<XElement> fields = GetFields(listSettings);
			FieldsInfo = GetFieldsInfo(fields);
		}

		public Dictionary<string, int> FieldsInfo { get; private set; }

		/// <summary>
		/// Настройка для текущего листа (определяется по карточке)
		/// </summary>
		private XElement GetSettingsList(XDocument xDoc)
		{
			return xDoc.Element("Settings")?
				.Elements("Setting")
				.FirstOrDefault(IsCurrentSettingsList)
					?? throw new Exception($"В настройке '{Consts.ConfigParams.LimitMaxSymbols}' для списка '{_item.List.Name}' не удалось найти xml-узел");
		}

		private bool IsCurrentSettingsList(XElement el)
		{
			return string.Equals(el.Attribute("listname")?.Value, _item.List.Name, StringComparison.OrdinalIgnoreCase)
				&& string.Equals(el.Attribute("webname")?.Value?.Trim('/'), _item.List.Web.RelativeUrl.Trim('/'),
				StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		/// Xml-узлы с полями для текущего списка
		/// </summary>
		private IEnumerable<XElement> GetFields(XElement element)
		{
			IEnumerable<XElement> thisFields = element.Elements("Field");
			if (thisFields.Count() == 0)
				throw new Exception($"В настройке '{Consts.ConfigParams.LimitMaxSymbols}' нет xml-узлов 'Field'");
			return thisFields;
		}

		/// <summary>
		/// Название поля : макс символов
		/// </summary>
		private Dictionary<string, int> GetFieldsInfo(IEnumerable<XElement> elements)
		{
			return elements.ToDictionary(el =>
				{
					string fieldname = el.Attribute("name")?.Value;
					if (string.IsNullOrEmpty(fieldname))
						throw new Exception($"В '{Consts.ConfigParams.LimitMaxSymbols}' в '{el.ToString()}' в атрибуте 'name' не указано поле");

					return fieldname;
				},
				el =>
				{
					string maxchars = el.Attribute("maxchars")?.Value;
					if (string.IsNullOrEmpty(maxchars))
						throw new Exception($"В '{Consts.ConfigParams.LimitMaxSymbols}' в '{el.ToString()}' в атрибуте 'maxchars' не указано макс символов");

					if (!int.TryParse(maxchars, out int max) || max < 1)
						throw new Exception($"В '{Consts.ConfigParams.LimitMaxSymbols}' в '{el.ToString()}' в атрибуте 'maxchars' некорректные данные");

					return max;
				}
			);
		}
	}
}