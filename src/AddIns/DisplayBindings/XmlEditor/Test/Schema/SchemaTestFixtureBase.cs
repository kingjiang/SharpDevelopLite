// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Matthew Ward" email="mrward@users.sourceforge.net"/>
//     <version>$Revision$</version>
// </file>

using ICSharpCode.TextEditor.Gui.CompletionWindow;
using ICSharpCode.XmlEditor;
using NUnit.Framework;
using System;
using System.IO;

namespace XmlEditor.Tests.Schema
{
	public abstract class SchemaTestFixtureBase
	{		
		XmlSchemaCompletionData schemaCompletionData;

		/// <summary>
		/// Gets the <see cref="XmlSchemaCompletionData"/> object generated
		/// by this class.
		/// </summary>
		/// <remarks>This object will be null until the <see cref="FixtureInitBase"/>
		/// has been run.</remarks>
		public XmlSchemaCompletionData SchemaCompletionData {
			get {
				return schemaCompletionData;
			}
		}
		
		/// <summary>
		/// Creates the <see cref="XmlSchemaCompletionData"/> object from 
		/// the derived class's schema.
		/// </summary>
		/// <remarks>Calls <see cref="FixtureInit"/> at the end of the method.
		/// </remarks>
		[TestFixtureSetUp]
		public void FixtureInitBase()
		{
			schemaCompletionData = CreateSchemaCompletionDataObject();
			FixtureInit();
		}
		
		/// <summary>
		/// Method overridden by derived class so it can execute its own
		/// fixture initialisation.
		/// </summary>
		public virtual void FixtureInit()
		{
		}
	
		/// <summary>
		/// Checks whether the specified name exists in the completion data.
		/// </summary>
		public static bool Contains(ICompletionData[] items, string name)
		{
			bool Contains = false;
			
			foreach (ICompletionData data in items) {
				if (data.Text == name) {
					Contains = true;
					break;
				}
			}
				
			return Contains;
		}
		
		/// <summary>
		/// Checks whether the completion data specified by name has
		/// the correct description.
		/// </summary>
		public static bool ContainsDescription(ICompletionData[] items, string name, string description)
		{
			bool Contains = false;
			
			foreach (ICompletionData data in items) {
				if (data.Text == name) {
					if (data.Description == description) {
						Contains = true;
						break;						
					}
				}
			}
				
			return Contains;
		}		
		
		/// <summary>
		/// Gets a count of the number of occurrences of a particular name
		/// in the completion data.
		/// </summary>
		public static int GetItemCount(ICompletionData[] items, string name)
		{
			int count = 0;
			
			foreach (ICompletionData data in items) {
				if (data.Text == name) {
					++count;
				}
			}
			
			return count;
		}
		
		/// <summary>
		/// Returns the schema that will be used in this test fixture.
		/// </summary>
		/// <returns></returns>
		protected virtual string GetSchema()
		{
			return String.Empty;
		}
		
		/// <summary>
		/// Creates an <see cref="XmlSchemaCompletionData"/> object that 
		/// will be used in the test fixture.
		/// </summary>
		protected virtual XmlSchemaCompletionData CreateSchemaCompletionDataObject()
		{
			StringReader reader = new StringReader(GetSchema());
			return new XmlSchemaCompletionData(reader);
		}
	}
}
