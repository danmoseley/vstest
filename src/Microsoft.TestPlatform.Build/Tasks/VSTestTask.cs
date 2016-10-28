﻿// Copyright (c) Microsoft. All rights reserved.

namespace Microsoft.TestPlatform.Build.Tasks
{
    using System.Collections.Generic;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;
    using System;
    using System.IO;

    public class VSTestTask : Task
    {
        public string TestFileFullPath
        {
            get;
            set;
        }

        public string VSTestSetting
        {
            get;
            set;
        }

        public string VSTestTestAdapterPath
        {
            get;
            set;
        }

        public string VSTestFramework
        {
            get;
            set;
        }

        public string VSTestTestCaseFilter
        {
            get;
            set;
        }
        public string VSTestLogger
        {
            get;
            set;
        }

        public string VSTestListTests
        {
            get;
            set;
        }

        public string VSTestDiag
        {
            get;
            set;
        }

        public override bool Execute()
        {
            var vsTestForwardingApp = new VSTestForwardingApp(this.CreateArgument());
            if (!string.IsNullOrEmpty(this.VSTestFramework))
            {
                Console.WriteLine("Test run for {0}({1})", this.TestFileFullPath, this.VSTestFramework);
            }
            vsTestForwardingApp.Execute();
            return true;
        }

        private string AddDoubleQuotes(string x)
        {
            return "\"" + x + "\"";
        }

        private IEnumerable<string> CreateArgument()
        {
            var allArgs = new List<string>();

            // TODO log arguments in task
            if (!string.IsNullOrEmpty(this.VSTestSetting))
            {
                allArgs.Add("--settings:" + this.AddDoubleQuotes(this.VSTestSetting));
            }

            if (!string.IsNullOrEmpty(this.VSTestTestAdapterPath))
            {
                allArgs.Add("--testAdapterPath:" + this.AddDoubleQuotes(this.VSTestTestAdapterPath));
            }

            if (!string.IsNullOrEmpty(this.VSTestFramework))
            {
                allArgs.Add("--framework:" + this.AddDoubleQuotes(this.VSTestFramework));
            }

            if (!string.IsNullOrEmpty(this.VSTestTestCaseFilter))
            {
                allArgs.Add("--testCaseFilter:" + this.AddDoubleQuotes(this.VSTestTestCaseFilter));
            }

            if (!string.IsNullOrEmpty(this.VSTestLogger))
            {
                allArgs.Add("--logger:" + this.VSTestLogger);
            }

            if (!string.IsNullOrEmpty(this.VSTestListTests))
            {
                allArgs.Add("--listTests");
            }

            if (!string.IsNullOrEmpty(this.VSTestDiag))
            {
                allArgs.Add("--Diag:" + this.AddDoubleQuotes(this.VSTestDiag));
            }

            if (string.IsNullOrEmpty(this.TestFileFullPath))
            {
                this.Log.LogError("Test file path cannot be empty or null.");
            }
            else
            {
                allArgs.Add(this.AddDoubleQuotes(this.TestFileFullPath));
            }

            // For Full CLR, add source directory as test adapter path.
            if (string.IsNullOrEmpty(this.VSTestTestAdapterPath))
            {
                if (this.VSTestFramework.StartsWith(".NETFramework", StringComparison.OrdinalIgnoreCase))
                {
                    allArgs.Add("--testAdapterPath:" + this.AddDoubleQuotes(Path.GetDirectoryName(this.TestFileFullPath)));
                }
            }

            return allArgs;
        }
    }
}
