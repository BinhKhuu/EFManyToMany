using System;
using System.Collections.Generic;

namespace ManyToMany.Infrastructure.Models;

public partial class Tag
{
    public int Id { get; set; }
    public virtual ICollection<Post> Posts { get; set; }
}
