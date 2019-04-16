using System;
using System.IO;
using Kesco.Lib.BaseExtention;
using Kesco.Lib.Log;
using Kesco.Lib.Web.Controls.V4.Common;
using Kesco.Lib.Web.Controls.V4.TreeView;
using Kesco.Lib.Web.Settings;
using System.Collections.Specialized;
using Kesco.Lib.Entities.Persons.BusinessProject;

namespace Kesco.App.Web.V4.BProjects
{
    /// <summary>
    ///     Форма поиска бизнес-проектов
    /// </summary>
    public partial class Search : EntityPage
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса WndSizePosKeeper
        /// </summary>
        public Search()
        {
            HelpUrl = "hlp/help.htm?id=1";
        }

        /// <summary>
        ///     Ссылка на справку
        /// </summary>
        public override string HelpUrl { get; set; }


        /// <summary>
        ///     Отрисовка верхней панели меню
        /// </summary>
        /// <returns>Строка, получаемая из StringWriter</returns>
        protected string RenderDocumentHeader()
        {
            using (var w = new StringWriter())
            {
                try
                {
                    ClearMenuButtons();
                    RenderButtons(w);
                }
                catch (Exception e)
                {
                    var dex = new DetailedException(
                        Resx.GetString("BProject_errFailedGenerateButtons") + ": " + e.Message, e);
                    Logger.WriteEx(dex);
                    throw dex;
                }

                return w.ToString();
            }
        }


        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Параметр события</param>
        protected void Page_PreInit(object sender, EventArgs e)
        {
            tvProject.SetJsonData("ProjectData.ashx");
            tvProject.SetService(deleteCmdFuncName: "DeleteBusinessProject");
            tvProject.SetDataSource("БизнесПроекты", "vwБизнесПроекты", Config.DS_person,
                "КодБизнесПроекта", "БизнесПроект", "БизнесПроектыPath1");
            tvProject.IsOrderMenu = true;
            tvProject.IsLoadData = false;
            tvProject.Resizable = false;
            tvProject.Dock = TreeView.DockStyle.None;
            tvProject.IsSaveState = true;
            tvProject.ParamName = "BProjectTreeState";
            tvProject.ClId = ClId;
            tvProject.IsContextMenu = true;
            tvProject.ContextMenuAdd = true;
            tvProject.ContextMenuRename = true;
            tvProject.ContextMenuDelete = true;
            tvProject.IsSearchMenu = true;
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

            IsRememberWindowProperties = true;
            WindowParameters = new WindowParameters("BProjectSrchWndLeft", "BProjectSrchWndTop", "BProjectSrchWndWidth",
                "BProjectSrcWndHeight");
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
                    JS.Write("v4_reloadParentNode('{0}');", tvProject.ClientID);
                    break;
                default:
                    base.ProcessCommand(cmd, param);
                    break;
            }
        }
    }
}