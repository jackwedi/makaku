using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IGameManager
{
    ManagerStatus status { get; }
    void Startup();


}
