declare namespace FOS_ClearFieldsByChangeField_JSObject {
    var MainField: string;
    var ChildFields: string[];
}

function FOS_ClearFieldsByChangeField_Init() {
    //Проверка поля
    var mainField: IField = ListForm.GetField(FOS_ClearFieldsByChangeField_JSObject.MainField);
    if (mainField == null)
        throw new Error(`Не найдено поле ${FOS_ClearFieldsByChangeField_JSObject.MainField}`);

    var fieldValue: FieldValueSeter = new FieldValueSeter(FOS_ClearFieldsByChangeField_JSObject.ChildFields);
    fieldValue.CheckField();
}

class FieldValueSeter {
    private _childs: Array<string>;

    public SetFieldValue(): void {
        for (var i = 0; i < this._childs.length; i++) {

            var child: IField = ListForm.GetField(this._childs[i], true);
            var value: any = child.GetValue();

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
    }

    public CheckField(): void {
        //не поддерживаемые поля
        var notSupportedFields: string[] = ['MSLField', 'DBFieldMarkup', 'PDField', 'CMField', 'PField', 'DBFieldFiles', 'DBFieldFileLink', 'DBFieldLink', 'WSSC.V4.DMS.Fields.TableItems.TableItemsField'];

        //Проверка полей
        for (var i = 0; i < this._childs.length; i++) {
            var child: IField = ListForm.GetField(this._childs[i], true);

            if (notSupportedFields.indexOf(child.Type) != -1)
                throw new Error(`Тип поля ${child.Type} не поддерживается`);
        }
    }

    public constructor(Childs: Array<string>) {
        this._childs = Childs;
    }
}

//обработчик полей
function FOS_ClearFieldsByChangeField_Handler() {
    var fieldValue: FieldValueSeter = new FieldValueSeter(FOS_ClearFieldsByChangeField_JSObject.ChildFields);
    fieldValue.SetFieldValue();
}