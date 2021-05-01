namespace StuckInALoop
{
    using System;
    using System.Text;
    using Spineless;
    using UnityEngine;


    [Serializable]
    public class BreakCondition
    {
        public string VariableName;
        public Evaluator Is;
        public string Value;


        public bool ShouldContinue(GameLevel gameLevel, string leftValue = null)
        {
            // TODO: Shoddy for loop. 
            if (this.VariableName == null)
                return false;
            
            if (leftValue == null)
            {
                LevelVariable leftVariable = gameLevel.GetVariable(this.VariableName);
                if (leftVariable == null)
                    return false;
                leftValue = leftVariable.AsString();
            }

            LevelVariable rightVariable = gameLevel.GetVariable(this.Value);
            string rightValue = rightVariable != null
                                    ? rightVariable.AsString()
                                    : this.Value;

            switch (this.Is)
            {
                case Evaluator.Equals:
                    return (leftValue == rightValue);
                case Evaluator.LessThan:
                    return (int.Parse(leftValue) < int.Parse(rightValue));
                case Evaluator.MoreThan:
                    return (int.Parse(leftValue) > int.Parse(rightValue));
                case Evaluator.Not:
                    return (leftValue != rightValue);
            }

            return false;
        }


        public string ToString(GameLevel gameLevel, string forIterator = null)
        {
            string conditionVariable = forIterator != null
                                           ? forIterator
                                           : this.VariableName;
            string valueString = this.Value;
            LevelVariable valueVariable = gameLevel.GetVariable(this.Value);
            if (valueVariable != null)
            {
                valueString = valueVariable.Name;
            }
            
            return
                $"{conditionVariable} {StringValueAttribute.StringEnum.GetStringValue(this.Is)} {valueString}";
        }
    }


    public enum Evaluator
    {
        [StringValue("==")]
        Equals,
        
        [StringValue(">")]
        MoreThan,
        
        [StringValue("<")]
        LessThan,
        
        [StringValue("!=")]
        Not
    }
}