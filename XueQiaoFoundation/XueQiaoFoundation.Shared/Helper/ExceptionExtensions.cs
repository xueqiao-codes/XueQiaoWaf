using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XueQiaoFoundation.Shared.Helper
{
    public static class ExceptionExtensions
    {
        public static string GetAllFootprints(this Exception x)
        {
            var st = new StackTrace(x, true);
            var traceString = new StringBuilder();

            traceString.Append($"{x.GetType().FullName}:{x.Message}");
            for (var i = 0; i < st.FrameCount; i++)
            {
                traceString.Append("\n\t");

                var frame = st.GetFrame(i);

                traceString.Append($"Method:{frame.GetMethod().Name}");
                var fileName = frame.GetFileName();
                traceString.Append($", File:{(fileName ?? "Unkown")}");
                var lineNum = frame.GetFileLineNumber();
                traceString.Append($"(Line:{(lineNum<=0?"Unkown":lineNum.ToString())})");
            }

            return traceString.ToString();
        }
    }
}
