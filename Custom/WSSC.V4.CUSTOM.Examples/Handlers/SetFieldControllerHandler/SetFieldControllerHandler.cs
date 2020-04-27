using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSSC.V4.SYS.DBFramework;
using WSSC.V4.SYS.Fields.Lookup;
using WSSC.V4.CUSTOM.Examples;

namespace WSSC.V4.DMS.FOS
{
    public class SetFieldControllerHandler : DBItemHandler
    {
        private const string ControllerFieldName = "Контролер";
        private const string OnСontrollerFieldName = "На контроле";

        public SetFieldControllerHandler(DBItemHandlerDefinition handlerDefinition, DBList list)
            : base(handlerDefinition, list) { }

        protected override void OnBeforeItemUpdate(DBItemOperation operationProperties)
        {
            if (operationProperties == null)
                return;

            DBItem item = operationProperties.Item;
            if (item == null)
                return;

            //только при создании
            if (!item.IsNew && !item.ContextCreated)
                return;

            bool onController = item.GetValue<bool>(OnСontrollerFieldName);
            DBFieldLookupValue controller = item.GetLookupValue(ControllerFieldName);
            bool haveValue = controller != null;

            //Если "Контролер" пусто и  "На контроле" false заново ставить false не нужно, с true аналогично
            if (!haveValue && !onController || (haveValue && onController))
                return;

            item.SetValue(OnСontrollerFieldName, haveValue);
        }
    }
}