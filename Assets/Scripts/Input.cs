// GENERATED AUTOMATICALLY FROM 'Assets/Scripts/Input.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Input : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Input()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Input"",
    ""maps"": [
        {
            ""name"": ""Battle"",
            ""id"": ""d5e7abc5-76a2-4a25-acc5-ac19a33be446"",
            ""actions"": [
                {
                    ""name"": ""MouseClick"",
                    ""type"": ""Button"",
                    ""id"": ""b7d12558-71d1-4fac-8a63-9820c76d7178"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Continue"",
                    ""type"": ""Button"",
                    ""id"": ""ff04ca85-d8e1-4620-8f7c-3a347d3a1165"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MouseMove"",
                    ""type"": ""Value"",
                    ""id"": ""5b8efd28-81dd-4ca3-903d-00fa94aa0a70"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""777003ec-dd6e-4836-b278-109d3afa3961"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f436c1c5-840b-4183-8752-7e1cb00b29ac"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9eb12709-9ba3-4075-8c65-d3d61791bf2a"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a479a1fe-b969-460d-a574-2a0d7c62af5c"",
                    ""path"": ""<Mouse>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3243ab7-befe-4519-ad0d-9e0c15fb6037"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MouseMove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Battle
        m_Battle = asset.FindActionMap("Battle", throwIfNotFound: true);
        m_Battle_MouseClick = m_Battle.FindAction("MouseClick", throwIfNotFound: true);
        m_Battle_Continue = m_Battle.FindAction("Continue", throwIfNotFound: true);
        m_Battle_MouseMove = m_Battle.FindAction("MouseMove", throwIfNotFound: true);
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

    // Battle
    private readonly InputActionMap m_Battle;
    private IBattleActions m_BattleActionsCallbackInterface;
    private readonly InputAction m_Battle_MouseClick;
    private readonly InputAction m_Battle_Continue;
    private readonly InputAction m_Battle_MouseMove;
    public struct BattleActions
    {
        private @Input m_Wrapper;
        public BattleActions(@Input wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouseClick => m_Wrapper.m_Battle_MouseClick;
        public InputAction @Continue => m_Wrapper.m_Battle_Continue;
        public InputAction @MouseMove => m_Wrapper.m_Battle_MouseMove;
        public InputActionMap Get() { return m_Wrapper.m_Battle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(BattleActions set) { return set.Get(); }
        public void SetCallbacks(IBattleActions instance)
        {
            if (m_Wrapper.m_BattleActionsCallbackInterface != null)
            {
                @MouseClick.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnMouseClick;
                @MouseClick.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnMouseClick;
                @MouseClick.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnMouseClick;
                @Continue.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnContinue;
                @Continue.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnContinue;
                @Continue.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnContinue;
                @MouseMove.started -= m_Wrapper.m_BattleActionsCallbackInterface.OnMouseMove;
                @MouseMove.performed -= m_Wrapper.m_BattleActionsCallbackInterface.OnMouseMove;
                @MouseMove.canceled -= m_Wrapper.m_BattleActionsCallbackInterface.OnMouseMove;
            }
            m_Wrapper.m_BattleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MouseClick.started += instance.OnMouseClick;
                @MouseClick.performed += instance.OnMouseClick;
                @MouseClick.canceled += instance.OnMouseClick;
                @Continue.started += instance.OnContinue;
                @Continue.performed += instance.OnContinue;
                @Continue.canceled += instance.OnContinue;
                @MouseMove.started += instance.OnMouseMove;
                @MouseMove.performed += instance.OnMouseMove;
                @MouseMove.canceled += instance.OnMouseMove;
            }
        }
    }
    public BattleActions @Battle => new BattleActions(this);
    public interface IBattleActions
    {
        void OnMouseClick(InputAction.CallbackContext context);
        void OnContinue(InputAction.CallbackContext context);
        void OnMouseMove(InputAction.CallbackContext context);
    }
}
