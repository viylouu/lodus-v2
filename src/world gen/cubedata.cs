using System.Numerics;

partial class lodus {
    static Vector3[] face_normals = {
        new (1, 0, 0),  // +x
        new (-1, 0, 0), // -x
        new (0, 1, 0),  // +y
        new (0, -1, 0), // -y
        new (0, 0, 1),  // +z
        new (0, 0, -1)  // -z
    };

    static Vector3[,] face_verts = new Vector3[6, 4] {
        { new (1, 1, 1), new (1, 0, 1), new (1, 0, 0), new (1, 1, 0) }, // +x
        { new (0, 1, 0), new (0, 0, 0), new (0, 0, 1), new (0, 1, 1) }, // -x
        { new (0, 1, 1), new (0, 1, 0), new (1, 1, 0), new (1, 1, 1) }, // +y
        { new (1, 0, 1), new (1, 0, 0), new (0, 0, 0), new (0, 0, 1) }, // -y
        { new (0, 1, 1), new (1, 1, 1), new (1, 0, 1), new (0, 0, 1) }, // +z
        { new (1, 1, 0), new (0, 1, 0), new (0, 0, 0), new (1, 0, 0) }  // -z
    };

    static int[,] face_inds = new int[6, 6] {
        { 0, 1, 2, 2, 3, 0 },    // w
        { 0, 1, 2, 2, 3, 0 },    // h
        { 0, 1, 2, 2, 3, 0 },    // y
        { 0, 1, 2, 2, 3, 0 },    // s
        { 0, 1, 2, 2, 3, 0 },    // o
        { 0, 1, 2, 2, 3, 0 }     // repeitive
    };

    static Vector2[,] face_uvs = new Vector2[6, 4] {
        { new (0, .3333f), new (0, .6667f), new (.5f, .6667f), new (.5f, .3333f) }, ///// +x
        { new (.5f, .3333f), new (.5f, .6667f), new (1f, .6667f), new (1f, .3333f) }, /// -x
        { new (0, 0), new (0, .3333f), new (.5f, .3333f), new (.5f, 0) }, /////////////// +y
        { new (.5f, 0), new (.5f, .3333f), new (1f, .3333f), new (1f, 0) }, ///////////// -y
        { new (0, .6667f), new (.5f, .6667f), new (.5f, 1f), new (0, 1f) }, ///////////// +z
        { new (.5f, .6667f), new (1f, .6667f), new (1f, 1f), new (.5f, 1f) } //////////// -z
    };
}