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
        Simulation.SetFixedResolution(960,540,Color.Black,false,true,false);

        intro.loadintro();
        intro.playintro();

        (cube_verts, cube_inds, cube_uvs) = misc.loadfbx(@"assets\meshes\cube.fbx");

        grass = Graphics.LoadTexture(@"assets\sprites\voxels\grass.png");

        fragment_shader.tex = grass;
        vertex_shader.chunk_size = chunk_size;

        dmask = Graphics.CreateDepthMask(960,540);

        for(int x = 0; x < 3; x++)
            for(int y = 0; y < 3; y++)
                for(int z = 0; z < 3; z++)
                    gen_new_chunk(new(x,y,z));
    }
}