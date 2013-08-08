using System;

namespace SDLite
{
    /// <summary>
    /// The update info.
    /// </summary>
    public class UpdateInfo
    {
        /// <summary>
        /// Use "Ctrl+F3" to search in current document.
        /// </summary>
        public static bool UseCtrlF3ToSearchInCurrentDocument
        {
            get { return true; }
        }
        
        /// <summary>
        /// The tools version in project xml.
        /// </summary>
        public static string ProjectToolsVersion
        {
            //get { return "3.5"; }
            get { return "4.0"; }
        }
        
        /// <summary>
        /// Gets if expand new project after created.
        /// </summary>
        public static bool ExpandNewProjectByDefaut
        {
            get { return true; }
        }
    }
}
