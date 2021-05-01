namespace StuckInALoop
{
    using System;
    using System.Collections;
    using PrimitiveEngine.Unity;
    using Spineless;
    using Spineless.Extensions.Objects;
    using UnityEngine;
    using UnityEngine.Events;


    [Serializable]
    public class CodeStatement
    {
        public CodeStatementType Type;

        public CodeStatementMethodEvent InvokeMethod;

        [ReorderableList(
            listHeaderFormat = "Arguments",
            hideFooterButtons = true)]
        public string[] Arguments;


        public IEnumerator Evaluate(GameLevel gameLevel)
        {
            if (this.Type == CodeStatementType.Assignment)
            {
                yield return EvaluateAssignment(gameLevel);
            }
            
            else if (this.Type == CodeStatementType.Method)
            {
                yield return EvaluateMethod(gameLevel);
            }
            
            else if (this.Type == CodeStatementType.Sleep)
            {
                yield return EvaludateSleep(gameLevel);
            }
            
            gameLevel.UpdateLevelCode();
        }


        public string ToCode()
        {
            switch (this.Type)
            {
                case CodeStatementType.Assignment:
                    return $"{this.Arguments[0]} = {this.Arguments[1]}";
                
                case CodeStatementType.Method:
                    string[] arguments = new string[this.Arguments.Length];
                    for (int i = 0; i < this.Arguments.Length; i++)
                    {
                        arguments[i] = this.Arguments[i];
                    }
                    return $"{this.InvokeMethod.GetPersistentMethodName(0)}({string.Join(", ", arguments)})";
                
                case CodeStatementType.Sleep:
                    return $"Sleep({this.Arguments[0]})";
            }

            return null;
        }


        private IEnumerator EvaluateMethod(GameLevel gameLevel)
        {
            LevelVariable[] variables = new LevelVariable[this.Arguments.Length];
            for (int i = 0; i < variables.Length; i++)
            {
                variables[i] = gameLevel.GetVariable(this.Arguments[i]);
                if (variables[i] == null)
                {
                    variables[i] = new LevelVariable();
                    variables[i].Name = "argument";
                    variables[i].DataValue = this.Arguments[i];
                }
            }

            this.InvokeMethod.Invoke(variables);
            yield return null;
        }


        private IEnumerator EvaluateAssignment(GameLevel gameLevel)
        {
            LevelVariable variable = gameLevel.GetVariable(this.Arguments[0]);
            LevelVariable assignmentVariable = gameLevel.GetVariable(this.Arguments[1]);

            if (assignmentVariable != null)
            {
                variable.DataValue = assignmentVariable.DataValue;
                variable.ObjectValue = assignmentVariable.ObjectValue;
            }
            else
            {
                variable.DataValue = this.Arguments[1];
            }

            yield return null;
        }


        private IEnumerator EvaludateSleep(GameLevel gameLevel)
        {
            LevelVariable variable = gameLevel.GetVariable(this.Arguments[0]);
            int sleepTime = (variable != null)
                ? int.Parse(variable.DataValue)
                : int.Parse(this.Arguments[0]);
            
            yield return new WaitForSeconds(sleepTime);
        }
    }
    
    
    public enum CodeStatementType
    {
        Assignment,
        Method,
        Sleep
    }
    
    [Serializable]
    public class CodeStatementMethodEvent : UnityEvent<LevelVariable[]>
    {}
}