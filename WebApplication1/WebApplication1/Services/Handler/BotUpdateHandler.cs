using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace WebApplication1.Services.Handler
{
    public class BotUpdateHandler : IUpdateHandler
    {
        private readonly TelegramBotClient _botClient;
        private readonly UpdateQueue _updateQueue;

        public BotUpdateHandler(TelegramBotClient botClient)
        {
            _botClient = botClient;
            _updateQueue = new UpdateQueue();
            StartSendingQales();
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            _updateQueue.Enqueue(update);
        }

        private async void StartSendingQales()
        {
            while (true)
            {
                if (_updateQueue.TryDequeue(out var update))
                {
                    if (update.Message is not { } message)
                        continue;
                    if (message.Text is not { } messageText)
                        continue;

                    var chatId = message.Chat.Id;

                    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

                    await _botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "You said:\n" + messageText);

                    await Task.Delay(30000);
                }
                else
                {
                    await Task.Delay(1000);
                }
            }
        }
    }

    public class UpdateQueue
    {
        private readonly Queue<Update> _queue = new Queue<Update>();
        private readonly object _lock = new object();

        public void Enqueue(Update update)
        {
            lock (_lock)
            {
                _queue.Enqueue(update);
            }
        }

        public bool TryDequeue(out Update update)
        {
            lock (_lock)
            {
                if (_queue.Count > 0)
                {
                    update = _queue.Dequeue();
                    return true;
                }

                update = null;
                return false;
            }
        }
    }
}
