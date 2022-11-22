//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Input/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Grid"",
            ""id"": ""3a4e22be-6d27-4cf0-bdc9-cc87ac6315df"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""5a681415-1a1f-4801-a423-6d95c0cd996c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""7f8edb07-ae5d-4749-aa30-78b4b6973085"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CursorMove"",
                    ""type"": ""Value"",
                    ""id"": ""c69ae62f-ad12-4fe6-96b0-2afd7ed1e40e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""0b32bfa4-85c1-4018-bc88-d40be53cf81f"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""17fcdc67-6ddc-47be-a709-9152da17b3ab"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""caa2850a-6fc3-438e-96a6-3935c3bacf5d"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""3c96186c-1475-4a4c-8499-ba369fc560b2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""dc4eb746-9c63-4f9e-a74a-fef38287215b"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0a49f88e-0167-4840-b9ab-f9dfb75f3023"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d7fae21-0344-433a-afc6-82c05ff31492"",
                    ""path"": ""<Pointer>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CursorMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Grid
        m_Grid = asset.FindActionMap("Grid", throwIfNotFound: true);
        m_Grid_Movement = m_Grid.FindAction("Movement", throwIfNotFound: true);
        m_Grid_Select = m_Grid.FindAction("Select", throwIfNotFound: true);
        m_Grid_CursorMove = m_Grid.FindAction("CursorMove", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Grid
    private readonly InputActionMap m_Grid;
    private IGridActions m_GridActionsCallbackInterface;
    private readonly InputAction m_Grid_Movement;
    private readonly InputAction m_Grid_Select;
    private readonly InputAction m_Grid_CursorMove;
    public struct GridActions
    {
        private @PlayerControls m_Wrapper;
        public GridActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Grid_Movement;
        public InputAction @Select => m_Wrapper.m_Grid_Select;
        public InputAction @CursorMove => m_Wrapper.m_Grid_CursorMove;
        public InputActionMap Get() { return m_Wrapper.m_Grid; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GridActions set) { return set.Get(); }
        public void SetCallbacks(IGridActions instance)
        {
            if (m_Wrapper.m_GridActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_GridActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_GridActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_GridActionsCallbackInterface.OnMovement;
                @Select.started -= m_Wrapper.m_GridActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_GridActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_GridActionsCallbackInterface.OnSelect;
                @CursorMove.started -= m_Wrapper.m_GridActionsCallbackInterface.OnCursorMove;
                @CursorMove.performed -= m_Wrapper.m_GridActionsCallbackInterface.OnCursorMove;
                @CursorMove.canceled -= m_Wrapper.m_GridActionsCallbackInterface.OnCursorMove;
            }
            m_Wrapper.m_GridActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
                @CursorMove.started += instance.OnCursorMove;
                @CursorMove.performed += instance.OnCursorMove;
                @CursorMove.canceled += instance.OnCursorMove;
            }
        }
    }
    public GridActions @Grid => new GridActions(this);
    public interface IGridActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
        void OnCursorMove(InputAction.CallbackContext context);
    }
}
