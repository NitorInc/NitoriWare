using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DatingSimListExtension {

    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list) {
        int n = list.Count;
        while (n > 1) {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

