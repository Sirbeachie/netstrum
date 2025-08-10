using System;

namespace netstrum_gui
{
    public class Packet
    {
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public string Data { get; set; } = string.Empty;
    }
}
