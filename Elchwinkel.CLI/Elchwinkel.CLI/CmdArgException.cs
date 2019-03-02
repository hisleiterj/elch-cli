using System;

namespace Elchwinkel.CLI
{
    public class CmdArgException : Exception
    {
        public CmdArgException(string message) : base(message){}
    }
}