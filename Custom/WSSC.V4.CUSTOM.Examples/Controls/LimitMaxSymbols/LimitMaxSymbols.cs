using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using WSSC.V4.SYS.DBFramework;
using WSSC.V4.SYS.Fields.MultiLineText;
using WSSC.V4.SYS.Fields.Text;

namespace WSSC.V4.DMS.SLD.Controls.LimitMaxSymbols
{
    /// <summary>
    /// Устанавливаются ограничение количества символов в полях типа "Однострочный" или "Многострочный" текст.
    /// </summary>
	public class LimitMaxSymbols : DBListFormWebControl
    {
        protected LimitMaxSymbols(DBListFormWebControlMetadata metadata, DBListFormControl listForm)
            : base(metadata, listForm)
        {
            Setting = new Setting(this.Item);
        }

        protected class Factory : DBListFormWebControlFactory
        {
            protected override DBListFormWebControl CreateListFormWebControl(DBListFormWebControlMetadata metadata, DBListFormControl listForm)
            {
                return new LimitMaxSymbols(metadata, listForm);
            }
        }

        /// <summary>
        /// Настройка для текущего списка
        /// </summary>
        private Setting Setting { get; set; }

        /// <summary>
        /// Вызывается при инициализации формы, до инициализации полей.
        /// </summary>
        protected override void OnListFormInit()
        {
            this.AppContext.ScriptManager.RegisterResource(@"Controls\LimitMaxSymbols\SLD_LimitMaxSymbols.js", CUSTOM.Examples.VersionProvider.ModulePath);

            //Проверям каждое полученное поле на тип, является он текстовым или нет + наличие его в списке
            List<string> wrongFields = new List<string>();

            foreach (KeyValuePair<string, int> key in this.Setting.FieldsInfo)
            {
                DBField field = this.Item.List.GetField(key.Key, true);
                //Если поле не текстовое добавляем его к коллекции "ошибочных полей"
                if (field as DBFieldText == null && field as DBFieldMultiLineText == null /*&& field as DBSystemFieldText == null*/)
                    wrongFields.Add(key.Key);
            }

            //если есть поля которые не текстовые - выводим сообщения о них
            if (wrongFields.Count > 0)
                throw new Exception($"Данные поля не являются текстовыми: '{string.Join("; ", wrongFields.ToArray())}'");
        }

        protected override string ClientInitHandler => "SLD_LimitMaxSymbols_Init";

        //Создаём options в js содержащий основное поле и макс его длину

        protected override object CreateClientInstance()
        {
            return new JSInstanceObject(this.Setting.FieldsInfo);
        }

        protected override string ClientInstanceName => "SLD_LimitMaxSymbols_JSObject";
    }

    [DataContract]
    public class JSInstanceObject
    {
        [DataMember]
        Dictionary<string, int> FieldsInfo { get; set; }

        public JSInstanceObject(Dictionary<string, int> fieldsInfo)
        {
            this.FieldsInfo = fieldsInfo;
        }
    }

}