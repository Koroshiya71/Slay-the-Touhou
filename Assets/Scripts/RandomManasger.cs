using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomManasger : MonoBehaviour
{
    /// <summary>
    /// 随机数方法
    /// </summary>
    /// <param name="count">取随机数的个数</param>
    /// <param name="maxnum">随机数的最小值（包含）</param>
    /// <param name="minnum">随机数的最大值（包含）</param>
    /// <returns>结果数组</returns>
    public static int Random( int minNum, int maxNum)
    {
        int result=0;
        System.Random random = new System.Random(GetRandomSeed());
        result = random.Next(minNum, maxNum + 1);
        return result;
    }
    /// <summary>
    /// 拿到随机种子
    /// </summary>
    /// <returns></returns>
    private static int GetRandomSeed()
    {
        byte[] bytes = new byte[4];
        System.Security.Cryptography.RNGCryptoServiceProvider rng =
            new System.Security.Cryptography.RNGCryptoServiceProvider();
        rng.GetBytes(bytes);
        return BitConverter.ToInt32(bytes, 0);
    }
   
}
