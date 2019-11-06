using UnityEngine;
using SemihOrhan.WaveOne.StartPoints.StartPointPickers;
using SemihOrhan.WaveOne.StartPoints;
using SemihOrhan.WaveOne.Spawners;
using SemihOrhan.WaveOne.Spawners.SpawnerPickers;
using SemihOrhan.WaveOne.EndPoints;
using SemihOrhan.WaveOne.Formations;

namespace SemihOrhan.WaveOne
{
#pragma warning disable 0649
    public class WaveConfigurator : MonoBehaviour
    {
        [Header("Start point")]
        [SerializeField] private StartPointEnum.StartPointType startPointType;
        [SerializeField] private StartPointPickerEnum.StartPointPickerType startPointPickerType;

        [Header("Spawner")]
        [SerializeField] private SpawnerEnum.SpawnerType spawnerType;
        [SerializeField] private SpawnerPickerEnum.SpawnerPickerType spawnerPickerType;

        [Header("End point")]
        [SerializeField] private EndPointsEnum.EndPointsType endPointsType;

        [Header("Formation")]
        [SerializeField] private FormationEnum.FormationType formationType;

        [SerializeField, HideInInspector] private StartPointEnum.StartPointType prevStartPointType;
        [SerializeField, HideInInspector] private StartPointPickerEnum.StartPointPickerType prevStartPointPickerType;
        [SerializeField, HideInInspector] private SpawnerEnum.SpawnerType prevSpawnerType;
        [SerializeField, HideInInspector] private SpawnerPickerEnum.SpawnerPickerType prevSpawnerPickerType;
        [SerializeField, HideInInspector] private EndPointsEnum.EndPointsType prevEndPointsType;

        [SerializeField, HideInInspector] private FormationEnum.FormationType prevFormationType;

        [SerializeField, HideInInspector] private Component currentStartPoint;
        [SerializeField, HideInInspector] private Component currentStartPointPicker;
        [SerializeField, HideInInspector] private Component currentSpawner;
        [SerializeField, HideInInspector] private Component currentSpawnerPicker;
        [SerializeField, HideInInspector] private Component currentEndPoint;
        [SerializeField, HideInInspector] private Component currentFormation;

        public IStartPoint StartPointScript { get; private set; }
        public ISpawner SpawnerScript { get; private set; }
        public IFormation FormationScript { get; private set; }

        [SerializeField, HideInInspector] private bool needSpawnerPicker;

        private void Start()
        {
            UpdateScriptReferences();
        }

        public void AddStartPointComponents()
        {
            if (prevStartPointType != startPointType ||
                prevStartPointPickerType != startPointPickerType ||
                currentStartPoint == null || currentStartPointPicker == null)
            {
                if (currentStartPointPicker != null)
                    DestroyImmediate(currentStartPointPicker);

                if (prevStartPointType != startPointType || currentStartPoint == null)
                {
                    if (currentStartPoint != null)
                        DestroyImmediate(currentStartPoint);

                    switch (startPointType)
                    {
                        case StartPointEnum.StartPointType.Transform:
                            gameObject.AddComponent<ListOfTransforms>();
                            break;
                        case StartPointEnum.StartPointType.Box:
                            gameObject.AddComponent<ListOfBoxes>();
                            break;
                        case StartPointEnum.StartPointType.Sphere:
                            gameObject.AddComponent<ListOfSpheres>();
                            break;
                    }
                }

                switch (startPointPickerType)
                {
                    case StartPointPickerEnum.StartPointPickerType.InOrder:
                        gameObject.AddComponent<InOrderStartPoint>();
                        break;
                    case StartPointPickerEnum.StartPointPickerType.RandomGuaranteed:
                        gameObject.AddComponent<RandomGuaranteedStartPoint>();
                        break;
                    case StartPointPickerEnum.StartPointPickerType.Random:
                        gameObject.AddComponent<RandomStartPoint>();
                        break;
                    case StartPointPickerEnum.StartPointPickerType.ReverseOrder:
                        gameObject.AddComponent<ReverseOrderStartPoint>();
                        break;
                }

                currentStartPoint = GetComponent<IStartPoint>() as Component;
                currentStartPointPicker = GetComponent<IStartPointPicker>() as Component;

                prevStartPointType = startPointType;
                prevStartPointPickerType = startPointPickerType;
            }
        }

