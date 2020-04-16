using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Collections;
using Unity.NetCode;
using Cheky.Networking;

public struct WorldInfoGhostSerializer : IGhostSerializer<WorldInfoSnapshotData>
{
    private ComponentType componentTypeWorldInfoComponent;
    // FIXME: These disable safety since all serializers have an instance of the same type - causing aliasing. Should be fixed in a cleaner way
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<WorldInfoComponent> ghostWorldInfoComponentType;


    public int CalculateImportance(ArchetypeChunk chunk)
    {
        return 1;
    }

    public int SnapshotSize => UnsafeUtility.SizeOf<WorldInfoSnapshotData>();
    public void BeginSerialize(ComponentSystemBase system)
    {
        componentTypeWorldInfoComponent = ComponentType.ReadWrite<WorldInfoComponent>();
        ghostWorldInfoComponentType = system.GetArchetypeChunkComponentType<WorldInfoComponent>(true);
    }

    public void CopyToSnapshot(ArchetypeChunk chunk, int ent, uint tick, ref WorldInfoSnapshotData snapshot, GhostSerializerState serializerState)
    {
        snapshot.tick = tick;
        var chunkDataWorldInfoComponent = chunk.GetNativeArray(ghostWorldInfoComponentType);
        snapshot.SetWorldInfoComponentWorldSeed(chunkDataWorldInfoComponent[ent].WorldSeed, serializerState);
        snapshot.SetWorldInfoComponentRegionSize(chunkDataWorldInfoComponent[ent].RegionSize, serializerState);
        snapshot.SetWorldInfoComponentDifficultyLevel(chunkDataWorldInfoComponent[ent].DifficultyLevel, serializerState);
    }
}
