namespace StuckInALoop
{
    using System;
    using Object = UnityEngine.Object;


    [Serializable]
    public class LevelVariable
    {
        public string Name;
        public string DataValue;
        public Object ObjectValue;


        public string AsString()
        {
            return this.ObjectValue != null 
                       ? this.ObjectValue.name 
                       : this.DataValue;
        }


        public int AsInteger()
        {
            return int.TryParse(this.DataValue, out int value)
                       ? value
                       : 0;
        }


        public float AsFloat()
        {
            return float.TryParse(this.DataValue, out float value)
                       ? value
                       : 0f;
        }


        public bool AsBoolean()
        {
            // ReSharper disable once SimplifyConditionalTernaryExpression
            return bool.TryParse(this.DataValue, out bool value)
                       ? value
                       : false;
        }


        public T AsObjectType<T>()
            where T : Object
        {
            return this.ObjectValue as T;
        }
    }
}