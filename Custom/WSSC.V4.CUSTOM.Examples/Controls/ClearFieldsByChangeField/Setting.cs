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
        private readonly DBItem Item;
        public Setting(DBItem item) => this.Item = item
            ?? throw new Exception("Не удалось получить карточку.");

        public XDocument GetXmlSetting()
        {
            return this.Item.Site.ConfigParams.GetXDocument(Consts.ConfigParams.ClearFieldsByChangeField)
                   ?? throw new Exception($"Не удалось получить настройку '{Consts.ConfigParams.ClearFieldsByChangeField}'");
        }

        public List<XElement> GetListsSetting(XDocument xDoc)
        {
            List<XElement> listSetting = xDoc.Element("Setting")?.Elements("List")?.ToList();
            if (listSetting == null || listSetting.Count == 0)
                throw new Exception($"Ошибка в константе '{Consts.ConfigParams.ClearFieldsByChangeField}' - не удалось получить настройку списков");
            return listSetting;
        }

        public XElement GetCurrentListSetting(List<XElement> elements)
        {
            IEnumerable<XElement> fields = elements.FirstOrDefault(IsCurrentListSettings)
                ?.Elements("Field")
                ?? throw new Exception($"Для списка '{this.Item.List.Name}' не найдена или некорректная настройка");

            if (fields.ToList().Count > 1)
                throw new Exception($"Для списка '{this.Item.List.Name}' указано несколько xml-узлов Field, а должен быть только один");
            return fields.First();
        }

        private bool IsCurrentListSettings(XElement el)
        {
            return string.Equals(el.Attribute("name")?.Value, this.Item.List.Name, StringComparison.OrdinalIgnoreCase)
                && string.Equals(el.Attribute("web")?.Value?.Trim('/'),
                this.Item.List.Web.RelativeUrl.Trim('/'), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Зависимость полей. Key = ключевое поле, Value[] = поля которые следует очищать
        /// Не может быть пустой. И не может быть менее 1.
        /// </summary>
        public Dictionary<string, string[]> GetFieldAddition(XElement element)
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

            return new Dictionary<string, string[]>
            {
                { key, values }
            };
        }
    }
}
