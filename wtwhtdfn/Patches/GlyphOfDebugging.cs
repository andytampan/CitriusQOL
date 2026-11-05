using MonoMod;
using System;
using System.Collections.Generic;
using System.Text;


[MonoModPatch("Sim")]
public class patch_Sim_DebuggerGlyph : Sim
{

    [MonoModPublic]
    [MonoModIgnore]
    public extern void SetInCycle(bool inCycle);

}

[MonoModPatch("SolutionEditorScreen")]
public class patch_SolutionEditorScreen_DebuggerGlyph : SolutionEditorScreen
{
    public patch_SolutionEditorScreen_DebuggerGlyph(Solution solution, MusicTrack musicTrack, Sound fanfare) : base(solution, musicTrack, fanfare)
    {
    }

    [MonoModPublic]
    public SimPlayState simulationState;

    [MonoModPublic]
    [MonoModIgnore]
    public extern void CompleteAndPauseSim();

}


