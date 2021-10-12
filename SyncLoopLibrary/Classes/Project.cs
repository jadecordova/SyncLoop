using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncLoopLibrary
{
    /// <summary>
    /// Project information.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Set on video window class, on Open_Executed command.
        /// </summary>
        public string VideoFile { get; set; } = null;

        /// <summary>
        /// Set on OpenTextFile when opening a text file.
        /// </summary>
        public string TextFile { get; set; } = null;

        /// <summary>
        /// Set on OpenTextFile when opening a RTF file.
        /// </summary>
        public string ProofReadFile { get; set; } = null;

        /// <summary>
        /// Set on OpenTextFile when opening a XAML file.
        /// </summary>
        public string XamlFile { get; set; } = null;

        /// <summary>
        /// Set on GetProgramInfo.
        /// </summary>
        public string ProjectFolder { get; set; } = null;

        /// <summary>
        /// Set on GetProgramInfo.
        /// </summary>
        public string DocumentName { get; set; } = null;

        /// <summary>
        /// Set on GetProgramInfo.
        /// </summary>
        public string ProgramInfoFile { get; set; } = null;

        /// <summary>
        /// Set on SaveCharacters() on CharacterSelector window.
        /// </summary>
        public string CharactersFile { get; set; } = null;

        /// <summary>
        /// Set on GenerateExcelDocument_Executed.
        /// </summary>
        public string ExcelFile { get; set; } = null;

        /// <summary>
        /// Set on GenerateSubtitlesDocuments_Executed.
        /// </summary>
        public string SubtitlesFile { get; set; } = null;
    }
}
