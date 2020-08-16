using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Unity.Mathematics;
using System;

namespace JoinCatCode
{
   
    public class CamaraMovimientoSistema : MonoBehaviour
    {
        /*-------------*/
        public event EventHandler<CamaraPosicionComponente> NotificarPantalla;
        public  Camera                          CamaraIsometrica;        
        private Transform                      PosicionCamara;

        public static Camera CamaraLocal;
        protected void Start()
        {
            CamaraLocal = CamaraIsometrica;
            PosicionCamara = this.transform;
            CamaraGrid.instanciar().actualizarGrid(ConfiguracionMapa.mapaTam, ConfiguracionMapa.azulejoTam);
        }

        
        protected  void Update()
        {           
            if (Input.mousePosition.x <= 0 || Input.mousePosition.x >= Screen.width || Input.mousePosition.y >= Screen.height || Input.mousePosition.y <= 0)
            {

                return;
            }
            else
            {
                Zoom();
                Movimiento(ref PosicionCamara);
             //   MovimientoIsometrico();                 
            }           
        }

        void Zoom()
        {
            //CamaraIsometrica.transform.localPosition;
            bool flag = true;
            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
            float signo = 0;
            if (scroll> 0)
            {
                signo = 1;
            }
            if (scroll < 0)
            {
                signo = -1;
            }
            if (CamaraIsometrica.orthographicSize <= CamaraConfiguracionComponente.ZoomMinimo && signo != 0 && flag)
            {
               
                CamaraIsometrica.orthographicSize = CamaraConfiguracionComponente.ZoomMinimo;
               // CamaraIsometrica.transform.localPosition = new Vector3(CamaraConfiguracionComponente.ZoomMinimo * (-1.0f), CamaraConfiguracionComponente.ZoomMinimo, CamaraConfiguracionComponente.ZoomMinimo * (-1.0f) );
                   
            }

            if (CamaraIsometrica.orthographicSize >= CamaraConfiguracionComponente.ZoomMaximo && signo != 0 && flag)
            {
                
                CamaraIsometrica.orthographicSize = CamaraConfiguracionComponente.ZoomMaximo;
              //  CamaraIsometrica.transform.localPosition = new Vector3(CamaraConfiguracionComponente.ZoomMaximo * (-1.0f), CamaraConfiguracionComponente.ZoomMaximo, CamaraConfiguracionComponente.ZoomMaximo * (-1.0f));
               
            }
            if ((CamaraIsometrica.orthographicSize >= CamaraConfiguracionComponente.ZoomMinimo) && (CamaraIsometrica.orthographicSize <= CamaraConfiguracionComponente.ZoomMaximo) && signo != 0 && flag)
            {
             
                float valor = signo * CamaraConfiguracionComponente.ZoomVelocidad; 
                CamaraIsometrica.orthographicSize += valor;
                if (CamaraIsometrica.orthographicSize != CamaraConfiguracionComponente.ZoomMinimo && CamaraIsometrica.orthographicSize != CamaraConfiguracionComponente.ZoomMaximo)
                {
                  //  CamaraIsometrica.transform.localPosition += new Vector3(valor * (-1.0f), valor, valor * (-1.0f));
                    
                }
             
            }          
        }

