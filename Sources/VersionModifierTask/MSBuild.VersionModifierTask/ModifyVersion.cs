using System;
using MSBuild.VersionModifierTask.Helpers;
using Microsoft.Build.Framework;

namespace MSBuild.VersionModifierTask
{
    /// <summary>
    /// This class is the entry point of the Version Modifier Task
    /// </summary>
    public class ModifyVersion : Microsoft.Build.Utilities.Task
    {
        /// <summary>
        /// Start the task by calling it with MSBuild 
        /// </summary>
        public override bool Execute()
        {
            var reader = new AssemblyInfoReader(AssemblyInfoPath);
            var writer = new AssemblyInfoWriter(AssemblyInfoPath);

            if (!string.IsNullOrEmpty(AssemblyVersionPattern))
            {
                ModifyAssemblyVersion(writer, reader);
            }

            if (!string.IsNullOrEmpty(AssemblyFileVersionPattern))
            {
                ModifyAssemblyFileVersion(writer, reader);
            }

            if (!string.IsNullOrEmpty(AssemblyInformationalVersion))
            {
                ModifyAssemblyInformationalVersion(writer, reader);
            }
            
            return true;
        }

        private void ModifyAssemblyVersion(AssemblyInfoWriter writer, AssemblyInfoReader reader)
        {
            Log.LogMessage(MessageImportance.Normal,
                           string.Format("Modifying AssemblyVersion with pattern: {0}", AssemblyVersionPattern));

            Log.LogMessage(MessageImportance.Normal,
                           string.Format("AssemblyInfo file: : {0}", AssemblyInfoPath));

            try
            {
                string newValue = new PatternBasedParser().Replace(reader.GetAssemblyVersion(), AssemblyVersionPattern);

                writer.ModifyAssemblyVersionTo(newValue);
            }
            catch (ApplicationException exception)
            {
                Log.LogErrorFromException(exception);
            }
        }

        private void ModifyAssemblyFileVersion(AssemblyInfoWriter writer, AssemblyInfoReader reader)
        {
            Log.LogMessage(MessageImportance.Normal,
                           string.Format("Modifying AssemblyFileVersion with pattern: {0}", AssemblyVersionPattern));

            try
            {
                string newValue = new PatternBasedParser().Replace(reader.GetAssemblyFileVersion(), AssemblyFileVersionPattern);

                writer.ModifyAssemblyFileVersionTo(newValue);
            }
            catch (ApplicationException exception)
            {
                Log.LogErrorFromException(exception);
            }
        }

        private void ModifyAssemblyInformationalVersion(AssemblyInfoWriter writer, AssemblyInfoReader reader)
        {
            Log.LogMessage(MessageImportance.Normal,
                           string.Format("Modifying AssemblyInformationalVersion with pattern: {0}", AssemblyInformationalVersion));

            try
            {
                string newValue = new PatternBasedParser().Replace(reader.GetAssemblyFileVersion(), AssemblyInformationalVersion);

                writer.ModifyAssemblyInformationalVersion(newValue);
            }
            catch (ApplicationException exception)
            {
                Log.LogErrorFromException(exception);
            }
        }

        /// <summary>
        /// Gets or sets the location of the AssemblyInfo file 
        /// </summary>
        [Required]
        public string AssemblyInfoPath { get; set; }

        /// <summary>
        /// Gets or sets the Assembly Version Pattern. 
        /// For more information about the pattern check the documentation on https://github.com/rvdkooy/VersionModifierTask 
        /// </summary>
        public string AssemblyVersionPattern { get; set; }

        /// <summary>
        /// Gets or sets the Assembly File Version Pattern. 
        /// For more information about the pattern check the documentation on https://github.com/rvdkooy/VersionModifierTask 
        /// </summary>
        public string AssemblyFileVersionPattern { get; set; }

        /// <summary>
        /// Gets or sets the Assembly Informational Version Pattern. 
        /// For more information about the pattern check the documentation on https://github.com/rvdkooy/VersionModifierTask 
        /// </summary>
        public string AssemblyInformationalVersion { get; set; }
    }
}
