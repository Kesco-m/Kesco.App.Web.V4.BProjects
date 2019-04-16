using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.Services;
using Kesco.Lib.Entities.Persons.BusinessProject;
using Kesco.Lib.Web.DBSelect.V4;
using Kesco.Lib.Web.Settings;

namespace Kesco.App.Web.V4.BProjects
{
    /// <summary>
    ///     Веб-службы для бизнес-проектов
    /// </summary>
    public class srv : WebService
    {
        private SqlConnection cn = new SqlConnection(Config.DS_person);

        public srv()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Метод веб-службы для поиска бизнес-проекта
        /// </summary>
        /// <param name="id">Идентификатор найденного бизнес-проекта (если найдена ровно 1 запись)</param>
        /// <param name="searchText">Текст запроса</param>
        /// <param name="searchParams">Параметры поиска</param>
        /// <returns>Количество найденных бизнес-проектов</returns>
        [WebMethod]
        public int Search(out int id, string searchText, string searchParams)
        {
            var i = 0;
            id = 0;

            var bProject = new DBSBusinessProject();
            var bProjectList = bProject.GetBusinessProject(searchText);

            i = bProjectList.Count;

            if (i == 1)
                id = int.Parse(bProjectList[0].Id);
            else
                id = 0;

            return i;
        }


        /// <summary>
        ///     Метод веб-службы для получения названия бизнес-проекта по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор бизнес-проекта</param>
        /// <returns>Название бизнес-проекта</returns>
        [WebMethod]
        public string GetCaption(int id)
        {
            var result = string.Empty;

            var bProject = new BusinessProject(id.ToString());

            if (bProject != null)
                result = bProject.Name;

            return result;
        }

        #region Component Designer generated code

        //Required by the Web Services Designer 
        private IContainer components = null;


        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }


        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}