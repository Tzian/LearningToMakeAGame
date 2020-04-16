using Unity.Networking.Transport;
using Unity.NetCode;
using Unity.Mathematics;
using Unity.Collections;

public struct NetworkPlayerSnapshotData : ISnapshotData<NetworkPlayerSnapshotData>
{
    public uint tick;
    private int NetworkPlayerComponentPlayerId;
    private int PlayerInfoComponentPlayerId;
    private NativeString64 PlayerInfoComponentAvatarModelName;
    private NativeString64 PlayerInfoComponentPlayerName;
    private int RotationValueX;
    private int RotationValueY;
    private int RotationValueZ;
    private int RotationValueW;
    private int TranslationValueX;
    private int TranslationValueY;
    private int TranslationValueZ;
    uint changeMask0;

    public uint Tick => tick;
    public int GetNetworkPlayerComponentPlayerId(GhostDeserializerState deserializerState)
    {
        return (int)NetworkPlayerComponentPlayerId;
    }
    public int GetNetworkPlayerComponentPlayerId()
    {
        return (int)NetworkPlayerComponentPlayerId;
    }
    public void SetNetworkPlayerComponentPlayerId(int val, GhostSerializerState serializerState)
    {
        NetworkPlayerComponentPlayerId = (int)val;
    }
    public void SetNetworkPlayerComponentPlayerId(int val)
    {
        NetworkPlayerComponentPlayerId = (int)val;
    }
    public int GetPlayerInfoComponentPlayerId(GhostDeserializerState deserializerState)
    {
        return (int)PlayerInfoComponentPlayerId;
    }
    public int GetPlayerInfoComponentPlayerId()
    {
        return (int)PlayerInfoComponentPlayerId;
    }
    public void SetPlayerInfoComponentPlayerId(int val, GhostSerializerState serializerState)
    {
        PlayerInfoComponentPlayerId = (int)val;
    }
    public void SetPlayerInfoComponentPlayerId(int val)
    {
        PlayerInfoComponentPlayerId = (int)val;
    }
    public NativeString64 GetPlayerInfoComponentAvatarModelName(GhostDeserializerState deserializerState)
    {
        return PlayerInfoComponentAvatarModelName;
    }
    public NativeString64 GetPlayerInfoComponentAvatarModelName()
    {
        return PlayerInfoComponentAvatarModelName;
    }
    public void SetPlayerInfoComponentAvatarModelName(NativeString64 val, GhostSerializerState serializerState)
    {
        PlayerInfoComponentAvatarModelName = val;
    }
    public void SetPlayerInfoComponentAvatarModelName(NativeString64 val)
    {
        PlayerInfoComponentAvatarModelName = val;
    }
    public NativeString64 GetPlayerInfoComponentPlayerName(GhostDeserializerState deserializerState)
    {
        return PlayerInfoComponentPlayerName;
    }
    public NativeString64 GetPlayerInfoComponentPlayerName()
    {
        return PlayerInfoComponentPlayerName;
    }
    public void SetPlayerInfoComponentPlayerName(NativeString64 val, GhostSerializerState serializerState)
    {
        PlayerInfoComponentPlayerName = val;
    }
    public void SetPlayerInfoComponentPlayerName(NativeString64 val)
    {
        PlayerInfoComponentPlayerName = val;
    }
    public quaternion GetRotationValue(GhostDeserializerState deserializerState)
    {
        return GetRotationValue();
    }
    public quaternion GetRotationValue()
    {
        return new quaternion(RotationValueX * 0.001f, RotationValueY * 0.001f, RotationValueZ * 0.001f, RotationValueW * 0.001f);
    }
    public void SetRotationValue(quaternion q, GhostSerializerState serializerState)
    {
        SetRotationValue(q);
    }
    public void SetRotationValue(quaternion q)
    {
        RotationValueX = (int)(q.value.x * 1000);
        RotationValueY = (int)(q.value.y * 1000);
        RotationValueZ = (int)(q.value.z * 1000);
        RotationValueW = (int)(q.value.w * 1000);
    }
    public float3 GetTranslationValue(GhostDeserializerState deserializerState)
    {
        return GetTranslationValue();
    }
    public float3 GetTranslationValue()
    {
        return new float3(TranslationValueX * 0.01f, TranslationValueY * 0.01f, TranslationValueZ * 0.01f);
    }
    public void SetTranslationValue(float3 val, GhostSerializerState serializerState)
    {
        SetTranslationValue(val);
    }
    public void SetTranslationValue(float3 val)
    {
        TranslationValueX = (int)(val.x * 100);
        TranslationValueY = (int)(val.y * 100);
        TranslationValueZ = (int)(val.z * 100);
    }

