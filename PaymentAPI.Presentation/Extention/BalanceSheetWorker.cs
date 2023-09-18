using Coravel.Invocable;
using Coravel.Scheduling.Schedule.Interfaces;
using PaymentAPI.Application.Interface;
using Quartz;

namespace PaymentAPI.Presentation.Extention
{
    [DisallowConcurrentExecution]
    public class BalanceSheetWorker : IJob
    {
        private readonly ILogger<BalanceSheetWorker> _logger;
        private readonly ITransaction _transaction;

        public BalanceSheetWorker(ILogger<BalanceSheetWorker> logger, ITransaction transaction)
        {
            _logger = logger;
            _transaction = transaction;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await _transaction.BalanceSheetLoader();
            }

            catch (Exception x)
            {
                _logger.LogError(x, x.Message);
            }
        }
           
    }
}
