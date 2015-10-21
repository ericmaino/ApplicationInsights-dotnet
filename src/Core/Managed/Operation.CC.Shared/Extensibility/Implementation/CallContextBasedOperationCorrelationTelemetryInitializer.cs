﻿namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// Telemetry initializer that populates OperationContext for the telemetry item based on context stored in CallContext.
    /// </summary>
    internal class CallContextBasedOperationCorrelationTelemetryInitializer : ITelemetryInitializer
    {
        /// <summary>
        /// Initializes/Adds operation id to the existing telemetry item.
        /// </summary>
        /// <param name="telemetryItem">Target telemetry item to add operation id.</param>
        public void Initialize(ITelemetry telemetryItem)
        {
            var itemContext = telemetryItem.Context.Operation;

            if (string.IsNullOrEmpty(itemContext.ParentId) || string.IsNullOrEmpty(itemContext.RootId) || string.IsNullOrEmpty(itemContext.RootName))
            {
                var parentContext = CallContextHelpers.GetCurrentOperationContextFromCallContext();
                if (parentContext != null)
                {
                    if (string.IsNullOrEmpty(itemContext.ParentId)
                        && !string.IsNullOrEmpty(parentContext.ParentOperationId))
                    {
                        itemContext.ParentId = parentContext.ParentOperationId;
                    }

                    if (string.IsNullOrEmpty(itemContext.RootId)
                        && !string.IsNullOrEmpty(parentContext.RootOperationId))
                    {
                        itemContext.RootId = parentContext.RootOperationId;
                    }

                    if (string.IsNullOrEmpty(itemContext.RootName)
                        && !string.IsNullOrEmpty(parentContext.OperationName))
                    {
                        itemContext.RootName = parentContext.OperationName;
                    }
                }
            }
        }
    }
}