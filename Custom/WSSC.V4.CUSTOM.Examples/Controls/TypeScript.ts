﻿declare var ListForm: FormClass;

declare class FormClass {
    GetField(fieldName: string): DBField;
    GetField(fieldName: string, isNull: boolean): DBField;
}

declare class DBField {
    ID: number;
    Name: string;
    GetValue(): any;
    SetValue(any): void;
    Type: string;
    ContainerID: number;
    TypedField: any;
}