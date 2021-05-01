using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using CodeHelper;


public class Stage_Pillar : StageGenerator
{
    [SerializeField] Vector3 kakuritu = new Vector3(0.04f, 0.04f, 0.04f);

    // Update is called once per frame
    void Update()
    {

    }

    public override List<SavingObject> Generate()
    {
        var targetList = new List<SavingObject>();
        firstPos = transform.position;
        //blockSize = size;
        blockSize.x = transform.localScale.x / size.x;
        blockSize.y = transform.localScale.y / size.y;
        blockSize.z = transform.localScale.z / size.z;
        map = new int[size.x, size.y, size.z];
        
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    
                    switch (Random.Range(0,4))
                    {
                        case 0:
                            break;
                        case 1:
                            var obj = GenerateBuilt(vec.vec3(x,y,z),Built.N2);
                            
                            break;
                        case 2:
                            
                            break;
                        case 3:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

         return targetList;
    }
}
