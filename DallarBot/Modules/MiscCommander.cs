﻿using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;
using Discord;

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using System;

using DallarBot.Classes;
using DallarBot.Services;

namespace DallarBot.Modules
{
    public class MiscCommander : ModuleBase<SocketCommandContext>
    {
        private readonly GlobalHandlerService global;

        public MiscCommander(GlobalHandlerService _global)
        {
            global = _global;
        }

        //[Command("life")]
        //public async Task GetLife()
        //{
        //    long localTicks = Context.Guild.CreatedAt.ToLocalTime().Ticks;
        //    DateTime localDate = new DateTime(localTicks);
        //    await Context.Channel.SendMessageAsync("Server was created on: " + localDate.ToLongDateString() + "!");
        //}

        //[Command("count")]
        //public async Task GetUserCount()
        //{
        //    await Context.Message.DeleteAsync();
        //    await Context.Channel.SendMessageAsync("Member count is: " + Context.Guild.MemberCount);
        //}

        //[Command("ping")]
        //public async Task PingPong()
        //{
        //    await Context.Channel.SendMessageAsync("Pong!" + Environment.NewLine + "Discord Server responded in: " + global.discord.Latency.ToString() + "ms");
        //}

        //[Command("help")]
        //public async Task GetHelp()
        //{
        //    if (settings.dallarSettings.helpCommands != null)
        //    {
        //        string helpString = "```" + Environment.NewLine + "DALLAR COMMANDS" + Environment.NewLine;
        //        foreach (var item in settings.dallarSettings.helpCommands)
        //        {
        //            helpString += item.command + " - " + item.description + Environment.NewLine;
        //        }
        //        helpString += "```";
        //        await Context.User.SendMessageAsync(helpString);
        //    }
        //    await Context.Message.DeleteAsync();
        //}

        [Command("giveafuck")]
        public async Task GiveAFuck()
        {
            var user = Context.User as SocketGuildUser;
            if (global.isUserAdmin(user) || global.isUserModerator(user) || global.isUserDevTeam(user))
            {
                await Context.Channel.SendMessageAsync(":regional_indicator_g: :regional_indicator_i: :regional_indicator_v: :regional_indicator_e: :a: :regional_indicator_f: :regional_indicator_u: :regional_indicator_c: :regional_indicator_k:");
            }
        }

        [Command("dal-btc")]
        public async Task DallarToBTC()
        {
            var user = Context.User as SocketGuildUser;

            var client = new WebClient();
            var jsonString = await client.DownloadStringTaskAsync("https://digitalprice.io/markets/get-currency-summary?currency=BALANCE_COIN_BITCOIN");

            var dalConverter = DallarSerialization.FromJson(jsonString);

            for (int i = 0; i < dalConverter.Length; i++)
            {
                if (dalConverter[i].MiniCurrency == "dal-btc")
                {
                    Console.WriteLine(Environment.NewLine +
                        "Dallar Price (DAL / BTC): " + dalConverter[i].Price.ToString() + Environment.NewLine +
                        "24 Hour Low: " + dalConverter[i].Low.ToString() + Environment.NewLine +
                        "24 Hour High: " + dalConverter[i].High.ToString() + Environment.NewLine +
                        "% Change: " + dalConverter[i].PriceChange + Environment.NewLine +
                        "24 Hour Volume: " + dalConverter[i].VolumeMarket.ToString());

                    await Context.Channel.SendMessageAsync("Dallar Price (DAL / BTC): " + (decimal.Round(dalConverter[i].Price, 9, MidpointRounding.AwayFromZero) + Environment.NewLine +
                                "24 Hour Low: " + (dalConverter[i].Low.ToString() + Environment.NewLine +
                                "24 Hour High: " + dalConverter[i].High.ToString() + Environment.NewLine +
                                "% Change: " + dalConverter[i].PriceChange + Environment.NewLine +
                                "24 Hour Volume: " + dalConverter[i].VolumeMarket.ToString())));
                }
            }
        }

        [Command("dal-usd")]
        public async Task DallarToUSD()
        {
            var user = Context.User as SocketGuildUser;

            var client = new WebClient();
            var jsonString = await client.DownloadStringTaskAsync("https://digitalprice.io/markets/get-currency-summary?currency=BALANCE_COIN_BITCOIN");
            var btcPrice = await client.DownloadStringTaskAsync("https://blockchain.info/tobtc?currency=USD&value=1");

            var dalConverter = DallarSerialization.FromJson(jsonString);

            for (int i = 0; i < dalConverter.Length; i++)
            {
                if (dalConverter[i].MiniCurrency == "dal-btc")
                {
                    Console.WriteLine(Environment.NewLine +
                        "DAL / USD: $" + decimal.Round((dalConverter[i].Price / Convert.ToDecimal(btcPrice)), 8, MidpointRounding.AwayFromZero));

                    await Context.Channel.SendMessageAsync("DAL / USD: $" + decimal.Round((dalConverter[i].Price / Convert.ToDecimal(btcPrice)), 9, MidpointRounding.AwayFromZero));
                }
            }
        }
    }
}
