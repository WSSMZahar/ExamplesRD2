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
            this.AppContext.ScriptManager.RegisterResource("Controls/SetFieldController/SetFieldController.js", VersionProvider.ModulePath);
            this.AddFieldChangeHandler(Consts.SetFieldControllerHandler.ControllerFieldName, "FOS_SetFieldController");            
        }

        /// <summary>
        /// Событие открытия формы
        /// </summary>
        protected override string ClientInitHandler
        {
            get
            {
                return "FOS_SetFieldController_Init";
            }
        }
    }
}
