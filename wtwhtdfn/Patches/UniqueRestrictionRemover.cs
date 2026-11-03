using MonoMod;
using System;
using System.Collections.Generic;
using System.Text;
using wtwhtdfn;

public class patch_PartTypeForToolbar
{
    [MonoModIgnore]
    public extern static PartTypeForToolbar orig_Make(PartType type, int indexOfType, bool isAvailable, bool hasPerms);


    public static PartTypeForToolbar Make(PartType type, int indexOfType, bool isAvailable, bool hasPerms)
    {
        
        return orig_Make(type, indexOfType, wtwhtdfn.wtwhtdfn.MQSettings.Instance.disableUnique || isAvailable, hasPerms);

    }
}