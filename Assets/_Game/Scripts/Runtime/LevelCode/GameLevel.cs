namespace StuckInALoop
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using PrimitiveEngine.Unity;
    using Spineless;
    using UnityEngine;
    using UnityEngine.SceneManagement;


    public class GameLevel : EntityMonoBehaviour
    {
        public const string IndentString = "  ";

        [ReorderableList(
            listHeaderFormat = "Initial Variables",
            hideFooterButtons = true)]
        public List<LevelVariable> Variables = new List<LevelVariable>();

        public LevelLoop MainLoop;

        private Dictionary<string, LevelVariable> variableLookup = new Dictionary<string, LevelVariable>();


        public override void Awake()
        {
            base.Awake();
            foreach (LevelVariable entry in this.Variables)
            {
                if (!this.variableLookup.ContainsKey(entry.Name))
                    this.variableLookup.Add(entry.Name, entry);
            }
        }


        public void Start()
        {
            UpdateLevelCode();
            GameManager.FadeIn(Color.white, 5f, null);
            StartCoroutine(RunLevelCode());
        }


        public LevelVariable GetVariable(string name)
        {
            return this.variableLookup.ContainsKey(name)
                       ? this.variableLookup[name]
                       : null;
        }


        public void LoadNextLevel()
        {
            StopAllCoroutines();
            PoolManager.ManagedPools.Clear();
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }


        public void SetVariable(LevelVariable newVariable)
        {
            if (!this.variableLookup.ContainsKey(newVariable.Name))
            {
                this.variableLookup.Add(newVariable.Name, newVariable);
            }
            else
            {
                this.variableLookup[newVariable.Name] = newVariable;    
            }

            UpdateLevelCode();
        }


        public void SetVariable(string name, string value)
        {
            LevelVariable newVariable = new LevelVariable();
            newVariable.Name = name;
            newVariable.DataValue = value;
            SetVariable(newVariable);
        }


        public void RemoveVariable(string name)
        {
            if (this.variableLookup.ContainsKey(name))
            {
                this.variableLookup.Remove(name);
                UpdateLevelCode();
            }
        }


        public IEnumerator RunLevelCode()
        {
            // Start next frame, to give other components a chance to run Awake().
            yield return null;
            
            yield return this.MainLoop.Evaluate(this, 0, null);
            CompleteLevel();
        }


        public void CompleteLevel()
        {
            Debug.Log("LEVEL FINISHED");
            StopAllCoroutines();
            GameManager.FadeOut(Color.white, 1, LoadNextLevel);
        }


        public void UpdateLevelCode()
        {
            if (GameManager.LevelCodeText != null)
                GameManager.LevelCodeText.text = GetCode();
        }


        public string GetCode()
        {
            string variableDeclarations = "<color=#FF0>";

            if (this.variableLookup.Count > 0)
            {
                foreach (LevelVariable variable in this.variableLookup.Values)
                {
                    variableDeclarations += $"{variable.Name} = {variable.AsString()}\n";
                }

                variableDeclarations += "</color>\n";
            }

            string loopsCode = this.MainLoop.ToCode(this, 0, this.MainLoop);
            
            string finalStatement = this.MainLoop.ShouldContinue(this)
                                        ? "<color=#C00>level++</color>"
                                        : "<color=#0F0>level++</color>";
            
            return $"{variableDeclarations}{loopsCode}\n\n{finalStatement}";
        }
    }



    
}