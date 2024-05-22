
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace WebApplication1.Services
{
    public class BkService : BackgroundService
    {
        private readonly TelegramBotClient _client;
        private readonly IUpdateHandler _updateHandler;
        public BkService(TelegramBotClient client,IUpdateHandler handler)
        {
            _client = client;
            _updateHandler = handler;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _client.StartReceiving
                (
                    updateHandler: _updateHandler.HandleUpdateAsync,
                    pollingErrorHandler: _updateHandler.HandlePollingErrorAsync,
                    receiverOptions: new Telegram.Bot.Polling.ReceiverOptions()
                    {
                        ThrowPendingUpdates = true
                    }
                ); ;
        }
    }
}
