using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.Events;
using Prometheus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Metrics
{
    /// <summary>
    /// This class demonstrates how a subsystem subscribed to the domain events can perform additional tasks without requiring change in
    /// the main domain code. In this case the domain event is processed and some metrics are logged and added to the metrics.
    /// The logging here demonstrates the hability of Serilog to do structured logging.
    /// </summary>
    public class PaymentMetricsHandler : INotificationHandler<PaymentProcessed>
    {
        private static readonly Counter ProcessedPaymentCounter = 
            Prometheus.Metrics.CreateCounter("paymentgateway_payments_processed_total", "Number of processed payments.");

        private static readonly Histogram PaymentProcessDuration =
            Prometheus.Metrics.CreateHistogram("paymentgateway_payments_processing_duration", "Histogram of payment processing times.",
                new HistogramConfiguration { Buckets = Histogram.LinearBuckets(start: 0, width: 100, count: 10) });
        
        private readonly ILogger<PaymentMetricsHandler> logger;

        public PaymentMetricsHandler(ILogger<PaymentMetricsHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(PaymentProcessed notification, CancellationToken cancellationToken)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            this.logger.LogInformation("Processed payment with id {Id} for merchant {MerchantId} with status {Status} in {TotalProcesssingTime} ms.",
                notification.Payment.Id,
                notification.Payment.MerchantId,
                notification.Payment.Status,
                notification.TotalProcessingTime);

            ProcessedPaymentCounter.Inc();
            PaymentProcessDuration.Observe(notification.TotalProcessingTime);

            return Task.CompletedTask;
        }
    }
}
