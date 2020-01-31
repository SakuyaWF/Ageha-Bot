namespace Ageha.Util
{
    public class Games
    {
        public static string Rps(string bot, string player_choose)
        {
            string result = "Draw";

            switch (bot)
            {
                case "Rock":
                    if (player_choose == "paper")
                    {
                        result = "You win...";
                    }
                    else
                    {
                        result = "Too bad, I win";
                    }
                    break;

                case "Paper":
                    if (player_choose == "scissors")
                        result = "You win...";
                    else
                        result = "Too bad, I win";
                    break;

                case "Scissors":
                    if (player_choose == "rock")
                        result = "You win...";
                    else
                        result = "Too bad, I win";
                    break;
            }

            return result;
        }
    }
}