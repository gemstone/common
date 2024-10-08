﻿//******************************************************************************************************
//  PasswordGenerator.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  10/27/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Gemstone.Collections.CollectionExtensions;

namespace Gemstone.Security.Cryptography;

/// <summary>
/// Generates random passwords.
/// </summary>
public class PasswordGenerator
{
    #region [ Constructors ]

    /// <summary>
    /// Creates a new instance of the <see cref="PasswordGenerator"/> class.
    /// </summary>
    public PasswordGenerator() : this(DefaultCharacterGroups)
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="PasswordGenerator"/> class.
    /// </summary>
    /// <param name="characterGroups">The list of character groups used to generate passwords.</param>
    public PasswordGenerator(IEnumerable<CharacterGroup> characterGroups)
    {
        CharacterGroups = new List<CharacterGroup>(characterGroups);
    }

    #endregion

    #region [ Properties ]

    /// <summary>
    /// Gets the collection of character groups used by this <see cref="PasswordGenerator"/>.
    /// </summary>
    public List<CharacterGroup> CharacterGroups { get; }

    /// <summary>
    /// Gets a string representing the total collection
    /// of characters across all character groups.
    /// </summary>
    public string AllCharacters
    {
        get
        {
            return new string(CharacterGroups
                .SelectMany(characterGroup => characterGroup.Characters)
                .ToArray());
        }
    }

    /// <summary>
    /// Gets the absolute minimum length of password that can be
    /// generated by the <see cref="PasswordGenerator"/> based
    /// on the minimum occurrence of each character group.
    /// </summary>
    public int MinLength
    {
        get
        {
            return CharacterGroups.Sum(characterGroup => characterGroup.MinOccurrence);
        }
    }

    #endregion

    #region [ Methods ]

    /// <summary>
    /// Generates a random password with 8 characters or
    /// <see cref="MinLength"/> if it is greater than 8 characters.
    /// </summary>
    /// <returns>A randomly generated password.</returns>
    public string GeneratePassword()
    {
        return GeneratePassword(Math.Max(8, MinLength));
    }

    /// <summary>
    /// Generates a password with length between the given
    /// <paramref name="minLength"/> and <paramref name="maxLength"/>.
    /// </summary>
    /// <param name="minLength">The minimum length of the generated password.</param>
    /// <param name="maxLength">The maximum length of the generated password.</param>
    /// <returns>A randomly generated password.</returns>
    public string GeneratePassword(int minLength, int maxLength)
    {
        if (minLength > maxLength)
            throw new ArgumentException("minLength must be less than or equal to maxLength");

        return GeneratePassword(Random.Int32Between(minLength, maxLength + 1));
    }

    /// <summary>
    /// Generates a password with the given length.
    /// </summary>
    /// <param name="length">The length of the password to be generated.</param>
    /// <returns>A randomly generated password.</returns>
    public string GeneratePassword(int length)
    {
        if (length < MinLength)
            throw new ArgumentOutOfRangeException(nameof(length), $"The length of the generated password must be at least {MinLength} characters.");

        // Create a collection of character groups the size of the password to be generated.
        // The number of times a character group appears in this collection is equal to the
        // minimum number of occurrences of that character group. An AllCharacters group is
        // created to fill in the characters not accounted for by the other character groups.
        List<CharacterGroup> characterGroups = CharacterGroups
            .SelectMany(characterGroup => Enumerable.Repeat(characterGroup, characterGroup.MinOccurrence))
            .Concat(Enumerable.Repeat(new CharacterGroup(AllCharacters), length - MinLength))
            .ToList();

        char[] passwordChars = new char[length];

        for (int i = 0; i < length; i++)
        {
            // Pull a random character from the character group into the password
            CharacterGroup characterGroup = characterGroups[i];
            int index = Random.Int32Between(0, characterGroup.Characters.Length);
            passwordChars[i] = characterGroup.Characters[index];
        }

        // Scramble the generated set of characters
        passwordChars.Scramble();

        return new string(passwordChars);
    }

    #endregion

    #region [ Static ]

    // Static Fields

    /// <summary>
    /// Default set of character groups used by the <see cref="PasswordGenerator"/>.
    /// </summary>
    public static readonly IReadOnlyList<CharacterGroup> DefaultCharacterGroups = new List<CharacterGroup>
    {
        new() { Characters = "abcdefghijklmnopqrstuvwxyz", MinOccurrence = 1 },
        new() { Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", MinOccurrence = 1 },
        new() { Characters = "0123456789", MinOccurrence = 1 },
        new() { Characters = "~!@#$%^&*-_+:,.?", MinOccurrence = 1 }
    };

    /// <summary>
    /// Defines the default password generator.
    /// </summary>
    public static readonly PasswordGenerator Default = new();

    #endregion
}
