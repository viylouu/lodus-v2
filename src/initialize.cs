using SimulationFramework;
using SimulationFramework.Drawing;
using thrustr.basic;
using thrustr.utils;

partial class lodus {
    static void Main() {
        Simulation sim = Simulation.Create(init, rend);
        sim.Run();
    }

    static void init() {
        Window.Title = "lodus";
        Simulation.SetFixedResolution(960,540,Color.Black);

        intro.loadintro();
        intro.playintro();

        (verts, inds, uvs) = misc.loadfbx(@"assets\meshes\cube.fbx");

        grass = Graphics.LoadTexture(@"assets\sprites\voxels\grass.png");

        fragment_shader.tex = grass;

        dmask = Graphics.CreateDepthMask(960,540);

        vsdat = new vertshad.vsdata[verts.Length];

        for(int i = 0; i < vsdat.Length; i++)
        vsdat[i] = new() { vert = verts[i], uv = uvs[i] };
    }
}