using System;
using System.Collections.Generic;

namespace ManyToMany.Infrastructure.Models;

public partial class Tag
{
    public int Id { get; set; }
    // Navigation (skip) collection the new way to map many-to-many
    public virtual ICollection<Post> Posts { get; set; }
}
