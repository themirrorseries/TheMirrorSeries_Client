using UnityEngine;
using System.Collections;
using System.Linq;
using System.Text;

interface IHandler
{
    void MessageReceive(SocketModel model);
}
