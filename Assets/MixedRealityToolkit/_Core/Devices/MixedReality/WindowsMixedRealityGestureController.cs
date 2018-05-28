﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.MixedReality.Toolkit.Internal.Definitions;
using Microsoft.MixedReality.Toolkit.Internal.Definitions.Devices;
using Microsoft.MixedReality.Toolkit.Internal.Definitions.InputSystem;
using Microsoft.MixedReality.Toolkit.Internal.Interfaces.InputSystem;
using System;
using System.Collections.Generic;
using UnityEngine.XR.WSA.Input;

namespace Microsoft.MixedReality.Toolkit.Internal.Devices.WindowsMixedReality
{
    // TODO - Only scaffold at present
    public struct WindowsMixedRealityGestureController : IMixedRealityController
    {
        #region Private properties

        #endregion Private properties

        public WindowsMixedRealityGestureController(uint sourceId, Handedness handedness)
        {
            ControllerState = ControllerState.None;
            ControllerHandedness = handedness;

            InputSource = null;

            Interactions = new Dictionary<DeviceInputType, InteractionDefinition>();
        }

        #region IMixedRealityController Interface Members

        public ControllerState ControllerState { get; private set; }

        public Handedness ControllerHandedness { get; }

        public IMixedRealityInputSource InputSource { get; private set; }

        public Dictionary<DeviceInputType, InteractionDefinition> Interactions { get; private set; }

        public void SetupInputSource<T>(IMixedRealityInputSystem inputSystem, T state)
        {
            if (inputSystem != null)
            {
                InputSource = inputSystem.RequestNewGenericInputSource($"XBox Controller {ControllerHandedness}");
            }

            InteractionSourceState interactionSourceState = CheckIfValidInteractionSourceState(state);
            SetupFromInteractionSource(interactionSourceState);
        }

        public void UpdateInputSource<T>(IMixedRealityInputSystem inputSystem, T state)
        {
            InteractionSourceState interactionSourceState = CheckIfValidInteractionSourceState(state);
            UpdateFromInteractionSource(interactionSourceState);
        }

        #endregion IMixedRealityInputSource Interface Members

        #region Setup and Update functions

        public void SetupFromInteractionSource(InteractionSourceState interactionSourceState)
        {
            //Update the Tracked state of the controller
            UpdateControllerData(interactionSourceState);

            MixedRealityControllerMappingProfile controllerMapping = Managers.MixedRealityManager.Instance.ActiveProfile.GetControllerMapping(typeof(WindowsMixedRealityController),ControllerHandedness);
            //MixedRealityControllerMappingProfile controllerMapping = Managers.MixedRealityManager.Instance.ActiveProfile.GetControllerMapping<WindowsMixedRealityController>(ControllerHandedness);
            if (controllerMapping?.Interactions?.Length > 0)
            {
                SetupFromMapping(controllerMapping.Interactions);
            }
            else
            {
                SetupWMRControllerDefaults(interactionSourceState);
            }
        }

        private void SetupFromMapping(InteractionDefinitionMapping[] mappings)
        {
            for (uint i = 0; i < mappings.Length; i++)
            {
                // Add interaction for Mapping
                Interactions.Add(mappings[i].InputType, new InteractionDefinition(i, mappings[i].AxisType, mappings[i].InputType, mappings[i].InputAction));
            }
        }

        private void SetupWMRControllerDefaults(InteractionSourceState interactionSourceState)
        {
            InputAction[] inputActions = Managers.MixedRealityManager.Instance.ActiveProfile.InputActionsProfile.InputActions;
            if (inputActions == null)
            {
                return;
            }
            
            // TODO - add setup for Gesture
        }

        public void UpdateFromInteractionSource(InteractionSourceState interactionSourceState)
        {
            UpdateControllerData(interactionSourceState);

            // TODO - add defaults for Gesture
        }

        #region Update data functions

        private void UpdateControllerData(InteractionSourceState interactionSourceState)
        {
            // TODO - add update for Gesture
        }

        #endregion Update data functions

        #endregion Setup and Update functions

        #region Utilities
        private static InteractionSourceState CheckIfValidInteractionSourceState<T>(T state)
        {
            InteractionSourceState interactionSourceState = (InteractionSourceState)Convert.ChangeType(state, typeof(InteractionSourceState));
            if (interactionSourceState.source.id == 0) { throw new ArgumentOutOfRangeException(nameof(state), "Incorrect state type provided to controller,did you send an InteractionSourceState?"); }

            return interactionSourceState;
        }
        #endregion Utilities
    }
}