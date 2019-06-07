using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJob1 : PlayerJob
{
    // Start is called before the first frame update
    public override void _Start()
    {
        player.OnFloorHeight *= 100;
        //base.Start();
    }

    // Update is called once per frame
    public override void _Update()
    {
        //base.Update();
    }
}
