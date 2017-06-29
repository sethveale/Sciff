using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sciff.Tests.LibraryDomain
{
    public class Topic
    {
        [Key]
        [MaxLength(Constants.LongTextLength)]
        public string Name { get; set; }

        public virtual ICollection<Topic> Aliases { get; set; }

        public virtual ICollection<Topic> SubTopics { get; set; }

        public virtual ICollection<Page> Pages { get; set; }
    }
}