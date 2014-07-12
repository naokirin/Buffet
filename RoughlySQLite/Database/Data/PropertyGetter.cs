using System;
using System.Reflection;

// Referenced by http://d.hatena.ne.jp/machi_pon/20090821/1250813986

namespace RoughlySQLite
{
	public interface IGetter
	{
		object GetValue(object target);
	}

	sealed class Getter<TTarget, TProperty> : IGetter
	{
		private readonly Func<TTarget, TProperty> getter;

		public Getter(Func<TTarget, TProperty> getter)
		{
			this.getter = getter;
		}

		public object GetValue(object target)
		{
			return this.getter((TTarget)target);
			;
		}
	}

	public static class PropertyExtension
	{
		public static IGetter ToGetter(this PropertyInfo pi)
		{
			Type getterDelegateType = typeof(Func<,>).MakeGenericType(pi.DeclaringType, pi.PropertyType);
			Delegate getter = Delegate.CreateDelegate(getterDelegateType, pi.GetGetMethod());

			Type accessorType = typeof(Getter<,>).MakeGenericType(pi.DeclaringType, pi.PropertyType);
			IGetter accessor = (IGetter)Activator.CreateInstance(accessorType, getter);

			return accessor;
		}
	}
}

