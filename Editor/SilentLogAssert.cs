// Copyright (c) AIR Pty Ltd. All rights reserved.

using NUnit.Framework;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AIR.SilentLogAssert
{
    public class SilentLogAssert : ILogHandler, IDisposable
    {
        private readonly ILogHandler _previousUnityLogHandler;
        private readonly LogType _expectedLogType;
        private readonly string _expectedLog;
        private bool _hasReceivedExpectedLog = false;

        public SilentLogAssert(
            LogType logType,
            string logMessage)
        {
            _previousUnityLogHandler = Debug.unityLogger.logHandler;
            Debug.unityLogger.logHandler = this;

            // Presently only expect one log of interest,
            // in future may wish to support an array of these.
            _expectedLogType = logType;
            _expectedLog = logMessage;
        }

        public bool Enabled { get; set; } = true;

        public static void Expect(
            LogType logType,
            string expectedMessage,
            TestDelegate testDelegate)
        {
            using (new SilentLogAssert(logType, expectedMessage))
            {
                testDelegate.Invoke();
            }
        }

        public void LogFormat(
            LogType logType,
            Object context,
            string format,
            params object[] args)
        {
            var isExpectedLog = _expectedLogType == logType;
            var formatMatchesExpectedLog = string.Format(format, args) == _expectedLog;
            _hasReceivedExpectedLog = isExpectedLog && formatMatchesExpectedLog;

            if (!Enabled)
                _previousUnityLogHandler.LogFormat(logType, context, format, args);
        }

        public void LogException(
            Exception exception,
            Object context)
        {
            // Should be hit so rethrow.
            // This is not for exception logs.
            // Which is a tad confusing.
            throw exception;
        }

        public void Dispose()
        {
            Debug.unityLogger.logHandler = _previousUnityLogHandler;
            if (!_hasReceivedExpectedLog)
                throw new Exception("Expected Log was not received. " + _expectedLog);
        }
    }
}