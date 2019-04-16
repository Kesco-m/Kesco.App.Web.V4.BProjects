//Глобальные переменные должны быть инициализированы во время загрузки страницы, обычно в методе Page_Load()
var callbackUrl;
var control;
var multiReturn;

var mvc = 0;
var domain = '';

$(document).ready(function () {
    $(function () {
        /* Установить способность изменять расположение inline элементов при отображении страницы в диалоге IE */
        v4_setResizableInDialog();

        /* Умещаем дерево в пределах окна при готовности страницы */
        v4_treeViewHandleResize('tvProject');
        $(window).resize(function () {
            /* Умещаем дерево в пределах окна при изменении размеров окна */
            v4_treeViewHandleResize('tvProject');
        });
    });
});