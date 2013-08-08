using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Xml;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace BuildLoggers
{
	/// <summary>
	/// <para>This logger is used to log msbuild result in xml format.</para>
	/// <para><b>Syntax: </b></para>
	/// <para>     /logger:XmlLogger,BuildLoggers.dll;logfile=msbuild.xml;verbosity=Quiet|Minimal|...;encoding=ASCII|UTF-8|...</para>
	/// <para><b>Parameters: </b></para>
	/// <para>Logfile: An optional parameter that specifies the file in which to store the log information. Defaults to msbuildresult.xml</para>
	/// <para>Verbosity: An optional parameter that overrides the global verbosity setting for this file logger only. This enables you to log to several loggers, each with a different verbosity. The verbosity setting is case sensitive. e.g: Quiet, Minimal, Normal, Detailed, Diagnostic.</para>
	/// <para>Encoding: An optional parameter that specifies the encoding for the file, e.g: UTF-8.</para>
	/// </summary>
	public class XmlLogger : Logger
	{
		private static readonly char[] FileLoggerParameterDelimiters = new[] { ';' };
		private static readonly char[] FileLoggerParameterValueSplitCharacter = new[] { '=' };
		private XmlTextWriter _xmlWriter;
		private string _logFileName;
		private Encoding _encoding;
		private int _warnings;
		private int _errors;
		private DateTime _startTime;

		public override void Initialize(IEventSource eventSource)
		{
			_logFileName = "msbuildresult.xml";
			_encoding = Encoding.Default;

			InitializeFileLogger();

			eventSource.BuildFinished += BuildFinished;
			eventSource.BuildStarted += BuildStarted;
			eventSource.ErrorRaised += ErrorRaised;
			eventSource.WarningRaised += WarningRaised;

			if (Verbosity != LoggerVerbosity.Quiet)
			{
				eventSource.MessageRaised += MessageRaised;
				eventSource.CustomEventRaised += CustomBuildEventRaised;
				eventSource.ProjectStarted += ProjectStarted;
				eventSource.ProjectFinished += ProjectFinished;
			}

			if (IsVerbosityAtLeast(LoggerVerbosity.Normal))
			{
				eventSource.TargetStarted += TargetStarted;
				eventSource.TargetFinished += TargetFinished;
			}

			if (IsVerbosityAtLeast(LoggerVerbosity.Detailed))
			{
				eventSource.TaskStarted += TaskStarted;
				eventSource.TaskFinished += TaskFinished;
			}
		}

		/// <summary>
		/// Shutdown() is guaranteed to be called by MSBuild at the end of the build, after all
		/// events have been raised.
		/// </summary>
		public override void Shutdown()
		{
			if (_xmlWriter != null)
			{
				_xmlWriter.Close();
			}
		}

		private static bool NotExpectedException(Exception e)
		{
			return ((!(e is UnauthorizedAccessException) && !(e is ArgumentNullException)) && (!(e is PathTooLongException) && !(e is DirectoryNotFoundException))) && ((!(e is NotSupportedException) && !(e is ArgumentException)) && (!(e is SecurityException) && !(e is IOException)));
		}

		private void ParseFileLoggerParameters()
		{
			if (Parameters != null)
			{
				string[] strArray = Parameters.Split(FileLoggerParameterDelimiters);
				
				foreach (string[] strArray2 in from t in strArray where t.Length > 0 select t.Split(FileLoggerParameterValueSplitCharacter))
				{
					ApplyFileLoggerParameter(strArray2[0], strArray2.Length > 1 ? strArray2[1] : null);
				}
			}
		}

		private void ApplyFileLoggerParameter(string parameterName, string parameterValue)
		{
			switch (parameterName.ToUpperInvariant())
			{
				case "LOGFILE":
					_logFileName = parameterValue;
					break;
				case "VERBOSITY":
					Verbosity = (LoggerVerbosity)Enum.Parse(typeof(LoggerVerbosity), parameterValue);
					break;
				case "ENCODING":
					try
					{
						_encoding = Encoding.GetEncoding(parameterValue);
					}
					catch (ArgumentException exception)
					{
						throw new LoggerException(exception.Message, exception.InnerException, "MSB4128", null);
					}

					break;
				case null:
					return;
			}
		}

		private void InitializeFileLogger()
		{
			ParseFileLoggerParameters();
			
			try
			{
				_xmlWriter = new XmlTextWriter(_logFileName, _encoding) { Formatting = Formatting.Indented };
				_xmlWriter.WriteStartDocument();
				_xmlWriter.WriteStartElement("build");
				_xmlWriter.Flush();
			}
			catch (Exception exception)
			{
				if (NotExpectedException(exception))
				{
					throw;
				}

				string message = string.Format(CultureInfo.InvariantCulture, "Invalid File Logger File {0}. {1}", _logFileName, exception.Message);
				
				if (_xmlWriter != null)
				{
					_xmlWriter.Close();
				}

				throw new LoggerException(message, exception.InnerException);
			}
		}

		private void BuildFinished(object sender, BuildFinishedEventArgs e)
		{
			_xmlWriter.WriteStartElement("warnings");
			_xmlWriter.WriteValue(_warnings);
			_xmlWriter.WriteEndElement();
			_xmlWriter.WriteStartElement("errors");
			_xmlWriter.WriteValue(_errors);
			_xmlWriter.WriteEndElement();
			_xmlWriter.WriteStartElement("starttime");
			_xmlWriter.WriteValue(_startTime.ToString());
			_xmlWriter.WriteEndElement();
			_xmlWriter.WriteStartElement("endtime");
			_xmlWriter.WriteValue(DateTime.UtcNow.ToString());
			_xmlWriter.WriteEndElement();
			_xmlWriter.WriteStartElement("timeelapsed");
			
			TimeSpan s = DateTime.UtcNow - _startTime;
			
			_xmlWriter.WriteValue(string.Format(CultureInfo.InvariantCulture, "{0}", s));
			_xmlWriter.WriteEndElement();
			
			LogFinished();
		}

		private void BuildStarted(object sender, BuildStartedEventArgs e)
		{
			_startTime = DateTime.UtcNow;
			LogStarted("build", string.Empty, string.Empty);
		}

		private void ErrorRaised(object sender, BuildErrorEventArgs e)
		{
			_errors++;
			LogErrorOrWarning("error", e.Message, e.Code, e.File, e.LineNumber, e.ColumnNumber, e.Subcategory);
		}

		private void MessageRaised(object sender, BuildMessageEventArgs e)
		{
			LogMessage("message", e.Message, e.Importance);
		}

		private void ProjectFinished(object sender, ProjectFinishedEventArgs e)
		{
			LogFinished();
		}

		private void ProjectStarted(object sender, ProjectStartedEventArgs e)
		{
			LogStarted("project", e.TargetNames, e.ProjectFile);
			
			if (IsVerbosityAtLeast(LoggerVerbosity.Diagnostic))
			{
				_xmlWriter.WriteStartElement("InitialProperties");
				SortedDictionary<string, string> sortedProperties = new SortedDictionary<string, string>();
				
				foreach (DictionaryEntry k in e.Properties.Cast<DictionaryEntry>())
				{
					sortedProperties.Add(k.Key.ToString(), k.Value.ToString());
				}

				foreach (var p in sortedProperties)
				{
					_xmlWriter.WriteStartElement(p.Key);
					_xmlWriter.WriteCData(p.Value);
					_xmlWriter.WriteEndElement();
				}

				_xmlWriter.WriteEndElement();
			}
		}

		private void TargetFinished(object sender, TargetFinishedEventArgs e)
		{
			LogFinished();
		}

		private void TargetStarted(object sender, TargetStartedEventArgs e)
		{
			LogStarted("target", e.TargetName, string.Empty);
		}

		private void TaskFinished(object sender, TaskFinishedEventArgs e)
		{
			LogFinished();
		}

		private void TaskStarted(object sender, TaskStartedEventArgs e)
		{
			LogStarted("task", e.TaskName, e.ProjectFile);
		}

		private void WarningRaised(object sender, BuildWarningEventArgs e)
		{
			_warnings++;
			LogErrorOrWarning("warning", e.Message, e.Code, e.File, e.LineNumber, e.ColumnNumber, e.Subcategory);
		}

		private void CustomBuildEventRaised(object sender, CustomBuildEventArgs e)
		{
			LogMessage("custom", e.Message, MessageImportance.Normal);
		}

		private void LogStarted(string elementName, string stageName, string file)
		{
			if (elementName != "build")
				_xmlWriter.WriteStartElement(elementName);

			SetAttribute(elementName == "project" ? "targets" : "name", stageName);
			SetAttribute("file", file);
			SetAttribute("started", DateTime.UtcNow);
			
			_xmlWriter.Flush();
		}

		private void LogFinished()
		{
			_xmlWriter.WriteEndElement();
			_xmlWriter.Flush();
		}

		private void LogErrorOrWarning(string messageType, string message, string code, string file, int line, int column, string subcategory)
		{
			_xmlWriter.WriteStartElement(messageType);
			
			SetAttribute("code", code);
			SetAttribute("file", file);
			SetAttribute("line", line);
			SetAttribute("column", column);
			SetAttribute("subcategory", subcategory);
			
			WriteMessage(message, code != "Properties");
			
			_xmlWriter.WriteEndElement();
		}

		private void LogMessage(string messageType, string message, MessageImportance importance)
		{
			if (importance == MessageImportance.Low && Verbosity != LoggerVerbosity.Detailed && Verbosity != LoggerVerbosity.Diagnostic)
				return;

			if (importance == MessageImportance.Normal && (Verbosity == LoggerVerbosity.Minimal || Verbosity == LoggerVerbosity.Quiet))
				return;

			_xmlWriter.WriteStartElement(messageType);
			SetAttribute("importance", importance);
			WriteMessage(message, false);
			_xmlWriter.WriteEndElement();
		}

		private void WriteMessage(string message, bool escape)
		{
			if (string.IsNullOrEmpty(message))
				return;

			message = message.Replace("&", "&amp;");
			
			if (escape)
			{
				message = message.Replace("<", "&lt;");
				message = message.Replace(">", "&gt;");
			}

			_xmlWriter.WriteCData(message);
		}

		private void SetAttribute(string name, object value)
		{
			if (value == null)
				return;

			Type t = value.GetType();
			
			if (t == typeof(int))
			{
				int number;
				if (int.TryParse(value.ToString(), out number))
					_xmlWriter.WriteAttributeString(name, number.ToString(CultureInfo.InvariantCulture));
			}
			else if (t == typeof(bool))
			{
				_xmlWriter.WriteAttributeString(name, value.ToString());
			}
			else if (t == typeof(MessageImportance))
			{
				MessageImportance importance = (MessageImportance)value;
				_xmlWriter.WriteAttributeString(name, importance.ToString());
			}
			else
			{
				string text = value.ToString();
				
				if (!string.IsNullOrEmpty(text))
					_xmlWriter.WriteAttributeString(name, text);
			}
		}
	}
}
