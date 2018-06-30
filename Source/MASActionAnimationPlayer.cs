﻿/*****************************************************************************
 * The MIT License (MIT)
 * 
 * Copyright (c) 2016-2018 MOARdV
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
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AvionicsSystems
{
    internal class MASActionAnimationPlayer : IMASSubComponent
    {
        private float animationSpeed = 1.0f;
        private string animationName = string.Empty;
        private Animation animation;
        private AnimationState animationState;
        private bool playedOnce = false;
        private bool currentState = false;

        // TODO: Support 'reverse'?
        internal MASActionAnimationPlayer(ConfigNode config, InternalProp prop, MASFlightComputer comp)
            : base(config, prop, comp)
        {
            bool exterior = false;
            if (!config.TryGetValue("animation", ref animationName))
            {
                if (!config.TryGetValue("externalAnimation", ref animationName) || string.IsNullOrEmpty(animationName))
                {
                    throw new ArgumentException("Invalid or missing 'externalAnimation' or 'animation' in ANIMATION_PLAYER " + name);
                }

                exterior = true;
            }

            // Set up the animation.
            Animation[] animators = (exterior) ? prop.part.FindModelAnimators(animationName) : prop.FindModelAnimators(animationName);
            if (animators.Length == 0)
            {
                throw new ArgumentException("Unable to find" + ((exterior) ? " external " : " ") + "animation " + animationName + " for ANIMATION_PLAYER " + name);
            }
            animation = animators[0];
            animationState = animation[animationName];
            animationState.wrapMode = WrapMode.Once;

            config.TryGetValue("animationSpeed", ref animationSpeed);

            string variableName = string.Empty;
            if (!config.TryGetValue("variable", ref variableName) || string.IsNullOrEmpty(variableName))
            {
                throw new ArgumentException("Invalid or missing 'variable' in ANIMATION_PLAYER " + name);
            }
            variableName = variableName.Trim();

            variableRegistrar.RegisterVariableChangeCallback(variableName, VariableCallback);
        }

        /// <summary>
        /// Variable callback used to update the animation when it is playing.
        /// </summary>
        /// <param name="newValue"></param>
        private void VariableCallback(double newValue)
        {
            if (playedOnce)
            {
                bool newState = (newValue > 0.0);

                if (newState != currentState)
                {
                    currentState = newState;
                    if (newState)
                    {
                        animationState.normalizedTime = 0.0f;
                        animationState.speed = 1.0f * animationSpeed;
                    }
                    else
                    {
                        animationState.normalizedTime = 1.0f;
                        animationState.speed = -1.0f * animationSpeed;
                    }
                    animation.Play(animationName);
                }
            }
            else
            {
                if (newValue > 0.0)
                {
                    animationState.speed = float.MaxValue;
                    animationState.normalizedTime = 0.0f;
                    currentState = true;
                }
                else
                {
                    animationState.speed = float.MinValue;
                    animationState.normalizedTime = 1.0f;
                    currentState = false;
                }

                animation.Play(animationName);
                playedOnce = true;
            }
        }

        /// <summary>
        /// Release resources
        /// </summary>
        public override void ReleaseResources(MASFlightComputer comp, InternalProp prop)
        {
            animationState = null;
            animation = null;
        }
    }
}
