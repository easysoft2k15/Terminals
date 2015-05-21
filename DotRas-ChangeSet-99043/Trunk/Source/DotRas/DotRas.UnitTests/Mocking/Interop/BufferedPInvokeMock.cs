﻿//--------------------------------------------------------------------------
// <copyright file="BufferedPInvokeMock.cs" company="Jeff Winn">
//      Copyright (c) Jeff Winn. All rights reserved.
//
//      The use and distribution terms for this software is covered by the
//      GNU Library General Public License (LGPL) v2.1 which can be found
//      in the License.rtf at the root of this distribution.
//      By using this software in any fashion, you are agreeing to be bound by
//      the terms of this license.
//
//      You must not remove this notice, or any other, from this software.
// </copyright>
//--------------------------------------------------------------------------

namespace DotRas.UnitTests.Mocking.Interop
{
    using System;
    using System.Runtime.InteropServices;
    using DotRas.Internal;

    /// <summary>
    /// Provides the implementation for buffered p/invoke mock objects.
    /// </summary>
    /// <typeparam name="TInput">The type of object used as input for the call.</typeparam>
    /// <typeparam name="TResult">The type of objects being stored in memory.</typeparam>
    internal class BufferedPInvokeMock<TInput, TResult> : PInvokeMock
        where TInput : BufferedPInvokeParams
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BufferedPInvokeMock&lt;TInput, TResult&gt;"/> class.
        /// </summary>
        public BufferedPInvokeMock()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the results of the call.
        /// </summary>
        public TResult[] Result
        {
            get;
            set;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes the mock call.
        /// </summary>
        /// <param name="value">An <typeparamref name="TInput"/> object containing call information.</param>
        public virtual void Execute(TInput value)
        {
            int size = Marshal.SizeOf(typeof(TResult));
            long expectedSize = size * this.Result.Length;

            if (value.BufferSize.ToInt64() != expectedSize && expectedSize > 0)
            {
                value.BufferSize = new IntPtr(expectedSize);
            }
            else if (this.Result.Length > 0)
            {
                Utilities.CopyObjectsToPtr<TResult>(this.Result, value.Address, ref size);
                value.BufferSize = new IntPtr(expectedSize);
            }
            else
            {
                value.BufferSize = IntPtr.Zero;
            }
        }

        #endregion
    }
}