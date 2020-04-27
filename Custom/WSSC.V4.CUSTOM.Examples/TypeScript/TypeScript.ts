declare var ListForm: IListForm;

interface IListForm {
    GetField(fieldName: string): IField;
    GetField(fieldName: string, isNull: boolean): IField;
}

interface IField {
    GetValue(): any;
    SetValue(value: any): any;
    TypedField: any;
    Type: any;
}

