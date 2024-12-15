using System.Numerics;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using static SimulationFramework.Drawing.Shaders.ShaderIntrinsics;

public class fragshad : CanvasShader {
    public int[] dithermatrix = {
        0, 48, 12, 60,  3, 51, 15, 63,
        32, 16, 44, 28, 35, 19, 47, 31,
        8, 56,  4, 52, 11, 59,  7, 55,
        40, 24, 36, 20, 43, 27, 39, 23,
        2, 50, 14, 62,  1, 49, 13, 61,
        34, 18, 46, 30, 33, 17, 45, 29,
        10, 58,  6, 54,  9, 57,  5, 53,
        42, 26, 38, 22, 41, 25, 37, 21
    };

    [VertexShaderOutput]
    Vector2 vert_uv;

    [VertexShaderOutput]
    Vector3 vert_pos;

    [VertexShaderOutput]
    Vector3 v_chunk_pos;

    public Vector3 cam;

    public ITexture tex;

    public float time;

    public int rend_dist;

    public int chunk_size;

    public float fog_scaling_factor;

    public override ColorF GetPixelColor(Vector2 pos) {
        /*float depth = 1-Distance(vert_pos, cam) * fog_scaling_factor;

        depth += .65f;

        if(depth <= 0)
            Discard();

        ColorF x = tex.SampleUV(vert_uv);
        if(x.A < .1f)
            Discard();

        depth = Clamp(depth, 0, 1);

        depth *= depth;

        return new ColorF(
            Lerp(100/255f, x.R, depth),
            Lerp(149/255f, x.G, depth),
            Lerp(237/255f, x.B, depth)
        );*/

        return new ColorF(
            Mod(v_chunk_pos.X/4f,1),
            Mod(v_chunk_pos.Y/4f,1),
            Mod(v_chunk_pos.Z/4f,1)
        );
    }
}