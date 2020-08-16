using System.Collections;
using System.Collections.Generic;

using Unity.Jobs;
using UnityEngine;

namespace JoinCatCode
{
    public enum Brocha
    {
        Seleccion,
        Creacion,
        Modificar,
        Eliminar
    }

    public enum MantenerEstado
    { 
        Ninguno,
        Guardar,
        Cargar
    }

    public class EditorEntradaSistema : MonoBehaviour
    {
        public Brocha         brocha;
        public TipoAzulejo    tipoAzulejo;
        public static int     capa;
        public MantenerEstado estado; 
        private void Start()
        {
            capa = 1;
            tipoAzulejo = TipoAzulejo.Automatico;
            
        }
        protected void Update()
        {           
            bool Creacion = Input.GetKeyDown(KeyCode.C);
            bool Seleccion = Input.GetKeyDown(KeyCode.S);
            bool Eliminar = Input.GetKeyDown(KeyCode.E);

            bool subirCapa = Input.GetKeyDown(KeyCode.KeypadPlus);
            bool bajarCapa = Input.GetKeyDown(KeyCode.KeypadMinus);

            bool planoGrid = Input.GetKeyDown(KeyCode.Tab);

            bool ModoTipoAzulejo = Input.GetKeyDown(KeyCode.Space);

         
            if (ModoTipoAzulejo)
            {
                if (tipoAzulejo == TipoAzulejo.Automatico)
                {
                    tipoAzulejo = TipoAzulejo.Simple;
                }
                else
                {
                    tipoAzulejo = TipoAzulejo.Automatico;
                }    
                
            }
            if (planoGrid)
            {
                CamaraGrid.instanciar().gameObject.SetActive(!CamaraGrid.instanciar().gameObject.activeSelf);
            }
            if (subirCapa)
            {
                if (capa < CamaraConfiguracionComponente.mapaTam.y)
                {
                    capa += 1;                    
                    CamaraGrid.instanciar().actualizarPosicion(CamaraConfiguracionComponente.azulejoTam.y,CamaraGrid.GridElevacion.Subir );
                }                
            }
            if (bajarCapa)
            {
                if (capa > 1)
                {
                    capa -= 1;                  
                    CamaraGrid.instanciar().actualizarPosicion(CamaraConfiguracionComponente.azulejoTam.y, CamaraGrid.GridElevacion.Bajar);
                }
            }
            if (Creacion)
            {
                brocha = Brocha.Creacion;
            }
            if (Seleccion)
            {
                brocha = Brocha.Seleccion;
            }
            if (Eliminar)
            {
                brocha = Brocha.Eliminar;
            }        
        }      
    }
}