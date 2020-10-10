using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Programming.Utils
{
    public static class ResultHelper
    {
        public static Object GetOkResult(Object data, string msg = "ok")
        {
            return new { status = "ok", message = msg, data };
        }

        public static Object GetErrResult(Object data, string msg = "error")
        {
            return new { status = "error", message = msg, data };
        }
    }
}
