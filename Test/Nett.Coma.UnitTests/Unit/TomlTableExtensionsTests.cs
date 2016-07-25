﻿using System;
using Nett.UnitTests.Util;

namespace Nett.Coma.Tests.Unit
{
    public sealed class TomlTableExtensionsTests
    {
        [MFact(nameof(TomlTableExtensions), nameof(TomlTableExtensions.OverwriteWithValuesForSaveFrom),
            "When target table doesn't contain any row, rows of source table will not be added to target.",
            Skip = TestingConstants.AddTestWhenFeatureWorks)]
        public void OverwriteForSaveFrom_WhenTargetTableDoesntContainRow_RowWillNotBeAddedToTargetTable()
        {
            throw new NotImplementedException();
        }
    }
}