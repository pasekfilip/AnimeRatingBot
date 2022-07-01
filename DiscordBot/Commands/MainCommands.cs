using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class MainCommands : BaseCommandModule
    {
        [Command("rate")]
        public async Task Rate(CommandContext ctx)

        {
            var dd = DiscordEmoji.FromGuildEmote(ctx.Client, 850708320830226503);
            var interactivity = ctx.Client.GetInteractivity();
            var anime = await ctx.Client.GetChannelAsync(849971302797279265);
            var addedAnime = await ctx.Client.GetChannelAsync(850711592957509633);
            var namesOfEmojis = new string[] { ":Rate5outof5:", ":Rate4outof5:", ":Rate3outof5:", ":Rate2outof5:", ":Rate1outof5:" };
            var listOfRatings = new List<DiscordEmoji>();
            
            var message = await interactivity.WaitForMessageAsync(x => x.Channel.Id == anime.Id).ConfigureAwait(false);


            foreach (var item in namesOfEmojis)
            {
                
                listOfRatings.Add(DiscordEmoji.FromName(ctx.Client, item));
            }

            await message.Result.CreateReactionAsync(listOfRatings[0]).ConfigureAwait(false);
            await message.Result.CreateReactionAsync(listOfRatings[1]).ConfigureAwait(false);
            await message.Result.CreateReactionAsync(listOfRatings[2]).ConfigureAwait(false);
            await message.Result.CreateReactionAsync(listOfRatings[3]).ConfigureAwait(false);
            await message.Result.CreateReactionAsync(listOfRatings[4]).ConfigureAwait(false);

            await addedAnime.SendMessageAsync(message.Result.Embeds[0].Title + ": " + message.Result.JumpLink).ConfigureAwait(false);


            await ctx.Channel.DeleteMessageAsync(ctx.Message);
        }

        [Command("clear")]
        public async Task Clear(CommandContext ctx, int howManyMessages)
        {
            await DeleteCommand(ctx);

            var messages = await ctx.Channel.GetMessagesAsync(howManyMessages).ConfigureAwait(false);
            await ctx.Channel.DeleteMessagesAsync(messages);
        }


        private async Task DeleteCommand(CommandContext ctx)
        {
            var command = ctx.Message;
            await ctx.Channel.DeleteMessageAsync(command);
        }
    }
}
