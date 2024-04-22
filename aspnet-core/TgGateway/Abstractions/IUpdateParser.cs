using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IUpdateHandler = Telegram.Bot.Polling.IUpdateHandler;

namespace TgGateway.Abstractions;

public interface IUpdateParser : Telegram.Bot.Polling.IUpdateHandler
{
}
