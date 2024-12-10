using SimulationFramework;

using thrustr.basic;

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
    }
}