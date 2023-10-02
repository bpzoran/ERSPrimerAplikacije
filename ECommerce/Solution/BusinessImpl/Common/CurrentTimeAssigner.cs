using Application.Common.Interfaces;
using System;

namespace BusinessImpl.Common
{
    public class CurrentTimeAssigner : ITimeAssigner
    {
        public DateTime DateTime { get => DateTime.Now; }
    }
}
