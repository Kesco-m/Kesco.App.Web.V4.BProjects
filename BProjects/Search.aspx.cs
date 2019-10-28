using Kesco.Lib.BaseExtention;
using Kesco.Lib.Entities;
using Kesco.Lib.Entities.Persons.BusinessProject;
using Kesco.Lib.Web.Controls.V4.Common;
using Kesco.Lib.Web.Controls.V4.TreeView;
using Kesco.Lib.Web.Settings;
using System;
using System.Collections.Specialized;

namespace Kesco.App.Web.V4.BProjects
{
    /// <summary>
    ///     Форма поиска бизнес-проектов
    /// </summary>
    public partial class Search : Page
    {
        /// <summary>
        ///     Ссылка на справку
        /// </summary>
        public override string HelpUrl { get; set; }

        /// <summary>
        ///     Инициализирует новый экземпляр класса Search
        /// </summary>
        public Search()
        {
            HelpUrl = "hlp/help.htm?id=1";
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Параметр события</param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            tvProject.SetJsonData("ProjectData.ashx");
            tvProject.SetService(deleteCmdFuncName: "DeleteBusinessProject");
            tvProject.DbSourceSettings = new TreeViewDbSourceSettings()
            {
                ConnectionString = Config.DS_person,
                TableName = "БизнесПроекты",
                ViewName = "vwБизнесПроекты",
                PkField = "КодБизнесПроекта",
                NameField = "БизнесПроект",
                PathField = "БизнесПроектыPath1",
                ModifyUserField = "Изменил",
                ModifyDateField = "Изменено",
                RootName = "Бизнес-проекты"
            };

            tvProject.IsOrderMenu = false;
            tvProject.IsLoadData = false;
            tvProject.Resizable = false;
            tvProject.IsSaveState = true;
            tvProject.ParamName = "BProjectTreeState";
            tvProject.ClId = ClId;
            tvProject.ContextMenuAdd = true;
            tvProject.ContextMenuRename = true;
            tvProject.ContextMenuDelete = true;
            tvProject.IsSearchMenu = true;
            tvProject.IsDraggable = true;
            tvProject.NodeNameHeader = "Бизнес-проект";
            tvProject.ShowTopNodesInSearchResult = false;
            tvProject.HelpButtonVisible = true;
            tvProject.LikeButtonVisible = true;
        }

        /// <summary>
        ///     Обработчик события загрузки страницы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Параметр события</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var loadById = 0;
            if (!Request.QueryString["id"].IsNullEmptyOrZero())
                int.TryParse(Request.QueryString["id"], out loadById);
            else if (!Request.QueryString["idloc"].IsNullEmptyOrZero())
                int.TryParse(Request.QueryString["idloc"], out loadById);
            if (loadById != 0)
                tvProject.LoadById = loadById;
        }

        /// <summary>
        ///     Обработка клиентских команд
        /// </summary>
        /// <param name="cmd">Команды</param>
        /// <param name="param">Параметры</param>
        protected override void ProcessCommand(string cmd, NameValueCollection param)
        {
            switch (cmd)
            {
                case "DeleteBusinessProject":
                    int id  = int.Parse(param["Id"]);
                    var entity = new BusinessProject(id.ToString());
                    entity.Delete();
                    JS.Write("v4_reloadParentNode('{0}', '{1}');", tvProject.ClientID, id);
                    break;
                default:
                    base.ProcessCommand(cmd, param);
                    break;
            }
        }
    }
}