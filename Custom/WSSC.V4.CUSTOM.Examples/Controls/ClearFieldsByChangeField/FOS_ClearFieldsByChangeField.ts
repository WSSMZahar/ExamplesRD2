﻿declare var ListForm: any;
declare namespace FOS_ClearFieldsByChangeField_JSObject {
    var MainField: string;
    var ChildFields: string[];
}

function FOS_ClearFieldsByChangeField_Init()
{
    //Проверка поля
    var mainField = ListForm.GetField(FOS_ClearFieldsByChangeField_JSObject.MainField);
    if (mainField == null)
        throw new Error('Не найдено поле ' + FOS_ClearFieldsByChangeField_JSObject.MainField);

    //не поддерживаемые поля
    var notSupportedFields: string[] = ['MSLField', 'DBFieldMarkup', 'PDField', 'CMField', 'PField', 'DBFieldFiles', 'DBFieldFileLink', 'DBFieldLink', 'WSSC.V4.DMS.Fields.TableItems.TableItemsField'];

    //Проверка полей
    var childs = FOS_ClearFieldsByChangeField_JSObject.ChildFields;
    for (var i = 0; i < childs.length; i++) {
        var child = ListForm.GetField(childs[i]);

        if (mainField == null)
            throw new Error('Не найдено поле ' + childs[i]);

        if (notSupportedFields.indexOf(child.Type) != -1)
            throw new Error('Тип поля ' + child.Type + ' не поддерживается');
    } 
}

class SetFieldsValue {
    childs: Array<string>;

    SetValue(): void {
        for (var i = 0; i < this.childs.length; i++) {

            var child = ListForm.GetField(this.childs[i]);
            var value = child.GetValue();

            switch (child.Type) {
                //поля подстановки
                case 'DBFieldLookupSingle':
                    if (value != "" && value != null)
                        child.SetValue();
                    break;

                case 'DBFieldLookupMulti':
                    if (value.length > 0)
                        child.SetValue();
                    break;

                //остальные поля
                default:
                    if (value != "" && value != null)
                        child.SetValue();
                    break;
            }
        }
    }

    constructor(Childs: Array<string>) {
        this.childs = Childs;
    }
}

//обработчик полей
function FOS_ClearFieldsByChangeField_Handler() {
    var fieldValue: SetFieldsValue = new SetFieldsValue(FOS_ClearFieldsByChangeField_JSObject.ChildFields);
    fieldValue.SetValue();
}