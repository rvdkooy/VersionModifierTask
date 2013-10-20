using System;
using MSBuild.VersionModifierTask.Helpers;
using NUnit.Framework;

namespace MSBuild.VersionModifierTask.Tests
{
    [TestFixture]
    public class AssemblyInfoReaderTests
    {
        [Test]
        public void When_trying_to_get_the_current_assembly_version_from_the_assemblyinfo_it_should_return()
        {
            // ----------------------------------------------------------------
            // ARRANGE
            //-----------------------------------------------------------------
            string assemblyInfoFile = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\AssemblyInfoToReadFrom.txt";
            var parser = new AssemblyInfoReader(assemblyInfoFile);

            // ----------------------------------------------------------------
            // ACT
            //-----------------------------------------------------------------
            string assemblyVersion = parser.GetAssemblyVersion();

            // ----------------------------------------------------------------
            // ASSERT
            //-----------------------------------------------------------------
            Assert.AreEqual("1.2.3.4.5.6.7.8", assemblyVersion);
        }

        [Test]
        public void When_trying_to_get_the_current_assembly_file_version_from_the_assemblyinfo_it_should_return()
        {
            // ----------------------------------------------------------------
            // ARRANGE
            //-----------------------------------------------------------------
            string assemblyInfoFile = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\AssemblyInfoToReadFrom.txt";
            var parser = new AssemblyInfoReader(assemblyInfoFile);

            // ----------------------------------------------------------------
            // ACT
            //-----------------------------------------------------------------
            string assemblyFileVersion = parser.GetAssemblyFileVersion();

            // ----------------------------------------------------------------
            // ASSERT
            //-----------------------------------------------------------------
            Assert.AreEqual("5.6", assemblyFileVersion);
        }
    }
}
