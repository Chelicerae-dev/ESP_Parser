using System;

namespace ESP_Parser
{
    public class CsvLine
    {
        public string name { get; set; }
        public string model { get; set; }
        public string price { get; set; }
        public string categories { get; set; }
        public int quantity { get; set; }
        public string manufacturer { get; set; }
        public string description { get; set; }
        public string attributes { get; set; }
        public string attributes_group { get; set; }
        public string options { get; set; }
        public string option_type { get; set; }
        public string images { get; set; }
    }
}
