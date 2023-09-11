using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAFFLE.Utils
{
    public enum MsgType
    {
        AppExit = 0x0001,
        Other = 0x0002,
        DocSelRemove = 0x1001,
        PhotoRemove = 0x1002,
    }
}
