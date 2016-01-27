﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 * Copyright by The HDF Group.                                               *
 * Copyright by the Board of Trustees of the University of Illinois.         *
 * All rights reserved.                                                      *
 *                                                                           *
 * This file is part of HDF5.  The full HDF5 copyright notice, including     *
 * terms governing use, modification, and redistribution, is contained in    *
 * the files COPYING and Copyright.html.  COPYING can be found at the root   *
 * of the source code distribution tree; Copyright.html can be found at the  *
 * root level of an installed copy of the electronic HDF5 document set and   *
 * is linked from the top-level documents page.  It can also be found at     *
 * http://hdfgroup.org/HDF5/doc/Copyright.html.  If you do not have          *
 * access to either file, you may request a copy from help@hdfgroup.org.     *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HDF.PInvoke;

using hid_t = System.Int32;
using size_t = System.IntPtr;
using ssize_t = System.IntPtr;

namespace UnitTests
{
    public partial class H5ATest
    {
        [TestMethod]
        public void H5Aget_nameTest1()
        {
            size_t buf_size = IntPtr.Zero, size = IntPtr.Zero,
                buf = IntPtr.Zero;
            hid_t att = H5A.create(m_v2_test_file, "H5Aget_name",
                H5T.IEEE_F64LE, m_space_scalar);
            Assert.IsTrue(att >= 0);

            // pretend we don't know the size
            size = H5A.get_name(att, buf_size, buf);
            Assert.IsTrue(size.ToInt32() == 11);
            buf_size = new IntPtr(size.ToInt32() + 1);
            buf = Marshal.AllocHGlobal(buf_size.ToInt32());
            size = H5A.get_name(att, buf_size, buf);
            Assert.IsTrue(size.ToInt32() == 11);
            string name = Marshal.PtrToStringAnsi(buf);
            // names should match
            Assert.AreEqual("H5Aget_name", name);

            // read a truncated version
            buf_size = new IntPtr(3);
            size = H5A.get_name(att, buf_size, buf);
            Assert.IsTrue(size.ToInt32() == 11);
            name = Marshal.PtrToStringAnsi(buf);
            // names won't match
            Assert.AreNotEqual("H5Aget_name", name);
            Assert.AreEqual("H5", name);

            Marshal.FreeHGlobal(buf);
            Assert.IsTrue(H5A.close(att) >= 0);
        }

        [TestMethod]
        public void H5Aget_nameTest2()
        {
            Assert.IsFalse(H5A.get_name(Utilities.RandomInvalidHandle(),
                IntPtr.Zero, IntPtr.Zero).ToInt32() >= 0);
        }
    }
}