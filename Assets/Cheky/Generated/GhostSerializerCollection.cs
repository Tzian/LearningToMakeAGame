using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Networking.Transport;
using Unity.NetCode;

public struct OurGameNameGhostSerializerCollection : IGhostSerializerCollection
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
    public static int FindGhostType<T>()
        where T : struct, ISnapshotData<T>
    {
        if (typeof(T) == typeof(NetworkPlayerSnapshotData))
            return 0;
        if (typeof(T) == typeof(WorldInfoSnapshotData))
            return 1;
        return -1;
    }

    public void BeginSerialize(ComponentSystemBase system)
    {
        m_NetworkPlayerGhostSerializer.BeginSerialize(system);
        m_WorldInfoGhostSerializer.BeginSerialize(system);
    }

    public int CalculateImportance(int serializer, ArchetypeChunk chunk)
    {
        switch (serializer)
        {
            case 0:
                return m_NetworkPlayerGhostSerializer.CalculateImportance(chunk);
            case 1:
                return m_WorldInfoGhostSerializer.CalculateImportance(chunk);
        }

        throw new ArgumentException("Invalid serializer type");
    }

    public int GetSnapshotSize(int serializer)
    {
        switch (serializer)
        {
            case 0:
                return m_NetworkPlayerGhostSerializer.SnapshotSize;
            case 1:
                return m_WorldInfoGhostSerializer.SnapshotSize;
        }

        throw new ArgumentException("Invalid serializer type");
    }

    public int Serialize(ref DataStreamWriter dataStream, SerializeData data)
    {
        switch (data.ghostType)
        {
            case 0:
            {
                return GhostSendSystem<OurGameNameGhostSerializerCollection>.InvokeSerialize<NetworkPlayerGhostSerializer, NetworkPlayerSnapshotData>(m_NetworkPlayerGhostSerializer, ref dataStream, data);
            }
            case 1:
            {
                return GhostSendSystem<OurGameNameGhostSerializerCollection>.InvokeSerialize<WorldInfoGhostSerializer, WorldInfoSnapshotData>(m_WorldInfoGhostSerializer, ref dataStream, data);
            }
            default:
                throw new ArgumentException("Invalid serializer type");
        }
    }
    private NetworkPlayerGhostSerializer m_NetworkPlayerGhostSerializer;
    private WorldInfoGhostSerializer m_WorldInfoGhostSerializer;
}

public struct EnableOurGameNameGhostSendSystemComponent : IComponentData
{}
public class OurGameNameGhostSendSystem : GhostSendSystem<OurGameNameGhostSerializerCollection>
{
    protected override void OnCreate()
    {
        base.OnCreate();
        RequireSingletonForUpdate<EnableOurGameNameGhostSendSystemComponent>();
    }
}
