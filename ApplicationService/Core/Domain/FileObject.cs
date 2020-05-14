using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationService.Core.Domain
{
    public class FileObject: ShareableResource
    {
        public int Id { get; set; }
        public string Url { get; set; }
    }
}
