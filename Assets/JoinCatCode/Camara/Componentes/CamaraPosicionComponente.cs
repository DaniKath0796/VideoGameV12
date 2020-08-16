
using System;
using Unity.Mathematics;
using UnityEngine;

namespace JoinCatCode
{
   [Serializable]
    public class CamaraPosicionComponente : MonoBehaviour
    {
        private static CamaraPosicionComponente instancia;

        public static Vector3Int posicionIsometrica;
        public static float3 posicionNormal;
        public static int posicionNumerica;

        public  Vector3Int posicionIsometricaDebug;
        public  float3 posicionNormalDebug;
        public  int posicionNumericaDebug;

        public void Update()
        {
            posicionIsometricaDebug = posicionIsometrica;
            posicionNormalDebug     = posicionNormal;
            posicionNumericaDebug   = posicionNumerica;
        }
        
    }
}