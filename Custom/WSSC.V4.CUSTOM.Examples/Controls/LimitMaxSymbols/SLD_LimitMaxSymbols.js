function SLD_LimitMaxSymbols_Init() {
    var maxSymbols = new MaxSymbols();
    maxSymbols.ChekMaxSymbols(SLD_LimitMaxSymbols_JSObject.FieldsInfo);
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
var MaxSymbols = /** @class */ (function () {
    function MaxSymbols() {
    }
    MaxSymbols.prototype.ChekMaxSymbols = function (fieldsInfo) {
        for (var index = 0; index < fieldsInfo.length; index++) {
            var fieldname = fieldsInfo[index].Key;
            var maxchars = fieldsInfo[index].Value;
            var field = ListForm.GetField(fieldname, true);
            if (field.TypedField == null)
                throw new Error('Не удалось получить типизированное поле для ' + fieldname);
            this.GetTypeField(field, fieldname, maxchars);
        }
    };
    MaxSymbols.prototype.GetTypeField = function (field, fieldName, maxChars) {
        switch (field.Type) {
            case 'DBFieldText':
                $('#' + field.ContainerID).find('#' + field.TypedField.ContainerID).find('.txt_input').attr('maxlength', maxChars);
                break;
            case 'DBFieldMultiLineText':
                $('#' + field.ContainerID).find('#' + field.TypedField.ControlID).attr('maxlength', maxChars);
                break;
            default:
                throw new Error('Поле ' + fieldName + ' не поддерживается');
        }
    };
    return MaxSymbols;
}());
//# sourceMappingURL=SLD_LimitMaxSymbols.js.map