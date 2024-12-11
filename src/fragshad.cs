using System.Numerics;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Drawing.Shaders;
using static SimulationFramework.Drawing.Shaders.ShaderIntrinsics;

public class fragshad : CanvasShader {
    [VertexShaderOutput]
    Vector2 uv;

    public ITexture tex;

    public override ColorF GetPixelColor(Vector2 pos) {
        ColorF x = tex.SampleUV(uv);
        if(x.A < .1f)
            Discard();
        return x;
    }
}