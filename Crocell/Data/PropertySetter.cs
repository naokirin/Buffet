using System;
using System.Reflection;

// Referenced by http://d.hatena.ne.jp/machi_pon/20090821/1250813986

namespace Crocell
{
	public interface ISetter
	{
		void SetValue(object target, object value);
	}

	sealed class Setter<TTarget, TProperty> : ISetter
	{
		private readonly Action<TTarget, TProperty> setter;

		public Setter(Action<TTarget, TProperty> setter)
		{
			this.setter = setter;
		}

		public void SetValue(object target, object value)
		{
			this.setter((TTarget)target, (TProperty)value);
		}
	}

	public static class PropertyExtension
	{
		public static ISetter ToSetter(this PropertyInfo pi)
		{
			Type setterDelegateType = typeof(Action<,>).MakeGenericType(pi.DeclaringType, pi.PropertyType);
			Delegate setter = Delegate.CreateDelegate(setterDelegateType, pi.GetSetMethod());

			Type accessorType = typeof(Setter<,>).MakeGenericType(pi.DeclaringType, pi.PropertyType);
			ISetter accessor = (ISetter)Activator.CreateInstance(accessorType, setter);

			return accessor;
		}
	}
}


