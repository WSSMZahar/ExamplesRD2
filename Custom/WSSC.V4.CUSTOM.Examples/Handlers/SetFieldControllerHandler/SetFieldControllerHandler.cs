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
            if (!item.ContextCreated)
                return;

            bool onController = item.GetValue<bool>(Consts.SetFieldControllerHandler.OnСontrollerFieldName);
            DBFieldLookupValue controller = item.GetLookupValue(Consts.SetFieldControllerHandler.ControllerFieldName);
            bool isHaveValue = controller != null;

            //Если "Контролер" пусто и  "На контроле" false заново ставить false не нужно, с true аналогично
            if (!isHaveValue && !onController || (isHaveValue && onController))
                return;

            item.SetValue(Consts.SetFieldControllerHandler.OnСontrollerFieldName, isHaveValue);
        }
    }
}