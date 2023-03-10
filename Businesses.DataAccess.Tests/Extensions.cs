
using System;
using Microsoft.Extensions.Logging;
using Moq;

namespace Businesses.DataAccess.Tests;

public static class Extensions
{
    /// <summary>
    /// Verify that there no Warning or Error messages logged.
    /// </summary>
    /// <param name="mockLogger">
    /// The mock logger to check for messages
    /// </param>
    /// <typeparam name="T"></typeparam>
    public static void VerifyNoErrorsOrWarnings<T>(this Mock<ILogger<T>> mockLogger)
    {
        // Check that no error or warning level messages have been logged.
        mockLogger.Verify(l => l.Log(
            It.Is<LogLevel>(l => l == LogLevel.Error || l == LogLevel.Warning), 
            It.IsAny<EventId>(), 
            It.IsAny<It.IsAnyType>(), 
            It.IsAny<Exception>(), 
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), 
          Times.Never);
    }

    /// <summary>
    /// Verify that there were exactly <see cref="expectedMessageCount"> messages logged with the specified <see cref="logLevel"> 
    /// </summary>
    /// <param name="mockLogger">
    /// The mock logger to check for messages
    /// </param>
    /// <param name="logLevel">
    /// The level of log messages to check for
    /// </param>
    /// <param name="expectedMessageCount">
    /// The count of messages that were expected to be logged at the specified level.
    /// </param>
    /// <typeparam name="T"></typeparam>
    public static void VerifyLogMessageCount<T>(this Mock<ILogger<T>> mockLogger, LogLevel logLevel, int expectedMessageCount)
    {
        // Check that no error or warning level messages have been logged.
        mockLogger.Verify(l => l.Log(
            It.Is<LogLevel>(l => l == logLevel), 
            It.IsAny<EventId>(), 
            It.IsAny<It.IsAnyType>(), 
            It.IsAny<Exception>(), 
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), 
          Times.Exactly(expectedMessageCount));
    }
}