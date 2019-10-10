using UnityEngine;
using WaveOne.StartPoints.StartPointPickers;
using WaveOne.StartPoints;
using WaveOne.Spawners;

namespace WaveOne
{
#pragma warning disable 0649
    public class WaveConfigurator : MonoBehaviour
    {
        [Header("Start Point")]
        [SerializeField] private StartPointEnum.StartPointType startPointType;
        [SerializeField] private StartPointPickerEnum.StartPointPickerType startPointPickerType;

        [Header("Spawner")]
        [SerializeField] private SpawnerEnum.SpawnerType spawnerType;

        [SerializeField, HideInInspector] private StartPointEnum.StartPointType prevStartPointType;
        [SerializeField, HideInInspector] private StartPointPickerEnum.StartPointPickerType prevStartPointPickerType;
        [SerializeField, HideInInspector] private SpawnerEnum.SpawnerType prevSpawnerType;

        [SerializeField, HideInInspector] private Component currentStartPoint;
        [SerializeField, HideInInspector] private Component currentStartPointPicker;
        [SerializeField, HideInInspector] private Component currentSpawner;

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
                currentSpawner == null)
            {
                if (currentSpawner != null)
                    DestroyImmediate(currentSpawner);

                switch (spawnerType)
                {
                    case SpawnerEnum.SpawnerType.ProgressiveWithDeployment:
                        gameObject.AddComponent(typeof(ProgressiveWithDeployments));
                        break;
                }

                currentSpawner = GetComponent<ISpawner>() as Component;

                prevSpawnerType = spawnerType;
            }
        }

        public void RemoveAllComponents()
        {
            DestroyImmediate(currentStartPointPicker);
            DestroyImmediate(currentStartPoint);
            DestroyImmediate(currentSpawner);
        }
    }
}