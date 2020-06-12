namespace Ageha.Util
{
    public class Games
    {
        /// <summary>
        /// Rock, paper, yuri
        /// </summary>
        /// <param name="bot">The bot choice</param>
        /// <param name="player">The player choice</param>
        public static string Rps(string bot, string player)
        {
            // The default result
            string result = "Draw";

            // Switch based on the bot choice
            switch (bot)
            {
                case "Rock":
                    if (player == "paper")
                    {
                        result = "You win...";
                    }
                    else
                    {
                        result = "Too bad, I win";
                    }
                    break;

                case "Paper":
                    if (player == "scissors")
                        result = "You win...";
                    else
                        result = "Too bad, I win";
                    break;

                case "Scissors":
                    if (player == "rock")
                        result = "You win...";
                    else
                        result = "Too bad, I win";
                    break;
            }

            return result;
        }
    }
}