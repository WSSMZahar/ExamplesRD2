declare var ListForm: IListForm;

interface IListForm {
    GetField(fieldName: string): IField;
    GetField(fieldName: string, throwNotFoundException: boolean): IField;
}

interface LookupValue {
    LookupID: number;
}

interface IField {
    GetValue(): any;
    SetValue(value: any): void;
}