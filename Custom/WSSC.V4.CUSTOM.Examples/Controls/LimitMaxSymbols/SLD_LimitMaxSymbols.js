function SLD_LimitMaxSymbols_Init() {
    var maxSymbols = new FieldsMaxSymbolsSetter();
    maxSymbols.AffixMaxSymbols(SLD_LimitMaxSymbols_JSObject.FieldsInfo);
}
var JSInstanceObject = /** @class */ (function () {
    function JSInstanceObject() {
    }
    return JSInstanceObject;
}());
var JSFieldInfo = /** @class */ (function () {
    function JSFieldInfo() {
    }
    return JSFieldInfo;
}());
var FieldsMaxSymbolsSetter = /** @class */ (function () {
    function FieldsMaxSymbolsSetter() {
    }
    /**
     * Проставляет максимальное количетсво символов
     */
    FieldsMaxSymbolsSetter.prototype.AffixMaxSymbols = function (fieldsInfo) {
        for (var index = 0; index < fieldsInfo.length; index++) {
            var fieldname = fieldsInfo[index].Key;
            var maxchars = fieldsInfo[index].Value;
            var field = ListForm.GetField(fieldname, true);
            if (field.TypedField == null)
                throw new Error("\u041D\u0435 \u0443\u0434\u0430\u043B\u043E\u0441\u044C \u043F\u043E\u043B\u0443\u0447\u0438\u0442\u044C \u0442\u0438\u043F\u0438\u0437\u0438\u0440\u043E\u0432\u0430\u043D\u043D\u043E\u0435 \u043F\u043E\u043B\u0435 \u0434\u043B\u044F " + fieldname);
            this.SetTypedFieldMaxLength(field, fieldname, maxchars);
        }
    };
    /**
    * Проствляет атрибут максимальная длинна у полей
    * @param field
    * @param fieldName
    * @param maxChars
    */
    FieldsMaxSymbolsSetter.prototype.SetTypedFieldMaxLength = function (field, fieldName, maxChars) {
        switch (field.Type) {
            case 'DBFieldText':
                $("#" + field.ContainerID).find("#" + field.TypedField.ContainerID).find('.txt_input').attr('maxlength', maxChars);
                break;
            case 'DBFieldMultiLineText':
                $("#" + field.ContainerID).find("#" + field.TypedField.ControlID).attr('maxlength', maxChars);
                break;
            default:
                throw new Error("\u041F\u043E\u043B\u0435 " + fieldName + " \u043D\u0435 \u043F\u043E\u0434\u0434\u0435\u0440\u0436\u0438\u0432\u0430\u0435\u0442\u0441\u044F");
        }
    };
    return FieldsMaxSymbolsSetter;
}());
//# sourceMappingURL=SLD_LimitMaxSymbols.js.map