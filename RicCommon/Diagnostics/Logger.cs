using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using log4net;

namespace RicCommon.Diagnostics
{
    /// <summary>
    ///     Error logging class
    /// </summary>
    public static class Logger
    {
        public static void Write(string message)
        {
            Write(message, LoggerLevel.Information);
        }

        /// <summary>
        ///     Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Write(object message)
        {
            Write(message, LoggerLevel.Information);
        }

        /// <summary>
        ///     Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="category">The category.</param>
        /// <param name="memberName"></param>
        public static void Write(object message, LoggerLevel category, [CallerMemberName] string memberName = "")
        {
            var logger = LogManager.GetLogger(memberName);

            Console.WriteLine(message);

            switch (category)
            {
                case LoggerLevel.Information:
                    logger.Info(message);
                    break;
                case LoggerLevel.Error:
                    logger.Error(message);
                    break;
                case LoggerLevel.Fatal:
                    logger.Fatal(message);
                    break;
                case LoggerLevel.Warning:
                    logger.Warn(message);
                    break;
            }
        }

        public static void Write(Exception ex)
        {
            Console.WriteLine(ex.ToString());
            HandleException(ex);
        }

        public static void WriteDecoration()
        {
            Write("----------------------------------------------------");
        }

        /// <summary>
        ///     Handles the exception.
        /// </summary>
        /// <param name="exceptionToHandle">The exception to handle.</param>
        public static void HandleException(Exception exceptionToHandle)
        {
            const string message = "No message";
            LogException(message, exceptionToHandle);
        }

        /// <summary>
        ///     Handles the exception.
        /// </summary>
        /// <param name="exceptionToHandle">The exception to handle.</param>
        /// <param name="message">Name of the policy.</param>
        public static void HandleException(Exception exceptionToHandle, string message)
        {
            LogException(message, exceptionToHandle);
        }

        private static void LogException(string message, Exception ex)
        {
            var logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            logger.Error(message, ex);
        }
    }
}
