using DiscordBot.Commands;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public async Task RunAsync()
        {
            var json = string.Empty;
            using (var sr = new StreamReader("../../../config.json", new UTF8Encoding(false)))
            {
                json = await sr.ReadToEndAsync().ConfigureAwait(false);
            }
            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            Client = new DiscordClient(new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            });

            Client.Ready += OnClientReady;
            Client.MessageCreated += OnMessageCreated;
            Client.MessageUpdated += OnMessageUpdate;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = false,
                DmHelp = true
            };

            Client.UseInteractivity(new InteractivityConfiguration()
            {
                Timeout = TimeSpan.FromMinutes(3)
            });



            Commands = Client.UseCommandsNext(commandsConfig);
            Commands.RegisterCommands<MainCommands>();
            await Client.ConnectAsync();



            await Task.Delay(-1);
        }


        private async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs e)
        {
            var animeToRate = await client.GetChannelAsync(849971302797279265);
            var mee6 = await client.GetUserAsync(159985870458322944);

            if (e.Channel == animeToRate && e.Author == mee6)
            {
                var namesOfEmojies = new string[] { ":Rate5outof5:", ":Rate4outof5:", ":Rate3outof5:", ":Rate2outof5:", ":Rate1outof5:" };
                var listOfRatings = new List<DiscordEmoji>();
                var message = e.Message;
                foreach (var nameOfEmoji in namesOfEmojies)
                {
                    listOfRatings.Add(DiscordEmoji.FromName(client, nameOfEmoji));
                }
                
                await message.CreateReactionAsync(listOfRatings[0]);
                await message.CreateReactionAsync(listOfRatings[1]);
                await message.CreateReactionAsync(listOfRatings[2]);
                await message.CreateReactionAsync(listOfRatings[3]);
                await message.CreateReactionAsync(listOfRatings[4]);

            }
        }
        public async Task OnMessageUpdate(DiscordClient client, MessageUpdateEventArgs e)
        {
            var addedAnime = await client.GetChannelAsync(850711592957509633);
            await addedAnime.SendMessageAsync(e.Message.Embeds[0].Title + ": " + e.Message.JumpLink).ConfigureAwait(false);
        }

        private Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
