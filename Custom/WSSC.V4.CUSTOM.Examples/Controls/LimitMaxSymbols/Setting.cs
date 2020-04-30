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
		public Setting(DBItem item)
		{
			_item = item 
				?? throw new Exception($"В настройку '{GetType().FullName}' не передана карточка");
		}

		private bool __init__XmlDocument = false;
		private XDocument _XmlDocument;
		private XDocument XmlDocument
		{
			get
			{
				if (!__init__XmlDocument)
				{
					_XmlDocument = _item.Site.ConfigParams.GetXDocument(Consts.ConfigParams.LimitMaxSymbols)
						?? throw new Exception($"Не удалось получить настройку из константы '{Consts.ConfigParams.LimitMaxSymbols}'");
					__init__XmlDocument = true;
				}
				return _XmlDocument;
			}
		}

		private bool __init_ListSettings = false;
		private XElement _ListSettings;
		private XElement ListSettings
		{
			get
			{
				if (!__init_ListSettings)
				{
					_ListSettings = this.GetSettingsList(XmlDocument);
					__init_ListSettings = true;
				}
				return _ListSettings;
			}
		}

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

		private bool __init_Fields = false;
		private IEnumerable<XElement> _Fields;
		private IEnumerable<XElement> Fields
		{
			get
			{
				if (!__init_Fields)
				{
					_Fields = this.GetFields(ListSettings);
					__init_FieldsInfo = true;
				}
				return _Fields;
			}
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


		private bool __init_FieldsInfo = false;
		private Dictionary<string, int> _FieldsInfo;
		/// <summary>
		/// Название поля : макс символов
		/// </summary>
		public Dictionary<string, int> FieldsInfo
		{
			get
			{
				if (!__init_FieldsInfo)
				{
					_FieldsInfo = this.GetFieldsInfo(Fields);
					__init_FieldsInfo = true;
				}
				return _FieldsInfo;
			}
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