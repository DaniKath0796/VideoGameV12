using JoinCatCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorEdicion : MonoBehaviour
{
    public Material material;
    MapaVoxel mapa;
    public int direccion=0;
    private void Awake()
    {
                 
    }
    void Start()
    {
      /*  mapa = new MapaVoxel("MapaVoxel",material, new Vector3Int(500,4,500),16);
        mapa.LlenarMapaTodo(0);*/
              
       /* mapa = new Mapa("Mapa1", new Vector3Int(10, 10, 10), new Vector3(1, 1, 1), 2);
        List<Cuadrante> cuadrantes = mapa.RellenarMapa(1);
        foreach (var item in cuadrantes)
        {
            List<Capa> capas = item.ObtenerCapas();
            foreach (var capa in capas)
            {
                StartCoroutine(capa.CrearCapaLlena(ClaseAzulejo.Terreno, 1));
            }
        }*/
    }

    // Update is called once per frame
    void Update()
    {
       /* if (Input.GetMouseButtonDown(0))
        {
            mapa.AgregarAzulejoVoxel(1, CamaraPosicionComponente.posicionIsometrica);
            //mapa.ObtenerBloqueVoxel(CamaraPosicionComponente.posicionIsometrica);
        }
        */

        bool Guardar = Input.GetKeyDown(KeyCode.S);
        if (Guardar)
        {
           /* List<Cuadrante> cuadrantesGuardar = mapa.ObtenerCuadrantes();
            foreach (var item in cuadrantesGuardar)
            {

                StartCoroutine(item.CombinarMallas());
            }*/
        }
    }

    private void OnDestroy()
    {
   //     mapa.LiberarNativos();
    }

    ~ControladorEdicion()
    {
      //  mapa.LiberarNativos();
    }
}
