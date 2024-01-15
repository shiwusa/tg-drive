using DataTransfer.Objects;
using DriveServices;
using TgFrontend.Abstractions;
using TgFrontend.Models;
using TgGateway.Abstractions;
using TgGateway.Models;

namespace TgFrontend.Menus;

[TgMenu("settings")]
public class SettingsMenu : MenuBase
{
    private readonly IUserService _userService;
    private readonly IRedirectHandler _redirectHandler;

    public SettingsMenu(
        IBotClient botClient,
        IUserService userService,
        IRedirectHandler redirectHandler)
        : base(botClient)
    {
        _userService = userService;
        _redirectHandler = redirectHandler;
    }


    [TgButtonCallback("set_storage")]
    public async Task MenuBtn_SetStorageChannel(long chatId, IEnumerable<string> parameters)
    {
        await _botClient.SendText(chatId, "You are setting a new storage channel.\n" +
                                          "Keep in ming that this bot should be added to the channel beforehand.\n" +
                                          "If you don't want to do that, please return to main menu.\n\n" +
                                          "Now forward here any message from your new storage channel " +
                                          "and the channel id will be captured automatically:");
    }

    [TgMessageResponse("set_storage")]
    public async Task SetStorageChannel(
        long chatId,
        IEnumerable<string> parameters,
        TgMessage message)
    {
        if (message.ForwardedFromChatId == 0)
        {
            await _botClient.SendText(
                chatId,
                "Please, forward here any message from your new storage channel " +
                "and the channel id will be captured automatically.");
            return;
        }
        
        var verifyMsgId = await _botClient.SendTextUnmanaged(
            message.ForwardedFromChatId,
            $"This message is used by TgDrive bot to verify this chat can be used as a storage.");
        if (verifyMsgId == 0)
        {
            await _botClient.SendText(
                chatId,
                "The chat you specified can't be used as storage!\n\n" +
                "Please, make sure the bot is added to the chat and has the right to send and receive messages.");
            return;
        }

        var newInfo = new UserInfoDto
        {
            Id = chatId,
            StorageChannelId = message.ForwardedFromChatId,
        };
        await _userService.SetUserInfo(newInfo);
        await Open(chatId);
        await _botClient.SendText(
            chatId,
            $"The chat #{message.ForwardedFromChatId} is now set as your storage channel.");
    }

    [TgButtonCallback("main")]
    public async Task MenuBtn_OpenMainMenu(long chatId, IEnumerable<string> parameters)
    {
        await _redirectHandler.Redirect(chatId, typeof(RootMenu));
    }

    private TgKeyboard GetKeyboard()
    {
        var buttons = new List<TgMenuButton>
        {
            new("Set storage channel", MenuBtn_SetStorageChannel),
            new("Back to main menu", MenuBtn_OpenMainMenu)
        };

        var keyboard = GetKeyboard(buttons);
        return keyboard;
    }

    public async Task Open(long chatId)
    {
        var keyboard = GetKeyboard();
        var menu = new MenuData("TgDrive settings", keyboard);
        await _botClient.SendMenu(chatId, menu);
    }
}
