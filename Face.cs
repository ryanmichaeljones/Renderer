using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Renderer
{
    public class Face
    {
        public List<Vector3> positions;
        public List<Vector2> texCoords;
        public List<Vector3> normals;

        public Face()
        {
            positions = new List<Vector3>();
            texCoords = new List<Vector2>();
            normals = new List<Vector3>();
        }
    }
}