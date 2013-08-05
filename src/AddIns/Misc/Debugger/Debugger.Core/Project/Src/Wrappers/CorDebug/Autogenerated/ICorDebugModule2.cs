// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision$</version>
// </file>

// This file is automatically generated - any changes will be lost

#pragma warning disable 1591

namespace Debugger.Wrappers.CorDebug
{
	using System;
	
	
	public partial class ICorDebugModule2
	{
		
		private Debugger.Interop.CorDebug.ICorDebugModule2 wrappedObject;
		
		internal Debugger.Interop.CorDebug.ICorDebugModule2 WrappedObject
		{
			get
			{
				return this.wrappedObject;
			}
		}
		
		public ICorDebugModule2(Debugger.Interop.CorDebug.ICorDebugModule2 wrappedObject)
		{
			this.wrappedObject = wrappedObject;
			ResourceManager.TrackCOMObject(wrappedObject, typeof(ICorDebugModule2));
		}
		
		public static ICorDebugModule2 Wrap(Debugger.Interop.CorDebug.ICorDebugModule2 objectToWrap)
		{
			if ((objectToWrap != null))
			{
				return new ICorDebugModule2(objectToWrap);
			} else
			{
				return null;
			}
		}
		
		~ICorDebugModule2()
		{
			object o = wrappedObject;
			wrappedObject = null;
			ResourceManager.ReleaseCOMObject(o, typeof(ICorDebugModule2));
		}
		
		public bool Is<T>() where T: class
		{
			System.Reflection.ConstructorInfo ctor = typeof(T).GetConstructors()[0];
			System.Type paramType = ctor.GetParameters()[0].ParameterType;
			return paramType.IsInstanceOfType(this.WrappedObject);
		}
		
		public T As<T>() where T: class
		{
			try {
				return CastTo<T>();
			} catch {
				return null;
			}
		}
		
		public T CastTo<T>() where T: class
		{
			return (T)Activator.CreateInstance(typeof(T), this.WrappedObject);
		}
		
		public static bool operator ==(ICorDebugModule2 o1, ICorDebugModule2 o2)
		{
			return ((object)o1 == null && (object)o2 == null) ||
			       ((object)o1 != null && (object)o2 != null && o1.WrappedObject == o2.WrappedObject);
		}
		
		public static bool operator !=(ICorDebugModule2 o1, ICorDebugModule2 o2)
		{
			return !(o1 == o2);
		}
		
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		
		public override bool Equals(object o)
		{
			ICorDebugModule2 casted = o as ICorDebugModule2;
			return (casted != null) && (casted.WrappedObject == wrappedObject);
		}
		
		
		public void SetJMCStatus(int bIsJustMyCode, uint cTokens, ref uint pTokens)
		{
			this.WrappedObject.SetJMCStatus(bIsJustMyCode, cTokens, ref pTokens);
		}
		
		public void ApplyChanges(uint cbMetadata, byte[] pbMetadata, uint cbIL, byte[] pbIL)
		{
			this.WrappedObject.ApplyChanges(cbMetadata, pbMetadata, cbIL, pbIL);
		}
		
		public void SetJITCompilerFlags(uint dwFlags)
		{
			this.WrappedObject.SetJITCompilerFlags(dwFlags);
		}
		
		public uint JITCompilerFlags
		{
			get
			{
				uint pdwFlags;
				this.WrappedObject.GetJITCompilerFlags(out pdwFlags);
				return pdwFlags;
			}
		}
		
		public void ResolveAssembly(uint tkAssemblyRef, ref ICorDebugAssembly ppAssembly)
		{
			Debugger.Interop.CorDebug.ICorDebugAssembly ref_ppAssembly = ppAssembly.WrappedObject;
			this.WrappedObject.ResolveAssembly(tkAssemblyRef, ref ref_ppAssembly);
			ppAssembly = ICorDebugAssembly.Wrap(ref_ppAssembly);
		}
	}
}

#pragma warning restore 1591