    public void PredictDelta(uint tick, ref NetworkPlayerSnapshotData baseline1, ref NetworkPlayerSnapshotData baseline2)
    {
        var predictor = new GhostDeltaPredictor(tick, this.tick, baseline1.tick, baseline2.tick);
        NetworkPlayerComponentPlayerId = predictor.PredictInt(NetworkPlayerComponentPlayerId, baseline1.NetworkPlayerComponentPlayerId, baseline2.NetworkPlayerComponentPlayerId);
        PlayerInfoComponentPlayerId = predictor.PredictInt(PlayerInfoComponentPlayerId, baseline1.PlayerInfoComponentPlayerId, baseline2.PlayerInfoComponentPlayerId);
        RotationValueX = predictor.PredictInt(RotationValueX, baseline1.RotationValueX, baseline2.RotationValueX);
        RotationValueY = predictor.PredictInt(RotationValueY, baseline1.RotationValueY, baseline2.RotationValueY);
        RotationValueZ = predictor.PredictInt(RotationValueZ, baseline1.RotationValueZ, baseline2.RotationValueZ);
        RotationValueW = predictor.PredictInt(RotationValueW, baseline1.RotationValueW, baseline2.RotationValueW);
        TranslationValueX = predictor.PredictInt(TranslationValueX, baseline1.TranslationValueX, baseline2.TranslationValueX);
        TranslationValueY = predictor.PredictInt(TranslationValueY, baseline1.TranslationValueY, baseline2.TranslationValueY);
        TranslationValueZ = predictor.PredictInt(TranslationValueZ, baseline1.TranslationValueZ, baseline2.TranslationValueZ);
    }

    public void Serialize(int networkId, ref NetworkPlayerSnapshotData baseline, ref DataStreamWriter writer, NetworkCompressionModel compressionModel)
    {
        changeMask0 = (NetworkPlayerComponentPlayerId != baseline.NetworkPlayerComponentPlayerId) ? 1u : 0;
        changeMask0 |= (PlayerInfoComponentPlayerId != baseline.PlayerInfoComponentPlayerId) ? (1u<<1) : 0;
        changeMask0 |= PlayerInfoComponentAvatarModelName.Equals(baseline.PlayerInfoComponentAvatarModelName) ? 0 : (1u<<2);
        changeMask0 |= PlayerInfoComponentPlayerName.Equals(baseline.PlayerInfoComponentPlayerName) ? 0 : (1u<<3);
        changeMask0 |= (RotationValueX != baseline.RotationValueX ||
                                           RotationValueY != baseline.RotationValueY ||
                                           RotationValueZ != baseline.RotationValueZ ||
                                           RotationValueW != baseline.RotationValueW) ? (1u<<4) : 0;
        changeMask0 |= (TranslationValueX != baseline.TranslationValueX ||
                                           TranslationValueY != baseline.TranslationValueY ||
                                           TranslationValueZ != baseline.TranslationValueZ) ? (1u<<5) : 0;
        writer.WritePackedUIntDelta(changeMask0, baseline.changeMask0, compressionModel);
        if ((changeMask0 & (1 << 0)) != 0)
            writer.WritePackedIntDelta(NetworkPlayerComponentPlayerId, baseline.NetworkPlayerComponentPlayerId, compressionModel);
        if ((changeMask0 & (1 << 1)) != 0)
            writer.WritePackedIntDelta(PlayerInfoComponentPlayerId, baseline.PlayerInfoComponentPlayerId, compressionModel);
        if ((changeMask0 & (1 << 2)) != 0)
            writer.WritePackedStringDelta(PlayerInfoComponentAvatarModelName, baseline.PlayerInfoComponentAvatarModelName, compressionModel);
        if ((changeMask0 & (1 << 3)) != 0)
            writer.WritePackedStringDelta(PlayerInfoComponentPlayerName, baseline.PlayerInfoComponentPlayerName, compressionModel);
        if ((changeMask0 & (1 << 4)) != 0)
        {
            writer.WritePackedIntDelta(RotationValueX, baseline.RotationValueX, compressionModel);
            writer.WritePackedIntDelta(RotationValueY, baseline.RotationValueY, compressionModel);
            writer.WritePackedIntDelta(RotationValueZ, baseline.RotationValueZ, compressionModel);
            writer.WritePackedIntDelta(RotationValueW, baseline.RotationValueW, compressionModel);
        }
        if ((changeMask0 & (1 << 5)) != 0)
        {
            writer.WritePackedIntDelta(TranslationValueX, baseline.TranslationValueX, compressionModel);
            writer.WritePackedIntDelta(TranslationValueY, baseline.TranslationValueY, compressionModel);
            writer.WritePackedIntDelta(TranslationValueZ, baseline.TranslationValueZ, compressionModel);
        }
    }