        public void AddSpawnerComponents()
        {
            if (prevSpawnerType != spawnerType ||
                prevSpawnerPickerType != spawnerPickerType ||
                currentSpawner == null || currentSpawnerPicker == null)
            {
                if (currentSpawnerPicker != null)
                    DestroyImmediate(currentSpawnerPicker);

                if (prevSpawnerType != spawnerType || currentSpawner == null)
                {
                    if (currentSpawner != null)
                        DestroyImmediate(currentSpawner);

                    switch (spawnerType)
                    {
                        case SpawnerEnum.SpawnerType.PerWaveCustom:
                            gameObject.AddComponent<PerWaveCustom>();
                            break;
                        case SpawnerEnum.SpawnerType.PerWaveRandomPool:
                            gameObject.AddComponent<PerWaveRandomPool>();
                            break;
                        case SpawnerEnum.SpawnerType.TimedSpawner:
                            gameObject.AddComponent<TimedSpawner>();
                            break;
                    }
                }

                if (needSpawnerPicker)
                {
                    switch (spawnerPickerType)
                    {
                        case SpawnerPickerEnum.SpawnerPickerType.InOrder:
                            gameObject.AddComponent<InOrderSpawner>();
                            break;
                        case SpawnerPickerEnum.SpawnerPickerType.RandomGuaranteed:
                            gameObject.AddComponent<RandomGuaranteedSpawner>();
                            break;
                        case SpawnerPickerEnum.SpawnerPickerType.ReverseOrder:
                            gameObject.AddComponent<ReverseOrderSpawner>();
                            break;
                    }
                }

                currentSpawner = GetComponent<ISpawner>() as Component;
                currentSpawnerPicker = GetComponent<ISpawnerPicker>() as Component;

                prevSpawnerType = spawnerType;
                prevSpawnerPickerType = spawnerPickerType;
            }
        }

        public void AddEndPointComponents()
        {
            if (prevEndPointsType != endPointsType ||
                currentEndPoint == null)
            {
                if (currentEndPoint != null)
                    DestroyImmediate(currentEndPoint);

                switch (endPointsType)
                {
                    case EndPointsEnum.EndPointsType.Disabled:
                        break;
                    case EndPointsEnum.EndPointsType.Enabled:
                        gameObject.AddComponent<EndPoint>();
                        break;
                }

                currentEndPoint = GetComponent<EndPoint>() as Component;

                prevEndPointsType = endPointsType;
            }
        }

        public void AddFormationComponents()
        {
            if (prevFormationType != formationType ||
                currentFormation == null)
            {
                if (currentFormation != null)
                    DestroyImmediate(currentFormation);

                switch (formationType)
                {
                    case FormationEnum.FormationType.Square:
                        gameObject.AddComponent<SquareGroupFormation>();
                        break;
                }

                currentFormation = GetComponent<IFormation>() as Component;

                prevFormationType = formationType;
            }
        }

        public void RemoveAllComponents()
        {
            DestroyImmediate(currentStartPointPicker);
            DestroyImmediate(currentStartPoint);
            DestroyImmediate(currentSpawnerPicker);
            DestroyImmediate(currentSpawner);
            DestroyImmediate(currentEndPoint);
            DestroyImmediate(currentFormation);
        }

        public void UpdateScriptReferences()
        {
            StartPointScript = GetComponent<IStartPoint>();
            SpawnerScript = GetComponent<ISpawner>();
            FormationScript = GetComponent<IFormation>();
        }
    }
}