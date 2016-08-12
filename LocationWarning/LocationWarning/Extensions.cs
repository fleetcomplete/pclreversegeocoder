using System;


namespace LocationWarning
{
    public static class Extensions
    {
        public static string ToDateString(this DateTime value)
        {
            return value.ToLocalTime().ToString("MMM dd - hh:mm:ss");
        }


        public static string ToDateString(this DateTime? value, string empty = "") 
        {
            if (value == null)
                return empty;

            return value.Value.ToDateString();
        }   


        public static string ToDateString(this DateTimeOffset? value, string empty = "") 
        {
            if (value == null)
                return empty;

            return value.Value.LocalDateTime.ToString("MMM dd - hh:mm:ss");
        }    
    }
}