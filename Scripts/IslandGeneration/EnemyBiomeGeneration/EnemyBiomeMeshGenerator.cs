using UnityEngine;

public sealed class EnemyBiomeMeshGenerator : MeshGenerator
{
    protected override void ApplyTextureToFace(Vector3Int position, Vector3Int checkDirection, BlockType blockType, FaceData faceToApply)
    {
        CubeTextures cubeTextures = _textureManager.GetCubeTexture(position, blockType);

        Vector2[] UVsToAdd = cubeTextures.GetUVsAtDirection(checkDirection);

        for (int i = 0; i < faceToApply.UVOrder.Length; i++)
        {
            UVs.Add(UVsToAdd[faceToApply.UVOrder[i]]);
        }
    }

    protected override bool ShouldCheckThisBlockType(BlockType type)
    {
        return (type == BlockType.Corruption) || (type == BlockType.CorruptionOnWater);
    }
}
