using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dixy
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
            private void Awake()
            {
                DontDestroyOnLoad(gameObject);
            }
    }
}


