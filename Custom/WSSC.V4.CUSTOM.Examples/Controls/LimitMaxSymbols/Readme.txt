﻿Постановка.
При заполнении поля «КПП»/ «КПП поставщика» необходимо ограничить количество вводимых символов до 9. 
Настройку названия поля (в зависимости от списка) вынести в константу.

Настройка.
1. В константы системы добавить элемент с «Системное название» «SLD.LimitMaxSymbols» и заполнить поле «Значение»:
<Settings>

  <Setting listname="Contracts" webname="dms/contracts">
    <Field name="Том" maxchars="9" />
    <Field name="Регистрационный номер" maxchars="8" />      
  </Setting>

</Settings>

Где Setting содержит информацию о списке:
listname - имя списка
Webname – имя узла

Field содержит информацию о поле:
name – имя поля
Maxchars – максимальное количество символов

2. В настройки списка на форму редактирования добавить контрол 
«Контрол для ограничения количества символов в текстовых полях»
(WSSC.V4.DMS.SLD.Controls.LimitMaxSymbols.LimitMaxSymbols)

