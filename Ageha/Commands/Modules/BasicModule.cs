using Ageha.Util;
using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ageha.Commands.Modules
{
    public class BasicModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo) => ReplyAsync(echo);

        [Command("summon")]
        [Summary("*smug intensifies*")]
        public async Task SummonAsync()
        {
            EmbedBuilder smug = new EmbedBuilder();

            smug.Title = "I AM AWAKE !";
            smug.ImageUrl = @"https://i.imgur.com/gJvYUOM.jpg";

            smug.Color = Color.DarkRed;

            await Context.Channel.SendMessageAsync(embed: smug.Build());
        }

        [Command("ask")]
        [Summary("Send a question and she will answer")]
        public Task AskAsync([Remainder] [Summary("The question")] string question = null)
        {
            string response = "Try typing something, fag.";

            if(question != null)
            {
                response = Utils.Choose("Yes", "No", "Maybe");
            }

            return ReplyAsync(response);
        }
    }
}
