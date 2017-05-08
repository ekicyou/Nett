﻿using System.Diagnostics.CodeAnalysis;
using System.IO;
using FluentAssertions;
using Nett.Coma.Tests.TestData;
using Nett.UnitTests.Util;
using Xunit;

namespace Nett.Coma.Tests
{
    [ExcludeFromCodeCoverage]
    public sealed class CreateTests : TestsBase
    {
        private const string FuncInitMergeConfig = "Init merge config";

        [Fact(DisplayName = "If config file doesn't exist yet, the config manager will create one with the provided defaults.")]
        public void ConfigManager_WhenFileDoesntExistYet_WillCreateItInitially()
        {
            string fileName = "autocreated".TestRunUniqueName(Toml.FileExtension);

            try
            {
                // Act
                const int ExpectedIntValue = 3;
                var cfg = new SingleLevelConfig() { IntValue = ExpectedIntValue };
                Config.Create(() => cfg, fileName);

                // Assert
                File.Exists(fileName).Should().Be(true);
                var read = Toml.ReadFile<SingleLevelConfig>(fileName);
                read.IntValue.Should().Be(ExpectedIntValue);
            }
            finally
            {
                TryDeleteFile(fileName);
            }
        }

        [FFact(FuncInitMergeConfig, "Create a file split config from a single object creates the two files and the are merged correctly")]
        public void Foo()
        {
            string mainFile = null;
            string userFile = null;

            try
            {
                // Arrange
                CreateMergedTestAppConfig(out mainFile, out userFile);

                // Act
                var merged = Config.Create(() => new TestData.TestAppSettings(), mainFile, userFile);

                // Assert
                merged.Get(c => c.BinDir).Should().Be(TestData.TestAppSettings.GlobalSettings.BinDir);
                merged.Get(c => c.User.UserName).Should().Be(TestData.TestAppSettings.User1Settings.UserName);
            }
            finally
            {
                TryDeleteFile(mainFile);
                TryDeleteFile(userFile);
            }
        }

        [Fact]
        public void SaveSetting_WhenItDoesNotExistYetInConfigFile_GetsCreatedAndSaved()
        {
            using (var scenario = SingleConfigFileScenario.Setup(nameof(SaveSetting_WhenItDoesNotExistYetInConfigFile_GetsCreatedAndSaved)))
            {
                // Arrange
                var cfg = Config.Create(
                    () => new SingleConfigFileScenario.ConfigContent(), ConfigSource.CreateFileSource(scenario.File));

                // Act
                cfg.Set(c => c.Sub.Z, 1);

                // Assert
                File.ReadAllText(scenario.File).Should().Be("Add expectation when exception is fixed.");
            }
        }

        [Fact]
        public void SaveSetting_WhenMovedBetweenConfigScopes_SavesThatSettingCorrectly()
        {
            using (var scenario = GitScenario.Setup(nameof(SaveSetting_WhenMovedBetweenConfigScopes_SavesThatSettingCorrectly)))
            {
                // Arrange
                var cfg = scenario.CreateMergedFromDefaults();

                // Act
                cfg.Set(c => c.Core.AutoClrf, true, scenario.UserFileSource);

                // Assert
                File.ReadAllText(scenario.UserFile).Should().Be("Add expectation when exception is fixed.");
            }
        }
    }
}
