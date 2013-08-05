// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="David Srbecký" email="dsrbecky@gmail.com"/>
//     <version>$Revision$</version>
// </file>

// This file is automatically generated - any changes will be lost

#pragma warning disable 1591

namespace Debugger.Wrappers.CorSym
{
	using System;
	
	
	public partial class ISymUnmanagedSymbolSearchInfo
	{
		
		private Debugger.Interop.CorSym.ISymUnmanagedSymbolSearchInfo wrappedObject;
		
		internal Debugger.Interop.CorSym.ISymUnmanagedSymbolSearchInfo WrappedObject
		{
			get
			{
				return this.wrappedObject;
			}
		}
		
		public ISymUnmanagedSymbolSearchInfo(Debugger.Interop.CorSym.ISymUnmanagedSymbolSearchInfo wrappedObject)
		{
			this.wrappedObject = wrappedObject;
			ResourceManager.TrackCOMObject(wrappedObject, typeof(ISymUnmanagedSymbolSearchInfo));
		}
		
		public static ISymUnmanagedSymbolSearchInfo Wrap(Debugger.Interop.CorSym.ISymUnmanagedSymbolSearchInfo objectToWrap)
		{
			if ((objectToWrap != null))
			{
				return new ISymUnmanagedSymbolSearchInfo(objectToWrap);
			} else
			{
				return null;
			}
		}
		
		~ISymUnmanagedSymbolSearchInfo()
		{
			object o = wrappedObject;
			wrappedObject = null;
			ResourceManager.ReleaseCOMObject(o, typeof(ISymUnmanagedSymbolSearchInfo));
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
		
		public static bool operator ==(ISymUnmanagedSymbolSearchInfo o1, ISymUnmanagedSymbolSearchInfo o2)
		{
			return ((object)o1 == null && (object)o2 == null) ||
			       ((object)o1 != null && (object)o2 != null && o1.WrappedObject == o2.WrappedObject);
		}
		
		public static bool operator !=(ISymUnmanagedSymbolSearchInfo o1, ISymUnmanagedSymbolSearchInfo o2)
		{
			return !(o1 == o2);
		}
		
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		
		public override bool Equals(object o)
		{
			ISymUnmanagedSymbolSearchInfo casted = o as ISymUnmanagedSymbolSearchInfo;
			return (casted != null) && (casted.WrappedObject == wrappedObject);
		}
		
		
		public uint SearchPathLength
		{
			get
			{
				uint pcchPath;
				this.WrappedObject.GetSearchPathLength(out pcchPath);
				return pcchPath;
			}
		}
		
		public void GetSearchPath(uint cchPath, out uint pcchPath, System.IntPtr szPath)
		{
			this.WrappedObject.GetSearchPath(cchPath, out pcchPath, szPath);
		}
		
		public int HRESULT
		{
			get
			{
				int phr;
				this.WrappedObject.GetHRESULT(out phr);
				return phr;
			}
		}
	}
}

#pragma warning restore 1591
