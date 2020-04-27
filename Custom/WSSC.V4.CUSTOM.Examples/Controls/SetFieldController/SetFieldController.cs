using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSSC.V4.CUSTOM.Examples;
using WSSC.V4.SYS.DBFramework;

namespace WSSC.V4.DMS.FOS
{
    /// <summary>
    /// Простановка значения поля "На контроле" по значению поля "Контролер"
    /// </summary>
    public class SetFieldController : DBListFormWebControl
    {
        private const string ControllerFieldName = "Контролер";

        protected SetFieldController(DBListFormWebControlMetadata metadata, DBListFormControl listForm)
            : base(metadata, listForm) { }

        protected class Factory : DBListFormWebControlFactory
        {
            protected override DBListFormWebControl CreateListFormWebControl(DBListFormWebControlMetadata metadata, DBListFormControl listForm)
            {
                return new SetFieldController(metadata, listForm);
            }
        }

        /// <summary>
        /// Добавление обработчиков на поля
        /// </summary>
        protected override void OnListFormInitCompleted()
        {
            if (!Item.IsNew && !Item.ContextCreated)
            {
                AppContext.ScriptManager.RegisterResource("Controls/SetFieldController/SetFieldController.js", CUSTOM.Examples.VersionProvider.ModulePath);
                AddFieldChangeHandler(ControllerFieldName, "FOS_SetFieldController");
            }
        }

        /// <summary>
        /// Событие открытия формы
        /// </summary>
        protected override string ClientInitHandler
        {
            get
            {
                if (!Item.IsNew && !Item.ContextCreated)
                    return "FOS_SetFieldController_Init";

                return null;
            }

        }
    }
}