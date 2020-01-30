using Ageha.Commands;
using Ageha.Util;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

using System;
using System.Threading.Tasks;

namespace Ageha
{
    class Ageha
    {
        /// <summary>
        /// SocketClient is the type of connection the bot makes to Discord's API
        /// </summary>
        private DiscordSocketClient _client;

        private CommandHandler _commandHandler;
        private CommandService _commandService;

        /// <summary>
        /// The main method of any program, redirected to the asynchronous version
        /// </summary>
        static void Main(string[] args) => new Ageha().MainAsync().GetAwaiter().GetResult();

        /// <summary>
        /// The async version of the main method
        /// </summary>
        /// <returns></returns>
        public async Task MainAsync()
        {
            // Instantiate Discord Socket Client and hook the logging method
            _client = new DiscordSocketClient();
            _client.Log += Log;

            await _client.LoginAsync(TokenType.Bot, JsonWrapper.ReadJSON(@"D:\Development\_Projects\Ageha\Ageha\Resources\auth.json").Value<string>("token"));
            await _client.StartAsync();

            // Activate the commands and modules
            _commandService = new CommandService();
            _commandHandler = new CommandHandler(_client, _commandService, '|');
            _commandHandler.MessageOnError = true;

            await _commandHandler.InstallCommandsAsync();

            _client.Ready += OnReady;
            //_client.MessageReceived += OnMessageReceived;

            // Block this task until the bot is closed (prevent the main method from closing upon completing one task)
            await Task.Delay(-1);
        }

        private Task OnReady()
        {
            Log(LogSeverity.Info, $"Connected to these servers as '{_client.CurrentUser.Username}': ");

            foreach (var guild in _client.Guilds)
            {
                Log(LogSeverity.Info, $"- {guild.Name}");
            }

            // Choose between the activities and change to the chosen one
            string[] options = new string[] { "with your waifu", "with lolis", "with myself", "with your heart" };
            _client.SetGameAsync(Utils.Choose(options));
            Log(LogSeverity.Info, $"Actiity set to 'Playing {_client.Activity.Name}'");

            return Task.CompletedTask;
        }

        private async Task OnMessageReceived(SocketMessage message)
        {
            if (message.Content == "!ping")
            {
               await message.Channel.SendMessageAsync("Pong");
            }
        }

        /// <summary>
        /// The logging method of the bot
        /// </summary>
        /// <param name="msg">A discord log message</param>
        /// <returns>A completed task</returns>
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private Task Log(LogSeverity logSeverity, string message, string source = "Debugging") => Log(new LogMessage(logSeverity, source, message));
    }
}
