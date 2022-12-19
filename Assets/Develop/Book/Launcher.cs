using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;
using FGUFW.GamePlay;
using System.IO;

namespace Book
{
    public class Launcher : MonoBehaviour
    {
        // Start is called before the first frame update
        async void Start()
        {
            await new BookPlay().Creating(null);
        }

    }
}
