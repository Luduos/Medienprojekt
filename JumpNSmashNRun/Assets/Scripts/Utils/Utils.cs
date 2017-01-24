using UnityEngine;
using System.Collections;
using System.Reflection;

public static class Utils{
	public static Component CopyComponentFromTo(Component toCopy, GameObject toAddTo){
		System.Type componentType = toCopy.GetType ();

		Component copy = toAddTo.AddComponent (componentType);

		BindingFlags fieldsToGet = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

		System.Reflection.FieldInfo[] componentFields = componentType.GetFields(fieldsToGet);
		foreach(System.Reflection.FieldInfo componentField in componentFields)
		{
			componentField.SetValue (copy, componentField.GetValue (toCopy));
		}
		return copy;
	}
}
