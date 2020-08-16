using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraGrid 
{
    public enum GridElevacion
    {
        Subir,
        Bajar
    }
    static CamaraGrid instancia;
    float posicionY;
    public GameObject gameObject;
    public static CamaraGrid instanciar()
    {
        if (instancia ==null)
        {
            instancia = new CamaraGrid();
        }
        return instancia;
    }
    public void actualizarGrid(Vector3Int mapaTam, Vector3 azulejoTam)
    {
        gameObject = GameObject.Find("Plano");
        gameObject.transform.localScale = new Vector3(azulejoTam.x * (0.1f * mapaTam.x), 1, azulejoTam.z * (0.1f * mapaTam.z));
        gameObject.transform.position = new Vector3((azulejoTam.x * (0.1f * mapaTam.x) * 5) - azulejoTam.x / 2, (azulejoTam.y/2) + 0.01f, (azulejoTam.z * (0.1f * mapaTam.z) * 5) - azulejoTam.z / 2);
        gameObject.GetComponent<Renderer>().material.SetVector("Vector2_642EB6F1", new Vector4(mapaTam.x, mapaTam.z, 0, 0));
    }

    public void actualizarPosicion(float azulejoTamY, GridElevacion elevacion)
    {
        Vector3 pos = new Vector3(0, 1 , 0);
        if (elevacion == GridElevacion.Subir)
        {
            gameObject.transform.position += pos;
        }
        else
        {
            gameObject.transform.position -= pos;
        }        
    }
    public void ActivarDesactivarGrid()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    public void actualizarPosicion(float azulejoTamY, int capa)
    {      
      gameObject.transform.position = new Vector3(gameObject.transform.position.x, capa-azulejoTamY / 2 + 0.01f, gameObject.transform.position.z);     
    }
    public void actualizarPosicion(float azulejoTamY, Vector3 posicion)
    {
        gameObject.transform.position = posicion;
    }

}
