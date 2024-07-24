﻿using EmailMicroService.Application.Repositories;
using EmailMicroService.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmailMicroService.Infrastructure.Services
{
    public class EmailBackgroundService(IServiceScopeFactory serviceScopeFactory, 
        ILogger<EmailBackgroundService> logger) : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory = serviceScopeFactory;
        private readonly ILogger<EmailBackgroundService> _logger = logger;
        private readonly PeriodicTimer _timer = new(TimeSpan.FromSeconds(15));

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            try
            {
                while (await _timer.WaitForNextTickAsync(stoppingToken))
                {
                    await DoWork(stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Handle the task cancellation, if needed
            }
            finally
            {
                _logger.LogInformation("Timed Hosted Service is stopping.");
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is working.");

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var emailRepository = scope.ServiceProvider.GetRequiredService<IEmailRepository>();
                var emailSenderService = scope.ServiceProvider.GetRequiredService<IEmailSenderService>();

                var emails = await emailRepository.GetAllAsync();

                foreach (var email in emails)
                {
                    await emailSenderService.SendEmailAsync(email.UserEmail, "Confirm", email.Data);
                }

                await emailRepository.DeleteAsync([.. emails]);
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");
            await base.StopAsync(stoppingToken);
        }

        public override void Dispose()
        {
            _timer.Dispose();
            base.Dispose();
        }
    }
}
