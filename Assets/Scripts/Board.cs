using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class Board : MonoBehaviour
{
    public Color whiteColor;
    public Color blackColor;

    void SetTrianglesAtPos(int x, int y, int[] tris, int triIndex) {
        tris[triIndex] = y * 9 + x;
        tris[triIndex + 1] = (y + 1) * 9 + x;
        tris[triIndex + 2] = y * 9 + x + 1;

        tris[triIndex + 3] = y * 9 + x + 1;
        tris[triIndex + 4] = (y + 1) * 9 + x;
        tris[triIndex + 5] = (y + 1) * 9 + x + 1;
    }

    public void SetBoardColors() {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        Material[] materials = meshRenderer.sharedMaterials;
        materials[0].SetColor("_Color", whiteColor);
        materials[1].SetColor("_Color", blackColor);
    }
    
    public void GenerateBoard() {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        if (!meshFilter.sharedMesh) {
            Mesh mesh = new Mesh
            {
                subMeshCount = 2
            };
            
            Vector3[] vertices = new Vector3[81];
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                vertices[y * 9 + x] = new Vector3(x, y);
                }
            }

            mesh.vertices = vertices;

            int[] whiteTris = new int[32 * 6];
            int[] blackTris = new int[32 * 6];
            int whiteTriIndex = 0;
            int blackTriIndex = 0;
            
            for (int y = 0; y < 8; y++) {
                for (int x = 0; x < 8; x++)
                {
                    if ((y + x) % 2 == 0) {
                        SetTrianglesAtPos(x, y, whiteTris, whiteTriIndex);
                        whiteTriIndex += 6;
                    } else {
                        SetTrianglesAtPos(x, y, blackTris, blackTriIndex);
                        blackTriIndex += 6;
                    }
                }
            }

            mesh.SetTriangles(whiteTris, 0);
            mesh.SetTriangles(blackTris, 1);
            mesh.RecalculateNormals();

            meshFilter.mesh = mesh;
        }
        
        SetBoardColors();
    }
}
