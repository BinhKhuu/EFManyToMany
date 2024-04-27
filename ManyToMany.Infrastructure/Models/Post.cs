using System;
using System.Collections.Generic;

namespace ManyToMany.Infrastructure.Models;

public partial class Post
{
    public int Id { get; set; }
    public virtual ICollection<Tag> Tags { get; set; }
}
