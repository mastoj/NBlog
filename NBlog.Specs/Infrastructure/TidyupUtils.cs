using System;
using System.Collections.Generic;

namespace NBlog.Specs.Infrastructure
{
    public static class TidyupUtils
    {
        private static readonly List<Action> _tasks = new List<Action>();

        public static void AddTidyupTask(Action task)
        {
            _tasks.Add(task);
        }

        public static void PerformTidyup()
        {
            try
            {
                foreach (var task in _tasks)
                    task();
            }
            finally
            {
                _tasks.Clear();
            }
        }
    }
}