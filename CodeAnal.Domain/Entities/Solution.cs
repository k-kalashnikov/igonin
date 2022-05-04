using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnal.Domain.Entities
{
    public class Solution
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<Project> Projects { get; set; }
    }
}
