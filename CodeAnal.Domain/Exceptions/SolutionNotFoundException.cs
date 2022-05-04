using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnal.Domain.Exceptions
{
    public class SolutionNotFoundException : Exception
    {
        public SolutionNotFoundException(string message) : base(message)
        {

        }
    }

}
