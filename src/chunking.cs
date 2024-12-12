using System.Numerics;
using thrustr.utils;

partial class lodus {
    static byte chunk_size = 16;

    /*static void init_chunk(Vector3 position) {
        chunks.Add(new() { data = new byte[chunk_size,chunk_size,chunk_size], pos = position });
    }*/

    static int init_chunk_out(Vector3 position) {
        chunks.Add(new() { data = new byte[chunk_size,chunk_size,chunk_size], pos = position });
        return chunks.Count-1;
    }

    static void gen_new_chunk(Vector3 pos) {
        int cur_chunk = init_chunk_out(pos);

        for(int x = 0; x < chunk_size; x++)
            for(int y = 0; y < chunk_size; y++)
                for(int z = 0; z < chunk_size; z++) {// 0, 1
                    chunks[cur_chunk].data[x,y,z] = 0x01;
                }

        int i = 0;

        List<uint> inds = new();
        List<vsdata> vsdat = new();

        for(int x = 0; x < chunk_size; x++)
            for(int y = 0; y < chunk_size; y++)
                for(int z = 0; z < chunk_size; z++)
                    if(chunks[cur_chunk].data[x,y,z] != 0) 
                        if(
                            (x > 0 && y > 0 && z > 0 && x < chunk_size-1 && y < chunk_size-1 && z < chunk_size-1)? ( 
                                chunks[cur_chunk].data[x-1,y,z] == 0 ||
                                chunks[cur_chunk].data[x+1,y,z] == 0 ||
                                chunks[cur_chunk].data[x,y-1,z] == 0 ||
                                chunks[cur_chunk].data[x,y+1,z] == 0 ||
                                chunks[cur_chunk].data[x,y,z-1] == 0 ||
                                chunks[cur_chunk].data[x,y,z+1] == 0
                            ) : true
                        ) {
                            for(int a = 0; a < cube_verts.Length; a++)
                                vsdat.Add(new() { 
                                    vert = cube_verts[a] + new Vector3(x,y,z)*2,

                                    uv = ( 
                                        cube_uvs[a] + 
                                        new Vector2(
                                            math.floor(chunks[cur_chunk].data[x,y,z]/256f),
                                            chunks[cur_chunk].data[x,y,z]%256
                                        ) 
                                    )/256f 
                                });

                            for(int a = 0; a < cube_inds.Length; a++)
                                inds.Add((uint)(cube_inds[a] + i));

                            i+=cube_verts.Length;
                        }

        chunks[cur_chunk].mesh_inds = inds.ToArray();
        chunks[cur_chunk].mesh_data = vsdat.ToArray();
    }

    /*static void gen_existing_chunk(int chunk_id) {
        for(int x = 0; x < chunk_size; x++)
            for(int y = 0; y < chunk_size; y++)
                for(int z = 0; z < chunk_size; z++) {
                    chunks[chunk_id].data[x,y,z] = 1;
                }
    }*/
}