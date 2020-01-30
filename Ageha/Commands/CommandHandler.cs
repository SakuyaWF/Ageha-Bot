using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ageha.Commands
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _service;
        public char Prefix { get; protected set; }
        public bool MessageOnError { get; set; }

        public CommandHandler(DiscordSocketClient client, CommandService commands, char prefix)
        {
            this._client = client;
            this._service = commands;
            this.Prefix = prefix;
            this.MessageOnError = false;
        }

        public async Task InstallCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _service.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: null);
        }

        private async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            // Don't process system messages
            SocketUserMessage message = socketMessage as SocketUserMessage;

            // Don't process null messages
            if (message == null)
            {
                return;
            }

            if(message.Author == _client.CurrentUser)
            {
                return;
            }

            // Track where the prefix ends and the command begins
            int argumentPos = 0;

            // Make sure that the commands wasn't issued by a bot and that it has the prefix
            if( !(message.HasCharPrefix(this.Prefix, ref argumentPos) || message.HasMentionPrefix(_client.CurrentUser, ref argumentPos) || message.Author.IsBot) )
            {
                return;
            }

            // Create a WebSocket based command context based on the message
            SocketCommandContext commandContext = new SocketCommandContext(_client, message);

            // The result is just an object stating that the command was executed successfully
            var result = await _service.ExecuteAsync(context: commandContext, argPos: argumentPos, services: null);

            // Optional method for giving the user an error message (disabled by default to prevent spam)
            if (this.MessageOnError && !result.IsSuccess)
            {
                //await commandContext.Channel.SendMessageAsync(result.ErrorReason);
                Console.WriteLine($"The command {message.ToString()} from {message.Author} , failed because: {result.ErrorReason}");
            }
        }
    }
}
