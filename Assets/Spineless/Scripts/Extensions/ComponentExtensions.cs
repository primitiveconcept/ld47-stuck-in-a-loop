namespace Spineless.Extensions.Components
{
    using System.Reflection;
    using UnityEngine;


    public static class ComponentExtensions
    {
        public static T CopyComponentTo<T>(this T original, GameObject gameObject)
            where T : Component
        {
            System.Type type = original.GetType();
            T copiedComponent = gameObject.GetComponent(type) as T;

            if (copiedComponent == null)
                copiedComponent = gameObject.AddComponent(type) as T;

            FieldInfo[] fields = type.GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo field = fields[i];
                if (field.IsStatic)
                    continue;
                field.SetValue(copiedComponent, field.GetValue(original));
            }

            PropertyInfo[] props = type.GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                PropertyInfo prop = props[i];
                if (!prop.CanWrite
                    || !prop.CanWrite
                    || prop.Name == "name"
                    || prop.Name == "tag"
                    || prop.Name == "layer")
                    continue;
                prop.SetValue(copiedComponent, prop.GetValue(original, null), null);
            }

            return copiedComponent;
        }
    }
}