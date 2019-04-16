using System.ComponentModel;
using System.Data.SqlClient;
using System.Web.Services;
using Kesco.Lib.Entities.Persons.BusinessProject;
using Kesco.Lib.Web.DBSelect.V4;
using Kesco.Lib.Web.Settings;

namespace Kesco.App.Web.V4.BProjects
{
    /// <summary>
    ///     ���-������ ��� ������-��������
    /// </summary>
    public class srv : WebService
    {
        private SqlConnection cn = new SqlConnection(Config.DS_person);

        public srv()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     ����� ���-������ ��� ������ ������-�������
        /// </summary>
        /// <param name="id">������������� ���������� ������-������� (���� ������� ����� 1 ������)</param>
        /// <param name="searchText">����� �������</param>
        /// <param name="searchParams">��������� ������</param>
        /// <returns>���������� ��������� ������-��������</returns>
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
        ///     ����� ���-������ ��� ��������� �������� ������-������� �� ��� ��������������
        /// </summary>
        /// <param name="id">������������� ������-�������</param>
        /// <returns>�������� ������-�������</returns>
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