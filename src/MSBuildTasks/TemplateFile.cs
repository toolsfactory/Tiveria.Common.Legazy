using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Tiveria.MSBuildTasks
{
    public class TemplateFile : Task
    {
        public static readonly string MetadataValueTag = "ReplacementValue";
        private ITaskItem _outputFile;
        private string _outputFilename;
        private Regex _regex;
        private ITaskItem _templateFile;
        private Dictionary<string, string> _tokenPairs;
        private ITaskItem[] _tokens;
        private static readonly string DefaultExt = ".out";
        [Output]
        public ITaskItem OutputFile
        {
            get
            {
                return this._outputFile;
            }
            set
            {
                this._outputFile = value;
            }
        }
        public string OutputFilename
        {
            get
            {
                return this._outputFilename;
            }
            set
            {
                this._outputFilename = value;
            }
        }
        [Required]
        public ITaskItem Template
        {
            get
            {
                return this._templateFile;
            }
            set
            {
                this._templateFile = value;
            }
        }
        public ITaskItem[] Tokens
        {
            get
            {
                return this._tokens;
            }
            set
            {
                this._tokens = value;
            }
        }

        public bool EnableLogging { get; set; }

        public TemplateFile()
        {
            this._regex = new Regex("(?<token>\\$\\{(?<identifier>\\w*)\\})", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.Singleline);
            EnableLogging = false;

        }
        public override bool Execute()
        {
            bool result = false;
            if (File.Exists(this._templateFile.ItemSpec))
            {
                this.ParseTokens();
                using (StreamReader streamReader = new StreamReader(this._templateFile.ItemSpec))
                {
                    string value = this._regex.Replace(streamReader.ReadToEnd(), new MatchEvaluator(this.MatchEval));
                    using (StreamWriter streamWriter = new StreamWriter(this.GetOutputFilename()))
                    {
                        streamWriter.Write(value);
                        streamWriter.Flush();
                        if (EnableLogging)
                            base.Log.LogMessage("Template replaced and written to '{0}'", new object[]
						    {
							    this._outputFilename
						    });
                        result = true;
                    }
                    return result;
                }
            }
            if (EnableLogging)
                base.Log.LogError("Template File '{0}' cannot be found", new object[]
			    {
				    this._templateFile.ItemSpec
			    });
            return result;
        }
        private string GetOutputFilename()
        {
            if (string.IsNullOrEmpty(this._outputFilename))
            {
                this._outputFilename = Path.ChangeExtension(this._templateFile.ItemSpec, TemplateFile.DefaultExt);
            }
            this._outputFilename = (Path.IsPathRooted(this._outputFilename) ? this._outputFilename : Path.Combine(Path.GetDirectoryName(this._templateFile.ItemSpec), this._outputFilename));
            this._outputFile = new TaskItem(this._outputFilename);
            return this._outputFilename;
        }
        private string MatchEval(Match match)
        {
            string result = match.Value;
            if (this._tokenPairs.ContainsKey(match.Groups[2].Value))
            {
                result = this._tokenPairs[match.Groups[2].Value];
            }
            return result;
        }
        private void ParseTokens()
        {
            if (EnableLogging)
                base.Log.LogMessage("Found {0} Tokens", new object[]
			    {
				    this._tokens.Length
			    });

            this._tokenPairs = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            if (this._tokens != null)
            {
                ITaskItem[] tokens = this._tokens;
                for (int i = 0; i < tokens.Length; i++)
                {
                    ITaskItem taskItem = tokens[i];
                    if (!string.IsNullOrEmpty(taskItem.ItemSpec))
                    {
                        if (EnableLogging)
                            base.Log.LogMessage("Token: {0} - Value: {1}", new object[]
						    {
							    taskItem.ItemSpec,
							    taskItem.GetMetadata(TemplateFile.MetadataValueTag)
						    });
                        this._tokenPairs.Add(taskItem.ItemSpec, taskItem.GetMetadata(TemplateFile.MetadataValueTag));
                    }
                }
            }
        }
    }
}
