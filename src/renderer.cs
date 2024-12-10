using SimulationFramework;
using SimulationFramework.Drawing;

using thrustr.basic;
using thrustr.utils;

partial class lodus {
    static void rend(ICanvas c) {
         c.Clear(Color.Black);

        if (!intro.introplayed)
        { intro.dointro(c, fontie.dfont); return; }

        fontie.rendertext(c, fontie.dfont, $"{math.round(1/Time.DeltaTime)} fps", 3,3, ColorF.White);
    }
}