using System;
using System.Reflection;


namespace NPerf.Core
{
	using NPerf.Core.Collections;

	/// <summary>
	/// Summary description for TypeHelper.
	/// </summary>
	public sealed class TypeHelper
	{
		internal TypeHelper()
		{}

		public static bool HasCustomAttribute(Type t,Type customAttributeType)
		{
			if (t==null)
				throw new ArgumentNullException("t");
			if (customAttributeType==null)
				throw new ArgumentNullException("customAttributeType");

			return t.GetCustomAttributes(customAttributeType,true).Length!=0;
		}

		public static bool HasCustomAttribute(MethodInfo t,Type customAttributeType)
		{
			if (t==null)
				throw new ArgumentNullException("t");
			if (customAttributeType==null)
				throw new ArgumentNullException("customAttributeType");

			return t.GetCustomAttributes(customAttributeType,true).Length!=0;
		}

		public static Object GetFirstCustomAttribute(Type t, Type customAttributeType)
		{
			if (t==null)
				throw new ArgumentNullException("t");
			if (customAttributeType==null)
				throw new ArgumentNullException("customAttributeType");

			Object[] attrs = t.GetCustomAttributes(customAttributeType,true);
			if (attrs.Length==0)
				throw new ArgumentException("type does not have custom attribute");
			return attrs[0];
		}

		public static Object GetFirstCustomAttribute(MethodInfo mi, Type customAttributeType)
		{
			if (mi==null)
				throw new ArgumentNullException("mi");
			if (customAttributeType==null)
				throw new ArgumentNullException("customAttributeType");

			Object[] attrs = mi.GetCustomAttributes(customAttributeType,true);
			if (attrs.Length==0)
				throw new ArgumentException("type does not have custom attribute");
			return attrs[0];
		}

		public static MethodInfo GetAttributedMethod(Type t, Type customAttributeType)
		{
			if (t==null)
				throw new ArgumentNullException("t");
			if (customAttributeType==null)
				throw new ArgumentNullException("customAttributeType");

			foreach(MethodInfo m in t.GetMethods())
			{
				if (HasCustomAttribute(m,customAttributeType))
					return m;
			}

			return null;
		}

		public static AttributedMethodCollection GetAttributedMethods(Type t, Type customAttributeType)
		{
			if (t==null)
				throw new ArgumentNullException("t");
			if (customAttributeType==null)
				throw new ArgumentNullException("customAttributeType");

			return new AttributedMethodCollection(t,customAttributeType);
		}
		
		public static void CheckSignature(MethodInfo mi, Type returnType, params Type[] argumentTypes)
		{
			if (mi==null)
				throw new ArgumentNullException("mi");
			if (returnType==null)
				throw new ArgumentNullException("returnType");
			
			if (mi.ReturnType!=returnType)
				throw new ArgumentException("return type do not match");

			CheckArguments(mi,argumentTypes);		
		}
		
		public static void CheckArguments(MethodInfo mi, params Type[] argumentTypes)
		{
			if (mi==null)
				throw new ArgumentNullException("mi");
			foreach(Type t in argumentTypes)
				if (t==null)
					throw new ArgumentNullException("argumentType");
			
			ParameterInfo[] pis = mi.GetParameters();
			if (pis.Length != argumentTypes.Length)
				throw new ArgumentException("number of arguments do not match");
			
			for(int i = 0; i< pis.Length;++i)
			{
				if (pis[i].ParameterType != argumentTypes[i])
					throw new ArgumentException("argument type do not match");
			}			
		}
	}
}
