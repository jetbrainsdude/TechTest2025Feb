using System;
using NLog;

namespace Lexxika.Utility
{

    #region Enums
    /// <summary>
    /// Severity of log.
    /// </summary>
    public enum LogSeverity
    {
        Fatal,
        Error,
        Warn,
        Info,
        Debug,
        Trace,
        Unknown
    }
    #endregion

    #region Interface
    public interface ILogging
    {
        /// <summary>
        /// Initialise the log with given name.
        /// </summary>
        /// <param name="loggerName">Name of logger (normally the class name).</param>
        void Initialise(string loggerName);

        /// <summary>
        /// Log a debug message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Debug(object message);

        /// <summary>
        /// Log an error message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Error(object message);

        /// <summary>
        /// Log error message including message from exception.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="exception">Exception to be logged.</param>
        void Error(string message, Exception exception);

        /// <summary>
        /// Log a fatal message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Fatal(object message);

        /// <summary>
        /// Log fatal message including message from exception.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="exception">Exception to be logged.</param>
        void Fatal(string message, Exception exception);

        /// <summary>
        /// Log an information message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Info(object message);

        /// <summary>
        /// Log a warning message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        void Warn(object message);
    }
    #endregion

    /// <summary>
    /// Configuration driven general purpose logging (uses nLog). Construction of internal logger is delayed until CheckOrCreateLogger is called.
    /// </summary>
    public class Logging : ILogging
    {
        #region Private
        private readonly object _syncRoot = new object();
        private Logger _log;
        private object _recentFatalMessage = string.Empty;
        private object _recentErrorMessage = string.Empty;
        private object _recentWarnMessage = string.Empty;
        private object _recentInfoMessage = string.Empty;
        private int _fatalMessageRepeatCount;
        private int _errorMessageRepeatCount;
        private int _warnMessageRepeatCount;
        private int _infoMessageRepeatCount;

        private const int MAX_MESSAGE_REPEATS = 10;
        private const string REPEATING_MESSAGE_DISPLAY = "<repeating message not displayed anymore>";
        #endregion

