//******************************************************************************************************
//  FilePath.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  02/05/2003 - J. Ritchie Carroll
//       Generated original version of source code.
//  12/29/2005 - Pinal C. Patel
//       Migrated 2.0 version of source code from 1.1 source (GSF.Shared.FilePath).
//  08/22/2007 - Darrell Zuercher
//       Edited code comments.
//  09/19/2008 - J. Ritchie Carroll
//       Converted to C#.
//  10/24/2008 - Pinal C. Patel
//       Edited code comments.
//  12/17/2008 - F. Russell Robertson
//       Fixed issue in GetFilePatternRegularExpression().
//  06/30/2009 - Pinal C. Patel
//       Removed FilePathHasFileName() since the result was error prone.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  09/17/2009 - Pinal C. Patel
//       Modified GetAbsolutePath() to remove dependency on HttpContext.Current.
//  04/19/2010 - Pinal C. Patel
//       Added GetApplicationDataFolder() method.
//  04/21/2010 - Pinal C. Patel
//       Updated GetApplicationDataFolder() to include the company name if available.
//  01/28/2011 - J. Ritchie Carroll
//       Added IsValidFileName function.
//  02/14/2011 - J. Ritchie Carroll
//       Fixed issue in GetDirectoryName where last directory was being truncated as a file name.
//  06/06/2011 - Stephen C. Wills
//       Fixed issue in GetFileName where path suffix was being removed before extracting the file name.
//  07/29/2011 - Pinal C. Patel
//       Updated GetApplicationDataFolder() to use the TEMP directory for web applications.
//  12/14/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Gemstone.CharExtensions;
using Gemstone.Console;
using Gemstone.Interop;
using Gemstone.Reflection;
using Gemstone.StringExtensions;
using Gemstone.Units;
using static Gemstone.Common;
using static Gemstone.Interop.WindowsApi;

namespace Gemstone.IO;

/// <summary>
/// Contains File and Path manipulation methods.
/// </summary>
public static class FilePath
{
    #region [ Members ]

    // Fields
    private static readonly string s_directorySeparatorCharPattern;
    private static readonly string s_fileNameCharPattern;
    private static AssemblyInfo? s_hostAssemblyInfo;
    private static string? s_hostApplicationPath;

    #endregion

    #region [ Constructor ]

    static FilePath()
    {
        char[] directorySeparatorChars =
        {
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar
        };

        char[] encodedInvalidFileNameChars = Path.GetInvalidFileNameChars()
            .SelectMany(c => c.RegexEncode())
            .ToArray();

        char[] encodedDirectorySeparatorChars = directorySeparatorChars
            .Distinct()
            .SelectMany(c => c.RegexEncode())
            .ToArray();

        // Defines a regular expression pattern for a valid directory separator character.
        s_directorySeparatorCharPattern = $"[{new string(encodedDirectorySeparatorChars)}]";

        // Defines a regular expression pattern for a valid file name character. We do this by
        // allowing any characters except those that would not be valid as part of a filename.
        // This essentially builds the "?" wild-card pattern match.
        s_fileNameCharPattern = $"[^{new string(encodedInvalidFileNameChars)}]";
    }

    #endregion

    #region [ Properties ]

    /// <summary>
    /// Connects to a network share with the specified user's credentials.
    /// </summary>
    /// <param name="shareName">UNC share name to connect to.</param>
    /// <param name="userName">Username to use for connection.</param>
    /// <param name="password">Password to use for connection.</param>
    /// <param name="domain">Domain name to use for connection. Specify the computer name for local system accounts.</param>
    public static void ConnectToNetworkShare(string shareName, string userName, string password, string domain)
    {
        // TODO: Add #include <sys/mount.h> implementation for POSIX environment, see Gemstone.POSIX library
        if (IsPosixEnvironment)
            throw new NotImplementedException("Failed to connect to network share \"" + shareName + "\" as user " + userName + " - not implemented in POSIX environment");

        NETRESOURCE resource = new()
        {
            dwType = RESOURCETYPE_DISK, 
            lpRemoteName = shareName
        };

        if (domain.Length > 0)
            userName = domain + "\\" + userName;

        int result = WNetAddConnection2(ref resource, password, userName, 0);

        if (result != 0)
            throw new InvalidOperationException("Failed to connect to network share \"" + shareName + "\" as user " + userName + " - " + WindowsApi.GetErrorMessage(result));
    }

    /// <summary>
    /// Disconnects the specified network share.
    /// </summary>
    /// <param name="shareName">UNC share name to disconnect from.</param>
    public static void DisconnectFromNetworkShare(string shareName)
    {
        DisconnectFromNetworkShare(shareName, true);
    }

    /// <summary>
    /// Disconnects the specified network share.
    /// </summary>
    /// <param name="shareName">UNC share name to disconnect from.</param>
    /// <param name="force"><c>true</c> to force a disconnect; otherwise <c>false</c>.</param>
    public static void DisconnectFromNetworkShare(string shareName, bool force)
    {
        // TODO: Add #include <sys/mount.h> implementation for POSIX environment, see Gemstone.POSIX library
        if (IsPosixEnvironment)
            throw new NotImplementedException("Failed to disconnect from network share \"" + shareName + "\" - not implemented in POSIX environment");

        int result = WNetCancelConnection2(shareName, 0, force);

        if (result != 0)
            throw new InvalidOperationException("Failed to disconnect from network share \"" + shareName + "\" - " + WindowsApi.GetErrorMessage(result));
    }

    /// <summary>
    /// Gets the <see cref="AssemblyInfo"/> instance of the host application.
    /// </summary>
    /// <exception cref="NullReferenceException">Failed to derive host assembly info.</exception>
    public static AssemblyInfo HostAssemblyInfo
    {
        get
        {
            if (s_hostAssemblyInfo is not null)
                return s_hostAssemblyInfo;

            AssemblyInfo? hostAssemblyInfo = null;

            try
            {
                hostAssemblyInfo = AssemblyInfo.EntryAssembly;
            }
            catch (Exception ex)
            {
                LibraryEvents.OnSuppressedException(typeof(FilePath), new InvalidOperationException($"Failed to get entry assembly info: {ex.Message}", ex));
            }

            hostAssemblyInfo ??= AssemblyInfo.ExecutingAssembly;

            s_hostAssemblyInfo = hostAssemblyInfo ?? throw new NullReferenceException("Failed to derive host application assembly info");

            return s_hostAssemblyInfo;
        }
    }

