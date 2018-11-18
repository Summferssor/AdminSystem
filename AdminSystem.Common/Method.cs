using System;
using System.Collections.Generic;
using System.Text;

namespace AdminSystem.Common
{
    public static class Method
    {
        public static string GetGuid32()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Trim();
        }
    }
}
