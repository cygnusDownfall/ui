using UnityEngine;
using System.Collections.Generic;

public class gacchaSystem<T> //where T:ScriptableObject
{
    public List<T> repo;
    [SerializeField] byte max = 100;
    public virtual T gaccha()
    {
        if (repo.Count == 0) return default;
        int id = Random.Range(0, repo.Count);
        return repo[id];

    }
}