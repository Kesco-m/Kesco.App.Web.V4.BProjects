using Kesco.Lib.BaseExtention;
using Kesco.Lib.Web.Controls.V4.TreeView;
using System.Data;
using System.Text;
using SQLQueries = Kesco.Lib.Entities.SQLQueries;

namespace Kesco.App.Web.V4.BProjects
{
    /// <summary>
    ///     Обработчик клиентских запросов для справочника бизнес-проектов
    /// </summary>
    public sealed class ProjectData : TreeViewDataHandler
    {
        /// <summary>
        ///     Получение строки запроса по переданным параметрам
        /// </summary>
        /// <param name="orderByField">Поле сортировки</param>
        /// <param name="orderByDirection">Направление сортировки</param>
        /// <param name="searchText">Строка поиска</param>
        /// <param name="searchParam">Параметры поиска</param>
        /// <param name="openList">Список открытых нодов</param>
        /// <returns></returns>
        protected override string GetTreeData_Sql(string orderByField = "L", string orderByDirection = "ASC", 
            string searchText = "", string searchParam = "", string openList = "")
        {
            string orderBy = orderByField + " " + orderByDirection;

            if (string.IsNullOrEmpty(searchText))
            {
                if (openList.IsNullEmptyOrZero())
                    return string.Format(SQLQueries.SELECT_БизнесПроектыДанныеДляДерева, 
                        orderBy
                        );

                return string.Format(SQLQueries.SELECT_БизнесПроектыДанныеДляДерева_State, 
                    orderBy, 
                    openList
                    );
            }

            searchText = searchParam == "1" ? searchText + "%" : "%" + searchText + "%";

            return string.Format(SQLQueries.SELECT_БизнесПроектыДанныеДляДерева_Фильтр,
                orderBy,
                searchText,
                ShowTopNodesInSearchResult ? 1 : 0
                );
        }

        /// <summary>
        ///     Вывод иконок до названия
        /// </summary>
        /// <param name="dt">DataRow</param>
        /// <returns>возвращаямая строка, содержащая готовую разметку</returns>
        protected override string GetPrefixIcon(DataRow dt)
        {
            var iconsString = new StringBuilder();

            // при коде возврата равным 1, рисуем кнопки выбора напротив каждого узла 
            if (ReturnId == "1")
                iconsString.Append(
                    string.Format(
                        "<img style='border:none' src='/styles/BackToList.gif' title='Выбрать значение' onclick=\"v4_returnValue({0},'{1}');\">&nbsp;",
                        dt["id"], dt["text"]));

            return iconsString.ToString();
        }

        /// <summary>
        ///     Вывод иконок до названия
        /// </summary>
        /// <param name="dt">DataRowView</param>
        /// <returns>возвращаямая строка, содержащая готовую разметку</returns>
        protected override string GetPrefixIcon(DataRowView dt)
        {
            return GetPrefixIcon(dt.Row);
        }
    }
}