using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfiguracionMapa : MonoBehaviour
{
    public Vector3Int globalmapaTam;
    public Vector3 globalazulejoTam;

    public static Vector3Int mapaTam;
    public static Vector3 azulejoTam;
    void Start()
    {
        mapaTam    = globalmapaTam;
        azulejoTam = globalazulejoTam;
    }

  
}
