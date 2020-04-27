function FOS_ClearFieldsByChangeField_Init() {
    //Проверка поля
    var mainField = ListForm.GetField(FOS_ClearFieldsByChangeField_JSObject.MainField);
    if (mainField == null)
        throw new Error("\u041D\u0435 \u043D\u0430\u0439\u0434\u0435\u043D\u043E \u043F\u043E\u043B\u0435 " + FOS_ClearFieldsByChangeField_JSObject.MainField);
    var fieldValue = new SetFieldsValue(FOS_ClearFieldsByChangeField_JSObject.ChildFields);
    fieldValue.CheckField();
}
var SetFieldsValue = /** @class */ (function () {
    function SetFieldsValue(Childs) {
        this._childs = Childs;
    }
    SetFieldsValue.prototype.SetFieldValue = function () {
        for (var i = 0; i < this._childs.length; i++) {
            var child = ListForm.GetField(this._childs[i], true);
            var value = child.GetValue();
            switch (child.Type) {
                //поля подстановки
                case 'DBFieldLookupSingle':
                    if (value != "" && value != null)
                        child.SetValue(value);
                    break;
                case 'DBFieldLookupMulti':
                    if (value.length > 0)
                        child.SetValue(value);
                    break;
                //остальные поля
                default:
                    if (value != "" && value != null)
                        child.SetValue(value);
                    break;
            }
        }
    };
    SetFieldsValue.prototype.CheckField = function () {
        //не поддерживаемые поля
        var notSupportedFields = ['MSLField', 'DBFieldMarkup', 'PDField', 'CMField', 'PField', 'DBFieldFiles', 'DBFieldFileLink', 'DBFieldLink', 'WSSC.V4.DMS.Fields.TableItems.TableItemsField'];
        //Проверка полей
        for (var i = 0; i < this._childs.length; i++) {
            var child = ListForm.GetField(this._childs[i], true);
            if (notSupportedFields.indexOf(child.Type) != -1)
                throw new Error("\u0422\u0438\u043F \u043F\u043E\u043B\u044F " + child.Type + " \u043D\u0435 \u043F\u043E\u0434\u0434\u0435\u0440\u0436\u0438\u0432\u0430\u0435\u0442\u0441\u044F");
        }
    };
    return SetFieldsValue;
}());
//обработчик полей
function FOS_ClearFieldsByChangeField_Handler() {
    var fieldValue = new SetFieldsValue(FOS_ClearFieldsByChangeField_JSObject.ChildFields);
    fieldValue.SetFieldValue();
}
//# sourceMappingURL=FOS_ClearFieldsByChangeField.js.map