namespace StuckInALoop
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Spineless;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Rendering.PostProcessing;
    using UnityEngine.SceneManagement;


    public class LevelEvents : MonoBehaviour
    {
        private static GameLevel _gameLevel;

        public UnityEvent OnAwake;
        public UnityEvent OnStart;
        public UnityEvent OnDestroyed;

        public List<Spawner> EnemySpawners = new List<Spawner>();

        private Coroutine filterCoroutine;


        public static GameLevel GameLevel
        {
            get
            {
                if (_gameLevel == null)
                    _gameLevel = FindObjectOfType<GameLevel>();
                return _gameLevel;
            }
        }


        public void Awake()
        {
            GameObject[] enemySpawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
            foreach (GameObject enemySpawner in enemySpawners)
            {
                this.EnemySpawners.Add(enemySpawner.GetComponent<Spawner>());
            }
            
            if (this.OnAwake != null)
                this.OnAwake.Invoke();
        }


        public void Start()
        {
            if (this.OnStart != null)
                this.OnStart.Invoke();
        }


        public void OnDestroy()
        {
            if (this.OnDestroyed != null)
                this.OnDestroyed.Invoke();
        }


        public void ThrowStackOverflow()
        {
            throw new StackOverflowException("You fallen too deep into the rabbit hole.");
        }


        public void DebugLog(string message)
        {
            Debug.Log(message);
        }


        public void RemoveThisIf(VariableSet variableSet)
        {
            if (GameLevel == null)
                return;
            
            LevelVariable conditionVariable = variableSet.Variables[0];
            LevelVariable levelVariable = GameLevel.GetVariable(conditionVariable.Name);
            if (levelVariable == null
                || levelVariable.AsString() != conditionVariable.DataValue)
            {
                return;
            }
            
            HideCaption();

            AudioClip destroyAudio = conditionVariable.ObjectValue as AudioClip;
            if (destroyAudio != null)
                PlayOneShotAudio(destroyAudio);
            
            Destroy(this.gameObject);
        }


        public void ReplaceLevelCode(PsiCode newCode)
        {
            if (GameLevel == null)
                return;
            
            GameLevel.MainLoop = newCode.Code;
            GameLevel.UpdateLevelCode();
        }


        public void RemoveObstacle(GameObject gameObject)
        {
            if (gameObject != null)
                Destroy(gameObject);
        }


        public void RemoveObstacle(LevelVariable[] variables)
        {
            foreach (LevelVariable variable in variables)
            {
                if (variable.ObjectValue != null)
                    Destroy(variable.ObjectValue);
            }
        }


        public void TeleportPlayer(LevelVariable[] variables)
        {
            TeleportPlayer(variables[0].ObjectValue as GameObject);
        }


        public void TeleportPlayer(GameObject target)
        {
            GameManager.Player.transform.position = target.transform.position;
        }


        public void ReloadScene()
        {
            if (GameLevel == null)
                return;
            
            GameLevel.StopAllCoroutines();
            PoolManager.ManagedPools.Clear();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


        public void SpawnEnemies()
        {
            foreach (Spawner spawner in this.EnemySpawners)
            {
                spawner.Spawn();
            }
        }


        public void IncrementVariable(string variableName)
        {
            if (GameLevel == null)
                return;
            
            LevelVariable levelVariable = GameLevel.GetVariable(variableName);
            if (levelVariable == null)
            {
                levelVariable = new LevelVariable();
                levelVariable.Name = variableName;
                levelVariable.DataValue = "1";
                GameLevel.SetVariable(levelVariable);
            }
                
            else if (int.TryParse(levelVariable.DataValue, out int value))
            {
                levelVariable.DataValue = (value + 1).ToString();
                GameLevel.UpdateLevelCode();
            }
        }


        public void SetVariable(VariableSet variableSet)
        {
            if (GameLevel == null)
                return;
            
            foreach (LevelVariable variable in variableSet.Variables)
            {
                GameLevel.SetVariable(variable);
            }
        }


        public void IgnorePlaything()
        {
            PostProcessing.ClearFilter();
            ClearFilterCoroutines();
        }


        public void DropHit()
        {
            if (GameLevel == null)
                return;
            
            PostProcessing.SetFilter("Tripping");
            GameLevel.SetVariable("player", "tripping");
            
            if (this.filterCoroutine == null)
            {
                this.filterCoroutine = GameLevel.StartCoroutine(TrippingCoroutine());
            }
        }


        public void Disorient()
        {
            PostProcessing.SetFilter("Sick");
            if (this.filterCoroutine == null)
            {
                Debug.Log("DISORIENT");
                this.filterCoroutine = GameLevel.StartCoroutine(SickCoroutine());
            }
        }


        public void ShowCaption(string message)
        {
            TextMeshPro caption = GameManager.Player.GetComponentInChildren<TextMeshPro>(true);
            caption.text = message;
            caption.gameObject.SetActive(true);
        }


        public void HideCaption()
        {
            TextMeshPro caption = GameManager.Player.GetComponentInChildren<TextMeshPro>(true);
            caption.gameObject.SetActive(false);
        }


        public void PlayOneShotAudio(AudioClip audioClip)
        {
            AudioPlayer.Play(audioClip);
        }


        private IEnumerator SickCoroutine()
        {
            LensDistortion lensDistortion = PostProcessing.CurrentFilter.GetSetting<LensDistortion>();
            lensDistortion.enabled.Override(true);

            while (true)
            {
                for (float i =-1; i < 1; i += 0.01f)
                {
                    if (PostProcessing.CurrentFilter != null)
                    {
                        FloatParameter newValue = new FloatParameter();
                        newValue.value = i;
                        lensDistortion.centerY.Override(newValue);
                    }
                    yield return null;
                }
                
                for (float i = 1; i > -1; i -= 0.01f)
                {
                    if (PostProcessing.CurrentFilter != null)
                    {
                        FloatParameter newValue = new FloatParameter();
                        newValue.value = i;
                        lensDistortion.centerY.Override(newValue);
                    }
                    yield return null;
                }
            }
        }


        private IEnumerator TrippingCoroutine()
        {
            ColorGrading colorGrading = PostProcessing.CurrentFilter.GetSetting<ColorGrading>();
            colorGrading.enabled.Override(true);

            while (true)
            {
                for (int i = 0; i < 101; i++)
                {
                    if (PostProcessing.CurrentFilter != null)
                    {
                        FloatParameter newValue = new FloatParameter();
                        newValue.value = i;
                        colorGrading.temperature.Override(newValue);
                    }
                    yield return null;
                }
                for (int i = -100; i < 1; i++)
                {
                    if (PostProcessing.CurrentFilter != null)
                    {
                        FloatParameter newValue = new FloatParameter();
                        newValue.value = i;
                        colorGrading.temperature.Override(newValue);
                    }
                    yield return null;
                }
            }
        }


        private void ClearFilterCoroutines()
        {
            if (this.filterCoroutine != null)
                GameLevel.StopCoroutine(this.filterCoroutine);
            PostProcessing.ClearFilter();
            this.filterCoroutine = null;
        }


        public void Peek()
        {
            Spooks.Play("Cat-Peek");
        }


        public void Grow(LevelVariable[] variables)
        {
            foreach (LevelVariable variable in variables)
            {
                Grow(variable.ObjectValue as GameObject);
            }
        }


        public void Grow(GameObject gameObject)
        {
            
            float currentScale = gameObject.transform.localScale.x;
            if (currentScale >= 2)
                return;

            Transform[] children = new Transform[gameObject.transform.childCount];
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = gameObject.transform.GetChild(i);
                children[i].SetParent(null);
            }
            
            gameObject.transform.localScale = 
                new Vector3(currentScale + 0.25f, currentScale + 0.25f, 1f);
            
            for (int i = 0; i < children.Length; i++)
            {
                children[i].SetParent(gameObject.transform);
            }
            
            if (gameObject == GameManager.Player)
                UpdatePlayerSizeVariable(gameObject.transform.localScale.x);
        }


        public void Shrink(LevelVariable[] variables)
        {
            foreach (LevelVariable variable in variables)
            {
                Grow(variable.ObjectValue as GameObject);
            }
        }


        public void Shrink(GameObject gameObject)
        {
            float currentScale = gameObject.transform.localScale.x;
            if (currentScale <= 0.5f)
                return;

            Transform[] children = new Transform[gameObject.transform.childCount];
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = gameObject.transform.GetChild(i);
                children[i].SetParent(null);
            }
            
            gameObject.transform.localScale = 
                new Vector3(currentScale - 0.25f, currentScale - 0.25f, 1f);
            
            for (int i = 0; i < children.Length; i++)
            {
                children[i].SetParent(gameObject.transform);
            }
            
            if (gameObject == GameManager.Player)
                UpdatePlayerSizeVariable(gameObject.transform.localScale.x);
        }


        private void UpdatePlayerSizeVariable(float scale)
        {
            if (GameLevel == null)
                return;
            
            if (scale == 1f)
            {
                GameLevel.SetVariable("playerSize", "normal");
            }
            else if (scale > 1)
            {
                GameLevel.SetVariable("playerSize", "large");
            }
            else if (scale < 1)
            {
                GameLevel.SetVariable("playerSize", "small");
            }
                
        }


        public void DecrementVariable(string variableName)
        {
            if (GameLevel == null)
                return;
            
            LevelVariable levelVariable = GameLevel.GetVariable(variableName);
            if (levelVariable != null && int.TryParse(levelVariable.DataValue, out int value))
            {
                levelVariable.DataValue = (value - 1).ToString();
                GameLevel.UpdateLevelCode();
            }
        }
    }
}