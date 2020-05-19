namespace RicModel.Helpers
{
    public static class CommonFunctions
    {
        public static string GetSuffix(string day)
        {
            string suffix = "th";

            if (int.Parse(day) < 11 || int.Parse(day) > 20)
            {
                day = day.ToCharArray()[day.ToCharArray().Length - 1].ToString();
                switch (day)
                {
                    case "1":
                        suffix = "st";
                        break;
                    case "2":
                        suffix = "nd";
                        break;
                    case "3":
                        suffix = "rd";
                        break;
                }
            }

            return suffix;
        }

    }

}
