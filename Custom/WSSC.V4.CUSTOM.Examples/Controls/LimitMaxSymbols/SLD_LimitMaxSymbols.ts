declare var ListForm: IListForm;

declare var SLD_LimitMaxSymbols_JSObject: JSInstanceObject;

declare var $: any;


function SLD_LimitMaxSymbols_Init() {
    var maxSymbols: FieldsMaxSymbolsSetter = new FieldsMaxSymbolsSetter();
    maxSymbols.AffixMaxSymbols(SLD_LimitMaxSymbols_JSObject.FieldsInfo);
}

class JSInstanceObject {
    public readonly FieldsInfo: JSFieldInfo[];
}

class JSFieldInfo {
    public readonly Key: string;
    public readonly Value: number;
}

class FieldsMaxSymbolsSetter {
    /**
     * Проставляет максимальное количетсво символов
     */
    public AffixMaxSymbols(fieldsInfo: JSFieldInfo[]): void {
        for (var index = 0; index < fieldsInfo.length; index++) {
            var fieldname: string = fieldsInfo[index].Key;
            var maxchars: number = fieldsInfo[index].Value;

            var field: IField = ListForm.GetField(fieldname, true);

            if (field.TypedField == null)
                throw new Error(`Не удалось получить типизированное поле для ${fieldname}`);

            this.SetTypedFieldMaxLength(field, fieldname, maxchars);
        }
    }

    /**
    * Проствляет атрибут максимальная длинна у полей
    * @param field
    * @param fieldName
    * @param maxChars
    */
    public SetTypedFieldMaxLength(field, fieldName: string, maxChars: number): void {
        switch (field.Type) {
            case 'DBFieldText':
                $(`#${field.ContainerID}`).find(`#${field.TypedField.ContainerID}`).find('.txt_input').attr('maxlength', maxChars);
                break;

            case 'DBFieldMultiLineText':
                $(`#${field.ContainerID}`).find(`#${field.TypedField.ControlID}`).attr('maxlength', maxChars);
                break;

            default:
                throw new Error(`Поле ${fieldName} не поддерживается`);
        }
    }
}