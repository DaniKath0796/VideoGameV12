using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JoinCatCode
{
    public class AdministradorMundos
    {
        
        static AdministradorMundos instancia;
        static Mundo mundoActual;
        Dictionary<string, Mundo> contenedor;
        public static AdministradorMundos Instanciar()
        {
            if (instancia == null)
            {
                instancia = new AdministradorMundos();
            }
            return instancia;
        }

        AdministradorMundos()
        {
            contenedor = new Dictionary<string, Mundo>();
        }
        public static Mundo MundoActual()
        {
            return mundoActual;
        }
        public Mundo CrearNuevoMundo(string nombre, int ancho, int altura, int largo, int idMaterialTerreno)
        {
            Mundo mundo = new Mundo(nombre, ancho, altura,largo, idMaterialTerreno);
            contenedor.Add(nombre, mundo);
            return mundo;
        }
        public Mundo SetearMundoActual(string mundoNombre)
        {
            if (contenedor.ContainsKey(mundoNombre))
            {
                mundoActual = contenedor[mundoNombre];
            }
            return mundoActual;
        }

    }
}