﻿//******************************************************************************************************
//  CollectionExtensions.cs - Gbtc
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
//  02/23/2003 - J. Ritchie Carroll
//       Generated original version of source code.
//  01/23/2005 - J. Ritchie Carroll
//       Migrated 2.0 version of source code from 1.1 source (GSF.Shared.Common).
//  08/17/2007 - Darrell Zuercher
//       Edited code comments.
//  09/11/2008 - J. Ritchie Carroll
//       Converted to C# extension functions.
//  02/17/2009 - Josh L. Patterson
//       Edited Code Comments.
//  02/20/2009 - J. Ritchie Carroll
//       Added predicate based IndexOf that extends IList<T>.
//  04/02/2009 - J. Ritchie Carroll
//       Added seed based scramble and unscramble IList<T> extensions.
//  06/05/2009 - Pinal C. Patel
//       Added generic AddRange() extension method for IList<T>.
//  06/09/2009 - Pinal C. Patel
//       Added generic GetRange() extension method for IList<T>.
//  08/05/2009 - Josh L. Patterson
//       Update comments.
//  09/14/2009 - Stephen C. Wills
//       Added new header and license agreement.
//  11/17/2009 - Pinal C. Patel
//       Added generic UpdateRange() extension method for IList<T>.
//  03/31/2009 - J. Ritchie Carroll
//       Added Majority() and Minority() extension methods to IEnumerable<T>.
//  03/18/2011 - J. Ritchie Carroll
//       Added dictionary Merge() extensions method for IDictionary and Any/All extensions for BitArray.
//  03/22/2011 - J. Ritchie Carroll
//       Modified array copy extension to handle zero length (i.e., empty) source arrays.
//  10/17/2011 - J. Ritchie Carroll
//       Added the ability to specify a default value for Majority and Minority when none exists.
//  12/13/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//  03/13/2016 - J. Ritchie Carroll
//       Added ToPagedList extension for enumerations.
//
//******************************************************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Random = Gemstone.Security.Cryptography.Random;

namespace Gemstone.Collections.CollectionExtensions;

