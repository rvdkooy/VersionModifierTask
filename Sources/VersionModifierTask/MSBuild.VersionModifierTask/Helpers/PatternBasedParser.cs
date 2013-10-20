using System;
using System.Linq;

namespace MSBuild.VersionModifierTask.Helpers
{
    /// <summary>
    /// This class is responsible for parsing the patterns with the original values.
    /// </summary>
    public class PatternBasedParser
    {
        public const string IgnoreCharacter = "-";
        public const string DateCharacter = "date";
        public const string SplitCharacter = "=";

        /// <summary>
        /// Replaces the original version based on the specifief pattern
        /// </summary>
        /// <param name="originalVersion">the original version retrieved from the AssemblyInfo file.</param>
        /// <param name="pattern">The Pattern to use</param>
        /// <returns>The new version</returns>
        public string Replace(string originalVersion, string pattern)
        {
            string[] originalVersionPositions = originalVersion.Split('.');
            string[] patternPositions = pattern.Split('.');

            AssertArgumentsAreValid(patternPositions, originalVersionPositions);

            var arrayOfNewValues = ProcessPositions(originalVersionPositions, patternPositions);

            return string.Join(".", arrayOfNewValues);
        }

        private string[] ProcessPositions(string[] originalVersionPositions, string[] patternPositions)
        {
            var arrayOfNewValues = new string[patternPositions.Length];

            for (int i = 0; i < patternPositions.Length; i++)
            {
                var originalValue = originalVersionPositions[i];
                var patternValue = GetValueBetweenBrackets(patternPositions[i]);

                patternValue = ReplaceValueWithPattern(patternValue, originalValue);

                arrayOfNewValues[i] = patternValue;
            }

            return arrayOfNewValues;
        }

        private string ReplaceValueWithPattern(string patternValue, string originalValue)
        {
            string result = patternValue;

            if (result == IgnoreCharacter)
            {
                result = originalValue;
            }
            if (result.ToLower().Contains(DateCharacter) && result.Contains(SplitCharacter))
            {
                var splitted = result.Split(SplitCharacter.ToCharArray());
                result = DateContext.Date().ToString(splitted[1]);
            }

            return result;
        }

        private static string GetValueBetweenBrackets(string position)
        {
            return position.Substring(position.IndexOf('[') + 1, position.IndexOf(']') - 1);
        }

        private static void AssertArgumentsAreValid(string[] patternPositions, string[] originalVersionPositions)
        {
            if (originalVersionPositions.Length > patternPositions.Length)
            {
                throw new ArgumentException(
                    string.Format("the original assemblyinfo version ({0}) has more positions ({1}) to process than the pattern",
                    string.Join(".",  originalVersionPositions.ToArray()),
                    originalVersionPositions.Length));
            }
            if (originalVersionPositions.Length < patternPositions.Length)
            {
                throw new ArgumentException(
                    string.Format("the pattern has more positions ({1}) to process than the_original assemblyinfo version ({0})",
                    string.Join(".", originalVersionPositions.ToArray()),
                    patternPositions.Length));
            }
        }
    }
}
