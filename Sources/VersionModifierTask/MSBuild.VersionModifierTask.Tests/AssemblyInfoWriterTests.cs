using System;
using System.IO;
using MSBuild.VersionModifierTask.Helpers;
using NUnit.Framework;

namespace MSBuild.VersionModifierTask.Tests
{
    [TestFixture]
    public class AssemblyInfoWriterTests
    {
        [Test]
        public void When_modifying_the_assembly_version_it_should_modify_it_and_save_it()
        {
            // ----------------------------------------------------------------
            // ARRANGE
            //-----------------------------------------------------------------
            string assemblyInfoFile = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\AssemblyInfo_1.txt";
            var parser = new AssemblyInfoWriter(assemblyInfoFile);

            // ----------------------------------------------------------------
            // ACT
            //-----------------------------------------------------------------
            parser.ModifyAssemblyVersionTo("1.1.1.1");

            // ----------------------------------------------------------------
            // ASSERT
            //-----------------------------------------------------------------
            bool parserReplacedValue = FindInFile(AssemblyInfoContext.InfoVersionBeginText + "1.1.1.1" + AssemblyInfoContext.LastPartOfLine, assemblyInfoFile);

            Assert.IsTrue(parserReplacedValue);
        }

        [Test]
        public void When_modifying_the_file_version_it_should_modify_it_and_save_it()
        {
            // ----------------------------------------------------------------
            // ARRANGE
            //-----------------------------------------------------------------
            string assemblyInfoFile = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\AssemblyInfo_1.txt";
            var parser = new AssemblyInfoWriter(assemblyInfoFile);

            // ----------------------------------------------------------------
            // ACT
            //-----------------------------------------------------------------
            parser.ModifyAssemblyFileVersionTo("2.2.2.2");

            // ----------------------------------------------------------------
            // ASSERT
            //-----------------------------------------------------------------
            bool parserReplacedValue = FindInFile(AssemblyInfoContext.FileVersionBeginText + "2.2.2.2" + AssemblyInfoContext.LastPartOfLine, assemblyInfoFile);

            Assert.IsTrue(parserReplacedValue);
        }

        [Test]
        public void When_the_file_is_set_to_readonly_it_should_still_modify_it()
        {
            // ----------------------------------------------------------------
            // ARRANGE
            //-----------------------------------------------------------------
            string assemblyInfoFile = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\ReadOnlyAssemblyInfo.txt";

            new FileInfo(assemblyInfoFile) {IsReadOnly = true};

            var parser = new AssemblyInfoWriter(assemblyInfoFile);

            // ----------------------------------------------------------------
            // ACT
            //-----------------------------------------------------------------
            parser.ModifyAssemblyFileVersionTo("2.2.2.2");
            parser.ModifyAssemblyVersionTo("1.1.1.1");

            // ----------------------------------------------------------------
            // ASSERT
            //-----------------------------------------------------------------
            bool parserReplacedAssemblyFileVersionValue = FindInFile(AssemblyInfoContext.FileVersionBeginText + "2.2.2.2" + AssemblyInfoContext.LastPartOfLine, assemblyInfoFile);
            bool parserReplacedAssemblyVersionValue = FindInFile(AssemblyInfoContext.InfoVersionBeginText + "1.1.1.1" + AssemblyInfoContext.LastPartOfLine, assemblyInfoFile);

            Assert.IsTrue(parserReplacedAssemblyFileVersionValue);
            Assert.IsTrue(parserReplacedAssemblyVersionValue);
            Assert.IsTrue(new FileInfo(assemblyInfoFile).IsReadOnly);
        }

        #region private helpers

        private static bool FindInFile(string valueToLookFor, string assemblyInfoFile)
        {
            bool parserReplacedValue = false;
            using (var sr = new StreamReader(assemblyInfoFile))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Contains(valueToLookFor))
                    {
                        parserReplacedValue = true;
                    }
                }
            }
            return parserReplacedValue;
        }

        #endregion
    }
}
