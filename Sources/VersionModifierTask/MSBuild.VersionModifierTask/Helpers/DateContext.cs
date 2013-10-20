using System;

namespace MSBuild.VersionModifierTask.Helpers
{
    /// <summary>
    /// This DateContext is used for retrieving and manipulating dates 
    /// </summary>
    public static class DateContext
    {
        [ThreadStatic] 
        private static DateTime date;

        public static Func<DateTime> Date
        {
            get { return () =>  (date == DateTime.MinValue) ? DateTime.Now : date; }
            set { date = value(); }
        }
    }
}