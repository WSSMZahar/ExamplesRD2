declare var ListForm: FormClass;

interface FormClass {
    GetField(fieldName: string);
    GetField(fieldName: string, isNull: boolean);
}