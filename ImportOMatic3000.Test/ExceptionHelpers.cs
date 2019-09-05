using System;

namespace ImportOMatic3000.Test
{
    public static class ExceptionHelpers
    {
        public static Exception InnermostException(this Exception ex)
        {
            if (ex.InnerException == null)
            { return ex; }
            return ex.InnerException.InnermostException();
        }
    }
}
