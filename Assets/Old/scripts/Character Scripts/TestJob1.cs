using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJob1 : PlayerJob
{
    public override void _Start()
    {
        player.OnFloorHeight *= 100;
        //base.Start();
    }

    public override void _Update()
    {
        //base.Update();
    }

    public override void _End()
    {
        player.OnFloorHeight /= 100;
    }
}
