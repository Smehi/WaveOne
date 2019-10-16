using UnityEngine;
using SemihOrhan.WaveOne.StartPoints.StartPointPickers;
using SemihOrhan.WaveOne.StartPoints;
using SemihOrhan.WaveOne.Spawners;
using SemihOrhan.WaveOne.Spawners.SpawnerPickers;
using SemihOrhan.WaveOne.EndPoints;

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

        [SerializeField, HideInInspector] private StartPointEnum.StartPointType prevStartPointType;
        [SerializeField, HideInInspector] private StartPointPickerEnum.StartPointPickerType prevStartPointPickerType;
        [SerializeField, HideInInspector] private SpawnerEnum.SpawnerType prevSpawnerType;
        [SerializeField, HideInInspector] private SpawnerPickerEnum.SpawnerPickerType prevSpawnerPickerType;
        [SerializeField, HideInInspector] private EndPointsEnum.EndPointsType prevEndPointsType;

        [SerializeField, HideInInspector] private Component currentStartPoint;
        [SerializeField, HideInInspector] private Component currentStartPointPicker;
        [SerializeField, HideInInspector] private Component currentSpawner;
        [SerializeField, HideInInspector] private Component currentSpawnerPicker;
        [SerializeField, HideInInspector] private Component currentEndPoint;

        public IStartPoint StartPointScript { get; private set; }
        public ISpawner SpawnerScript { get; private set; }

        private void Start()
        {
            StartPointScript = GetComponent<IStartPoint>();
            SpawnerScript = GetComponent<ISpawner>();
        }

        public void AddStartPointComponents()
        {
            if (prevStartPointType != startPointType ||
                prevStartPointPickerType != startPointPickerType ||
                currentStartPoint == null || currentStartPointPicker == null)
            {
                if (currentStartPointPicker != null)
                    DestroyImmediate(currentStartPointPicker);

                if (prevStartPointType != startPointType && currentStartPoint != null)
                    DestroyImmediate(currentStartPoint);

                switch (startPointType)
                {
                    case StartPointEnum.StartPointType.Transform:
                        switch (startPointPickerType)
                        {
                            case StartPointPickerEnum.StartPointPickerType.InOrder:
                                currentStartPointPicker = gameObject.AddComponent(typeof(InOrderTransform));
                                break;
                            case StartPointPickerEnum.StartPointPickerType.ReverseOrder:
                                currentStartPointPicker = gameObject.AddComponent(typeof(ReverseOrderTransform));
                                break;
                        }
                        break;
                    case StartPointEnum.StartPointType.Box:
                        switch (startPointPickerType)
                        {
                            case StartPointPickerEnum.StartPointPickerType.InOrder:
                                currentStartPointPicker = gameObject.AddComponent(typeof(InOrderBox));
                                break;
                            case StartPointPickerEnum.StartPointPickerType.ReverseOrder:
                                currentStartPointPicker = gameObject.AddComponent(typeof(ReverseOrderBox));
                                break;
                        }
                        break;
                    case StartPointEnum.StartPointType.Sphere:
                        switch (startPointPickerType)
                        {
                            case StartPointPickerEnum.StartPointPickerType.InOrder:
                                currentStartPointPicker = gameObject.AddComponent(typeof(InOrderSphere));
                                break;
                            case StartPointPickerEnum.StartPointPickerType.ReverseOrder:
                                currentStartPointPicker = gameObject.AddComponent(typeof(ReverseOrderSphere));
                                break;
                        }
                        break;
                }

                currentStartPoint = GetComponent<IStartPoint>() as Component;

                prevStartPointType = startPointType;
                prevStartPointPickerType = startPointPickerType;
            }
        }

        public void AddSpawnerComponents()
        {
            if (prevSpawnerType != spawnerType ||
                prevSpawnerPickerType != spawnerPickerType ||
                currentSpawner == null)
            {
                if (currentSpawnerPicker != null)
                    DestroyImmediate(currentSpawnerPicker);

                if (prevSpawnerType != spawnerType && currentSpawner != null)
                    DestroyImmediate(currentSpawner);

                switch (spawnerType)
                {
                    case SpawnerEnum.SpawnerType.PerWaveCustom:
                        break;
                    case SpawnerEnum.SpawnerType.PerWaveRandomPool:
                        break;
                }

                switch (spawnerPickerType)
                {
                    case SpawnerPickerEnum.SpawnerPickerType.InOrder:
                        gameObject.AddComponent(typeof(InOrderSpawner));
                        break;
                    case SpawnerPickerEnum.SpawnerPickerType.RandomGuaranteed:
                        gameObject.AddComponent(typeof(RandomGuaranteedSpawner));
                        break;
                    case SpawnerPickerEnum.SpawnerPickerType.ReverseOrder:
                        break;
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
                        gameObject.AddComponent(typeof(EndPoint));
                        break;
                }

                currentEndPoint = GetComponent<EndPoint>() as Component;

                prevEndPointsType = endPointsType;
            }
        }

        public void RemoveAllComponents()
        {
            DestroyImmediate(currentStartPointPicker);
            DestroyImmediate(currentStartPoint);
            DestroyImmediate(currentSpawner);
            DestroyImmediate(currentEndPoint);
        }
    }
}