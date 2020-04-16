using Unity.Networking.Transport;
using Unity.NetCode;
using Unity.Mathematics;

public struct WorldInfoSnapshotData : ISnapshotData<WorldInfoSnapshotData>
{
    public uint tick;
    private int WorldInfoComponentWorldSeed;
    private uint WorldInfoComponentRegionSize;
    private uint WorldInfoComponentDifficultyLevel;
    uint changeMask0;

    public uint Tick => tick;
    public int GetWorldInfoComponentWorldSeed(GhostDeserializerState deserializerState)
    {
        return (int)WorldInfoComponentWorldSeed;
    }
    public int GetWorldInfoComponentWorldSeed()
    {
        return (int)WorldInfoComponentWorldSeed;
    }
    public void SetWorldInfoComponentWorldSeed(int val, GhostSerializerState serializerState)
    {
        WorldInfoComponentWorldSeed = (int)val;
    }
    public void SetWorldInfoComponentWorldSeed(int val)
    {
        WorldInfoComponentWorldSeed = (int)val;
    }
    public byte GetWorldInfoComponentRegionSize(GhostDeserializerState deserializerState)
    {
        return (byte)WorldInfoComponentRegionSize;
    }
    public byte GetWorldInfoComponentRegionSize()
    {
        return (byte)WorldInfoComponentRegionSize;
    }
    public void SetWorldInfoComponentRegionSize(byte val, GhostSerializerState serializerState)
    {
        WorldInfoComponentRegionSize = (uint)val;
    }
    public void SetWorldInfoComponentRegionSize(byte val)
    {
        WorldInfoComponentRegionSize = (uint)val;
    }
    public byte GetWorldInfoComponentDifficultyLevel(GhostDeserializerState deserializerState)
    {
        return (byte)WorldInfoComponentDifficultyLevel;
    }
    public byte GetWorldInfoComponentDifficultyLevel()
    {
        return (byte)WorldInfoComponentDifficultyLevel;
    }
    public void SetWorldInfoComponentDifficultyLevel(byte val, GhostSerializerState serializerState)
    {
        WorldInfoComponentDifficultyLevel = (uint)val;
    }
    public void SetWorldInfoComponentDifficultyLevel(byte val)
    {
        WorldInfoComponentDifficultyLevel = (uint)val;
    }

    public void PredictDelta(uint tick, ref WorldInfoSnapshotData baseline1, ref WorldInfoSnapshotData baseline2)
    {
        var predictor = new GhostDeltaPredictor(tick, this.tick, baseline1.tick, baseline2.tick);
        WorldInfoComponentWorldSeed = predictor.PredictInt(WorldInfoComponentWorldSeed, baseline1.WorldInfoComponentWorldSeed, baseline2.WorldInfoComponentWorldSeed);
        WorldInfoComponentRegionSize = (uint)predictor.PredictInt((int)WorldInfoComponentRegionSize, (int)baseline1.WorldInfoComponentRegionSize, (int)baseline2.WorldInfoComponentRegionSize);
        WorldInfoComponentDifficultyLevel = (uint)predictor.PredictInt((int)WorldInfoComponentDifficultyLevel, (int)baseline1.WorldInfoComponentDifficultyLevel, (int)baseline2.WorldInfoComponentDifficultyLevel);
    }

    public void Serialize(int networkId, ref WorldInfoSnapshotData baseline, ref DataStreamWriter writer, NetworkCompressionModel compressionModel)
    {
        changeMask0 = (WorldInfoComponentWorldSeed != baseline.WorldInfoComponentWorldSeed) ? 1u : 0;
        changeMask0 |= (WorldInfoComponentRegionSize != baseline.WorldInfoComponentRegionSize) ? (1u<<1) : 0;
        changeMask0 |= (WorldInfoComponentDifficultyLevel != baseline.WorldInfoComponentDifficultyLevel) ? (1u<<2) : 0;
        writer.WritePackedUIntDelta(changeMask0, baseline.changeMask0, compressionModel);
        if ((changeMask0 & (1 << 0)) != 0)
            writer.WritePackedIntDelta(WorldInfoComponentWorldSeed, baseline.WorldInfoComponentWorldSeed, compressionModel);
        if ((changeMask0 & (1 << 1)) != 0)
            writer.WritePackedUIntDelta(WorldInfoComponentRegionSize, baseline.WorldInfoComponentRegionSize, compressionModel);
        if ((changeMask0 & (1 << 2)) != 0)
            writer.WritePackedUIntDelta(WorldInfoComponentDifficultyLevel, baseline.WorldInfoComponentDifficultyLevel, compressionModel);
    }

    public void Deserialize(uint tick, ref WorldInfoSnapshotData baseline, ref DataStreamReader reader,
        NetworkCompressionModel compressionModel)
    {
        this.tick = tick;
        changeMask0 = reader.ReadPackedUIntDelta(baseline.changeMask0, compressionModel);
        if ((changeMask0 & (1 << 0)) != 0)
            WorldInfoComponentWorldSeed = reader.ReadPackedIntDelta(baseline.WorldInfoComponentWorldSeed, compressionModel);
        else
            WorldInfoComponentWorldSeed = baseline.WorldInfoComponentWorldSeed;
        if ((changeMask0 & (1 << 1)) != 0)
            WorldInfoComponentRegionSize = reader.ReadPackedUIntDelta(baseline.WorldInfoComponentRegionSize, compressionModel);
        else
            WorldInfoComponentRegionSize = baseline.WorldInfoComponentRegionSize;
        if ((changeMask0 & (1 << 2)) != 0)
            WorldInfoComponentDifficultyLevel = reader.ReadPackedUIntDelta(baseline.WorldInfoComponentDifficultyLevel, compressionModel);
        else
            WorldInfoComponentDifficultyLevel = baseline.WorldInfoComponentDifficultyLevel;
    }
    public void Interpolate(ref WorldInfoSnapshotData target, float factor)
    {
    }
}
