using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManyToMany.Infrastructure.Models
{
    public class PostsToTagsJoinTable
    {
        public int PostsId { get; set; }
        public int TagsId { get; set; }
        public Post Post { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
