using System;
using System.IO;

namespace MSBuild.VersionModifierTask.Helpers
{
    /// <summary>
    /// This class is used for reading version values from the AssemblyInfo file.
    /// </summary>
    public class AssemblyInfoReader : AssemblyInfoContext
    {
        public AssemblyInfoReader(string assemblyInfoFile)
        {
            AssemblyInfoFile = assemblyInfoFile;
        }

        /// <summary>
        /// Return the Assembly Version value from the AssemblyInfo file.
        /// </summary>
        public string GetAssemblyVersion()
        {
            return RetrieveVersionBasedOn(InfoVersionBeginText);
        }


        /// <summary>
        /// Return the Assembly File Version value from the AssemblyInfo file.
        /// </summary>
        public string GetAssemblyFileVersion()
        {
            return RetrieveVersionBasedOn(FileVersionBeginText);
        }

        private string RetrieveVersionBasedOn(string beginText)
        {
            try
            {
                using (var sr = new StreamReader(AssemblyInfoFile))
                {
                    string line;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith(beginText))
                        {
                            int startIndex = (line.IndexOf(beginText) + beginText.Length);
                            int endIndex = ((line.Length - startIndex) - LastPartOfLine.Length);
                            return line.Substring(startIndex, endIndex);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Error occured during retrieving the version based on: " + beginText, exception);
            }
            
            return string.Empty;
        }
    }
}
