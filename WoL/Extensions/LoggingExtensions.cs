using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WoL.Data;
using WoL.Models.ViewModels;

namespace WoL.Extensions
{
    // to be accessible from razor views this needs to be public
    public static class LoggingExtensions
    {
        // this class is build like this one from the framework:
        // https://github.com/dotnet/aspnetcore/blob/f2e6e6ff334176540ef0b3291122e359c2106d1a/src/Security/Authentication/Core/src/LoggingExtensions.cs

        private static readonly Action<ILogger, Exception> addHostUnknownException;
        private static readonly Action<ILogger, Models.Host, Exception> addHostDuplicateEntryException;
        private static readonly Action<ILogger, Exception> getHostPlatformNotSupportedException;
        private static readonly Action<ILogger, string, string, string, Exception> getHostUnknownException;
        private static readonly Action<ILogger, string, string, string, Exception> getHostHostNotFound;
        private static readonly Action<ILogger, Models.Host, Exception> hostAdded;
        private static readonly Action<ILogger, Models.Host, Exception> wakeHost;
        private static readonly Action<ILogger, Models.Host, int, HostViewModel.HostStatus, Exception> wakeHostFinalStatus;
        private static readonly Action<ILogger, Models.Host, Exception> hostDeleted;

        static LoggingExtensions()
        {
            addHostUnknownException = LoggerMessage.Define(
                LogLevel.Error,
                new EventId(1, "AddHostUnknownException"),
                "An unknown exception was thrown when adding a host to the HostService."
            );

            addHostDuplicateEntryException = LoggerMessage.Define<Models.Host>(
                LogLevel.Information,
                new EventId(2, "AddHostDuplicateEntryException"),
                "The host {Host} could not be added as one of it's entries that should be unique weren't."
            );

            getHostPlatformNotSupportedException = LoggerMessage.Define(
                LogLevel.Warning,
                new EventId(3, "GetHostPlatformNotSupportedException"),
                "A PlatformNotSupported exception was thrown when trying to get a host for the user's input."
            );

            getHostUnknownException = LoggerMessage.Define<string, string, string>(
                LogLevel.Warning,
                new EventId(4, "GetHostUnknownException"),
                "An unknown exception was thrown while resolving the host '{CaptionInput}' {HostNameInput} / {MacAddressInput}."
            );

            getHostHostNotFound = LoggerMessage.Define<string, string, string>(
                LogLevel.Information,
                new EventId(5, "GetHostUnknownException"),
                "While resolving '{CaptionInput}' {HostNameInput} / {MacAddressInput} no host could be found."
            );

            hostAdded = LoggerMessage.Define<Models.Host>(
                LogLevel.Information,
                new EventId(10, "HostAdded"),
                "Host {Host} was added successfully."
            );

            wakeHost = LoggerMessage.Define<Models.Host>(
                LogLevel.Information,
                new EventId(20, "WakeHost"),
                "Waking {Host}..."
            );

            wakeHostFinalStatus = LoggerMessage.Define<Models.Host, int, HostViewModel.HostStatus>(
                LogLevel.Information,
                new EventId(21, "WakeHostFinalStatus"),
                "Final HostStatus of {Host} after {PingTries} ping tries is {HostStatus}."
            );

            hostDeleted = LoggerMessage.Define<Models.Host>(
                LogLevel.Information,
                new EventId(30, "HostDeleted"),
                "Host {Host} deleted."
            );
        }

        public static void AddHostUnknownException(this ILogger logger, Exception exc)
        {
            addHostUnknownException(logger, exc);
        }

        public static void AddHostDuplicateEntryException(this ILogger logger, Models.Host value, IHostService.DuplicateEntryException exc)
        {
            addHostDuplicateEntryException(logger, value, exc);
        }

        public static void GetHostPlatformNotSupportedException(this ILogger logger, PlatformNotSupportedException exc)
        {
            getHostPlatformNotSupportedException(logger, exc);
        }

        public static void GetHostUnknownException(this ILogger logger, string captionInput, string hostNameInput, string macAddressInput, Exception exc)
        {
            getHostUnknownException(logger, captionInput, hostNameInput, macAddressInput, exc);
        }

        public static void GetHostHostNotFound(this ILogger logger, string captionInput, string hostNameInput, string macAddressInput, Exception exc)
        {
            getHostHostNotFound(logger, captionInput, hostNameInput, macAddressInput, exc);
        }

        public static void HostAdded(this ILogger logger, Models.Host host)
        {
            hostAdded(logger, host, null);
        }

        public static void WakeHost(this ILogger logger, Models.Host host)
        {
            wakeHost(logger, host, null);
        }

        public static void WakeHostFinalStatus(this ILogger logger, Models.Host host, int pingTries, HostViewModel.HostStatus finalStatus)
        {
            wakeHostFinalStatus(logger, host, pingTries, finalStatus, null);
        }

        public static void HostDeleted(this ILogger logger, Models.Host host)
        {
            hostDeleted(logger, host, null);
        }
    }
}
