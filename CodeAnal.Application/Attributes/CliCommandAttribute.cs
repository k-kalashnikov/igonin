using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnal.Application.Attributes
{
    public class CliCommandAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
