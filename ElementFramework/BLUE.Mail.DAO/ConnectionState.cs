using System;
using System.Collections.Generic;
using System.Text;

namespace BLUE.Mail.DAO
{
    public enum ConnectionState
    {
        Disconnected,
        Connecting,
        Authorization,
        Transaction
    }
}
