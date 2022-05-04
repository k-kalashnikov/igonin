using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnal.Domain.Entities
{
    public class Project
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public ICollection<Class> Classes { get; set; }

        public override string ToString()
        {
            return $"{Name}\n {string.Join("\n", Classes.Select(m => m.ToString()))}";
        }
    }
}
