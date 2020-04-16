using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Collections;
using Unity.NetCode;
using Cheky.Networking;
using Unity.Physics;
using Unity.Transforms;

public struct NetworkPlayerGhostSerializer : IGhostSerializer<NetworkPlayerSnapshotData>
{
    private ComponentType componentTypeNetworkPlayerComponent;
    private ComponentType componentTypePlayerInfoComponent;
    private ComponentType componentTypePhysicsCollider;
    private ComponentType componentTypeLocalToWorld;
    private ComponentType componentTypeRotation;
    private ComponentType componentTypeTranslation;
    // FIXME: These disable safety since all serializers have an instance of the same type - causing aliasing. Should be fixed in a cleaner way
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<NetworkPlayerComponent> ghostNetworkPlayerComponentType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<PlayerInfoComponent> ghostPlayerInfoComponentType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Rotation> ghostRotationType;
    [NativeDisableContainerSafetyRestriction][ReadOnly] private ArchetypeChunkComponentType<Translation> ghostTranslationType;


    public int CalculateImportance(ArchetypeChunk chunk)
    {
        return 1;
    }

    public int SnapshotSize => UnsafeUtility.SizeOf<NetworkPlayerSnapshotData>();
    public void BeginSerialize(ComponentSystemBase system)
    {
        componentTypeNetworkPlayerComponent = ComponentType.ReadWrite<NetworkPlayerComponent>();
        componentTypePlayerInfoComponent = ComponentType.ReadWrite<PlayerInfoComponent>();
        componentTypePhysicsCollider = ComponentType.ReadWrite<PhysicsCollider>();
        componentTypeLocalToWorld = ComponentType.ReadWrite<LocalToWorld>();
        componentTypeRotation = ComponentType.ReadWrite<Rotation>();
        componentTypeTranslation = ComponentType.ReadWrite<Translation>();
        ghostNetworkPlayerComponentType = system.GetArchetypeChunkComponentType<NetworkPlayerComponent>(true);
        ghostPlayerInfoComponentType = system.GetArchetypeChunkComponentType<PlayerInfoComponent>(true);
        ghostRotationType = system.GetArchetypeChunkComponentType<Rotation>(true);
        ghostTranslationType = system.GetArchetypeChunkComponentType<Translation>(true);
    }

    public void CopyToSnapshot(ArchetypeChunk chunk, int ent, uint tick, ref NetworkPlayerSnapshotData snapshot, GhostSerializerState serializerState)
    {
        snapshot.tick = tick;
        var chunkDataNetworkPlayerComponent = chunk.GetNativeArray(ghostNetworkPlayerComponentType);
        var chunkDataPlayerInfoComponent = chunk.GetNativeArray(ghostPlayerInfoComponentType);
        var chunkDataRotation = chunk.GetNativeArray(ghostRotationType);
        var chunkDataTranslation = chunk.GetNativeArray(ghostTranslationType);
        snapshot.SetNetworkPlayerComponentPlayerId(chunkDataNetworkPlayerComponent[ent].PlayerId, serializerState);
        snapshot.SetPlayerInfoComponentPlayerId(chunkDataPlayerInfoComponent[ent].PlayerId, serializerState);
        snapshot.SetPlayerInfoComponentAvatarModelName(chunkDataPlayerInfoComponent[ent].AvatarModelName, serializerState);
        snapshot.SetPlayerInfoComponentPlayerName(chunkDataPlayerInfoComponent[ent].PlayerName, serializerState);
        snapshot.SetRotationValue(chunkDataRotation[ent].Value, serializerState);
        snapshot.SetTranslationValue(chunkDataTranslation[ent].Value, serializerState);
    }
}
