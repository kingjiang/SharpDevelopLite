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
	
	
	public partial class ISymUnmanagedWriter
	{
		
		private Debugger.Interop.CorSym.ISymUnmanagedWriter wrappedObject;
		
		internal Debugger.Interop.CorSym.ISymUnmanagedWriter WrappedObject
		{
			get
			{
				return this.wrappedObject;
			}
		}
		
		public ISymUnmanagedWriter(Debugger.Interop.CorSym.ISymUnmanagedWriter wrappedObject)
		{
			this.wrappedObject = wrappedObject;
			ResourceManager.TrackCOMObject(wrappedObject, typeof(ISymUnmanagedWriter));
		}
		
		public static ISymUnmanagedWriter Wrap(Debugger.Interop.CorSym.ISymUnmanagedWriter objectToWrap)
		{
			if ((objectToWrap != null))
			{
				return new ISymUnmanagedWriter(objectToWrap);
			} else
			{
				return null;
			}
		}
		
		~ISymUnmanagedWriter()
		{
			object o = wrappedObject;
			wrappedObject = null;
			ResourceManager.ReleaseCOMObject(o, typeof(ISymUnmanagedWriter));
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
		
		public static bool operator ==(ISymUnmanagedWriter o1, ISymUnmanagedWriter o2)
		{
			return ((object)o1 == null && (object)o2 == null) ||
			       ((object)o1 != null && (object)o2 != null && o1.WrappedObject == o2.WrappedObject);
		}
		
		public static bool operator !=(ISymUnmanagedWriter o1, ISymUnmanagedWriter o2)
		{
			return !(o1 == o2);
		}
		
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		
		public override bool Equals(object o)
		{
			ISymUnmanagedWriter casted = o as ISymUnmanagedWriter;
			return (casted != null) && (casted.WrappedObject == wrappedObject);
		}
		
		
		public ISymUnmanagedDocumentWriter DefineDocument(System.IntPtr url, ref System.Guid language, ref System.Guid languageVendor, ref System.Guid documentType)
		{
			return ISymUnmanagedDocumentWriter.Wrap(this.WrappedObject.DefineDocument(url, ref language, ref languageVendor, ref documentType));
		}
		
		public void SetUserEntryPoint(uint entryMethod)
		{
			this.WrappedObject.SetUserEntryPoint(entryMethod);
		}
		
		public void OpenMethod(uint method)
		{
			this.WrappedObject.OpenMethod(method);
		}
		
		public void CloseMethod()
		{
			this.WrappedObject.CloseMethod();
		}
		
		public uint OpenScope(uint startOffset)
		{
			return this.WrappedObject.OpenScope(startOffset);
		}
		
		public void CloseScope(uint endOffset)
		{
			this.WrappedObject.CloseScope(endOffset);
		}
		
		public void SetScopeRange(uint scopeID, uint startOffset, uint endOffset)
		{
			this.WrappedObject.SetScopeRange(scopeID, startOffset, endOffset);
		}
		
		public void DefineLocalVariable(System.IntPtr name, uint attributes, uint cSig, ref byte signature, uint addrKind, uint addr1, uint addr2, uint addr3, uint startOffset, uint endOffset)
		{
			this.WrappedObject.DefineLocalVariable(name, attributes, cSig, ref signature, addrKind, addr1, addr2, addr3, startOffset, endOffset);
		}
		
		public void DefineParameter(System.IntPtr name, uint attributes, uint sequence, uint addrKind, uint addr1, uint addr2, uint addr3)
		{
			this.WrappedObject.DefineParameter(name, attributes, sequence, addrKind, addr1, addr2, addr3);
		}
		
		public void DefineField(uint parent, System.IntPtr name, uint attributes, uint cSig, ref byte signature, uint addrKind, uint addr1, uint addr2, uint addr3)
		{
			this.WrappedObject.DefineField(parent, name, attributes, cSig, ref signature, addrKind, addr1, addr2, addr3);
		}
		
		public void DefineGlobalVariable(System.IntPtr name, uint attributes, uint cSig, ref byte signature, uint addrKind, uint addr1, uint addr2, uint addr3)
		{
			this.WrappedObject.DefineGlobalVariable(name, attributes, cSig, ref signature, addrKind, addr1, addr2, addr3);
		}
		
		public void Close()
		{
			this.WrappedObject.Close();
		}
		
		public void SetSymAttribute(uint parent, System.IntPtr name, uint cData, ref byte data)
		{
			this.WrappedObject.SetSymAttribute(parent, name, cData, ref data);
		}
		
		public void OpenNamespace(System.IntPtr name)
		{
			this.WrappedObject.OpenNamespace(name);
		}
		
		public void CloseNamespace()
		{
			this.WrappedObject.CloseNamespace();
		}
		
		public void UsingNamespace(System.IntPtr fullName)
		{
			this.WrappedObject.UsingNamespace(fullName);
		}
		
		public void SetMethodSourceRange(ISymUnmanagedDocumentWriter startDoc, uint startLine, uint startColumn, ISymUnmanagedDocumentWriter endDoc, uint endLine, uint endColumn)
		{
			this.WrappedObject.SetMethodSourceRange(startDoc.WrappedObject, startLine, startColumn, endDoc.WrappedObject, endLine, endColumn);
		}
		
		public void Initialize(object emitter, System.IntPtr filename, IStream pIStream, int fFullBuild)
		{
			this.WrappedObject.Initialize(emitter, filename, pIStream.WrappedObject, fFullBuild);
		}
		
		public void GetDebugInfo(ref uint pIDD, uint cData, out uint pcData, System.IntPtr data)
		{
			this.WrappedObject.GetDebugInfo(ref pIDD, cData, out pcData, data);
		}
		
		public void DefineSequencePoints(ISymUnmanagedDocumentWriter document, uint spCount, ref uint offsets, ref uint lines, ref uint columns, ref uint endLines, ref uint endColumns)
		{
			this.WrappedObject.DefineSequencePoints(document.WrappedObject, spCount, ref offsets, ref lines, ref columns, ref endLines, ref endColumns);
		}
		
		public void RemapToken(uint oldToken, uint newToken)
		{
			this.WrappedObject.RemapToken(oldToken, newToken);
		}
		
		public void Initialize2(object emitter, System.IntPtr tempfilename, IStream pIStream, int fFullBuild, System.IntPtr finalfilename)
		{
			this.WrappedObject.Initialize2(emitter, tempfilename, pIStream.WrappedObject, fFullBuild, finalfilename);
		}
		
		public void DefineConstant(System.IntPtr name, object value, uint cSig, ref byte signature)
		{
			this.WrappedObject.DefineConstant(name, value, cSig, ref signature);
		}
		
		public void Abort()
		{
			this.WrappedObject.Abort();
		}
	}
}

#pragma warning restore 1591
