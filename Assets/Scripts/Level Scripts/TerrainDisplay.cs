using UnityEngine;

public class TerrainDisplay : MonoBehaviour {

    public Renderer texRender;
    public MeshFilter meshFilter, meshFilter2;
    public MeshRenderer meshRenderer, meshRenderer2;

    public void DrawTexture(Texture2D texture) {


        texRender.sharedMaterial.mainTexture = texture;
        texRender.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture) {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshFilter2.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
        meshRenderer2.sharedMaterial.mainTexture = texture;
    }
}
