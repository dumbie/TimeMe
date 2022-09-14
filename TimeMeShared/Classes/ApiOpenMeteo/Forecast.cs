namespace TimeMeShared.Classes.ApiOpenMeteo
{
    public class Forecast
    {
        public current_weather current_weather { get; set; }
        public daily_units daily_units { get; set; }
        public daily daily { get; set; }
    }

    public class current_weather
    {
        public float temperature { get; set; }
        public string time { get; set; }
        public float weathercode { get; set; }
        public float winddirection { get; set; }
        public float windspeed { get; set; }
    }

    public class daily_units
    {
        public string precipitation_sum { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
        public string temperature_2m_max { get; set; }
        public string temperature_2m_min { get; set; }
        public string time { get; set; }
        public string weathercode { get; set; }
        public string winddirection_10m_dominant { get; set; }
        public string windspeed_10m_max { get; set; }
    }

    public class daily
    {
        public float[] precipitation_sum { get; set; }
        public string[] sunrise { get; set; }
        public string[] sunset { get; set; }
        public float[] temperature_2m_max { get; set; }
        public float[] temperature_2m_min { get; set; }
        public string[] time { get; set; }
        public float[] weathercode { get; set; }
        public float[] winddirection_10m_dominant { get; set; }
        public float[] windspeed_10m_max { get; set; }
    }
}