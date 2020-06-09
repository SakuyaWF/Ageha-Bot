using Discord;
using Discord.Commands;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Ageha.Commands.Modules
{
    public class QuoteModule : ModuleBase<SocketCommandContext>
    {
        private readonly string MessageBaseLink = @"https://discordapp.com/channels/";

        [Command("quote", false)]
        [Summary("Quotes a message")]
        [Alias("q")]
        public async Task QuoteAsync([Summary("The message link to quote")] string link, [Remainder][Summary("The message to follow the quote.")] string quoteMessage = "")
        {
            // Initializes the control boolean
            bool success = false;

            // Remove the message base part and splits all the IDs into an array
            string[] stringIDs = link.Replace(MessageBaseLink, "").Split('/');

            // Creates a new array to hold the IDs
            ulong[] ids = new ulong[3];

            // Parses all the IDs from strings to unsigned longs and saves the result into the array, also it stores the successes or failures into the boolean
            for (int i = 0; i < 3; i++)
            {
                success = UInt64.TryParse(stringIDs[i], out ids[i]);
            }

            // If the parse fails, delete the original user command message
            if (!success)
            {
                await Context.Message.DeleteAsync();
                await Ageha.Log(LogSeverity.Error, "Could not parse some ID");
                return;
            }

            // Free up memory
            stringIDs = null;

            // Tries to get the guild from the ID
            var sourceGuild = Context.Client.GetGuild(ids[0]);

            // If the guild is null, reply to the user
            if (sourceGuild == null)
            {
                await ReplyAsync("I can't quote this message, because I'm not in the server");
                return;
            }

            // Tries to get the channel from the ID
            var sourceChannel = sourceGuild?.GetTextChannel(ids[1]);

            // If the channel is null, reply to the user
            if (sourceChannel == null)
            {
                await ReplyAsync("I can't quote this message, because I can't view the channel");
                return;
            }

            // Tries to get the message from the ID
            var quotedMessage = sourceChannel?.GetMessageAsync(ids[2]).Result;

            // If the message is null, reply to the user
            if (quotedMessage == null)
            {
                await ReplyAsync("I can't quote this message, because it doesn't exist");
                return;
            }

            // Free up some memory
            ids = null;

            // Initialize the embed builder, sets the author as the user from the quoted message and their avatar,
            // put the message in the description with a follow up link, also puts the channel, server and timestamp
            EmbedBuilder response = new EmbedBuilder();
            response.WithAuthor($"{quotedMessage.Author.ToString()}", quotedMessage.Author.GetAvatarUrl());
            response.WithDescription($"{quotedMessage.Content} \n\n [*Go to message*]({link})");
            response.WithFooter($"{sourceChannel.Name} | {sourceGuild.Name} | {quotedMessage.Timestamp.ToString()}", sourceGuild.IconUrl);
            response.Color = Color.LighterGrey;

            // Sends the message back to the user
            if (string.IsNullOrWhiteSpace(quoteMessage))
            {
                await ReplyAsync($"*Quoted by {Context.Message.Author.Mention}*", embed: response.Build());
            }
            else
            {
                await ReplyAsync($"*Quoted by {Context.Message.Author.Mention}*:\n> {quoteMessage}", embed: response.Build());
            }

            // Delete the original user command message
            await Context.Message.DeleteAsync();
        }
    }
}