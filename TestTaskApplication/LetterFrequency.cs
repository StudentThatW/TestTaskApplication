using System;
using System.Collections.Generic;

#nullable disable

namespace TestTaskApplication
{
    public partial class LetterFrequency
    {
        public string Letter { get; set; }
        public int? Frequency { get; set; }
        public int MessageNumber { get; set; }
        public int OwnerId { get; set; }

        public virtual User Owner { get; set; }
    }
}
