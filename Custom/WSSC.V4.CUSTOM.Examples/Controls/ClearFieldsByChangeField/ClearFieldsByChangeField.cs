using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WSSC.V4.SYS.DBFramework;
using System.Xml.Linq;

namespace WSSC.V4.DMS.FOS.Controls.ClearFieldsByChangeField
{
    /// <summary>
    /// Очищение нескольких полей при изменении ключевого поля.
    /// </summary>
	public class ClearFieldsByChangeField : DBListFormWebControl
    {
        protected ClearFieldsByChangeField(DBListFormWebControlMetadata metadata, DBListFormControl listForm)
            : base(metadata, listForm) { }

        protected class Factory : DBListFormWebControlFactory
        {
            protected override DBListFormWebControl CreateListFormWebControl(DBListFormWebControlMetadata metadata, DBListFormControl listForm)
            {
                return new ClearFieldsByChangeField(metadata, listForm);
            }
        }

        private KeyValuePair<string, string[]> _Fields;
        private bool __init_Fields = false;
        /// <summary>
        /// Key - основное поле, Value - зависящие поля
        /// </summary>
        public KeyValuePair<string, string[]> Fields
        {
            get
            {
                if (!__init_Fields)
                {
                    _Fields = this.GetFieldAddition();
                    __init_Fields = true;
                }
                return _Fields;
            }
        }

        private KeyValuePair<string, string[]> GetFieldAddition()
        {
            Setting setting = new Setting(this.Item);
            XDocument doc = setting.GetXmlSetting();
            List<XElement> listsSetting = setting.GetListsSetting(doc);
            XElement currentListSetting = setting.GetCurrentListSetting(listsSetting);
            return setting.GetFieldAddition(currentListSetting).First();
        }

        protected override void OnListFormInitCompleted()
        {
            this.AddFieldChangeHandler(this.Fields.Key, "FOS_ClearFieldsByChangeField_Handler");
            this.AppContext.ScriptManager.RegisterResource(@"Controls\ClearFieldsByChangeField\FOS_ClearFieldsByChangeField.js", VersionProvider.ModulePath);
        }

        protected override string ClientInitHandler => "FOS_ClearFieldsByChangeField_Init";

        //Создаём options в js содержащий основное поле и зависимые
        [DataContract]
        private class JSInstanceObject
        {
            [DataMember]
            public string MainField { get; set; }

            [DataMember]
            public string[] ChildFields { get; set; }

            public JSInstanceObject(string mainField, string[] childFields)
            {
                this.MainField = mainField;
                this.ChildFields = childFields;
            }
        }

        protected override object CreateClientInstance()
        {
            return new JSInstanceObject(this.Fields.Key, this.Fields.Value);
        }

        protected override string ClientInstanceName => "FOS_ClearFieldsByChangeField_JSObject";
    }
}