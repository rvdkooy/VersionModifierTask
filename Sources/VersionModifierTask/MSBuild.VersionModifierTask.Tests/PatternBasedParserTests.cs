using System;
using MSBuild.VersionModifierTask.Helpers;
using NUnit.Framework;

namespace MSBuild.VersionModifierTask.Tests
{
    [TestFixture]
    public class PatternBasedParserTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "the original assemblyinfo version (1.1.0.0) has more positions (4) to process than the pattern")]
        public void When_the_original_assemblyinfo_version_has_more_positions_to_process_then_the_pattern_it_should_throw()
        {
            // ----------------------------------------------------------------
            // ARRANGE
            //-----------------------------------------------------------------
            const string originalVersion = "1.1.0.0";
            const string pattern = "[2].[2].[2]";
            var patternBasedVersionReplacer = new PatternBasedParser();

            // ----------------------------------------------------------------
            // ASSERT
            //-----------------------------------------------------------------
            patternBasedVersionReplacer.Replace(originalVersion, pattern);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException),
            ExpectedMessage = "the pattern has more positions (4) to process than the_original assemblyinfo version (1.1.0)")]
        public void When_pattern_has_more_positions_to_process_then_the_the_original_assemblyinfo_version_it_should_throw()
        {
            // ----------------------------------------------------------------
            // ARRANGE
            //-----------------------------------------------------------------
            const string originalVersion = "1.1.0";
            const string pattern = "[2].[2].[2].[2]";
            var patternBasedVersionReplacer = new PatternBasedParser();

            // ----------------------------------------------------------------
            // ASSERT
            //-----------------------------------------------------------------
            patternBasedVersionReplacer.Replace(originalVersion, pattern);
        }

        [Test]
        public void When_patterns_only_contains_numbers_all_the_original_positions_should_be_overwritten()
        {
            // ----------------------------------------------------------------
            // ARRANGE
            //-----------------------------------------------------------------
            const string stringToReplace = "1.1.0.0";
            const string pattern = "[2].[22].[2].[22]";
            var patternBasedVersionReplacer = new PatternBasedParser();

            // ----------------------------------------------------------------
            // ACT
            //-----------------------------------------------------------------
            string output = patternBasedVersionReplacer.Replace(stringToReplace, pattern);

            // ----------------------------------------------------------------
            // ASSERT
            //-----------------------------------------------------------------
            Assert.AreEqual("2.22.2.22", output);
        }

        [Test]
        public void When_pattern_value_contains_a_hash_the_original_position_value_should_be_used()
        {
            // ----------------------------------------------------------------
            // ARRANGE
            //-----------------------------------------------------------------
            const string stringToReplace = "1.1.0.0";
            const string pattern = "[2].[-].[3].[4]";
            var patternBasedVersionReplacer = new PatternBasedParser();

            // ----------------------------------------------------------------
            // ACT
            //-----------------------------------------------------------------
            string output = patternBasedVersionReplacer.Replace(stringToReplace, pattern);

            // ----------------------------------------------------------------
            // ASSERT
            //-----------------------------------------------------------------
            Assert.AreEqual("2.1.3.4", output);
        }

        [Test]
        public void When_pattern_value_contains_date_the_original_position_value_should_be_overwritten_with_that_date()
        {
            // ----------------------------------------------------------------
            // ARRANGE
            //-----------------------------------------------------------------
            DateContext.Date = () => new DateTime(2011, 02, 01);

            const string stringToReplace = "2.1.0.0";
            const string pattern = "[-].[-].[date=yyyy].[2645]";
            var patternBasedVersionReplacer = new PatternBasedParser();

            // ----------------------------------------------------------------
            // ACT
            //-----------------------------------------------------------------
            string output = patternBasedVersionReplacer.Replace(stringToReplace, pattern);

            // ----------------------------------------------------------------
            // ASSERT
            //-----------------------------------------------------------------
            Assert.AreEqual("2.1.2011.2645", output);
        }
    }
}
