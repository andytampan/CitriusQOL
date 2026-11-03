using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod;
using MonoMod.Cil;
using MonoMod.InlineRT;
using Quintessential;
using Quintessential.Settings;
using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wtwhtdfn;



public class patch_Solution
    {

        [MonoModPatch("Solution")]
        public extern bool orig_IsAllowedPlacement(Part part, HexIndex otherInputOutputIndex, HexIndex offset, HexRotation rotationOffset, out string errorMessage);

        
        public bool IsAllowedPlacement(Part part, HexIndex otherInputOutputIndex, HexIndex offset, HexRotation rotationOffset, out string errorMessage)
        {
            
            bool yea = orig_IsAllowedPlacement(part, otherInputOutputIndex, offset, rotationOffset, out errorMessage);
            if (errorMessage == Translations.Translate("There is already another part here.") && wtwhtdfn.wtwhtdfn.MQSettings.Instance.enableOverlap) return true ;
            return yea;
        }
}







[MonoModPatch("SolutionEditorProgramPanel")]
public class patch_SolutionEditorProgramPanel2

{
    [MonoModIgnore]
    public extern void orig_RenderFrame(float deltaTime);

    public static bool cm_turboActivated = false;
    public void RenderFrame(float deltaTime)
    {
        Vector2 position = new Vector2(InputManager.screenSize.X - 55, InputManager.screenSize.Y / 2);
        Bounds2 bound = Bounds2.CenteredOn(position, 55, 55);

        wtwhtdfn.Resource.RenderCenter(cm_turboActivated ? Assets.textures.solution_editor.program_panel.instruction_disabled : Assets.textures.solution_editor.program_panel.instruction, position);
        wtwhtdfn.Resource.RenderCenter(Assets.textures.instructions.track_advance, position);

        if (bound.Contains(InputManager.MousePos()))
        {
            wtwhtdfn.Resource.RenderCenter(Assets.textures.solution_editor.program_panel.instruction_highlight, position);
            if (InputManager.IsClickPressed(MouseButtonType.LeftClick)) cm_turboActivated = !cm_turboActivated;
        }


        orig_RenderFrame(deltaTime);
    }

}

 [MonoModPatch("SolutionEditorScreen")]
public class patch_SolutionEditorScreen_TurboSpeed
{

    public static float cm_TurboSpeed = 124f;

    [MonoModILInject("GetSimulationSpeed")]

    public static void GetSimulationSpeed(MethodDefinition method, CustomAttribute attribute)
    {


        //want to make sure your code isn't secretly empty? do this
        /* MonoModRule.Modder.Log("patching hell");
        ILCursor cursorA = new(new ILContext(method));     
            MonoModRule.Modder.Log(cursorA.Context);
        */

        ILCursor cursor = new(new ILContext(method));

        TypeDefinition sim = MonoModRule.Modder.FindType("SolutionEditorProgramPanel").Resolve();
        FieldDefinition outputsPlaced = sim.Fields.First(f => f.Name.Equals("cm_turboActivated"));
        FieldDefinition simStartSpeed = method.DeclaringType.Fields.First(f => f.Name.Equals("cm_TurboSpeed"));


        cursor.Index = cursor.Index + 8;
        var instructionEnd = cursor.Next.Operand;
        Instruction instructionDefault;

        cursor.TryGotoNext(MoveType.Before, instr => instr.MatchLdcR4(1));

        cursor.Emit(OpCodes.Ldsfld, outputsPlaced);

        Instruction lastPos = cursor.Next;
        Instruction currentPos = cursor.Previous;

        cursor.TryGotoPrev(MoveType.Before, instr => instr.MatchStloc0());
        instructionDefault = (Instruction)cursor.Previous.Previous.Operand;
        cursor.Previous.Previous.Operand = currentPos;

        cursor.TryGotoNext(MoveType.Before, instr => instr == lastPos);
        cursor.Emit(OpCodes.Brfalse_S, instructionDefault);
        cursor.Emit(OpCodes.Ldsfld, simStartSpeed);
        cursor.Emit(OpCodes.Stloc_0);
        cursor.Emit(OpCodes.Br, instructionEnd);

    }

}

