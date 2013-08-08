using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Xml;
using ICSharpCode.Core;
using ICSharpCode.SharpDevelop.BuildWorker;
using ICSharpCode.SharpDevelop.BuildWorker.Interprocess;
using Microsoft.Build.BuildEngine;
using Microsoft.Build.Framework;

namespace SDLite.BuildWorker
{
	/// <summary>
	/// Action delegate.
	/// </summary>
	public delegate void Action<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
	
	internal class MSBuild40
	{
		#region Members
		private Program _program;
		#endregion Members
		
		#region Constructors
		public MSBuild40(Program program)
		{
			_program = program;
		}
		#endregion Constructors
		
		#region	Internal Methods
		private void ReportError(string message, string subcategory, string code, string file, int lineNumber, int columnNumber)
		{
			_program.HostReportEvent(new BuildErrorEventArgs(subcategory, code, file, lineNumber, columnNumber, -1, -1,
			                                                 message, null, "SharpDevelopBuildWorker"));
		}
		
		private void ReportWarning(string message, string subcategory, string code, string file, int lineNumber, int columnNumber)
		{
			_program.HostReportEvent(new BuildWarningEventArgs(subcategory, code, file, lineNumber, columnNumber, -1, -1,
			                                                   message, null, "SharpDevelopBuildWorker"));
		}
		
		private void ReportMessage(string message, string subcategory, string code, string file, int lineNumber, int columnNumber)
		{
			_program.HostReportEvent(new BuildMessageEventArgs(message, null, "SharpDevelopBuildWorker", MessageImportance.High));
		}
		
		private void ReportResult(string projectFile, XmlNodeList list, Action<string, string, string, string, int, int> reportMethod)
		{
			foreach(XmlNode node in list)
			{
				string file = GetAttributeValue(node, "file");
				
				reportMethod(node.InnerText,
				             GetAttributeValue(node, "subcategory"),
				             GetAttributeValue(node, "code"),
				             !string.IsNullOrEmpty(file) ? Path.Combine(Path.GetDirectoryName(projectFile), file) : null,
				             int.Parse(GetAttributeValue(node, "line") ?? "-1", CultureInfo.InvariantCulture),
				             int.Parse(GetAttributeValue(node, "column") ?? "-1", CultureInfo.InvariantCulture));
			}
		}
		
		private string GetAttributeValue(XmlNode node, string attributeName)
		{
			XmlAttribute attribute = node.Attributes[attributeName];
			
			return attribute != null ? attribute.Value : null;
		}
		#endregion	Internal Methods
		
		#region Public Methods
		public bool BuildProject(Project project, BuildJob job)
		{
			string buildResultDirectory = "Temp";
			
			if(!Directory.Exists(buildResultDirectory))
				Directory.CreateDirectory(buildResultDirectory);
			
			// Generate build result file name from project path
			char[] chars = Path.GetInvalidFileNameChars();
			string buildResultFile = project.FullFileName;
			
			foreach(var ch in chars)
				buildResultFile = buildResultFile.Replace(ch, '_');
			
			buildResultFile = Path.Combine(buildResultDirectory,  Path.ChangeExtension(buildResultFile, ".tmp"));
			
			string msBuild = Environment.ExpandEnvironmentVariables(@"%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe");
			string binDirectory = AppDomain.CurrentDomain.BaseDirectory;
			
			if(!File.Exists(msBuild))
			{
				MessageService.ShowError(".Net Framework 4.0 not installed.");
				return false;
			}
			
			Environment.SetEnvironmentVariable("BooBinPath", Path.Combine(binDirectory, @"..\AddIns\AddIns\BackendBindings\BooBinding\v4.0"));
			
			// Start msbuild
			ProcessStartInfo info = new ProcessStartInfo()
			{
				FileName = msBuild,
				//Arguments = string.Format(@"/c {0} ""{1}"" /clp:ErrorsOnly;WarningsOnly /nologo", msBuild, project.FullFileName),
				Arguments = string.Format(@"""{1}"" /logger:XmlLogger,""{2}\MSBuildLoggers40.dll"";logfile=""{3}"" /t:{4} /p:{5} /verbosity:m",
				                          null,	// Reserved for testing in "cmd /c"
				                          project.FullFileName,
				                          Path.Combine(binDirectory, "Tools\\Frameworks"),
				                          buildResultFile,
				                          job.Target,
				                          "Configuration=" + job.Properties["Configuration"] + ";" +
				                          "Platform=" + job.Properties["Platform"]),
				UseShellExecute = false,
				RedirectStandardOutput = true,
				CreateNoWindow = true
			};
			
			File.WriteAllText(Path.Combine(buildResultDirectory, "log.tmp"), info.FileName + " " + info.Arguments);
			
			Process process = Process.Start(info);
			process.WaitForExit();
			
			bool succeeded = process.ExitCode == 0;

			try
			{
				// Parse build result
				XmlDocument document = new XmlDocument();
				document.Load(buildResultFile);

				XmlNodeList projects = document.SelectNodes("/build/project");

				foreach(XmlNode p in projects)
				{
					string projectFile = p.Attributes["file"].Value;

					ReportResult(projectFile, p.SelectNodes("error"), ReportError);
					ReportResult(projectFile, p.SelectNodes("warning"), ReportWarning);
					ReportResult(projectFile, p.SelectNodes("message"), ReportMessage);
				}
			}
			catch(Exception ex)
			{
				LoggingService.Error("Parse build result failed.", ex);
				succeeded = false;
			}
			
			if(!succeeded)
				LoggingService.Info(process.StandardOutput.ReadToEnd());
			
			return succeeded;
		}
		#endregion Public Methods
	}
}
