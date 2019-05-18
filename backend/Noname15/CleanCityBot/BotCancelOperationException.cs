using System;

namespace CleanCityBot
{
    public class BotCancelOperationException : Exception
    {
        public BotCancelOperationException(string message) : base(message)
        {
        }
    }
}