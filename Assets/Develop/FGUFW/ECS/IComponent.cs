using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;
using System;

namespace FGUFW.ECS
{

    public interface IComponent:IStruct,IDisposable
    {
        int EntityUId { get; set; }
        int CompType { get; set; }
    }

}