        void Movimiento(ref Transform PivoteCamara)
        {
            
            float AnchoBorde = CamaraConfiguracionComponente.AnchoBorde;
            float VelocidadMovimiento = CamaraConfiguracionComponente.DesplazamientoVelocidad;
            float Delta = Time.deltaTime;


      

            if (Input.mousePosition.x <=0 || Input.mousePosition.x >= Screen.width || Input.mousePosition.y >= Screen.height || Input.mousePosition.y <= 0)
            {
                return;
            }
      

            if (Input.mousePosition.y > (Screen.height - AnchoBorde))
            {
                PivoteCamara.position += new Vector3(VelocidadMovimiento * Delta, 0, VelocidadMovimiento * Time.deltaTime);
                if (PivoteCamara.position.z >= ConfiguracionMapa.mapaTam.z * ConfiguracionMapa.azulejoTam.z)
                {
                    PivoteCamara.position = new Vector3(PivoteCamara.position.x, 0, ConfiguracionMapa.mapaTam.z * ConfiguracionMapa.azulejoTam.z);
                  //  return;
                }
              if ( PivoteCamara.position.x >= ConfiguracionMapa.mapaTam.x * ConfiguracionMapa.azulejoTam.x)
                {
                    PivoteCamara.position = new Vector3(ConfiguracionMapa.mapaTam.x * ConfiguracionMapa.azulejoTam.x, 0, PivoteCamara.position.z);
                 //   return;
                }

               
            }
            else if (Input.mousePosition.y < AnchoBorde)
            {
                PivoteCamara.position += new Vector3(-1 * VelocidadMovimiento * Delta, 0, -1 * VelocidadMovimiento * Delta);

                if (PivoteCamara.position.x <= 0)
                {
                    PivoteCamara.position = new Vector3(0, 0, PivoteCamara.position.z);
                  //  return;
                }
                if (PivoteCamara.position.z <= 0 )
                {
                    PivoteCamara.position = new Vector3(PivoteCamara.position.x, 0, 0);
                  //  return;
                }
             

            }
                
            if (Input.mousePosition.x > (Screen.width - AnchoBorde))
            {
                PivoteCamara.position += new Vector3(VelocidadMovimiento * Delta, 0, -1 * VelocidadMovimiento * Delta);
                if ( PivoteCamara.position.x >= ConfiguracionMapa.mapaTam.x * ConfiguracionMapa.azulejoTam.x)
                {
                    PivoteCamara.position = new Vector3(ConfiguracionMapa.mapaTam.x * ConfiguracionMapa.azulejoTam.x, 0, PivoteCamara.position.z);
                    //return;
                }
               if ( PivoteCamara.position.z <= 0)
                {
                    PivoteCamara.position = new Vector3(PivoteCamara.position.x, 0, 0);
                    //return;
                }
             


            }
            else if (Input.mousePosition.x < AnchoBorde)
            {
                PivoteCamara.position += new Vector3(-1 * VelocidadMovimiento * Delta, 0, VelocidadMovimiento * Delta);
                if (PivoteCamara.position.x <= 0  )
                {
                    PivoteCamara.position = new Vector3(0, 0, PivoteCamara.position.z);
                   // return;
                }
                if ( PivoteCamara.position.z >= ConfiguracionMapa.mapaTam.z * ConfiguracionMapa.azulejoTam.z)
                {
                    PivoteCamara.position = new Vector3(PivoteCamara.position.x, 0, ConfiguracionMapa.mapaTam.z * ConfiguracionMapa.azulejoTam.z);
                   // return;
                }
                
             

            }        
           // Debug.Log("Mouse Position:" + Input.mousePosition);
        }

        void MovimientoIsometrico()
        {
            Vector3 Posicion = new Vector3(0, 0, 0);
            Plane PlanoX = new Plane(Vector3.up, 0);
            Ray ray = CamaraIsometrica.ScreenPointToRay(Input.mousePosition);
            float distancia;
            if (PlanoX.Raycast(ray, out distancia))
            {
                Posicion = ray.GetPoint(distancia);
            }

            /*--------------------------------------*/
            float r = (EditorEntradaSistema.capa -1) +0.25f* (EditorEntradaSistema.capa - 1);
            int x = Mathf.FloorToInt(Posicion.x - r / ConfiguracionMapa.azulejoTam.x ) + 1 ;
            int y = EditorEntradaSistema.capa; // Mathf.FloorToInt(Posicion.y / CamaraConfiguracionComponente.azulejoTam.y) + 1;
            int z = Mathf.FloorToInt(Posicion.z - r / ConfiguracionMapa.azulejoTam.z  ) + 1 ;

            CamaraPosicionComponente.posicionIsometrica = new Vector3Int { x = x, y = y, z = z };
            CamaraPosicionComponente.posicionNormal = Posicion;
            CamaraPosicionComponente.posicionNumerica = (ConfiguracionMapa.mapaTam.x * x) - ConfiguracionMapa.mapaTam.z + z;

            if (CamaraPosicionComponente.posicionIsometrica.x <= 0 || CamaraPosicionComponente.posicionIsometrica.x > ConfiguracionMapa.mapaTam.x || CamaraPosicionComponente.posicionIsometrica.z <= 0 || CamaraPosicionComponente.posicionIsometrica.z > ConfiguracionMapa.mapaTam.z)
            {

                CamaraPosicionComponente.posicionIsometrica = new Vector3Int(0, 0, 0);
                CamaraPosicionComponente.posicionNormal = new float3(0f, 0f, 0f);
                CamaraPosicionComponente.posicionNumerica = 0;
                return;
            }
            else
            {
               // NotificarPantalla?.Invoke(this, PosicionIsometrica);
            }
            
        }
    }
}
