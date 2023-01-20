using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseApplication.WebClient
{
    public class PostEntityModel<T>
    {
        public string PostUrl { get; set; } = null!;
        public T? PostContent { get; set; }

    }
}