        #region Private Helper Routines
        /// <summary>
        /// Check logger exists, if not then create with logger name.
        /// </summary>
        /// <param name="loggerName">Name of logger (normally the class name).</param>
        private void CheckOrCreateLogger(string loggerName)
        {
            if (_log == null)
            {
                _log = LogManager.GetLogger(loggerName);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialise the log with given name.
        /// </summary>
        /// <param name="loggerName">Name of logger (normally the class name).</param>
        public void Initialise(string loggerName)
        {
            lock (_syncRoot)
            {
                CheckOrCreateLogger(loggerName);
            }
        }

        /// <summary>
        /// Log a debug message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public void Debug(object message)
        {
            lock (_syncRoot)
            {
                CheckOrCreateLogger(string.Empty);

                if (_log.IsDebugEnabled)
                {
                    _log.Debug(message);
                }
            }
        }

        /// <summary>
        /// Log an error message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public void Error(object message)
        {
            lock (_syncRoot)
            {
                CheckOrCreateLogger(string.Empty);

                if (_log.IsErrorEnabled)
                {
                    #region Check For Message Repetition
                    if (message.ToString() == _recentErrorMessage.ToString())
                    {
                        // limit the repeat count
                        if (_errorMessageRepeatCount < MAX_MESSAGE_REPEATS + 20)
                        {
                            _errorMessageRepeatCount++;
                        }

                        // limit the logging of the repeated message
                        if (_errorMessageRepeatCount < MAX_MESSAGE_REPEATS)
                        {
                            _log.Error(message);
                        }
                        else if (_errorMessageRepeatCount == MAX_MESSAGE_REPEATS)
                        {
                            // log the repeating message
                            _log.Error(REPEATING_MESSAGE_DISPLAY);
                        }

                        return;
                    }

                    // reset repetition
                    _errorMessageRepeatCount = 0;
                    _recentErrorMessage = message;
                    #endregion

                    _log.Error(message);
                }
            }
        }

        /// <summary>
        /// Log error message including message from exception.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="exception">Exception to be logged.</param>
        public void Error(string message, Exception exception)
        {
            lock (_syncRoot)
            {
                CheckOrCreateLogger(string.Empty);

                if (_log.IsErrorEnabled)
                {
                    _log.Error("{0} {1}", message, exception.Message);
                }
            }
        }

        /// <summary>
        /// Log a fatal message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public void Fatal(object message)
        {
            lock (_syncRoot)
            {
                CheckOrCreateLogger(string.Empty);

                if (_log.IsFatalEnabled)
                {
                    #region Check For Message Repetition
                    if (message.ToString() == _recentFatalMessage.ToString())
                    {
                        // limit the repeat count
                        if (_fatalMessageRepeatCount < MAX_MESSAGE_REPEATS + 20)
                        {
                            _fatalMessageRepeatCount++;
                        }

                        // limit the logging of the repeated message
                        if (_fatalMessageRepeatCount < MAX_MESSAGE_REPEATS)
                        {
                            _log.Fatal(message);
                        }
                        else if (_fatalMessageRepeatCount == MAX_MESSAGE_REPEATS)
                        {
                            // log the repeating message
                            _log.Fatal(REPEATING_MESSAGE_DISPLAY);
                        }

                        return;
                    }

                    // reset repetition
                    _fatalMessageRepeatCount = 0;
                    _recentFatalMessage = message;
                    #endregion

                    _log.Fatal(message);
                }
            }
        }

        /// <summary>
        /// Log fatal message including message from exception.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        /// <param name="exception">Exception to be logged.</param>
        public void Fatal(string message, Exception exception)
        {
            lock (_syncRoot)
            {
                CheckOrCreateLogger(string.Empty);

                if (_log.IsFatalEnabled)
                {
                    _log.Fatal("{0} {1}", message, exception.Message);
                }
            }
        }

        /// <summary>
        /// Log an information message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public void Info(object message)
        {
            lock (_syncRoot)
            {
                CheckOrCreateLogger(string.Empty);

                if (_log.IsInfoEnabled)
                {
                    #region Check For Message Repetition
                    if (message.ToString() == _recentInfoMessage.ToString())
                    {
                        // limit the repeat count
                        if (_infoMessageRepeatCount < MAX_MESSAGE_REPEATS + 20)
                        {
                            _infoMessageRepeatCount++;
                        }

                        // limit the logging of the repeated message
                        if (_infoMessageRepeatCount < MAX_MESSAGE_REPEATS)
                        {
                            _log.Info(message);
                        }
                        else if (_infoMessageRepeatCount == MAX_MESSAGE_REPEATS)
                        {
                            // log the repeating message
                            _log.Info(REPEATING_MESSAGE_DISPLAY);
                        }

                        return;
                    }

                    // reset repetition
                    _infoMessageRepeatCount = 0;
                    _recentInfoMessage = message;
                    #endregion

                    _log.Info(message);
                }
            }
        }

        /// <summary>
        /// Log a warning message.
        /// </summary>
        /// <param name="message">Message to be logged.</param>
        public void Warn(object message)
        {
            lock (_syncRoot)
            {
                CheckOrCreateLogger(string.Empty);

                if (_log.IsWarnEnabled)
                {
                    #region Check For Message Repetition
                    if (message.ToString() == _recentWarnMessage.ToString())
                    {
                        // limit the repeat count
                        if (_warnMessageRepeatCount < MAX_MESSAGE_REPEATS + 20)
                        {
                            _warnMessageRepeatCount++;
                        }

                        // limit the logging of the repeated message
                        if (_warnMessageRepeatCount < MAX_MESSAGE_REPEATS)
                        {
                            _log.Warn(message);
                        }
                        else if (_warnMessageRepeatCount == MAX_MESSAGE_REPEATS)
                        {
                            // log the repeating message
                            _log.Warn(REPEATING_MESSAGE_DISPLAY);
                        }

                        return;
                    }

                    // reset repetition
                    _warnMessageRepeatCount = 0;
                    _recentWarnMessage = message;
                    #endregion

                    _log.Warn(message);
                }
            }
        }
        #endregion
    }
}