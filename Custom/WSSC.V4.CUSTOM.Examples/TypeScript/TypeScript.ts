declare var ListForm: IListForm;

interface IListForm {
    GetField(fieldName: string);
    GetField(fieldName: string, throwNotFoundException: boolean);
}

interface LookupValue {
    LookupID: number;
}