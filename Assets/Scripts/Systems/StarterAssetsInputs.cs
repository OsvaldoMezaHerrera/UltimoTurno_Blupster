using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        // ── Callbacks de PlayerInput (se usan si PlayerInput está presente) ──
#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)  => MoveInput(value.Get<Vector2>());
        public void OnLook(InputValue value)  { if (cursorInputForLook) LookInput(value.Get<Vector2>()); }
        public void OnJump(InputValue value)  => JumpInput(value.isPressed);
        public void OnSprint(InputValue value) => SprintInput(value.isPressed);
#endif

        public void MoveInput(Vector2 newMoveDirection)  => move   = newMoveDirection;
        public void LookInput(Vector2 newLookDirection)  => look   = newLookDirection;
        public void JumpInput(bool newJumpState)         => jump   = newJumpState;
        public void SprintInput(bool newSprintState)     => sprint = newSprintState;

        private void Start()
        {
            SetCursorState(cursorLocked);
        }

        private void Update()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current == null) return;

            // ── Lectura directa de teclado y mouse ───────────────────────
            // Movimiento WASD
            Vector2 moveInput = Vector2.zero;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)    moveInput.y += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)  moveInput.y -= 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)  moveInput.x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput.x += 1f;
            move = moveInput;

            // Sprint (Shift izquierdo)
            sprint = Keyboard.current.leftShiftKey.isPressed;

            // Salto (Space) — se activa y lo apaga el ThirdPersonController
            if (Keyboard.current.spaceKey.wasPressedThisFrame) jump = true;

            // Cámara (mouse delta, solo si cursor está bloqueado)
            if (cursorInputForLook && Mouse.current != null && cursorLocked)
                look = Mouse.current.delta.ReadValue();
            else
                look = Vector2.zero;

            // ESC: toggle cursor libre / bloqueado
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                cursorLocked = !cursorLocked;
                SetCursorState(cursorLocked);
            }
#endif
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
