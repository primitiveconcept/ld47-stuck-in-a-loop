namespace StuckInALoop
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using PrimitiveEngine.Unity;
    using UnityEngine;


    [Serializable]
    public class LevelLoop
    {
        public LoopType Type;
        public BreakCondition BreakCondition;

        [Space]
        [ReorderableList(
            listHeaderFormat = "Statements",
            hideFooterButtons = true)]
        public List<CodeStatement> Statements = new List<CodeStatement>();

        [Space]
        [ReorderableList(
            listHeaderFormat = "Nested Loops",
            hideFooterButtons = true)]
        public List<LevelLoop> NestedLoops = new List<LevelLoop>();

        public int Iterator = 0;


        public IEnumerator Evaluate(GameLevel gameLevel, int loopDepth, LevelLoop parentLoop)
        {
            // Main loop should always break out immediately upon finishing.
            if (!gameLevel.MainLoop.ShouldContinue(gameLevel))
            {
                gameLevel.CompleteLevel();
                yield break;
            }
            
            switch (this.Type)
            {
                case LoopType.Do:
                    do
                    {
                        yield return EvaluateChildren(gameLevel, loopDepth, this);
                    }
                    while (ShouldContinue(gameLevel));
                    yield break;
                
                case LoopType.For:
                    for (this.Iterator = 0; ShouldContinue(gameLevel); this.Iterator++)
                    {
                        yield return EvaluateChildren(gameLevel, loopDepth, this);
                    }
                    yield break;
                
                case LoopType.While:
                    while (ShouldContinue(gameLevel))
                    {
                        yield return EvaluateChildren(gameLevel, loopDepth, this);
                    }
                    yield break;
                
                case LoopType.If:
                    if (ShouldContinue(gameLevel))
                        yield return EvaluateChildren(gameLevel, loopDepth, this);
                    yield break;
                
                case LoopType.None:
                    yield return EvaluateChildren(gameLevel, loopDepth, this);
                    yield break;
            }
            
            gameLevel.UpdateLevelCode();
        }


        private IEnumerator EvaluateChildren(GameLevel gameLevel, int loopDepth, LevelLoop parentLoop)
        {
            // Main loop should always break out immediately upon finishing.
            if (!gameLevel.MainLoop.ShouldContinue(gameLevel))
            {
                gameLevel.CompleteLevel();
                yield break;
            }

            foreach (CodeStatement statement in this.Statements)
            {
                yield return statement.Evaluate(gameLevel);
            }

            foreach (LevelLoop nestedLoop in this.NestedLoops)
            {
                yield return nestedLoop.Evaluate(gameLevel, loopDepth + 1, parentLoop);
            }
            gameLevel.UpdateLevelCode();
            yield return null;
        }


        public string ToCode(GameLevel gameLevel, int loopDepth, LevelLoop parentLoop)
        {
            void GetScopeColor(bool breaks, StringBuilder stringBuilder)
            {
                // Determine color of loop code.
                string colorCode = "#0F0";
                if (breaks)
                {
                    switch (this.Type)
                    {
                        case LoopType.None:
                        case LoopType.Do:
                            if (parentLoop != null
                                && parentLoop.ShouldContinue(gameLevel))
                            {
                                colorCode = "#FF0";
                            }
                            else
                            {
                                colorCode = "#F00";
                            }

                            break;
                        case LoopType.For:
                        case LoopType.While:
                        case LoopType.If:
                            colorCode = "#F00";
                            break;
                    }
                }

                stringBuilder.Append($"<color={colorCode}>");
            }

            bool breakConditionMet = !ShouldContinue(gameLevel);
            
            StringBuilder code = new StringBuilder();

            if (parentLoop != null
                && parentLoop.ShouldContinue(gameLevel)
                && gameLevel.MainLoop.ShouldContinue(gameLevel))
            {
                GetScopeColor(breaks: breakConditionMet, stringBuilder: code);
            }

            // Determine how much to indent the loop code.
            string indentTab = "";
            for (int i = 0; i < loopDepth; i++)
            {
                indentTab += GameLevel.IndentString;
            }
            
            // Text for beginning of loop.
            switch(this.Type)
            {
                case LoopType.Do:
                    code.Append("do\n")
                        .Append("{\n");
                    break;
                case LoopType.While:
                    code.Append($"while ({this.BreakCondition.ToString(gameLevel)})\n")
                        .Append("{\n");
                    break;
                case LoopType.For:
                    string iterator = ((char)(104 + loopDepth)).ToString(); // Starts at i
                    code.Append($"for ({iterator} = 0; {this.BreakCondition.ToString(gameLevel, iterator)}; {iterator}++)\n")
                        .Append("{\n");
                    break;
                case LoopType.If:
                    code.Append($"if ({this.BreakCondition.ToString(gameLevel)})\n")
                        .Append("{\n");
                    break;
            }

            // Fill loop body with statements.
            foreach (CodeStatement statement in this.Statements)
            {
                // Hack, first loop doesn't indent statements like it should.
                string extraIndent = loopDepth == 0
                                         ? GameLevel.IndentString
                                         : indentTab;
                code.Append($"{extraIndent}{statement.ToCode()}\n");
            }

            // Add additional line between statements and any nested loops. 
            if (this.Statements.Count > 0
                && this.NestedLoops.Count > 0)
            {
                code.Append("\n");
            }

            // Create code for nested loops.
            foreach (LevelLoop nestedLoop in this.NestedLoops)
            {
                code.Append($"{nestedLoop.ToCode(gameLevel, loopDepth + 1, this)}\n");
            }

            
            
            // End of loop.
            GetScopeColor(breaks: breakConditionMet, stringBuilder: code);
            if (this.Type == LoopType.Do
                && this.BreakCondition != null)
            {
                code.Append("} while (" + this.BreakCondition.ToString(gameLevel) + ")");
            }
            else if (this.Type == LoopType.None)
            {
                
            }
            else
            {
                code.Append("}");    
            }
            code.Append("</color>");
            
            // Apply indent to each line.
            string[] codeLines = code.ToString().Split('\n');
            for (int i = 0; i < codeLines.Length; i++)
            {
                codeLines[i] = indentTab + codeLines[i];
            }

            // Final code.
            string result = string.Join("\n", codeLines);

            if (parentLoop != null
                && !parentLoop.ShouldContinue(gameLevel))
            {
                result += "</color>";
            }

            return result;
        }


        public bool ShouldContinue(GameLevel gameLevel)
        {
            if (this.Type == LoopType.None)
                return true;
            
            bool breakConditionMet = this.Type == LoopType.For
                                         ? this.BreakCondition.ShouldContinue(gameLevel, this.Iterator.ToString())
                                         : this.BreakCondition.ShouldContinue(gameLevel);
            
            return breakConditionMet;
        }
    }
    
    public enum LoopType
    {
        Do,
        While,
        For,
        If,
        None
    }
}