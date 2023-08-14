using UnityEngine;

[CreateAssetMenu(fileName = "IslandData", menuName = "ScriptableObjects/IslandData")]

public sealed class IslandData : ScriptableObject
{
    [Header("ObjectsSettings")][Space] 

    [SerializeField] private GameObject _townHallPrefab;
    public GameObject TownHallPrefab {get => _townHallPrefab;}

    [SerializeField] private CubeTextures _roadBlock;
    public CubeTextures RoadBlock {get => _roadBlock;}

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
        public GameObject[] Prefabs;
        public int Amount;
        [Range(0f, 1f)] public float AppearRate;
        public float MinScale, MaxScale;
        
        [Header("Position")][Space] 
        [Range(0f, 1f)] public float PlacmentOffset;

        [Header("RandomRotation")][Space] 
        [Range(0f, 180f)]public float MaxXRotation;
        [Range(0f, 180f)]public float MaxYRotation;
        [Range(0f, 180f)]public float MaxZRotation;
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
    
    public CubeTextures CorruptionBlock;

    [System.Serializable] public struct EnemyBiomeStage
    {
        [SerializeField] private int _lessZeroHeight;
        public int LessZeroHeight {get => _lessZeroHeight;}

        public DecorationModule DecorationsModule;

        [SerializeField] private int _enemyBiomeRadius;
        public int EnemyBiomeRadius {get => _enemyBiomeRadius;}
        
        public AnimationCurve EnemyBiomeEdgeReductionCurve;
    }

    [Header("IslandResourcesSettings")][Space] 
    [SerializeField] private Resource[] _resources;
    public Resource[] Resources => _resources;

    [System.Serializable] public struct Resource
    {
        public ResourceSource SourcePrefab;
        public int SourcesAmount;
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