/// <summary>
/// Defines extension functions related to manipulation of arrays and collections.
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// Gets a row of data out of a 2-dimensional array.
    /// </summary>
    /// <typeparam name="T">Type of array.</typeparam>
    /// <param name="source">Source array.</param>
    /// <param name="rowIndex">Row index, i.e., dimension zero value, to retrieve.</param>
    /// <returns>Values from specified <paramref name="rowIndex"/>.</returns>
    public static IEnumerable<T> GetRow<T>(this T[,]? source, int rowIndex)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        if (rowIndex < 0 || source.GetLength(0) > 1)
            throw new ArgumentOutOfRangeException(nameof(rowIndex));

        int length = source.GetLength(1);

        for (int i = 0; i < length; i++)
            yield return source[rowIndex, i];
    }

    /// <summary>
    /// Gets a column of data out of a 2-dimensional array.
    /// </summary>
    /// <typeparam name="T">Type of array.</typeparam>
    /// <param name="source">Source array.</param>
    /// <param name="columnIndex">Column index, i.e., dimension one value, to retrieve.</param>
    /// <returns>Values from specified <paramref name="columnIndex"/>.</returns>
    public static IEnumerable<T> GetColumn<T>(this T[,]? source, int columnIndex)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        if (columnIndex < 0 || source.GetLength(1) > 1)
            throw new ArgumentOutOfRangeException(nameof(columnIndex));

        int length = source.GetLength(0);

        for (int i = 0; i < length; i++)
            yield return source[i, columnIndex];
    }

    /// <summary>
    /// Gets a <see cref="PagedList{T}"/> to paginate <paramref name="source"/> enumeration for
    /// a given <paramref name="page"/> and specified <paramref name="pageSize"/>.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IEnumerable{T}"/> to paginate.</typeparam>
    /// <param name="source">Source enumeration to paginate.</param>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="count">Total records in source if known.</param>
    /// <returns>Set of items on a given <paramref name="page"/> for specified <paramref name="pageSize"/>.</returns>
    /// <remarks>
    /// If count is known or can be calculated early, specify the value in the
    /// <paramref name="count"/> parameter as an optimization to prevent a full
    /// enumeration on <paramref name="source"/> to get a count.
    /// </remarks>
    public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int page, int pageSize, int count = -1)
    {
        return new PagedList<T>(source, page, pageSize, count);
    }

    /// <summary>
    /// Merges elements of multiple dictionaries into a single dictionary with no duplicate key values overwritten.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IDictionary{TKey, TValue}"/> to merge.</typeparam>
    /// <typeparam name="TKey">Type of <see cref="IDictionary{TKey, TValue}"/> keys.</typeparam>
    /// <typeparam name="TValue">Type of <see cref="IDictionary{TKey, TValue}"/> values.</typeparam>
    /// <param name="source">Source dictionary to merge with another dictionary.</param>
    /// <param name="others">Other dictionaries to merge with source dictionary.</param>
    /// <returns>
    /// A merged collection of all unique dictionary elements, all <paramref name="others"/> merged left to the source with no duplicate
    /// key values overwritten (i.e., first encountered key value pair is the one that remains in the returned merged dictionary).
    /// </returns>
    public static T Merge<T, TKey, TValue>(this T source, params IDictionary<TKey, TValue>[] others) where T : IDictionary<TKey, TValue>, new()
    {
        return source.Merge(false, others);
    }

    /// <summary>
    /// Merges elements of multiple dictionaries into a single dictionary.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IDictionary{TKey, TValue}"/> to merge.</typeparam>
    /// <typeparam name="TKey">Type of <see cref="IDictionary{TKey, TValue}"/> keys.</typeparam>
    /// <typeparam name="TValue">Type of <see cref="IDictionary{TKey, TValue}"/> values.</typeparam>
    /// <param name="source">Source dictionary to merge with another dictionary.</param>
    /// <param name="overwriteExisting">Set to <c>true</c> to overwrite duplicate key values as they are encountered.</param>
    /// <param name="others">Other dictionaries to merge with source dictionary.</param>
    /// <returns>A merged collection of all unique dictionary elements, all <paramref name="others"/> merged left to the source.</returns>
    public static T Merge<T, TKey, TValue>(this T source, bool overwriteExisting, params IDictionary<TKey, TValue>[] others) where T : IDictionary<TKey, TValue>, new()
    {
        T mergedDictionary = new();
        List<IDictionary<TKey, TValue>> allDictionaries = [source];

        allDictionaries.AddRange(others);

        foreach (KeyValuePair<TKey, TValue> kvPair in allDictionaries.SelectMany(dictionary => dictionary))
        {
            if (overwriteExisting)
            {
                mergedDictionary[kvPair.Key] = kvPair.Value;
            }
            else
            {
                if (!mergedDictionary.ContainsKey(kvPair.Key))
                    mergedDictionary.Add(kvPair);
            }
        }

        return mergedDictionary;
    }

    /// <summary>
    /// Attempts to get the value for the given key and returns the default value instead if the key does not exist in the <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to check for the given key.</param>
    /// <param name="key">The key to be checked for the existence of a value.</param>
    /// <returns>The value of the key in the dictionary or the default value if no such value exists.</returns>
    public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
    {
        return dictionary.GetOrDefault(key, _ => default!);
    }

    /// <summary>
    /// Attempts to get the value for the given key and returns the default value instead if the key does not exist in the <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to check for the given key.</param>
    /// <param name="key">The key to be checked for the existence of a value.</param>
    /// <param name="defaultValueFactory">The function used to generate the default value if no value exists for the given key.</param>
    /// <returns>The value of the key in the dictionary or the default value if no such value exists.</returns>
    public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> defaultValueFactory)
    {
        return dictionary.TryGetValue(key, out TValue? value) ? value : defaultValueFactory(key);
    }

    /// <summary>
    /// Adds a key/value pair to the <see cref="IDictionary{TKey, TValue}"/> if the key does not already exist.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to add the key/value pair to if the key does not already exist.</param>
    /// <param name="key">The key to be added to the dictionary if it does not already exist.</param>
    /// <param name="valueFactory">The function used to generate a value for the key.</param>
    /// <returns>The value of the key in the dictionary.</returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
    {
        if (dictionary.TryGetValue(key, out TValue? value))
            return value;

        value = valueFactory(key);
        dictionary.Add(key, value);

        return value;
    }

    /// <summary>
    /// Adds a key/value pair to the <see cref="IDictionary{TKey, TValue}"/> if the key does not already exist.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to add the key/value pair to if the key does not already exist.</param>
    /// <param name="key">The key to be added to the dictionary if it does not already exist.</param>
    /// <param name="value">The value to assign to the key if the key does not already exist.</param>
    /// <returns>The value of the key in the dictionary.</returns>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.TryGetValue(key, out TValue? mapValue))
            return mapValue;

        dictionary.Add(key, value);

        return value;
    }

    /// <summary>
    /// Adds a key/value pair to the <see cref="IDictionary{TKey, TValue}"/> if the key does not already exist,
    /// or updates a key/value pair in the <see cref="IDictionary{TKey, TValue}"/> if the key already exists.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to add the key/value pair to if the key does not already exist.</param>
    /// <param name="key">The key to be added or whose value should be updated</param>
    /// <param name="addValueFactory">The function used to generate a value for an absent key</param>
    /// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value</param>
    /// <returns>The new value for the key. This will be either be the result of addValueFactory (if the key was absent) or the result of updateValueFactory (if the key was present).</returns>
    public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
    {
        if (dictionary.TryGetValue(key, out TValue? value))
            value = updateValueFactory(key, value);
        else
            value = addValueFactory(key);

        dictionary[key] = value;

        return value;
    }

    /// <summary>
    /// Adds a key/value pair to the <see cref="IDictionary{TKey, TValue}"/> if the key does not already exist,
    /// or updates a key/value pair in the <see cref="IDictionary{TKey, TValue}"/> if the key already exists.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to add the key/value pair to if the key does not already exist.</param>
    /// <param name="key">The key to be added or whose value should be updated</param>
    /// <param name="addValue">The value to be added for an absent key</param>
    /// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value</param>
    /// <returns>The new value for the key. This will be either be the result of addValueFactory (if the key was absent) or the result of updateValueFactory (if the key was present).</returns>
    public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue addValue, Func<TKey, TValue, TValue> updateValueFactory)
    {
        if (dictionary.TryGetValue(key, out TValue? value))
            value = updateValueFactory(key, value);
        else
            value = addValue;

        dictionary[key] = value;

        return value;
    }

    /// <summary>
    /// Adds a key/value pair to the <see cref="IDictionary{TKey, TValue}"/> if the key does not already exist,
    /// or updates a key/value pair in the <see cref="IDictionary{TKey, TValue}"/> if the key already exists.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to add the key/value pair to if the key does not already exist.</param>
    /// <param name="key">The key to be added or updated.</param>
    /// <param name="valueFactory">The function used to generate a value for the key.</param>
    /// <returns>The value of the key in the dictionary after updating.</returns>
    public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
    {
        TValue value = valueFactory(key);

        dictionary[key] = value;

        return value;
    }

    /// <summary>
    /// Adds a key/value pair to the <see cref="IDictionary{TKey, TValue}"/> if the key does not already exist,
    /// or updates a key/value pair in the <see cref="IDictionary{TKey, TValue}"/> if the key already exists.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="dictionary">The dictionary to add the key/value pair to if the key does not already exist.</param>
    /// <param name="key">The key to be added or updated.</param>
    /// <param name="value">The value to be assigned to the key.</param>
    /// <returns>The value of the key in the dictionary after updating.</returns>
    public static TValue AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        dictionary[key] = value;
        return value;
    }

    /// <summary>
    /// Converts a name/value collection to a dictionary.
    /// </summary>
    /// <param name="collection">Name/value collection.</param>
    /// <returns>Dictionary converted from a name/value collection.</returns>
    public static Dictionary<string, string> ToDictionary(this NameValueCollection collection)
    {
        return collection.AllKeys.ToDictionary(key => key!, key => collection[key]!);
    }

    /// <summary>
    /// Returns <c>true</c> if any item in <see cref="BitArray"/> is equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="source">Source <see cref="BitArray"/>.</param>
    /// <param name="value"><see cref="bool"/> value to test for.</param>
    /// <returns><c>true</c> if any item in <see cref="BitArray"/> is equal to <paramref name="value"/>.</returns>
    public static bool Any(this BitArray source, bool value)
    {
        return source.Cast<bool>().Any(item => item == value);
    }

    /// <summary>
    /// Returns <c>true</c> if all items in <see cref="BitArray"/> are equal to <paramref name="value"/>.
    /// </summary>
    /// <param name="source">Source <see cref="BitArray"/>.</param>
    /// <param name="value"><see cref="bool"/> value to test for.</param>
    /// <returns><c>true</c> if all items in <see cref="BitArray"/> are equal to <paramref name="value"/>.</returns>
    public static bool All(this BitArray source, bool value)
    {
        return source.Cast<bool>().All(item => item == value);
    }

    /// <summary>
    /// Determines whether all elements of a sequence satisfy a condition with each item being tested in parallel.
    /// </summary>
    /// <param name="source">An <see cref="IEnumerable{TSource}" /> that contains the elements to apply the predicate to.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="options">Any <see cref="ParallelOptions"/> to apply.</param>
    /// <returns>
    /// <c>true</c> if every element of the source sequence passes the test in the specified predicate, or if the sequence is empty; otherwise, <c>false</c>.
    /// </returns>
    /// <typeparam name="TSource">The type of the elements of <paramref name="source" />.</typeparam>
    /// <exception cref="ArgumentNullException"><paramref name="source" /> or <paramref name="predicate" /> is <c>null</c>.</exception>
    public static bool AllParallel<TSource>(this IEnumerable<TSource>? source, Func<TSource, bool> predicate, ParallelOptions? options = null)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        if (predicate is null)
            throw new ArgumentNullException(nameof(predicate));

        bool succeeded = true;

        Parallel.ForEach(source, options ?? new ParallelOptions(), (element, state) =>
        {
            if (predicate(element))
                return;

            succeeded = false;
            state.Break();
        });

        return succeeded;
    }

    /// <summary>
    /// Gets the median item(s) from an enumeration, i.e., one return item for odd lengths, two for even lengths.
    /// </summary>
    /// <remarks>
    /// If enumeration length is even, two items will be returned representing the items in the middle of the set;
    /// otherwise, if the enumeration length is odd, one item will be returned from exactly the middle of the set.
    /// </remarks>
    /// <typeparam name="T"><see cref="Type"/> of elements in the <paramref name="source"/></typeparam>
    /// <param name="source">An enumeration over which to find the median element(s).</param>
    /// <returns>The median item(s) from an enumeration, or <c>null</c> if <paramref name="source"/> is <c>null</c>.</returns>
    public static IEnumerable<T>? Median<T>(this IEnumerable<T> source)
    {
        if (source is null)
            return null;

        return source
            .OrderBy(t => t)
            .Middle();
    }

    /// <summary>
    /// Gets the middle item(s) from an enumeration, i.e., one return item for odd lengths, two for even lengths.
    /// </summary>
    /// <remarks>
    /// If enumeration length is even, two items will be returned representing the items in the middle of the set;
    /// otherwise, if the enumeration length is odd, one item will be returned from exactly the middle of the set.
    /// </remarks>
    /// <typeparam name="T"><see cref="Type"/> of elements in the <paramref name="source"/></typeparam>
    /// <param name="source">An enumeration over which to find the middle element(s).</param>
    /// <returns>The middle item(s) from an enumeration, or <c>null</c> if <paramref name="source"/> is <c>null</c>.</returns>
    public static IEnumerable<T>? Middle<T>(this IEnumerable<T> source)
    {
        if (source is null)
            return null;

        if (source is IList<T> sourceAsList)
            return middleList(sourceAsList);

        if (source is ICollection<T> sourceAsCollection)
            return middleCollection(sourceAsCollection);

        // Instead of source.ToList(), we could use source.Count()
        // followed by source.Skip(...).Take(...) to avoid memory overhead
        // but at the cost of enumerating twice
        return middleList(source.ToList());

        IEnumerable<T> middleList(IList<T> list)
        {
            if (list.Count == 0)
                yield break;

            int midIndex = list.Count / 2;

            if (list.Count % 2 == 0)
                yield return list[midIndex - 1];

            yield return list[midIndex];
        }

        IEnumerable<T> middleCollection(ICollection<T> collection)
        {
            int midIndex = collection.Count / 2;
            int takeCount = 1;

            if (collection.Count % 2 == 0)
            {
                midIndex--;
                takeCount++;
            }

            return collection
                .Skip(midIndex)
                .Take(takeCount);
        }
    }

    /// <summary>
    /// Returns the majority value in the collection, or default type value if no item represents the majority.
    /// </summary>
    /// <typeparam name="TSource"><see cref="Type"/> of elements in the <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The generic <see cref="Type"/> of the objects to be compared.</typeparam>
    /// <param name="source">An enumeration over which to find the majority element.</param>
    /// <param name="keySelector">A delegate that takes an object and produces the key for comparison.</param>
    /// <param name="forwardSearch"><c>true</c> to search forward in <paramref name="source"/>; otherwise <c>false</c> to search backwards.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{TKey}" /> implementation to use when comparing keys, or <c>null</c> to use the default comparer for the type of the key.</param>
    /// <returns>The majority value in the collection.</returns>
    public static TSource MajorityBy<TSource, TKey>(this IEnumerable<TSource>? source, Func<TSource, TKey> keySelector, bool forwardSearch = true, IEqualityComparer<TKey>? comparer = null) where TKey : notnull
    {
        return source.MajorityBy(default!, keySelector, forwardSearch, comparer);
    }

    /// <summary>
    /// Returns the majority value in the collection, or <paramref name="defaultValue"/> if no item represents the majority.
    /// </summary>
    /// <typeparam name="TSource"><see cref="Type"/> of elements in the <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The generic <see cref="Type"/> of the objects to be compared.</typeparam>
    /// <param name="source">An enumeration over which to find the majority element.</param>
    /// <param name="defaultValue">Default value to return if no item represents the majority.</param>
    /// <param name="keySelector">A delegate that takes an object and produces the key for comparison.</param>
    /// <param name="forwardSearch"><c>true</c> to search forward in <paramref name="source"/>; otherwise <c>false</c> to search backwards.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{TKey}" /> implementation to use when comparing keys, or <c>null</c> to use the default comparer for the type of the key.</param>
    /// <returns>The majority value in the collection.</returns>
    public static TSource MajorityBy<TSource, TKey>(this IEnumerable<TSource>? source, TSource defaultValue, Func<TSource, TKey> keySelector, bool forwardSearch = true, IEqualityComparer<TKey>? comparer = null) where TKey : notnull
    {
        TSource majority = defaultValue;

        if (source is null)
            return majority;

        TSource[] values;

        if (forwardSearch)
            values = source as TSource[] ?? source.ToArray();
        else
            values = source.Reverse().ToArray();

        if (values.Length <= 1)
            return majority;

        Dictionary<TKey, Tuple<int, TSource>> itemCounts = new(comparer);

        // Count each number of items in the list
        foreach (TSource value in values)
        {
            TKey key = keySelector(value);

            if (itemCounts.TryGetValue(key, out Tuple<int, TSource>? valueCount))
            {
                int count = valueCount.Item1 + 1;
                itemCounts[key] = new Tuple<int, TSource>(count, valueCount.Item2);
            }
            else
            {
                itemCounts.Add(key, new Tuple<int, TSource>(1, value));
            }
        }

        // Find the largest number of items in the list
        KeyValuePair<TKey, Tuple<int, TSource>> maxItem = itemCounts.Max((a, b) => a.Value.Item1 < b.Value.Item1 ? -1 : a.Value.Item1 > b.Value.Item1 ? 1 : 0);

        // If item with the largest count has a plural majority, then it is the majority item
        if (maxItem.Value.Item1 > 1)
            majority = maxItem.Value.Item2;

        return majority;
    }

    /// <summary>
    /// Returns the majority value in the collection, or default type value if no item represents the majority.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of elements in the <paramref name="source"/>.</typeparam>
    /// <param name="source">An enumeration over which to find the majority element.</param>
    /// <param name="forwardSearch"><c>true</c> to search forward in <paramref name="source"/>; otherwise <c>false</c> to search backwards.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}" /> implementation to use when comparing keys, or <c>null</c> to use the default comparer for the type of the key.</param>
    /// <returns>The majority value in the collection.</returns>
    public static T Majority<T>(this IEnumerable<T>? source, bool forwardSearch = true, IEqualityComparer<T>? comparer = null) where T : notnull
    {
        return source.Majority(default!, forwardSearch, comparer);
    }

    /// <summary>
    /// Returns the majority value in the collection, or <paramref name="defaultValue"/> if no item represents the majority.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of elements in the <paramref name="source"/>.</typeparam>
    /// <param name="source">An enumeration over which to find the majority element.</param>
    /// <param name="defaultValue">Default value to return if no item represents the majority.</param>
    /// <param name="forwardSearch"><c>true</c> to search forward in <paramref name="source"/>; otherwise <c>false</c> to search backwards.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}" /> implementation to use when comparing keys, or <c>null</c> to use the default comparer for the type of the key.</param>
    /// <returns>The majority value in the collection.</returns>
    public static T Majority<T>(this IEnumerable<T>? source, T defaultValue, bool forwardSearch = true, IEqualityComparer<T>? comparer = null) where T : notnull
    {
        T majority = defaultValue;

        if (source is null)
            return majority;

        T[] values;

        if (forwardSearch)
            values = source as T[] ?? source.ToArray();
        else
            values = source.Reverse().ToArray();

        if (values.Length <= 1)
            return majority;

        Dictionary<T, int> itemCounts = new(comparer);

        // Count each number of items in the list
        foreach (T item in values)
        {
            if (itemCounts.TryGetValue(item, out int count))
            {
                count++;
                itemCounts[item] = count;
            }
            else
            {
                itemCounts.Add(item, 1);
            }
        }

        // Find the largest number of items in the list
        KeyValuePair<T, int> maxItem = itemCounts.Max((a, b) => a.Value < b.Value ? -1 : a.Value > b.Value ? 1 : 0);

        // If item with the largest count has a plural majority, then it is the majority item
        if (maxItem.Value > 1)
            majority = maxItem.Key;

        return majority;
    }

    /// <summary>
    /// Returns the minority value in the collection, or default type value if no item represents the majority.
    /// </summary>
    /// <typeparam name="TSource"><see cref="Type"/> of elements in the <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey">The generic <see cref="Type"/> of the objects to be compared.</typeparam>
    /// <param name="source">An enumeration over which to find the majority element.</param>
    /// <param name="keySelector">A delegate that takes an object and produces the key for comparison.</param>
    /// <param name="forwardSearch"><c>true</c> to search forward in <paramref name="source"/>; otherwise <c>false</c> to search backwards.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{TKey}" /> implementation to use when comparing keys, or <c>null</c> to use the default comparer for the type of the key.</param>
    /// <returns>The minority value in the collection.</returns>
    public static TSource MinorityBy<TSource, TKey>(this IEnumerable<TSource>? source, Func<TSource, TKey> keySelector, bool forwardSearch = true, IEqualityComparer<TKey>? comparer = null) where TKey : notnull
    {
        return source.MinorityBy(default!, keySelector, forwardSearch, comparer);
    }

    /// <summary>
    /// Returns the minority value in the collection, or <paramref name="defaultValue"/> if no item represents the majority.
    /// </summary>
    /// <typeparam name="TSource"><see cref="Type"/> of elements in the <paramref name="source"/>.</typeparam>
    /// <typeparam name="TKey"><see cref="Type"/> of the objects to be compared.</typeparam>
    /// <param name="source">An enumeration over which to find the majority element.</param>
    /// <param name="defaultValue">Default value to return if no item represents the majority.</param>
    /// <param name="keySelector">A delegate that takes an object and produces the key for comparison.</param>
    /// <param name="forwardSearch"><c>true</c> to search forward in <paramref name="source"/>; otherwise <c>false</c> to search backwards.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{TKey}" /> implementation to use when comparing keys, or <c>null</c> to use the default comparer for the type of the key.</param>
    /// <returns>The majority value in the collection.</returns>
    public static TSource MinorityBy<TSource, TKey>(this IEnumerable<TSource>? source, TSource defaultValue, Func<TSource, TKey> keySelector, bool forwardSearch = true, IEqualityComparer<TKey>? comparer = null) where TKey : notnull
    {
        TSource minority = defaultValue;

        if (source is null)
            return minority;

        TSource[] values;

        if (forwardSearch)
            values = source as TSource[] ?? source.ToArray();
        else
            values = source.Reverse().ToArray();

        if (values.Length <= 1)
            return minority;

        Dictionary<TKey, Tuple<int, TSource>> itemCounts = new(comparer);

        // Count each number of items in the list
        foreach (TSource value in values)
        {
            TKey key = keySelector(value);

            if (itemCounts.TryGetValue(key, out Tuple<int, TSource>? valueCount))
            {
                int count = valueCount.Item1 + 1;
                itemCounts[key] = new Tuple<int, TSource>(count, valueCount.Item2);
            }
            else
            {
                itemCounts.Add(key, new Tuple<int, TSource>(1, value));
            }
        }

        // Find the smallest number of items in the list
        KeyValuePair<TKey, Tuple<int, TSource>> minItem = itemCounts.Min((a, b) => a.Value.Item1 < b.Value.Item1 ? -1 : a.Value.Item1 > b.Value.Item1 ? 1 : 0);
        minority = minItem.Value.Item2;

        return minority;
    }

    /// <summary>
    /// Returns the minority value in the collection, or default type value if no item represents the minority.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of elements in the <paramref name="source"/>.</typeparam>
    /// <param name="source">An enumeration over which to find the minority element.</param>
    /// <param name="forwardSearch"><c>true</c> to search forward in <paramref name="source"/>; otherwise <c>false</c> to search backwards.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}" /> implementation to use when comparing keys, or <c>null</c> to use the default comparer for the type of the key.</param>
    /// <returns>The minority value in the collection.</returns>
    public static T Minority<T>(this IEnumerable<T>? source, bool forwardSearch = true, IEqualityComparer<T>? comparer = null) where T : notnull
    {
        return source.Minority(default!, forwardSearch, comparer);
    }

    /// <summary>
    /// Returns the minority value in the collection, or <paramref name="defaultValue"/> if no item represents the minority.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of elements in the <paramref name="source"/>.</typeparam>
    /// <param name="source">An enumeration over which to find the minority element.</param>
    /// <param name="defaultValue">Default value to return if no item represents the minority.</param>
    /// <param name="forwardSearch"><c>true</c> to search forward in <paramref name="source"/>; otherwise <c>false</c> to search backwards.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}" /> implementation to use when comparing keys, or <c>null</c> to use the default comparer for the type of the key.</param>
    /// <returns>The minority value in the collection.</returns>
    public static T Minority<T>(this IEnumerable<T>? source, T defaultValue, bool forwardSearch = true, IEqualityComparer<T>? comparer = null) where T : notnull
    {
        T minority = defaultValue;

        if (source is null)
            return minority;

        T[] values;

        if (forwardSearch)
            values = source as T[] ?? source.ToArray();
        else
            values = source.Reverse().ToArray();

        if (values.Length <= 1)
            return minority;

        Dictionary<T, int> itemCounts = new(comparer);

        // Count each number of items in the list
        foreach (T item in values)
        {
            if (itemCounts.TryGetValue(item, out int count))
            {
                count++;
                itemCounts[item] = count;
            }
            else
            {
                itemCounts.Add(item, 1);
            }
        }

        // Find the smallest number of items in the list
        KeyValuePair<T, int> minItem = itemCounts.Min((a, b) => a.Value < b.Value ? -1 : a.Value > b.Value ? 1 : 0);
        minority = minItem.Key;

        return minority;
    }

    /// <summary>
    /// Adds the specified <paramref name="items"/> to the <paramref name="collection"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of elements in the <paramref name="collection"/>.</typeparam>
    /// <param name="collection">The collection to which the <paramref name="items"/> are to be added.</param>
    /// <param name="items">The elements to be added to the <paramref name="collection"/>.</param>
    public static void AddRange<T>(this IList<T> collection, IEnumerable<T> items)
    {
        foreach (T item in items)
            collection.Add(item);
    }

    /// <summary>
    /// Updates <paramref name="collection"/> starting at the <paramref name="index"/> with the specified <paramref name="items"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of elements in the <paramref name="collection"/>.</typeparam>
    /// <param name="collection">The collection whose elements are to be updated with the specified <paramref name="items"/>.</param>
    /// <param name="index">The zero-based index in the <paramref name="collection"/> at which elements are to be updated.</param>
    /// <param name="items">The elements that will replace the <paramref name="collection"/> elements starting at the <paramref name="index"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException">The specified <paramref name="index"/> is present in the <paramref name="collection"/>.</exception>
    public static void UpdateRange<T>(this IList<T> collection, int index, IEnumerable<T> items)
    {
        foreach (T item in items)
            collection[index++] = item;
    }

    /// <summary>
    /// Returns elements in the specified range from the <paramref name="collection"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of elements in the <paramref name="collection"/>.</typeparam>
    /// <param name="collection">The collection from which elements are to be retrieved.</param>
    /// <param name="index">The 0-based index position in the <paramref name="collection"/> from which elements are to be retrieved.</param>
    /// <param name="count">The number of elements to be retrieved from the <paramref name="collection"/> starting at the <paramref name="index"/>.</param>
    /// <returns>An <see cref="IList{T}"/> object.</returns>
    public static IList<T> GetRange<T>(this IList<T> collection, int index, int count)
    {
        List<T> result = [];

        for (int i = index; i < index + count; i++)
            result.Add(collection[i]);

        return result;
    }

    /// <summary>
    /// Returns the index of the first element of the sequence that satisfies a condition or <c>-1</c> if no such element is found.
    /// </summary>
    /// <param name="source">A <see cref="IList{T}"/> to find an index in.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>Index of the first element in <paramref name="source"/> that matches the specified <paramref name="predicate"/>; otherwise, <c>-1</c>.</returns>
    /// <typeparam name="T"><see cref="Type"/> of elements of <paramref name="source"/>.</typeparam>
    public static int IndexOf<T>(this IList<T> source, Func<T, bool> predicate)
    {
        for (int index = 0; index < source.Count; index++)
        {
            if (predicate(source[index]))
                return index;
        }

        return -1;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Array"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of the <see cref="Array"/> to be copied.</typeparam>
    /// <param name="source">The source <see cref="Array"/> whose elements are to be copied.</param>
    /// <param name="startIndex">The source array index from where the elements are to be copied.</param>
    /// <param name="length">The number of elements to be copied starting from the startIndex.</param>
    /// <returns>An <see cref="Array"/> of elements copied from the specified portion of the source <see cref="Array"/>.</returns>
    /// <remarks>
    /// Returned <see cref="Array"/> will be extended as needed to make it the specified <paramref name="length"/>, but
    /// it will never be less than the source <see cref="Array"/> length - <paramref name="startIndex"/>.
    /// </remarks>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="startIndex"/> is outside the range of valid indexes for the source <see cref="Array"/> -or-
    /// <paramref name="length"/> is less than 0.
    /// </exception>
    public static T[] Copy<T>(this T[] source, int startIndex, int length)
    {
        if (startIndex < 0 || length < 0)
            throw new ArgumentOutOfRangeException(startIndex < 0 ? "startIndex" : "length", "Cannot be negative");

        if (source.Length == 0)
            return new T[length];

        if (startIndex >= source.Length)
            throw new ArgumentOutOfRangeException(nameof(startIndex), "Not a valid index into source buffer");

        // Create a new array that will be returned with the specified array elements.
        T[] copyOfSource = new T[source.Length - startIndex < length ? source.Length - startIndex : length];
        Array.Copy(source, startIndex, copyOfSource, 0, copyOfSource.Length);

        return copyOfSource;
    }

    /// <summary>Selects the smallest item from the enumeration.</summary>
    /// <typeparam name="TSource"><see cref="Type"/> of the objects to be selected from.</typeparam>
    /// <typeparam name="TKey"><see cref="Type"/> of the objects to be compared.</typeparam>
    /// <param name="source">An enumeration that contains the objects to be selected from.</param>
    /// <param name="keySelector">A delegate that takes an object and produces the key for comparison.</param>
    /// <returns>Returns the smallest item from the enumeration.</returns>
    public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable
    {
        TSource minItem = default!;

        using IEnumerator<TSource> enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
            return minItem;

        minItem = enumerator.Current;
        TKey minKey = keySelector(minItem);

        while (enumerator.MoveNext())
        {
            TKey nextKey = keySelector(enumerator.Current);

            if (nextKey.CompareTo(minKey) >= 0)
                continue;

            minItem = enumerator.Current;
            minKey = nextKey;
        }

        return minItem;
    }

    /// <summary>Returns the smallest item from the enumeration.</summary>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IEnumerable{T}"/>.</typeparam>
    /// <param name="source">An enumeration that is compared against.</param>
    /// <param name="comparer">A delegate that takes two generic types to compare, and returns an integer based on the comparison.</param>
    /// <returns>Returns a generic type.</returns>
    public static TSource Min<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, int> comparer)
    {
        TSource minItem = default!;

        using IEnumerator<TSource> enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
            return minItem;

        minItem = enumerator.Current;

        while (enumerator.MoveNext())
        {
            if (comparer(enumerator.Current, minItem) < 0)
                minItem = enumerator.Current;
        }

        return minItem;
    }

    /// <summary>Returns the smallest item from the enumeration.</summary>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IEnumerable{T}"/>.</typeparam>
    /// <param name="source">An enumeration that is compared against.</param>
    /// <param name="comparer">A comparer object.</param>
    /// <returns>Returns a generic type.</returns>
    public static TSource Min<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
    {
        return source.Min(comparer.Compare);
    }

    /// <summary>Selects the largest item from the enumeration.</summary>
    /// <typeparam name="TSource"><see cref="Type"/> of the objects to be selected from.</typeparam>
    /// <typeparam name="TKey"><see cref="Type"/> of the objects to be compared.</typeparam>
    /// <param name="source">An enumeration that contains the objects to be selected from.</param>
    /// <param name="keySelector">A delegate that takes an object and produces the key for comparison.</param>
    /// <returns>Returns the largest item from the enumeration.</returns>
    public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector) where TKey : IComparable
    {
        TSource maxItem = default!;

        using IEnumerator<TSource> enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
            return maxItem;

        maxItem = enumerator.Current;
        TKey maxKey = keySelector(maxItem);

        while (enumerator.MoveNext())
        {
            TKey nextKey = keySelector(enumerator.Current);

            if (nextKey.CompareTo(maxKey) <= 0)
                continue;

            maxItem = enumerator.Current;
            maxKey = nextKey;
        }

        return maxItem;
    }

    /// <summary>Returns the largest item from the enumeration.</summary>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IEnumerable{T}"/>.</typeparam>
    /// <param name="source">An enumeration that is compared against.</param>
    /// <param name="comparer">A delegate that takes two generic types to compare, and returns an integer based on the comparison.</param>
    /// <returns>Returns a generic type.</returns>
    public static TSource Max<TSource>(this IEnumerable<TSource> source, Func<TSource, TSource, int> comparer)
    {
        TSource maxItem = default!;

        using IEnumerator<TSource> enumerator = source.GetEnumerator();

        if (!enumerator.MoveNext())
            return maxItem;

        maxItem = enumerator.Current;

        while (enumerator.MoveNext())
        {
            if (comparer(enumerator.Current, maxItem) > 0)
                maxItem = enumerator.Current;
        }

        return maxItem;
    }

    /// <summary>Returns the largest item from the enumeration.</summary>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IEnumerable{T}"/>.</typeparam>
    /// <param name="source">An enumeration that is compared against.</param>
    /// <param name="comparer">A comparer object.</param>
    /// <returns>Returns a generic type.</returns>
    public static TSource Max<TSource>(this IEnumerable<TSource> source, IComparer<TSource> comparer)
    {
        return source.Max(comparer.Compare);
    }

    /// <summary>
    /// Returns only the elements whose keys are distinct.
    /// </summary>
    /// <typeparam name="TSource">The type of the source objects in the collection.</typeparam>
    /// <typeparam name="TKey">The type of the keys to be compared.</typeparam>
    /// <param name="source">The collection of source objects.</param>
    /// <param name="keySelector">The function used to access the keys of the source objects.</param>
    /// <param name="forwardSearch"><c>true</c> to search forward in <paramref name="source"/>; otherwise <c>false</c> to search backwards.</param>
    /// <returns>The elements from <paramref name="source"/> whose keys are distinct.</returns>
    public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool forwardSearch = true)
    {
        return forwardSearch ?
            source
                .Select(obj => new DistinctByWrapper<TKey, TSource>(keySelector(obj), obj))
                .Distinct()
                .Select(wrapper => wrapper.Value) :
            source
                .Select(obj => new DistinctByWrapper<TKey, TSource>(keySelector(obj), obj))
                .Reverse()
                .Distinct()
                .Select(wrapper => wrapper.Value);
    }

    /// <summary>Converts an enumeration to a string, using the default delimiter ("|") that can later be
    /// converted back to a list using LoadDelimitedString.</summary>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IEnumerable{T}"/>.</typeparam>
    /// <param name="source">The source object to be converted into a delimited string.</param>
    /// <returns>Returns a <see cref="string"/> that is result of combining all elements in the list delimited by the '|' character.</returns>
    public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source)
    {
        return source.ToDelimitedString('|');
    }

    /// <summary>Converts an enumeration to a string that can later be converted back to a list using
    /// LoadDelimitedString.</summary>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IEnumerable{T}"/>.</typeparam>
    /// <param name="source">The source object to be converted into a delimited string.</param>
    /// <param name="delimiter">The delimiting character used.</param>
    /// <returns>Returns a <see cref="string"/> that is result of combining all elements in the list delimited by <paramref name="delimiter"/>.</returns>
    public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source, char delimiter)
    {
        return ToDelimitedString<TSource, char>(source, delimiter);
    }

    /// <summary>Converts an enumeration to a string that can later be converted back to a list using
    /// LoadDelimitedString.</summary>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IEnumerable{T}"/>.</typeparam>
    /// <param name="source">The source object to be converted into a delimited string.</param>
    /// <param name="delimiter">The delimiting <see cref="string"/> used.</param>
    /// <returns>Returns a <see cref="string"/> that is result of combining all elements in the list delimited by <paramref name="delimiter"/>.</returns>
    public static string ToDelimitedString<TSource>(this IEnumerable<TSource> source, string delimiter)
    {
        return ToDelimitedString<TSource, string>(source, delimiter);
    }

    /// <summary>Converts an enumeration to a string that can later be converted back to a list using
    /// LoadDelimitedString.</summary>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IEnumerable{T}"/>.</typeparam>
    /// <typeparam name="TDelimiter"><see cref="Type"/> of <paramref name="delimiter"/>.</typeparam>
    /// <param name="source">The source object to be converted into a delimited string.</param>
    /// <param name="delimiter">The delimiter of type TDelimiter used.</param>
    /// <returns>Returns a <see cref="string"/> that is result of combining all elements in the list delimited by <paramref name="delimiter"/>.</returns>
    private static string ToDelimitedString<TSource, TDelimiter>(IEnumerable<TSource> source, TDelimiter delimiter)
    {
        if (Common.IsReference(delimiter!) && delimiter is null)
            throw new ArgumentNullException(nameof(delimiter), "delimiter cannot be null");

        StringBuilder delimitedString = new();

        foreach (TSource item in source)
        {
            if (delimitedString.Length > 0)
                delimitedString.Append(delimiter);

            delimitedString.Append(item);
        }

        return delimitedString.ToString();
    }

    /// <summary>Appends items parsed from delimited string, created with ToDelimitedString, using the default
    /// delimiter ("|") into the given list.</summary>
    /// <remarks>Items that are converted are added to list. The list is not cleared in advance.</remarks>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IList{T}"/>.</typeparam>
    /// <param name="destination">The list we are adding items to.</param>
    /// <param name="delimitedString">The delimited string to parse for items.</param>
    /// <param name="convertFromString">Delegate that takes one parameter and converts from string to type TSource.</param>
    public static void LoadDelimitedString<TSource>(this IList<TSource> destination, string delimitedString, Func<string, TSource> convertFromString)
    {
        destination.LoadDelimitedString(delimitedString, '|', convertFromString);
    }

    /// <summary>Appends items parsed from delimited string, created with ToDelimitedString, into the given list.</summary>
    /// <remarks>Items that are converted are added to list. The list is not cleared in advance.</remarks>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IList{T}"/>.</typeparam>
    /// <param name="destination">The list we are adding items to.</param>
    /// <param name="delimitedString">The delimited string to parse for items.</param>
    /// <param name="delimiter">The <see cref="char"/> value to look for in the <paramref name="delimitedString"/> as the delimiter.</param>
    /// <param name="convertFromString">Delegate that takes one parameter and converts from string to type TSource.</param>
    public static void LoadDelimitedString<TSource>(this IList<TSource> destination, string delimitedString, char delimiter, Func<string, TSource> convertFromString)
    {
        if (delimitedString is null)
            throw new ArgumentNullException(nameof(delimitedString), "delimitedString cannot be null");

        if (destination.IsReadOnly)
            throw new ArgumentException("Cannot add items to a read only list");

        foreach (string item in delimitedString.Split(delimiter))
            destination.Add(convertFromString(item.Trim()));
    }

    /// <summary>Appends items parsed from delimited string, created with ToDelimitedString, into the given list.</summary>
    /// <remarks>Items that are converted are added to list. The list is not cleared in advance.</remarks>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IList{T}"/>.</typeparam>
    /// <param name="destination">The list we are adding items to.</param>
    /// <param name="delimitedString">The delimited string to parse for items.</param>
    /// <param name="delimiters">An array of delimiters to look for in the <paramref name="delimitedString"/> as the delimiter.</param>
    /// <param name="convertFromString">Delegate that takes a <see cref="string"/> and converts to type TSource.</param>
    public static void LoadDelimitedString<TSource>(this IList<TSource> destination, string delimitedString, string[] delimiters, Func<string, TSource> convertFromString)
    {
        if (delimiters is null)
            throw new ArgumentNullException(nameof(delimiters), "delimiters cannot be null");

        if (delimitedString is null)
            throw new ArgumentNullException(nameof(delimitedString), "delimitedString cannot be null");

        if (destination.IsReadOnly)
            throw new ArgumentException("Cannot add items to a read only list");

        foreach (string item in delimitedString.Split(delimiters, StringSplitOptions.None))
            destination.Add(convertFromString(item.Trim()));
    }

    /// <summary>
    /// Iterates through each item in the list. Allowing items to be removed from the list.
    /// </summary>
    /// <typeparam name="T"><see cref="Type"/> of <see cref="List{T}"/>.</typeparam>
    /// <param name="list">the list to iterate though</param>
    /// <param name="shouldRemove">the function to call to determine 
    /// if the items should be removed from the list. </param>
    /// <returns>
    /// The number of items removed from the list.
    /// </returns>
    /// <remarks>
    /// In order to minimize the overhead of a removal. Any item removed with be replaced with
    /// the last item in the list. Sequence will not be preserved using this method.
    /// </remarks>
    public static int RemoveWhere<T>(this List<T> list, Func<T, bool> shouldRemove)
    {
        // 10/24/2016 - Steven E. Chisholm
        int removedCount = 0;

        if (list is null)
            throw new ArgumentNullException(nameof(list));

        if (shouldRemove is null)
            throw new ArgumentNullException(nameof(shouldRemove));

        for (int i = 0; i < list.Count; i++)
        {
            if (!shouldRemove(list[i]))
                continue;

            removedCount++;

            if (list.Count > 1 && i != list.Count - 1)
                list[i] = list[^1];

            list.RemoveAt(list.Count - 1);
            i--;
        }

        return removedCount;
    }

    /// <summary>
    /// Rearranges all the elements in the list into a highly-random order.
    /// </summary>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IList{T}"/>.</typeparam>
    /// <param name="source">The input list of generic types to scramble.</param>
    /// <remarks>This function uses a cryptographically strong random number generator to perform the scramble.</remarks>
    public static void Scramble<TSource>(this IList<TSource> source)
    {
        if (source.IsReadOnly && source is not TSource[])
            throw new ArgumentException("Cannot modify items in a read only list");

        // Mixes up the data in random order.
        for (int i = 0; i < source.Count; i++)
        {
            // Calls random function from Gemstone namespace.
            int j = Random.Int32Between(0, source.Count - 1);

            if (i == j)
                continue;

            // Swaps items
            (source[i], source[j]) = (source[j], source[i]);
        }
    }

    /// <summary>
    /// Rearranges all the elements in the list into a repeatable pseudo-random order.
    /// </summary>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IList{T}"/>.</typeparam>
    /// <param name="source">The input list of generic types to scramble.</param>
    /// <param name="seed">A number used to calculate a starting value for the pseudo-random number sequence.</param>
    /// <remarks>This function uses the <see cref="System.Random"/> generator to perform the scramble using a sequence that is repeatable.</remarks>
    public static void Scramble<TSource>(this IList<TSource> source, int seed)
    {
        if (source.IsReadOnly && source is not TSource[])
            throw new ArgumentException("Cannot modify items in a read only list");

        System.Random random = new(seed);
        int count = source.Count;

        // Mixes up the data in random order.
        for (int i = 0; i < count; i++)
        {
            // Calls random function from System namespace.
            int j = random.Next(count);

            if (i == j)
                continue;

            // Swaps items
            (source[i], source[j]) = (source[j], source[i]);
        }
    }

    /// <summary>
    /// Rearranges all the elements in the list previously scrambled with <see cref="Scramble{TSource}(IList{TSource},int)"/> back into their original order.
    /// </summary>
    /// <typeparam name="TSource"><see cref="Type"/> of <see cref="IList{T}"/>.</typeparam>
    /// <param name="source">The input list of generic types to unscramble.</param>
    /// <param name="seed">The same number used in <see cref="Scramble{TSource}(IList{TSource},int)"/> call to scramble original list.</param>
    /// <remarks>This function uses the <see cref="System.Random"/> generator to perform unscramble using a sequence that is repeatable.</remarks>
    public static void Unscramble<TSource>(this IList<TSource> source, int seed)
    {
        if (source.IsReadOnly && source is not TSource[])
            throw new ArgumentException("Cannot modify items in a read only list");

        System.Random random = new(seed);
        List<int> sequence = [];
        int count = source.Count;

        // Generate original scramble sequence.
        for (int i = 0; i < count; i++)
            sequence.Add(random.Next(count)); // Calls random function from System namespace.

        // Unmix the data order (traverse same sequence in reverse order).
        for (int i = count - 1; i >= 0; i--)
        {
            int j = sequence[i];

            if (i == j)
                continue;

            // Swaps items
            (source[i], source[j]) = (source[j], source[i]);
        }
    }

    /// <summary>Compares two arrays.</summary>
    /// <param name="array1">The first type array to compare to.</param>
    /// <param name="array2">The second type array to compare against.</param>
    /// <param name="orderIsImportant"><c>true</c> if order of elements should be considered for equality; otherwise, <c>false</c>.</param>
    /// <returns>An <see cref="int"/> which returns 0 if they are equal, 1 if <paramref name="array1"/> is larger, or -1 if <paramref name="array2"/> is larger.</returns>
    /// <typeparam name="TSource"><see cref="Type"/> of the array.</typeparam>
    /// <exception cref="ArgumentException">Cannot compare multidimensional arrays.</exception>
    public static int CompareTo<TSource>(this TSource[] array1, TSource[] array2, bool orderIsImportant = true)
    {
        return CompareTo(array1, array2, Comparer<TSource>.Default, orderIsImportant);
    }

    /// <summary>Compares two arrays.</summary>
    /// <param name="array1">The first <see cref="Array"/> to compare to.</param>
    /// <param name="array2">The second <see cref="Array"/> to compare against.</param>
    /// <param name="comparer">An interface <see cref="IComparer"/> that exposes a method to compare the two arrays.</param>
    /// <param name="orderIsImportant"><c>true</c> if order of elements should be considered for equality; otherwise, <c>false</c>.</param>
    /// <returns>An <see cref="int"/> which returns 0 if they are equal, 1 if <paramref name="array1"/> is larger, or -1 if <paramref name="array2"/> is larger.</returns>
    /// <remarks>This is a default comparer to make arrays comparable.</remarks>
    /// <exception cref="ArgumentException">Cannot compare multidimensional arrays.</exception>
    private static int CompareTo(this Array? array1, Array? array2, IComparer comparer, bool orderIsImportant = true)
    {
        if (comparer is null)
            throw new ArgumentNullException(nameof(comparer));

        if (array1 is null && array2 is null)
            return 0;

        if (array1 is null)
            return -1;

        if (array2 is null)
            return 1;

        if (array1.Rank != 1 || array2.Rank != 1)
            throw new ArgumentException("Cannot compare multidimensional arrays");

        // For arrays that do not have the same number of elements, the array with most elements
        // is assumed to be larger.
        if (array1.Length != array2.Length)
            return array1.Length.CompareTo(array2.Length);

        if (!orderIsImportant)
        {
            array1 = array1.Cast<object>().ToArray();
            array2 = array2.Cast<object>().ToArray();

            Array.Sort(array1, comparer);
            Array.Sort(array2, comparer);
        }

        int comparison = 0;

        for (int x = 0; x < array1.Length; x++)
        {
            comparison = comparer.Compare(array1.GetValue(x), array2.GetValue(x));

            if (comparison != 0)
                break;
        }

        return comparison;
    }

    private class DistinctByWrapper<TKey, TValue> : IEquatable<DistinctByWrapper<TKey, TValue>>
    {
        #region [ Constructors ]

        public DistinctByWrapper(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        #endregion

        #region [ Properties ]

        private TKey Key { get; }

        public TValue Value { get;}

        #endregion

        #region [ Methods ]

        // ReSharper disable once PossibleNullReferenceException
        public bool Equals(DistinctByWrapper<TKey, TValue>? other)
        {
            return Equals(Key, other!.Key);
        }

        public override bool Equals(object? obj)
        {
            return obj is DistinctByWrapper<TKey, TValue> wrapper && Equals(wrapper);
        }

        public override int GetHashCode()
        {
            return Key?.GetHashCode() ?? 0;
        }

        #endregion
    }
}
