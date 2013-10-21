using System;
using System.IO;

namespace MSBuild.VersionModifierTask.Helpers
{
    /// <summary>
    /// This class is used for writing version values to the AssemblyInfo file.
    /// </summary>
    public class AssemblyInfoWriter : AssemblyInfoContext
    {
        bool wasReadonly;
        
        public AssemblyInfoWriter(string assemblyInfoFile)
        {
            AssemblyInfoFile = assemblyInfoFile;
        }

        /// <summary>
        /// Writes the new Assembly Version to the AssemblyInfo file.
        /// </summary>
        public void ModifyAssemblyVersionTo(string newVersion)
        {
            ReplaceTextWith(InfoVersionBeginText, newVersion);
        }

        /// <summary>
        /// Writes the new Assembly File Version to the AssemblyInfo file.
        /// </summary>
        public void ModifyAssemblyFileVersionTo(string newVersion)
        {
            ReplaceTextWith(FileVersionBeginText, newVersion);
        }
        
        /// <summary>
        /// Writes the new Assembly Informational Version to the AssemblyInfo file.
        /// </summary>
        public void ModifyAssemblyInformationalVersion(string newVersion)
        {
            ReplaceTextWith(AssemblyInformationalVersionBeginText, newVersion);
        }

        private void ClearReadOnlyAttribute()
        {
            FileAttributes attributes = File.GetAttributes(AssemblyInfoFile);

            if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                File.SetAttributes(AssemblyInfoFile, attributes ^ FileAttributes.ReadOnly);
                wasReadonly = true;
            }
        }

        private void RestoreReadOnlyAttribute()
        {
            if (wasReadonly)
            {
                File.SetAttributes(AssemblyInfoFile, FileAttributes.ReadOnly);

                wasReadonly = false;
            }
        }
        private void ReplaceTextWith(string startText, string newVersion)
        {
            try
            {
                ClearReadOnlyAttribute();
                
                string wholeDocument = ReplaceValueInAssemblyInfoFile(startText, newVersion);

                WriteChangesToAssemblyInfoFile(wholeDocument);

                RestoreReadOnlyAttribute();
            }
            catch (Exception e)
            {
                throw new ApplicationException("Something went wrong: " + e);
            }
        }

        private void WriteChangesToAssemblyInfoFile(string wholeDocument)
        {
            using (StreamWriter streamWriter = File.CreateText(AssemblyInfoFile))
            {
                streamWriter.Write(wholeDocument);
                streamWriter.Close();
            }
        }

        private string ReplaceValueInAssemblyInfoFile(string startText, string newVersion)
        {
            string document = "";

            try
            {
                using (var sr = new StreamReader(AssemblyInfoFile))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith(startText))
                        {
                            line = startText + newVersion + LastPartOfLine;
                        }
                        document += line + Environment.NewLine;
                    }
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException(
                    "Error occured during replacing the value in the assmeblyinfo starttext: " + startText +
                    " newVersion: " + newVersion, exception);
            }
            
            return document;
        }
    }
}