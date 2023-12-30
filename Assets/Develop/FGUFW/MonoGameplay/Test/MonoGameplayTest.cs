using System.Collections;
using System.Collections.Generic;
using FGUFW.MonoGameplay;
using UnityEngine;

namespace MonoGameplayTest
{

    public class MonoGameplayTest : MonoBehaviour
    {
        // Start is called before the first frame update
        async void Start()
        {
            await Part.Create<MonoGameplayTestPlay>(default).Create(default);
        }


    }

}