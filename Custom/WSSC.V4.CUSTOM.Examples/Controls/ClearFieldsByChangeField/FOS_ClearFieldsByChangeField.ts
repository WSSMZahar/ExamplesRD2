﻿declare namespace FOS_ClearFieldsByChangeField_JSObject {
    var MainField: string;
    var ChildFields: string[];
}

function FOS_ClearFieldsByChangeField_Init() {
    //Проверка поля
    var mainField = ListForm.GetField(FOS_ClearFieldsByChangeField_JSObject.MainField);
    if (mainField == null)
        throw new Error('Не найдено поле ' + FOS_ClearFieldsByChangeField_JSObject.MainField);

    var fieldValue = new SetFieldsValue(FOS_ClearFieldsByChangeField_JSObject.ChildFields);
    fieldValue.CheckField();
}

class SetFieldsValue {
    childs: Array<string>;

    SetFieldValue(): void {
        for (var i = 0; i < this.childs.length; i++) {

            var child = ListForm.GetField(this.childs[i], true);
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
    }

    CheckField(): void {
        //не поддерживаемые поля
        var notSupportedFields: string[] = ['MSLField', 'DBFieldMarkup', 'PDField', 'CMField', 'PField', 'DBFieldFiles', 'DBFieldFileLink', 'DBFieldLink', 'WSSC.V4.DMS.Fields.TableItems.TableItemsField'];

        //Проверка полей

        for (var i = 0; i < this.childs.length; i++) {
            var child = ListForm.GetField(this.childs[i], true);

            if (notSupportedFields.indexOf(child.Type) != -1)
                throw new Error('Тип поля ' + child.Type + ' не поддерживается');
        }
    }

    constructor(Childs: Array<string>) {
        this.childs = Childs;
    }
}

//обработчик полей
function FOS_ClearFieldsByChangeField_Handler() {
    var fieldValue = new SetFieldsValue(FOS_ClearFieldsByChangeField_JSObject.ChildFields);
    fieldValue.SetFieldValue();
}