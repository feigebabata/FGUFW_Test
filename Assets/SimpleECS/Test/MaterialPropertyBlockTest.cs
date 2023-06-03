using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;

namespace FGUFW.SimpleECS.Test
{
    public class MaterialPropertyBlockTest : MonoBehaviour
    {
        int _filpbookIndexId;
        Renderer _render;

        // Start is called before the first frame update
        void Awake()
        {
            _filpbookIndexId = Shader.PropertyToID("_FlipbookIndex");
            _render = GetComponent<Renderer>();
        }

        // Update is called once per frame
        void Update()
        {
            _render.SetMaterialProperty(_filpbookIndexId,Random.Range(0,40f));
        }
    }

}
