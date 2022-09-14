namespace TimeMeShared.Classes.ApiOpenMeteo
{
    public class ApiOpenMeteo
    {
        public static string WmoCodeToString(float wmoCode)
        {
            switch (wmoCode)
            {
                case 0:
                    return "Clear sky";
                case 1:
                    return "Mainly clear";
                case 2:
                    return "Partly cloudy";
                case 3:
                    return "Cloudy";
                case 45:
                    return "Fog";
                case 48:
                    return "Freezing fog";
                case 51:
                    return "Light drizzle";
                case 53:
                    return "Moderate drizzle";
                case 55:
                    return "Dense drizzle";
                case 56:
                    return "Light chill drizzle";
                case 57:
                    return "Dense chill drizzle";
                case 61:
                    return "Light rain";
                case 63:
                    return "Moderate rain";
                case 65:
                    return "Heavy rain";
                case 66:
                    return "Light chill rain";
                case 67:
                    return "Heavy chill rain";
                case 71:
                    return "Light snow";
                case 73:
                    return "Moderate snow";
                case 75:
                    return "Heavy snow";
                case 77:
                    return "Snow grains";
                case 80:
                    return "Light rain shower";
                case 81:
                    return "Moderate rain shower";
                case 82:
                    return "Heavy rain shower";
                case 85:
                    return "Light snow shower";
                case 86:
                    return "Heavy snow shower";
                case 95:
                    return "Thunderstorm";
                case 96:
                    return "Thunderstorm, light hail";
                case 99:
                    return "Thunderstorm, heavy hail";
                default:
                    return "Unknown";
            }
        }
    }
}