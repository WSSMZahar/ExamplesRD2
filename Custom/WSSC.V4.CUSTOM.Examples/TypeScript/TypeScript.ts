declare var ListForm: FormClass;

interface FormClass {
    GetField(fieldName: string): DBField;
    GetField(fieldName: string, isNull: boolean): DBField;
}

declare class DBField {
    ID: number;
    Name: string;
    GetValue(): any;
    SetValue(any: any): void;
}

class LookupValue {
    LookupID: number;
}