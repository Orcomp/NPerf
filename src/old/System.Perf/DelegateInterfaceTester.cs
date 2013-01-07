
namespace System.Perf 
{
	using System;
	using NPerf.Framework;

	// a dummy class with empty call method
	public interface IMethodCall
	{
		void Call();
		void Call(object sender, EventArgs args);
	}
	
	public class MethodCall : IMethodCall
	{		
		public void Call()
		{}
		public void Call(object sender, EventArgs args)
		{}
	}

    // the method caller interface
	public interface IMethodCaller
	{
		void SetMethod(IMethodCall m, int n);
		void CallMethod();		
	}
	
	// first test class, that uses interface call
	public class InterfaceMethodCall : IMethodCaller
	{
		int n = 0;
		IMethodCall mc = null;
		
		public void SetMethod(IMethodCall mc, int n)
		{
			this.n = n;
			this.mc = mc;
		}
		public void CallMethod()
		{
			for(int i = 0;i<n;++i)
				mc.Call();
		}
	}
	
	public delegate void MyDelegate();	
	
	public class DelegateMethodCaller : IMethodCaller
	{
		int n = 0;
		event MyDelegate d = null;
		
		public void SetMethod(IMethodCall mc, int n)
		{
			this.n = n;
			this.d += new MyDelegate(mc.Call);
		}
		public void CallMethod()
		{	
			for(int i = 0;i<n;++i)
				d();			
		}
	}
	
	public class EventHandlerMethodCaller : IMethodCaller
	{
		int n = 0;
		event EventHandler d = null;
		
		public void SetMethod(IMethodCall mc, int n)
		{
			this.n = n;
			this.d += new EventHandler(mc.Call);
		}
		public void CallMethod()
		{	
			for(int i = 0;i<n;++i)
				d(this, new EventArgs());			
		}
	}

	
	[PerfTester(
	            typeof(IMethodCaller),
	            50,
	            Description="Comparing interface and delegate calls",
	            FeatureDescription="Method call number"
	            )]
	public class DelegateInterfaceTester
	{
		internal int CallCount(int index)
		{
			if (index<0)
				return 10;
			else
				return (int)Math.Floor(Math.Pow(index,2)*1000);						
		}
		[PerfRunDescriptor]
		public double RunDescriptor(int index)
		{
			return (double)CallCount(index);			
		}
		
		[PerfSetUp]
		public void SetUp(int index, IMethodCaller mc)
		{
			mc.SetMethod(new MethodCall(), CallCount(index));
		}
		
		[PerfTest]
		public void Call(IMethodCaller mc)
		{
			mc.CallMethod();
		}
	}
}
