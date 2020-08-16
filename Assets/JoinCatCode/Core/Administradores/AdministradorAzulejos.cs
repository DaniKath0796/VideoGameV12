using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoinCatCode
{
    public enum TipoAzulejo
    { 
        Simple,
        Automatico
    }
    public enum ClaseAzulejo
    {
        Terreno,
        Arbol,
        Objetos       
    }
    public class AdministradorAzulejos
    {
        private Dictionary<int, AzulejoPlantilla> contenedorTerrenos;
        static AdministradorAzulejos instancia;

        AdministradorAzulejos()
        {
            contenedorTerrenos = new Dictionary<int, AzulejoPlantilla>();
        }
        public static AdministradorAzulejos Instanciar()
        {
            if (instancia == null)
            {
                instancia = new AdministradorAzulejos();
            }
            return instancia;
        }

        public AzulejoPlantilla ObtenerAzulejo(ClaseAzulejo claseAzulejo, int id)
        {
            switch (claseAzulejo)
            {
                case ClaseAzulejo.Terreno:
                    if (contenedorTerrenos.ContainsKey(id))
                    {
                        return contenedorTerrenos[id];
                    }
                    break;
                case ClaseAzulejo.Arbol:
                    break;
                case ClaseAzulejo.Objetos:
                    break;
                default:
                    return null;
            }
            return null;

        }
        public bool AgregarAzulejo(ClaseAzulejo claseAzulejo, AzulejoPlantilla azulejoPlantilla)
        {
            switch (claseAzulejo)
            {
                case ClaseAzulejo.Terreno:
                    if (!contenedorTerrenos.ContainsKey(azulejoPlantilla.id))
                    {
                        contenedorTerrenos.Add(azulejoPlantilla.id, azulejoPlantilla);
                        return true;
                    }
                    break;
                case ClaseAzulejo.Arbol:
                    return true;
                    break;
                case ClaseAzulejo.Objetos:
                    return true;
                    break;
                default:
                    return false;
            }            
            return false;
        }

    }
}
