using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestJob2 : PlayerJob
{
    public override void _Start()
    {
        //base.Start();
    }

    public override void _Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            player.Energy -= 5 * Time.deltaTime;
            player.HP+=2.5f * Time.deltaTime;
        }
        //base.Update();
    }
}
