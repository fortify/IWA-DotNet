using System;

namespace MicroFocus.InsecureWebApp.Models
{
    [Serializable]
    public class tmpOrder
    {
        public string pid { get; set; }
        public int quantity { get; set; }
    }
}
