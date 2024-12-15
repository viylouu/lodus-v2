using System.ComponentModel;
using System.ComponentModel.Design;
using System.Numerics;
using ImGuiNET;
using SimulationFramework;
using SimulationFramework.Drawing;
using SimulationFramework.Input;

using thrustr.basic;
using thrustr.utils;

public struct Vector3Int
{
    public int X, Y, Z;

    public Vector3Int(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    // Override Equals and GetHashCode for consistent comparison
    public override bool Equals(object obj)
    {
        if (obj is Vector3Int other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return X ^ Y ^ Z; // Simple hash code using XOR
    }

    // Implement equality operators
    public static bool operator ==(Vector3Int left, Vector3Int right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector3Int left, Vector3Int right)
    {
        return !(left == right);
    }
}

partial class lodus {
    /* chunk data */

    static Dictionary<Vector3Int, chunk> chunks = new();

    /* shaders */

    static vertshad vertex_shader = new();
    static fragshad fragment_shader = new();

    /* textures */

    static ITexture atlas;

    /* rendering variables */

    static IDepthMask dmask;

    /* camera variables */

    static Vector3 cam;
    static float pitch, yaw, pitchr, yawr, pitchs, yaws;

    /* misc variables */

    static bool fullscreen;

    static bool inui;

    static int render_dist = 4;

    static int ax, ay, az, bx, by, bz;

    static void rend(ICanvas c) {
        c.Clear(Color.CornflowerBlue);
        dmask.Clear(1);

        // if (!intro.introplayed)
        // { intro.dointro(c, fontie.dfont); return; }

        movement();
        camera();

        misc_keybinds();

        pausemenu();

        Matrix4x4 view_matrix = Matrix4x4.CreateTranslation(-cam) * Matrix4x4.CreateRotationY(pitchr) * Matrix4x4.CreateRotationX(yawr);
        Matrix4x4 proj_matrix = Matrix4x4.CreatePerspectiveFieldOfView(MathF.PI / 3f, c.Width / (float)c.Height, 0.1f, 1024f);

        vertex_shader.view = view_matrix;
        vertex_shader.proj = proj_matrix;

        vertex_shader.time = Time.TotalTime;

        fragment_shader.cam = cam;

        fragment_shader.time = Time.TotalTime;

        fragment_shader.rend_dist = render_dist;
        fragment_shader.chunk_size = chunk_size;

        fragment_shader.fog_scaling_factor = precalc_fog_scaling_factor;

        c.Fill(fragment_shader, vertex_shader);
        c.Mask(dmask);
        c.WriteMask(dmask, null);

        Vector3 cams = cam * precalc_1div_chunk_size;

        ax = (int)math.floor(cams.X)-render_dist;
        bx = (int)math.floor(cams.X)+render_dist;
        ay = (int)math.floor(cams.Y)-render_dist;
        by = (int)math.floor(cams.Y)+render_dist;
        az = (int)math.floor(cams.Z)-render_dist;
        bz = (int)math.floor(cams.Z)+render_dist;

        for(int x = ax; x < bx; x++)
            for(int y = ay; y < by; y++)
                for(int z = az; z < bz; z++) {
                    Vector3Int pos = new(x,y,z);

                    bool has_chunk = chunks.TryGetValue(pos, out chunk? cur);

                    Vector3 pos_vec = new(pos.X,pos.Y,pos.Z);

                    if(!has_chunk) {
                        gen_new_chunk(pos);

                        continue;
                    }

                    if(cur.genning)
                        continue;

                    if(math.sqrdist(cam, pos_vec * chunk_size) < precalc_max_chunk_dist) {
                        vertex_shader.world = Matrix4x4.CreateTranslation(pos_vec * chunk_size + precalc_chunk_offset);
                        vertex_shader.chunk_pos = pos_vec;

                        c.DrawTriangles<vsdata>(cur.mesh_data, cur.mesh_inds);
                    }
                }

        c.ResetState();

        fontie.rendertext(c, fontie.dfont, $"{math.round(1f / Time.DeltaTime)} fps", 3, 3, ColorF.White);
        fontie.rendertext(c, fontie.dfont, $"{chunks.Count} chunks", 3, 4+fontie.dfont.charh, ColorF.White);
    }

    static void misc_keybinds() {
        if (Keyboard.IsKeyPressed(Key.F11)) {
            fullscreen = !fullscreen;

            if (fullscreen)
                Window.EnterFullscreen();
            else
                Window.ExitFullscreen();
        }

        if (Keyboard.IsKeyPressed(Key.Escape))
            Environment.Exit(0);

        if(Keyboard.IsKeyPressed(Key.Tab))
            inui = !inui;
    }

    static void pausemenu() {
        if(inui) {
            Mouse.Visible = true;

            ImGui.Begin("pausemenu");

            int p_rend_dist = render_dist;
            ImGui.InputInt("render distance", ref render_dist);

            if(render_dist != p_rend_dist)
                calc_precalcs();

            ImGui.End();
        }
        else
            Mouse.Visible = false;
    }
} 