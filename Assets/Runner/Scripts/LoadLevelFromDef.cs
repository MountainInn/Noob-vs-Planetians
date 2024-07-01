using System;
using System.Collections;
using System.Collections.Generic;
using HyperCasual.Core;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HyperCasual.Runner
{
    public class LoadWorldLevelState : AbstractState
    {
        public readonly WorldLevel worldLevel;
        readonly SceneController sceneController;
        readonly GameObject[] managerPrefabs;

        public LoadWorldLevelState(SceneController sceneController, WorldLevel worldLevel, GameObject[] managerPrefabs)
        {
            this.worldLevel = worldLevel;
            this.managerPrefabs = managerPrefabs;
            this.sceneController = sceneController;
        }

        public override IEnumerator Execute()
        {
            yield return sceneController.LoadNewScene(worldLevel.name);

            foreach (var prefab in managerPrefabs)
            {
                Object.Instantiate(prefab);
            }

            GameManager.Instance.LoadLevel(worldLevel);
        }
    }

    public class LoadLevelFromDef : AbstractState
    {
        public readonly LevelDefinition m_LevelDefinition;
        readonly SceneController m_SceneController;
        readonly GameObject[] m_ManagerPrefabs;

        public LoadLevelFromDef(SceneController sceneController, AbstractLevelData levelData, GameObject[] managerPrefabs)
        {
            if (levelData is LevelDefinition levelDefinition)
                m_LevelDefinition = levelDefinition;

            m_ManagerPrefabs = managerPrefabs;
            m_SceneController = sceneController;
        }
        
        public override IEnumerator Execute()
        {
            if (m_LevelDefinition == null)
                throw new Exception($"{nameof(m_LevelDefinition)} is null!");

            yield return m_SceneController.LoadNewScene(nameof(m_LevelDefinition));

            // Load managers specific to the level
            foreach (var prefab in m_ManagerPrefabs)
            {
                Object.Instantiate(prefab);
            }

            GameManager.Instance.LoadLevel(m_LevelDefinition);
        }
    }
}
