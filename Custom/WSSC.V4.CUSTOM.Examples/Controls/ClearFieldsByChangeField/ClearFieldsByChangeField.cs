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
            : base(metadata, listForm)
        {
            FieldsAddition = GetFieldAddition();
        }

        protected class Factory : DBListFormWebControlFactory
        {
            protected override DBListFormWebControl CreateListFormWebControl(DBListFormWebControlMetadata metadata, DBListFormControl listForm)
            {
                return new ClearFieldsByChangeField(metadata, listForm);
            }
        }

        public KeyValuePair<string, string[]> FieldsAddition { get; set; }

        private KeyValuePair<string, string[]> GetFieldAddition()
        {
            Setting setting = new Setting(Item);
            return setting.FieldAddition.First();
        }

        protected override void OnListFormInitCompleted()
        {
            if (!Item.IsNewOrContextCreated)
            {
                AddFieldChangeHandler(FieldsAddition.Key, "FOS_ClearFieldsByChangeField_Handler");
                AppContext.ScriptManager.RegisterResource(@"Controls\ClearFieldsByChangeField\FOS_ClearFieldsByChangeField.js", VersionProvider.ModulePath);
            }
        }

        protected override string ClientInitHandler
        {
            get
            {
                if (!Item.IsNewOrContextCreated)
                    return "FOS_ClearFieldsByChangeField_Init";

                return null;
            }
        }

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
                MainField = mainField;
                ChildFields = childFields;
            }
        }

        protected override object CreateClientInstance()
        {
            return new JSInstanceObject(FieldsAddition.Key, FieldsAddition.Value);
        }

        protected override string ClientInstanceName => "FOS_ClearFieldsByChangeField_JSObject";
    }
}