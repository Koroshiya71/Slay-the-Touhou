using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomManasger : MonoBehaviour
{
    /// <summary>
    /// ���������
    /// </summary>
    /// <param name="count">ȡ������ĸ���</param>
    /// <param name="maxnum">���������Сֵ��������</param>
    /// <param name="minnum">����������ֵ��������</param>
    /// <returns>�������</returns>
    public static int Random( int minNum, int maxNum)
    {
        int result=0;
        System.Random random = new System.Random(GetRandomSeed());
        result = random.Next(minNum, maxNum + 1);
        return result;
    }
    /// <summary>
    /// �õ��������
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
