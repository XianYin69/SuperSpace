// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CommandPalette.Extensions;
using Microsoft.CommandPalette.Extensions.Toolkit;
using OfficePlus.Pages;

namespace OfficePlus;

public partial class OfficePlusCommandsProvider : CommandProvider
{
    private readonly ICommandItem[] _commands;

    public OfficePlusCommandsProvider()
    {
        DisplayName = "OfficePlus";
        Icon = IconHelpers.FromRelativePath("Assets\\StoreLogo.png");
        _commands = new ICommandItem[]
        {
            new CommandItem(new OfficePlusPage()) { Title = DisplayName },
        };
    }

    public override ICommandItem[] TopLevelCommands()
    {
        return _commands;
    }

}
