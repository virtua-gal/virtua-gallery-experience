using UnityEngine;

namespace SimplestarGame.MeshSubdivider
{
    /// <summary>
    /// Subdivided Quad Mesh Generator
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    public class QuadMeshSubdivider : MonoBehaviour
    {
        /// <summary>
        /// Resolution of Quad Mesh
        /// </summary>
        [SerializeField] [Range(64, 512)] int resolution = 256;

        void Start()
        {
            // Default by WaveSimulator
            var waveSimulator = GameObject.FindObjectOfType<SimplestarGame.Wave.WaveSimulator>();
            if (null != waveSimulator)
            {
                this.resolution = waveSimulator.resolution;
            }
            this.lastResolution = this.resolution;
            this.SubDivide(this.lastResolution);
        }

        void OnValidate()
        {
            if (Application.isPlaying)
            {
                if (this.lastResolution != this.resolution)
                {
                    this.lastResolution = this.resolution;
                    this.SubDivide(this.lastResolution);
                }
            }
        }

        /// <summary>
        /// Sub Divie Quad Mesh
        /// </summary>
        /// <param name="subDivCount"></param>
        void SubDivide(int subDivCount)
        {
            var subDivIndices = new int[6 * subDivCount * subDivCount];
            var subDivVerts = new Vector3[4 * subDivCount * subDivCount];
            var subDivUvs = new Vector2[4 * subDivCount * subDivCount];
            var edgeLength = 1.0f / subDivCount;
            for (int xIndex = 0; xIndex < subDivCount; xIndex++)
            {
                var offsetX = edgeLength * xIndex;
                for (int yIndex = 0; yIndex < subDivCount; yIndex++)
                {
                    var offsetY = edgeLength * yIndex;
                    var offsetIndex = subDivCount * xIndex + yIndex;

                    var leftBottom = new Vector3(offsetX - 0.5f, offsetY - 0.5f);
                    var rightBottom = leftBottom + new Vector3(edgeLength, 0);
                    var leftUp = leftBottom + new Vector3(0, edgeLength);
                    var rightUp = leftBottom + new Vector3(edgeLength, edgeLength);

                    subDivVerts[4 * offsetIndex + 0] = leftBottom;
                    subDivVerts[4 * offsetIndex + 1] = rightBottom;
                    subDivVerts[4 * offsetIndex + 2] = leftUp;
                    subDivVerts[4 * offsetIndex + 3] = rightUp;

                    var uvLeftBottom = new Vector2(offsetX, offsetY);
                    var uvRightBottom = uvLeftBottom + new Vector2(edgeLength, 0);
                    var uvLeftUp = uvLeftBottom + new Vector2(0, edgeLength);
                    var uvRightUp = uvLeftBottom + new Vector2(edgeLength, edgeLength);

                    subDivUvs[4 * offsetIndex + 0] = uvLeftBottom;
                    subDivUvs[4 * offsetIndex + 1] = uvRightBottom;
                    subDivUvs[4 * offsetIndex + 2] = uvLeftUp;
                    subDivUvs[4 * offsetIndex + 3] = uvRightUp;

                    subDivIndices[6 * offsetIndex + 0] = 4 * offsetIndex + 0;
                    subDivIndices[6 * offsetIndex + 1] = 4 * offsetIndex + 3;
                    subDivIndices[6 * offsetIndex + 2] = 4 * offsetIndex + 1;
                    subDivIndices[6 * offsetIndex + 3] = 4 * offsetIndex + 3;
                    subDivIndices[6 * offsetIndex + 4] = 4 * offsetIndex + 0;
                    subDivIndices[6 * offsetIndex + 5] = 4 * offsetIndex + 2;
                }
            }

            var subDivMesh = new Mesh();
            subDivMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            subDivMesh.name = SubDivMeshName;
            subDivMesh.SetVertices(subDivVerts);
            subDivMesh.SetTriangles(subDivIndices, 0);
            subDivMesh.SetUVs(0, subDivUvs);
            subDivMesh.RecalculateBounds();
            subDivMesh.RecalculateNormals();
            subDivMesh.RecalculateTangents();

            var meshFilter = this.GetComponent<MeshFilter>();
            if(this.isMeshSubdivided)
                meshFilter.mesh.Clear();
            this.isMeshSubdivided = true;
            meshFilter.mesh = subDivMesh;
        }

        int lastResolution = 0;
        bool isMeshSubdivided = false;
        const string SubDivMeshName = "SimplestarGame-MeshSubDivider";
    }
}