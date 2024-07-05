using Telegram.Bot;
using Telegram.Bot.Types;

namespace CherryBot;

public class TgBot
{
    private readonly ILogger _logger;
    
    // ReSharper disable once ConvertToPrimaryConstructor
    public TgBot(ILogger<TgBot> logger)
    {
        _logger = logger;
    }
    
    private static async Task HandleUpdate(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        if (update.Message?.Text is null) return; // we want only updates about new Text Message
        var msg = update.Message;
        
        Console.WriteLine($"Received message '{msg.Text}' in {msg.Chat}");
        
        var isDecentCommand = Enum.TryParse<Command>(msg.Text.Split('/').Last(), true, out var command);
        if (!isDecentCommand) return;

        var respond = command switch
        {
            Command.Start => "Добро пожаловать в самый лучший тг бот Димы и Миши - CherryBot\ud83c\udf52!",
            Command.Hello => $"Привет, {msg.From?.Username ?? msg.From?.FirstName}! \ud83e\udde1\ud83d\udc9c\ud83d\udc9b",
            Command.Whoami => $"{msg.From?.Username ?? msg.From?.FirstName} хорооооший \ud83e\udd32",
            Command.Idea => """
                            В общем, это проба пера. Есть идея написать бота, но нет идеи что он будет делать.
                            У меня в голове крутится что-то связанное с Айзеком, к примеру, взять твои стикеры
                            и сделать что-то типа гайда... Подумай, у тебя должна быть куча идей 💡😍
                            """,
            _ => throw new Exception("Wrong command")
        };
        
        await bot.SendTextMessageAsync(msg.Chat, respond, cancellationToken: ct);
    }

    public async Task RunAsync()
    {
        using var cts = new CancellationTokenSource();
        
        var bot = new TelegramBotClient("7247531326:AAEkVAG7VB2X6bo0hu4-PoNOlL20V0iClDk");
        
        bot.StartReceiving(HandleUpdate, async (bot, ex, ct) => _logger.LogError(ex.Message), cancellationToken: cts.Token);

        var me = await bot.GetMeAsync(cts.Token);
        Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
        Console.ReadLine();
        await cts.CancelAsync(); // stop the bot
    }
}