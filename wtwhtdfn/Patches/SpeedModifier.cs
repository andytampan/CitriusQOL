using Mono.Cecil;
using MonoMod;
using MonoMod.Cil;
using MonoMod.InlineRT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[MonoModPatch("SolutionEditorProgramPanel")]
public class patch_SolutionEditorProgramPanel2

{
    [MonoModIgnore]
    public extern void orig_RenderFrame(float deltaTime);
    int speedLevel = 1;

    public void RenderFrame(float deltaTime)
    {


        Vector2 position = new Vector2(InputManager.screenSize.X - 55, InputManager.screenSize.Y / 2);
        Bounds2 bound = Bounds2.CenteredOn(position, 55, 55);

        wtwhtdfn.Resource.RenderCenter(Assets.textures.solution_editor.program_panel.instruction, position);
        wtwhtdfn.Resource.RenderCenter(Assets.textures.instructions.track_advance, position);

        if (bound.Contains(InputManager.MousePos()))
        {

            wtwhtdfn.Resource.RenderCenter(Assets.textures.solution_editor.program_panel.instruction_highlight, position);
            if (InputManager.IsClickPressed(MouseButtonType.LeftClick))
            {
                speedLevel++;
                patch_SolutionEditorScreen_TurboSpeed.cm_TurboSpeed *= 2;
            }

        }
        Vector2 position2 = new Vector2(InputManager.screenSize.X - 55, InputManager.screenSize.Y / 2 - 40);
        Bounds2 bound2 = Bounds2.CenteredOn(position2, 55, 55);

        wtwhtdfn.Resource.RenderCenter(Assets.textures.solution_editor.program_panel.instruction, position2);
        wtwhtdfn.Resource.RenderCenter(Assets.textures.instructions.track_retreat, position2);

        if (bound2.Contains(InputManager.MousePos()))
        {
            wtwhtdfn.Resource.RenderCenter(Assets.textures.solution_editor.program_panel.instruction_highlight, position2);
            if (InputManager.IsClickPressed(MouseButtonType.LeftClick))
            {
                speedLevel--;
                patch_SolutionEditorScreen_TurboSpeed.cm_TurboSpeed /= 2;
            }

        }
        Vector2 position3 = new Vector2(InputManager.screenSize.X - 55, InputManager.screenSize.Y / 2 - 80);
        wtwhtdfn.Resource.RenderCenter(Assets.textures.solution_editor.program_panel.instruction, position3);
        TextureRenderer.RenderText(speedLevel.ToString(), position3 - new Vector2(0, 5), Assets.fonts.crimson_16_5, UIConsts.darkTextColor, TextAlignment.Center, 1f, 0.6f, float.MaxValue, float.MaxValue, 0, Color.Black, null, int.MaxValue, false, true);

        orig_RenderFrame(deltaTime);
    }

}

[MonoModPatch("SolutionEditorScreen")]
public class patch_SolutionEditorScreen_TurboSpeed
{

    public static float cm_TurboSpeed = 1f;

    [MonoModILInject("GetSimulationSpeed")]

    public static void GetSimulationSpeed(MethodDefinition method, CustomAttribute attribute)
    {


        //want to make sure your code isn't secretly empty? do this
        /* MonoModRule.Modder.Log("patching hell");
        ILCursor cursorA = new(new ILContext(method));     
            MonoModRule.Modder.Log(cursorA.Context);
        */

        ILCursor cursor = new(new ILContext(method));


        TypeDefinition sim = MonoModRule.Modder.FindType("SolutionEditorScreen").Resolve();
        MethodDefinition multiplySpeed = sim.Methods.First(f => f.Name.Equals("MultiplySpeed"));
        FieldDefinition simStartSpeed = method.DeclaringType.Fields.First(f => f.Name.Equals("cm_TurboSpeed"));

        cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdsfld(out _));
        cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdsfld(out FieldReference simSpeed));
        cursor.EmitLdsfld(simStartSpeed);
        cursor.EmitCallvirt(multiplySpeed);



    }

    public static float MultiplySpeed(float simSpeed, float multiplier)
    {
        return simSpeed *= multiplier;
    }
}


