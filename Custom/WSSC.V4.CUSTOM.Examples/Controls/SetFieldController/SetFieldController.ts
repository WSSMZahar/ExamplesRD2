FOS_ControllerFieldName = "Контролер";
FOS_OnControlFieldName = "На контроле";

function FOS_SetFieldController() {
    var controller = ListForm.GetField(FOS_ControllerFieldName, true);
    if (controller == null)
        return;

    var onControl = ListForm.GetField(FOS_OnControlFieldName, true);
    if (onControl == null)
        return;

    var controllerValue: LookupValue = controller.GetValue();

    //если контролер не проставлен то "На контроле" ставим "Нет", иначе "Да"
    var value = controllerValue == null || controllerValue.LookupID != 0;
    onControl.SetValue(value);
}

function FOS_SetFieldController_Init() {
    FOS_SetFieldController();
}