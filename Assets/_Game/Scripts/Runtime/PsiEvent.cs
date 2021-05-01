namespace StuckInALoop
{
    using PrimitiveEngine.Unity;
    using Spineless;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.Serialization;
    using UnityEngine.Tilemaps;


    public class PsiEvent : MonoBehaviour
    {
        [Header("Scene")]
        public GameObject Psi;
        public GameLevel GameLevel;
        public GameObject SpawnedBackground;
        public GameObject[] WeaponPickupLocations;
        public GameObject[] SummonLocations;

        [Header("Code")]
        public PsiCode WaitingCode;
        [Space][ReorderableList]
        public PsiCode[] ReadyCode;
        [ReorderableList]
        public GameObject[] Backgrounds;

        [Header("Spawns")]
        public Pickup[] WeaponPickups;
        public GameObject[] Enemies;

        private int attackPhaseIndex = -1;
        private bool canIncrement = true;
        private MusicPlayer musicPlayer;
        private SimpleStateMachine<PsiState> state;
        private Renderer[] tilemapRenderers;
        private PsiCode currentPsiCode;


        public void Awake()
        {
            this.state = new SimpleStateMachine<PsiState>(this);
            this.musicPlayer = FindObjectOfType<MusicPlayer>();
            this.tilemapRenderers = FindObjectsOfType<TilemapRenderer>();
            this.Psi.SetActive(false);
        }


        public void Start()
        {
            ToggleRenderers(this.tilemapRenderers, false);
            ToggleBackgrounds(this.Backgrounds, false);
            
            this.state.SetState(PsiState.WaitingForMusicLoop);
        }


        public void Update()
        {
            this.state.Execute();
        }


        public void ContinueRitualSummoning()
        {
            if (GameLevel == null)
                return;
            
            LevelVariable levelVariable = GameLevel.GetVariable("summoningProgress");
            if (levelVariable == null)
            {
                levelVariable = new LevelVariable();
                levelVariable.Name = "summoningProgress";
                levelVariable.DataValue = "1";
                GameLevel.SetVariable(levelVariable);
            }
                
            else if (int.TryParse(levelVariable.DataValue, out int value))
            {
                levelVariable.DataValue = (value + 1).ToString();
                GameLevel.UpdateLevelCode();
            }
        }


        public void Awaken()
        {
            
        }
        

        public void ReplaceLevelCode(PsiCode newCode)
        {
            if (this.currentPsiCode != null)
                this.currentPsiCode.Exit();
            
            this.currentPsiCode = newCode;
            this.GameLevel.SetVariable("response", newCode.PhaseName);
            newCode.Initialize();
            
            this.GameLevel.StopAllCoroutines();
            this.GameLevel.MainLoop = newCode.Code;
            this.GameLevel.UpdateLevelCode();
            this.GameLevel.StartCoroutine(this.GameLevel.RunLevelCode());
        }


        public void UpdateHealthVariable(ObservableRangeInt health)
        {
            this.GameLevel.SetVariable("psiHealth", health.Current.ToString());
        }


        private static bool FloatIsAbout(float value, float target)
        {
            const float fudgeFactor = 0.02f;
            
            return value == target 
                || (value > target - fudgeFactor
                   && value < target + fudgeFactor);
        }


        private void SwitchBackground(GameObject newBackground)
        {
            ToggleBackgrounds(this.Backgrounds, false);
            newBackground.SetActive(true);
        }


        private void IncrementAttackPhase()
        {
            if (!this.canIncrement)
                return;
            
            this.attackPhaseIndex++;
            if (this.attackPhaseIndex >= this.Backgrounds.Length)
                this.attackPhaseIndex = 0;
            GameManager.FadeIn(Color.white, 2f, () => this.canIncrement = true);
            SwitchBackground(this.Backgrounds[this.attackPhaseIndex]);
            ReplaceLevelCode(this.ReadyCode[this.attackPhaseIndex]);

            this.canIncrement = false;
        }


        private static void ToggleBackgrounds(GameObject[] gameObjects, bool value)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.SetActive(value);    
            }
        }


        private static void ToggleRenderers(Renderer[] renderers, bool value)
        {
            foreach (Renderer rendererComponent in renderers)
            {
                rendererComponent.enabled = value;    
            }
        }


        #region States
        private enum PsiState
        {
            WaitingForMusicLoop,
            Spawning,
            PostSpawnWaiting,
            PhaseSwitchCooldown,
            Ready
        }


        public void WaitingForMusicLoop()
        {
            if (this.musicPlayer.MusicIntro.isPlaying
                && this.musicPlayer.MusicIntro.time < 20.6f)
            {
                return;
            }
                
            this.state.SetState(PsiState.Spawning);
            this.GameLevel.RemoveVariable("summoningProgress");
            this.GameLevel.SetVariable("summoning", "COMPLETE");
        }


        public void Spawning()
        {
            GameManager.FadeIn(Color.red, 4f, null);
            GameObject.Find("Vignette").SetActive(false);
            
            ToggleRenderers(this.tilemapRenderers, true);
            SwitchBackground(this.SpawnedBackground);
            
            this.Psi.SetActive(true);
            this.GameLevel.SetVariable(
                "psiHealth", 
                this.Psi.GetComponent<Health>().Max.ToString());
            
            this.GameLevel.SetVariable(
                "player",
                "assessing");
            this.state.SetState(PsiState.PostSpawnWaiting);
            
            ReplaceLevelCode(this.WaitingCode);
        }


        public void PostSpawnWaiting()
        {
            if (FloatIsAbout(this.musicPlayer.MusicIntro.time, 34.2f))
            {
                this.state.SetState(PsiState.Ready);   
                IncrementAttackPhase();
            }
        }


        public void PhaseSwitchCooldown()
        {
            this.GameLevel.SetVariable(
                "player",
                "assessing");
        }


        public void Ready()
        {
            if (FloatIsAbout(this.musicPlayer.MusicLoop.time, 41f))
            {
                IncrementAttackPhase();
            }
        }
        #endregion
    }
}