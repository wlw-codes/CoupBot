﻿namespace CoupBot.Common.Structures
{
    public class Credentials
    {
        public string Token { get; set; }
        public string DatabaseConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public ulong[] OwnerIds { get; set; }
    }
}