    public void Deserialize(uint tick, ref NetworkPlayerSnapshotData baseline, ref DataStreamReader reader,
        NetworkCompressionModel compressionModel)
    {
        this.tick = tick;
        changeMask0 = reader.ReadPackedUIntDelta(baseline.changeMask0, compressionModel);
        if ((changeMask0 & (1 << 0)) != 0)
            NetworkPlayerComponentPlayerId = reader.ReadPackedIntDelta(baseline.NetworkPlayerComponentPlayerId, compressionModel);
        else
            NetworkPlayerComponentPlayerId = baseline.NetworkPlayerComponentPlayerId;
        if ((changeMask0 & (1 << 1)) != 0)
            PlayerInfoComponentPlayerId = reader.ReadPackedIntDelta(baseline.PlayerInfoComponentPlayerId, compressionModel);
        else
            PlayerInfoComponentPlayerId = baseline.PlayerInfoComponentPlayerId;
        if ((changeMask0 & (1 << 2)) != 0)
            PlayerInfoComponentAvatarModelName = reader.ReadPackedStringDelta(baseline.PlayerInfoComponentAvatarModelName, compressionModel);
        else
            PlayerInfoComponentAvatarModelName = baseline.PlayerInfoComponentAvatarModelName;
        if ((changeMask0 & (1 << 3)) != 0)
            PlayerInfoComponentPlayerName = reader.ReadPackedStringDelta(baseline.PlayerInfoComponentPlayerName, compressionModel);
        else
            PlayerInfoComponentPlayerName = baseline.PlayerInfoComponentPlayerName;
        if ((changeMask0 & (1 << 4)) != 0)
        {
            RotationValueX = reader.ReadPackedIntDelta(baseline.RotationValueX, compressionModel);
            RotationValueY = reader.ReadPackedIntDelta(baseline.RotationValueY, compressionModel);
            RotationValueZ = reader.ReadPackedIntDelta(baseline.RotationValueZ, compressionModel);
            RotationValueW = reader.ReadPackedIntDelta(baseline.RotationValueW, compressionModel);
        }
        else
        {
            RotationValueX = baseline.RotationValueX;
            RotationValueY = baseline.RotationValueY;
            RotationValueZ = baseline.RotationValueZ;
            RotationValueW = baseline.RotationValueW;
        }
        if ((changeMask0 & (1 << 5)) != 0)
        {
            TranslationValueX = reader.ReadPackedIntDelta(baseline.TranslationValueX, compressionModel);
            TranslationValueY = reader.ReadPackedIntDelta(baseline.TranslationValueY, compressionModel);
            TranslationValueZ = reader.ReadPackedIntDelta(baseline.TranslationValueZ, compressionModel);
        }
        else
        {
            TranslationValueX = baseline.TranslationValueX;
            TranslationValueY = baseline.TranslationValueY;
            TranslationValueZ = baseline.TranslationValueZ;
        }
    }
    public void Interpolate(ref NetworkPlayerSnapshotData target, float factor)
    {
        SetRotationValue(math.slerp(GetRotationValue(), target.GetRotationValue(), factor));
        SetTranslationValue(math.lerp(GetTranslationValue(), target.GetTranslationValue(), factor));
    }
}
