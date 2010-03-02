namespace CardWeb
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Text;

    public static class WebUtilities
    {
        /// <summary>
        /// Carriage return character
        /// </summary>
        public const char CarriageReturn = '\r';

        /// <summary>
        /// Line feed character
        /// </summary>
        public const char LineFeed = '\n';

        /// <summary>
        /// Number of stack trace frames to skip when constructing a new StackTrace
        /// </summary>
        private const int StackTraceFramesToSkip = 1;

        /// <summary>
        /// StackTrace frame index number to retrieve
        /// </summary>
        private const int StackTraceFrameIndex = 0;

        /// <summary>
        /// Constant required for maintaining file information when constructing a new StackTrace
        /// </summary>
        private const bool StackTraceNeedFileInfoFlag = true;

        /// <summary>
        /// Gets the current line.
        /// </summary>
        /// <returns>Line number from which GetCurrentLine() was called</returns>
        public static int GetCurrentLine()
        {
            try
            {
                return (new StackTrace(StackTraceFramesToSkip, StackTraceNeedFileInfoFlag)).GetFrame(StackTraceFrameIndex).GetFileLineNumber();
            }
            catch (Exception e)
            {
                Console.WriteLine("WebController: Unable to calculate line number in GetCurrentLine().");
                Console.WriteLine("-->" + e.Message);
                return 0;
            }
        } /* GetCurrentLine() */
    }
}
