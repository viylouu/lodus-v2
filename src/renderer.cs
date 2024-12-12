using System.Numerics;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

using thrustr.basic;
using thrustr.utils;

public struct vsdata {
    public Vector3 vert;
    public Vector2 uv;
}

public class chunk {
    public uint[] mesh_inds;
    public vsdata[] mesh_data;

    public byte[,,] data;

    public Vector3 pos;
}

partial class lodus {
    /* meshing variables */

    static Vector3[] cube_verts;
    static uint[] cube_inds;
    static Vector2[] cube_uvs;

    /* chunk data */

    static List<chunk> chunks = new();

    /* shaders */

    static vertshad vertex_shader = new();
    static fragshad fragment_shader = new();

    /* textures */

    static ITexture atlas;

    /* rendering variables */

    static IDepthMask dmask;

    /* camera variables */

    static Vector3 cam;
    static float pitch, yaw, pitchr, yawr;

    static void rend(ICanvas c) {
        c.Clear(Color.CornflowerBlue);
        dmask.Clear(1);

        // if (!intro.introplayed)
        // { intro.dointro(c, fontie.dfont); return; }

        movement();
        camera();

        vertex_shader.view = Matrix4x4.CreateTranslation(cam) * Matrix4x4.CreateRotationY(pitchr) * Matrix4x4.CreateRotationX(yawr);
        vertex_shader.proj = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 3f, c.Width / (float)c.Height, 0.1f, 1024f);

        fragment_shader.cam = cam;

        c.Fill(fragment_shader, vertex_shader);
        c.Mask(dmask);
        c.WriteMask(dmask, null);

        for (int i = 0; i < chunks.Count; i++) {
            vertex_shader.world = Matrix4x4.CreateTranslation(chunks[i].pos * chunk_size * 2);
            vertex_shader.chunk_pos = chunks[i].pos;

            c.DrawTriangles<vsdata>(chunks[i].mesh_data, chunks[i].mesh_inds);

            //bad (i think) do something else later
            c.Flush();
        }

        fontie.rendertext(c, fontie.dfont, $"{math.round(1 / Time.DeltaTime)} fps", 3, 3, ColorF.White);
    }

    static void camera() {
        float center_x = math.round(Window.Width/2),
              center_y = math.round(Window.Height/2);

        pitch += (math.round(Mouse.Position.X) - center_x) * .125f;
        yaw += (math.round(Mouse.Position.Y) - center_y) * .125f;

        yaw = math.clamp(yaw, -90, 90);

        pitchr = math.rad(pitch);
        yawr = math.rad(yaw);

        Mouse.Position = new(center_x, center_y);
    }

    static void movement() {
        float speed = 64;

        if (Keyboard.IsKeyDown(Key.W))
            cam += new Vector3(math.cos(pitchr + math.pi / 2), 0, math.sin(pitchr + math.pi / 2)) * Time.DeltaTime * speed;
        if (Keyboard.IsKeyDown(Key.S))
            cam -= new Vector3(math.cos(pitchr + math.pi / 2), 0, math.sin(pitchr + math.pi / 2)) * Time.DeltaTime * speed;

        if (Keyboard.IsKeyDown(Key.A))
            cam += new Vector3(math.cos(pitchr), 0, math.sin(pitchr)) * Time.DeltaTime * speed;
        if (Keyboard.IsKeyDown(Key.D))
            cam -= new Vector3(math.cos(pitchr), 0, math.sin(pitchr)) * Time.DeltaTime * speed;

        if (Keyboard.IsKeyDown(Key.Space))
            cam.Y -= Time.DeltaTime * speed;
        if (Keyboard.IsKeyDown(Key.LeftShift))
            cam.Y += Time.DeltaTime * speed;
    }
} 