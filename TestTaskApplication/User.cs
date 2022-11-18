using System;
using System.Collections.Generic;

#nullable disable

namespace TestTaskApplication
{
    public partial class User
    {
        public User()
        {
            LetterFrequencies = new HashSet<LetterFrequency>();
        }

        public int UserId { get; set; }

        public virtual ICollection<LetterFrequency> LetterFrequencies { get; set; }
    }
}
