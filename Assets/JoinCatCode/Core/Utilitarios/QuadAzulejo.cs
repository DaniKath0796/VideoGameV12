using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadAzulejo : MonoBehaviour
{
    // Start is called before the first frame update

    MeshFilter meshfilter;
    Mesh mesh;
    void Start()
    {
        meshfilter = GetComponent<MeshFilter>();
        mesh =meshfilter.mesh;
       
    }

    public Vector2 convertPixelToUvCoordenadas(int x, int y, int texturaAncho, int texturaAltura)
    {
        return new Vector2((float)x/texturaAncho, (float) y / texturaAltura);
    }

    public int posicionX = 2;
    public int posicionY = 2;
    void actualizarMalla()
    {
       
        int tamTile = 128;
        Vector2[] uv = new Vector2[4];


        // 128
        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(1, 0);

        uv[0] = convertPixelToUvCoordenadas((posicionX - 1)*tamTile , posicionY * 128, 2048, 2048);
        uv[1] = convertPixelToUvCoordenadas(posicionX * 128, posicionY * 128, 2048, 2048);
        uv[2] = convertPixelToUvCoordenadas((posicionX - 1) * tamTile, (posicionY - 1) * tamTile, 2048, 2048);
        uv[3] = convertPixelToUvCoordenadas(posicionX * 128, (posicionY - 1) * tamTile, 2048, 2048);
       
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }
    // Update is called once per frame
    void Update()
    {
        actualizarMalla();
    }
}
