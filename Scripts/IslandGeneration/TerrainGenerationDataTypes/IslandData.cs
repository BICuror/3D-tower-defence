using UnityEngine;

[CreateAssetMenu(fileName = "IslandData", menuName = "ScriptableObjects/IslandData")]

public sealed class IslandData : ScriptableObject
{
    [Header("EniviromentSettings")][Space]
    [SerializeField] private GameObject _eniviromentObject;
    public GameObject EniviromentObject => _eniviromentObject; 


    [Header("TextureSettings")][Space] 
    [SerializeField] private Texture _eniviromentTexture;
    public Texture EniviromentTexture => _eniviromentTexture;

    [SerializeField] private Texture _buildingsTexture;
    public Texture BuildingsTexture => _buildingsTexture;

    [Header("RoadSettings")][Space] 
    [SerializeField] private RoadType _roadType;
    public RoadType RoadTypeToGenerate => _roadType;

    [SerializeField] private SpawnerPositionValidator _spawnerPositionValidator;
    public SpawnerPositionValidator SpawnerPositionValidator => _spawnerPositionValidator;
    public enum RoadType
    {
        Node,
        Random
    }
    
    [SerializeField] private CubeTextures _roadBlock;
    public CubeTextures RoadBlock {get => _roadBlock;}

    [SerializeField] private CubeTextures _roadBlockOnWater;
    public CubeTextures RoadBlockOnWater {get => _roadBlockOnWater;}

    [SerializeField] private int _amountOfRoadNodesBetweenCenterAndEdge; 
    public int AmountOfRoadNodesBetweenCenterAndEdge {get => _amountOfRoadNodesBetweenCenterAndEdge;}

    [Header("GeneralIslandSettings")][Space] 

    [SerializeField] private int _islandSize;
    public int IslandSize {get => _islandSize;}

    public int MiddleIndex {get => _islandSize / 2;}

    [SerializeField] private int _islandMaxHeight;
    public int IslandMaxHeight {get => _islandMaxHeight;}

    [SerializeField] private int _islandHeightOffset;
    public int IslandHeightOffset {get => _islandHeightOffset;}
    
    [System.Serializable] public struct NoiseSetting 
    {
        public AnimationCurve NoiseCurve;
        public Vector2 NoiseScale;
    }
    
    [System.Serializable] public struct Biome
    {
        [Range(0f, 1f)] public float AppearRate;
        public string BiomeName;
        public CubeTextures SurfaceBiomBlock;
        public CubeTextures RockBiomeBlock;
        public CubeTextures BedrockBiomBlock;

        public float HeightMultiplier;

        public NoiseSetting[] Noises;

        public DecorationModule DecorationsModule;
    }

    [System.Serializable] public struct DecorationModule
    {
        [Range(0f, 1f)] public float DecorationAppearRate;
        public Decoration[] Decorations; 

    }

    [System.Serializable] public struct Decoration
    {
        [Range(0f, 1f)] [SerializeField] private float _appearRate;
        public float AppearRate => _appearRate;

        public DecorationData DecorationData; 
    }
    
    [Header("BiomesGenerationSettings")][Space] 
    
    [SerializeField] private Biome[] _biomes;
    public Biome[] Biomes {get => _biomes;}

    public NoiseSetting[] BiomeGenerationNoises;

    [Header("EnemyBiomeStagesSettings")][Space] 

    [SerializeField] private int _maxAmountOfEnemyBiomes;
    public int MaxAmountOfEnemyBiomes {get => _maxAmountOfEnemyBiomes;}

    [Range(1, 10)] [SerializeField] private int _begginingAmountOfEnemyBiomes;
    public int BegginingAmountOfEnemyBiomes {get => _begginingAmountOfEnemyBiomes;}

    [SerializeField] private EnemyBiomeStage[] _enemyBiomeStages;
    public EnemyBiomeStage[] EnemyBiomeStages {get => _enemyBiomeStages;}
    
    [SerializeField] private CubeTextures _corruptionBlock;
    public CubeTextures CorruptionBlock => _corruptionBlock;   

    [SerializeField] private CubeTextures _corruptionBlockOnWater;
    public CubeTextures CorruptionBlockOnWater => _corruptionBlockOnWater;        
    
    [SerializeField] private int _corruptionLessZeroHeight;
    public int CorruptionLessZeroHeight {get => _corruptionLessZeroHeight;}


    [System.Serializable] public struct EnemyBiomeStage
    {
        public DecorationModule DecorationsModule;

        [SerializeField] private int _enemyBiomeRadius;
        public int EnemyBiomeRadius {get => _enemyBiomeRadius;}
        
        public AnimationCurve EnemyBiomeEdgeReductionCurve;
    }

    [Header("IslandShapeSettings")][Space] 
    [SerializeField] private SmoothingType _smoothingType;
    public SmoothingType IslandSmoothingType {get => _smoothingType;}
    
    [Range(0f, 1f)]
    [SerializeField] private float _smoothingStrength;
    public float SmoothingStrength {get => _smoothingStrength;}

    [Header("IslandBordersSettings")][Space] 

    [SerializeField] private bool _clearSingleEmptyBlocks;
    public bool ClearSingleEmptyBlocks {get => _clearSingleEmptyBlocks;}

    [SerializeField] private bool _clearSingleSolidBlocks;
    public bool ClearSingleSolidBlocks {get => _clearSingleSolidBlocks;}

    [SerializeField] private AnimationCurve _borderCurve;
    public AnimationCurve BorderCurve {get => _borderCurve;}

    [Range(0f, 0.99f)]
    [SerializeField] private float _edgePrecantageCutout;
    public float EdgePrecantageCutout {get => _edgePrecantageCutout;}

    [Range(0f, 1f)]
    [SerializeField] private float _edgeRandomAdditionalHeight;
    public float EdgeRandomAdditionalHeight {get => _edgeRandomAdditionalHeight;}   
    
    public enum SmoothingType
    {
        None,
        CloseNeibours,
        AllNeibours
    }

    [Header("IslandCenterFlatSettings")][Space] 

    [SerializeField] private bool _centerShouldBeFlat;
    public bool CenterShouldBeFlat {get => _centerShouldBeFlat;}

    [SerializeField] private int _flatRadius;
    public int FlatRadius {get => _flatRadius;}

    [SerializeField] private int _flatHeightIncrease;
    public int FlatHeightIncrease {get => _flatHeightIncrease;}
}
