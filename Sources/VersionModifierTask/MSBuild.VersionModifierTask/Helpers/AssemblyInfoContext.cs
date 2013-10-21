namespace MSBuild.VersionModifierTask.Helpers
{
    /// <summary>
    /// This class contains all context information for reading or writing the AssemblyInfo file
    /// </summary>
    public abstract class AssemblyInfoContext
    {
        public const string InfoVersionBeginText = "[assembly: AssemblyVersion(\"";
        public const string FileVersionBeginText = "[assembly: AssemblyFileVersion(\"";
        public const string AssemblyInformationalVersionBeginText = "[assembly: AssemblyInformationalVersion(\"";
        public const string LastPartOfLine = "\")]";

        protected string AssemblyInfoFile;
    }
}