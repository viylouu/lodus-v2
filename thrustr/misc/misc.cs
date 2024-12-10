using SimulationFramework;
using SimulationFramework.Drawing;

namespace thrustr.utils;

public static class misc_extentions {
    public static void trydispose(this ITexture todisp) {
        if(todisp != null)
            todisp.Dispose();
    }

    public static void trydispose(this ISound todisp) {
        if(todisp != null)
            todisp.Dispose();
    }

    public static void trydispose(this SoundPlayback todisp) {
        if(todisp != null)
            todisp.Dispose();
    }
}