    /// <summary>
    /// Gets the application path of the host application.
    /// </summary>
    /// <exception cref="NullReferenceException">Failed to derive host application path.</exception>
    public static string HostApplicationPath
    {
        get
        {
            if (s_hostApplicationPath is not null)
                return s_hostApplicationPath;

            string? hostFileName = null;
                    
            try
            {
                hostFileName = Process.GetCurrentProcess().MainModule?.FileName;
            }
            catch (Exception ex)
            {
                LibraryEvents.OnSuppressedException(typeof(FilePath), new InvalidOperationException($"Failed to get main module filename of current process: {ex.Message}", ex));
            }

            if (string.IsNullOrEmpty(hostFileName))
                hostFileName = HostAssemblyInfo.Location;

            if (string.IsNullOrEmpty(hostFileName))
                throw new NullReferenceException("Failed to derive host application path");

            s_hostApplicationPath = GetDirectoryName(hostFileName);

            return s_hostApplicationPath;
        }
    }

    #endregion

    #region [ Methods ]

    /// <summary>
    /// Tries to get the free space values for a given path. This path can be a network share or a mount point.
    /// </summary>
    /// <param name="pathName">The path to the location</param>
    /// <param name="freeSpace">The number of user space bytes</param>
    /// <param name="totalSize">The total number of bytes on the drive.</param>
    /// <returns><c>true</c> if successful; otherwise <c>false</c> if there was an error.</returns>
    public static bool GetAvailableFreeSpace(string pathName, out long freeSpace, out long totalSize)
    {
        try
        {
            if (IsPosixEnvironment)
            {
                string output = Command.Execute("df", $"-k {pathName}").StandardOutput;
                string[] lines = output.Split('\n');

                if (lines.Length > 1)
                {
                    string[] elems = lines[1].Trim().RemoveDuplicateWhiteSpace().Split(' ');

                    if (elems.Length > 4)
                    {
                        if (long.TryParse(elems[1], out long totalKilobytes) && long.TryParse(elems[3], out long availableKilobytes))
                        {
                            freeSpace = availableKilobytes * SI2.Kilo;
                            totalSize = totalKilobytes * SI2.Kilo;
                            return true;
                        }
                    }
                }

                freeSpace = 0L;
                totalSize = 0L;
                return false;
            }

            string fullPath = Path.GetFullPath(pathName);

            bool success = GetDiskFreeSpaceEx(fullPath, out ulong lpFreeBytesAvailable, out ulong lpTotalNumberOfBytes, out ulong _);

            freeSpace = (long)lpFreeBytesAvailable;
            totalSize = (long)lpTotalNumberOfBytes;

            return success;
        }
        catch
        {
            freeSpace = 0L;
            totalSize = 0L;
            return false;
        }
    }

    /// <summary>
    /// Determines if the specified <paramref name="filePath"/> is contained with the current executable path.
    /// </summary>
    /// <param name="filePath">File name or relative file path.</param>
    /// <returns><c>true</c> if the specified <paramref name="filePath"/> is contained with the current executable path; otherwise <c>false</c>.</returns>
    public static bool InApplicationPath(string filePath)
    {
        return GetAbsolutePath(filePath).StartsWith(GetAbsolutePath(""), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the path to the folder where data related to the current
    /// application can be stored as well as shared among different users.
    /// </summary>
    /// <returns>Path to the folder where data related to the current application can be stored.</returns>
    public static string GetCommonApplicationDataFolder()
    {
        string rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        AssemblyInfo assembly = HostAssemblyInfo;

        return string.IsNullOrEmpty(assembly.Company) ? Path.Combine(rootFolder, assembly.Name!) : Path.Combine(rootFolder, assembly.Company, assembly.Name!);
    }

    /// <summary>
    /// Gets the path to the folder where data related to the current application can be stored.
    /// </summary>
    /// <returns>Path to the folder where data related to the current application can be stored.</returns>
    public static string GetApplicationDataFolder()
    {
        string rootFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        AssemblyInfo assembly = HostAssemblyInfo;

        return string.IsNullOrEmpty(assembly.Company) ? Path.Combine(rootFolder, assembly.Name!) : Path.Combine(rootFolder, assembly.Company, assembly.Name!);
    }

    /// <summary>
    /// Gets the absolute file path for the specified file name or relative file path.
    /// </summary>
    /// <param name="filePath">File name or relative file path.</param>
    /// <returns>Absolute file path for the specified file name or relative file path.</returns>
    public static string GetAbsolutePath(string filePath)
    {
        if (!Path.IsPathRooted(filePath))
            filePath = Path.Combine(HostApplicationPath, filePath);

        return RemovePathSuffix(filePath);
    }

    /// <summary>
    /// Gets a unique file path for the given file by checking for name collisions and
    /// adding a sequence number to the end of the file name if there is a collision.
    /// </summary>
    /// <param name="originalFilePath">The path to the original file before adding the sequence number.</param>
    /// <returns>The unique path to the file.</returns>
    /// <remarks>
    /// This method is designed to handle the case where the user wishes to create a file in a folder
    /// with a given name when there is a possibility that the name is already taken. Using this method,
    /// it is possible to create files with names in the following format:
    /// 
    /// <ul>
    ///     <li>File.ext</li>
    ///     <li>File (1).ext</li>
    ///     <li>File (2).ext</li>
    ///     <li>...</li>
    /// </ul>
    /// 
    /// This method uses a linear search to find a unique file name, so it is suitable for situations where
    /// there are a small number of collisions for each file name. This will detect and fill gaps that can
    /// occur when files are deleted (for instance, if "File (1).ext" were deleted from the list above).
    /// </remarks>
    public static string GetUniqueFilePath(string originalFilePath)
    {
        string uniqueFilePath = GetAbsolutePath(originalFilePath);
        string directory = GetDirectoryName(uniqueFilePath);
        string originalFileRoot = GetFileNameWithoutExtension(uniqueFilePath);
        string fileExtension = GetExtension(uniqueFilePath);
        int i = 1;

        while (File.Exists(uniqueFilePath))
        {
            uniqueFilePath = Path.Combine(directory, $"{originalFileRoot} ({i}){fileExtension}");
            i++;
        }

        return uniqueFilePath;
    }

    /// <summary>
    /// Gets a unique file path for the given file by checking for name collisions and
    /// adding a sequence number to the end of the file name if there is a collision.
    /// </summary>
    /// <param name="originalFilePath">The path to the original file before adding the sequence number.</param>
    /// <returns>The unique path to the file.</returns>
    /// <remarks>
    /// This method is designed to handle the case where the user wishes to create a file in a folder
    /// with a given name when there is a possibility that the name is already taken. Using this method,
    /// it is possible to create files with names in the following format:
    /// 
    /// <ul>
    ///     <li>File.ext</li>
    ///     <li>File (1).ext</li>
    ///     <li>File (2).ext</li>
    ///     <li>...</li>
    /// </ul>
    /// 
    /// This method uses a binary search to find a unique file name, so it is suitable for situations where
    /// a large number of files will be created with the same file name, and the next unique file name needs
    /// to be found relatively quickly. It will not always detect gaps in the sequence numbers that can occur
    /// when files are deleted (for instance, if "File (1).ext" were deleted from the list above).
    /// </remarks>
    public static string GetUniqueFilePathWithBinarySearch(string originalFilePath)
    {
        string uniqueFilePath = GetAbsolutePath(originalFilePath);
        string directory = GetDirectoryName(uniqueFilePath);
        string originalFileRoot = GetFileNameWithoutExtension(uniqueFilePath);
        string fileExtension = GetExtension(uniqueFilePath);

        int i = 1;
        int j = 1;
        int k = 1;

        if (!File.Exists(uniqueFilePath))
            return uniqueFilePath;

        while (File.Exists(uniqueFilePath))
        {
            uniqueFilePath = Path.Combine(directory, $"{originalFileRoot} ({i}){fileExtension}");
            j = k;
            k = i;
            i *= 2;
        }

        while (j < k)
        {
            i = (j + k) / 2;
            uniqueFilePath = Path.Combine(directory, $"{originalFileRoot} ({i}){fileExtension}");

            if (File.Exists(uniqueFilePath))
                j = i + 1;
            else
                k = i;
        }

        return Path.Combine(directory, $"{originalFileRoot} ({k}){fileExtension}");
    }

    /// <summary>
    /// Returns the names of the subdirectories (including their paths) that match the specified search pattern in the specified directory, and optionally searches subdirectories.
    /// </summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in <paramref name="path"/>. This parameter can contain a combination of valid literal and wild-card characters, but doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
    /// <param name="exceptionHandler">Handles exceptions thrown during directory enumeration.</param>
    /// <returns>An array of the full names (including paths) of the subdirectories that match the specified criteria, or an empty array if no directories are found.</returns>
    public static string[] GetDirectories(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories, Action<Exception>? exceptionHandler = null)
    {
        return EnumerateDirectories(path, searchPattern, searchOption, exceptionHandler).ToArray();
    }

    /// <summary>
    /// Returns an enumerable collection of subdirectories that match a search pattern in a specified path, and optionally searches subdirectories.
    /// </summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories.</param>
    /// <param name="exceptionHandler">Handles exceptions thrown during directory enumeration.</param>
    /// <returns>An enumerable collection of the full names (including paths) for the directories in the directory specified by <paramref name="path"/> and that match the specified search pattern and option.</returns>
    public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories, Action<Exception>? exceptionHandler = null)
    {
        IEnumerable<string> enumerable;
        IEnumerator<string> enumerator;

        void handleException(Exception ex)
        {
            InvalidOperationException enumerationEx = new($"Failed while enumerating directories in \"{path}\": {ex.Message}", ex);

            if (exceptionHandler is null)
                LibraryEvents.OnSuppressedException(typeof(FilePath), enumerationEx);
            else
                exceptionHandler(enumerationEx);
        }

        try
        {
            IEnumerable<string> topDirectory = Directory.EnumerateDirectories(path, searchPattern, SearchOption.TopDirectoryOnly);
            IEnumerable<string> recursive = Enumerable.Empty<string>();

            if (searchOption == SearchOption.AllDirectories)
            {
                recursive = Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly)
                    .SelectMany(directory => EnumerateDirectories(directory, searchPattern, searchOption, exceptionHandler));
            }

            enumerable = topDirectory.Concat(recursive);
            enumerator = enumerable.GetEnumerator();
        }
        catch (Exception ex)
        {
            handleException(ex);
            yield break;
        }

        // yield return cannot be used in a try block with a catch clause,
        // so in order to handle errors in enumerator.MoveNext() and enumerator.Current,
        // the enumerator must be accessed directly rather than using foreach
        using (enumerable as IDisposable)
        using (enumerator)
        {
            while (true)
            {
                string? current;

                try
                {
                    if (!enumerator.MoveNext())
                        break;

                    current = enumerator.Current;
                }
                catch (Exception ex)
                {
                    handleException(ex);

                    // To avoid an infinite exception loop,
                    // break out at the first sign of trouble
                    break;
                }

                if (current is not null)
                    yield return current;
            }
        }
    }

