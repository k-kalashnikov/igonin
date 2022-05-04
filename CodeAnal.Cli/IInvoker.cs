using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnal.Cli
{
    public interface IInvoker
    {

        void Invoke(string[] args);
        void OutputToConsole();
        void OutputToFile();

    }
}
