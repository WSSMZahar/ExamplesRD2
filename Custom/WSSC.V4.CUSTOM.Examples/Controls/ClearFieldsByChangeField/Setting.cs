using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WSSC.V4.CUSTOM.Examples;
using WSSC.V4.SYS.DBFramework;

namespace WSSC.V4.DMS.FOS.Controls.ClearFieldsByChangeField
{
	public class Setting
	{
		private readonly DBItem _item;
		public Setting(DBItem item)
		{
			_item = item
				?? throw new Exception("Не удалось получить карточку.");
		}


		private bool __init_XmlDocument = false;
		private XDocument _XmlDocument;
		private XDocument XmlDocument 
		{
			get 
			{
				if (!__init_XmlDocument)
				{
					_XmlDocument = this.GetXmlSetting();
					__init_XmlDocument = true;
				}
				return _XmlDocument;
			}
		}
		private XDocument GetXmlSetting()
		{
			return _item.Site.ConfigParams.GetXDocument(Consts.ConfigParams.ClearFieldsByChangeField)
				   ?? throw new Exception($"Не удалось получить настройку '{Consts.ConfigParams.ClearFieldsByChangeField}'");
		}

		private bool __init_ListsSetting = false;
		private List<XElement> _ListsSetting;
		private List<XElement> ListsSetting 
		{ 
			get
			{
				if (!__init_ListsSetting)
				{
					_ListsSetting = this.GetListsSetting(XmlDocument);
					__init_ListsSetting = true;
				}
				return _ListsSetting;
			}
		}
		private List<XElement> GetListsSetting(XDocument xDoc)
		{
			List<XElement> listSetting = xDoc.Element("Setting")?.Elements("List")?.ToList();
			if (listSetting == null || listSetting.Count == 0)
				throw new Exception($"Ошибка в константе '{Consts.ConfigParams.ClearFieldsByChangeField}' - не удалось получить настройку списков");
			return listSetting;
		}

		private bool __init_CurrentListSetting = false;
		private XElement _CurrentListSetting;
		private XElement CurrentListSetting 
		{ 
			get
			{
				if (!__init_CurrentListSetting)
				{
					_CurrentListSetting = this.GetCurrentListSetting(ListsSetting);
					__init_CurrentListSetting = true;
				}
				return _CurrentListSetting;
			}
		}
		private XElement GetCurrentListSetting(List<XElement> elements)
		{
			IEnumerable<XElement> fields = elements.FirstOrDefault(IsCurrentListSettings)
				?.Elements("Field")
				?? throw new Exception($"Для списка '{_item.List.Name}' не найдена или некорректная настройка");

			if (fields.ToList().Count > 1)
				throw new Exception($"Для списка '{_item.List.Name}' указано несколько xml-узлов Field, а должен быть только один");
			return fields.First();
		}

		private bool IsCurrentListSettings(XElement el)
		{
			return string.Equals(el.Attribute("name")?.Value, _item.List.Name, StringComparison.OrdinalIgnoreCase)
				&& string.Equals(el.Attribute("web")?.Value?.Trim('/'),
				_item.List.Web.RelativeUrl.Trim('/'), StringComparison.OrdinalIgnoreCase);
		}


		/// <summary>
		/// Зависимость полей. Key = ключевое поле, Value[] = поля которые следует очищать
		/// Не может быть пустой. И не может быть менее 1.
		/// </summary>
		private bool __init_FieldAddition = false;
		private KeyValuePair<string, string[]> _FieldAddition;
		public KeyValuePair<string, string[]> FieldAddition 
		{
			get
			{
				if (!__init_FieldAddition)
				{
					_FieldAddition = this.GetFieldAddition(CurrentListSetting);
					__init_FieldAddition = true;
				}
				return _FieldAddition;
			}			 
		}
		private KeyValuePair<string, string[]> GetFieldAddition(XElement element)
		{
			string key = element.Attribute("parent")?.Value;
			if (string.IsNullOrEmpty(key))
				throw new Exception($"Не заполнен атрибут parent в настройке '{Consts.ConfigParams.ClearFieldsByChangeField}': '{element.ToString()}'");

			//Получаем значения разделяя их через ; и очищая от лишних пробелов
			string[] values = element
				.Attribute("childs")?
				.Value?
				.Split(new char[] { ';' })
				.Select(value => value.TrimStart(' ').TrimEnd(' '))
				.Where(value => !string.IsNullOrEmpty(value))
				.ToArray();

			if (values.Length < 1)
				throw new Exception($"Не заполнен корректно атрибут childs в настройке '{Consts.ConfigParams.ClearFieldsByChangeField}': '{element.ToString()}'");

			return new KeyValuePair<string, string[]>(key, values);
		}
	}
}