    /// <summary>
    /// Returns the names of files (including their paths) that match the specified search pattern in the specified directory, using a value to determine whether to search subdirectories.
    /// </summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wild-card (* and ?) characters, but doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
    /// <param name="exceptionHandler">Handles exceptions thrown during file enumeration.</param>
    /// <returns>An array of the full names (including paths) for the files in the specified directory that match the specified search pattern and option, or an empty array if no files are found.</returns>
    public static string[] GetFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories, Action<Exception>? exceptionHandler = null)
    {
        return EnumerateFiles(path, searchPattern, searchOption, exceptionHandler).ToArray();
    }

    /// <summary>
    /// Returns an enumerable collection of file names that match a search pattern in a specified path, and optionally searches subdirectories.
    /// </summary>
    /// <param name="path">The relative or absolute path to the directory to search. This string is not case-sensitive.</param>
    /// <param name="searchPattern">The search string to match against the names of files in <paramref name="path"/>. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories.</param>
    /// <param name="exceptionHandler">Handles exceptions thrown during file enumeration.</param>
    /// <returns>An enumerable collection of the full names (including paths) for the files in the directory specified by <paramref name="path"/> and that match the specified search pattern and option.</returns>
    public static IEnumerable<string> EnumerateFiles(string path, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories, Action<Exception>? exceptionHandler = null)
    {
        IEnumerable<string> enumerable;
        IEnumerator<string> enumerator;

        void handleException(Exception ex)
        {
            InvalidOperationException enumerationEx = new($"Failed while enumerating files in \"{path}\": {ex.Message}", ex);

            if (exceptionHandler is null)
                LibraryEvents.OnSuppressedException(typeof(FilePath), enumerationEx);
            else
                exceptionHandler(enumerationEx);
        }

        try
        {
            IEnumerable<string> topDirectory = Directory.EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
            IEnumerable<string> recursive = Enumerable.Empty<string>();

            if (searchOption == SearchOption.AllDirectories)
            {
                recursive = Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly)
                    .SelectMany(directory => EnumerateFiles(directory, searchPattern, searchOption, exceptionHandler));
            }

            enumerable = topDirectory.Concat(recursive);
            enumerator = enumerable.GetEnumerator();
        }
        catch (Exception ex)
        {
            handleException(ex);
            yield break;
        }

        // yield return cannot be used in a try block with a catch clause,
        // so in order to handle errors in enumerator.MoveNext() and enumerator.Current,
        // the enumerator must be accessed directly rather than using foreach
        using (enumerable as IDisposable)
        using (enumerator)
        {
            while (true)
            {
                string? current;

                try
                {
                    if (!enumerator.MoveNext())
                        break;

                    current = enumerator.Current;
                }
                catch (Exception ex)
                {
                    handleException(ex);

                    // To avoid an infinite exception loop,
                    // break out at the first sign of trouble
                    break;
                }

                if (current is not null)
                    yield return current;
            }
        }
    }

    /// <summary>
    /// Returns the subdirectories that match the specified search pattern in the specified directory, and optionally searches subdirectories.
    /// </summary>
    /// <param name="directory">The directory to search.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in the <paramref name="directory"/>. This parameter can contain a combination of valid literal and wildcard characters, but doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
    /// <param name="exceptionHandler">Handles exceptions thrown during directory enumeration.</param>
    /// <returns>An array of the <see cref="DirectoryInfo"/> objects representing the subdirectories that match the specified search criteria.</returns>
    public static DirectoryInfo[] GetDirectories(DirectoryInfo directory, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories, Action<Exception>? exceptionHandler = null)
    {
        return EnumerateDirectories(directory, searchPattern, searchOption, exceptionHandler).ToArray();
    }

    /// <summary>
    /// Returns an enumerable collection of subdirectories that match a search pattern in a specified path, and optionally searches subdirectories.
    /// </summary>
    /// <param name="directory">The directory to search.</param>
    /// <param name="searchPattern">The search string to match against the names of subdirectories in the <paramref name="directory"/>. This parameter can contain a combination of valid literal and wildcard characters, but doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories.</param>
    /// <param name="exceptionHandler">Handles exceptions thrown during directory enumeration.</param>
    /// <returns>An enumerable collection of the <see cref="DirectoryInfo"/> objects representing the subdirectories that match the specified search criteria.</returns>
    public static IEnumerable<DirectoryInfo> EnumerateDirectories(DirectoryInfo directory, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories, Action<Exception>? exceptionHandler = null)
    {
        IEnumerable<DirectoryInfo> enumerable;
        IEnumerator<DirectoryInfo> enumerator;

        try
        {
            IEnumerable<DirectoryInfo> topDirectory = directory.EnumerateDirectories(searchPattern, SearchOption.TopDirectoryOnly);
            IEnumerable<DirectoryInfo> recursive = Enumerable.Empty<DirectoryInfo>();

            if (searchOption == SearchOption.AllDirectories)
            {
                recursive = directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
                    .SelectMany(subdirectory => EnumerateDirectories(subdirectory, searchPattern, searchOption, exceptionHandler));
            }

            enumerable = topDirectory.Concat(recursive);
            enumerator = enumerable.GetEnumerator();
        }
        catch (Exception ex)
        {
            string path = directory.FullName;
            exceptionHandler?.Invoke(new InvalidOperationException($"Failed while enumerating directories in \"{path}\": {ex.Message}", ex));
            yield break;
        }

        // yield return cannot be used in a try block with a catch clause,
        // so in order to handle errors in enumerator.MoveNext() and enumerator.Current,
        // the enumerator must be accessed directly rather than using foreach
        using (enumerable as IDisposable)
        using (enumerator)
        {
            while (true)
            {
                DirectoryInfo? current;

                try
                {
                    if (!enumerator.MoveNext())
                        break;

                    current = enumerator.Current;
                }
                catch (Exception ex)
                {
                    string path = directory.FullName;
                    exceptionHandler?.Invoke(new InvalidOperationException($"Failed while enumerating directories in \"{path}\": {ex.Message}", ex));

                    // To avoid an infinite exception loop,
                    // break out at the first sign of trouble
                    break;
                }

                if (current is not null)
                    yield return current;
            }
        }
    }

    /// <summary>
    /// Returns the files that match the specified search pattern in the specified directory, using a value to determine whether to search subdirectories.
    /// </summary>
    /// <param name="directory">The directory to search.</param>
    /// <param name="searchPattern">The search string to match against the names of files in the <paramref name="directory"/>. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include all subdirectories or only the current directory.</param>
    /// <param name="exceptionHandler">Handles exceptions thrown during file enumeration.</param>
    /// <returns>An array of the <see cref="FileInfo"/> objects representing the files in the specified directory that match the specified search pattern, or an empty array if no files are found.</returns>
    public static FileInfo[] GetFiles(DirectoryInfo directory, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories, Action<Exception>? exceptionHandler = null)
    {
        return EnumerateFiles(directory, searchPattern, searchOption, exceptionHandler).ToArray();
    }

    /// <summary>
    /// Returns an enumerable collection of files that match a search pattern in a specified path, and optionally searches subdirectories.
    /// </summary>
    /// <param name="directory">The directory to search.</param>
    /// <param name="searchPattern">The search string to match against the names of files in the <paramref name="directory"/>. This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but doesn't support regular expressions.</param>
    /// <param name="searchOption">One of the enumeration values that specifies whether the search operation should include only the current directory or should include all subdirectories.</param>
    /// <param name="exceptionHandler">Handles exceptions thrown during file enumeration.</param>
    /// <returns>An enumerable collection of the <see cref="FileInfo"/> objects representing the files in the specified directory that match the specified search pattern, or an empty array if no files are found.</returns>
    public static IEnumerable<FileInfo> EnumerateFiles(DirectoryInfo directory, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories, Action<Exception>? exceptionHandler = null)
    {
        IEnumerable<FileInfo> enumerable;
        IEnumerator<FileInfo> enumerator;

        try
        {
            IEnumerable<FileInfo> topDirectory = directory.EnumerateFiles(searchPattern, SearchOption.TopDirectoryOnly);
            IEnumerable<FileInfo> recursive = Enumerable.Empty<FileInfo>();

            if (searchOption == SearchOption.AllDirectories)
            {
                recursive = directory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly)
                    .SelectMany(subdirectory => EnumerateFiles(subdirectory, searchPattern, searchOption, exceptionHandler));
            }

            enumerable = topDirectory.Concat(recursive);
            enumerator = enumerable.GetEnumerator();
        }
        catch (Exception ex)
        {
            string path = directory.FullName;
            exceptionHandler?.Invoke(new InvalidOperationException($"Failed while enumerating files in \"{path}\": {ex.Message}", ex));
            yield break;
        }

        // yield return cannot be used in a try block with a catch clause,
        // so in order to handle errors in enumerator.MoveNext() and enumerator.Current,
        // the enumerator must be accessed directly rather than using foreach
        using (enumerable as IDisposable)
        using (enumerator)
        {
            while (true)
            {
                FileInfo? current;

                try
                {
                    if (!enumerator.MoveNext())
                        break;

                    current = enumerator.Current;
                }
                catch (Exception ex)
                {
                    string path = directory.FullName;
                    exceptionHandler?.Invoke(new InvalidOperationException($"Failed while enumerating files in \"{path}\": {ex.Message}", ex));

                    // To avoid an infinite exception loop,
                    // break out at the first sign of trouble
                    break;
                }

                if (current is not null)
                    yield return current;
            }
        }
    }

    /// <summary>
    /// Ensures the supplied path name is valid.
    /// </summary>
    /// <param name="filePath">The file path to be validated.</param>
    /// <remarks>
    /// Throws argument exceptions if the <see param="pathName"/> is invalid.
    /// </remarks>
    public static void ValidatePathName(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentNullException(nameof(filePath), "Path cannot be null or empty space");

        if (filePath.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            throw new ArgumentException("Path has invalid characters.", nameof(filePath));
    }

    /// <summary>
    /// Determines whether the specified file name matches any of the given file specs (wild-cards are defined as '*' or '?' characters).
    /// </summary>
    /// <param name="fileSpecs">The file specs used for matching the specified file name.</param>
    /// <param name="filePath">The file path to be tested against the specified file specs for a match.</param>
    /// <param name="ignoreCase"><c>true</c> to specify a case-insensitive match; otherwise <c>false</c>.</param>
    /// <returns><c>true</c> if the specified file name matches any of the given file specs; otherwise <c>false</c>.</returns>
    /// <exception cref="ArgumentException"><paramref name="filePath"/> contains one or more of the invalid characters defined in <see cref="Path.GetInvalidPathChars"/>.</exception>
    /// <remarks>
    /// The syntax for <paramref name="fileSpecs"/> adheres to the following rules:
    /// 
    /// <ul>
    /// <li>Either '\' or '/' (as defined by <see cref="Path.DirectorySeparatorChar"/> and <see cref="Path.AltDirectorySeparatorChar"/>) can match the other.</li>
    /// <li>A single '\' or '/' at the beginning of the pattern matches any valid path root (such as "C:\" or "\\server\share").</li>
    /// <li>A '?' matches a single character which would be valid in a file name (as defined by <see cref="Path.GetInvalidFileNameChars"/>).</li>
    /// <li>A '*' matches any number of characters which would be valid in a file name.</li>
    /// <li>A sequence of "**\" or "**/" matches any number of sequential directories.</li>
    /// <li>Any other character matches itself.</li>
    /// </ul>
    /// </remarks>
    public static bool IsFilePatternMatch(string[] fileSpecs, string filePath, bool ignoreCase)
    {
        return fileSpecs.Any(fileSpec => IsFilePatternMatch(fileSpec, filePath, ignoreCase));
    }

    /// <summary>
    /// Determines whether the specified file name matches the given file spec (wild-cards are defined as '*' or '?' characters).
    /// </summary>
    /// <param name="fileSpec">The file spec used for matching the specified file name.</param>
    /// <param name="filePath">The file path to be tested against the specified file spec for a match.</param>
    /// <param name="ignoreCase"><c>true</c> to specify a case-insensitive match; otherwise <c>false</c>.</param>
    /// <returns><c>true</c> if the specified file name matches the given file spec; otherwise <c>false</c>.</returns>
    /// <exception cref="ArgumentException"><paramref name="filePath"/> contains one or more of the invalid characters defined in <see cref="Path.GetInvalidPathChars"/>.</exception>
    /// <remarks>
    /// The syntax for <paramref name="fileSpec"/> adheres to the following rules:
    /// 
    /// <ul>
    /// <li>Either '\' or '/' (as defined by <see cref="Path.DirectorySeparatorChar"/> and <see cref="Path.AltDirectorySeparatorChar"/>) can match the other.</li>
    /// <li>A single '\' or '/' at the beginning of the pattern matches any valid path root (such as "C:\" or "\\server\share").</li>
    /// <li>A '?' matches a single character which would be valid in a file name (as defined by <see cref="Path.GetInvalidFileNameChars"/>).</li>
    /// <li>A '*' matches any number of characters which would be valid in a file name.</li>
    /// <li>A sequence of "**\" or "**/" matches any number of sequential directories.</li>
    /// <li>Any other character matches itself.</li>
    /// </ul>
    /// </remarks>
    public static bool IsFilePatternMatch(string fileSpec, string filePath, bool ignoreCase)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        // Define regular expression patterns for the three possible sequences that can match the path root.
        string recursiveDirPattern = $"{Regex.Escape("**")}{s_directorySeparatorCharPattern}";
        string pathRootPattern = string.Format("({0}[^{0}]|{0}$)", Regex.Escape(Path.DirectorySeparatorChar.ToString()));
        string altPathRootPattern = string.Format("({0}[^{0}]|{0}$)", Regex.Escape(Path.AltDirectorySeparatorChar.ToString()));

        // If any of the three patterns are found at the start of fileSpec and the fileName refers to a rooted path,
        // remove the path root from fileName and then remove all leading directory separator chars from both fileSpec and fileName.
        if (Regex.IsMatch(fileSpec, $"^({recursiveDirPattern}|{pathRootPattern}|{altPathRootPattern})") && Path.IsPathRooted(filePath))
        {
            fileSpec = fileSpec.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            filePath = Regex.Replace(filePath, $"^{Regex.Escape(Path.GetPathRoot(filePath) ?? string.Empty)}", "").TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        // Use regular expression matching to determine whether fileSpec matches fileName.
        return new Regex(GetFilePatternRegularExpression(fileSpec), ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None).IsMatch(filePath);
    }

    /// <summary>
    /// Determines if the specified file name and path is valid.
    /// </summary>
    /// <param name="filePath">The file name with optional path to test for validity.</param>
    /// <returns><c>true</c> if the specified <paramref name="filePath"/> is a valid name; otherwise <c>false</c>.</returns>
    public static bool IsValidFileName(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return false;

        try
        {
            // Attempt to parse directory and file name - this will catch most issues
            string directory = Path.GetDirectoryName(filePath) ?? string.Empty;
            string filename = Path.GetFileName(filePath);

            // Check for invalid directory characters
            if (!string.IsNullOrWhiteSpace(directory) && directory.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
                return false;

            // Check for invalid file name characters
            if (string.IsNullOrWhiteSpace(filename) || filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return false;

            // Recurse in to check validity of each directory name
            if (!string.IsNullOrWhiteSpace(directory) && !string.IsNullOrWhiteSpace(Path.GetFileName(directory)))
                return IsValidFileName(directory);
        }
        catch
        {
            // If you can't parse a file name or directory, file path is definitely not valid
            return false;
        }

        return true;
    }

    /// <summary>
    /// Gets a valid file name by replacing invalid file name characters with <paramref name="replaceWithCharacter"/>.
    /// </summary>
    /// <param name="fileName">File name to validate.</param>
    /// <param name="replaceWithCharacter">Character to replace invalid file name characters with. Set to '\0' to remove invalid file name characters.</param>
    /// <returns>A valid file name with no invalid file name characters.</returns>
    /// <remarks>
    /// This function expects a file name, not a file name with a path. To properly get a valid file path, where all directory
    /// names and the file name are validated, use the <see cref="GetValidFilePath"/>. Calling the <see cref="GetValidFileName"/>
    /// function will a full path will yield all directory separators replaced with the <paramref name="replaceWithCharacter"/>.
    /// </remarks>
    public static string GetValidFileName(string fileName, char replaceWithCharacter = '_') => replaceWithCharacter == '\0' ? fileName.RemoveInvalidFileNameCharacters() : fileName.ReplaceInvalidFileNameCharacters(replaceWithCharacter);

    /// <summary>
    /// Gets a valid file path by replacing invalid file or directory name characters with <paramref name="replaceWithCharacter"/>.
    /// </summary>
    /// <param name="filePath">File path to validate.</param>
    /// <param name="replaceWithCharacter">Character to replace invalid file or directory name characters with. Set to '\0' to remove invalid file or directory name characters.</param>
    /// <returns>A valid file path with no invalid file or directory name characters.</returns>
    public static string GetValidFilePath(string filePath, char replaceWithCharacter = '_')
    {
        string[] fileParts = filePath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        for (int i = 0; i < fileParts.Length; i++)
        {
            // Leave any volume specification alone
            if (i == 0 &&
                Path.VolumeSeparatorChar != Path.DirectorySeparatorChar &&
                Path.VolumeSeparatorChar != Path.AltDirectorySeparatorChar &&
                fileParts[0].IndexOfAny(new[] { Path.VolumeSeparatorChar }) > 0)
            {
                continue;
            }

            fileParts[i] = GetValidFileName(fileParts[i], replaceWithCharacter);
        }

        return string.Join(Path.DirectorySeparatorChar.ToString(), fileParts);
    }

    /// <summary>
    /// Gets the file name and extension from the specified file path.
    /// </summary>
    /// <param name="filePath">The file path from which the file name and extension is to be obtained.</param>
    /// <returns>File name and extension if the file path has it; otherwise empty string.</returns>
    public static string GetFileName(string filePath) => Path.GetFileName(filePath);

    /// <summary>
    /// Gets the extension from the specified file path.
    /// </summary>
    /// <param name="filePath">The file path from which the extension is to be obtained.</param>
    /// <returns>File extension.</returns>
    public static string GetExtension(string filePath) => Path.GetExtension(RemovePathSuffix(filePath));

    /// <summary>
    /// Gets the file name without extension from the specified file path.
    /// </summary>
    /// <param name="filePath">The file path from which the file name is to be obtained.</param>
    /// <returns>File name without the extension if the file path has it; otherwise empty string.</returns>
    public static string GetFileNameWithoutExtension(string filePath) => Path.GetFileNameWithoutExtension(RemovePathSuffix(filePath));

    /// <summary>
    /// Gets the size of the specified file.
    /// </summary>
    /// <param name="fileName">Name of file whose size is to be retrieved.</param>
    /// <returns>The size of the specified file.</returns>
    public static long GetFileLength(string fileName)
    {
        try
        {
            return new FileInfo(fileName).Length;
        }
        catch
        {
            return -1;
        }
    }

    /// <summary>
    /// Gets a list of files under the specified path. Search wild card pattern (c:\Data\*.dat) can be used for 
    /// including only the files matching the pattern or path wild-card pattern (c:\Data\*\*.dat) to indicate the 
    /// inclusion of files under all subdirectories in the list.
    /// </summary>
    /// <param name="path">The path for which a list of files is to be returned.</param>
    /// <param name="exceptionHandler">Handles exceptions thrown during file enumeration.</param>
    /// <returns>A list of files under the given path.</returns>
    public static string[] GetFileList(string path, Action<Exception>? exceptionHandler = null)
    {
        string directory = GetDirectoryName(path);
        string filePattern = GetFileName(path);
        SearchOption options = SearchOption.TopDirectoryOnly;

        // No wild-card pattern was specified, so get a listing of all files.
        if (string.IsNullOrEmpty(filePattern))
            filePattern = "*.*";

        if (GetLastDirectoryName(directory) == "*")
        {
            // Path wild-card pattern is used to specify the option to include subdirectories.
            options = SearchOption.AllDirectories;
            directory = directory.Remove(directory.LastIndexOf("*", StringComparison.OrdinalIgnoreCase));
        }

        if (exceptionHandler is null)
            return Directory.GetFiles(directory, filePattern, options);

        return GetFiles(directory, filePattern, options, exceptionHandler);
    }

    /// <summary>
    /// Gets a regular expression pattern that simulates wild-card matching for filenames (wild-cards are defined as '*' or '?' characters).
    /// </summary>
    /// <param name="fileSpec">The file spec for which the regular expression pattern if to be generated.</param>
    /// <returns>Regular expression pattern that simulates wild-card matching for filenames.</returns>
    public static string GetFilePatternRegularExpression(string fileSpec)
    {
        List<Tuple<string, string>> replacements = new()
        {
            // Replaces directory separator characters with their equivalent regular expressions.
            Tuple.Create($"{s_directorySeparatorCharPattern}+", $"{s_directorySeparatorCharPattern}+"),
                
            // Replaces wild-card file patterns with their equivalent regular expression.
            Tuple.Create(Regex.Escape("?"), s_fileNameCharPattern),
            Tuple.Create(Regex.Escape("**") + s_directorySeparatorCharPattern, $"({s_fileNameCharPattern}*{s_directorySeparatorCharPattern})*"),
            Tuple.Create(Regex.Escape("*"), $"{s_fileNameCharPattern}*")
        };

        StringBuilder input = new(fileSpec);
        StringBuilder output = new();

        while (input.Length > 0)
        {
            // Determine if any of the replacement patterns match the input.
            Tuple<string, string>? replacement = replacements.FirstOrDefault(r => Regex.IsMatch(input.ToString(), $"^{r.Item1}"));

            if (replacement is null)
            {
                // If not, move a single character as-is from input to the output.
                output.Append(Regex.Escape(input[0].ToString()));
                input.Remove(0, 1);
            }
            else
            {
                // If one of the replacement patterns matches the input,
                // apply that replacement and write it to the output.
                Match match = Regex.Match(input.ToString(), $"^{replacement.Item1}");
                output.Append(Regex.Replace(match.Value, replacement.Item1, replacement.Item2));
                input.Remove(0, match.Length);
            }
        }

        // Return the output string as the regular expression pattern.
        return $"^{output}$";
    }

    /// <summary>
    /// Gets the directory information from the specified file path.
    /// </summary>
    /// <param name="filePath">The file path from which the directory information is to be obtained.</param>
    /// <returns>Directory information.</returns>
    /// <remarks>
    /// This differs from <see cref="Path.GetDirectoryName(string)"/> in that it will see if last name in path is
    /// a directory and, if it exists, will properly treat that part of path as a directory. The .NET path
    /// function always assumes last entry is a file name if path is not suffixed with a slash. For example:
    ///     Path.GetDirectoryName(@"C:\Music") will return "C:\", however, 
    /// FilePath.GetDirectoryName(@"C:\Music") will return "C:\Music\", so long as Music directory exists.
    /// </remarks>
    public static string GetDirectoryName(string filePath)
    {
        // Test for case where valid path does not end in directory separator, Path.GetDirectoryName assumes
        // this is a file name - whether is exists or not :-(
        string directoryName = AddPathSuffix(filePath);

        return Directory.Exists(directoryName) ? directoryName : AddPathSuffix(Path.GetDirectoryName(filePath) ?? filePath);
    }

    /// <summary>
    /// Gets the last directory name from a file path.
    /// </summary>
    /// <param name="filePath">The file path from where the last directory name is to be retrieved.</param>
    /// <returns>The last directory name from a file path.</returns>
    /// <remarks>
    /// <see cref="GetLastDirectoryName(string)"/> would return sub2 from c:\windows\sub2\filename.ext.
    /// </remarks>
    public static string GetLastDirectoryName(string filePath)
    {
        // Test case should verify the following:
        //   FilePath.GetLastDirectoryName(@"C:\Test\sub") == "Test" <-- sub assumed to be filename
        //   FilePath.GetLastDirectoryName(@"C:\Test\sub\") == "sub" <-- sub assumed to be directory

        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentNullException(nameof(filePath));

        int index;
        char[] dirVolChars = { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar, Path.VolumeSeparatorChar };

        // Remove file name and trailing directory separator character from the file path.
        filePath = RemovePathSuffix(GetDirectoryName(filePath));

        // Keep going through the file path until all directory separator characters are removed.
        while ((index = filePath.IndexOfAny(dirVolChars)) > -1)
            filePath = filePath[(index + 1)..];

        return filePath;
    }

    /// <summary>
    /// Makes sure path is suffixed with standard <see cref="Path.DirectorySeparatorChar"/>.
    /// </summary>
    /// <param name="filePath">The file path to be suffixed.</param>
    /// <returns>Suffixed path.</returns>
    public static string AddPathSuffix(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = "." + Path.DirectorySeparatorChar;
        }
        else
        {
            char suffixChar = filePath[^1];

            if (suffixChar != Path.DirectorySeparatorChar && suffixChar != Path.AltDirectorySeparatorChar)
                filePath += Path.DirectorySeparatorChar;
        }

        return filePath;
    }

    /// <summary>
    /// Makes sure path is not suffixed with <see cref="Path.DirectorySeparatorChar"/> or <see cref="Path.AltDirectorySeparatorChar"/>.
    /// </summary>
    /// <param name="filePath">The file path to be unsuffixed.</param>
    /// <returns>Unsuffixed path.</returns>
    public static string RemovePathSuffix(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            filePath = "";
        }
        else
        {
            char suffixChar = filePath[^1];

            while ((suffixChar == Path.DirectorySeparatorChar || suffixChar == Path.AltDirectorySeparatorChar) && filePath.Length > 0)
            {
                filePath = filePath[..^1];

                if (filePath.Length > 0)
                    suffixChar = filePath[^1];
            }
        }

        return filePath;
    }

    /// <summary>
    /// Remove any path root present in the path.
    /// </summary>
    /// <param name="filePath">The file path whose root is to be removed.</param>
    /// <returns>The path with the root removed if it was present.</returns>
    public static string DropPathRoot(string filePath)
    {
        return string.IsNullOrEmpty(filePath) ? "" : filePath.Remove(0, Path.GetPathRoot(filePath)!.Length);
    }

    /// <summary>
    /// Returns a file name, for display purposes, of the specified length using "..." to indicate a longer name.
    /// </summary>
    /// <param name="filePath">The file path to be trimmed.</param>
    /// <param name="length">The maximum length of the trimmed file path.</param>
    /// <returns>Trimmed file path.</returns>
    /// <remarks>
    /// Minimum value for the <paramref name="length" /> parameter is 12. 12 will be used for any value 
    /// specified as less than 12.
    /// </remarks>
    public static string TrimFileName(string filePath, int length)
    {
        filePath = string.IsNullOrEmpty(filePath) ? "" : filePath.Trim();

        if (length < 12)
            length = 12;

        if (filePath.Length <= length)
            return filePath;

        string justName = GetFileName(filePath);

        if (justName.Length == filePath.Length)
        {
            // This is just a file name. Make sure extension shows.
            string justExtension = GetExtension(filePath);
            string trimName = GetFileNameWithoutExtension(filePath);

            // We can not trim file names less than 8 with a "...", so we truncate long extension.
            if (trimName.Length <= 8)
                return trimName + justExtension[..(length - trimName.Length)];

            if (justExtension.Length > length - 8)
                justExtension = justExtension[..(length - 8)];

            double offsetLen = (length - justExtension.Length - 3) / 2.0D;

            return $"{trimName[..(int)Math.Ceiling(offsetLen)]}...{trimName[(int)Math.Round(trimName.Length - Math.Floor(offsetLen) + 1.0D)..]}{justExtension}";
        }

        // File name alone exceeds length - recurses into function without path.
        if (justName.Length > length)
            return TrimFileName(justName, length);

        // File name contains path. Trims path before file name.
        string justFilePath = GetDirectoryName(filePath);
        int offset = length - justName.Length - 4;

        if (justFilePath.Length > offset && offset > 0)
            return $"{justFilePath[..offset]}...{Path.DirectorySeparatorChar}{justName}";

        // Can not fit path. Trims file name.
        return TrimFileName(justName, length);
    }

    /// <summary>
    /// Gets a lock on the file using the given lock function.
    /// </summary>
    /// <typeparam name="T">The return value of the lock function.</typeparam>
    /// <param name="fileName">The name of the on which the lock is to be obtained.</param>
    /// <param name="lockFunction">The function to be called in order to get the file lock.</param>
    /// <param name="secondsToWait">The number of seconds to wait before giving up on the file lock.</param>
    /// <param name="retryMilliseconds">The number of milliseconds to wait between attempts to obtain the file lock.</param>
    /// <returns>The return value of the lock function.</returns>
    /// <remarks>
    /// <para>
    /// The intent of this function is to provide a sane method for opening
    /// a file which may produce errors due to read/write contention.
    /// Usage of this class is fairly simple using the static methods
    /// built into the <see cref="File"/> class.
    /// </para>
    ///
    /// <code>
    /// using (StreamReader reader = GetFileLock(File.OpenText))
    /// {
    ///     // Read lines from the file
    /// }
    /// </code>
    ///
    /// <code>
    /// using (FileStream stream = GetFileLock(File.Create))
    /// {
    ///     // Write bytes into the file
    /// }
    /// </code>
    ///
    /// <para>
    /// This method will only retry if an <see cref="IOException"/>
    /// occurs while executing the <paramref name="lockFunction"/>.
    /// After retrying for at least <paramref name="secondsToWait"/>
    /// seconds, this function will throw the last IOException it encountered.
    /// </para>
    /// </remarks>
    public static T GetFileLock<T>(string fileName, Func<string, T> lockFunction, double secondsToWait = 5.0, int retryMilliseconds = 200)
    {
        double startTime = SystemTimer;
        double endTime = startTime + secondsToWait;

        while (true)
        {
            try
            {
                return lockFunction(fileName);
            }
            catch (IOException)
            {
                if ((int)secondsToWait != Timeout.Infinite && SystemTimer > endTime)
                    throw;
            }

            Thread.Sleep(retryMilliseconds);
        }
    }

    /// <summary>
    /// Attempts to get read access on a file.
    /// </summary>
    /// <param name="fileName">The file to check for read access.</param>
    /// <returns>True if read access is obtained; false otherwise.</returns>
    public static bool TryGetReadLock(string fileName)
    {
        try
        {
            using (File.OpenRead(fileName))
                return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Attempts to get read access on a file.
    /// </summary>
    /// <param name="fileName">The file to check for read access.</param>
    /// <returns>True if read access is obtained; false otherwise.</returns>
    public static bool TryGetReadLockExclusive(string fileName)
    {
        try
        {
            using (new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Attempts to get write access on a file.
    /// </summary>
    /// <param name="fileName">The file to check for write access.</param>
    /// <returns>True if write access is obtained; false otherwise.</returns>
    public static bool TryGetWriteLock(string fileName)
    {
        try
        {
            using (File.OpenWrite(fileName))
                return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Waits for the default duration (5 seconds) for read access on a file.
    /// </summary>
    /// <param name="fileName">The name of the file to wait for to obtain read access.</param>
    public static void WaitForReadLock(string fileName) => WaitForReadLock(fileName, 5);

    /// <summary>
    /// Waits for read access on a file for the specified number of seconds.
    /// </summary>
    /// <param name="fileName">The name of the file to wait for to obtain read access.</param>
    /// <param name="secondsToWait">The time to wait for in seconds to obtain read access on a file. Set to zero to wait infinitely.</param>
    public static void WaitForReadLock(string fileName, double secondsToWait)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Could not test file lock for \"{fileName}\", file does not exist", fileName);

        // Keeps trying for a file lock.
        double startTime = SystemTimer;

        while (!TryGetReadLock(fileName))
        {
            if (secondsToWait > 0)
            {
                if (SystemTimer > startTime + secondsToWait)
                    throw new IOException($"Could not open \"{fileName}\" for read access, tried for {secondsToWait} seconds");
            }

            // Yields to all other system threads.
            Thread.Sleep(250);
        }
    }

    /// <summary>
    /// Waits for the default duration (5 seconds) for read access on a file.
    /// </summary>
    /// <param name="fileName">The name of the file to wait for to obtain read access.</param>
    public static void WaitForReadLockExclusive(string fileName) => WaitForReadLockExclusive(fileName, 5);

    /// <summary>
    /// Waits for read access on a file for the specified number of seconds.
    /// </summary>
    /// <param name="fileName">The name of the file to wait for to obtain read access.</param>
    /// <param name="secondsToWait">The time to wait for in seconds to obtain read access on a file. Set to zero to wait infinitely.</param>
    public static void WaitForReadLockExclusive(string fileName, double secondsToWait)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Could not test file lock for \"{fileName}\", file does not exist", fileName);

        // Keeps trying for a file lock.
        double startTime = SystemTimer;

        while (!TryGetReadLockExclusive(fileName))
        {
            if (secondsToWait > 0)
            {
                if (SystemTimer > startTime + secondsToWait)
                    throw new IOException($"Could not open \"{fileName}\" for read access, tried for {secondsToWait} seconds");
            }

            // Yields to all other system threads.
            Thread.Sleep(250);
        }
    }

    /// <summary>
    /// Waits for the default duration (5 seconds) for write access on a file.
    /// </summary>
    /// <param name="fileName">The name of the file to wait for to obtain write access.</param>
    public static void WaitForWriteLock(string fileName) => WaitForWriteLock(fileName, 5);

    /// <summary>
    /// Waits for write access on a file for the specified number of seconds.
    /// </summary>
    /// <param name="fileName">The name of the file to wait for to obtain write access.</param>
    /// <param name="secondsToWait">The time to wait for in seconds to obtain write access on a file. Set to zero to wait infinitely.</param>
    public static void WaitForWriteLock(string fileName, double secondsToWait)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException($"Could not test file lock for \"{fileName}\", file does not exist", fileName);

        // Keeps trying for a file lock.
        double startTime = SystemTimer;

        while (!TryGetWriteLock(fileName))
        {
            if (secondsToWait > 0)
            {
                if (SystemTimer > startTime + secondsToWait)
                    throw new IOException($"Could not open \"{fileName}\" for write access, tried for {secondsToWait} seconds");
            }

            // Yields to all other system threads.
            Thread.Sleep(250);
        }
    }

    /// <summary>
    /// Waits for the default duration (5 seconds) for a file to exist.
    /// </summary>
    /// <param name="fileName">The name of the file to wait for until it is created.</param>
    public static void WaitTillExists(string fileName) => WaitTillExists(fileName, 5);

    /// <summary>
    /// Waits for a file to exist for the specified number of seconds.
    /// </summary>
    /// <param name="fileName">The name of the file to wait for until it is created.</param>
    /// <param name="secondsToWait">The time to wait for in seconds for the file to be created. Set to zero to wait infinitely.</param>
    public static void WaitTillExists(string fileName, double secondsToWait)
    {
        // Keeps waiting for a file to be created.
        double startTime = SystemTimer;

        while (!File.Exists(fileName))
        {
            if (secondsToWait > 0)
            {
                if (SystemTimer > startTime + secondsToWait)
                    throw new IOException($"Waited for \"{fileName}\" to exist for {secondsToWait} seconds, but it was never created");
            }

            // Yields to all other system threads.
            Thread.Sleep(250);
        }
    }

    private static double SystemTimer => DateTime.UtcNow.Ticks / (double)TimeSpan.TicksPerSecond;

    #endregion
}
