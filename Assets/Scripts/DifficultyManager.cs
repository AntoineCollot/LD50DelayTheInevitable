using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DifficultyManager
{
    public const float MAX_DIFFICULTY_TIME = 90;
    public static float Difficulty01 { get => Mathf.Clamp01(Time.timeSinceLevelLoad / MAX_DIFFICULTY_TIME); }

    const float MantisAttackPreparationEZ = 5;
    const float MantisAttackPreparationHard = 5;
    public static float MantisAttackPreparation { get => Mathf.Lerp(MantisAttackPreparationEZ, MantisAttackPreparationHard, Difficulty01); }

    const float MoskitoBaseMoveSpeedEZ = 0.2f;
    const float MoskitoBaseMoveSpeedHard = 0.4f;
    public static float MoskitoBaseMoveSpeed { get => Mathf.Lerp(MoskitoBaseMoveSpeedEZ, MoskitoBaseMoveSpeedHard, Difficulty01); }

    const float MantisSpawnIntervalEZ = 25f;
    const float MantisSpawnIntervalHard = 15f;
    public static float MantisSpawnInterval { get => Mathf.Lerp(MantisSpawnIntervalEZ, MantisSpawnIntervalHard, Difficulty01); }

    const float MoskitoSpawnIntervalEZ = 25f;
    const float MoskitoSpawnIntervalHard = 15f;
    public static float MoskitoSpawnInterval { get => Mathf.Lerp(MoskitoSpawnIntervalEZ, MoskitoSpawnIntervalHard, Difficulty01); }
}
