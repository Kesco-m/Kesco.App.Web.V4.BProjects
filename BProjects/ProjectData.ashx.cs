using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web;
using Kesco.Lib.BaseExtention;
using Kesco.Lib.BaseExtention.Enums.Docs;
using Kesco.Lib.DALC;
using Kesco.Lib.Web.Controls.V4.TreeView;
using Kesco.Lib.Web.Settings;
using Kesco.Lib.Web.Settings.Parameters;
using SQLQueries = Kesco.Lib.Entities.SQLQueries;

namespace Kesco.App.Web.V4.BProjects
{
    /// <summary>
    ///     Обработчик клиентских запросов для справочника бизнес-проектов
    /// </summary>
    public sealed class ProjectData : TreeViewDataHandler
    {
        /// <summary>
        ///     Разрешает обработку веб-запросов НТТР для пользовательского элемента HttpHandler,
        ///     который реализует интерфейс System.Web.IHttpHandler
        /// </summary>
        /// <param name="context">
        ///     Объект System.Web.HttpContext, предоставляющий ссылки на внутренние серверные
        ///     объекты (например, Request, Response, Session и Server), используемые для обслуживания HTTP-запросов
        /// </param>
        public override void ProcessRequest(HttpContext context)
        {
            DbSourceSettings = new TreeViewDbSourceSettings
            {
                TableName = "БизнесПроекты",
                ViewName = "vwБизнесПроекты",
                ConnectionString = Config.DS_person,
                PkField = "КодБизнесПроекта",
                NameField = "БизнесПроект",
                PathField = "БизнесПроектыPath1",
                RootName = "Бизнес-проекты"
            };

            var type = context.Request.QueryString["type"];
            if (type == "get_state")
            {
                var clid = context.Request.QueryString["Clid"];
                var paramName = context.Request.QueryString["ParamName"];
                var parametersManager = new AppParamsManager(Convert.ToInt32(clid), new StringCollection {paramName});
                var appParam = parametersManager.Params.Find(p => p.Name == paramName);


                if (null != appParam) context.Response.Write(appParam.Value);
            }
            else if (type == "save_state")
            {
                var clid = context.Request.QueryString["Clid"];
                var paramName = context.Request.QueryString["ParamName"];
                var state = context.Request.Form["state"];
                var parametersManager = new AppParamsManager(Convert.ToInt32(clid), new StringCollection());
                parametersManager.Params.Add(new AppParameter(paramName, state, AppParamType.SavedWithClid));
                parametersManager.SaveParams();
            }
            else if (type == "create_state")
            {
                var loadById = context.Request.QueryString["loadid"];
                var sqlParams = new Dictionary<string, object> {{"@id", loadById}};
                var dt = DBManager.GetData(SQLQueries.SELECT_Родители, Config.DS_person, CommandType.Text, sqlParams);
                var openStrNodeList = "";

                foreach (DataRow row in dt.Rows)
                {
                    if (openStrNodeList != "") openStrNodeList += ",";
                    openStrNodeList += "\"" + row["КодБизнесПроекта"] + "\"";
                }

                var state = "{\"core\":{\"open\":[" + openStrNodeList +
                            "],\"loaded\":[],\"scroll\":{\"left\":0,\"top\":0},\"selected\":[\"" + loadById +
                            "\"]}}";

                context.Response.Write(state);
            }
            else
            {
                base.ProcessRequest(context);
            }
        }

        /// <summary>
        ///     Получение строки запроса по переданным параметрам
        /// </summary>
        /// <param name="orderBy">Порядок сортировки</param>
        /// <param name="searchText">Строка поиска</param>
        /// <param name="searchParam">Параметры поиска</param>
        /// <param name="stateLoad">State</param>
        /// <returns></returns>
        protected override string GetTreedata_Sql(string orderBy = "L", string searchText = "", string searchParam = "",
            string openList = "")
        {
            if (orderBy != "L") orderBy = DbSourceSettings.NameField;

            if (searchText.IsNullEmptyOrZero())
            {
                if (openList.IsNullEmptyOrZero())
                    return string.Format(SQLQueries.SELECT_БизнесПроектыДанныеДляДерева, orderBy);

                return string.Format(SQLQueries.SELECT_БизнесПроектыДанныеДляДерева_State, orderBy, openList);
            }

            searchText = searchParam == "1" ? searchText + "%" : "%" + searchText + "%";
            return string.Format(SQLQueries.SELECT_БизнесПроектыДанныеДляДерева_Фильтр, orderBy, searchText);
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

    /// <summary>
    ///     Класс состояния дерева
    /// </summary>
    internal class TreeViewState
    {
        internal List<Core> core { get; set; }
    }

    /// <summary>
    ///     Сущности состояния
    /// </summary>
    public class Core
    {
        public List<string> loaded;
        public List<string> open;
        public List<Scroll> scroll;
        public List<string> selected;
    }

    /// <summary>
    ///     Позиция дерева
    /// </summary>
    public class Scroll
    {
        public int left;
        public List<string> scroll;
        public int top;
    }
}