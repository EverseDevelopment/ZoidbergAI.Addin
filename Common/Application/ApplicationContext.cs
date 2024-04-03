using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Common.Application
{
    public class ApplicationContext
    {
        #region Singleton

        static ApplicationContext instance;
        public static ApplicationContext Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ApplicationContext();
                }
                return instance;
            }
        }

        private ApplicationContext()
        {
            
        }

        #endregion

        public string AppFormsTitle = "Title of your Addin here.";

        /// <summary>
        /// Current Command Data
        /// </summary>
        private ExternalCommandData externalCommandData;

        /// <summary>
        /// Set Context.
        /// </summary>
        /// <param name="data">ExternalCommandData</param>
        public void SetContext(ExternalCommandData data)
        {
            this.externalCommandData = data;
        }

        /// <summary>
        /// Get Document
        /// </summary>
        /// <returns>Document</returns>
        public Document GetDocument()
        {
            return externalCommandData.Application.ActiveUIDocument.Document;
        }

        /// <summary>
        /// Get UI Document
        /// </summary>
        /// <returns>UIDocument</returns>
        public UIDocument GetUIDocument()
        {
            return externalCommandData.Application.ActiveUIDocument;
        }
    }
}
