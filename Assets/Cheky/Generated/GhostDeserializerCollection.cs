using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;
using Unity.NetCode;

public struct OurGameNameGhostDeserializerCollection : IGhostDeserializerCollection
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
    public string[] CreateSerializerNameList()
    {
        var arr = new string[]
        {
            "NetworkPlayerGhostSerializer",
            "WorldInfoGhostSerializer",
        };
        return arr;
    }

    public int Length => 2;
#endif
    public void Initialize(World world)
    {
        var curNetworkPlayerGhostSpawnSystem = world.GetOrCreateSystem<NetworkPlayerGhostSpawnSystem>();
        m_NetworkPlayerSnapshotDataNewGhostIds = curNetworkPlayerGhostSpawnSystem.NewGhostIds;
        m_NetworkPlayerSnapshotDataNewGhosts = curNetworkPlayerGhostSpawnSystem.NewGhosts;
        curNetworkPlayerGhostSpawnSystem.GhostType = 0;
        var curWorldInfoGhostSpawnSystem = world.GetOrCreateSystem<WorldInfoGhostSpawnSystem>();
        m_WorldInfoSnapshotDataNewGhostIds = curWorldInfoGhostSpawnSystem.NewGhostIds;
        m_WorldInfoSnapshotDataNewGhosts = curWorldInfoGhostSpawnSystem.NewGhosts;
        curWorldInfoGhostSpawnSystem.GhostType = 1;
    }

    public void BeginDeserialize(JobComponentSystem system)
    {
        m_NetworkPlayerSnapshotDataFromEntity = system.GetBufferFromEntity<NetworkPlayerSnapshotData>();
        m_WorldInfoSnapshotDataFromEntity = system.GetBufferFromEntity<WorldInfoSnapshotData>();
    }
    public bool Deserialize(int serializer, Entity entity, uint snapshot, uint baseline, uint baseline2, uint baseline3,
        ref DataStreamReader reader, NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
            case 0:
                return GhostReceiveSystem<OurGameNameGhostDeserializerCollection>.InvokeDeserialize(m_NetworkPlayerSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, ref reader, compressionModel);
            case 1:
                return GhostReceiveSystem<OurGameNameGhostDeserializerCollection>.InvokeDeserialize(m_WorldInfoSnapshotDataFromEntity, entity, snapshot, baseline, baseline2,
                baseline3, ref reader, compressionModel);
            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }
    public void Spawn(int serializer, int ghostId, uint snapshot, ref DataStreamReader reader,
        NetworkCompressionModel compressionModel)
    {
        switch (serializer)
        {
            case 0:
                m_NetworkPlayerSnapshotDataNewGhostIds.Add(ghostId);
                m_NetworkPlayerSnapshotDataNewGhosts.Add(GhostReceiveSystem<OurGameNameGhostDeserializerCollection>.InvokeSpawn<NetworkPlayerSnapshotData>(snapshot, ref reader, compressionModel));
                break;
            case 1:
                m_WorldInfoSnapshotDataNewGhostIds.Add(ghostId);
                m_WorldInfoSnapshotDataNewGhosts.Add(GhostReceiveSystem<OurGameNameGhostDeserializerCollection>.InvokeSpawn<WorldInfoSnapshotData>(snapshot, ref reader, compressionModel));
                break;
            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }

    private BufferFromEntity<NetworkPlayerSnapshotData> m_NetworkPlayerSnapshotDataFromEntity;
    private NativeList<int> m_NetworkPlayerSnapshotDataNewGhostIds;
    private NativeList<NetworkPlayerSnapshotData> m_NetworkPlayerSnapshotDataNewGhosts;
    private BufferFromEntity<WorldInfoSnapshotData> m_WorldInfoSnapshotDataFromEntity;
    private NativeList<int> m_WorldInfoSnapshotDataNewGhostIds;
    private NativeList<WorldInfoSnapshotData> m_WorldInfoSnapshotDataNewGhosts;
}
public struct EnableOurGameNameGhostReceiveSystemComponent : IComponentData
{}
public class OurGameNameGhostReceiveSystem : GhostReceiveSystem<OurGameNameGhostDeserializerCollection>
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<EnableOurGameNameGhostReceiveSystemComponent>();
    }
}
