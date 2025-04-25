using Unity.AI.Navigation;
using UnityEngine;

public class MapBake : Singleton<MapBake>
{
    public NavMeshSurface[] surfaces;

    public void Bake()
    {
        for(int i=0; i<surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }
}
