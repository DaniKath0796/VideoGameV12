using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoinCatCode
{
   
    public class AdministradorAzulejosVoxel
    {
        private Dictionary<int, AzulejoVoxelPlantilla> contenedor;
        static AdministradorAzulejosVoxel instancia;

        AdministradorAzulejosVoxel()
        {
            contenedor = new Dictionary<int, AzulejoVoxelPlantilla>();
        }
        public static AdministradorAzulejosVoxel Instanciar()
        {
            if (instancia == null)
            {
                instancia = new AdministradorAzulejosVoxel();
            }
            return instancia;
        }

        public AzulejoVoxelPlantilla ObtenerAzulejo(int idAzulejoVoxel)
        {            
            if (contenedor.ContainsKey(idAzulejoVoxel))
            {
                return contenedor[idAzulejoVoxel];
            }           
            return null;
        }
        public bool AgregarAzulejo(AzulejoVoxelPlantilla azulejoPlantilla)
        {
       
            if (!contenedor.ContainsKey(azulejoPlantilla.idAzulejoVoxel))
            {
                contenedor.Add(azulejoPlantilla.idAzulejoVoxel, azulejoPlantilla);
                return true;
            }
            
            return false;
        }

    }
}
