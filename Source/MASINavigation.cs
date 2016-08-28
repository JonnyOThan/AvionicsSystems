﻿/*****************************************************************************
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016 MOARdV
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to
 * deal in the Software without restriction, including without limitation the
 * rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
 * sell copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 * 
 ****************************************************************************/
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AvionicsSystems
{
    /// <summary>
    /// The MASINavigation module encapsulates navigation-related functionality.
    /// </summary>
    /// <LuaName>nav</LuaName>
    /// <mdDoc>MASINavigation encapsulates navigation functionality, including
    /// emulating radio navigation.</mdDoc>
    internal class MASINavigation
    {
        private Vessel vessel;

        [MoonSharpHidden]
        public MASINavigation(Vessel vessel)
        {
            this.vessel = vessel;
        }

        ~MASINavigation()
        {
            vessel = null;
        }

        [MoonSharpHidden]
        internal void Update()
        {

        }

        [MoonSharpHidden]
        internal void UpdateVessel(Vessel vessel)
        {
            this.vessel = vessel;
        }
    }

    /*
    public struct NavObject
    {
        double latitude;
        double longitude;
        double altitude;
        float frequency;
        string name;
        string identifier;
        int todo_navType;
    }
     */
}