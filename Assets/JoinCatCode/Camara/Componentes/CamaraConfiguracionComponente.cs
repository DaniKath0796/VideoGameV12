using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JoinCatCode
{
    public class CamaraConfiguracionComponente : MonoBehaviour
    {
        public float globalZoomVelocidad;
        public float globalZoomMaximo;
        public float globalZoomMinimo;
        public float globalAnchoBorde;
        public float globalDesplazamientoVelocidad;

        public Vector3Int globalmapaTam;
        public Vector3 globalazulejoTam;
        /*-------------------------------*/
        public static float ZoomVelocidad;
        public static float ZoomMaximo;
        public static float ZoomMinimo;
        public static float AnchoBorde;
        public static float DesplazamientoVelocidad;

        public static Vector3Int mapaTam;
        public static Vector3 azulejoTam;

        private void Start()
        {
            ZoomVelocidad           = globalZoomVelocidad;
            ZoomMaximo              = globalZoomMaximo;
            ZoomMinimo              = globalZoomMinimo;
            AnchoBorde              = globalAnchoBorde;
            DesplazamientoVelocidad = globalDesplazamientoVelocidad;
            mapaTam                 = globalmapaTam;
            azulejoTam              = globalazulejoTam;
        }
    }
}