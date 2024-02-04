using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoneCompress 
{
    public static Bone CompressBone(ref Bone target, ref Bone source)
    {
        Bone compressedBone = new Bone();
        compressedBone.Location = target.Location - source.Location;

        return compressedBone;
    }

    public static Bone UncompressBone(ref Bone target, ref Bone source)
    {
        Bone uncompressedBone = new Bone();
        uncompressedBone.Location = target.Location + source.Location;

        return uncompressedBone;
    }
    
}
