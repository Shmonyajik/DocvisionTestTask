using System.Collections.Generic;

namespace LettersRegistration.Client
{
    public partial class MessageWindow
    {
       
        public class ErrorResponse
        {
            public string type { get; set; }
            public string title { get; set; }
            public int status { get; set; }
            public Dictionary<string, string[]>? errors { get; set; }

            public string? detail { get; set; }
        }
    }
}
